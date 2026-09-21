namespace LightningCars.Api.DTOs
{
    public class CreateBookingRequest
    {
        public string CarType { get; set; } = string.Empty;

        public DateTime PickupDate { get; set; }

        public DateTime DropDate { get; set; }

        public string FromLocation { get; set; } = string.Empty;

        public string ToLocation { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}