using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoneyTransferAPI.Data;
using MoneyTransferAPI.Interfaces;
using MoneyTransferAPI.Models;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyTransferAPI.Services
{
    public class UserService: IUserService
    {
        private readonly MongoDBContext _context;
        private readonly IConfiguration _configuration;
        public UserService(MongoDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResLoginModel> Login([FromForm] LoginModel model)
        {
            var hasher = new PasswordHasher<User>();
            ResLoginModel res = new ResLoginModel();

            var user = await _context.Users.Find(c => c.username == model.UserName).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            var  c = hasher.HashPassword(user, model.Password);
            var result = hasher.VerifyHashedPassword(user, user.password, model.Password);
            if (result != PasswordVerificationResult.Success)
            {
                return null;
            }
            res.UserId = user.Id;
            res.Name = user.name;
            return res;
        }

        public async Task<ResLoginModel> GetUserByCreditId(string creditCardId)
        {
            var user = await _context.Users.Find(c=>c.creditCardId == creditCardId).FirstOrDefaultAsync();
            ResLoginModel res = new ResLoginModel
            {
                UserId = user.Id,
                Name = user.name
            };
            return res;
        }


        public async Task<Boolean> UserChargeMoney(string userId, int money)
        {
            var user = await _context.Users.Find(c=>c.Id == userId).FirstOrDefaultAsync();
            if (user == null || user.money < money)
            {
                return false;
            }
            user.money = user.money - money;
            var result = await _context.Users.ReplaceOneAsync(c => c.Id==user.Id, user);
            if (result != null)
            {
                return true;
            }
            return false;
        }
        public async Task<Boolean> UserMoneyReceived(string userId, int money)
        {
            var user = await _context.Users.Find(c => c.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            user.money += money;
            var result = await _context.Users.ReplaceOneAsync(c => c.Id == user.Id, user);
            if (result != null)
            {
                return true;
            }
            return false;
        }
        public JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SymmetricKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256));

            return token;

        }

    }
}
