using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MembershipSystem.Repository;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using MembershipSystem.Domain.DomainModels;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Repository.DbModels;

namespace MembershipSystem.Domain.UnitTests
{
    public class EmployeeServiceUnitTests
    {
        private Mock<SharedContext> _mockShared;

        [SetUp]
        public void Setup()
        {

            {
                _mockShared = new Mock<SharedContext>();

                var membershipCardData = new MembershipCardDbModel[]
                {
                new MembershipCardDbModel()
                {
                    MembershipCardId = "new Guid()",
                    Balance = 3.4,
                    EmployeeDbModel = new EmployeeDbModel()
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "07faf5a9-02f2-4bcc-91de-a0b7a8487884",
                    Balance = 3.4,
                    EmployeeDbModel = new EmployeeDbModel()
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "r7jTGvdqBy5wFO4L",
                    Balance = 3.4,
                    EmployeeDbModel = new EmployeeDbModel()
                },
                new MembershipCardDbModel()
                {
                    MembershipCardId = "d0770110-36f5-4f0f-a474-2edda10b7e77"
                }
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
                    MembershipCardDbModelId = "d0770110-36f5-4f0f-a474-2edda10b7e77"
                },

                }.AsQueryable();


                var employeeDbModel = new Mock<DbSet<EmployeeDbModel>>();
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.Provider).Returns(employeeData.Provider);
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.Expression).Returns(employeeData.Expression);
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.ElementType).Returns(employeeData.ElementType);
                employeeDbModel.As<IQueryable<EmployeeDbModel>>().Setup(m => m.GetEnumerator()).Returns(employeeData.GetEnumerator);

                _mockShared.Setup(s => s.MembershipCards).Returns(membershipCardDbModel.Object);
                _mockShared.Setup(s => s.Employees).Returns(employeeDbModel.Object);
            }
        }

        [Test]
        public void CreatingEmployee_EntersInaNull_ThrowsNullInputException()
        {
            var sut = new EmployeeService(_mockShared.Object);

            Assert.Throws<NullInputException>(() =>
            {
                sut.CreatingEmployee(new Employee()
                {
                    MembershipCardId = "joe"
                });
            });

        }

        [Test]
        public void CreatingEmployee_EntersInvalidEmail_ThrowsEmailInvalidException()
        {
            var sut = new EmployeeService(_mockShared.Object);

            Assert.Throws<EmailInvalidException>(() =>
                {
                    sut.CreatingEmployee(new Employee()
                    {
                        MembershipCardId = "joe",
                        EmailAddress = "sadde",
                        MobileNumber = "2340234",
                        EmployeeId = Guid.NewGuid(),
                        FirstName = "Sadde",
                        LastName = "Aden",
                        Pin = "3453"
                    });
                });
        }

        [Test]
        public void CreatingEmployee_EntersInvalidNumber_ThrowsNumberInvalidException()
        {
            var sut = new EmployeeService(_mockShared.Object);

            Assert.Throws<InvalidPhoneNumberException>(() =>
            {
                sut.CreatingEmployee(new Employee()
                {
                    MembershipCardId = "r7jTGvdqBy5wFO4L",
                    EmailAddress = "adensadde@gmail.com",
                    MobileNumber = "2340234",
                    EmployeeId = Guid.NewGuid(),
                    FirstName = "Sadde",
                    LastName = "Aden",
                    Pin = "3453"
                });
            });
        }

        [Test]
        public void CreatingEmployee_UsingACardThatIsAlreadyRegistered_ThrowsEmployeeExistsException()
        {
            var sut = new EmployeeService(_mockShared.Object);

            Assert.Throws<CardAlreadyRegisteredException>(() =>
            {
                sut.CreatingEmployee(new Employee()
                {
                    MembershipCardId = "d0770110-36f5-4f0f-a474-2edda10b7e77",
                    EmailAddress = "adensadde@gmail.com",
                    MobileNumber = "07956765678",
                    EmployeeId = new Guid("d0770410-36f5-4f0f-a474-2edda10b7e77"),
                    FirstName = "Sadde",
                    LastName = "Aden",
                    Pin = "3453"
                });
            });
        }


        [Test]
        public void CreatingEmployee_ValidEntry_PassessValidation()
        {
            var sut = new EmployeeService(_mockShared.Object);

            Assert.DoesNotThrow(() =>
            {
                sut.CreatingEmployee(new Employee()
                {
                    MembershipCardId = "r7jTGvdqBy5wFO4L",
                    EmailAddress = "adensadde@gmail.com",
                    MobileNumber = "07987898767",
                    EmployeeId = new Guid("bb366cb3-08ad-4e46-8d0c-f0f9588f256d"),
                    FirstName = "Sadde",
                    LastName = "Aden",
                    Pin = "3453"
                });
            });

        }


        [Test]
        public void CreatingEmployee_ValidEntry_IsAddedToDb()
        {
            var list = new List<EmployeeDbModel>();

            _mockShared.Setup(m => m.Employees.Add(It.IsAny<EmployeeDbModel>())).Callback<EmployeeDbModel>((entity) => list.Add(entity));
            

            var sut = new EmployeeService(_mockShared.Object);

            sut.CreatingEmployee(new Employee()
            {
                MembershipCardId = "r7jTGvdqBy5wFO4L",
                EmailAddress = "adensadde@gmail.com",
                MobileNumber = "07987898767",
                EmployeeId = new Guid("bb366cb3-08ad-4e46-8d0c-f0f9588f256d"),
                FirstName = "Sadde",
                LastName = "Aden",
                Pin = "3453"
            });

            list.Count.Should().Be(1);
            list.First().FirstName.Should().Be("Sadde");
        }

        
        [Test]
        public void CreatingEmployee_EntersInvalidCardType_ThrowsInvalidCardTypeException()
        {
            var sut = new EmployeeService(_mockShared.Object);

            Assert.Throws<InvalidCardTypeException>(() =>
            {
                sut.CreatingEmployee(new Employee()
                {
                    MembershipCardId = "nocardd",
                    EmailAddress = "adensadde@gmail.com",
                    MobileNumber = "07987898767",
                    EmployeeId = new Guid("bb366cb3-08ad-4e46-8d0c-f0f9588f256d"),
                    FirstName = "Sadde",
                    LastName = "Aden",
                    Pin = "3453"
                });
            });

        }


        [Test]
        public void EntersInvalidPinType()
        {
            var sut = new EmployeeService(_mockShared.Object);

            Assert.Throws<InvalidPinTypeException>(() =>
            {
                sut.CreatingEmployee(new Employee()
                {
                    MembershipCardId = "07faf5a9-02f2-4bcc-91de-a0b7a8487884",
                    EmailAddress = "adensadde@gmail.com",
                    MobileNumber = "07987898767",
                    EmployeeId = new Guid("bb366cb3-08ad-4e46-8d0c-f0f9588f256d"),
                    FirstName = "Sadde",
                    LastName = "Aden",
                    Pin = "343"
                });
            });
        }
    }
}