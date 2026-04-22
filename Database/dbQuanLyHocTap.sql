CREATE DATABASE db_QuanLySinhVien;
GO

USE db_QuanLySinhVien;
GO

CREATE TABLE tblVaiTro (
    pk_sMaVaiTro NVARCHAR(20) PRIMARY KEY,
    sTenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    sMoTa NVARCHAR(255)
);
GO

CREATE TABLE tblTaiKhoan (
    pk_sTenDangNhap NVARCHAR(50) PRIMARY KEY,
    sMatKhau NVARCHAR(255) NOT NULL,
    fk_pk_sMaVaiTro NVARCHAR(20) NOT NULL,
    bTrangThai BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (fk_pk_sMaVaiTro) REFERENCES tblVaiTro(pk_sMaVaiTro)
);
GO

CREATE TABLE tblAdmin (
    pk_sTenDangNhap NVARCHAR(50) PRIMARY KEY,
    sTenAdmin NVARCHAR(100) NOT NULL,
    sEmail NVARCHAR(100),
    sSoDienThoai NVARCHAR(15),
    FOREIGN KEY (pk_sTenDangNhap) REFERENCES tblTaiKhoan(pk_sTenDangNhap)
);
GO

CREATE TABLE tblSinhVien (
    pk_sMaSinhVien NVARCHAR(50) PRIMARY KEY,
    sTenSinhVien NVARCHAR(100) NOT NULL,
    dNgaySinh DATE,
    sGioiTinh NVARCHAR(10),
    sLopHanhChinh NVARCHAR(50),
    sEmail NVARCHAR(100),
    sSoDienThoai NVARCHAR(15),
    bTrangThai BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (pk_sMaSinhVien) REFERENCES tblTaiKhoan(pk_sTenDangNhap),
    CHECK (sGioiTinh IN (N'Nam', N'Nữ'))
);
GO

CREATE TABLE tblGiangVien (
    pk_sMaGiangVien NVARCHAR(50) PRIMARY KEY,
    sTenGiangVien NVARCHAR(100) NOT NULL,
    dNgaySinh DATE,
    sGioiTinh NVARCHAR(10),
    sEmail NVARCHAR(100),
    sSoDienThoai NVARCHAR(15),
    bTrangThai BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (pk_sMaGiangVien) REFERENCES tblTaiKhoan(pk_sTenDangNhap),
    CHECK (sGioiTinh IN (N'Nam', N'Nữ'))
);
GO

CREATE TABLE tblMonHoc (
    pk_sMaMonHoc NVARCHAR(20) PRIMARY KEY,
    sTenMonHoc NVARCHAR(100) NOT NULL,
    iSoTinChi INT NOT NULL,
    iSoTietLyThuyet INT NOT NULL DEFAULT 0,
    iSoTietThucHanh INT NOT NULL DEFAULT 0,
    bTrangThai BIT NOT NULL DEFAULT 1,
    CHECK (iSoTinChi > 0),
    CHECK (iSoTietLyThuyet >= 0),
    CHECK (iSoTietThucHanh >= 0)
);
GO

CREATE TABLE tblLopHocPhan (
    pk_sMaLopHocPhan NVARCHAR(20) PRIMARY KEY,
    fk_pk_sMaMonHoc NVARCHAR(20) NOT NULL,
    fk_pk_sMaGiangVien NVARCHAR(50) NOT NULL,

    sHocKy NVARCHAR(20) NOT NULL,
    iNamHoc INT NOT NULL,
    sPhongHoc NVARCHAR(50),

    iThu INT NOT NULL,
    iTietBatDau INT NOT NULL,
    iTietKetThuc INT NOT NULL,
    dNgayBatDau DATE NOT NULL,
    dNgayKetThuc DATE NOT NULL,
    iSiSoToiDa INT NOT NULL,
    bTrangThai BIT NOT NULL DEFAULT 1,

    FOREIGN KEY (fk_pk_sMaMonHoc) REFERENCES tblMonHoc(pk_sMaMonHoc),
    FOREIGN KEY (fk_pk_sMaGiangVien) REFERENCES tblGiangVien(pk_sMaGiangVien),

    CHECK (iThu BETWEEN 2 AND 8),
    CHECK (iTietBatDau > 0),
    CHECK (iTietKetThuc >= iTietBatDau),
    CHECK (dNgayBatDau <= dNgayKetThuc),
    CHECK (iSiSoToiDa > 0)
);
GO

