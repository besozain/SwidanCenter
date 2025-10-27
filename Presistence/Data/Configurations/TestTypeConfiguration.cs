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
    public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> e)
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.HasMany(x => x.Results)
             .WithOne(r => r.TestType)
             .HasForeignKey(r => r.TestTypeId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
