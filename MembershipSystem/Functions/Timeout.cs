using MembershipSystem.Domain;
using MembershipSystem.Domain.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace MembershipSystem.Functions
{
    public class Timeout
    {

        private readonly ISessionService _sessionService;
        public Timeout(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [FunctionName("Timeoutcs")]
        public void Run([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var service = new SessionService();
            service.TimeOutAllOldSessions();
        }
    }
}