CREATE TABLE tblDangKyHocPhan (
    fk_pk_sMaSinhVien NVARCHAR(50) NOT NULL,
    fk_pk_sMaLopHocPhan NVARCHAR(20) NOT NULL,
    dNgayDangKy DATETIME NOT NULL DEFAULT GETDATE(),
    sTrangThaiDangKy NVARCHAR(30) NOT NULL DEFAULT N'Đã đăng ký',
    PRIMARY KEY (fk_pk_sMaSinhVien, fk_pk_sMaLopHocPhan),
    FOREIGN KEY (fk_pk_sMaSinhVien) REFERENCES tblSinhVien(pk_sMaSinhVien),
    FOREIGN KEY (fk_pk_sMaLopHocPhan) REFERENCES tblLopHocPhan(pk_sMaLopHocPhan)
);
GO

CREATE TABLE tblDiem (
    fk_pk_sMaSinhVien NVARCHAR(50) NOT NULL,
    fk_pk_sMaLopHocPhan NVARCHAR(20) NOT NULL,
    fDiemChuyenCan FLOAT,
    fDiemGiuaKy FLOAT,
    fDiemCuoiKy FLOAT,
    fDiemTongKet FLOAT,
    dNgayNhapDiem DATETIME NOT NULL DEFAULT GETDATE(),
    dNgayCapNhat DATETIME,
    PRIMARY KEY (fk_pk_sMaSinhVien, fk_pk_sMaLopHocPhan),
    FOREIGN KEY (fk_pk_sMaSinhVien, fk_pk_sMaLopHocPhan)
        REFERENCES tblDangKyHocPhan(fk_pk_sMaSinhVien, fk_pk_sMaLopHocPhan),
    CHECK (fDiemChuyenCan IS NULL OR fDiemChuyenCan BETWEEN 0 AND 10),
    CHECK (fDiemGiuaKy IS NULL OR fDiemGiuaKy BETWEEN 0 AND 10),
    CHECK (fDiemCuoiKy IS NULL OR fDiemCuoiKy BETWEEN 0 AND 10),
    CHECK (fDiemTongKet IS NULL OR fDiemTongKet BETWEEN 0 AND 10)
);
GO

CREATE TABLE tblLichThi (
    pk_sMaLichThi NVARCHAR(20) PRIMARY KEY,
    fk_pk_sMaLopHocPhan NVARCHAR(20) NOT NULL,
    dNgayThi DATE NOT NULL,
    tGioBatDau TIME NOT NULL,
    tGioKetThuc TIME NOT NULL,
    sPhongThi NVARCHAR(50),
    sHinhThucThi NVARCHAR(50),
    sGhiChu NVARCHAR(255),
    FOREIGN KEY (fk_pk_sMaLopHocPhan) REFERENCES tblLopHocPhan(pk_sMaLopHocPhan),
    CHECK (tGioBatDau < tGioKetThuc)
);
GO
--Produce
--Dang nhap
CREATE PROC sp_Login
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255)
AS
BEGIN
    SELECT tk.pk_sTenDangNhap,
           tk.fk_pk_sMaVaiTro,
           vt.sTenVaiTro,
           ISNULL(ad.sTenAdmin, ISNULL(sv.sTenSinhVien, gv.sTenGiangVien)) AS HoTen
    FROM tblTaiKhoan tk
    INNER JOIN tblVaiTro vt ON tk.fk_pk_sMaVaiTro = vt.pk_sMaVaiTro
    LEFT JOIN tblAdmin ad ON tk.pk_sTenDangNhap = ad.pk_sTenDangNhap
    LEFT JOIN tblSinhVien sv ON tk.pk_sTenDangNhap = sv.pk_sMaSinhVien
    LEFT JOIN tblGiangVien gv ON tk.pk_sTenDangNhap = gv.pk_sMaGiangVien
    WHERE tk.pk_sTenDangNhap = @TenDangNhap
      AND tk.sMatKhau = @MatKhau
      AND tk.bTrangThai = 1
END
--Themsv
CREATE PROC sp_ThemSinhVien
    @MaSinhVien NVARCHAR(50),
    @TenSinhVien NVARCHAR(100),
    @NgaySinh DATE,
    @GioiTinh NVARCHAR(10),
    @Lop NVARCHAR(50),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15),
    @MatKhau NVARCHAR(255)
