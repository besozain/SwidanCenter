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
    public class PeriodDataConfiguration : IEntityTypeConfiguration<PeriodData>
    {
        public void Configure(EntityTypeBuilder<PeriodData> e)
        {
            e.HasIndex(x => new { x.PatientId, x.Date });
            e.HasOne(x => x.Patient)
             .WithMany(p => p.PeriodDataRecords)
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
