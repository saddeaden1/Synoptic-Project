using MembershipSystem.Repository;
using TechTalk.SpecFlow;

namespace MembershipSystem.AcceptanceTests
{
    [Binding]
    public sealed class Hooks
    {

        private readonly BeforeTestSetupClass _beforeTestSetupClass;

        public Hooks(BeforeTestSetupClass beforeTestSetupClass)
        {
            _beforeTestSetupClass = beforeTestSetupClass;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var db = new SharedContext();

            foreach (var employee in db.Employees)
            {
                db.Employees.Remove(employee);
            }

            foreach (var activeSession in db.ActiveSessions)
            {
                db.ActiveSessions.Remove(activeSession);
            }

            foreach (var thing in db.MembershipCards)
            {
                db.MembershipCards.Remove(thing);
            }

            foreach (var thing in db.SessionHistorys)
            {
                db.SessionHistorys.Remove(thing);
            }

            db.SaveChanges();
        }
    }
}