AS
BEGIN
    INSERT INTO tblTaiKhoan
    VALUES (@MaSinhVien, @MatKhau, 'SV', 1)

    INSERT INTO tblSinhVien
    VALUES (@MaSinhVien, @TenSinhVien, @NgaySinh, @GioiTinh, @Lop, @Email, @SDT, 1)
END
--themHP
CREATE PROC sp_ThemLopHocPhan
    @MaLHP NVARCHAR(20),
    @MaMon NVARCHAR(20),
    @MaGV NVARCHAR(50),
    @HocKy NVARCHAR(20),
    @NamHoc INT,
    @Phong NVARCHAR(50),
    @Thu INT,
    @TietBD INT,
    @TietKT INT,
    @NgayBD DATE,
    @NgayKT DATE,
    @SiSo INT
AS
BEGIN
    INSERT INTO tblLopHocPhan
    VALUES (@MaLHP, @MaMon, @MaGV, @HocKy, @NamHoc,
            @Phong, @Thu, @TietBD, @TietKT,
            @NgayBD, @NgayKT, @SiSo, 1)
END
--nhapdiem
CREATE PROC sp_LuuDiem
    @MaSV NVARCHAR(50),
    @MaLHP NVARCHAR(20),
    @CC FLOAT,
    @GK FLOAT,
    @CK FLOAT
AS
BEGIN
    DECLARE @TK FLOAT
    SET @TK = @CC*0.1 + @GK*0.3 + @CK*0.6

    IF EXISTS (SELECT 1 FROM tblDiem WHERE fk_pk_sMaSinhVien=@MaSV AND fk_pk_sMaLopHocPhan=@MaLHP)
    BEGIN
        UPDATE tblDiem
        SET fDiemChuyenCan=@CC,
            fDiemGiuaKy=@GK,
            fDiemCuoiKy=@CK,
            fDiemTongKet=@TK
        WHERE fk_pk_sMaSinhVien=@MaSV AND fk_pk_sMaLopHocPhan=@MaLHP
    END
    ELSE
    BEGIN
        INSERT INTO tblDiem
        VALUES (@MaSV, @MaLHP, @CC, @GK, @CK, @TK, GETDATE(), NULL)
    END
END
--Dangkyhp
ALTER PROC sp_DangKyHocPhan_CheckSiSo
    @MaSV NVARCHAR(50),
    @MaLHP NVARCHAR(20)
AS
BEGIN
    DECLARE @SiSoToiDa INT
    DECLARE @SoLuongDaDangKy INT
    DECLARE @MaMonHoc NVARCHAR(20)

    -- Lấy mã môn của lớp học phần đang chọn
    SELECT 
        @SiSoToiDa = iSiSoToiDa,
        @MaMonHoc = fk_pk_sMaMonHoc
    FROM tblLopHocPhan
    WHERE pk_sMaLopHocPhan = @MaLHP

    IF @SiSoToiDa IS NULL
    BEGIN
        RAISERROR(N'Lớp học phần không tồn tại', 16, 1)
        RETURN
    END

    -- Kiểm tra đã đăng ký đúng lớp học phần này chưa
    IF EXISTS (
        SELECT 1
        FROM tblDangKyHocPhan
        WHERE fk_pk_sMaSinhVien = @MaSV
          AND fk_pk_sMaLopHocPhan = @MaLHP
    )
    BEGIN
        RAISERROR(N'Sinh viên đã đăng ký lớp học phần này rồi', 16, 1)
        RETURN
    END

    -- Kiểm tra trùng mã môn
    IF EXISTS (
        SELECT 1
        FROM tblDangKyHocPhan dk
        INNER JOIN tblLopHocPhan lhp
            ON dk.fk_pk_sMaLopHocPhan = lhp.pk_sMaLopHocPhan
        WHERE dk.fk_pk_sMaSinhVien = @MaSV
          AND lhp.fk_pk_sMaMonHoc = @MaMonHoc
    )
    BEGIN
        RAISERROR(N'Sinh viên đã đăng ký môn học này rồi', 16, 1)
        RETURN
    END

    -- Kiểm tra sĩ số
    SELECT @SoLuongDaDangKy = COUNT(*)
    FROM tblDangKyHocPhan
    WHERE fk_pk_sMaLopHocPhan = @MaLHP

    IF @SoLuongDaDangKy >= @SiSoToiDa
    BEGIN
        RAISERROR(N'Lớp học phần đã đủ sĩ số', 16, 1)
        RETURN
    END

    -- Đăng ký
    INSERT INTO tblDangKyHocPhan
    VALUES (@MaSV, @MaLHP, GETDATE(), N'Đã đăng ký')
