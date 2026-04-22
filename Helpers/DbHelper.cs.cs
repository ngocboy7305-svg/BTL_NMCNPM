using System.Configuration;
using System.Data.SqlClient;

namespace QuanLySinhVien.Helpers
{
    public static class DbHelper
    {
        public static SqlConnection GetConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLSVConnection"].ConnectionString;
            return new SqlConnection(connStr);
        }
    }
}