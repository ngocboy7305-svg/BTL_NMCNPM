using QuanLySinhVien.Helpers;
using QuanLySinhVien.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace QuanLySinhVien.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Login", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenDangNhap", model.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", model.MatKhau);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    Session["UserName"] = rd["pk_sTenDangNhap"].ToString();
                    Session["HoTen"] = rd["HoTen"].ToString();
                    Session["MaVaiTro"] = rd["fk_pk_sMaVaiTro"].ToString();
                    Session["TenVaiTro"] = rd["sTenVaiTro"].ToString();

                    string maVaiTro = rd["fk_pk_sMaVaiTro"].ToString().ToUpper();

                    if (maVaiTro == "AD")
                        return RedirectToAction("ThongKe", "Admin");

                    if (maVaiTro == "SV")
                        return RedirectToAction("Index", "SinhVien");

                    if (maVaiTro == "GV")
                        return RedirectToAction("LopGiangDay", "GiangVien");
                }

                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}