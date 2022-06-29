using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web.Http;
using MembershipSystem.Domain;
using MembershipSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MembershipSystem.Functions
{
    public class CheckBalance
    {

        private readonly ITransactionService _transactionService;

        private readonly ISessionService _sessionService;
        public CheckBalance(ITransactionService transactionService, ISessionService sessionService)
        {
            _sessionService = sessionService;
            _transactionService = transactionService;
        }

        [FunctionName("CheckBalance")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "CheckBalance/{sessionToken}/{pin}")] HttpRequest req,
            string sessionToken,string pin, ILogger log)
        {

            var transactionService = new TransactionService();
            var sessionService = new SessionService();

            try
            {
                if (sessionService.ValidateUser(sessionToken, pin))
                {
                    var balance = transactionService.CheckBalance(sessionToken);
                    sessionService.UpdateLastRequestTime(sessionToken);
                    return new OkObjectResult(balance);
                }
            }
            catch(InvalidCredentialException)
            {
                return new BadRequestObjectResult("Incorrect Pin");
            }
            catch(TimeoutException)
            {
                return new BadRequestObjectResult("User Timeout please login again");
            }
            catch(Exception e)
            {
                log.LogInformation(e.ToString());
                return new InternalServerErrorResult();
            }

            return new OkResult();
        }
    }
}

