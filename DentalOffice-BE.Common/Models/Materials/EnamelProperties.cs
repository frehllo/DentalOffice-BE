using Newtonsoft.Json;

namespace DentalOffice_BE.Common.Models;

public class EnamelProperties : MaterialProperties
{
    [JsonProperty("dentinColorsIds")]
    public long[]? dentinColorsIds { get; set; }
    [JsonProperty("dentinId")]
    public long? dentinId { get; set; }
}
