# Bài tập 4 – WinForms + CSDL (SQL Server)

| Bài | Nội dung |
|-----|----------|
| 4.1 | Login Form: ErrorProvider, ẩn/hiện mật khẩu, Enter/Esc, **kiểm tra tài khoản trong CSDL** (mật khẩu băm PBKDF2) |
| 4.2 | Form đăng ký: MaskedTextBox, DateTimePicker, ComboBox binding, RadioButton, CheckBox, **lưu vào CSDL** + hiển thị danh sách bằng DataGridView |

## Yêu cầu
- .NET 8 SDK + Visual Studio 2022 (Windows)
- SQL Server LocalDB (đi kèm Visual Studio) hoặc SQL Server Express

## Chạy
1. Mở thư mục, chạy `dotnet run --project Bai4_1_Login` hoặc `Bai4_2_DangKy` (hoặc mở bằng Visual Studio).
2. Lần đầu ứng dụng tự tạo database `BaiTap4`, các bảng và dữ liệu mẫu (có thể chạy thủ công `Database/BaiTap4.sql`).
3. Tài khoản mẫu bài 4.1: **admin / 123456**
4. Dùng SQL Server khác: sửa hằng `Server` trong `Db.cs`.
