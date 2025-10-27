using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence
{
    public class DataSeeding : IDataSeeding
    {
        private readonly AppDbContext appDbContext;
        public DataSeeding(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task SeedAllAsync()
        {
            await SeedPatientsAsync();
            await SeedHusbandsAsync();

            await SeedOperationCategoriesAsync();
            await SeedOperationTypesAsync();
            await SeedOperationsAsync();
            
            await SeedExaminationTypesAsync();
            await SeedMedicalExaminationsAsync();
            await SeedDrugsAsync();
            await SeedTestCategoriesAsync();
            await SeedTestTypesAsync();
            await SeedPregnanciesAsync();

            await SeedFirstTrimesterExamsAsync();
            await SeedSecondThirdTrimesterExamsAsync();

            
            await SeedPeriodDataAsync();

            
            await SeedVisitsAsync();
            await SeedPatientNotesAsync();

            await SeedPatientOperationsAsync();
            await SeedPatientMedicationCoursesAsync();
            await SeedTestResultsAsync();
        }

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        private static string SeedPath(string fileName)
        {

            var try1 = Path.Combine(".." ,"Presistence", "Data", "DataSeed", fileName);

            // آخر حاجة: DataSeed بجانب الـ EXE
            return try1; // هنرمي Exception لو مش لاقي الملف بالفعل
        }

        private async Task ReadAndSeedAsync<TEntity>(string fileName, DbSet<TEntity> dbset)
            where TEntity : class
        {
            // لو فيه داتا بالفعل، سيبه
            if (await dbset.AnyAsync()) return;

            var path = SeedPath(fileName);
            if (!File.Exists(path))
                throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var items = JsonSerializer.Deserialize<List<TEntity>>(json, _jsonOptions)
                        ?? new List<TEntity>();

            if (items.Count == 0) return;

            await using var tx = await appDbContext.Database.BeginTransactionAsync();
            try
            {
                await dbset.AddRangeAsync(items);
                await appDbContext.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= Implementations لكل ملف =========

        public async Task SeedDrugsAsync()
            => await ReadAndSeedAsync("Drug.json", appDbContext.Drugs);

        public async Task SeedExaminationTypesAsync()
            => await ReadAndSeedAsync("ExaminationType.json", appDbContext.ExaminationTypes);

        public async Task SeedFirstTrimesterExamsAsync()
            => await ReadAndSeedAsync("FirstTrimesterExam.json", appDbContext.FirstTrimesterExams);

        public async Task SeedHusbandsAsync()
            => await ReadAndSeedAsync("Husband.json", appDbContext.Husbands);

        public async Task SeedMedicalExaminationsAsync()
            => await ReadAndSeedAsync("MedicalExamination.json", appDbContext.MedicalExaminations);

        public async Task SeedOperationCategoriesAsync()
            => await ReadAndSeedAsync("OperationCategories.json", appDbContext.OperationCategories);

        public async Task SeedOperationsAsync()
            => await ReadAndSeedAsync("Operations.json", appDbContext.Operations);

        public async Task SeedOperationTypesAsync()
            => await ReadAndSeedAsync("OperationTypes.json", appDbContext.OperationTypes);

        public async Task SeedPatientMedicationCoursesAsync()
            => await ReadAndSeedAsync("PatientMedicationCourse.json", appDbContext.PatientMedicationCourses);

        public async Task SeedPatientNotesAsync()
            => await ReadAndSeedAsync("PatientNotes.json", appDbContext.PatientNotes);

        public async Task SeedPatientOperationsAsync()
            => await ReadAndSeedAsync("PatientOperations.json", appDbContext.PatientOperations);

        public async Task SeedPatientsAsync()
            => await ReadAndSeedAsync("Patients.json", appDbContext.Patients);

        public async Task SeedPeriodDataAsync()
            => await ReadAndSeedAsync("PeriodData.json", appDbContext.PeriodData);

        public async Task SeedPregnanciesAsync()
            => await ReadAndSeedAsync("Pregnancies.json", appDbContext.Pregnancies);

        public async Task SeedSecondThirdTrimesterExamsAsync()
            => await ReadAndSeedAsync("SecondThirdTrimesterExams.json", appDbContext.SecondThirdTrimesterExams);

        public async Task SeedTestCategoriesAsync()
            => await ReadAndSeedAsync("TestCategories.json", appDbContext.TestCategories);

        public async Task SeedTestResultsAsync()
            => await ReadAndSeedAsync("TestResults.json", appDbContext.TestResults);

        public async Task SeedTestTypesAsync()
            => await ReadAndSeedAsync("TestTypes.json", appDbContext.TestTypes);

        public async Task SeedVisitsAsync()
            => await ReadAndSeedAsync("Visits.json", appDbContext.Visits);
    }
}
    
