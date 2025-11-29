namespace NetPay.DataProcessor.ImportDtos.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class ImportExpenseDto
    {
        [Required]
        [JsonProperty("ExpenseName")]
        [MinLength(MinExpenseNameLength)]
        [MaxLength(MaxExpenseNameLength)]
        public string ExpenseName { get; set; } = null!;

        [JsonProperty("Amount")]
        [Range((double)MinExpenseAmountValue, (double)MaxExpenseAmountValue)]
        public decimal Amount { get; set; }

        [Required]
        [JsonProperty("DueDate")]
        public string DueDate { get; set; } = null!;

        [Required]
        [JsonProperty("PaymentStatus")]
        public string PaymentStatus { get; set; } = null!;

        [JsonProperty("HouseholdId")]
        public int HouseholdId { get; set; }

        [JsonProperty("ServiceId")]
        public int ServiceId { get; set; }
    }
}
