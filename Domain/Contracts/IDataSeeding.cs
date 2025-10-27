using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDataSeeding
    {
        public Task SeedAllAsync();
        public Task SeedDrugsAsync();
        public Task SeedExaminationTypesAsync();
        public Task SeedFirstTrimesterExamsAsync();
        public Task SeedHusbandsAsync();
        public Task SeedMedicalExaminationsAsync();
        public Task SeedOperationCategoriesAsync();
        public Task SeedOperationsAsync();
        public Task SeedOperationTypesAsync();
        public Task SeedPatientMedicationCoursesAsync();
        public Task SeedPatientNotesAsync();
        public Task SeedPatientOperationsAsync();
        public Task SeedPatientsAsync();
        public Task SeedPeriodDataAsync();
        public Task SeedPregnanciesAsync();
        public Task SeedSecondThirdTrimesterExamsAsync();
        public Task SeedTestCategoriesAsync();
        public Task SeedTestResultsAsync();
        public Task SeedTestTypesAsync();
        public Task SeedVisitsAsync();
    }
}
