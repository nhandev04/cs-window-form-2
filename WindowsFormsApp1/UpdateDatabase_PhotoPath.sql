-- ===================================================
-- Script: Update Employees Table - Add PhotoPath Column
-- Description: Adds PhotoPath column to store employee photo file paths
-- ===================================================

USE QuanLyNhanSu;
GO

-- Check if PhotoPath column exists, if not add it
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Employees')
    AND name = 'PhotoPath'
)
BEGIN
    ALTER TABLE Employees
    ADD PhotoPath NVARCHAR(500) NULL;

    PRINT 'PhotoPath column added successfully to Employees table.';
END
ELSE
BEGIN
    PRINT 'PhotoPath column already exists in Employees table.';
END
GO

-- Verify the change
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Employees'
    AND COLUMN_NAME = 'PhotoPath';
GO

PRINT 'Update completed successfully!';
GO
