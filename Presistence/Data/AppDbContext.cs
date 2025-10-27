using Domain.Entities.CoreEntites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Drug> Drugs => Set<Drug>();
        public DbSet<ExaminationType> ExaminationTypes => Set<ExaminationType>();
        public DbSet<FirstTrimesterExam> FirstTrimesterExams => Set<FirstTrimesterExam>();
        public DbSet<Husband> Husbands => Set<Husband>();
        public DbSet<MedicalExamination> MedicalExaminations => Set<MedicalExamination>();
        public DbSet<Operation> Operations => Set<Operation>();
        public DbSet<OperationCategory> OperationCategories => Set<OperationCategory>();
        public DbSet<OperationType> OperationTypes => Set<OperationType>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<PatientMedicationCourse> PatientMedicationCourses => Set<PatientMedicationCourse>();
        public DbSet<PatientNotes> PatientNotes => Set<PatientNotes>();
        public DbSet<PeriodData> PeriodData => Set<PeriodData>();
        public DbSet<Pregnancy> Pregnancies => Set<Pregnancy>();
        public DbSet<SecondThirdTrimesterExam> SecondThirdTrimesterExams => Set<SecondThirdTrimesterExam>();
        public DbSet<TestCategory> TestCategories => Set<TestCategory>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<TestType> TestTypes => Set<TestType>();
        public DbSet<Visit> Visits => Set<Visit>();
        public DbSet<PatientOperations> PatientOperations => Set<PatientOperations>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations automatically
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
