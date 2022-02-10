using SMS_Report.Models.Configuration;
using SMS_Report.Models.Domains;
using System.Data.Entity;

namespace SMS_Report
{
    public class ServiceDbContext : DbContext
    {
        private static string _connectionString = ConnectionStringGenerator.GenerateConnectionString().ConnectionString;
        public ServiceDbContext()
            : base(_connectionString) { }

        public DbSet<Error> Errors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ErrorConfiguration());
        }
    }
}