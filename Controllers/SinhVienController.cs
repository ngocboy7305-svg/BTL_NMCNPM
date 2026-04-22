using QuanLySinhVien.Models;
using QuanLySinhVien.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;

namespace QuanLySinhVien.Controllers
{
    public class SinhVienController : Controller
    {
        private void SetData()
        {
            ViewBag.Role = "SV";
            ViewBag.UserName = Session["HoTen"] ?? "Sinh viên";
            ViewBag.PortalTitle = "Cổng sinh viên";
        }

        public ActionResult Index()
        {
            SetData();
            return View();
        }

        public ActionResult LichThi()
        {
            SetData();
            return View();
        }
        public ActionResult DangKyTin()
        {
            SetData();
            List<DangKyHocPhanViewModel> ds = new List<DangKyHocPhanViewModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetLopHocPhanConLai", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new DangKyHocPhanViewModel
                    {
                        MaLopHocPhan = rd["pk_sMaLopHocPhan"].ToString(),
                        TenMonHoc = rd["sTenMonHoc"].ToString(),
                        TenGiangVien = rd["sTenGiangVien"].ToString(),
                        HocKy = rd["sHocKy"].ToString(),
                        NamHoc = Convert.ToInt32(rd["iNamHoc"]),
                        Thu = Convert.ToInt32(rd["iThu"]),
                        TietBatDau = Convert.ToInt32(rd["iTietBatDau"]),
                        TietKetThuc = Convert.ToInt32(rd["iTietKetThuc"]),
                        PhongHoc = rd["sPhongHoc"].ToString(),
                        SiSoConLai = Convert.ToInt32(rd["SiSoConLai"])
                    });
                }
            }

            return View(ds);
        }

        [HttpPost]
        public ActionResult DangKyHocPhan(string maLopHocPhan)
        {
            SetData();
            string maSinhVien = Session["UserName"]?.ToString();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_DangKyHocPhan_CheckSiSo", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaSV", maSinhVien);
                cmd.Parameters.AddWithValue("@MaLHP", maLopHocPhan);

                try
                {
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "Đăng ký học phần thành công";
                }
                catch
                {
                    TempData["Error"] = "Đã đăng ký học phần";
                }
            }

            return RedirectToAction("DangKyTin");
        }

        public ActionResult DiemThi()
        {
            SetData();
            string maSinhVien = Session["UserName"]?.ToString();
            List<DiemViewModel> ds = new List<DiemViewModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetDiemSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new DiemViewModel
                    {
                        MaSinhVien = rd["fk_pk_sMaSinhVien"].ToString(),
                        MaLopHocPhan = rd["fk_pk_sMaLopHocPhan"].ToString(),
                        TenMonHoc = rd["sTenMonHoc"].ToString(),
                        DiemChuyenCan = rd["fDiemChuyenCan"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemChuyenCan"]),
                        DiemGiuaKy = rd["fDiemGiuaKy"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemGiuaKy"]),
                        DiemCuoiKy = rd["fDiemCuoiKy"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemCuoiKy"]),
                        DiemTongKet = rd["fDiemTongKet"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemTongKet"])
                    });
                }
            }

            return View(ds);
        }

        public ActionResult LichHocKy()
        {
            SetData();
            string maSinhVien = Session["UserName"]?.ToString();
            List<LichHocViewModel> ds = new List<LichHocViewModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetLichHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new LichHocViewModel
                    {
                        MaLopHocPhan = rd["pk_sMaLopHocPhan"].ToString(),
                        TenMonHoc = rd["sTenMonHoc"].ToString(),
                        TenGiangVien = rd["sTenGiangVien"].ToString(),
                        HocKy = rd["sHocKy"].ToString(),
                        NamHoc = Convert.ToInt32(rd["iNamHoc"]),
                        Thu = Convert.ToInt32(rd["iThu"]),
                        TietBatDau = Convert.ToInt32(rd["iTietBatDau"]),
                        TietKetThuc = Convert.ToInt32(rd["iTietKetThuc"]),
                        PhongHoc = rd["sPhongHoc"].ToString(),
                        NgayBatDau = Convert.ToDateTime(rd["dNgayBatDau"]),
                        NgayKetThuc = Convert.ToDateTime(rd["dNgayKetThuc"])
                    });
                }
            }

            return View(ds);
        }
        public ActionResult LichHocTheoTuan(int? tuan = 1, int? nam = null)
        {
            SetData();
            string maSinhVien = Session["UserName"]?.ToString();
            int namHoc = nam ?? DateTime.Now.Year;

            LichHocTuanViewModel lichHocTuan = new LichHocTuanViewModel
            {
                TuanSo = tuan ?? 1
            };

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmdTuan = new SqlCommand("sp_GetDanhSachTuanTheoNam", conn);
                cmdTuan.CommandType = CommandType.StoredProcedure;
                cmdTuan.Parameters.AddWithValue("@Nam", namHoc);

                SqlDataReader rdTuan = cmdTuan.ExecuteReader();
                while (rdTuan.Read())
                {
                    lichHocTuan.DanhSachTuan.Add(new DanhSachTuanViewModel
                    {
                        TuanThu = Convert.ToInt32(rdTuan["WeekNumber"]),
                        TuNgay = Convert.ToDateTime(rdTuan["StartDate"]),
                        DenNgay = Convert.ToDateTime(rdTuan["EndDate"])
                    });
                }
                rdTuan.Close();

                var tuanDangChon = lichHocTuan.DanhSachTuan
                    .FirstOrDefault(x => x.TuanThu == lichHocTuan.TuanSo);

                if (tuanDangChon != null)
                {
                    lichHocTuan.TuNgay = tuanDangChon.TuNgay;
                    lichHocTuan.DenNgay = tuanDangChon.DenNgay;
                }

                SqlCommand cmdLichHoc = new SqlCommand("sp_GetLichHocTheoTuan", conn);
                cmdLichHoc.CommandType = CommandType.StoredProcedure;
                cmdLichHoc.Parameters.AddWithValue("@MaSV", maSinhVien);
                cmdLichHoc.Parameters.AddWithValue("@Tuan", lichHocTuan.TuanSo);
                cmdLichHoc.Parameters.AddWithValue("@Nam", namHoc);

                SqlDataReader rd = cmdLichHoc.ExecuteReader();
                while (rd.Read())
                {
                    int tietBatDau = Convert.ToInt32(rd["iTietBatDau"]);

                    lichHocTuan.LichHocItems.Add(new LichHocItemViewModel
                    {
                        TenMonHoc = rd["sTenMonHoc"].ToString(),
                        MaLopHocPhan = rd["pk_sMaLopHocPhan"].ToString(),
                        GiangVien = rd["sTenGiangVien"].ToString(),
                        PhongHoc = rd["sPhongHoc"].ToString(),
                        HinhThucHoc = "",
                        Thu = Convert.ToInt32(rd["iThu"]),
                        CaHoc = tietBatDau <= 6 ? "Sáng" : "Chiều",
                        TietBatDau = tietBatDau,
                        TietKetThuc = Convert.ToInt32(rd["iTietKetThuc"])
                    });
                }
                rd.Close();
            }

            ViewBag.Nam = namHoc;
            return View(lichHocTuan);
        }
        public ActionResult SoYeuLyLich()
        {
            SetData();

            string maSV = Session["UserName"]?.ToString();

            if (string.IsNullOrEmpty(maSV))
                return RedirectToAction("Login", "Account");

            SinhVienModel model = new SinhVienModel();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetSinhVienById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaSV", maSV);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    model.MaSinhVien = rd["pk_sMaSinhVien"].ToString();
                    model.TenSinhVien = rd["sTenSinhVien"].ToString();
                    model.NgaySinh = rd["dNgaySinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["dNgaySinh"]);
                    model.GioiTinh = rd["sGioiTinh"].ToString();
                    model.LopHanhChinh = rd["sLopHanhChinh"].ToString();
                    model.Email = rd["sEmail"].ToString();
                    model.SoDienThoai = rd["sSoDienThoai"].ToString();
                }
            }

            return View(model);
        }
    }
}