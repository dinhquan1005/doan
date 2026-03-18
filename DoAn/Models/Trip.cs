using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
    public class Trip
    {
        [Key]
        public int TripID { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } // "Pending", "Accepted", "Completed"

        // ID của khách hàng (Quân)
        public int CustomerID { get; set; }

        // ID của tài xế (Khôi) - Để null nếu chưa có ai nhận
        public int? DriverID { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}