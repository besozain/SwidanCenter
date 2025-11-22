using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface INewDataSeeding
    {
        Task SeedAllAsync();
        Task SeedPatientsAsync();
        Task SeedHusbandsAsync();
        Task SeedOperationCategoriesAsync();
        Task SeedOperationTypesAsync();
        Task SeedOperationsAsync();
        Task SeedExaminationTypesAsync();
        Task SeedMedicalExaminationsAsync();
        Task SeedDrugsAsync();
        Task SeedTestCategoriesAsync();
        Task SeedTestTypesAsync();
        Task SeedPregnanciesAsync();
        Task SeedFirstTrimesterExamsAsync();
        Task SeedSecondThirdTrimesterExamsAsync();
        Task SeedPeriodDataAsync();
        Task SeedVisitsAsync();
        Task SeedPatientNotesAsync();
        Task SeedPatientOperationsAsync();
        Task SeedPatientMedicationCoursesAsync();
        Task SeedTestResultsAsync();
    }

}
