using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ImportDtos.Json
{
    using static DataValidation.DataValidation.Citizen;

    public class ImportCitizen
    {
        [Required]
        [JsonProperty("FirstName")]
        [MinLength(MinFirstNameLength)]
        [MaxLength(MaxFirstNameLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [JsonProperty("LastName")]
        [MinLength(MinLastNameLength)]
        [MaxLength(MaxLastNameLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [JsonProperty("BirthDate")]
        public string Birthday { get; set; } = null!;

        [Required]
        [JsonProperty("MaritalStatus")]
        public string MaritalStatus { get; set; } = null!;

        [Required]
        [JsonProperty("Properties")]
        public int[] Properties { get; set; } = null!;
    }
}
