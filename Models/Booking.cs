using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightningCars.Api.Models
{
    [Table("bookings")]
    public class Booking
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("car_type")]
        public string CarType { get; set; } = string.Empty;

        [Column("pickup_date")]
        public DateTime PickupDate { get; set; }

        [Column("drop_date")]
        public DateTime DropDate { get; set; }

        [Required]
        [Column("from_location")]
        public string FromLocation { get; set; } = string.Empty;

        [Required]
        [Column("to_location")]
        public string ToLocation { get; set; } = string.Empty;

        [Required]
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        public string Status { get; set; } = "PENDING";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}