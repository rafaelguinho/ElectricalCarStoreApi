using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectricalCarStoreApi.Models
{
    public class Car
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("manufactureYear")]
        public int ManufactureYear { get; set; }

        [JsonPropertyName("modelYear")]
        public int ModelYear { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("kilometersDriven")]
        public decimal KilometersDriven { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }


        [JsonPropertyName("plateLastNumber")]
        public string? PlateLastNumber { get; set; }


        [JsonPropertyName("transmissionType")]
        public string? TransmissionType { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }


    }
}
