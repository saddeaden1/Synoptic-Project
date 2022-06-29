using System.ComponentModel.DataAnnotations;

namespace MembershipSystem.Repository.DbModels
{
    public class EmployeeDbModel
    {
        [Key]
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string MembershipCardDbModelId { get; set; }
        public string Pin { get; set; }
    }
}
