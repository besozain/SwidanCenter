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
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> e)
        {
            // ---------- Columns ----------
            e.ToTable("Patients");

            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Address).HasMaxLength(256).IsRequired();

            e.Property(x => x.MedicalHistoryStory).HasMaxLength(4000);
            e.Property(x => x.SurgeryHistoryStory).HasMaxLength(4000);

            // فهارس مفيدة للاستعلام
            e.HasIndex(x => new { x.LastName, x.FirstName });
            e.HasIndex(x => x.Age);

            // ---------- One-to-Many ----------
            // Patient -> Husbands
            e.HasMany(p => p.Husbands)
             .WithOne(h => h.Patient)
             .HasForeignKey(h => h.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            // Patient -> PatientNotes
            e.HasMany(p => p.PatientNotes)
             .WithOne(n => n.Patient)
             .HasForeignKey(n => n.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            // Patient -> PeriodDataRecords
            e.HasMany(p => p.PeriodDataRecords)
             .WithOne(pd => pd.Patient)
             .HasForeignKey(pd => pd.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            // Patient -> Pregnancies
            e.HasMany(p => p.Pregnancies)
             .WithOne(g => g.Patient)
             .HasForeignKey(g => g.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            // Patient -> MedicalExaminations
            e.HasMany(p => p.MedicalExaminations)
             .WithOne(m => m.Patient)
             .HasForeignKey(m => m.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            // ---------- Many-to-Many: Patient <-> Operation (بـ Payload: PatientOperations) ----------
            // عندك كيان PatientOperations فيه Date/Notes و FK للـ Patient و Operation.
            // هنستخدمه كجدول ربط صريح:
            e.HasMany(p => p.Operations)
             .WithMany(o => o.Patients)
             .UsingEntity<PatientOperations>(
                // فرع Operation
                right => right
                    .HasOne(po => po.Operation)
                    .WithMany()                // لو عندك: Operation.PatientOperations حطها هنا بدل WithMany()
                    .HasForeignKey(po => po.OperationId)
                    .OnDelete(DeleteBehavior.Restrict),

                // فرع Patient
                left => left
                    .HasOne(po => po.Patient)
                    .WithMany()                // لو أضفت: Patient.PatientOperations حطها هنا بدل WithMany()
                    .HasForeignKey(po => po.PatientId)
                    .OnDelete(DeleteBehavior.Cascade),

                // إعدادات جدول الربط نفسه
                join =>
                {
                    join.ToTable("PatientOperations");
                    join.HasIndex(x => new { x.PatientId, x.OperationId, x.Date });
                    join.Property(x => x.Notes).HasMaxLength(1024);
                }
             );

            e.HasMany(p => p.TestResults)
             .WithOne(tr => tr.Patient)
             .HasForeignKey(tr => tr.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.Property(p => p.Id)
             .HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}
