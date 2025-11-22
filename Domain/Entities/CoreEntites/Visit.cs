using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CoreEntites
{
    public class Visit : BaseEntity<Guid>
    {
        public DateTime Date { get; set; }

        [ForeignKey("MedicalExamination")]
        public Guid MedicalExaminationId { get; set; }
        public MedicalExamination MedicalExamination { get; set; } = default!;
    }
}