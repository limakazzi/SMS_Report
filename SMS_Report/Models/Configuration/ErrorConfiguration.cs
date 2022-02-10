using SMS_Report.Models.Domains;
using System.Data.Entity.ModelConfiguration;

namespace SMS_Report.Models.Configuration
{
    public class ErrorConfiguration : EntityTypeConfiguration<Error>
    {
        public ErrorConfiguration()
        {
            ToTable("dbo.Errors");
            HasKey(x => x.Id);

            Property(x => x.Date)
                .IsRequired();

            Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();

            Property(x => x.Description)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.IsSent)
                .IsRequired();
        }
    }
}
