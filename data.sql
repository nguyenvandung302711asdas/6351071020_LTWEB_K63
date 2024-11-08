



---Xoa CSDL QLBansach neu da co
use master
Drop Database QLBansach




create database QLBANSACH
GO
use QLBANSACH

GO
CREATE TABLE KHACHHANG
(
    MaKH INT IDENTITY(1,1),
    HoTen NVARCHAR(50) NOT NULL,
    Taikhoan VARCHAR(50) UNIQUE,
    Matkhau VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    DiachiKH NVARCHAR(200),
    DienthoaiKH VARCHAR(50),
    Ngaysinh DATETIME,
    CONSTRAINT PK_Khachhang PRIMARY KEY(MaKH)
);

GO
CREATE TABLE CHUDE
(
    MaCD INT IDENTITY(1,1),
    TenChuDe NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_ChuDe PRIMARY KEY(MaCD)
);

GO
CREATE TABLE NHAXUATBAN
(
    MaNXB INT IDENTITY(1,1),
    TenNXB NVARCHAR(50) NOT NULL,
    Diachi NVARCHAR(200),
    DienThoai VARCHAR(50),
    CONSTRAINT PK_NhaXuatBan PRIMARY KEY(MaNXB)
);

GO
CREATE TABLE SACH
(
    Masach INT IDENTITY(1,1),
    Tensach NVARCHAR(100) NOT NULL,
    Giaban DECIMAL(18,0) CHECK (Giaban >= 0),
    Mota NVARCHAR(MAX),
    Anhbia VARCHAR(50),
    Ngaycapnhat DATETIME,
    Soluongton INT,
    MaCD INT,
    MaNXB INT,
    CONSTRAINT PK_Sach PRIMARY KEY(Masach),
    CONSTRAINT FK_Chude FOREIGN KEY(MaCD) REFERENCES CHUDE(MaCD),
    CONSTRAINT FK_NhaXB FOREIGN KEY(MaNXB) REFERENCES NHAXUATBAN(MaNXB)
);

GO
CREATE TABLE TACGIA
(
    MaTG INT IDENTITY(1,1),
    TenTG NVARCHAR(50) NOT NULL,
    Diachi NVARCHAR(100),
    Tieusu NVARCHAR(MAX),
    Dienthoai VARCHAR(50),
    CONSTRAINT PK_TacGia PRIMARY KEY(MaTG)
);

GO
CREATE TABLE VIETSACH
(
    MaTG INT NOT NULL,
    Masach INT NOT NULL,
    Vaitro NVARCHAR(50) NULL,
    Vitri NVARCHAR(50) NULL,
    CONSTRAINT PK_VietSach PRIMARY KEY (MaTG, Masach),
    CONSTRAINT FK_VietSach_TacGia FOREIGN KEY (MaTG) REFERENCES TACGIA(MaTG),
    CONSTRAINT FK_VietSach_Sach FOREIGN KEY (Masach) REFERENCES SACH(Masach)
);

GO
CREATE TABLE DONDATHANG
(
    MaDonHang INT IDENTITY(1,1),
    Dathanhtoan BIT,
    Tinhtranggiaohang BIT,
    Ngaydat DATETIME,
    Ngaygiao DATETIME,
    MaKH INT,
    CONSTRAINT FK_Khachhang FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
    CONSTRAINT PK_DonDatHang PRIMARY KEY (MaDonHang)
);

GO
CREATE TABLE CHITIETDONHANG
(
    MaDonHang INT,
    Masach INT,
    Soluong INT CHECK(Soluong > 0),
    Dongia DECIMAL(18,0) CHECK(Dongia >= 0),
    CONSTRAINT PK_CTDatHang PRIMARY KEY (MaDonHang, Masach),
    CONSTRAINT FK_Donhang FOREIGN KEY (MaDonHang) REFERENCES DONDATHANG(MaDonHang),
    CONSTRAINT FK_Sach FOREIGN KEY (Masach) REFERENCES SACH(Masach)
);

