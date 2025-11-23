namespace Invoices.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class ImportInvoiceDto
    {
        [Required]
        [JsonProperty("Number")]
        [Range(MinInvoiceNumberRange, MaxInvoiceNumberRange)]
        public int Number { get; set; }

        [Required]
        [JsonProperty("IssueDate")]
        public string IssueDate { get; set; } = null!;

        [Required]
        [JsonProperty("DueDate")]
        public string DueDate { get; set; } = null!;

        [Required]
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }

        [Required]
        [JsonProperty("CurrencyType")]
        [Range(MinInvoiceEnumRange, MaxInvoiceEnumRange)]
        public int CurrencyType { get; set; }

        [Required]
        [JsonProperty("ClientId")]
        public int ClientId { get; set; }
    }
}
