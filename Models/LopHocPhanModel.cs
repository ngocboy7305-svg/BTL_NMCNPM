using System;

namespace QuanLySinhVien.Models
{
    public class LopHocPhanModel
    {
        public string MaLopHocPhan { get; set; }
        public string MaMonHoc { get; set; }
        public string MaGiangVien { get; set; }
        public string HocKy { get; set; }
        public int NamHoc { get; set; }
        public string PhongHoc { get; set; }
        public int Thu { get; set; }
        public int TietBatDau { get; set; }
        public int TietKetThuc { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int SiSoToiDa { get; set; }
        
    }

}