using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataMigrations.IntegrationTests.EF
{
    public class TestContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Person { get; set; }
    }
}