END
GO
--Hienthidshp
CREATE PROC sp_GetLopHocPhanConLai
AS
BEGIN
    SELECT 
        lhp.pk_sMaLopHocPhan,
        mh.sTenMonHoc,
        gv.sTenGiangVien,
        lhp.sHocKy,
        lhp.iNamHoc,
        lhp.iThu,
        lhp.iTietBatDau,
        lhp.iTietKetThuc,
        lhp.sPhongHoc,
        lhp.iSiSoToiDa,
        COUNT(dk.fk_pk_sMaSinhVien) AS SoLuongDaDangKy,
        (lhp.iSiSoToiDa - COUNT(dk.fk_pk_sMaSinhVien)) AS SiSoConLai
    FROM tblLopHocPhan lhp
    INNER JOIN tblMonHoc mh 
        ON lhp.fk_pk_sMaMonHoc = mh.pk_sMaMonHoc
    INNER JOIN tblGiangVien gv 
        ON lhp.fk_pk_sMaGiangVien = gv.pk_sMaGiangVien
    LEFT JOIN tblDangKyHocPhan dk 
        ON lhp.pk_sMaLopHocPhan = dk.fk_pk_sMaLopHocPhan
    WHERE lhp.bTrangThai = 1
    GROUP BY 
        lhp.pk_sMaLopHocPhan,
        mh.sTenMonHoc,
        gv.sTenGiangVien,
        lhp.sHocKy,
        lhp.iNamHoc,
        lhp.iThu,
        lhp.iTietBatDau,
        lhp.iTietKetThuc,
        lhp.sPhongHoc,
        lhp.iSiSoToiDa
    ORDER BY lhp.pk_sMaLopHocPhan
END
GO
--Xemdiem
CREATE PROC sp_GetDiemSinhVien
    @MaSV NVARCHAR(50)
AS
BEGIN
    SELECT d.fk_pk_sMaSinhVien,
           d.fk_pk_sMaLopHocPhan,
           mh.sTenMonHoc,
           d.fDiemChuyenCan,
           d.fDiemGiuaKy,
           d.fDiemCuoiKy,
           d.fDiemTongKet
    FROM tblDiem d
    JOIN tblLopHocPhan lhp ON d.fk_pk_sMaLopHocPhan = lhp.pk_sMaLopHocPhan
    JOIN tblMonHoc mh ON lhp.fk_pk_sMaMonHoc = mh.pk_sMaMonHoc
    WHERE d.fk_pk_sMaSinhVien = @MaSV
END
--xemlichhoc
ALTER PROC sp_GetLichHoc
    @MaSV NVARCHAR(50)
AS
BEGIN
    SELECT 
        lhp.pk_sMaLopHocPhan,
        mh.sTenMonHoc,
        gv.sTenGiangVien,
        lhp.sHocKy,         
        lhp.iNamHoc,        
        lhp.iThu,
        lhp.iTietBatDau,
        lhp.iTietKetThuc,
        lhp.sPhongHoc,
        lhp.dNgayBatDau,
        lhp.dNgayKetThuc
    FROM tblDangKyHocPhan dk
    JOIN tblLopHocPhan lhp 
        ON dk.fk_pk_sMaLopHocPhan = lhp.pk_sMaLopHocPhan
    JOIN tblMonHoc mh 
        ON lhp.fk_pk_sMaMonHoc = mh.pk_sMaMonHoc
    JOIN tblGiangVien gv 
        ON lhp.fk_pk_sMaGiangVien = gv.pk_sMaGiangVien
    WHERE dk.fk_pk_sMaSinhVien = @MaSV
END
GO
--Dssvlop
CREATE PROC sp_GetSinhVienTrongLop
    @MaLHP NVARCHAR(20)
AS
BEGIN
    SELECT sv.pk_sMaSinhVien,
           sv.sTenSinhVien
    FROM tblDangKyHocPhan dk
    JOIN tblSinhVien sv ON dk.fk_pk_sMaSinhVien = sv.pk_sMaSinhVien
    WHERE dk.fk_pk_sMaLopHocPhan = @MaLHP
