using System;

namespace QuanLySinhVien.Models
{
    public class DiemDanhViewModel
    {
        public string MaSinhVien { get; set; }
        public string TenSinhVien { get; set; }
        public DateTime? NgaySinh { get; set; }

        public bool Buoi1 { get; set; }
        public bool Buoi2 { get; set; }
        public bool Buoi3 { get; set; }
        public bool Buoi4 { get; set; }
        public bool Buoi5 { get; set; }
    }
}