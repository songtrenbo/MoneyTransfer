using Microsoft.AspNetCore.Mvc;
using MoneyTransferAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MoneyTransferAPI.Interfaces
{
    public interface IUserService
    {
        Task<ResLoginModel> Login([FromForm] LoginModel model);
        Task<ResLoginModel> GetUserByCreditId(string creditCardId);
        Task<Boolean> UserChargeMoney(string userId, int money);
        Task<Boolean> UserMoneyReceived(string userId, int money);
        JwtSecurityToken CreateToken(List<Claim> authClaims);
    }
}
