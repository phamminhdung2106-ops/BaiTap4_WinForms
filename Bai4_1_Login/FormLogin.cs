namespace Bai4_1_Login;

public class FormLogin : Form
{
    readonly TextBox txtUsername = new() { Name = "txtUsername" };
    readonly TextBox txtPassword = new() { Name = "txtPassword", UseSystemPasswordChar = true };
    readonly CheckBox chkShowPassword = new() { Name = "chkShowPassword", Text = "Hiện mật khẩu", AutoSize = true };
    readonly Button btnLogin = new() { Name = "btnLogin", Text = "Đăng nhập" };
    readonly Button btnExit = new() { Name = "btnExit", Text = "Thoát" };
    readonly ErrorProvider errorProvider = new();

    public FormLogin()
    {
        Text = "Đăng nhập";
        ClientSize = new Size(340, 190);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var lblUser = new Label { Text = "Tên đăng nhập:", Left = 20, Top = 25, AutoSize = true };
        var lblPass = new Label { Text = "Mật khẩu:", Left = 20, Top = 65, AutoSize = true };
        txtUsername.SetBounds(125, 22, 190, 25);
        txtPassword.SetBounds(125, 62, 190, 25);
        chkShowPassword.Location = new Point(125, 95);
        btnLogin.SetBounds(125, 130, 90, 32);
        btnExit.SetBounds(225, 130, 90, 32);
        Controls.AddRange(new Control[] { lblUser, lblPass, txtUsername, txtPassword, chkShowPassword, btnLogin, btnExit });

        // Enter -> btnLogin, Esc -> btnExit
        AcceptButton = btnLogin;
        CancelButton = btnExit;
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        // Tick -> hiện (false); bỏ tick -> ẩn (true)
        chkShowPassword.CheckedChanged += (s, e) =>
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

        btnLogin.Click += BtnLogin_Click;
        btnExit.Click += (s, e) => Close();
        txtUsername.TextChanged += (s, e) => errorProvider.SetError(txtUsername, "");
        txtPassword.TextChanged += (s, e) => errorProvider.SetError(txtPassword, "");
    }

    bool ValidateInput()
    {
        bool ok = true;
        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        { errorProvider.SetError(txtUsername, "Tên đăng nhập không được để trống"); ok = false; }
        if (string.IsNullOrEmpty(txtPassword.Text))
        { errorProvider.SetError(txtPassword, "Mật khẩu không được để trống"); ok = false; }
        return ok;
    }

    void BtnLogin_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        try
        {
            if (Db.CheckLogin(txtUsername.Text.Trim(), txtPassword.Text))
                MessageBox.Show($"Đăng nhập thành công! Xin chào {txtUsername.Text.Trim()}.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
