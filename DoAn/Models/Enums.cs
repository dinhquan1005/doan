namespace DoAn.Models
{
    public enum TripStatus
    {
        Pending,   // Đang chờ
        Accepted,  // Đã nhận
        Completed, // Hoàn thành
        Cancelled  // Đã hủy
    }

    public static class AppRoles
    {
        public const string Customer = "Customer";
        public const string Driver = "Driver";
    }
}