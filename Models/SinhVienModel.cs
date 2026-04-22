using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class SinhVienModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã sinh viên!")]
        public string MaSinhVien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sinh viên!")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Tên chỉ được chứa chữ cái!")]
        public string TenSinhVien { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string GioiTinh { get; set; }

        public string LopHanhChinh { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Số điện thoại chỉ được nhập số!")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "SĐT phải từ 9-10 số!")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string MatKhau { get; set; }
    }
}