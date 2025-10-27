using SharedData.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CoreEntites
{
    public class Pregnancy : BaseEntity
    {
        public ChildGender? ChildGender { get; set; }

        // Obstetric timing
        public DateTime? LmpDate { get; set; }   // Last Menstrual Period (كان مكتوب LAB في الرسم، افترضتها LMP)
        public DateTime? Edd { get; set; }       // Estimated Due Date

        public string? Progress { get; set; }

        public List<FirstTrimesterExam> FirstTrimesterExams { get; set; } = new();
        public List<SecondThirdTrimesterExam> SecondThirdTrimesterExams { get; set; } = new();
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
    }
}