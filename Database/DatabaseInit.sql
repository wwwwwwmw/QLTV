SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users (
        UserID INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Password NVARCHAR(256) NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100),
        Phone NVARCHAR(20),
        Role NVARCHAR(20) NOT NULL DEFAULT 'Staff',
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        LastLogin DATETIME,
        CONSTRAINT CHK_Users_Role CHECK (Role IN ('Admin', 'Manager', 'Staff'))
    );
END
GO

IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories (
        CategoryID INT IDENTITY(1,1) PRIMARY KEY,
        CategoryName NVARCHAR(100) NOT NULL UNIQUE,
        Description NVARCHAR(500),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.Publishers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Publishers (
        PublisherID INT IDENTITY(1,1) PRIMARY KEY,
        PublisherName NVARCHAR(200) NOT NULL,
        Address NVARCHAR(300),
        Phone NVARCHAR(20),
        Email NVARCHAR(100),
        Website NVARCHAR(200),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.Authors', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Authors (
        AuthorID INT IDENTITY(1,1) PRIMARY KEY,
        AuthorName NVARCHAR(150) NOT NULL,
        Biography NVARCHAR(MAX),
        Country NVARCHAR(100),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.Books', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Books (
        BookID INT IDENTITY(1,1) PRIMARY KEY,
        ISBN NVARCHAR(20) NULL,
        Barcode NVARCHAR(50) NULL,
        Title NVARCHAR(300) NOT NULL,
        CategoryID INT NULL FOREIGN KEY REFERENCES dbo.Categories(CategoryID),
        AuthorID INT NULL FOREIGN KEY REFERENCES dbo.Authors(AuthorID),
        PublisherID INT NULL FOREIGN KEY REFERENCES dbo.Publishers(PublisherID),
        PublishYear INT NULL,
        Price DECIMAL(18,2) NOT NULL DEFAULT 0,
        TotalCopies INT NOT NULL DEFAULT 1,
        AvailableCopies INT NOT NULL DEFAULT 1,
        Description NVARCHAR(MAX),
        Location NVARCHAR(100),
        ImagePath NVARCHAR(500),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        CONSTRAINT CHK_Books_Copies CHECK (AvailableCopies >= 0 AND AvailableCopies <= TotalCopies)
    );
END
GO

IF OBJECT_ID(N'dbo.Members', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Members (
        MemberID INT IDENTITY(1,1) PRIMARY KEY,
        MemberCode NVARCHAR(20) NOT NULL UNIQUE,
        FullName NVARCHAR(150) NOT NULL,
        Gender NVARCHAR(10),
        DateOfBirth DATE,
        Address NVARCHAR(300),
        Phone NVARCHAR(20),
        Email NVARCHAR(100),
        IdentityCard NVARCHAR(20),
        MemberType NVARCHAR(50) NOT NULL DEFAULT N'Thường',
        JoinDate DATE NOT NULL DEFAULT GETDATE(),
        ExpiryDate DATE,
        TotalFine DECIMAL(18,2) NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        Notes NVARCHAR(500),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        CONSTRAINT CHK_Members_Gender CHECK (Gender IN (N'Nam', N'Nữ', N'Khác'))
    );
END
GO

IF OBJECT_ID(N'dbo.BorrowRecords', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BorrowRecords (
        BorrowID INT IDENTITY(1,1) PRIMARY KEY,
        BorrowCode NVARCHAR(20) NOT NULL UNIQUE,
        MemberID INT NOT NULL FOREIGN KEY REFERENCES dbo.Members(MemberID),
        BookID INT NOT NULL FOREIGN KEY REFERENCES dbo.Books(BookID),
        BorrowDate DATE NOT NULL DEFAULT GETDATE(),
        DueDate DATE NOT NULL,
        ReturnDate DATE,
        Quantity INT NOT NULL DEFAULT 1,
        Status NVARCHAR(20) NOT NULL DEFAULT N'Đang mượn',
        FineAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        FinePaid BIT NOT NULL DEFAULT 0,
        Notes NVARCHAR(500),
        StaffID INT NULL FOREIGN KEY REFERENCES dbo.Users(UserID),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        CONSTRAINT CHK_BorrowRecords_Status CHECK (Status IN (N'Đang mượn', N'Đã trả', N'Quá hạn', N'Mất sách'))
    );
END
GO

IF OBJECT_ID(N'dbo.Reservations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Reservations (
        ReservationID INT IDENTITY(1,1) PRIMARY KEY,
        MemberID INT NOT NULL FOREIGN KEY REFERENCES dbo.Members(MemberID),
        BookID INT NOT NULL FOREIGN KEY REFERENCES dbo.Books(BookID),
        ReservationDate DATETIME NOT NULL DEFAULT GETDATE(),
        ExpiryDate DATETIME,
        Status NVARCHAR(20) NOT NULL DEFAULT N'Chờ',
        Notes NVARCHAR(500),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT CHK_Reservations_Status CHECK (Status IN (N'Chờ', N'Đã nhận', N'Hủy', N'Hết hạn'))
    );
END
GO

IF OBJECT_ID(N'dbo.FinePayments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.FinePayments (
        PaymentID INT IDENTITY(1,1) PRIMARY KEY,
        MemberID INT NOT NULL FOREIGN KEY REFERENCES dbo.Members(MemberID),
        BorrowID INT NULL FOREIGN KEY REFERENCES dbo.BorrowRecords(BorrowID),
        Amount DECIMAL(18,2) NOT NULL,
        PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
        PaymentMethod NVARCHAR(50) NOT NULL DEFAULT N'Tiền mặt',
        Notes NVARCHAR(500),
        StaffID INT NULL FOREIGN KEY REFERENCES dbo.Users(UserID),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.SystemSettings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SystemSettings (
        SettingID INT IDENTITY(1,1) PRIMARY KEY,
        SettingKey NVARCHAR(100) NOT NULL UNIQUE,
        SettingValue NVARCHAR(500),
        Description NVARCHAR(300),
        UpdatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.ActivityLogs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ActivityLogs (
        LogID INT IDENTITY(1,1) PRIMARY KEY,
        UserID INT NULL FOREIGN KEY REFERENCES dbo.Users(UserID),
        Action NVARCHAR(100) NOT NULL,
        TableName NVARCHAR(50),
        RecordID INT,
        OldValue NVARCHAR(MAX),
        NewValue NVARCHAR(MAX),
        IPAddress NVARCHAR(50),
        ComputerName NVARCHAR(100),
        LogDate DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.BookRecommendations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BookRecommendations
    (
        RecommendationID INT IDENTITY(1,1) PRIMARY KEY,
        MemberID INT NOT NULL,
        BookID INT NOT NULL,
        Score INT NOT NULL,
        Reason NVARCHAR(255),
        CreatedDate DATETIME DEFAULT(GETDATE())
    );

    CREATE UNIQUE INDEX UX_BookRecommendations ON dbo.BookRecommendations(MemberID, BookID);
END
GO

IF OBJECT_ID(N'dbo.BookReservations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BookReservations
    (
        ReservationID INT IDENTITY(1,1) PRIMARY KEY,
        MemberID INT NOT NULL,
        BookID INT NOT NULL,
        Status NVARCHAR(30) NOT NULL DEFAULT(N'Chờ'),
        ReservedDate DATETIME NOT NULL DEFAULT(GETDATE()),
        NotifiedDate DATETIME NULL,
        Notes NVARCHAR(500) NULL,
        CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
    );

    CREATE INDEX IX_BookReservations_Book_Status ON dbo.BookReservations(BookID, Status, ReservedDate);
END
GO

IF COL_LENGTH(N'dbo.Books', N'Barcode') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD Barcode NVARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Books_Title' AND object_id = OBJECT_ID(N'dbo.Books'))
    CREATE INDEX IX_Books_Title ON dbo.Books(Title);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Books_ISBN' AND object_id = OBJECT_ID(N'dbo.Books'))
    CREATE INDEX IX_Books_ISBN ON dbo.Books(ISBN);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Books_Barcode' AND object_id = OBJECT_ID(N'dbo.Books'))
    CREATE UNIQUE INDEX IX_Books_Barcode ON dbo.Books(Barcode) WHERE Barcode IS NOT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Books_CategoryID' AND object_id = OBJECT_ID(N'dbo.Books'))
    CREATE INDEX IX_Books_CategoryID ON dbo.Books(CategoryID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Members_MemberCode' AND object_id = OBJECT_ID(N'dbo.Members'))
    CREATE INDEX IX_Members_MemberCode ON dbo.Members(MemberCode);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Members_FullName' AND object_id = OBJECT_ID(N'dbo.Members'))
    CREATE INDEX IX_Members_FullName ON dbo.Members(FullName);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_BorrowRecords_MemberID' AND object_id = OBJECT_ID(N'dbo.BorrowRecords'))
    CREATE INDEX IX_BorrowRecords_MemberID ON dbo.BorrowRecords(MemberID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_BorrowRecords_BookID' AND object_id = OBJECT_ID(N'dbo.BorrowRecords'))
    CREATE INDEX IX_BorrowRecords_BookID ON dbo.BorrowRecords(BookID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_BorrowRecords_Status' AND object_id = OBJECT_ID(N'dbo.BorrowRecords'))
    CREATE INDEX IX_BorrowRecords_Status ON dbo.BorrowRecords(Status);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_BorrowRecords_DueDate' AND object_id = OBJECT_ID(N'dbo.BorrowRecords'))
    CREATE INDEX IX_BorrowRecords_DueDate ON dbo.BorrowRecords(DueDate);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'admin')
INSERT INTO dbo.Users (Username, Password, FullName, Email, Role)
VALUES (N'admin', N'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', N'Lê Quang Minh', N'admin@library.vn', N'Admin'); 
-- password: admin
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'manager')
INSERT INTO dbo.Users (Username, Password, FullName, Email, Role)
VALUES (N'manager', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Nguyễn Hải Nam', N'manager@library.vn', N'Manager');
-- password: 123456
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'staff1')
INSERT INTO dbo.Users (Username, Password, FullName, Email, Role)
VALUES (N'staff1', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Trần Thu Hằng', N'staff1@library.vn', N'Staff');
-- password: 123456
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Văn học') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Văn học', N'Tiểu thuyết, truyện ngắn, tác phẩm kinh điển');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Khoa học') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Khoa học', N'Khoa học tự nhiên và ứng dụng');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Kinh tế') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Kinh tế', N'Kinh doanh, tài chính, quản trị');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Lịch sử') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Lịch sử', N'Lịch sử Việt Nam và thế giới');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Tâm lý - Kỹ năng') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Tâm lý - Kỹ năng', N'Phát triển bản thân và kỹ năng sống');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Công nghệ thông tin') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Công nghệ thông tin', N'Lập trình, dữ liệu, hệ thống');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Thiếu nhi') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Thiếu nhi', N'Sách dành cho thiếu nhi và tuổi mới lớn');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE CategoryName = N'Triết học') INSERT INTO dbo.Categories (CategoryName, Description) VALUES (N'Triết học', N'Tư tưởng và triết học');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Publishers WHERE PublisherName = N'NXB Trẻ') INSERT INTO dbo.Publishers (PublisherName, Address, Phone, Website) VALUES (N'NXB Trẻ', N'161B Lý Chính Thắng, Q.3, TP.HCM', N'028-39316289', N'https://nxbtre.com.vn');
IF NOT EXISTS (SELECT 1 FROM dbo.Publishers WHERE PublisherName = N'NXB Kim Đồng') INSERT INTO dbo.Publishers (PublisherName, Address, Phone, Website) VALUES (N'NXB Kim Đồng', N'55 Quang Trung, Hà Nội', N'024-39434730', N'https://nxbkimdong.com.vn');
IF NOT EXISTS (SELECT 1 FROM dbo.Publishers WHERE PublisherName = N'NXB Tổng hợp TP.HCM') INSERT INTO dbo.Publishers (PublisherName, Address, Phone, Website) VALUES (N'NXB Tổng hợp TP.HCM', N'62 Nguyễn Thị Minh Khai, Q.1, TP.HCM', N'028-38294409', N'https://nxbhcm.com.vn');
IF NOT EXISTS (SELECT 1 FROM dbo.Publishers WHERE PublisherName = N'NXB Thế Giới') INSERT INTO dbo.Publishers (PublisherName, Address, Phone, Website) VALUES (N'NXB Thế Giới', N'46 Trần Hưng Đạo, Hà Nội', N'024-38253841', N'https://thegioipublishers.vn');
IF NOT EXISTS (SELECT 1 FROM dbo.Publishers WHERE PublisherName = N'NXB Lao Động') INSERT INTO dbo.Publishers (PublisherName, Address, Phone, Website) VALUES (N'NXB Lao Động', N'175 Giảng Võ, Hà Nội', N'024-38515380', N'https://nxblaodong.com.vn');
IF NOT EXISTS (SELECT 1 FROM dbo.Publishers WHERE PublisherName = N'NXB Văn Học') INSERT INTO dbo.Publishers (PublisherName, Address, Phone, Website) VALUES (N'NXB Văn Học', N'18 Nguyễn Trường Tộ, Hà Nội', N'024-39230931', N'https://nxbvanhoc.com.vn');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Nguyễn Nhật Ánh') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Nguyễn Nhật Ánh', N'Việt Nam');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Tô Hoài') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Tô Hoài', N'Việt Nam');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Nam Cao') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Nam Cao', N'Việt Nam');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Dale Carnegie') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Dale Carnegie', N'Mỹ');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Paulo Coelho') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Paulo Coelho', N'Brazil');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Yuval Noah Harari') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Yuval Noah Harari', N'Israel');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Robert T. Kiyosaki') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Robert T. Kiyosaki', N'Mỹ');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Stephen Hawking') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Stephen Hawking', N'Anh');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Harper Lee') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Harper Lee', N'Mỹ');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'George Orwell') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'George Orwell', N'Anh');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'F. Scott Fitzgerald') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'F. Scott Fitzgerald', N'Mỹ');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Antoine de Saint-Exupéry') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Antoine de Saint-Exupéry', N'Pháp');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Dan Brown') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Dan Brown', N'Mỹ');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Eric Ries') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Eric Ries', N'Mỹ');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Daniel Kahneman') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Daniel Kahneman', N'Israel');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'James Clear') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'James Clear', N'Mỹ');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Nguyễn Phong') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Nguyễn Phong', N'Việt Nam');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Viktor E. Frankl') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Viktor E. Frankl', N'Áo');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Nhã Nam tuyển chọn') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Nhã Nam tuyển chọn', N'Việt Nam');
IF NOT EXISTS (SELECT 1 FROM dbo.Authors WHERE AuthorName = N'Nguyễn Đình Hòa') INSERT INTO dbo.Authors (AuthorName, Country) VALUES (N'Nguyễn Đình Hòa', N'Việt Nam');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786041132466')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786041132466', N'BC200001', N'Cho tôi xin một vé đi tuổi thơ', c.CategoryID, a.AuthorID, p.PublisherID, 2018, 98000, 6, 6, N'Truyện dài về ký ức tuổi thơ và hành trình trưởng thành của bốn người bạn nhỏ.', N'A1-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Văn học' AND a.AuthorName = N'Nguyễn Nhật Ánh' AND p.PublisherName = N'NXB Trẻ';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786042106732')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786042106732', N'BC200002', N'Dế Mèn phiêu lưu ký', c.CategoryID, a.AuthorID, p.PublisherID, 2021, 72000, 5, 5, N'Tác phẩm thiếu nhi kinh điển về chuyến phiêu lưu và bài học trách nhiệm.', N'A1-02'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Thiếu nhi' AND a.AuthorName = N'Tô Hoài' AND p.PublisherName = N'NXB Kim Đồng';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786049760609')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786049760609', N'BC200003', N'Chí Phèo', c.CategoryID, a.AuthorID, p.PublisherID, 2019, 56000, 4, 4, N'Truyện ngắn hiện thực phản ánh số phận người nông dân trong xã hội cũ.', N'A1-03'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Văn học' AND a.AuthorName = N'Nam Cao' AND p.PublisherName = N'NXB Văn Học';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786042105070')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786042105070', N'BC200004', N'Đắc nhân tâm', c.CategoryID, a.AuthorID, p.PublisherID, 2020, 98000, 8, 8, N'Sách kinh điển về nghệ thuật giao tiếp và xây dựng ảnh hưởng tích cực.', N'B1-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Tâm lý - Kỹ năng' AND a.AuthorName = N'Dale Carnegie' AND p.PublisherName = N'NXB Trẻ';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786042101386')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786042101386', N'BC200005', N'Nhà giả kim', c.CategoryID, a.AuthorID, p.PublisherID, 2022, 89000, 6, 6, N'Tiểu thuyết ngụ ngôn về hành trình đi tìm kho báu và ý nghĩa sống.', N'A2-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Văn học' AND a.AuthorName = N'Paulo Coelho' AND p.PublisherName = N'NXB Trẻ';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786045895879')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786045895879', N'BC200006', N'Sapiens: Lược sử loài người', c.CategoryID, a.AuthorID, p.PublisherID, 2021, 219000, 4, 4, N'Cái nhìn tổng quan về lịch sử tiến hóa và phát triển của loài người.', N'C1-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Lịch sử' AND a.AuthorName = N'Yuval Noah Harari' AND p.PublisherName = N'NXB Thế Giới';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786041223188')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786041223188', N'BC200007', N'Cha giàu cha nghèo', c.CategoryID, a.AuthorID, p.PublisherID, 2020, 126000, 7, 7, N'Sách tài chính cá nhân tập trung vào tư duy tiền bạc và đầu tư.', N'B1-02'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Kinh tế' AND a.AuthorName = N'Robert T. Kiyosaki' AND p.PublisherName = N'NXB Trẻ';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786045899891')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786045899891', N'BC200008', N'Lược sử thời gian', c.CategoryID, a.AuthorID, p.PublisherID, 2019, 168000, 4, 4, N'Tác phẩm phổ biến kiến thức vật lý thiên văn và bản chất thời gian.', N'C1-02'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Khoa học' AND a.AuthorName = N'Stephen Hawking' AND p.PublisherName = N'NXB Thế Giới';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780061120084')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780061120084', N'BC200009', N'To Kill a Mockingbird', c.CategoryID, a.AuthorID, p.PublisherID, 1960, 198000, 3, 3, N'Tiểu thuyết kinh điển về công lý, định kiến chủng tộc và lòng nhân ái.', N'A2-02'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Văn học' AND a.AuthorName = N'Harper Lee' AND p.PublisherName = N'NXB Thế Giới';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780451524935')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780451524935', N'BC200010', N'1984', c.CategoryID, a.AuthorID, p.PublisherID, 1949, 175000, 4, 4, N'Tiểu thuyết phản địa đàng về kiểm soát xã hội và tự do cá nhân.', N'A2-03'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Văn học' AND a.AuthorName = N'George Orwell' AND p.PublisherName = N'NXB Thế Giới';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780743273565')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780743273565', N'BC200011', N'The Great Gatsby', c.CategoryID, a.AuthorID, p.PublisherID, 1925, 169000, 3, 3, N'Tác phẩm kinh điển về giấc mơ Mỹ, tình yêu và bi kịch thời thịnh vượng.', N'A2-04'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Văn học' AND a.AuthorName = N'F. Scott Fitzgerald' AND p.PublisherName = N'NXB Thế Giới';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780156012195')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780156012195', N'BC200012', N'Le Petit Prince', c.CategoryID, a.AuthorID, p.PublisherID, 1943, 122000, 5, 5, N'Câu chuyện ngắn giàu triết lý về tình bạn, tình yêu và sự trưởng thành.', N'A3-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Thiếu nhi' AND a.AuthorName = N'Antoine de Saint-Exupéry' AND p.PublisherName = N'NXB Kim Đồng';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780307474278')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780307474278', N'BC200013', N'The Da Vinci Code', c.CategoryID, a.AuthorID, p.PublisherID, 2003, 199000, 4, 4, N'Tiểu thuyết trinh thám pha lịch sử với nhiều mật mã và biểu tượng tôn giáo.', N'A3-02'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Văn học' AND a.AuthorName = N'Dan Brown' AND p.PublisherName = N'NXB Thế Giới';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780307887894')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780307887894', N'BC200014', N'The Lean Startup', c.CategoryID, a.AuthorID, p.PublisherID, 2011, 210000, 4, 4, N'Phương pháp khởi nghiệp tinh gọn dựa trên học hỏi nhanh và kiểm chứng giả thuyết.', N'B2-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Kinh tế' AND a.AuthorName = N'Eric Ries' AND p.PublisherName = N'NXB Lao Động';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780374533557')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780374533557', N'BC200015', N'Thinking, Fast and Slow', c.CategoryID, a.AuthorID, p.PublisherID, 2011, 235000, 4, 4, N'Khám phá hai hệ thống tư duy của con người và các thiên kiến nhận thức.', N'B2-02'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Tâm lý - Kỹ năng' AND a.AuthorName = N'Daniel Kahneman' AND p.PublisherName = N'NXB Lao Động';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780735211292')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780735211292', N'BC200016', N'Atomic Habits', c.CategoryID, a.AuthorID, p.PublisherID, 2018, 199000, 6, 6, N'Sách về phương pháp xây dựng thói quen tốt và loại bỏ thói quen xấu.', N'B2-03'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Tâm lý - Kỹ năng' AND a.AuthorName = N'James Clear' AND p.PublisherName = N'NXB Trẻ';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786049787064')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786049787064', N'BC200017', N'Muôn kiếp nhân sinh', c.CategoryID, a.AuthorID, p.PublisherID, 2020, 188000, 5, 5, N'Tác phẩm kể nhiều câu chuyện nhân quả và triết lý sống qua các kiếp người.', N'D1-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Triết học' AND a.AuthorName = N'Nguyễn Phong' AND p.PublisherName = N'NXB Tổng hợp TP.HCM';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9780807014295')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9780807014295', N'BC200018', N'Man''s Search for Meaning', c.CategoryID, a.AuthorID, p.PublisherID, 1946, 185000, 3, 3, N'Hồi ký và triết luận về ý nghĩa cuộc sống trong hoàn cảnh khắc nghiệt.', N'D1-02'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Triết học' AND a.AuthorName = N'Viktor E. Frankl' AND p.PublisherName = N'NXB Thế Giới';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786045991564')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786045991564', N'BC200019', N'Lập trình C# cơ bản', c.CategoryID, a.AuthorID, p.PublisherID, 2022, 159000, 5, 5, N'Giáo trình nền tảng về C# và phát triển ứng dụng .NET cho người mới bắt đầu.', N'E1-01'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Công nghệ thông tin' AND a.AuthorName = N'Nguyễn Đình Hòa' AND p.PublisherName = N'NXB Lao Động';

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE ISBN = N'9786043651215')
INSERT INTO dbo.Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Description, Location)
SELECT N'9786043651215', N'BC200020', N'Những bài học lịch sử', c.CategoryID, a.AuthorID, p.PublisherID, 2021, 149000, 5, 5, N'Tuyển tập các bài học lịch sử tiêu biểu giúp nhìn lại các bước ngoặt của nhân loại.', N'C2-03'
FROM dbo.Categories c, dbo.Authors a, dbo.Publishers p
WHERE c.CategoryName = N'Lịch sử' AND a.AuthorName = N'Nhã Nam tuyển chọn' AND p.PublisherName = N'NXB Tổng hợp TP.HCM';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Members WHERE MemberCode = N'TV001')
INSERT INTO dbo.Members (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, IdentityCard, MemberType, JoinDate, ExpiryDate, Notes)
VALUES (N'TV001', N'Nguyễn Văn An', N'Nam', '1995-05-15', N'123 Nguyễn Huệ, Quận 1, TP.HCM', N'0901234567', N'an.nguyen@email.com', N'079095000111', N'Thường', '2025-01-05', '2026-12-31', N'Bạn đọc mượn đều');

