using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.CoreEntites;


namespace Presistence.Data.Configurations
{
    public class OperationConfiguration : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> e)
        {
            // علاقات Operation مع القواميس فقط
            e.HasOne(x => x.OperationType)
             .WithMany() // أو WithMany(t => t.Operations) لو عندك Navigation في OperationType
             .HasForeignKey(x => x.OperationTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.OperationCategory)
             .WithMany() // عادةً مفيش Navigation من Category
             .HasForeignKey(x => x.OperationCategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            // فهرس مفيد
            e.HasIndex(x => new { x.OperationTypeId, x.OperationCategoryId });
        }
    }
}
