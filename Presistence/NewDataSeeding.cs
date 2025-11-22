using System.Text.Json;
using Domain.Contracts;
using Domain.Entities.CoreEntites; // لو الصحيح CoreEntities عدّله هنا
using Microsoft.EntityFrameworkCore;
using Presistence.Data;

namespace Presistence
{
    public class NewDataSeeding : INewDataSeeding
    {
        private readonly AppDbContext _context;

        public NewDataSeeding(AppDbContext context)
        {
            _context = context;
        }

        // ========= JSON OPTIONS =========
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        // ========= PATH HELPERS =========
        private static string SeedPath(string fileName)
        {
            // يجعل المسار يعمل من أي working directory
            var baseDir = AppContext.BaseDirectory;

            // مسار شائع أثناء التشغيل من مشروع الـ API
            var candidate1 = Path.GetFullPath(
                Path.Combine(baseDir, "..", "..", "..", "DataSeed", fileName)
            );

            if (File.Exists(candidate1)) return candidate1;

            // مسار بديل عند التشغيل من مجلد آخر
            var candidate2 = Path.GetFullPath(
                Path.Combine(baseDir, "..", "Presistence", "Data", "DataSeed", fileName)
            );

            return File.Exists(candidate2) ? candidate2 : candidate1;
        }

        // ========= PUBLIC ENTRY =========
        public async Task SeedAllAsync()
        {
            // IMPORTANT: احفظ الترتيب بسبب الـ FK
            await SeedPatientsAsync();
            await SeedHusbandsAsync();

            await SeedOperationCategoriesAsync();
            await SeedOperationTypesAsync();
            await SeedOperationsAsync();

            await SeedExaminationTypesAsync();
            await SeedDrugsAsync();
            await SeedTestCategoriesAsync();
            await SeedTestTypesAsync();

            await SeedPregnanciesAsync();

            await SeedMedicalExaminationsAsync();
            await SeedFirstTrimesterExamsAsync();
            await SeedSecondThirdTrimesterExamsAsync();
            await SeedPeriodDataAsync();
            await SeedVisitsAsync();
            await SeedPatientNotesAsync();
            await SeedPatientOperationsAsync();
            await SeedPatientMedicationCoursesAsync();
            await SeedTestResultsAsync();
        }