-- Insert data into KHACHHANG
INSERT INTO KHACHHANG (HoTen, Taikhoan, Matkhau, Email, DiachiKH, DienthoaiKH, Ngaysinh)
VALUES 
(N'Nguyen Van A', 'user1', 'pass1', 'user1@example.com', N'123 Le Loi, HCMC', '0123456789', '1980-01-01'),
(N'Le Thi B', 'user2', 'pass2', 'user2@example.com', N'456 Nguyen Trai, HCMC', '0987654321', '1990-05-20'),
(N'Tran Van C', 'user3', 'pass3', 'user3@example.com', N'789 Dien Bien Phu, HCMC', '0912345678', '1985-03-15'),
(N'Pham Thi D', 'user4', 'pass4', 'user4@example.com', N'321 Tran Hung Dao, HCMC', '0934567890', '1992-12-10'),
(N'Hoang Van E', 'user5', 'pass5', 'user5@example.com', N'654 Ba Trieu, HCMC', '0901234567', '1988-06-25'),
(N'Bui Thi F', 'user6', 'pass6', 'user6@example.com', N'987 Ly Thuong Kiet, HCMC', '0986543210', '1995-09-30');

-- Insert data into CHUDE
INSERT INTO CHUDE (TenChuDe)
VALUES 
(N'Literature'),
(N'Science'),
(N'History'),
(N'Technology'),
(N'Art'),
(N'Philosophy');

-- Insert data into NHAXUATBAN
INSERT INTO NHAXUATBAN (TenNXB, Diachi, DienThoai)
VALUES 
(N'Publisher A', N'123 Dinh Tien Hoang, HCMC', '0281234567'),
(N'Publisher B', N'456 Vo Thi Sau, HCMC', '0282345678'),
(N'Publisher C', N'789 Le Lai, HCMC', '0283456789'),
(N'Publisher D', N'321 Tran Quoc Thao, HCMC', '0284567890'),
(N'Publisher E', N'654 Pham Ngu Lao, HCMC', '0285678901'),
(N'Publisher F', N'987 Nguyen Dinh Chieu, HCMC', '0286789012');

-- Insert data into SACH
INSERT INTO SACH (Tensach, Giaban, Mota, Anhbia, Ngaycapnhat, Soluongton, MaCD, MaNXB)
VALUES 
(N'book1', 50000, N'Description 1', 'templatemo_image_01.jpg', '2024-01-01', 100, 1, 1),
(N'book2', 60000, N'Description 2', 'templatemo_image_02.jpg', '2024-02-01', 200, 2, 2),
(N'book3', 70000, N'Description 3', 'templatemo_image_03.jpg', '2024-03-01', 150, 3, 3),
(N'book4', 80000, N'Description 4', 'templatemo_image_04.jpg', '2024-04-01', 180, 4, 4),
(N'book5', 90000, N'Description 5', 'templatemo_image_05.jpg', '2024-05-01', 120, 5, 5),
(N'book6', 100000, N'Description 6', 'templatemo_image_06.jpg', '2024-06-01', 130, 6, 6);



-- Insert data into TACGIA
INSERT INTO TACGIA (TenTG, Diachi, Tieusu, Dienthoai)
VALUES 
(N'Author A', N'Address A', N'Biography of Author A', '0901122334'),
(N'Author B', N'Address B', N'Biography of Author B', '0902233445'),
(N'Author C', N'Address C', N'Biography of Author C', '0903344556'),
(N'Author D', N'Address D', N'Biography of Author D', '0904455667'),
(N'Author E', N'Address E', N'Biography of Author E', '0905566778'),
(N'Author F', N'Address F', N'Biography of Author F', '0906677889');

-- Insert data into VIETSACH
INSERT INTO VIETSACH (MaTG, Masach, Vaitro, Vitri)
VALUES 
(1, 1, N'Main Author', N'Position A'),
(2, 2, N'Co-Author', N'Position B'),
(3, 3, N'Main Author', N'Position C'),
(4, 4, N'Editor', N'Position D'),
(5, 5, N'Contributor', N'Position E'),
(6, 6, N'Main Author', N'Position F');

-- Insert data into DONDATHANG
INSERT INTO DONDATHANG (Dathanhtoan, Tinhtranggiaohang, Ngaydat, Ngaygiao, MaKH)
VALUES 
(1, 1, '2024-10-01', '2024-10-05', 1),
(0, 0, '2024-10-02', '2024-10-06', 2),
(1, 1, '2024-10-03', '2024-10-07', 3),
(1, 0, '2024-10-04', '2024-10-08', 4),
(0, 1, '2024-10-05', '2024-10-09', 5),
(1, 1, '2024-10-06', '2024-10-10', 6);

-- Insert data into CHITIETDONHANG
INSERT INTO CHITIETDONHANG (MaDonHang, Masach, Soluong, Dongia)
VALUES 
(1, 1, 2, 50000),
(1, 2, 1, 60000),
(2, 3, 3, 70000),
(3, 4, 1, 80000),
(4, 5, 2, 90000),
(5, 6, 1, 100000),
(6, 1, 1, 50000);