END
--dssv khoa
CREATE PROC sp_GetDanhSachSinhVien
AS
BEGIN
    SELECT pk_sMaSinhVien,
           sTenSinhVien,
           dNgaySinh,
           sGioiTinh,
           sLopHanhChinh,
           sEmail,
           sSoDienThoai
    FROM tblSinhVien
    ORDER BY pk_sMaSinhVien
END
--LopHP
CREATE PROC sp_GetDanhSachLopHocPhan
AS
BEGIN
    SELECT pk_sMaLopHocPhan,
           fk_pk_sMaMonHoc,
           fk_pk_sMaGiangVien,
           sHocKy,
           iNamHoc,
           sPhongHoc,
           iThu,
           iTietBatDau,
           iTietKetThuc,
           dNgayBatDau,
           dNgayKetThuc,
           iSiSoToiDa
    FROM tblLopHocPhan
    ORDER BY pk_sMaLopHocPhan
END
--DsMonHoc
CREATE PROC sp_GetDanhSachMonHoc
AS
BEGIN
    SELECT pk_sMaMonHoc,
           sTenMonHoc,
           iSoTinChi,
           iSoTietLyThuyet,
           iSoTietThucHanh,
           bTrangThai
    FROM tblMonHoc
    ORDER BY pk_sMaMonHoc
END
GO
--XoaLHP
ALTER PROC sp_XoaLopHocPhan
    @MaLHP NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM tblDangKyHocPhan
    WHERE fk_pk_sMaLopHocPhan = @MaLHP;

    DELETE FROM tblLopHocPhan
    WHERE pk_sMaLopHocPhan = @MaLHP;
END
GO
--XoaSV
ALTER PROC sp_XoaSinhVien
    @MaSV NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM tblDangKyHocPhan
    WHERE fk_pk_sMaSinhVien = @MaSV;

    DELETE FROM tblSinhVien
    WHERE pk_sMaSinhVien = @MaSV;

    DELETE FROM tblTaiKhoan
    WHERE pk_sTenDangNhap = @MaSV;
END
GO
--Layttsv
CREATE PROC sp_GetSinhVienById
    @MaSV NVARCHAR(50)
AS
BEGIN
    SELECT *
    FROM tblSinhVien
    WHERE pk_sMaSinhVien = @MaSV
END
--Laydiemsv
CREATE PROC sp_GetSinhVienVaDiemTrongLop
    @MaLHP NVARCHAR(20)
AS
BEGIN
    SELECT 
        sv.pk_sMaSinhVien,
        sv.sTenSinhVien,
        d.fDiemChuyenCan,
        d.fDiemGiuaKy,
        d.fDiemCuoiKy,
        d.fDiemTongKet
    FROM tblDangKyHocPhan dk
    INNER JOIN tblSinhVien sv
        ON dk.fk_pk_sMaSinhVien = sv.pk_sMaSinhVien
    LEFT JOIN tblDiem d
        ON dk.fk_pk_sMaSinhVien = d.fk_pk_sMaSinhVien
        AND dk.fk_pk_sMaLopHocPhan = d.fk_pk_sMaLopHocPhan
    WHERE dk.fk_pk_sMaLopHocPhan = @MaLHP
END
--Laydshpnhapdiem
CREATE PROC sp_GetDanhSachLopHocPhanNhapDiem
AS
BEGIN
    SELECT lhp.pk_sMaLopHocPhan,
           mh.sTenMonHoc
    FROM tblLopHocPhan lhp
    INNER JOIN tblMonHoc mh ON lhp.fk_pk_sMaMonHoc = mh.pk_sMaMonHoc
    ORDER BY lhp.pk_sMaLopHocPhan
END
--Laydssdedd
CREATE PROC sp_GetDanhSachSinhVienTheoLopHocPhan
    @MaLHP NVARCHAR(20)
AS
BEGIN
    SELECT sv.pk_sMaSinhVien,
           sv.sTenSinhVien,
           sv.dNgaySinh
    FROM tblDangKyHocPhan dk
    INNER JOIN tblSinhVien sv
        ON dk.fk_pk_sMaSinhVien = sv.pk_sMaSinhVien
    WHERE dk.fk_pk_sMaLopHocPhan = @MaLHP
    ORDER BY sv.pk_sMaSinhVien
END
--LaydsGiangday
CREATE PROC sp_GetLichGiangDayTheoGiangVien
    @MaGV NVARCHAR(50)
