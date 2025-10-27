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
    public class FirstTrimesterExamConfiguration : IEntityTypeConfiguration<FirstTrimesterExam>
    {
        public void Configure(EntityTypeBuilder<FirstTrimesterExam> e)
        {
            e.HasIndex(x => new { x.PregnancyId, x.Week, x.Date });
            e.Property(x => x.CRL).HasPrecision(6, 2);
            e.Property(x => x.GSD).HasPrecision(6, 2);
            e.Property(x => x.Notes).HasMaxLength(1024);

            e.HasOne(x => x.Pregnancy)
             .WithMany(p => p.FirstTrimesterExams)
             .HasForeignKey(x => x.PregnancyId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
