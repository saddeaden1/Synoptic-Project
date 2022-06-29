using MembershipSystem;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using MembershipSystem.Domain;
using MembershipSystem.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace MembershipSystem
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IEmployeeService,EmployeeService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<ISessionService, SessionService>();
        }
    }
}
