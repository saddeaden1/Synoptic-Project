using System.Linq;
using System.Net;
using TechTalk.SpecFlow;
using FluentAssertions;
using MembershipSystem.Repository;

namespace MembershipSystem.AcceptanceTests.Steps
{
    [Binding]
    public class Then
    {
        private readonly BeforeTestSetupClass _beforeTestSetupClass;

        private readonly ResponseContext _responseContext;
        public Then(BeforeTestSetupClass beforeTestSetupClass, ResponseContext responseContext)
        {
            _beforeTestSetupClass = beforeTestSetupClass;
            _responseContext = responseContext;

        }


        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int p0)
        {
            var number = 5;

            number.Should().Be(6);
        }

        [Then(@"they should receive a ok result")]
        public void ThenTheyShouldReceiveAOkResult()
        {
            _responseContext.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"they should receive a bad request result")]
        public void ThenTheyShouldReceiveABadRequestResult()
        {
            _responseContext.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"they should be told the card does not exist")]
        public void ThenTheyShouldBeToldTheCardDoesNotExist()
        {
            _responseContext.Response.Content.ReadAsStringAsync().Result.Should().Be("Membership card does not exist");
        }

        [Then(@"they should be told their login details are incorrect")]
        public void ThenTheyShouldBeToldTheirLoginDetailsAreIncorrect()
        {
            _responseContext.Response.Content.ReadAsStringAsync().Result.Should().Be("");
        }

        [Then(@"they should be told which is incorrect")]
        public void ThenTheyShouldBeToldWhichIsIncorrect()
        {
            _responseContext.Response.Content.ReadAsStringAsync().Result.Should().Be("need to add table");
        }

        [Then(@"Their balance should increase by £(.*)")]
        public void ThenTheirBalanceShouldIncreaseBy(double p0)
        {
            var shared = new SharedContext();

            shared.MembershipCards.First().Balance.Should()
                    .Be(_beforeTestSetupClass.MembershipCards.First().Balance + p0);

        }

        [Then(@"Their balance should decrease by £(.*)")]
        public void ThenTheirBalanceShouldDecreaseBy(int p0)
        {
            var shared = new SharedContext();

            shared.MembershipCards.First().Balance.Should()
                .Be(_beforeTestSetupClass.MembershipCards.First().Balance - p0);
        }


        [Then(@"They should receive a authentication error")]
        public void ThenTheyShouldReceiveAAuthenticationError()
        {
            _responseContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Then(@"They should receive a timeout error")]
        public void ThenTheyShouldReceiveATimeoutError()
        {
            _responseContext.Response.Content.ReadAsStringAsync().Result.Should().Be("User timeout please login again");
        }


        [Then(@"they should be told '(.*)'")]
        public void ThenTheyShouldBeTold(string p0)
        {
            _responseContext.Response.Content.ReadAsStringAsync().Result.Should().Be(p0);
        }
    }
}