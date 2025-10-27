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
    public class OperationCategoryConfiguration : IEntityTypeConfiguration<OperationCategory>
    {
        public void Configure(EntityTypeBuilder<OperationCategory> e)
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.HasMany(x => x.OperationTypes)
             .WithOne(t => t.OperationCategory)
             .HasForeignKey(t => t.OperationCategoryId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
