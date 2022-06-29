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
    public class LogIn
    {

        private readonly ISessionService _sessionService;
        public LogIn(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [FunctionName("LogIn")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "LogIn/{membershipCard}/{pin}")] HttpRequest req,
            string membershipCard,string pin, ILogger log)
        {

            var service = new SessionService();

            try
            {
                var token = service.LoggingIn(membershipCard, pin);
                return new OkObjectResult(token);

            }
            catch (CardNotExistException)
            {
                return new BadRequestObjectResult("Membership card does not exist");
            }
            catch (CardNotRegisteredException)
            {
                return new BadRequestObjectResult("Card not registered please register your card");
            }
            catch (InvalidCredentialException)
            {
                return new UnauthorizedResult();
            }
            catch (InvalidCardTypeException)
            {
                return new BadRequestObjectResult("Invalid Card");
            }
            catch (InvalidPinTypeException)
            {
                return new UnauthorizedResult();
            }
            catch (AlreadyLoggedInException)
            {
                return new BadRequestObjectResult("Already Logged in");
            }
            catch (Exception e)
            {
                log.LogInformation(e.ToString());
                return new InternalServerErrorResult();
            }
        }
    }
}