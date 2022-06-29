using System.Collections.Generic;
using MembershipSystem.Repository.DbModels;

namespace MembershipSystem.AcceptanceTests
{
    public class BeforeTestSetupClass
    {
        public List<MembershipCardDbModel> MembershipCards { get; set; } = new List<MembershipCardDbModel>();
        public List<ActiveSessionDbModel> ActiveSessions { get; set; } = new List<ActiveSessionDbModel>();
        public List<EmployeeDbModel> Employees { get; set; } = new List<EmployeeDbModel>();
    }
}