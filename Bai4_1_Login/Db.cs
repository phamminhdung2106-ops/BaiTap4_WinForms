using Microsoft.Data.SqlClient;

namespace Bai4_1_Login;

static class Db
{
    // Đổi Server nếu dùng SQL Server khác, ví dụ ".\\SQLEXPRESS"
    const string Server = "(localdb)\\MSSQLLocalDB";
    const string DbName = "BaiTap4";

    public static string ConnStr =>
        $"Server={Server};Database={DbName};Trusted_Connection=True;TrustServerCertificate=True;";

    public static void Initialize()
    {
        using (var master = new SqlConnection($"Server={Server};Database=master;Trusted_Connection=True;TrustServerCertificate=True;"))
        {
            master.Open();
            Run(master, $"IF DB_ID('{DbName}') IS NULL CREATE DATABASE {DbName}");
        }
        using var c = new SqlConnection(ConnStr);
        c.Open();
        Run(c, @"IF OBJECT_ID('dbo.Users') IS NULL
                 CREATE TABLE dbo.Users(Id INT IDENTITY(1,1) PRIMARY KEY,
                                        Username NVARCHAR(50) NOT NULL UNIQUE,
                                        PasswordHash NVARCHAR(200) NOT NULL)");
        using var chk = new SqlCommand("SELECT COUNT(*) FROM dbo.Users", c);
        if ((int)chk.ExecuteScalar() == 0)
        {
            using var ins = new SqlCommand("INSERT INTO dbo.Users(Username,PasswordHash) VALUES(@u,@p)", c);
            ins.Parameters.AddWithValue("@u", "admin");
            ins.Parameters.AddWithValue("@p", PasswordHasher.Hash("123456"));
            ins.ExecuteNonQuery();
        }
    }

    public static bool CheckLogin(string username, string password)
    {
        using var c = new SqlConnection(ConnStr);
        c.Open();
        using var cmd = new SqlCommand("SELECT PasswordHash FROM dbo.Users WHERE Username=@u", c);
        cmd.Parameters.AddWithValue("@u", username);
        var stored = cmd.ExecuteScalar() as string;
        return stored != null && PasswordHasher.Verify(password, stored);
    }

    static void Run(SqlConnection c, string sql)
    {
        using var cmd = new SqlCommand(sql, c);
        cmd.ExecuteNonQuery();
    }
}
