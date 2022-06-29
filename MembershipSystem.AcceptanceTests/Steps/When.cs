using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Flurl;

namespace MembershipSystem.AcceptanceTests.Steps
{
    [Binding]
    public class When
    {

        private readonly BeforeTestSetupClass _beforeTestSetupClass;

        private readonly ResponseContext _responseContext;
        public When(BeforeTestSetupClass beforeTestSetupClass, ResponseContext responseContext)
        {
            _beforeTestSetupClass = beforeTestSetupClass;
            _responseContext = responseContext;
        }

        [When(@"the user registers with valid inputs")]
        public void WhenTheUserRegistersWithValidInputs()
        {

            var payload = new
            {
                FirstName = "Sadde",
                LastName = "Aden",
                EmailAddress = "adensaddeacceptancetest2@gmail.com",
                MobileNumber = "07967656789",
                MembershipCardId = "r7jTGvdqBy5wFO4L",
                Pin = 3453
            };

            var url = new Url("http://localhost:7071/api/Register");

            var stringPayload = JsonConvert.SerializeObject(payload);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.PostAsync(url, httpContent).Result;

            }
        }

        [When(@"the user registers with an invalid email")]
        public void WhenTheUserRegistersWithAnInvalidEmail()
        {
            var payload = new
            {
                FirstName = "Sadde",
                LastName = "Aden",
                EmailAddress = "adensaddeacceptancetest2gmail.com",
                MobileNumber = "07967656789",
                MembershipCardId = "r7jTGvdqBy5wFO4L",
                Pin = 3453
            };

            var url = new Url("http://localhost:7071/api/Register");

            var stringPayload = JsonConvert.SerializeObject(payload);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.PostAsync(url, httpContent).Result;

            }
        }

        [When(@"the user registers with an invalid phone number")]
        public void WhenTheUserRegistersWithAnInvalidPhoneNumber()
        {
            var payload = new
            {
                FirstName = "Sadde",
                LastName = "Aden",
                EmailAddress = "adensaddeacceptancetest2@gmail.com",
                MobileNumber = "079676567",
                MembershipCardId = "r7jTGvdqBy5wFO4L",
                Pin = 3453
            };

            var url = new Url("http://localhost:7071/api/Register");

            var stringPayload = JsonConvert.SerializeObject(payload);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.PostAsync(url, httpContent).Result;

            }
        }

        [When(@"the user registers with a null value")]
        public void WhenTheUserRegistersWithANullValue()
        {
            var payload = new
            {
                FirstName = "Sadde",
                LastName = "Aden",
                EmailAddress = "adensaddeacceptancetest2@gmail.com",
                MobileNumber = "07967656789",
                Pin = 3453
            };

            var url = new Url("http://localhost:7071/api/Register");

            var stringPayload = JsonConvert.SerializeObject(payload);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.PostAsync(url, httpContent).Result;

            }
        }



        [When(@"the user logs in")]
        public void WhenTheUserLogsIn()
        {
            var url = new Url($"http://localhost:7071/api/LogIn/{_beforeTestSetupClass.MembershipCards.First().MembershipCardId}/{_beforeTestSetupClass.Employees.First().Pin}");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }
        }

        [When(@"the user logs in while not being registered")]
        public void WhenTheUserLogsInWhileNotBeingRegistered()
        {
            var url = new Url($"http://localhost:7071/api/LogIn/{_beforeTestSetupClass.MembershipCards.First().MembershipCardId}/{5654}");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }
        }



        [When(@"they attempted to add £(.*)")]
        public void WhenTheyAttemptedToAdd(int p0)
        {
            var url = new Url($"http://localhost:7071/api/AddMoney/{_beforeTestSetupClass.ActiveSessions.First().SessionToken}/{_beforeTestSetupClass.Employees.First().Pin}/{p0}");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }
        }


        [When(@"they attempt to buy something for £(.*)")]
        public void WhenTheyAttemptToBuySomethingFor(int p0)
        {
            var url = new Url($"http://localhost:7071/api/BuySomething/{_beforeTestSetupClass.ActiveSessions.First().SessionToken}/{_beforeTestSetupClass.Employees.First().Pin}/{p0}");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }
        }

        [When(@"the user logs off")]
        public void WhenTheUserLogsOff()
        {

            var url = new Url($"http://localhost:7071/api/LogOff/{_beforeTestSetupClass.ActiveSessions.First().SessionToken}/{_beforeTestSetupClass.Employees.First().Pin}");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }

        }

        [When(@"the user logs off while having not logged in")]
        public void WhenTheUserLogsOffWhileHavingNotLoggedIn()
        {
            var guid = Guid.NewGuid().ToString();

            var url = new Url($"http://localhost:7071/api/LogOff/{guid}/{_beforeTestSetupClass.Employees.First().Pin}");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }
        }

        [When(@"they enter an incorrect pin")]
        public void WhenTheyEnterAnIncorrectPin()
        {
            var url = new Url($"http://localhost:7071/api/LogIn/{_beforeTestSetupClass.MembershipCards.First().MembershipCardId}/{4564}");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }
        }

        [When(@"the emlopyee enters an incorrect card id")]
        public void WhenTheEmlopyeeEntersAnIncorrectCardId()
        {

            var url = new Url($"http://localhost:7071/api/LogIn/r7jTGvdqBy5wFO4p/4564");

            using (var client = new HttpClient())
            {
                _responseContext.Response = client.GetAsync(url.ToString()).Result;
            }
        }


    }
}