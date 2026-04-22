using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class MonHocModel
    {
        [Required]
        public string MaMonHoc { get; set; }

        [Required]
        public string TenMonHoc { get; set; }

        [Required]
        public int SoTinChi { get; set; }

        public int SoTietLyThuyet { get; set; }

        public int SoTietThucHanh { get; set; }

        public bool TrangThai { get; set; }
    }
}