IF NOT EXISTS (SELECT 1 FROM dbo.Members WHERE MemberCode = N'TV002')
INSERT INTO dbo.Members (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, IdentityCard, MemberType, JoinDate, ExpiryDate, Notes)
VALUES (N'TV002', N'Trần Thị Bình', N'Nữ', '1998-08-20', N'456 Lê Lợi, Quận 3, TP.HCM', N'0912345678', N'binh.tran@email.com', N'079098000222', N'VIP', '2024-12-10', '2027-12-31', N'Ưu tiên sách kinh tế');

IF NOT EXISTS (SELECT 1 FROM dbo.Members WHERE MemberCode = N'TV003')
INSERT INTO dbo.Members (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, IdentityCard, MemberType, JoinDate, ExpiryDate, Notes)
VALUES (N'TV003', N'Lê Văn Cường', N'Nam', '2001-01-10', N'789 Trần Hưng Đạo, Quận 5, TP.HCM', N'0923456789', N'cuong.le@email.com', N'079101000333', N'Sinh viên', '2025-02-18', '2026-06-30', N'Ưu tiên sách CNTT');

IF NOT EXISTS (SELECT 1 FROM dbo.Members WHERE MemberCode = N'TV004')
INSERT INTO dbo.Members (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, IdentityCard, MemberType, JoinDate, ExpiryDate, Notes)
VALUES (N'TV004', N'Phạm Thị Dung', N'Nữ', '1990-12-25', N'321 Hai Bà Trưng, Quận 1, TP.HCM', N'0934567890', N'dung.pham@email.com', N'079090000444', N'Giáo viên', '2024-11-01', '2027-01-01', N'Mượn theo chủ đề lịch sử');

