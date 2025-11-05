-- =============================================
-- UPDATE DATABASE - Chuyển từ Photo (binary) sang PhotoPath (string)
-- =============================================

USE QuanLyNhanSu;
GO

PRINT 'Bắt đầu update database...';
PRINT '';

-- Kiểm tra xem cột Photo có tồn tại không
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Employees' AND COLUMN_NAME = 'Photo')
BEGIN
    PRINT 'Tìm thấy cột Photo (cũ) - Đang chuyển đổi...';

    -- Thêm cột PhotoPath mới
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Employees' AND COLUMN_NAME = 'PhotoPath')
    BEGIN
        ALTER TABLE Employees ADD PhotoPath NVARCHAR(500) NULL;
        PRINT '✓ Đã thêm cột PhotoPath';
    END
    ELSE
    BEGIN
        PRINT '✓ Cột PhotoPath đã tồn tại';
    END

    -- Xóa cột Photo cũ
    ALTER TABLE Employees DROP COLUMN Photo;
    PRINT '✓ Đã xóa cột Photo (binary)';

    PRINT '';
    PRINT '✓✓✓ CẬP NHẬT THÀNH CÔNG! ✓✓✓';
    PRINT 'Database đã được chuyển đổi để lưu path thay vì binary';
END
ELSE IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Employees' AND COLUMN_NAME = 'PhotoPath')
BEGIN
    PRINT '✓ Database đã ở cấu trúc mới (dùng PhotoPath)';
    PRINT 'Không cần update!';
END
ELSE
BEGIN
    PRINT '⚠ Không tìm thấy cột Photo hay PhotoPath';
    PRINT 'Thêm cột PhotoPath...';

    ALTER TABLE Employees ADD PhotoPath NVARCHAR(500) NULL;
    PRINT '✓ Đã thêm cột PhotoPath';
END

PRINT '';
PRINT '====================================';
PRINT 'KIỂM TRA CẤU TRÚC BẢNG';
PRINT '====================================';

-- Hiển thị cấu trúc bảng hiện tại
SELECT
    COLUMN_NAME AS [Tên Cột],
    DATA_TYPE AS [Kiểu Dữ Liệu],
    CHARACTER_MAXIMUM_LENGTH AS [Độ Dài],
    IS_NULLABLE AS [Null?]
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Employees'
ORDER BY ORDINAL_POSITION;

PRINT '';
PRINT 'Xong! Bây giờ bạn có thể chạy ứng dụng.';
GO
