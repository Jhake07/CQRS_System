namespace CleanArchitectureSystem.Application.Features.BatchSerial
{
    public class BatchSerialDto
    {
        public string ContractNo { get; set; } = string.Empty;
        public string DocNo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public required string StartSNo { get; set; }
        public required string EndSNo { get; set; }

        // Foreign Key / Navigation Properties       
        public required string Item_ModelCode { get; set; }


    }
}
