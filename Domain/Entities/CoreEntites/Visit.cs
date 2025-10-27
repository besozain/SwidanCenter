using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CoreEntites
{
    public class Visit : BaseEntity
    {
        public DateTime Date { get; set; }

        [ForeignKey("MedicalExamination")]
        public int MedicalExaminationId { get; set; }
        public MedicalExamination MedicalExamination { get; set; } = default!;
    }
}