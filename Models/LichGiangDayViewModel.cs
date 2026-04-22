using System;

namespace QuanLySinhVien.Models
{
    public class LichGiangDayViewModel
    {
        public string MaLopHocPhan { get; set; }
        public string TenMonHoc { get; set; }
        public string TenGiangVien { get; set; }
        public string HocKy { get; set; }
        public int NamHoc { get; set; }
        public int Thu { get; set; }
        public int TietBatDau { get; set; }
        public int TietKetThuc { get; set; }
        public string PhongHoc { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
    }
}