AS
BEGIN
    SELECT lhp.pk_sMaLopHocPhan,
           mh.sTenMonHoc,
           gv.sTenGiangVien,
           lhp.sHocKy,
           lhp.iNamHoc,
           lhp.iThu,
           lhp.iTietBatDau,
           lhp.iTietKetThuc,
           lhp.sPhongHoc,
           lhp.dNgayBatDau,
           lhp.dNgayKetThuc
    FROM tblLopHocPhan lhp
    INNER JOIN tblMonHoc mh ON lhp.fk_pk_sMaMonHoc = mh.pk_sMaMonHoc
    INNER JOIN tblGiangVien gv ON lhp.fk_pk_sMaGiangVien = gv.pk_sMaGiangVien
    WHERE lhp.bTrangThai = 1
      AND lhp.fk_pk_sMaGiangVien = @MaGV
    ORDER BY lhp.iNamHoc DESC, lhp.sHocKy, lhp.iThu, lhp.iTietBatDau
END
GO
--DSGV
CREATE PROC sp_GetDanhSachGiangVien
AS
BEGIN
    SELECT 
        pk_sMaGiangVien,
        sTenGiangVien,
        dNgaySinh,
        sGioiTinh,
        sEmail,
        sSoDienThoai,
        bTrangThai
    FROM tblGiangVien
    ORDER BY pk_sMaGiangVien
END
GO
--Lichhoctheotuan
ALTER PROCEDURE sp_GetLichHocTheoTuan
    @MaSV NVARCHAR(50),
    @Tuan INT,
    @Nam INT
AS
BEGIN
    DECLARE @TuNgay DATE, @DenNgay DATE;

    ;WITH DanhSachTuan AS
    (
        SELECT 
            1 AS WeekNumber,
            CAST(CAST(@Nam AS VARCHAR(4)) + '-01-01' AS DATE) AS StartDate,
            DATEADD(DAY, 6, CAST(CAST(@Nam AS VARCHAR(4)) + '-01-01' AS DATE)) AS EndDate
        UNION ALL
        SELECT
            WeekNumber + 1,
            DATEADD(WEEK, 1, StartDate),
            DATEADD(DAY, 6, DATEADD(WEEK, 1, StartDate))
        FROM DanhSachTuan
        WHERE DATEADD(WEEK, 1, StartDate) <= CAST(CAST(@Nam AS VARCHAR(4)) + '-12-31' AS DATE)
    )
    SELECT 
        @TuNgay = StartDate,
        @DenNgay = CASE 
                     WHEN EndDate > CAST(CAST(@Nam AS VARCHAR(4)) + '-12-31' AS DATE)
                     THEN CAST(CAST(@Nam AS VARCHAR(4)) + '-12-31' AS DATE)
                     ELSE EndDate
                   END
    FROM DanhSachTuan
    WHERE WeekNumber = @Tuan
    OPTION (MAXRECURSION 366);

    SELECT 
        lhp.pk_sMaLopHocPhan,
        mh.sTenMonHoc,
        gv.sTenGiangVien,
        lhp.sHocKy,
        lhp.iNamHoc,
        lhp.iThu,
        lhp.iTietBatDau,
        lhp.iTietKetThuc,
        lhp.sPhongHoc,
        lhp.dNgayBatDau,
        lhp.dNgayKetThuc
    FROM tblDangKyHocPhan dk
    JOIN tblLopHocPhan lhp 
        ON dk.fk_pk_sMaLopHocPhan = lhp.pk_sMaLopHocPhan
    JOIN tblMonHoc mh 
        ON lhp.fk_pk_sMaMonHoc = mh.pk_sMaMonHoc
    JOIN tblGiangVien gv 
        ON lhp.fk_pk_sMaGiangVien = gv.pk_sMaGiangVien
    WHERE dk.fk_pk_sMaSinhVien = @MaSV
      AND lhp.dNgayBatDau <= @DenNgay
      AND lhp.dNgayKetThuc >= @TuNgay
    ORDER BY lhp.iThu, lhp.iTietBatDau
END
--LaydsTuan
CREATE PROCEDURE sp_GetDanhSachTuanTheoNam
    @Nam INT   -- Nhận năm học để tính tuần học
