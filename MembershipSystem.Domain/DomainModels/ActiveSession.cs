using System;

namespace MembershipSystem.Domain.DomainModels
{
    public class ActiveSession
    {
        public Guid ActiveSessionId { get; set; } = Guid.NewGuid();
        public string LogOn { get; set; }
        public string EmployeeId { get; set; }
        public string SessionToken { get; set; }
        public string LastRequest { get; set; } = DateTime.Now.ToString();
    }
}

