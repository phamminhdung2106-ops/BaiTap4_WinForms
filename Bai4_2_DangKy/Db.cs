using System.Data;
using Microsoft.Data.SqlClient;

namespace Bai4_2_DangKy;

static class Db
{
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
        Run(c, @"IF OBJECT_ID('dbo.Courses') IS NULL
                 CREATE TABLE dbo.Courses(CourseId INT IDENTITY(1,1) PRIMARY KEY, CourseName NVARCHAR(100) NOT NULL)");
        Run(c, @"IF OBJECT_ID('dbo.Registrations') IS NULL
                 CREATE TABLE dbo.Registrations(
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    FullName NVARCHAR(100) NOT NULL,
                    Gender NVARCHAR(10) NOT NULL,
                    Phone NVARCHAR(20) NOT NULL,
                    BirthDate DATE NOT NULL,
                    CourseId INT NOT NULL REFERENCES dbo.Courses(CourseId),
                    Interests NVARCHAR(200) NULL,
                    RegisteredAt DATETIME NOT NULL DEFAULT GETDATE())");
        Run(c, @"IF NOT EXISTS(SELECT 1 FROM dbo.Courses)
                 INSERT INTO dbo.Courses(CourseName) VALUES
                 (N'Lập trình C#'),(N'Lập trình Web'),(N'Cơ sở dữ liệu'),(N'Lập trình Java'),(N'Python cơ bản')");
    }

    public static DataTable GetCourses()
    {
        using var c = new SqlConnection(ConnStr);
        using var da = new SqlDataAdapter("SELECT CourseId, CourseName FROM dbo.Courses ORDER BY CourseName", c);
        var dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public static int InsertRegistration(string fullName, string gender, string phone,
                                         DateTime birth, int courseId, string interests)
    {
        using var c = new SqlConnection(ConnStr);
        c.Open();
        using var cmd = new SqlCommand(@"INSERT INTO dbo.Registrations(FullName,Gender,Phone,BirthDate,CourseId,Interests)
                                         OUTPUT INSERTED.Id
                                         VALUES(@n,@g,@p,@b,@c,@i)", c);
        cmd.Parameters.AddWithValue("@n", fullName);
        cmd.Parameters.AddWithValue("@g", gender);
        cmd.Parameters.AddWithValue("@p", phone);
        cmd.Parameters.Add("@b", SqlDbType.Date).Value = birth.Date;
        cmd.Parameters.AddWithValue("@c", courseId);
        cmd.Parameters.AddWithValue("@i", (object)interests ?? DBNull.Value);
        return (int)cmd.ExecuteScalar();
    }

    public static DataTable GetRegistrations()
    {
        using var c = new SqlConnection(ConnStr);
        using var da = new SqlDataAdapter(@"SELECT r.Id AS [Mã], r.FullName AS [Họ tên], r.Gender AS [Giới tính],
                                                   r.Phone AS [SĐT], r.BirthDate AS [Ngày sinh],
                                                   c.CourseName AS [Khóa học], r.Interests AS [Sở thích],
                                                   r.RegisteredAt AS [Ngày đăng ký]
                                            FROM dbo.Registrations r JOIN dbo.Courses c ON c.CourseId = r.CourseId
                                            ORDER BY r.Id DESC", c);
        var dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    static void Run(SqlConnection c, string sql)
    {
        using var cmd = new SqlCommand(sql, c);
        cmd.ExecuteNonQuery();
    }
}
