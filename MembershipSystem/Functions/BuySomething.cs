using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web.Http;
using MembershipSystem.Domain;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MembershipSystem.Functions
{
    public class BuySomething
    {
        private readonly ITransactionService _transactionService;

        private readonly ISessionService _sessionService;
        public BuySomething(ITransactionService transactionService, ISessionService sessionService)
        {
            _sessionService = sessionService;
            _transactionService = transactionService;
        }

        [FunctionName("BuySomething")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "BuySomething/{sessionToken}/{pin}/{amount}")] HttpRequest req,
            string sessionToken, string pin, string amount, ILogger log)
        {
            var transactionService = new TransactionService();
            var sessionService = new SessionService();

            try
            {
                if (sessionService.ValidateUser(sessionToken, pin))
                {
                    transactionService.BuySomething(sessionToken, amount);
                    sessionService.UpdateLastRequestTime(sessionToken);
                }
            }
            catch (InvalidCredentialException)
            {
                return new UnauthorizedResult();
            }
            catch (TimeoutException)
            {
                return new BadRequestObjectResult("User timeout please login again");
            }
            catch (InsufficientFundsException)
            {
                return new BadRequestObjectResult("You do not have enough money");
            }
            catch (Exception e)
            {
                log.LogInformation(e.ToString());
                return new InternalServerErrorResult();
            }

            return new OkObjectResult("Success, have a nice day");
        }
    }
}