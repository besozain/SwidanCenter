namespace Domain.Entities.CoreEntites
{
    public class TestResult : BaseEntity<Guid>
    {
        public DateTime Date { get; set; }
        public string Result { get; set; } = default!;

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

        public int TestTypeId { get; set; }
        public TestType TestType { get; set; } = default!;
    }
}