using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Repository;
using MembershipSystem.Repository.DbModels;
using Moq;
using NUnit.Framework;

namespace MembershipSystem.Domain.UnitTests
{
    public class TransactionServiceUnitTests
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
                    MembershipCardId = "hiiiiiiiiiiii",
                    Balance = 3.4,
                    EmployeeDbModel = new EmployeeDbModel()
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "dab421fa-4ae2-4cd5-bbd2-7d9072b0d422",
                    Balance = 24.2,
                    EmployeeDbModel = new EmployeeDbModel()
                    {
                        EmployeeId = "d0770410-36f5-4f0f-a474-2edda10b7e77",
                    },
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
                    MembershipCardDbModelId = "dab421fa-4ae2-4cd5-bbd2-7d9072b0d422"
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
                    SessionToken = "dab421fa-4ae2-4cd5-bbd2-7d9072b0d422"
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
            }
        }

        private Mock<SharedContext> _mockShared;

        [Test]
        public void BuySomething_NotEnoughMoney_ThrowsNotEnoughMoneyException()
        {
            var sut1 = new TransactionService(_mockShared.Object);

            Assert.Throws<InsufficientFundsException>(() =>
            {
                sut1.BuySomething("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422", "45.45");
            });
        }

        [Test]
        public void AddMoney_DbThrowsException_IsNotAdded()
        {
            var addedCards = new List<ActiveSessionDbModel>();
            var deletedCards = new List<ActiveSessionDbModel>();


            _mockShared.Setup(s => s.MembershipCards.Add(It.IsAny<MembershipCardDbModel>())).Throws<Exception>();
            _mockShared.Setup(s => s.MembershipCards.Remove(It.IsAny<MembershipCardDbModel>())).Throws<Exception>();
            _mockShared.Setup(m => m.ActiveSessions.Add(It.IsAny<ActiveSessionDbModel>())).Callback<ActiveSessionDbModel>((entity) => addedCards.Add(entity));
            _mockShared.Setup(m => m.ActiveSessions.Remove(It.IsAny<ActiveSessionDbModel>())).Callback<ActiveSessionDbModel>((entity) => deletedCards.Add(entity));


            var sut = new TransactionService(_mockShared.Object);

            Assert.Throws<FailedTransactionException>(() =>
            {
                sut.AddMoney("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422", "5.4");
            });

            addedCards.Count.Should().Be(0);
            deletedCards.Count.Should().Be(0);

        }


        [Test]
        public void AddMoney_ValidInputs_AddsMoneyToCard()
        {
            var addedCards = new List<MembershipCardDbModel>();
            var deletedCards = new List<MembershipCardDbModel>();


            _mockShared.Setup(m => m.MembershipCards.Add(It.IsAny<MembershipCardDbModel>())).Callback<MembershipCardDbModel>((entity) => addedCards.Add(entity));
            _mockShared.Setup(m => m.MembershipCards.Remove(It.IsAny<MembershipCardDbModel>())).Callback<MembershipCardDbModel>((entity) => deletedCards.Add(entity));


            var sut = new TransactionService(_mockShared.Object);

            sut.AddMoney("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422", "5.4");

            addedCards.Count.Should().Be(1);
            deletedCards.Count.Should().Be(1);

            addedCards.First().Balance.Should().Be(5.4+24.2);

        }

        [Test]
        public void BuySomething_DbThrowsException_IsNotAdded()
        {
            var addedCards = new List<ActiveSessionDbModel>();
            var deletedCards = new List<ActiveSessionDbModel>();


            _mockShared.Setup(s => s.MembershipCards.Add(It.IsAny<MembershipCardDbModel>())).Throws<Exception>();
            _mockShared.Setup(s => s.MembershipCards.Remove(It.IsAny<MembershipCardDbModel>())).Throws<Exception>();
            _mockShared.Setup(m => m.ActiveSessions.Add(It.IsAny<ActiveSessionDbModel>())).Callback<ActiveSessionDbModel>((entity) => addedCards.Add(entity));
            _mockShared.Setup(m => m.ActiveSessions.Remove(It.IsAny<ActiveSessionDbModel>())).Callback<ActiveSessionDbModel>((entity) => deletedCards.Add(entity));


            var sut = new TransactionService(_mockShared.Object);

            Assert.Throws<FailedTransactionException>(() =>
            {
                sut.BuySomething("dab421fa-4ae2-4cd5-bbd2-7d9072b0d422", "5.4");
            });

            addedCards.Count.Should().Be(0);
            deletedCards.Count.Should().Be(0);
        }


    }
}