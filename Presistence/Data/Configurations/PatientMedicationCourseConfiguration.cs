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
    public class PatientMedicationCourseConfiguration : IEntityTypeConfiguration<PatientMedicationCourse>
    {
        public void Configure(EntityTypeBuilder<PatientMedicationCourse> e)
        {
            e.ToTable("PatientMedicationCourses");

            // أعمدة التواريخ (اختياري: خليه date لو مش محتاج وقت)
            // e.Property(x => x.StartDate).HasColumnType("date");
            // e.Property(x => x.EndDate).HasColumnType("date");

            // فهرس مفيد لتجنّب التكرار والبحث
            e.HasIndex(x => new { x.PatientId, x.DrugId, x.StartDate, x.EndDate });

           
            e.HasOne(x => x.Patient)
             .WithMany(p => p.PatientMedicationCourses) // ⬅️ محتاج تضيفها في Patient
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            // FK: Drug (Many)
            e.HasOne(x => x.Drug)
             .WithMany(d => d.PatientMedicationCourses) // ⬅️ محتاج تضيفها في Drug
             .HasForeignKey(x => x.DrugId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Property(p => p.Id)
             .HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}
