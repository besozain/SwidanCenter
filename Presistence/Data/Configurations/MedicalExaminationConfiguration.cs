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
    public class MedicalExaminationConfiguration : IEntityTypeConfiguration<MedicalExamination>
    {
        public void Configure(EntityTypeBuilder<MedicalExamination> e)
        {
            e.Property(x => x.Complain).HasMaxLength(1024);
            e.HasIndex(x => new { x.PatientId, x.Date });

            e.HasOne(x => x.Patient)
             .WithMany(p => p.MedicalExaminations)
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.ExaminationType)
             .WithMany(t => t.Examinations)
             .HasForeignKey(x => x.ExaminationTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Property(p => p.Id)
             .HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}
