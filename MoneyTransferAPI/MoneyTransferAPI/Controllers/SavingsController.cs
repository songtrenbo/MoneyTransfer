using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransferAPI.Interfaces;
using MoneyTransferAPI.Models;
using MoneyTransferAPI.Services;
using System.Data.Common;
using System.Transactions;

namespace MoneyTransferAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISavingService _savingService;
        public SavingsController(ISavingService savingService, IUserService userService)
        {
            _savingService = savingService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Saving>>> GetSavingsByUserId(string userId)
        {
            try
            {
                return Ok(await _savingService.GetSavingsByUserId(userId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Saving>> CreateSaving([FromBody] Saving saving, int months)
        {
            try
            {
                if (saving.deposits < 1000000)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Bad Request: Need at least 1.000.000VND to do a saving");
                }

                var charge = await _userService.UserChargeMoney(saving.userId, saving.deposits);
                if (charge == false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Bad Request: Insufficient funds");
                }

                return Ok(await _savingService.CreateSaving(saving, months));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Saving>> ChangeSavingStatus(string savingId)
        {
            try
            {
                var result = await _savingService.ChangeSavingStatus(savingId);
                if(result != null)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status400BadRequest, "Bad Request: Can't change status");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
