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
    public class TestCategoryConfiguration : IEntityTypeConfiguration<TestCategory>
    {
        public void Configure(EntityTypeBuilder<TestCategory> e)
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.HasMany(x => x.TestTypes)
             .WithOne(t => t.TestCategory)
             .HasForeignKey(t => t.TestCategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
