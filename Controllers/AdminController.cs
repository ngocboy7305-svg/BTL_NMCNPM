using QuanLySinhVien.Helpers;
using QuanLySinhVien.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace QuanLySinhVien.Controllers
{
    public class AdminController : Controller
    {
        private void SetData()
        {
            ViewBag.Role = "ADMIN";
            ViewBag.UserName = Session["HoTen"] ?? "ADMIN";
            ViewBag.PortalTitle = "Admin";
        }

        public ActionResult ThongKe()
        {
            SetData();
            return View();
        }
        public ActionResult XepLichThi()
        {
            SetData();
            return View();
        }
        public ActionResult ThemGiangVien()
        {
            SetData();
            ViewBag.DanhSachGiangVien = GetDanhSachGiangVien();
            return View();
        }

        private List<GiangVienModel> GetDanhSachGiangVien()
        {
            List<GiangVienModel> ds = new List<GiangVienModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetDanhSachGiangVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new GiangVienModel
                    {
                        MaGiangVien = rd["pk_sMaGiangVien"].ToString(),
                        TenGiangVien = rd["sTenGiangVien"].ToString(),
                        NgaySinh = rd["dNgaySinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["dNgaySinh"]),
                        GioiTinh = rd["sGioiTinh"].ToString(),
                        Email = rd["sEmail"].ToString(),
                        SoDienThoai = rd["sSoDienThoai"].ToString(),
                        TrangThai = Convert.ToBoolean(rd["bTrangThai"])
                    });
                }
            }

            return ds;
        }

        public ActionResult ThemSinhVien()
        {
            SetData();
            ViewBag.DanhSachSinhVien = GetDanhSachSinhVien();
            return View();
        }
        [HttpPost]
        public ActionResult XoaSinhVien(string maSinhVien)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_XoaSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

                try
                {
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "Xóa sinh viên thành công";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi: " + ex.Message;
                }
            }

            return RedirectToAction("ThemSinhVien");
        }

        [HttpPost]
        public ActionResult ThemSinhVien(SinhVienModel model)
        {
            SetData();

            if (string.IsNullOrWhiteSpace(model.MaSinhVien) ||
        string.IsNullOrWhiteSpace(model.TenSinhVien) ||
        string.IsNullOrWhiteSpace(model.MatKhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                ViewBag.DanhSachSinhVien = GetDanhSachSinhVien();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                var loiDauTien = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                ViewBag.Error = string.IsNullOrEmpty(loiDauTien)
                    ? "Dữ liệu không hợp lệ!"
                    : loiDauTien;

                ViewBag.DanhSachSinhVien = GetDanhSachSinhVien();
                return View(model);
            }
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_ThemSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaSinhVien", model.MaSinhVien);
                cmd.Parameters.AddWithValue("@TenSinhVien", model.TenSinhVien);
                cmd.Parameters.AddWithValue("@NgaySinh", (object)model.NgaySinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", model.GioiTinh ?? "");
                cmd.Parameters.AddWithValue("@Lop", model.LopHanhChinh ?? "");
                cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
                cmd.Parameters.AddWithValue("@SDT", model.SoDienThoai ?? "");
                cmd.Parameters.AddWithValue("@MatKhau", model.MatKhau);

                try
                {
                    cmd.ExecuteNonQuery();
                    ViewBag.Message = "Thêm sinh viên thành công";
                    ModelState.Clear();
                    ViewBag.DanhSachSinhVien = GetDanhSachSinhVien();
                    return View(new SinhVienModel());
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        ViewBag.Error = "Mã sinh viên đã tồn tại!";
                    }
                    else if (ex.Number == 245 || ex.Number == 8114)
                    {
                        ViewBag.Error = "Dữ liệu không hợp lệ!";
                    }
                    else
                    {
                        ViewBag.Error = "Thêm sinh viên thất bại, vui lòng kiểm tra lại dữ liệu!";
                    }

                    ViewBag.DanhSachSinhVien = GetDanhSachSinhVien();
                    return View(model);
                }
                catch (Exception)
                {
                    ViewBag.Error = "Đã xảy ra lỗi, vui lòng thử lại!";
                    ViewBag.DanhSachSinhVien = GetDanhSachSinhVien();
                    return View(model);
                }
            }
        }

        public ActionResult ThemLopHocPhan()
        {
            SetData();
            ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
            return View();
        }
        [HttpPost]
        public ActionResult XoaLopHocPhan(string maLopHocPhan)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_XoaLopHocPhan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLHP", maLopHocPhan);

                try
                {
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "Xóa thành công";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi: " + ex.Message;
                }
            }

            return RedirectToAction("ThemLopHocPhan");
        }
        [HttpPost]
        public ActionResult ThemLopHocPhan(LopHocPhanModel model)
        {
            SetData();

            if (string.IsNullOrWhiteSpace(model.MaLopHocPhan) ||
        string.IsNullOrWhiteSpace(model.MaMonHoc) ||
        string.IsNullOrWhiteSpace(model.MaGiangVien) ||
        string.IsNullOrWhiteSpace(model.HocKy))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                var loiDauTien = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                ViewBag.Error = string.IsNullOrEmpty(loiDauTien)
                    ? "Dữ liệu không hợp lệ!"
                    : loiDauTien;

                ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                return View(model);
            }

            if (model.Thu < 2 || model.Thu > 8)
            {
                ViewBag.Error = "Thứ không hợp lệ!";
                ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                return View(model);
            }

            if (model.TietBatDau <= 0 || model.TietKetThuc <= 0 || model.SiSoToiDa <= 0)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ!";
                ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                return View(model);
            }

            if (model.TietBatDau > model.TietKetThuc)
            {
                ViewBag.Error = "Tiết bắt đầu phải nhỏ hơn hoặc bằng tiết kết thúc!";
                ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                return View(model);
            }

            if (model.NgayBatDau > model.NgayKetThuc)
            {
                ViewBag.Error = "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!";
                ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                return View(model);
            }

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_ThemLopHocPhan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLHP", model.MaLopHocPhan);
                cmd.Parameters.AddWithValue("@MaMon", model.MaMonHoc);
                cmd.Parameters.AddWithValue("@MaGV", model.MaGiangVien);
                cmd.Parameters.AddWithValue("@HocKy", model.HocKy);
                cmd.Parameters.AddWithValue("@NamHoc", model.NamHoc);
                cmd.Parameters.AddWithValue("@Phong", model.PhongHoc ?? "");
                cmd.Parameters.AddWithValue("@Thu", model.Thu);
                cmd.Parameters.AddWithValue("@TietBD", model.TietBatDau);
                cmd.Parameters.AddWithValue("@TietKT", model.TietKetThuc);
                cmd.Parameters.AddWithValue("@NgayBD", model.NgayBatDau);
                cmd.Parameters.AddWithValue("@NgayKT", model.NgayKetThuc);
                cmd.Parameters.AddWithValue("@SiSo", model.SiSoToiDa);

                try
                {
                    cmd.ExecuteNonQuery();
                    ViewBag.Message = "Thêm lớp học phần thành công";
                    ModelState.Clear();
                    ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                    return View(new LopHocPhanModel());
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        ViewBag.Error = "Mã lớp học phần đã tồn tại!";
                    }
                    else if (ex.Number == 245 || ex.Number == 8114)
                    {
                        ViewBag.Error = "Dữ liệu không hợp lệ!";
                    }
                    else
                    {
                        ViewBag.Error = "Thêm lớp học phần thất bại, vui lòng kiểm tra lại dữ liệu!";
                    }

                    ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                    return View(model);
                }
                catch (Exception)
                {
                    ViewBag.Error = "Đã xảy ra lỗi, vui lòng thử lại!";
                    ViewBag.DanhSachLopHocPhan = GetDanhSachLopHocPhan();
                    return View(model);
                }
            }
        }

        private List<SinhVienModel> GetDanhSachSinhVien()
        {
            List<SinhVienModel> ds = new List<SinhVienModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetDanhSachSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new SinhVienModel
                    {
                        MaSinhVien = rd["pk_sMaSinhVien"].ToString(),
                        TenSinhVien = rd["sTenSinhVien"].ToString(),
                        NgaySinh = rd["dNgaySinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["dNgaySinh"]),
                        GioiTinh = rd["sGioiTinh"].ToString(),
                        LopHanhChinh = rd["sLopHanhChinh"].ToString(),
                        Email = rd["sEmail"].ToString(),
                        SoDienThoai = rd["sSoDienThoai"].ToString()
                    });
                }
            }

            return ds;
        }

        private List<LopHocPhanModel> GetDanhSachLopHocPhan()
        {
            List<LopHocPhanModel> ds = new List<LopHocPhanModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetDanhSachLopHocPhan", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new LopHocPhanModel
                    {
                        MaLopHocPhan = rd["pk_sMaLopHocPhan"].ToString(),
                        MaMonHoc = rd["fk_pk_sMaMonHoc"].ToString(),
                        MaGiangVien = rd["fk_pk_sMaGiangVien"].ToString(),
                        HocKy = rd["sHocKy"].ToString(),
                        NamHoc = Convert.ToInt32(rd["iNamHoc"]),
                        PhongHoc = rd["sPhongHoc"].ToString(),
                        Thu = Convert.ToInt32(rd["iThu"]),
                        TietBatDau = Convert.ToInt32(rd["iTietBatDau"]),
                        TietKetThuc = Convert.ToInt32(rd["iTietKetThuc"]),
                        NgayBatDau = Convert.ToDateTime(rd["dNgayBatDau"]),
                        NgayKetThuc = Convert.ToDateTime(rd["dNgayKetThuc"]),
                        SiSoToiDa = Convert.ToInt32(rd["iSiSoToiDa"])
                    });
                }
            }

            return ds;
        }
        public ActionResult MonHoc()
        {
            SetData();
            ViewBag.DanhSachMonHoc = GetDanhSachMonHoc();
            return View();
        }

        private List<MonHocModel> GetDanhSachMonHoc()
        {
            List<MonHocModel> ds = new List<MonHocModel>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetDanhSachMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ds.Add(new MonHocModel
                    {
                        MaMonHoc = rd["pk_sMaMonHoc"].ToString(),
                        TenMonHoc = rd["sTenMonHoc"].ToString(),
                        SoTinChi = Convert.ToInt32(rd["iSoTinChi"]),
                        SoTietLyThuyet = Convert.ToInt32(rd["iSoTietLyThuyet"]),
                        SoTietThucHanh = Convert.ToInt32(rd["iSoTietThucHanh"]),
                        TrangThai = Convert.ToBoolean(rd["bTrangThai"])
                    });
                }
            }

            return ds;
        }
    }
}