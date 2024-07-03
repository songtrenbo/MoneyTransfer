using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransferAPI.Interfaces;
using MoneyTransferAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MoneyTransferAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("userCreditId")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserByCreditId(string creditCardId)
        {
            try
            {
                return Ok(await _userService.GetUserByCreditId(creditCardId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data from the database");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login( LoginModel model)
        {
            var res = await _userService.Login(model);
            if (model != null)
            {
                if (res == null)
                {
                    return BadRequest("Wrong username or password, please try again!");
                }
            }

            var authClaims = new List<Claim>
            {
                new Claim("userId",res.UserId),
                new Claim("name", res.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString()),
            };

            var token = _userService.CreateToken(authClaims);

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            });
        }

    }
}
