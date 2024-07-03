using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransferAPI.Interfaces;
using MoneyTransferAPI.Models;
using MoneyTransferAPI.Services;

namespace MoneyTransferAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService, IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByUserId(string userSendId)
        {
            try
            {
                return Ok(await _transactionService.GetTransactionsByUserId(userSendId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateTransactions([FromBody] Transaction transaction, string creditCardId)
        {
            try
            {
                if (transaction.money < 10000)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Bad Request: Need at least 10.000VND to do a transaction");
                }
                var charge = await _userService.UserChargeMoney(transaction.userSendId, transaction.money);

                if (charge == false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Bad Request: Insufficient funds");
                }

                var user = await _userService.GetUserByCreditId(creditCardId);
                var received = await _userService.UserMoneyReceived(user.UserId, transaction.money);
                if (received == false)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User not found");
                }
                transaction.userReceivedId = user.UserId;
                return Ok(await _transactionService.CreateTransaction(transaction));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


    }
}
