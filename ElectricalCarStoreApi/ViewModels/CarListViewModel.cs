using System.Text.Json.Serialization;

namespace ElectricalCarStoreApi.ViewModels
{
    public class CarListViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public string? Model { get; set; }

        public int ManufactureYear { get; set; }

        public int ModelYear { get; set; }

        public decimal Price { get; set; }

        public decimal KilometersDriven { get; set; }

        public string? City { get; set; }
    }
}
