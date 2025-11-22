using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CoreEntites
{
    public class OperationType : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        [ForeignKey("OperationCategory")]
        public int OperationCategoryId { get; set; }
        public OperationCategory OperationCategory { get; set; } = default!;
    }
}