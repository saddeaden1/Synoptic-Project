using MembershipSystem.Repository.DbModels;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace MembershipSystem.Repository
{
    [DbConfigurationType(typeof(MyDbContextConfig))]
    public class SharedContext : DbContext
    {
        public SharedContext() : base("Data Source = (localdb)\\ProjectsV13; Initial Catalog = SharedContext")
        {

        }
        public virtual DbSet<SessionHistoryDbModel> SessionHistorys { get; set; }
        public virtual DbSet<MembershipCardDbModel> MembershipCards { get; set; }
        public virtual DbSet<ActiveSessionDbModel> ActiveSessions { get; set; }
        public virtual DbSet<EmployeeDbModel> Employees { get; set; }
    }

    public class MyDbContextConfig : DbConfiguration
    {
        public MyDbContextConfig()
        {
            SetProviderServices("System.Data.EntityClient",
                SqlProviderServices.Instance);
            SetDefaultConnectionFactory(new SqlConnectionFactory());

        }
    }
}