namespace Artillery.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;

    public class ImportCountryIdDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}