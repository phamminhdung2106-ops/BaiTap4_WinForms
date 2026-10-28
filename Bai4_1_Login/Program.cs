namespace Bai4_1_Login;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        try { Db.Initialize(); }
        catch (Exception ex)
        {
            MessageBox.Show("Không kết nối được CSDL:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        Application.Run(new FormLogin());
    }
}
