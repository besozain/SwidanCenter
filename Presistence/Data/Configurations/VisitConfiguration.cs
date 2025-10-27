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
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> e)
        {
            e.ToTable("Visits");

            e.HasIndex(x => new { x.MedicalExaminationId, x.Date });

            // العلاقة: Visit → MedicalExamination
            e.HasOne(x => x.MedicalExamination)
             .WithMany(me => me.Visits)          
             .HasForeignKey(x => x.MedicalExaminationId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
