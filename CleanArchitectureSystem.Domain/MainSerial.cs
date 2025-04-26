using CleanArchitectureSystem.Domain.Common;

namespace CleanArchitectureSystem.Domain
{
    public class MainSerial : BaseEntity
    {
        public required string SerialNo { get; set; }
        public required string ScanTo { get; set; }
        public string JoNo { get; set; } = string.Empty;

        // Foreign Key / Navigation Properties
        //public BatchSerial? BatchSerial { get; set; }
        public required string BatchSerial_ContractNo { get; set; }
    }
}
