using Hospital.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Configurations
{
    public class CheckupConfig : IEntityTypeConfiguration<Checkup>
    {
        public void Configure(EntityTypeBuilder<Checkup> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CheckupTime)
                .IsRequired();

            builder.Property(c => c.Procedure)
                .IsRequired()
                .HasConversion<string>(); 

            builder.Property(c => c.ImagePath)
                .HasMaxLength(255);

            builder.HasOne(c => c.Patient)
                .WithMany(p => p.Checkups)
                .HasForeignKey(c => c.PatientId);
            
            builder.HasMany(c => c.Prescriptions)
                .WithOne(p => p.Checkup)
                .HasForeignKey(p => p.CheckupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
