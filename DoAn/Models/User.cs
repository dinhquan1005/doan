using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Lưu ý: Thực tế nên mã hóa Hash (BCrypt)

        public string Role { get; set; } = AppRoles.Customer; // Mặc định là Khách hàng
    }
}