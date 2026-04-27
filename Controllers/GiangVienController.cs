using QuanLySinhVien.Helpers;
using QuanLySinhVien.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace QuanLySinhVien.Controllers
{
    public class GiangVienController : Controller
    {
        private void SetData()
        {
            ViewBag.Role = "GV";
            ViewBag.UserName = Session["HoTen"] ?? "Giảng viên";
            ViewBag.PortalTitle = "Giảng viên";
        }

        public ActionResult LopGiangDay()
        {
            SetData();

            string maGiangVien = Session["UserName"]?.ToString();
            List<LichGiangDayViewModel> ds = new List<LichGiangDayViewModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetLichGiangDayTheoGiangVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaGV", maGiangVien);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new LichGiangDayViewModel
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

        private List<SelectListItem> GetDanhSachLopHocPhan()
        {
            List<SelectListItem> ds = new List<SelectListItem>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetDanhSachLopHocPhanNhapDiem", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new SelectListItem
                    {
                        Value = rd["pk_sMaLopHocPhan"].ToString(),
                        Text = rd["pk_sMaLopHocPhan"].ToString() + " - " + rd["sTenMonHoc"].ToString()
                    });
                }
            }

            return ds;
        }

        public ActionResult DiemDanh(string maLopHocPhan)
        {
            SetData();

            ViewBag.MaLopHocPhan = maLopHocPhan;
            ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();

            List<DiemDanhViewModel> ds = new List<DiemDanhViewModel>();

            if (string.IsNullOrEmpty(maLopHocPhan))
            {
                ViewBag.Error = "Vui lòng chọn lớp học phần";
                return View(ds);
            }

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetDanhSachSinhVienTheoLopHocPhan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLHP", maLopHocPhan);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new DiemDanhViewModel
                    {
                        MaSinhVien = rd["pk_sMaSinhVien"].ToString(),
                        TenSinhVien = rd["sTenSinhVien"].ToString(),
                        NgaySinh = rd["dNgaySinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["dNgaySinh"])
                    });
                }
            }

            return View(ds);
        }

        public ActionResult NhapDiem(string maLopHocPhan)
        {
            SetData();

            ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
            ViewBag.MaLopHocPhan = maLopHocPhan;

            List<DiemViewModel> ds = new List<DiemViewModel>();

            if (string.IsNullOrEmpty(maLopHocPhan))
            {
                ViewBag.Error = "Vui lòng chọn lớp học phần để nhập điểm";
                return View(ds);
            }

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetSinhVienVaDiemTrongLop", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLHP", maLopHocPhan);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new DiemViewModel
                    {
                        MaSinhVien = rd["pk_sMaSinhVien"].ToString(),
                        TenSinhVien = rd["sTenSinhVien"].ToString(),
                        MaLopHocPhan = maLopHocPhan,
                        DiemChuyenCan = rd["fDiemChuyenCan"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemChuyenCan"]),
                        DiemGiuaKy = rd["fDiemGiuaKy"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemGiuaKy"]),
                        DiemCuoiKy = rd["fDiemCuoiKy"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemCuoiKy"]),
                        DiemTongKet = rd["fDiemTongKet"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["fDiemTongKet"])
                    });
                }
            }

            return View(ds);
        }

        [HttpPost]
        public ActionResult LuuDiem(string maSinhVien, string maLopHocPhan,
    double? diemChuyenCan, double? diemGiuaKy, double? diemCuoiKy)
        {
            SetData();

            // VALIDATE
            if ((diemChuyenCan.HasValue && (diemChuyenCan < 0 || diemChuyenCan > 10)) ||
                (diemGiuaKy.HasValue && (diemGiuaKy < 0 || diemGiuaKy > 10)) ||
                (diemCuoiKy.HasValue && (diemCuoiKy < 0 || diemCuoiKy > 10)))
            {
                TempData["Error"] = "Điểm không hợp lệ (phải từ 0 đến 10)";
                return RedirectToAction("NhapDiem", new { maLopHocPhan = maLopHocPhan });
            }

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_LuuDiem", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaSV", maSinhVien);
                cmd.Parameters.AddWithValue("@MaLHP", maLopHocPhan);
                cmd.Parameters.AddWithValue("@CC", (object)diemChuyenCan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GK", (object)diemGiuaKy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CK", (object)diemCuoiKy ?? DBNull.Value);

                try
                {
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "Lưu điểm thành công";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi: " + ex.Message;
                }
            }

            return RedirectToAction("NhapDiem", new { maLopHocPhan = maLopHocPhan });
        }
    }
}