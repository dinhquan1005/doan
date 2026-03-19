using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models
{
    public class Trip
    {
        [Key]
        public int TripID { get; set; }

        [Required]
        public string PickupLocation { get; set; }

        [Required]
        public string DropoffLocation { get; set; }

        public decimal Price { get; set; }

        public TripStatus Status { get; set; } = TripStatus.Pending;

        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual User Customer { get; set; } // Điều hướng EF Core

        public int? DriverID { get; set; }
        [ForeignKey("DriverID")]
        public virtual User Driver { get; set; } // Điều hướng EF Core

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}