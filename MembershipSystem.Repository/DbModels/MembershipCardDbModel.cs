using System.ComponentModel.DataAnnotations;

namespace MembershipSystem.Repository.DbModels
{
    public class MembershipCardDbModel
    {
        [Key]
        public string MembershipCardId { get; set; }
        public virtual EmployeeDbModel EmployeeDbModel { get; set; }
        public double Balance { get; set; }
    }
}
