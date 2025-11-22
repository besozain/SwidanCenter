

namespace Domain.Entities
{
    abstract public class BaseEntity <TKey>
    {
        public TKey Id { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
