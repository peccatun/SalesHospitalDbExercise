﻿using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(
                    "Server=DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
                    "Database=HospitalDatabase;" +
                    "Integrated Security=true;");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Visitation>()
                .HasOne(v => v.Patient)
                .WithMany(p => p.Visitations)
                .HasForeignKey(v => v.PatientId);

            modelBuilder
                .Entity<Diagnose>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(pm => new { pm.PatientId, pm.MedicamentId });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(pm => pm.Patient)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(p => p.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(pm => pm.MedicamentId);

            modelBuilder
                .Entity<Visitation>()
                .HasOne(v => v.Doctor)
                .WithMany(v => v.Visitations)
                .HasForeignKey(v => v.DoctorId);
        }
    }
}
