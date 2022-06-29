using System;

namespace MembershipSystem.Domain.DomainModels
{
    public class MembershipCard
    {
        public Guid MembershipCardId { get; set; } = Guid.NewGuid();
        public virtual Employee Employee { get; set; }
        public double Balance { get; set; }
    }
}
