using Domain.Entities.CoreEntites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Data.Configurations
{
    public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> e)
        {
            e.ToTable("TestResults");

            e.Property(x => x.Result).HasMaxLength(1024).IsRequired();
            e.HasIndex(x => new { x.PatientId, x.TestTypeId, x.Date });

            e.HasOne(x => x.Patient)
             .WithMany(p => p.TestResults)
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.TestType)
             .WithMany(t => t.Results)
             .HasForeignKey(x => x.TestTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Property(p => p.Id)
             .HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}
