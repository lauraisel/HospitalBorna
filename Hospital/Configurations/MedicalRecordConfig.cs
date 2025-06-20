using Hospital.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Configurations
{
    public class MedicalRecordConfig : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasKey(mr => mr.Id);

            builder.Property(mr => mr.DiseaseName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(mr => mr.StartDate)
                .IsRequired();

            builder.HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId);
        }
    }

}