AS
BEGIN
    DECLARE @StartDate DATE = CAST(@Nam AS VARCHAR) + '-01-01';  -- Ngày bắt đầu năm (1 tháng 1)
    DECLARE @EndDate DATE = CAST(@Nam AS VARCHAR) + '-12-31';    -- Ngày kết thúc năm (31 tháng 12)
    DECLARE @WeekNumber INT = 1;
    DECLARE @CurrentDate DATE;

    -- Tạo bảng tạm để lưu các tuần học
    CREATE TABLE #Weeks (WeekNumber INT, StartDate DATE, EndDate DATE);

    -- Tính toán các tuần học
    SET @CurrentDate = @StartDate;
    WHILE @CurrentDate <= @EndDate
    BEGIN
        DECLARE @EndOfWeek DATE = DATEADD(DAY, 6, @CurrentDate);  -- Ngày kết thúc tuần (6 ngày sau ngày bắt đầu)
        
        IF @EndOfWeek > @EndDate
        BEGIN
            SET @EndOfWeek = @EndDate;  -- Nếu ngày kết thúc tuần vượt quá ngày kết thúc năm, lấy ngày kết thúc năm
        END

        -- Thêm tuần vào bảng tạm
        INSERT INTO #Weeks (WeekNumber, StartDate, EndDate)
        VALUES (@WeekNumber, @CurrentDate, @EndOfWeek);

        -- Tiến đến tuần tiếp theo
        SET @CurrentDate = DATEADD(WEEK, 1, @CurrentDate);
        SET @WeekNumber = @WeekNumber + 1;
    END

    -- Trả về danh sách các tuần học
    SELECT * FROM #Weeks ORDER BY WeekNumber;

    -- Xóa bảng tạm
    DROP TABLE #Weeks;
END
INSERT INTO tblVaiTro VALUES ('AD', N'Admin', N'Quản trị hệ thống')
INSERT INTO tblVaiTro VALUES ('GV', N'Giảng viên', N'Giảng dạy')
INSERT INTO tblVaiTro VALUES ('SV', N'Sinh viên', N'Học tập')
-- Tài khoản
INSERT INTO tblTaiKhoan VALUES ('Admin', '123', 'AD', 1)

-- Thông tin admin
INSERT INTO tblAdmin VALUES ('Admin', N'Quản trị', 'admin@gmail.com', '0123456789')
-- Tài khoản GV
INSERT INTO tblTaiKhoan VALUES ('GV01', '123', 'GV', 1)
INSERT INTO tblTaiKhoan VALUES ('GV02', '123', 'GV', 1)
INSERT INTO tblTaiKhoan VALUES ('GV03', '123', 'GV', 1)
INSERT INTO tblTaiKhoan VALUES ('GV04', '123', 'GV', 1)
INSERT INTO tblTaiKhoan VALUES ('GV05', '123', 'GV', 1)

-- Thông tin GV
INSERT INTO tblGiangVien VALUES ('GV01', N'Nguyễn Văn A', '1980-01-01', N'Nam', 'a@gmail.com', '0900000001', 1)
INSERT INTO tblGiangVien VALUES ('GV02', N'Trần Văn B', '1982-02-02', N'Nam', 'b@gmail.com', '0900000002', 1)
INSERT INTO tblGiangVien VALUES ('GV03', N'Lê Thị C', '1985-03-03', N'Nữ', 'c@gmail.com', '0900000003', 1)
INSERT INTO tblGiangVien VALUES ('GV04', N'Phạm Văn D', '1983-04-04', N'Nam', 'd@gmail.com', '0900000004', 1)
INSERT INTO tblGiangVien VALUES ('GV05', N'Hoàng Thị E', '1986-05-05', N'Nữ', 'e@gmail.com', '0900000005', 1)
--Monhoc
INSERT INTO tblMonHoc VALUES ('MH01', N'Lập trình C', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH02', N'Lập trình Java', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH03', N'Cơ sở dữ liệu', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH04', N'Cấu trúc dữ liệu', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH05', N'Hệ điều hành', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH06', N'Mạng máy tính', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH07', N'Lập trình Web', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH08', N'Phân tích thiết kế HTTT', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH09', N'An toàn thông tin', 3, 30, 15, 1)
INSERT INTO tblMonHoc VALUES ('MH10', N'Trí tuệ nhân tạo', 3, 30, 15, 1)
select * from tblLopHocPhan
EXEC sp_GetLichHocTheoTuan @MaSV = 'SV001', @Tuan = 1, @Nam = 2026