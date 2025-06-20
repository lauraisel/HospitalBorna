using Hospital.Configurations;
using Hospital.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hospital
{
    public class AppDbContext : DbContext
    {
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
        public DbSet<Checkup> Checkups => Set<Checkup>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PatientConfig());
            modelBuilder.ApplyConfiguration(new MedicalRecordConfig());
            modelBuilder.ApplyConfiguration(new CheckupConfig());
            modelBuilder.ApplyConfiguration(new PrescriptionConfig());
        }
    }
}
