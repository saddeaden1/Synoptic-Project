using System;
using System.ComponentModel.DataAnnotations;

namespace MembershipSystem.Repository.DbModels
{
    public class ActiveSessionDbModel
    {
        [Key]
        public Guid ActiveSessionId { get; set; }
        public string LogOn { get; set; }
        public string EmployeeId { get; set; }
        public string SessionToken { get; set; }
        public string LastRequest { get; set; }
    }
}

