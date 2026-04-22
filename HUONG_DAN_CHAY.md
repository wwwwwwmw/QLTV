# 📚 Hướng Dẫn Cài Đặt & Chạy Dự Án Quản Lý Thư Viện (LibraryManagement)

## Mục Lục
- [1. Yêu Cầu Hệ Thống](#1-yêu-cầu-hệ-thống)
- [2. Cài Đặt Công Cụ](#2-cài-đặt-công-cụ)
- [3. Cấu Hình Database](#3-cấu-hình-database)
- [4. Chạy Trên Visual Studio](#4-chạy-trên-visual-studio)
- [5. Chạy Bằng Terminal (CLI)](#5-chạy-bằng-terminal-cli)
- [6. Tài Khoản Mặc Định](#6-tài-khoản-mặc-định)
- [7. Dữ Liệu Mẫu](#7-dữ-liệu-mẫu)
- [8. Cấu Trúc Dự Án](#8-cấu-trúc-dự-án)
- [9. Các Tính Năng Chính](#9-các-tính-năng-chính)
- [10. API Keys & Cấu Hình Bên Ngoài](#10-api-keys--cấu-hình-bên-ngoài)
- [11. Xử Lý Lỗi Thường Gặp](#11-xử-lý-lỗi-thường-gặp)
- [12. Lỗi Đã Phát Hiện & Sửa](#12-lỗi-đã-phát-hiện--sửa)

---

## 1. Yêu Cầu Hệ Thống

| Thành phần | Yêu cầu tối thiểu |
|---|---|
| **Hệ điều hành** | Windows 10 trở lên (x64) |
| **RAM** | 4 GB (khuyến nghị 8 GB) |
| **.NET SDK** | **.NET 8.0 SDK** trở lên |
| **Database** | **SQL Server** (LocalDB, Express, hoặc bản đầy đủ) |
| **IDE** | Visual Studio 2022 (v17.8+) |

---

## 2. Cài Đặt Công Cụ

### 2.1. Visual Studio 2022

Phải cài Visual Studio 2022 phiên bản 17.8 trở lên để hỗ trợ .NET 8.

1. Tải từ: https://visualstudio.microsoft.com/vs/
2. Khi cài đặt, chọn workload:
   - ✅ **.NET desktop development** (bắt buộc)
   - ✅ **Data storage and processing** (cho SQL Server tools)
3. Trong tab **Individual components**, đảm bảo chọn:
   - ✅ `.NET 8.0 Runtime`
   - ✅ `SQL Server Express LocalDB` (nếu muốn dùng LocalDB)

### 2.2. .NET 8.0 SDK (nếu chạy bằng Terminal)

Nếu chỉ chạy bằng lệnh, tải riêng:
- https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- Chọn **SDK** (không phải Runtime)

Kiểm tra sau khi cài:
```powershell
dotnet --version
# Kết quả phải >= 8.0.x
```

### 2.3. SQL Server

Chọn **một trong các phiên bản** sau:

| Phiên bản | Ghi chú |
|---|---|
| **SQL Server LocalDB** | Đi kèm Visual Studio, nhẹ nhất |
| **SQL Server Express** | Miễn phí, tải tại https://www.microsoft.com/en-us/sql-server/sql-server-downloads |
| **SQL Server Developer** | Miễn phí cho dev, đầy đủ tính năng |

> Nếu đã cài Visual Studio với workload .NET desktop development, LocalDB đã có sẵn.

### 2.4. SQL Server Management Studio (SSMS) — Tùy chọn

Để quản lý database bằng giao diện:
- Tải: https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms

---

## 3. Cấu Hình Database

### 3.1. Tự động khởi tạo

Ứng dụng **tự động tạo database schema và dữ liệu mẫu** khi chạy lần đầu thông qua file `Database/DatabaseInit.sql` (được nhúng vào assembly). Bạn **không cần chạy script SQL thủ công**.

### 3.2. Tạo Database trống trước (bắt buộc)

Bạn **phải tạo database trống** tên `LibraryManagement` trước khi chạy ứng dụng.

#### Cách 1: Dùng SSMS
1. Mở SSMS → Connect đến SQL Server của bạn
2. Click phải **Databases** → **New Database...**
3. Nhập tên: `LibraryManagement` → OK

#### Cách 2: Dùng lệnh SQL
```sql
CREATE DATABASE LibraryManagement;
GO
```

#### Cách 3: Dùng lệnh sqlcmd trong Terminal
```powershell
# Với SQL Server Express
sqlcmd -S .\SQLEXPRESS -Q "CREATE DATABASE LibraryManagement"

# Với LocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE LibraryManagement"
```

### 3.3. Cấu hình Connection String

Mở file `LibraryManagement/App.config` và sửa connection string phù hợp:

```xml
<connectionStrings>
    <!-- === CHỌN MỘT TRONG CÁC CẤU HÌNH SAU === -->

    <!-- 1. SQL Server LocalDB (mặc định khi cài VS) -->
    <add name="LibraryDB"
         connectionString="Server=(localdb)\MSSQLLocalDB;Database=LibraryManagement;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
         providerName="Microsoft.Data.SqlClient" />

    <!-- 2. SQL Server Express -->
    <!--
    <add name="LibraryDB"
         connectionString="Server=.\SQLEXPRESS;Database=LibraryManagement;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
         providerName="Microsoft.Data.SqlClient" />
    -->

    <!-- 3. SQL Server với tên máy cụ thể (thay YOUR-PC-NAME) -->
    <!--
    <add name="LibraryDB"
         connectionString="Server=YOUR-PC-NAME;Database=LibraryManagement;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
         providerName="Microsoft.Data.SqlClient" />
    -->

    <!-- 4. SQL Server với user/password -->
    <!--
    <add name="LibraryDB"
         connectionString="Server=YOUR-SERVER;Database=LibraryManagement;User ID=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True"
         providerName="Microsoft.Data.SqlClient" />
    -->
</connectionStrings>
```

> ⚠️ File `App.config` hiện tại đang trỏ đến `Server=DESKTP-ENDTJTR` — đây là tên máy cụ thể. Bạn phải sửa lại cho đúng với máy của bạn hoặc dùng LocalDB.

> 💡 Nếu không muốn sửa file, ứng dụng sẽ hiện dialog FormConnectionConfig để bạn nhập thông tin kết nối khi không thể kết nối DB.

---

## 4. Chạy Trên Visual Studio

### Bước 1: Mở Solution
- Mở file `LibraryManagement.sln` bằng Visual Studio 2022
- Hoặc: **File** → **Open** → **Project/Solution** → chọn file `.sln`

### Bước 2: Restore NuGet Packages
- Visual Studio tự động restore khi mở solution
- Nếu chưa, click phải Solution → **Restore NuGet Packages**

### Bước 3: Kiểm tra Target Framework
- Click phải project **LibraryManagement** → **Properties**
- Đảm bảo **Target framework** = `net8.0-windows`

### Bước 4: Build & Run
- Nhấn **F5** (Debug) hoặc **Ctrl+F5** (Run without debug)
- Hoặc: Menu **Debug** → **Start Debugging**
- Configuration: **Debug | Any CPU**

### Bước 5: Kết nối Database lần đầu
- Nếu connection string đúng → ứng dụng tự tạo schema + dữ liệu mẫu
- Nếu sai → hiện **FormConnectionConfig** để nhập lại

### Bước 6: Sử dụng
- Ứng dụng mở **trang công khai** (FormPublic) trước — không yêu cầu đăng nhập
- Người dùng có thể **xem sách, tìm kiếm** mà không cần đăng nhập
- Khi cần **mượn sách, quản lý** → click **Đăng nhập** → dùng tài khoản bên dưới

---

## 5. Chạy Bằng Terminal (CLI)

### 5.1. Restore packages
```powershell
cd d:\Documents\QLThuVien\QLThuVien
dotnet restore
```

### 5.2. Build
```powershell
dotnet build --configuration Debug
```

### 5.3. Run
```powershell
dotnet run --project LibraryManagement
```

### 5.4. Build Release
```powershell
dotnet build --configuration Release
```

### 5.5. Publish (tạo bản chạy độc lập)
```powershell
# Bản portable (cần .NET Runtime trên máy đích)
dotnet publish --configuration Release --output ./publish

# Bản self-contained (không cần cài .NET trên máy đích)
dotnet publish --configuration Release --runtime win-x64 --self-contained true --output ./publish-self
```

---

## 6. Tài Khoản Mặc Định

Các tài khoản này được tạo tự động khi chạy lần đầu (từ file `DatabaseInit.sql`).

| # | Username | Password | Vai trò | Họ tên | Email |
|---|---|---|---|---|---|
| 1 | `admin` | `admin` | **Admin** | Lê Quang Minh | admin@library.vn |
| 2 | `manager` | `123456` | **Manager** | Nguyễn Hải Nam | manager@library.vn |
| 3 | `staff1` | `123456` | **Staff** | Trần Thu Hằng | staff1@library.vn |

### Phân quyền vai trò

| Quyền | Admin | Manager | Staff |
|---|:---:|:---:|:---:|
| Quản lý sách (CRUD) | ✅ | ✅ | ✅ |
| Mượn/Trả sách | ✅ | ✅ | ✅ |
| Quản lý độc giả | ✅ | ✅ | ✅ |
| Quản lý danh mục | ✅ | ✅ | ❌ |
| Báo cáo thống kê | ✅ | ✅ | ❌ |
| Quản lý người dùng | ✅ | ❌ | ❌ |
| Cài đặt hệ thống | ✅ | ❌ | ❌ |

> Mật khẩu lưu dạng SHA-256 hash (legacy) hoặc BCrypt. Khi đăng nhập lần đầu bằng tài khoản cũ (SHA-256), hệ thống tự động nâng cấp mật khẩu sang BCrypt.

---

## 7. Dữ Liệu Mẫu

### Sách mẫu (20 cuốn)

Ứng dụng tự thêm 20 đầu sách từ nhiều thể loại:

| Thể loại | Ví dụ sách |
|---|---|
| Văn học | Cho tôi xin một vé đi tuổi thơ, Chí Phèo, Nhà giả kim, 1984 |
| Thiếu nhi | Dế Mèn phiêu lưu ký, Le Petit Prince |
| Tâm lý - Kỹ năng | Đắc nhân tâm, Atomic Habits |
| Kinh tế | Cha giàu cha nghèo, The Lean Startup |
| Khoa học | Lược sử thời gian |
| Lịch sử | Sapiens: Lược sử loài người |
| CNTT | Lập trình C# cơ bản |
| Triết học | Muôn kiếp nhân sinh |

### Độc giả mẫu (5 thành viên)

| Mã thẻ | Họ tên | Loại thẻ | Email |
|---|---|---|---|
| TV001 | Nguyễn Văn An | Thường | an.nguyen@email.com |
| TV002 | Trần Thị Bình | VIP | binh.tran@email.com |
| TV003 | Lê Văn Cường | Sinh viên | cuong.le@email.com |
| TV004 | Phạm Thị Dung | Giáo viên | dung.pham@email.com |
| TV005 | Hoàng Gia Khánh | Sinh viên | khanh.hoang@email.com |

### Cài đặt hệ thống mặc định

| Cấu hình | Giá trị |
|---|---|
| Số ngày mượn tối đa | 14 ngày |
| Số sách tối đa/lần mượn | 5 cuốn |
| Tiền phạt quá hạn/ngày | 10,000 VNĐ |
| % đền bù sách mất | 200% |
| Số ngày giữ đặt trước | 3 ngày |

---

## 8. Cấu Trúc Dự Án

```
QLThuVien/
├── Database/
│   └── DatabaseInit.sql          ← Script tạo schema + dữ liệu mẫu
├── LibraryManagement/
│   ├── Program.cs                ← Entry point
│   ├── App.config                ← Connection string & cấu hình
│   ├── LibraryManagement.csproj  ← File project (.NET 8.0 WinForms)
│   ├── Models/                   ← Lớp dữ liệu (POCO)
│   │   ├── Book.cs               ← Book, Category, Author, Publisher
│   │   ├── BorrowRecord.cs       ← BorrowRecord, Reservation, FinePayment
│   │   ├── Member.cs             ← Member (độc giả)
│   │   ├── SystemSetting.cs      ← SystemSetting, ActivityLog, DashboardStats
│   │   └── User.cs               ← User, CurrentUser (quản lý session)
│   ├── Data/                     ← Tầng truy xuất dữ liệu (DAO)
│   │   ├── DatabaseConnection.cs ← Quản lý connection
│   │   ├── BookDAO.cs            ← CRUD sách
│   │   ├── BorrowRecordDAO.cs    ← Mượn/trả sách
│   │   ├── MemberDAO.cs          ← CRUD độc giả
│   │   ├── UserDAO.cs            ← CRUD user + đăng nhập
│   │   └── SystemSettingDAO.cs   ← Cấu hình + ActivityLogDAO + ReservationDAO
│   ├── Services/                 ← Logic nghiệp vụ
│   │   ├── BarcodeService.cs     ← Quét barcode (ZXing + OpenCV)
│   │   ├── DashboardService.cs   ← Thống kê dashboard
│   │   ├── DatabaseSchemaService.cs ← Tự động tạo schema DB
│   │   ├── EmailService.cs       ← Gửi email (SMTP/MailerSend)
│   │   ├── ExcelReportService.cs ← Xuất báo cáo Excel
│   │   ├── GeminiBookChatService.cs ← AI Chatbot (Gemini API)
│   │   ├── OpenLibraryBookService.cs ← Import sách từ OpenLibrary
│   │   ├── ReceiptService.cs     ← Xuất phiếu mượn PDF
│   │   └── RecommendationService.cs ← Gợi ý sách
│   ├── Forms/                    ← Giao diện WinForms (22 form)
│   └── Utils/
│       ├── ThemeManager.cs       ← Quản lý theme
│       └── VietnamInputValidator.cs ← Validate input chuẩn VN
└── LibraryManagement.sln         ← Solution file
```

---

## 9. Các Tính Năng Chính

| # | Tính năng | Mô tả |
|---|---|---|
| 1 | Trang công khai | Xem sách, tìm kiếm, chatbot AI — không cần đăng nhập |
| 2 | Quản lý sách | Thêm, sửa, xóa sách. Hỗ trợ ảnh bìa, barcode |
| 3 | Quản lý độc giả | CRUD thành viên, phân loại thẻ |
| 4 | Mượn/Trả sách | Giao dịch mượn trả, tính quá hạn tự động |
| 5 | Đặt trước | Hàng đợi đặt trước khi sách hết |
| 6 | Quản lý phạt | Tính tiền phạt quá hạn, thanh toán |
| 7 | Gia hạn sách | Cho phép gia hạn trước khi hết hạn |
| 8 | Xuất báo cáo Excel | Thống kê mượn, sách, thành viên |
| 9 | Xuất PDF | Phiếu mượn/trả dạng PDF |
| 10 | Quét barcode | Quét sách bằng webcam |
| 11 | AI Chatbot | Tư vấn sách bằng Google Gemini AI |
| 12 | Gợi ý sách | Gợi ý dựa trên lịch sử mượn |
| 13 | Email thông báo | Nhắc quá hạn, xác nhận mượn/trả |
| 14 | Import sách | Import từ OpenLibrary bằng ISBN |
| 15 | Dashboard | Thống kê tổng quan (biểu đồ) |

---

## 10. API Keys & Cấu Hình Bên Ngoài

Các API key trong `App.config` → `<appSettings>`:

| Key | Mô tả |
|---|---|
| `GeminiApiKey` | Google Gemini AI cho chatbot |
| `GeminiModel` | Model AI (mặc định: `gemini-2.5-flash`) |
| `SmtpUser` / `SmtpPassword` | Email SMTP |
| `MailerSendApiKey` | API key MailerSend |

> ⚠️ Các API key có thể đã hết hạn. Nên tạo key mới.

---

## 11. Xử Lý Lỗi Thường Gặp

### Lỗi 1: "Không thể kết nối đến Database!"
```powershell
# Kiểm tra SQL Server có chạy không
Get-Service MSSQLSERVER       # SQL Server full
Get-Service MSSQL$SQLEXPRESS  # SQL Server Express
sqllocaldb start MSSQLLocalDB # Khởi động LocalDB
```

### Lỗi 2: NuGet package not found
```powershell
cd d:\Documents\QLThuVien\QLThuVien
dotnet restore --force
```

### Lỗi 3: "System.BadImageFormatException"
Đảm bảo build **Any CPU** hoặc **x64**.

### Lỗi 4: OpenCV / Camera không hoạt động
```powershell
dotnet clean && dotnet restore && dotnet build
```

---

## 12. Lỗi Đã Phát Hiện & Sửa

### BUG: `ArgumentException: Column named BookID cannot be found`

**Vị trí:** `FormBorrow.cs` dòng 274, 275, 292

**Nguyên nhân:** Cột trong `dgvBooks` (Designer) có tên mặc định (`dataGridViewTextBoxColumn5-10`)
nhưng code truy cập bằng tên `Cells["BookID"]`, `Cells["Title"]`.

**Đã sửa:** Set đúng Name cho các cột trong `FormBorrow.Designer.cs`.

---

## NuGet Packages

| Package | Version | Chức năng |
|---|---|---|
| `Microsoft.Data.SqlClient` | 5.2.2 | SQL Server |
| `Dapper` | 2.1.24 | ORM nhẹ |
| `BCrypt.Net-Next` | 4.0.3 | Hash mật khẩu |
| `ClosedXML` | 0.105.0 | Xuất Excel |
| `PdfSharpCore` | 1.3.67 | Xuất PDF |
| `MailKit` | 4.15.1 | Email SMTP |
| `ZXing.Net` | 0.16.9 | Barcode |
| `OpenCvSharp4` | 4.10.0 | Camera/ảnh |
| `System.Windows.Forms.DataVisualization` | 1.0.0-pre | Biểu đồ |
