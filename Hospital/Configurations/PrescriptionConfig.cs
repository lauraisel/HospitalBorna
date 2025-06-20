using Hospital.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Configurations
{
    public class PrescriptionConfig : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Medication)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Dosage)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(p => p.Checkup)
                .WithMany(c => c.Prescriptions)
                .HasForeignKey(p => p.CheckupId);
        }
    }

}
