using Microsoft.EntityFrameworkCore;
using Hospital.Context;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Context {
    public class HospitalSchema : DbContext, IDisposable{

        public DbSet<Department> Department {get; set;}
        public DbSet<Patient> Patient {get; set;}
        public DbSet<Doctor> Doctor {get; set;}
        public DbSet<Appointment> Appointment {get;set;}
        public DbSet<Score> Score {get; set;}

        public HospitalSchema(DbContextOptions options): base(options) {}

        public new void Dispose() {
            base.Dispose();
        }
    }
}