using System;
using System.ComponentModel.DataAnnotations;

namespace MembershipSystem.Repository.DbModels
{
    public class SessionHistoryDbModel
    {
        [Key]
        public Guid SessionHistoryId { get; set; }
        public string LogOn { get; set; }
        public string LogOff { get; set; }
        public string EmployeeId { get; set; }
        public string SessionToken { get; set; }
    }
}
