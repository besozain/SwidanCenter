
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.CoreEntites
{
    public class PeriodData : BaseEntity
    {
        public DateOnly? LastPeriodDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? Date { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
    }
}
