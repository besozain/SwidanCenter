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
    public class HusbandConfiguration : IEntityTypeConfiguration<Husband>
    {
        public void Configure(EntityTypeBuilder<Husband> e)
        {
            e.Property(x => x.Name).HasMaxLength(100);
            e.HasIndex(x => x.PatientId).IsUnique(); // 1-1

            e.HasOne(x => x.Patient)
             .WithMany(p => p.Husbands)
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