IF NOT EXISTS (SELECT 1 FROM dbo.Members WHERE MemberCode = N'TV005')
INSERT INTO dbo.Members (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, IdentityCard, MemberType, JoinDate, ExpiryDate, Notes)
VALUES (N'TV005', N'Hoàng Gia Khánh', N'Nam', '2002-03-08', N'654 Võ Văn Tần, Quận 3, TP.HCM', N'0945678901', N'khanh.hoang@email.com', N'079102000555', N'Sinh viên', '2025-03-12', '2026-06-30', N'Hay mượn sách kỹ năng');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'MaxBorrowDays')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'MaxBorrowDays', N'14', N'Số ngày mượn tối đa');
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'MaxBooksPerMember')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'MaxBooksPerMember', N'5', N'Số sách tối đa mỗi lần mượn');
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'FinePerDay')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'FinePerDay', N'10000', N'Tiền phạt quá hạn mỗi ngày');
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'LostBookFinePercent')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'LostBookFinePercent', N'200', N'Phần trăm đền bù sách mất');
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'LibraryName')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'LibraryName', N'Thư viện Sách', N'Tên thư viện');
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'LibraryAddress')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'LibraryAddress', N'120 Nguyễn Đình Chiểu, Quận 3, TP.HCM', N'Địa chỉ thư viện');
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'LibraryPhone')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'LibraryPhone', N'028-38223344', N'Số điện thoại thư viện');
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSettings WHERE SettingKey = N'ReservationDays')
INSERT INTO dbo.SystemSettings (SettingKey, SettingValue, Description) VALUES (N'ReservationDays', N'3', N'Số ngày giữ sách đặt trước');
GO

