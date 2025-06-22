using Hospital.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Configurations
{
    public class CheckupImageConfig : IEntityTypeConfiguration<CheckupImage>
    {
        public void Configure(EntityTypeBuilder<CheckupImage> builder)
        {
            builder.HasKey(ci => ci.Id);
            
            builder.Property(ci => ci.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasOne(ci => ci.Checkup)
                   .WithMany(c => c.CheckupImages)
                   .HasForeignKey(ci => ci.CheckupId);
        }
    }

}
