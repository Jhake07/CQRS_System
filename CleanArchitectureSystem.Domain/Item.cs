using CleanArchitectureSystem.Domain.Common;

namespace CleanArchitectureSystem.Domain
{
    public class Item : BaseEntity
    {
        public required string ModelCode { get; set; }
        public required string ModelName { get; set; }
    }
}
