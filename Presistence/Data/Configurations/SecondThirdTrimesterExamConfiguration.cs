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
    public class SecondThirdTrimesterExamConfiguration : IEntityTypeConfiguration<SecondThirdTrimesterExam>
    {
        public void Configure(EntityTypeBuilder<SecondThirdTrimesterExam> e)
        {
            e.HasIndex(x => new { x.PregnancyId, x.Week, x.Date });
            e.Property(x => x.FL).HasPrecision(6, 2);
            e.Property(x => x.AC).HasPrecision(6, 2);
            e.Property(x => x.BPD).HasPrecision(6, 2);
            e.Property(x => x.Notes).HasMaxLength(1024);

            e.HasOne(x => x.Pregnancy)
             .WithMany(p => p.SecondThirdTrimesterExams)
             .HasForeignKey(x => x.PregnancyId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
