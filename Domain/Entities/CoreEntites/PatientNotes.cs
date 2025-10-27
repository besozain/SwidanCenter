

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CoreEntites
{
    public class PatientNotes : BaseEntity
    {
        
        public string Notes { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
    }
}
