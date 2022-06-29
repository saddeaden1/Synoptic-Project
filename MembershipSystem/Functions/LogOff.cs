using System;
using System.Security.Authentication;
using System.Threading.Tasks;
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
    public class LogOff
    {
        private readonly ISessionService _sessionService;
        public LogOff(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [FunctionName("LogOff")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "LogOff/{sessionToken}/{pin}")] HttpRequest req,
            string sessionToken,string pin, ILogger log)
        {

            var service = new SessionService();

            try
            {
                service.LoggingOff(sessionToken, pin);
                return new OkObjectResult("Goodbye");
            }
            catch (InvalidCredentialException)
            {
                return new UnauthorizedResult();
            }
            catch (InvalidPinTypeException)
            {
                return new BadRequestObjectResult("Invalid pin type");
            }
            catch (Exception e)
            {
                log.LogInformation(e.ToString());
                return new BadRequestObjectResult(e.ToString());
            }
        }
    }
}

