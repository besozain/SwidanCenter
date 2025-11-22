namespace Domain.Entities.CoreEntites
{
    public class MedicalExamination : BaseEntity<Guid>
    {
        public DateTime Date { get; set; }
        public string Complain { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

        public int ExaminationTypeId { get; set; }
        public ExaminationType ExaminationType { get; set; } = default!;
        public List<Visit> Visits { get; set; } = new();
    }
}