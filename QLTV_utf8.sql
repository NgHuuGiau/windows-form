USE [QUANLYTHUVIEN]
GO
ALTER TABLE [dbo].[TheDocGia] DROP CONSTRAINT [CK_DocGia_Tuoi]
GO
ALTER TABLE [dbo].[TheDocGia] DROP CONSTRAINT [CK_DocGia_LoaiDocGia]
GO
ALTER TABLE [dbo].[HoSoNhanVien] DROP CONSTRAINT [CK_NhanVien_ChucVu]
GO
ALTER TABLE [dbo].[HoSoNhanVien] DROP CONSTRAINT [CK_NhanVien_BoPhan]
GO
ALTER TABLE [dbo].[HoSoNhanVien] DROP CONSTRAINT [CK_NhanVien_BangCap]
GO
ALTER TABLE [dbo].[ThongTinSach] DROP CONSTRAINT [FK_ThongTinSach_DauSach]
GO
ALTER TABLE [dbo].[ThanhLySach] DROP CONSTRAINT [FK_ThanhLySach_ThongTinSach]
GO
ALTER TABLE [dbo].[PhieuMuon] DROP CONSTRAINT [FK_DanhSachMuon_TheDocGia]
GO
ALTER TABLE [dbo].[GhiNhanMatSach] DROP CONSTRAINT [FK_GhiNhanMatSach_ThongTinSach]
GO
ALTER TABLE [dbo].[GhiNhanMatSach] DROP CONSTRAINT [FK_GhiNhanMatSach_TheDocGia]
GO
ALTER TABLE [dbo].[ChiTietMuon] DROP CONSTRAINT [FK_ChiTietMuon_ThongTinSach]
GO
ALTER TABLE [dbo].[ChiTietMuon] DROP CONSTRAINT [FK_ChiTietMuon_PhieuMuon]
GO
/****** Object:  Table [dbo].[ThongTinSach]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ThongTinSach]') AND type in (N'U'))
DROP TABLE [dbo].[ThongTinSach]
GO
/****** Object:  Table [dbo].[TheLoai]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TheLoai]') AND type in (N'U'))
DROP TABLE [dbo].[TheLoai]
GO
/****** Object:  Table [dbo].[TheDocGia]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TheDocGia]') AND type in (N'U'))
DROP TABLE [dbo].[TheDocGia]
GO
/****** Object:  Table [dbo].[ThanhLySach]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ThanhLySach]') AND type in (N'U'))
DROP TABLE [dbo].[ThanhLySach]
GO
/****** Object:  Table [dbo].[PhieuMuon]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhieuMuon]') AND type in (N'U'))
DROP TABLE [dbo].[PhieuMuon]
GO
/****** Object:  Table [dbo].[HoSoNhanVien]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HoSoNhanVien]') AND type in (N'U'))
DROP TABLE [dbo].[HoSoNhanVien]
GO
/****** Object:  Table [dbo].[GhiNhanMatSach]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GhiNhanMatSach]') AND type in (N'U'))
DROP TABLE [dbo].[GhiNhanMatSach]
GO
/****** Object:  Table [dbo].[DauSach]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DauSach]') AND type in (N'U'))
DROP TABLE [dbo].[DauSach]
GO
/****** Object:  Table [dbo].[ChiTietMuon]    Script Date: 23-Apr-26 11:13:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChiTietMuon]') AND type in (N'U'))
DROP TABLE [dbo].[ChiTietMuon]
GO
USE [master]
GO
/****** Object:  Database [QUANLYTHUVIEN]    Script Date: 23-Apr-26 11:13:11 PM ******/
DROP DATABASE [QUANLYTHUVIEN]
GO
/****** Object:  Database [QUANLYTHUVIEN]    Script Date: 23-Apr-26 11:13:11 PM ******/
CREATE DATABASE [QUANLYTHUVIEN]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QUANLYTHUVIEN', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\QUANLYTHUVIEN.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QUANLYTHUVIEN_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\QUANLYTHUVIEN_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QUANLYTHUVIEN] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QUANLYTHUVIEN].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QUANLYTHUVIEN] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET ARITHABORT OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET RECOVERY FULL 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET  MULTI_USER 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QUANLYTHUVIEN] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [QUANLYTHUVIEN] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [QUANLYTHUVIEN] SET QUERY_STORE = ON
GO
ALTER DATABASE [QUANLYTHUVIEN] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [QUANLYTHUVIEN]
GO
/****** Object:  Table [dbo].[ChiTietMuon]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietMuon](
	[IDChiTietMuon] [nchar](10) NOT NULL,
	[IDPhieuMuon] [nchar](10) NOT NULL,
	[IDSach] [nchar](10) NOT NULL,
	[NgayMuon] [date] NOT NULL,
	[HanTra] [date] NOT NULL,
	[NgayTra] [date] NULL,
	[TinhTrangTra] [nvarchar](50) NOT NULL,
	[TienPhat] [money] NULL,
 CONSTRAINT [PK_ChiTietMuon] PRIMARY KEY CLUSTERED 
(
	[IDChiTietMuon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DauSach]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DauSach](
	[IDDauSach] [nchar](10) NOT NULL,
	[IDTheLoai] [nchar](10) NULL,
	[TenDauSach] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_DauSach] PRIMARY KEY CLUSTERED 
(
	[IDDauSach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GhiNhanMatSach]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GhiNhanMatSach](
	[IDPhieuMatSach] [nchar](10) NOT NULL,
	[IDNguoiMuon] [nchar](10) NOT NULL,
	[IDSach] [nchar](10) NOT NULL,
	[NgayGhiNhan] [date] NOT NULL,
	[TienPhat] [money] NULL,
 CONSTRAINT [PK_GhiNhanMatSach] PRIMARY KEY CLUSTERED 
(
	[IDPhieuMatSach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoSoNhanVien]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoSoNhanVien](
	[IDNhanVien] [nchar](10) NOT NULL,
	[HoTen] [nvarchar](50) NOT NULL,
	[NgaySinh] [date] NOT NULL,
	[DiaChi] [nvarchar](50) NOT NULL,
	[DienThoai] [nvarchar](20) NOT NULL,
	[BangCap] [nvarchar](20) NOT NULL,
	[BoPhan] [nvarchar](20) NOT NULL,
	[ChucVu] [nvarchar](20) NOT NULL,
	[MatKhau] [varchar](50) NOT NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[IDNhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuMuon]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuMuon](
	[IDPhieuMuon] [nchar](10) NOT NULL,
	[IDNguoiMuon] [nchar](10) NOT NULL,
 CONSTRAINT [PK_DanhSachMuon] PRIMARY KEY CLUSTERED 
(
	[IDPhieuMuon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThanhLySach]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThanhLySach](
	[IDSach] [nchar](10) NOT NULL,
	[NgayThanhLy] [date] NOT NULL,
	[LyDoThanhLy] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_ThanhLySach] PRIMARY KEY CLUSTERED 
(
	[IDSach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TheDocGia]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TheDocGia](
	[IDDocGia] [nchar](10) NOT NULL,
	[HoTen] [nvarchar](50) NOT NULL,
	[NgaySinh] [date] NOT NULL,
	[DiaChi] [nvarchar](50) NOT NULL,
	[Email] [varchar](50) NULL,
	[NgayLap] [date] NOT NULL,
	[LoaiDocGia] [nvarchar](20) NOT NULL,
	[TienNo] [money] NOT NULL,
 CONSTRAINT [PK_DocGia] PRIMARY KEY CLUSTERED 
(
	[IDDocGia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TheLoai]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TheLoai](
	[IDTheLoai] [nchar](10) NOT NULL,
	[TenTheLoai] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_TheLoai] PRIMARY KEY CLUSTERED 
(
	[IDTheLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThongTinSach]    Script Date: 23-Apr-26 11:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThongTinSach](
	[IDSach] [nchar](10) NOT NULL,
	[TenSach] [nvarchar](50) NOT NULL,
	[TacGia] [nvarchar](50) NOT NULL,
	[NamXuatBan] [int] NOT NULL,
	[NhaXuatBan] [nvarchar](50) NOT NULL,
	[NgayNhap] [date] NOT NULL,
	[GiaBan] [money] NOT NULL,
	[GiaThue] [money] NOT NULL,
	[TinhTrang] [nvarchar](20) NOT NULL,
	[IDDauSach] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Sach] PRIMARY KEY CLUSTERED 
(
	[IDSach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[ChiTietMuon] ([IDChiTietMuon], [IDPhieuMuon], [IDSach], [NgayMuon], [HanTra], [NgayTra], [TinhTrangTra], [TienPhat]) VALUES (N'1         ', N'1         ', N'1         ', CAST(N'2026-04-23' AS Date), CAST(N'2026-04-25' AS Date), CAST(N'2026-04-25' AS Date), N'Hỏng', 1000000.0000)
GO
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS001     ', N'TL001     ', N'Mathematics')
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS002     ', N'TL001     ', N'Physics')
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS003     ', N'TL001     ', N'Chemistry')
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS004     ', N'TL001     ', N'Biology')
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS005     ', N'TL002     ', N'History')
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS006     ', N'TL002     ', N'Geography')
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS007     ', N'TL003     ', N'English')
INSERT [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES (N'DS008     ', N'TL002     ', N'Literature')
GO
INSERT [dbo].[HoSoNhanVien] ([IDNhanVien], [HoTen], [NgaySinh], [DiaChi], [DienThoai], [BangCap], [BoPhan], [ChucVu], [MatKhau]) VALUES (N'AD0001    ', N'Quản Trị Hệ Thống', CAST(N'2000-01-01' AS Date), N'Admin', N'0', N'Tiến Sĩ', N'Quản Trị', N'Giám Đốc', N'admin')
INSERT [dbo].[HoSoNhanVien] ([IDNhanVien], [HoTen], [NgaySinh], [DiaChi], [DienThoai], [BangCap], [BoPhan], [ChucVu], [MatKhau]) VALUES (N'BGD0001   ', N'Nguyễn Văn A', CAST(N'2000-01-01' AS Date), N'a', N'0', N'Tú Tài', N'Ban Giám Đốc', N'Giám Đốc', N'a')
INSERT [dbo].[HoSoNhanVien] ([IDNhanVien], [HoTen], [NgaySinh], [DiaChi], [DienThoai], [BangCap], [BoPhan], [ChucVu], [MatKhau]) VALUES (N'TK0001    ', N'Đoàn Văn C', CAST(N'2000-03-03' AS Date), N'c', N'2', N'Đại Học', N'Thủ Kho', N'Nhân Viên', N'c')
INSERT [dbo].[HoSoNhanVien] ([IDNhanVien], [HoTen], [NgaySinh], [DiaChi], [DienThoai], [BangCap], [BoPhan], [ChucVu], [MatKhau]) VALUES (N'TQ0001    ', N'Nguyễn Lê Văn D', CAST(N'2000-04-04' AS Date), N'd', N'3', N'Đại Học', N'Thủ Quỹ', N'Nhân Viên', N'd')
INSERT [dbo].[HoSoNhanVien] ([IDNhanVien], [HoTen], [NgaySinh], [DiaChi], [DienThoai], [BangCap], [BoPhan], [ChucVu], [MatKhau]) VALUES (N'TT0001    ', N'Lê Thị B', CAST(N'2000-02-02' AS Date), N'b', N'1', N'Cao Đẳng', N'Thủ Thư', N'Nhân Viên', N'b')
GO
INSERT [dbo].[PhieuMuon] ([IDPhieuMuon], [IDNguoiMuon]) VALUES (N'1         ', N'dg1       ')
GO
INSERT [dbo].[ThanhLySach] ([IDSach], [NgayThanhLy], [LyDoThanhLy]) VALUES (N'1         ', CAST(N'2026-04-23' AS Date), N'LostByUser')
GO
INSERT [dbo].[TheDocGia] ([IDDocGia], [HoTen], [NgaySinh], [DiaChi], [Email], [NgayLap], [LoaiDocGia], [TienNo]) VALUES (N'dg1       ', N'ahahahhah', CAST(N'2005-09-15' AS Date), N'a', N'a@gmail.com', CAST(N'2026-04-23' AS Date), N'Whitelist', 0.0000)
GO
INSERT [dbo].[TheLoai] ([IDTheLoai], [TenTheLoai]) VALUES (N'TL001     ', N'Khoa học tự nhiên')
INSERT [dbo].[TheLoai] ([IDTheLoai], [TenTheLoai]) VALUES (N'TL002     ', N'Khoa học xã hội')
INSERT [dbo].[TheLoai] ([IDTheLoai], [TenTheLoai]) VALUES (N'TL003     ', N'Ngoại ngữ')
GO
INSERT [dbo].[ThongTinSach] ([IDSach], [TenSach], [TacGia], [NamXuatBan], [NhaXuatBan], [NgayNhap], [GiaBan], [GiaThue], [TinhTrang], [IDDauSach]) VALUES (N'1         ', N'lalal', N'hahahaha', 2000, N'ahha', CAST(N'2026-04-23' AS Date), 5.0000, 5.0000, N'Sẵn sàng', N'DS001     ')
INSERT [dbo].[ThongTinSach] ([IDSach], [TenSach], [TacGia], [NamXuatBan], [NhaXuatBan], [NgayNhap], [GiaBan], [GiaThue], [TinhTrang], [IDDauSach]) VALUES (N'3         ', N'g', N'g', 2014, N'a', CAST(N'1995-01-02' AS Date), 4.0000, 3.0000, N'Đang mượn', N'DS002     ')
GO
ALTER TABLE [dbo].[ChiTietMuon]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietMuon_PhieuMuon] FOREIGN KEY([IDPhieuMuon])
REFERENCES [dbo].[PhieuMuon] ([IDPhieuMuon])
GO
ALTER TABLE [dbo].[ChiTietMuon] CHECK CONSTRAINT [FK_ChiTietMuon_PhieuMuon]
GO
ALTER TABLE [dbo].[ChiTietMuon]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietMuon_ThongTinSach] FOREIGN KEY([IDSach])
REFERENCES [dbo].[ThongTinSach] ([IDSach])
GO
ALTER TABLE [dbo].[ChiTietMuon] CHECK CONSTRAINT [FK_ChiTietMuon_ThongTinSach]
GO
ALTER TABLE [dbo].[GhiNhanMatSach]  WITH CHECK ADD  CONSTRAINT [FK_GhiNhanMatSach_TheDocGia] FOREIGN KEY([IDNguoiMuon])
REFERENCES [dbo].[TheDocGia] ([IDDocGia])
GO
ALTER TABLE [dbo].[GhiNhanMatSach] CHECK CONSTRAINT [FK_GhiNhanMatSach_TheDocGia]
GO
ALTER TABLE [dbo].[GhiNhanMatSach]  WITH CHECK ADD  CONSTRAINT [FK_GhiNhanMatSach_ThongTinSach] FOREIGN KEY([IDSach])
REFERENCES [dbo].[ThongTinSach] ([IDSach])
GO
ALTER TABLE [dbo].[GhiNhanMatSach] CHECK CONSTRAINT [FK_GhiNhanMatSach_ThongTinSach]
GO
ALTER TABLE [dbo].[PhieuMuon]  WITH CHECK ADD  CONSTRAINT [FK_DanhSachMuon_TheDocGia] FOREIGN KEY([IDNguoiMuon])
REFERENCES [dbo].[TheDocGia] ([IDDocGia])
GO
ALTER TABLE [dbo].[PhieuMuon] CHECK CONSTRAINT [FK_DanhSachMuon_TheDocGia]
GO
ALTER TABLE [dbo].[ThanhLySach]  WITH CHECK ADD  CONSTRAINT [FK_ThanhLySach_ThongTinSach] FOREIGN KEY([IDSach])
REFERENCES [dbo].[ThongTinSach] ([IDSach])
GO
ALTER TABLE [dbo].[ThanhLySach] CHECK CONSTRAINT [FK_ThanhLySach_ThongTinSach]
GO
ALTER TABLE [dbo].[ThongTinSach]  WITH CHECK ADD  CONSTRAINT [FK_ThongTinSach_DauSach] FOREIGN KEY([IDDauSach])
REFERENCES [dbo].[DauSach] ([IDDauSach])
GO
ALTER TABLE [dbo].[ThongTinSach] CHECK CONSTRAINT [FK_ThongTinSach_DauSach]
GO
ALTER TABLE [dbo].[HoSoNhanVien]  WITH CHECK ADD  CONSTRAINT [CK_NhanVien_BangCap] CHECK  (([BangCap]=N'Tiến Sĩ' OR [BangCap]=N'Thạc Sĩ' OR [BangCap]=N'Đại Học' OR [BangCap]=N'Cao Đẳng' OR [BangCap]=N'Trung Cấp' OR [BangCap]=N'Tú Tài'))
GO
ALTER TABLE [dbo].[HoSoNhanVien] CHECK CONSTRAINT [CK_NhanVien_BangCap]
GO
ALTER TABLE [dbo].[HoSoNhanVien]  WITH CHECK ADD  CONSTRAINT [CK_NhanVien_BoPhan] CHECK  (([BoPhan]=N'Quản Trị' OR [BoPhan]=N'Ban Giám Đốc' OR [BoPhan]=N'Thủ Quỹ' OR [BoPhan]=N'Thủ Kho' OR [BoPhan]=N'Thủ Thư'))
GO
ALTER TABLE [dbo].[HoSoNhanVien] CHECK CONSTRAINT [CK_NhanVien_BoPhan]
GO
ALTER TABLE [dbo].[HoSoNhanVien]  WITH CHECK ADD  CONSTRAINT [CK_NhanVien_ChucVu] CHECK  (([ChucVu]=N'Nhân Viên' OR [ChucVu]=N'Phó Phòng' OR [ChucVu]=N'Trưởng Phòng' OR [ChucVu]=N'Phó Giám Đốc' OR [ChucVu]=N'Giám Đốc'))
GO
ALTER TABLE [dbo].[HoSoNhanVien] CHECK CONSTRAINT [CK_NhanVien_ChucVu]
GO
ALTER TABLE [dbo].[TheDocGia]  WITH CHECK ADD  CONSTRAINT [CK_DocGia_LoaiDocGia] CHECK  (([LoaiDocGia]='Blacklist' OR [LoaiDocGia]='Whitelist' OR [LoaiDocGia]='Graylist'))
GO
ALTER TABLE [dbo].[TheDocGia] CHECK CONSTRAINT [CK_DocGia_LoaiDocGia]
GO
ALTER TABLE [dbo].[TheDocGia]  WITH CHECK ADD  CONSTRAINT [CK_DocGia_Tuoi] CHECK  ((datediff(year,[NgaySinh],getdate())>=(18) AND datediff(year,[NgaySinh],getdate())<=(55)))
GO
ALTER TABLE [dbo].[TheDocGia] CHECK CONSTRAINT [CK_DocGia_Tuoi]
GO
USE [master]
GO
ALTER DATABASE [QUANLYTHUVIEN] SET  READ_WRITE 
GO
