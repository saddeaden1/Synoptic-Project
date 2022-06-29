using System;

namespace MembershipSystem.Domain.DomainModels
{
    public class SessionHistory
    {
        public Guid SessionHistoryId { get; set; } = Guid.NewGuid();
        public string LogOn { get; set; }
        public string LogOff { get; set; }
        public string EmployeeId { get; set; }
    }
}
