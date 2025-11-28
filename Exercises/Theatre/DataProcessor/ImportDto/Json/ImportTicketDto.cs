namespace Theatre.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class ImportTicketDto
    {
        [JsonProperty("Price")]
        [Range((double)MinTicketPriceValue, (double)MaxTicketPriceValue)]
        public decimal Price { get; set; }

        [JsonProperty("RowNumber")]
        [Range(MinTickerRowNum, MaxTickerRowNum)]
        public sbyte RowNumber { get; set; }

        [JsonProperty("PlayId")]
        public int PlayId { get; set; }
    }
}