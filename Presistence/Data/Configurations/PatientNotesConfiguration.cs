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
    public class PatientNotesConfiguration : IEntityTypeConfiguration<PatientNotes>
    {
        public void Configure(EntityTypeBuilder<PatientNotes> e)
        {
            e.ToTable("PatientNotes");

            // Columns
            e.Property(x => x.Notes)
             .HasMaxLength(4000)        
             .IsRequired();             

          

            // Useful index for retrieval
            e.HasIndex(x => new { x.PatientId, x.Date });

            // Relation: Patient (1) -> PatientNotes (many)
            e.HasOne(x => x.Patient)
             .WithMany(p => p.PatientNotes)
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
