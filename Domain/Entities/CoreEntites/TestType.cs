namespace Domain.Entities.CoreEntites
{
    public class TestType : BaseEntity<int>
    {
        public string Name { get; set; } = default!;

        public int TestCategoryId { get; set; }
        public TestCategory TestCategory { get; set; } = default!;

        public List<TestResult> Results { get; set; } = new();
    }
}