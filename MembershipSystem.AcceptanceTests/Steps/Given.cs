using System;
using System.Linq;
using MembershipSystem.Repository;
using MembershipSystem.Repository.DbModels;
using TechTalk.SpecFlow;

namespace MembershipSystem.AcceptanceTests.Steps
{
    [Binding]
    public class Given
    {
        private readonly BeforeTestSetupClass _beforeTestSetupClass;
        public Given(BeforeTestSetupClass beforeTestSetupClass)
        {
            _beforeTestSetupClass = beforeTestSetupClass;
        }

        [Given(@"the employees card is registered")]
        public void GivenTheEmployeesCardIsRegistered()
        {
            var employeeOb = new EmployeeDbModel()
            {
                FirstName = "Sadde",
                EmailAddress = "adensaddeacceptancetest@gmail.com",
                LastName = "Aden",
                MobileNumber = "07965676545",
                Pin = "4567",
                EmployeeId = "65b0850e-b011-44ec-b138-a1e3b084f444",
                MembershipCardDbModelId = "r7jTGvdqBy5wFO4L",
                
            };

            var membershipCard = new MembershipCardDbModel()
            {
                MembershipCardId = "r7jTGvdqBy5wFO4L",
                Balance = 2.45,
                EmployeeDbModel = employeeOb
            };

            _beforeTestSetupClass.MembershipCards.Add(membershipCard);
            _beforeTestSetupClass.Employees.Add(employeeOb);

            var context = new SharedContext();

            context.MembershipCards.Add(membershipCard);
            context.Employees.Add(employeeOb);
            context.SaveChanges();
        }

        [Given(@"the employees card is not registered")]
        public void GivenTheEmployeesCardIsNotRegistered()
        {
            var membershipCard = new MembershipCardDbModel()
            {
                MembershipCardId = "r7jTGvdqBy5wFO4i",
                Balance = 23.5,
            };

            _beforeTestSetupClass.MembershipCards.Add(membershipCard);

            var context = new SharedContext();

            context.MembershipCards.Add(membershipCard);
            context.SaveChanges();
        }

        [Given(@"the user is logged in")]
        public void GivenTheUserIsLoggedIn()
        {
            var activeSession = new ActiveSessionDbModel()
            {
                EmployeeId = _beforeTestSetupClass.Employees.First().EmployeeId,
                SessionToken = "b5feaba2-c58b-4f83-9040-1b4ab9605ed9",
                ActiveSessionId = Guid.NewGuid(),
                LastRequest = (DateTime.Now-TimeSpan.FromMinutes(2)).ToString("F"),
                LogOn = (DateTime.Now-TimeSpan.FromMinutes(2)).ToString("F"),
                
            };

            _beforeTestSetupClass.ActiveSessions.Add(activeSession);

            var context = new SharedContext();

            context.ActiveSessions.Add(activeSession);
            context.SaveChanges();
        }

        [Given(@"the user is not logged in")]
        public void GivenTheUserIsNotLoggedIn()
        {
            //Does nothing
        }

        [Given(@"they have not done anything on the system for (.*) mins")]
        public void GivenTheyHaveNotDoneAnythingOnTheSystemForMins(int p0)
        {

            var oldSession = _beforeTestSetupClass.ActiveSessions.First();
            var newSession = oldSession;

            newSession.LastRequest =
                (DateTime.Now - TimeSpan.FromMinutes(p0)).ToString("G");

            var context = new SharedContext();

            context.ActiveSessions.Remove(oldSession);
            context.ActiveSessions.Add(newSession);
            context.SaveChanges();
            _beforeTestSetupClass.ActiveSessions.Add(newSession);

        }

        [Given(@"they have £(.*) in their account")]
        public void GivenTheyHaveInTheirAccount(double p0)
        {
            _beforeTestSetupClass.MembershipCards.First().Balance = p0;
        }

        [Given(@"the employees card is registered and the user is logged in and they have not done anything on the system for (.*) mins")]
        public void GivenTheEmployeesCardIsRegisteredAndTheUserIsLoggedInAndTheyHaveNotDoneAnythingOnTheSystemForMins(int p0)
        {
            var context = new SharedContext();

            var membershipCard = new MembershipCardDbModel()
            {
                MembershipCardId = "r7jTGvdqBy5wFO4L",
                Balance = 2.45
            };

            var employeeOb = new EmployeeDbModel()
            {
                FirstName = "Sadde",
                EmailAddress = "adensaddeacceptancetest@gmail.com",
                LastName = "Aden",
                MobileNumber = "07965676545",
                Pin = "4567",
                EmployeeId = "65b0850e-b011-44ec-b138-a1e3b084f444",
                MembershipCardDbModelId = "r7jTGvdqBy5wFO4L"
            };

            _beforeTestSetupClass.MembershipCards.Add(membershipCard);
            _beforeTestSetupClass.Employees.Add(employeeOb);

            
            var activeSession = new ActiveSessionDbModel()
            {
                EmployeeId = "65b0850e-b011-44ec-b138-a1e3b084f444",
                SessionToken = "b5feaba2-c58b-4f83-9040-1b4ab9605ed9",
                ActiveSessionId = Guid.NewGuid(),
                LastRequest = (DateTime.Now - TimeSpan.FromMinutes(p0)).ToString("F"),
                LogOn = (DateTime.Now - TimeSpan.FromMinutes(2)).ToString("F"),

            };

            _beforeTestSetupClass.ActiveSessions.Add(activeSession);

            context.MembershipCards.Add(membershipCard);
            context.Employees.Add(employeeOb);
            context.ActiveSessions.Add(activeSession);
            context.SaveChanges();

        }

        [Given(@"the employees card is registered And they have £(.*) in their account")]
        public void GivenTheEmployeesCardIsRegisteredAndTheyHaveInTheirAccount(int p0)
        {
            var employeeOb = new EmployeeDbModel()
            {
                FirstName = "Sadde",
                EmailAddress = "adensaddeacceptancetest@gmail.com",
                LastName = "Aden",
                MobileNumber = "07965676545",
                Pin = "4567",
                EmployeeId = "65b0850e-b011-44ec-b138-a1e3b084f444",
                MembershipCardDbModelId = "r7jTGvdqBy5wFO4L",

            };

            var membershipCard = new MembershipCardDbModel()
            {
                MembershipCardId = "r7jTGvdqBy5wFO4L",
                Balance = p0,
                EmployeeDbModel = employeeOb
            };

            _beforeTestSetupClass.MembershipCards.Add(membershipCard);
            _beforeTestSetupClass.Employees.Add(employeeOb);

            var context = new SharedContext();

            context.MembershipCards.Add(membershipCard);
            context.Employees.Add(employeeOb);
            context.SaveChanges();
        }


    }
}