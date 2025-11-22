using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.CoreEntites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            e.Property(p => p.Id)
             .HasDefaultValueSql("NEWSEQUENTIALID()"); 
        }
    }
}
