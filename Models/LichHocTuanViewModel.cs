using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models
{
    public class DanhSachTuanViewModel
    {
        public int TuanThu { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
    }

    public class LichHocItemViewModel
    {
        public string TenMonHoc { get; set; }
        public string MaLopHocPhan { get; set; }
        public string GiangVien { get; set; }
        public string PhongHoc { get; set; }
        public string HinhThucHoc { get; set; }
        public int Thu { get; set; }
        public string CaHoc { get; set; }
        public int TietBatDau { get; set; }
        public int TietKetThuc { get; set; }
    }

    public class LichHocTuanViewModel
    {
        public int TuanSo { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }

        public List<DanhSachTuanViewModel> DanhSachTuan { get; set; }
        public List<LichHocItemViewModel> LichHocItems { get; set; }

        public LichHocTuanViewModel()
        {
            DanhSachTuan = new List<DanhSachTuanViewModel>();
            LichHocItems = new List<LichHocItemViewModel>();
        }
    }
}