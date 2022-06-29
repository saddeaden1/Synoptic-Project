using System;

namespace MembershipSystem.Domain.DomainModels
{
    public class Employee
    {
        public Guid EmployeeId { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string MembershipCardId { get; set; }
        public string Pin { get; set; }
    }
}
