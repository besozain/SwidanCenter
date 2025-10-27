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
   public class ExaminationTypeConfiguration : IEntityTypeConfiguration<ExaminationType>
    {
        public void Configure(EntityTypeBuilder<ExaminationType> e)
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        }
    }
}