CREATE OR ALTER PROCEDURE dbo.sp_Login
    @Username NVARCHAR(50),
    @Password NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UserID, Username, FullName, Email, Phone, Role, IsActive
    FROM dbo.Users
    WHERE Username = @Username AND Password = @Password AND IsActive = 1;

    IF @@ROWCOUNT > 0
    BEGIN
        UPDATE dbo.Users SET LastLogin = GETDATE() WHERE Username = @Username;
    END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_SearchBooks
    @Keyword NVARCHAR(200) = NULL,
    @CategoryID INT = NULL,
    @AuthorID INT = NULL,
    @AvailableOnly BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
    FROM dbo.Books b
    LEFT JOIN dbo.Categories c ON b.CategoryID = c.CategoryID
    LEFT JOIN dbo.Authors a ON b.AuthorID = a.AuthorID
    LEFT JOIN dbo.Publishers p ON b.PublisherID = p.PublisherID
    WHERE b.IsActive = 1
      AND (@Keyword IS NULL OR b.Title LIKE '%' + @Keyword + '%' OR b.ISBN LIKE '%' + @Keyword + '%' OR b.Barcode LIKE '%' + @Keyword + '%')
      AND (@CategoryID IS NULL OR b.CategoryID = @CategoryID)
      AND (@AuthorID IS NULL OR b.AuthorID = @AuthorID)
      AND (@AvailableOnly = 0 OR b.AvailableCopies > 0)
    ORDER BY b.Title;
END
GO
