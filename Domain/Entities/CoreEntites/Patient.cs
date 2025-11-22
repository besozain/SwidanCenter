using SharedData.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class Patient : BaseEntity<Guid>
    {
        // Identity
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }

        // Demographics
        public int AbortionCount { get; set; }
        public int PregnancyCount { get; set; }
        public int NumberOfMales { get; set; }
        public int NumberOfFemales { get; set; }
        public BirthType? LastBirthType { get; set; }
        public DateTime? LastBirthDate { get; set; }
        public AbortionType? LastAbortionType { get; set; }
        public DateTime? LastAbortionDate { get; set; }
        public DateTime? LastPeriodDate { get; set; }
        public BloodType? BloodType { get; set; }
        public bool? HasBloodPressure { get; set; }

        // Medical background
        public bool? HasDiabetes { get; set; }
        public string? MedicalHistoryStory { get; set; }
        public string? SurgeryHistoryStory { get; set; }

        // Navigation properties
        public List<Husband> Husbands { get; set; } = new List<Husband>();
        public List<PatientNotes> PatientNotes { get; set; } = new List<PatientNotes>();
        public List<PeriodData> PeriodDataRecords { get; set; } = new List<PeriodData>();
        public List<Pregnancy> Pregnancies { get; set; } = new List<Pregnancy>();
        public List<MedicalExamination> MedicalExaminations { get; set; } = new List<MedicalExamination>();
        public List<Operation> Operations { get; set; } = new List<Operation>();

        
        public List<TestResult> TestResults { get; set; } = new List<TestResult>();
       
        // في Patient
        public List<PatientMedicationCourse> PatientMedicationCourses { get; set; } = new();
        public List<PatientOperations> PatientOperations { get; set; } = new();



    }
}
