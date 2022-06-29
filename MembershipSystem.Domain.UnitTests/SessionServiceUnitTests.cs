using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Authentication;
using FluentAssertions;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Repository;
using MembershipSystem.Repository.DbModels;
using Moq;
using NUnit.Framework;

namespace MembershipSystem.Domain.UnitTests
{
    public class SessionServiceUnitTests
    {
        [SetUp]
        public void Setup()
        {

            {
                _mockShared = new Mock<SharedContext>();

                var membershipCardData = new MembershipCardDbModel[]
                {
                new MembershipCardDbModel()
                {
                    MembershipCardId = "r7jTvbdqBy5wGO4L",
                    Balance = 3.4,
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "2b171734-b8d9-4014-a222-4791fc206899",
                    Balance = 3.4,
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "r7jTGvdqBy5wFO4L",
                    Balance = 24.2
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "R7jTvbdqBy5wGO4i"
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "r7jTvbdqBy5wGO4i"
                },

                }.AsQueryable();

                var membershipCardDbModel = new Mock<DbSet<MembershipCardDbModel>>();
                membershipCardDbModel.As<IQueryable<MembershipCardDbModel>>().Setup(m => m.Provider).Returns(membershipCardData.Provider);
                membershipCardDbModel.As<IQueryable<MembershipCardDbModel>>().Setup(m => m.Expression).Returns(membershipCardData.Expression);
                membershipCardDbModel.As<IQueryable<MembershipCardDbModel>>().Setup(m => m.ElementType).Returns(membershipCardData.ElementType);
                membershipCardDbModel.As<IQueryable<MembershipCardDbModel>>().Setup(m => m.GetEnumerator()).Returns(membershipCardData.GetEnumerator);

                var employeeData = new EmployeeDbModel[]
                {
                new EmployeeDbModel()
                {
                    EmployeeId = "d0770410-36f5-4f0f-a474-2edda10b7e77",
                    Pin = "4511",
                    MembershipCardDbModelId = "r7jTGvdqBy5wFO4L"
                },
                new EmployeeDbModel()
                {
                    EmployeeId = "00f63dc2-d165-4c38-957b-7c32144cdf01",
                    Pin = "4511",
                    MembershipCardDbModelId = "R7jTvbdqBy5wGO4i"
                },
                new EmployeeDbModel()
                {
                    EmployeeId = "00f65dc2-d165-4c38-957b-7c32144cdf01",
                    Pin = "4511",
                    MembershipCardDbModelId = "r7jTvbdqBy5wGO4L"
                },

                }.AsQueryable();


                var employeeDbModel = new Mock<DbSet<EmployeeDbModel>>();
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.Provider).Returns(employeeData.Provider);
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.Expression).Returns(employeeData.Expression);
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.ElementType).Returns(employeeData.ElementType);
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.GetEnumerator()).Returns(employeeData.GetEnumerator);

                var activeSessionData = new ActiveSessionDbModel[]
                {
                new ActiveSessionDbModel()
                {
                    ActiveSessionId = Guid.NewGuid(),
                    EmployeeId = "d0770410-36f5-4f0f-a474-2edda10b7e77",
                    SessionToken = "dab421fa-4ae2-4cd5-bbd2-7d9072b0d422",
                    LastRequest = (DateTime.Now-TimeSpan.FromMinutes(15)).ToString()

                },
                new ActiveSessionDbModel()
                {
                    ActiveSessionId = Guid.NewGuid(),
                    EmployeeId = "00f63dc2-d165-4c38-957b-7c32144cdf01",
                    SessionToken = "ef17fa3f-acc0-4776-bf71-1cdd270558eb",
                    LastRequest = (DateTime.Now-TimeSpan.FromMinutes(5)).ToString()
                },
                }.AsQueryable();



                var activeSessionDbModel = new Mock<DbSet<ActiveSessionDbModel>>();
                activeSessionDbModel.As<IQueryable<ActiveSessionDbModel>>().Setup(m => m.Provider).Returns(activeSessionData.Provider);
                activeSessionDbModel.As<IQueryable<ActiveSessionDbModel>>().Setup(m => m.Expression).Returns(activeSessionData.Expression);
                activeSessionDbModel.As<IQueryable<ActiveSessionDbModel>>().Setup(m => m.ElementType).Returns(activeSessionData.ElementType);
                activeSessionDbModel.As<IQueryable<ActiveSessionDbModel>>().Setup(m => m.GetEnumerator()).Returns(activeSessionData.GetEnumerator);


                var sessionHistoryData = new SessionHistoryDbModel[]
                {
                new SessionHistoryDbModel()
                {
                    SessionHistoryId = Guid.NewGuid(),
                    SessionToken = "fbfgb"
                },
                }.AsQueryable();

                var sessionHistoryDbModel = new Mock<DbSet<SessionHistoryDbModel>>();
                sessionHistoryDbModel.As<IQueryable<SessionHistoryDbModel>>().Setup(m => m.Provider).Returns(sessionHistoryData.Provider);
                sessionHistoryDbModel.As<IQueryable<SessionHistoryDbModel>>().Setup(m => m.Expression).Returns(sessionHistoryData.Expression);
                sessionHistoryDbModel.As<IQueryable<SessionHistoryDbModel>>().Setup(m => m.ElementType).Returns(sessionHistoryData.ElementType);
                sessionHistoryDbModel.As<IQueryable<SessionHistoryDbModel>>().Setup(m => m.GetEnumerator()).Returns(sessionHistoryData.GetEnumerator);



                _mockShared.Setup(s => s.MembershipCards).Returns(membershipCardDbModel.Object);
                _mockShared.Setup(s => s.Employees).Returns(employeeDbModel.Object);
                _mockShared.Setup(s => s.ActiveSessions).Returns(activeSessionDbModel.Object);
                _mockShared.Setup(s => s.SessionHistorys).Returns(sessionHistoryDbModel.Object);
            }
        }

        private Mock<SharedContext> _mockShared;

        [Test]
        public void LoggingIn_CardIsNotAValidType_ThrowsInvalidCardTypeException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<InvalidCardTypeException>(() => sut.LoggingIn("r7jTG7dqBy5wss", "4511"));
        }

        [Test]
        public void LoggingIn_CardIsAValidType_Passes()
        {

            var sut = new SessionService(_mockShared.Object);

            Assert.DoesNotThrow(() => sut.LoggingIn("r7jTvbdqBy5wGO4L", "4511"));

        }

        [Test]
        public void LoggingIn_CardDoesntExist_ThrowsCardNotExistException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<CardNotExistException>(() => sut.LoggingIn("r7jTvbdqBy5wGO4h", "3456"));
        }

        [Test]
        public void LoggingIn_CardIsNotRegistered_ThrowsCardNotRegisteredException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<CardNotRegisteredException>(() => sut.LoggingIn("r7jTvbdqBy5wGO4i", "3456"));
        }

        [Test]
        public void LoggingIn_InvalidPinType_Exception()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<InvalidPinTypeException>(() => sut.LoggingIn("r7jTGvdqBy5wFO4L", "45"));
        }

        [Test]
        public void LoggingIn_IncorrectPin_ThrowsInvalidCredentialException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<InvalidCredentialException>(() => sut.LoggingIn("r7jTGvdqBy5wFO4L", "4512"));
        }

        [Test]
        public void LoggingIn_AlreadyLoggedIn_ThrowsException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<AlreadyLoggedInException>(() => sut.LoggingIn("R7jTvbdqBy5wGO4i", "4511"));

        }

        [Test]
        public void LoggingOff_IncorrectPin_ShouldThrowInvalidCredentialException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<InvalidCredentialException>(() => sut.LoggingOff("ef17fa3f-acc0-4776-bf71-1cdd270558eb", "4534"));
        }
        [Test]
        public void LoggingOff_NoActiveSession_ShouldThrowException()
        {
            var sut = new SessionService(_mockShared.Object);

            var func = Assert.Throws<Exception>(() => sut.LoggingOff("0aaa78b6-59ca-4200-9e3b-89d4bbfa2913", "correct pin"));

            func.Message.Should().Be("You have no active session please log in again");
        }


        [Test]
        public void HasTimedOut_TimedOutSession_ReturnsTrue()
        {
            var sut = new SessionService(_mockShared.Object);

            var timeout = sut.HasTimedOut("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422");
            
            timeout.Should().Be(true);

        }

        [Test]
        public void HasTimedOut_NotTimedOutSession_ReturnsFalse()
        {
            var sut = new SessionService(_mockShared.Object);

            var timeout = sut.HasTimedOut("ef17fa3f-acc0-4776-bf71-1cdd270558eb");

            timeout.Should().Be(false);
        }

        [Test]
        public void UpdateLastRequestTime_UpdatesLastRequestTime_UpdateLastRequestShouldBeUpdated()
        {
            var updatedSessions = new List<ActiveSessionDbModel>();

            _mockShared.Setup(m => m.ActiveSessions.Add(It.IsAny<ActiveSessionDbModel>())).Callback<ActiveSessionDbModel>((entity) => updatedSessions.Add(entity));

            var sut = new SessionService(_mockShared.Object);
            sut.UpdateLastRequestTime("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422");

            updatedSessions.Count.Should().Be(1);

        }

        [Test]
        public void ValidateUser_WrongPin_ShouldThrowInvalidCredentialException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<InvalidCredentialException>(() => sut.ValidateUser("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422", "4532"));
        }

        [Test]
        public void ValidateUser_WrongSessionToken_ShouldThrowException()
        {
            var sut = new SessionService(_mockShared.Object);

            var func = Assert.Throws<Exception>(() => sut.ValidateUser("e6d4b7f9-b0ab-4272-ac6a-a6ea8cc0724a", "4532"));

            func.Message.Should().Be("Invalid Session token");
        }

        [Test]
        public void ValidateUser_InvalidSessionTokenType_ShouldThrowException()
        {
            var sut = new SessionService(_mockShared.Object);

            var func = Assert.Throws<Exception>(() => sut.ValidateUser("jon", "4532"));

            func.Message.Should().Be("Invalid Session Token Format");
        }

        [Test]
        public void ValidateUser_TimedOutSession_ThrowsTimeoutException()
        {
            var sut = new SessionService(_mockShared.Object);

            Assert.Throws<TimeoutException>(() => sut.ValidateUser("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422", "4511"));
        }


        [Test]
        public void TimeOutAllOldSessions_SessionsBeyond10Minutes_TimesThemOut()
        {
            
            var timedOutSessions = new List<ActiveSessionDbModel>();


            _mockShared.Setup(m => m.ActiveSessions.Remove(It.IsAny<ActiveSessionDbModel>())).Callback<ActiveSessionDbModel>((entity) => timedOutSessions.Add(entity));

            var sut = new SessionService(_mockShared.Object);
            sut.TimeOutAllOldSessions();

            timedOutSessions.Count.Should().Be(1);
            (DateTime.Now- DateTime.Parse(timedOutSessions.First().LastRequest) ).Should()
                .BeGreaterThan(TimeSpan.FromMinutes(10));

        }
    }
}