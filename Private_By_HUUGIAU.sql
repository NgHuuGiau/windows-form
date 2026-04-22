USE [master]
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QUANLYTHUVIEN')
BEGIN
    ALTER DATABASE [QUANLYTHUVIEN] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE [QUANLYTHUVIEN]
END
GO

CREATE DATABASE [QUANLYTHUVIEN]
GO

USE [QUANLYTHUVIEN]
GO

-- Tables
CREATE TABLE [dbo].[ChiTietMuon](
	[IDChiTietMuon] [nchar](10) NOT NULL,
	[IDPhieuMuon] [nchar](10) NOT NULL,
	[IDSach] [nchar](10) NOT NULL,
	[NgayMuon] [date] NOT NULL,
	[HanTra] [date] NOT NULL,
	[NgayTra] [date] NULL,
	[TinhTrangTra] [nvarchar](50) NOT NULL,
	[TienPhat] [money] NULL,
 CONSTRAINT [PK_ChiTietMuon] PRIMARY KEY CLUSTERED ([IDChiTietMuon] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DauSach](
	[IDDauSach] [nchar](10) NOT NULL,
	[IDTheLoai] [nchar](10) NOT NULL,
	[TenDauSach] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_DauSach] PRIMARY KEY CLUSTERED ([IDDauSach] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[GhiNhanMatSach](
	[IDPhieuMatSach] [nchar](10) NOT NULL,
	[IDNguoiMuon] [nchar](10) NOT NULL,
	[IDSach] [nchar](10) NOT NULL,
	[NgayGhiNhan] [date] NOT NULL,
	[TienPhat] [money] NULL,
 CONSTRAINT [PK_GhiNhanMatSach] PRIMARY KEY CLUSTERED ([IDPhieuMatSach] ASC)
) ON [PRIMARY]
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
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED ([IDNhanVien] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PhieuMuon](
	[IDPhieuMuon] [nchar](10) NOT NULL,
	[IDNguoiMuon] [nchar](10) NOT NULL,
 CONSTRAINT [PK_DanhSachMuon] PRIMARY KEY CLUSTERED ([IDPhieuMuon] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ThanhLySach](
	[IDSach] [nchar](10) NOT NULL,
	[NgayThanhLy] [date] NOT NULL,
	[LyDoThanhLy] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_ThanhLySach] PRIMARY KEY CLUSTERED ([IDSach] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TheDocGia](
	[IDDocGia] [nchar](10) NOT NULL,
	[HoTen] [nvarchar](50) NOT NULL,
	[NgaySinh] [date] NOT NULL,
	[DiaChi] [nvarchar](50) NOT NULL,
	[Email] [varchar](50) NULL,
	[NgayLap] [date] NOT NULL,
	[LoaiDocGia] [char](1) NOT NULL,
	[TienNo] [money] NOT NULL,
 CONSTRAINT [PK_DocGia] PRIMARY KEY CLUSTERED ([IDDocGia] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TheLoai](
	[IDTheLoai] [nchar](10) NOT NULL,
	[TenTheLoai] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_TheLoai] PRIMARY KEY CLUSTERED ([IDTheLoai] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ThongTinSach](
	[IDSach] [nchar](10) NOT NULL,
	[TenSach] [nvarchar](50) NOT NULL,
	[TacGia] [nvarchar](50) NOT NULL,
	[NamXuatBan] [int] NOT NULL,
	[NhaXuatBan] [nvarchar](50) NOT NULL,
	[NgayNhap] [date] NOT NULL,
	[TriGia] [money] NOT NULL,
	[GiaThue] [money] NOT NULL DEFAULT ((0)),
	[TinhTrang] [nvarchar](20) NOT NULL,
	[IDDauSach] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Sach] PRIMARY KEY CLUSTERED ([IDSach] ASC)
) ON [PRIMARY]
GO

-- Constraints (Foreign Keys)
ALTER TABLE [dbo].[ChiTietMuon] WITH CHECK ADD CONSTRAINT [FK_ChiTietMuon_PhieuMuon] FOREIGN KEY([IDPhieuMuon]) REFERENCES [dbo].[PhieuMuon] ([IDPhieuMuon])
GO
ALTER TABLE [dbo].[ChiTietMuon] CHECK CONSTRAINT [FK_ChiTietMuon_PhieuMuon]
GO
ALTER TABLE [dbo].[ChiTietMuon] WITH CHECK ADD CONSTRAINT [FK_ChiTietMuon_ThongTinSach] FOREIGN KEY([IDSach]) REFERENCES [dbo].[ThongTinSach] ([IDSach])
GO
ALTER TABLE [dbo].[ChiTietMuon] CHECK CONSTRAINT [FK_ChiTietMuon_ThongTinSach]
GO

ALTER TABLE [dbo].[DauSach] WITH CHECK ADD CONSTRAINT [FK_DauSach_TheLoai] FOREIGN KEY([IDTheLoai]) REFERENCES [dbo].[TheLoai] ([IDTheLoai])
GO
ALTER TABLE [dbo].[DauSach] CHECK CONSTRAINT [FK_DauSach_TheLoai]
GO

ALTER TABLE [dbo].[GhiNhanMatSach] WITH CHECK ADD CONSTRAINT [FK_GhiNhanMatSach_TheDocGia] FOREIGN KEY([IDNguoiMuon]) REFERENCES [dbo].[TheDocGia] ([IDDocGia])
GO
ALTER TABLE [dbo].[GhiNhanMatSach] CHECK CONSTRAINT [FK_GhiNhanMatSach_TheDocGia]
GO
ALTER TABLE [dbo].[GhiNhanMatSach] WITH CHECK ADD CONSTRAINT [FK_GhiNhanMatSach_ThongTinSach] FOREIGN KEY([IDSach]) REFERENCES [dbo].[ThongTinSach] ([IDSach])
GO
ALTER TABLE [dbo].[GhiNhanMatSach] CHECK CONSTRAINT [FK_GhiNhanMatSach_ThongTinSach]
GO

ALTER TABLE [dbo].[PhieuMuon] WITH CHECK ADD CONSTRAINT [FK_DanhSachMuon_TheDocGia] FOREIGN KEY([IDNguoiMuon]) REFERENCES [dbo].[TheDocGia] ([IDDocGia])
GO
ALTER TABLE [dbo].[PhieuMuon] CHECK CONSTRAINT [FK_DanhSachMuon_TheDocGia]
GO

ALTER TABLE [dbo].[ThanhLySach] WITH CHECK ADD CONSTRAINT [FK_ThanhLySach_ThongTinSach] FOREIGN KEY([IDSach]) REFERENCES [dbo].[ThongTinSach] ([IDSach])
GO
ALTER TABLE [dbo].[ThanhLySach] CHECK CONSTRAINT [FK_ThanhLySach_ThongTinSach]
GO

ALTER TABLE [dbo].[ThongTinSach] WITH CHECK ADD CONSTRAINT [FK_ThongTinSach_DauSach] FOREIGN KEY([IDDauSach]) REFERENCES [dbo].[DauSach] ([IDDauSach])
GO
ALTER TABLE [dbo].[ThongTinSach] CHECK CONSTRAINT [FK_ThongTinSach_DauSach]
GO

-- Constraints (CHECK)
ALTER TABLE [dbo].[HoSoNhanVien] WITH CHECK ADD CONSTRAINT [CK_NhanVien_BangCap] CHECK (([BangCap]=N'Tiến Sĩ' OR [BangCap]=N'Thạc Sĩ' OR [BangCap]=N'Đại Học' OR [BangCap]=N'Cao Đẳng' OR [BangCap]=N'Trung Cấp' OR [BangCap]=N'Tú Tài'))
GO
ALTER TABLE [dbo].[HoSoNhanVien] CHECK CONSTRAINT [CK_NhanVien_BangCap]
GO

ALTER TABLE [dbo].[HoSoNhanVien] WITH CHECK ADD CONSTRAINT [CK_NhanVien_BoPhan] CHECK (([BoPhan]=N'Ban Giám Đốc' OR [BoPhan]=N'Thủ Quỹ' OR [BoPhan]=N'Thủ Kho' OR [BoPhan]=N'Thủ Thư'))
GO
ALTER TABLE [dbo].[HoSoNhanVien] CHECK CONSTRAINT [CK_NhanVien_BoPhan]
GO

ALTER TABLE [dbo].[HoSoNhanVien] WITH CHECK ADD CONSTRAINT [CK_NhanVien_ChucVu] CHECK (([ChucVu]=N'Nhân Viên' OR [ChucVu]=N'Phó Phòng' OR [ChucVu]=N'Trưởng Phòng' OR [ChucVu]=N'Phó Giám Đốc' OR [ChucVu]=N'Giám Đốc'))
GO
ALTER TABLE [dbo].[HoSoNhanVien] CHECK CONSTRAINT [CK_NhanVien_ChucVu]
GO

ALTER TABLE [dbo].[ThanhLySach] WITH CHECK ADD CONSTRAINT [CHK_ThanhLySach_LyDoThanhLy] CHECK (([LyDoThanhLy]=N'Người Dùng Làm Mất' OR [LyDoThanhLy]=N'Hư Hỏng' OR [LyDoThanhLy]=N'Mất'))
GO
ALTER TABLE [dbo].[ThanhLySach] CHECK CONSTRAINT [CHK_ThanhLySach_LyDoThanhLy]
GO

ALTER TABLE [dbo].[TheDocGia] WITH CHECK ADD CONSTRAINT [CK_DocGia_LoaiDocGia] CHECK (([LoaiDocGia]='Y' OR [LoaiDocGia]='X'))
GO
ALTER TABLE [dbo].[TheDocGia] CHECK CONSTRAINT [CK_DocGia_LoaiDocGia]
GO

ALTER TABLE [dbo].[TheDocGia] WITH CHECK ADD CONSTRAINT [CK_DocGia_Tuoi] CHECK ((datediff(year,[NgaySinh],getdate())>=(18) AND datediff(year,[NgaySinh],getdate())<=(55)))
GO
ALTER TABLE [dbo].[TheDocGia] CHECK CONSTRAINT [CK_DocGia_Tuoi]
GO

ALTER TABLE [dbo].[TheLoai] WITH CHECK ADD CONSTRAINT [CK_TheLoai] CHECK (([TenTheLoai]='C' OR [TenTheLoai]='B' OR [TenTheLoai]='A'))
GO
ALTER TABLE [dbo].[TheLoai] CHECK CONSTRAINT [CK_TheLoai]
GO

ALTER TABLE [dbo].[ThongTinSach] WITH CHECK ADD CONSTRAINT [CK_Sach_NamXuatBan] CHECK (([NamXuatBan]>=(datepart(year,getdate())-(8))))
GO
ALTER TABLE [dbo].[ThongTinSach] CHECK CONSTRAINT [CK_Sach_NamXuatBan]
GO

-- INITIAL DATA
INSERT INTO [dbo].[TheLoai] ([IDTheLoai], [TenTheLoai]) VALUES ('TL001', N'A');
INSERT INTO [dbo].[TheLoai] ([IDTheLoai], [TenTheLoai]) VALUES ('TL002', N'B');
INSERT INTO [dbo].[TheLoai] ([IDTheLoai], [TenTheLoai]) VALUES ('TL003', N'C');
GO

INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS001', 'TL001', N'Mathematics');
INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS002', 'TL002', N'Physics');
INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS003', 'TL003', N'Chemistry');
INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS004', 'TL001', N'Biology');
INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS005', 'TL002', N'History');
INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS006', 'TL003', N'Geography');
INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS007', 'TL001', N'English');
INSERT INTO [dbo].[DauSach] ([IDDauSach], [IDTheLoai], [TenDauSach]) VALUES ('DS008', 'TL002', N'Literature');
GO