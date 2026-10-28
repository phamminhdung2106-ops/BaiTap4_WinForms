using System.Data;

namespace Bai4_2_DangKy;

public class FormDangKy : Form
{
    readonly TextBox txtFullName = new() { Name = "txtFullName" };
    readonly RadioButton rdoMale = new() { Name = "rdoMale", Text = "Nam", Checked = true, AutoSize = true };
    readonly RadioButton rdoFemale = new() { Name = "rdoFemale", Text = "Nữ", AutoSize = true };
    readonly MaskedTextBox mtxtPhone = new() { Name = "mtxtPhone", Mask = "(000) 000-0000" };
    readonly DateTimePicker dtpBirthDate = new() { Name = "dtpBirthDate", Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
    readonly ComboBox cboCourse = new() { Name = "cboCourse", DropDownStyle = ComboBoxStyle.DropDownList };
    readonly CheckBox chkReading = new() { Name = "chkReading", Text = "Đọc sách", AutoSize = true };
    readonly CheckBox chkMusic = new() { Name = "chkMusic", Text = "Âm nhạc", AutoSize = true };
    readonly CheckBox chkSport = new() { Name = "chkSport", Text = "Thể thao", AutoSize = true };
    readonly Button btnRegister = new() { Name = "btnRegister", Text = "Đăng ký" };
    readonly DataGridView dgvRegistrations = new() { Name = "dgvRegistrations", ReadOnly = true,
        AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect };
    readonly ErrorProvider errorProvider = new();

    public FormDangKy()
    {
        Text = "Đăng ký khóa học";
        ClientSize = new Size(640, 560);
        StartPosition = FormStartPosition.CenterScreen;

        int L = 20, X = 130;
        Controls.Add(new Label { Text = "Họ và tên:", Left = L, Top = 25, AutoSize = true });
        txtFullName.SetBounds(X, 22, 250, 25);
        Controls.Add(new Label { Text = "Giới tính:", Left = L, Top = 62, AutoSize = true });
        rdoMale.Location = new Point(X, 60); rdoFemale.Location = new Point(X + 70, 60);
        Controls.Add(new Label { Text = "Số điện thoại:", Left = L, Top = 97, AutoSize = true });
        mtxtPhone.SetBounds(X, 94, 150, 25);
        Controls.Add(new Label { Text = "Ngày sinh:", Left = L, Top = 132, AutoSize = true });
        dtpBirthDate.SetBounds(X, 129, 150, 25);
        dtpBirthDate.MaxDate = DateTime.Today;
        Controls.Add(new Label { Text = "Khóa học:", Left = L, Top = 167, AutoSize = true });
        cboCourse.SetBounds(X, 164, 250, 25);
        Controls.Add(new Label { Text = "Sở thích:", Left = L, Top = 205, AutoSize = true });
        chkReading.Location = new Point(X, 203); chkMusic.Location = new Point(X + 90, 203); chkSport.Location = new Point(X + 180, 203);
        btnRegister.SetBounds(X, 240, 120, 34);
        dgvRegistrations.SetBounds(L, 295, 600, 245);
        dgvRegistrations.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        Controls.AddRange(new Control[] { txtFullName, rdoMale, rdoFemale, mtxtPhone, dtpBirthDate, cboCourse,
                                          chkReading, chkMusic, chkSport, btnRegister, dgvRegistrations });
        AcceptButton = btnRegister;
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        Load += (s, e) => { LoadCourses(); LoadGrid(); };
        btnRegister.Click += BtnRegister_Click;
    }

    void LoadCourses()
    {
        // Binding data: hiển thị CourseName, giá trị là CourseId
        cboCourse.DataSource = Db.GetCourses();
        cboCourse.DisplayMember = "CourseName";
        cboCourse.ValueMember = "CourseId";
    }

    void LoadGrid() => dgvRegistrations.DataSource = Db.GetRegistrations();

    bool ValidateInput()
    {
        errorProvider.Clear();
        bool ok = true;
        if (string.IsNullOrWhiteSpace(txtFullName.Text))
        { errorProvider.SetError(txtFullName, "Vui lòng nhập họ tên"); ok = false; }
        if (!mtxtPhone.MaskCompleted)
        { errorProvider.SetError(mtxtPhone, "Vui lòng nhập đủ số điện thoại"); ok = false; }
        if (cboCourse.SelectedValue == null)
        { errorProvider.SetError(cboCourse, "Vui lòng chọn khóa học"); ok = false; }
        return ok;
    }

    void BtnRegister_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;

        string gender = rdoMale.Checked ? "Nam" : "Nữ";
        var interests = new List<string>();
        if (chkReading.Checked) interests.Add(chkReading.Text);
        if (chkMusic.Checked) interests.Add(chkMusic.Text);
        if (chkSport.Checked) interests.Add(chkSport.Text);
        string interestText = interests.Count > 0 ? string.Join(", ", interests) : null;

        try
        {
            int id = Db.InsertRegistration(txtFullName.Text.Trim(), gender, mtxtPhone.Text,
                dtpBirthDate.Value, Convert.ToInt32(cboCourse.SelectedValue), interestText);

            MessageBox.Show(
                $"ĐĂNG KÝ THÀNH CÔNG (Mã #{id})\n\n" +
                $"Họ tên: {txtFullName.Text.Trim()}\n" +
                $"Giới tính: {gender}\n" +
                $"SĐT: {mtxtPhone.Text}\n" +
                $"Ngày sinh: {dtpBirthDate.Value:dd/MM/yyyy}\n" +
                $"Khóa học: {cboCourse.Text} (ID = {cboCourse.SelectedValue})\n" +
                $"Sở thích: {interestText ?? "(không có)"}",
                "Thông tin đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadGrid();
            ResetForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    void ResetForm()
    {
        txtFullName.Clear(); mtxtPhone.Clear(); rdoMale.Checked = true;
        chkReading.Checked = chkMusic.Checked = chkSport.Checked = false;
        dtpBirthDate.Value = DateTime.Today; cboCourse.SelectedIndex = 0;
        txtFullName.Focus();
    }
}
