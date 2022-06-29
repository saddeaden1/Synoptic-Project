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
    public class AddMoney
    {

        private readonly ITransactionService _transactionService;

        private readonly ISessionService _sessionService;
        public AddMoney(ITransactionService transactionService, ISessionService sessionService)
        {
            _sessionService = sessionService;
            _transactionService = transactionService;
        }

        [FunctionName("AddMoney")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "AddMoney/{sessionToken}/{pin}/{amount}")] HttpRequest req,
            string sessionToken,string pin,string amount, ILogger log)
        {
            var transactionService = new TransactionService();
            var sessionService = new SessionService();

            try
            {
                if (sessionService.ValidateUser(sessionToken, pin))
                {
                    transactionService.AddMoney(sessionToken, amount);
                    sessionService.UpdateLastRequestTime(sessionToken);
                }
            }
            catch(InvalidCredentialException)
            {
                return new UnauthorizedResult();
            }
            catch(TimeoutException)
            {
                return new BadRequestObjectResult("User timeout please login again");
            }
            catch(FailedTransactionException)
            {
                return new BadRequestObjectResult("Transaction failed please try again");

            }catch (Exception e)
            {
                log.LogInformation(e.ToString());
                return new InternalServerErrorResult();
            }

            return new OkObjectResult("Money has been added have a nice day");
        }
    }
}

