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
    public class PatientOperationsConfiguration : IEntityTypeConfiguration<PatientOperations>
    {
        public void Configure(EntityTypeBuilder<PatientOperations> e)
        {
            e.ToTable("PatientOperations");

            e.Property(x => x.Notes).HasMaxLength(1024);
            e.HasIndex(x => new { x.PatientId, x.OperationId, x.Date });

            e.HasOne(x => x.Patient)
             .WithMany(p => p.PatientOperations)    
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Operation)
             .WithMany(o => o.PatientOperations)    
             .HasForeignKey(x => x.OperationId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Property(p => p.Id)
             .HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}