        // ========= PATIENTS =========
        public async Task SeedPatientsAsync()
        {
            if (await _context.Patients.AnyAsync()) return;

            var path = SeedPath("Patients.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<PatientDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = new List<Patient>();
            foreach (var dto in dtos)
            {
                patients.Add(new Patient
                {
                    Id = Guid.NewGuid(),
                    FullName = dto.FullName?? string.Empty,
                    Address = dto.Address ?? string.Empty,
                    Age = dto.Age,
                    PhoneNumber = dto.PhoneNumber ?? string.Empty,
                    AbortionCount = dto.AbortionCount,
                    PregnancyCount = dto.PregnancyCount,
                    NumberOfMales = dto.NumberOfMales,
                    NumberOfFemales = dto.NumberOfFemales,
                    LastBirthType = (SharedData.Enums.BirthType?)dto.LastBirthType,
                    LastBirthDate = dto.LastBirthDate,
                    LastAbortionType = (SharedData.Enums.AbortionType?)dto.LastAbortionType,
                    LastAbortionDate = dto.LastAbortionDate,
                    LastPeriodDate = dto.LastPeriodDate,
                    BloodType = (SharedData.Enums.BloodType?)dto.BloodType,
                    HasBloodPressure = dto.HasBloodPressure,
                    HasDiabetes = dto.HasDiabetes,
                    MedicalHistoryStory = dto.MedicalHistoryStory,
                    SurgeryHistoryStory = dto.SurgeryHistoryStory
                });
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Patients.AddRangeAsync(patients);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= HUSBANDS =========
        public async Task SeedHusbandsAsync()
        {
            if (await _context.Husbands.AnyAsync()) return;

            var path = SeedPath("Husband.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<HusbandDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var husbands = new List<Husband>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count)
                {
                    husbands.Add(new Husband
                    {
                        Id = 0, // identity
                        Name = dto.Name,
                        Age = dto.Age,
                        IsSmoking = dto.IsSmoking,
                        IsDiabetic = dto.IsDiabetic,
                        PatientId = patients[dto.PatientId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Husbands.AddRangeAsync(husbands);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= OPERATION CATEGORIES =========
        public async Task SeedOperationCategoriesAsync()
        {
            if (await _context.OperationCategories.AnyAsync()) return;

            var path = SeedPath("OperationCategories.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<OperationCategoryDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var entities = dtos.Select(d => new OperationCategory
            {
                Id = 0,
                Name = d.Name ?? string.Empty
            }).ToList();

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.OperationCategories.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= OPERATION TYPES =========
        public async Task SeedOperationTypesAsync()
        {
            if (await _context.OperationTypes.AnyAsync()) return;

            var path = SeedPath("OperationTypes.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<OperationTypeDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var categories = await _context.OperationCategories.ToListAsync();
            var toAdd = new List<OperationType>();

            foreach (var dto in dtos)
            {
                if (dto.OperationCategoryId > 0 && dto.OperationCategoryId <= categories.Count)
                {
                    toAdd.Add(new OperationType
                    {
                        Id = 0,
                        Name = dto.Name ?? string.Empty,
                        OperationCategoryId = categories[dto.OperationCategoryId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.OperationTypes.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= OPERATIONS =========
        public async Task SeedOperationsAsync()
        {
            if (await _context.Operations.AnyAsync()) return;

            var path = SeedPath("Operations.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<OperationDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var types = await _context.OperationTypes.ToListAsync();
            var categories = await _context.OperationCategories.ToListAsync();
            var toAdd = new List<Operation>();

            foreach (var dto in dtos)
            {
                if (dto.OperationTypeId > 0 && dto.OperationTypeId <= types.Count &&
                    dto.OperationCategoryId > 0 && dto.OperationCategoryId <= categories.Count)
                {
                    toAdd.Add(new Operation
                    {
                        Id = 0,
                        OperationTypeId = types[dto.OperationTypeId - 1].Id,
                        OperationCategoryId = categories[dto.OperationCategoryId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Operations.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= EXAMINATION TYPES =========
        public async Task SeedExaminationTypesAsync()
        {
            if (await _context.ExaminationTypes.AnyAsync()) return;

            var path = SeedPath("ExaminationType.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<ExaminationTypeDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var toAdd = dtos.Select(d => new ExaminationType
            {
                Id = 0,
                Name = d.Name ?? string.Empty
            }).ToList();

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.ExaminationTypes.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= DRUGS =========
        public async Task SeedDrugsAsync()
        {
            if (await _context.Drugs.AnyAsync()) return;

            var path = SeedPath("Drug.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<DrugDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var toAdd = dtos.Select(d => new Drug
            {
                Id = 0,
                Name = d.Name ?? string.Empty,
                Description = d.Description ?? string.Empty
            }).ToList();

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Drugs.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= TEST CATEGORIES =========
        public async Task SeedTestCategoriesAsync()
        {
            if (await _context.TestCategories.AnyAsync()) return;

            var path = SeedPath("TestCategories.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<TestCategoryDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var toAdd = dtos.Select(d => new TestCategory
            {
                Id = 0,
                Name = d.Name ?? string.Empty
            }).ToList();

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.TestCategories.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= TEST TYPES =========
        public async Task SeedTestTypesAsync()
        {
            if (await _context.TestTypes.AnyAsync()) return;

            var path = SeedPath("TestTypes.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<TestTypeDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var categories = await _context.TestCategories.ToListAsync();
            var toAdd = new List<TestType>();

            foreach (var dto in dtos)
            {
                if (dto.TestCategoryId > 0 && dto.TestCategoryId <= categories.Count)
                {
                    toAdd.Add(new TestType
                    {
                        Id = 0,
                        Name = dto.Name ?? string.Empty,
                        TestCategoryId = categories[dto.TestCategoryId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.TestTypes.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= PREGNANCIES =========
        public async Task SeedPregnanciesAsync()
        {
            if (await _context.Pregnancies.AnyAsync()) return;

            var path = SeedPath("Pregnancies.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<PregnancyDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var toAdd = new List<Pregnancy>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count)
                {
                    toAdd.Add(new Pregnancy
                    {
                        Id = Guid.NewGuid(),
                        LmpDate = dto.LmpDate,
                        Edd = dto.Edd,
                        Progress = dto.Progress,
                        ChildGender = (SharedData.Enums.ChildGender?)dto.ChildGender,
                        PatientId = patients[dto.PatientId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Pregnancies.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= MEDICAL EXAMINATIONS =========
        public async Task SeedMedicalExaminationsAsync()
        {
            if (await _context.MedicalExaminations.AnyAsync()) return;

            var path = SeedPath("MedicalExamination.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<MedicalExaminationDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var examTypes = await _context.ExaminationTypes.ToListAsync();
            var toAdd = new List<MedicalExamination>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count &&
                    dto.ExaminationTypeId > 0 && dto.ExaminationTypeId <= examTypes.Count)
                {
                    toAdd.Add(new MedicalExamination
                    {
                        Id = Guid.NewGuid(),
                        Date = dto.Date,
                        Complain = dto.Complain ?? string.Empty,
                        PatientId = patients[dto.PatientId - 1].Id,
                        ExaminationTypeId = examTypes[dto.ExaminationTypeId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.MedicalExaminations.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= FIRST TRIMESTER EXAMS =========
        public async Task SeedFirstTrimesterExamsAsync()
        {
            if (await _context.FirstTrimesterExams.AnyAsync()) return;

            var path = SeedPath("FirstTrimesterExam.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<FirstTrimesterExamDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var pregnancies = await _context.Pregnancies.OrderBy(p => p.LmpDate).ToListAsync();
            var toAdd = new List<FirstTrimesterExam>();

            foreach (var dto in dtos)
            {
                if (dto.PregnancyId > 0 && dto.PregnancyId <= pregnancies.Count)
                {
                    toAdd.Add(new FirstTrimesterExam
                    {
                        Id = Guid.NewGuid(),
                        PregnancyId = pregnancies[dto.PregnancyId - 1].Id,
                        Date = dto.Date,
                        Week = dto.Week,
                        CRL = dto.CRL,
                        GSD = dto.GSD,
                        Notes = dto.Notes
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.FirstTrimesterExams.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= SECOND/THIRD TRIMESTER EXAMS =========
        public async Task SeedSecondThirdTrimesterExamsAsync()
        {
            if (await _context.SecondThirdTrimesterExams.AnyAsync()) return;

            var path = SeedPath("SecondThirdTrimesterExams.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<SecondThirdTrimesterExamDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var pregnancies = await _context.Pregnancies.OrderBy(p => p.LmpDate).ToListAsync();
            var toAdd = new List<SecondThirdTrimesterExam>();

            foreach (var dto in dtos)
            {
                if (dto.PregnancyId > 0 && dto.PregnancyId <= pregnancies.Count)
                {
                    toAdd.Add(new SecondThirdTrimesterExam
                    {
                        Id = Guid.NewGuid(),
                        PregnancyId = pregnancies[dto.PregnancyId - 1].Id,
                        Date = dto.Date,
                        Week = dto.Week,
                        FL = dto.FL,
                        AC = dto.AC,
                        BPD = dto.BPD,
                        Notes = dto.Notes ?? string.Empty
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SecondThirdTrimesterExams.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= PERIOD DATA =========
        public async Task SeedPeriodDataAsync()
        {
            if (await _context.PeriodData.AnyAsync()) return;

            var path = SeedPath("PeriodData.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<PeriodDataDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var toAdd = new List<PeriodData>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count)
                {
                    toAdd.Add(new PeriodData
                    {
                        Id = Guid.NewGuid(),
                        LastPeriodDate = dto.LastPeriodDate.HasValue
                            ? DateOnly.FromDateTime(dto.LastPeriodDate.Value)
                            : (DateOnly?)null,
                        Date = dto.Date,
                        Notes = dto.Notes,
                        PatientId = patients[dto.PatientId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.PeriodData.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= VISITS =========
        public async Task SeedVisitsAsync()
        {
            if (await _context.Visits.AnyAsync()) return;

            var path = SeedPath("Visits.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<VisitDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var examinations = await _context.MedicalExaminations.OrderBy(e => e.Date).ToListAsync();
            var toAdd = new List<Visit>();

            foreach (var dto in dtos)
            {
                if (dto.MedicalExaminationId > 0 && dto.MedicalExaminationId <= examinations.Count)
                {
                    toAdd.Add(new Visit
                    {
                        Id = Guid.NewGuid(),
                        Date = dto.Date,
                        MedicalExaminationId = examinations[dto.MedicalExaminationId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Visits.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= PATIENT NOTES =========
        public async Task SeedPatientNotesAsync()
        {
            if (await _context.PatientNotes.AnyAsync()) return;

            var path = SeedPath("PatientNotes.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<PatientNotesDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var toAdd = new List<PatientNotes>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count)
                {
                    toAdd.Add(new PatientNotes
                    {
                        Id = Guid.NewGuid(),
                        Notes = dto.Notes ?? string.Empty,
                        Date = dto.Date,
                        PatientId = patients[dto.PatientId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.PatientNotes.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= PATIENT OPERATIONS =========
        public async Task SeedPatientOperationsAsync()
        {
            if (await _context.PatientOperations.AnyAsync()) return;

            var path = SeedPath("PatientOperations.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<PatientOperationsDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var operations = await _context.Operations.ToListAsync();
            var toAdd = new List<PatientOperations>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count &&
                    dto.OperationId > 0 && dto.OperationId <= operations.Count)
                {
                    toAdd.Add(new PatientOperations
                    {
                        Id = Guid.NewGuid(),
                        Date = dto.Date,
                        Notes = dto.Notes,
                        PatientId = patients[dto.PatientId - 1].Id,
                        OperationId = operations[dto.OperationId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.PatientOperations.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= PATIENT MEDICATION COURSES =========
        public async Task SeedPatientMedicationCoursesAsync()
        {
            if (await _context.PatientMedicationCourses.AnyAsync()) return;

            var path = SeedPath("PatientMedicationCourse.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<PatientMedicationCourseDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var drugs = await _context.Drugs.ToListAsync();
            var toAdd = new List<PatientMedicationCourse>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count &&
                    dto.DrugId > 0 && dto.DrugId <= drugs.Count)
                {
                    toAdd.Add(new PatientMedicationCourse
                    {
                        Id = Guid.NewGuid(),
                        StartDate = dto.StartDate,
                        EndDate = dto.EndDate,
                        PatientId = patients[dto.PatientId - 1].Id,
                        DrugId = drugs[dto.DrugId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.PatientMedicationCourses.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ========= TEST RESULTS =========
        public async Task SeedTestResultsAsync()
        {
            if (await _context.TestResults.AnyAsync()) return;

            var path = SeedPath("TestResults.json");
            if (!File.Exists(path)) throw new FileNotFoundException($"Seed file not found: {path}");

            var json = await File.ReadAllTextAsync(path);
            var dtos = JsonSerializer.Deserialize<List<TestResultDto>>(json, _jsonOptions);
            if (dtos is null || dtos.Count == 0) return;

            var patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            var testTypes = await _context.TestTypes.ToListAsync();
            var toAdd = new List<TestResult>();

            foreach (var dto in dtos)
            {
                if (dto.PatientId > 0 && dto.PatientId <= patients.Count &&
                    dto.TestTypeId > 0 && dto.TestTypeId <= testTypes.Count)
                {
                    toAdd.Add(new TestResult
                    {
                        Id = Guid.NewGuid(),
                        Date = dto.Date,
                        Result = dto.Result ?? string.Empty,
                        PatientId = patients[dto.PatientId - 1].Id,
                        TestTypeId = testTypes[dto.TestTypeId - 1].Id
                    });
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.TestResults.AddRangeAsync(toAdd);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        #region DTOs
        private class PatientDto
        {
            public string? FullName { get; set; }
            public string? Address { get; set; }
            public int Age { get; set; }
            public string PhoneNumber { get; set; }
            public int AbortionCount { get; set; }
            public int PregnancyCount { get; set; }
            public int NumberOfMales { get; set; }
            public int NumberOfFemales { get; set; }
            public int? LastBirthType { get; set; }
            public DateTime? LastBirthDate { get; set; }
            public int? LastAbortionType { get; set; }
            public DateTime? LastAbortionDate { get; set; }
            public DateTime? LastPeriodDate { get; set; }
            public int? BloodType { get; set; }
            public bool? HasBloodPressure { get; set; }
            public bool? HasDiabetes { get; set; }
            public string? MedicalHistoryStory { get; set; }
            public string? SurgeryHistoryStory { get; set; }
        }

        private class HusbandDto
        {
            public string? Name { get; set; }
            public int? Age { get; set; }
            public bool? IsSmoking { get; set; }
            public bool? IsDiabetic { get; set; }
            public int PatientId { get; set; }
        }

        private class OperationCategoryDto
        {
            public string? Name { get; set; }
        }

        private class OperationTypeDto
        {
            public int OperationCategoryId { get; set; }
            public string? Name { get; set; }
        }

        private class OperationDto
        {
            public int OperationTypeId { get; set; }
            public int OperationCategoryId { get; set; }
        }

        private class ExaminationTypeDto
        {
            public string? Name { get; set; }
        }

        private class DrugDto
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
        }

        private class TestCategoryDto
        {
            public string? Name { get; set; }
        }

        private class TestTypeDto
        {
            public int TestCategoryId { get; set; }
            public string? Name { get; set; }
        }

        private class PregnancyDto
        {
            public int PatientId { get; set; }
            public DateTime? LmpDate { get; set; }
            public DateTime? Edd { get; set; }
            public string? Progress { get; set; }
            public int? ChildGender { get; set; }
        }

        private class MedicalExaminationDto
        {
            public int PatientId { get; set; }
            public int ExaminationTypeId { get; set; }
            public DateTime Date { get; set; }
            public string? Complain { get; set; }
        }

        private class FirstTrimesterExamDto
        {
            public int PregnancyId { get; set; }
            public DateTime Date { get; set; }
            public int Week { get; set; }
            public decimal CRL { get; set; }
            public decimal GSD { get; set; }
            public string? Notes { get; set; }
        }

        private class SecondThirdTrimesterExamDto
        {
            public int PregnancyId { get; set; }
            public DateTime Date { get; set; }
            public int Week { get; set; }
            public decimal FL { get; set; }
            public decimal AC { get; set; }
            public decimal BPD { get; set; }
            public string? Notes { get; set; }
        }

        private class PeriodDataDto
        {
            public int PatientId { get; set; }
            public DateTime? LastPeriodDate { get; set; }
            public DateTime? Date { get; set; }
            public string? Notes { get; set; }
        }

        private class VisitDto
        {
            public int MedicalExaminationId { get; set; }
            public DateTime Date { get; set; }
        }

        private class PatientNotesDto
        {
            public int PatientId { get; set; }
            public string? Notes { get; set; }
            public DateTime Date { get; set; }
        }

        private class PatientOperationsDto
        {
            public int PatientId { get; set; }
            public int OperationId { get; set; }
            public DateTime Date { get; set; }
            public string? Notes { get; set; }
        }

        private class PatientMedicationCourseDto
        {
            public int DrugId { get; set; }
            public int PatientId { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        private class TestResultDto
        {
            public int TestTypeId { get; set; }
            public int PatientId { get; set; }
            public string? Result { get; set; }
            public DateTime Date { get; set; }
        }
        #endregion
    }
}
