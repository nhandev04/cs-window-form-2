-- ===================================================
-- Script: Fix EmployeeCode UNIQUE Constraint Issue
-- Description: Replace UNIQUE constraint with filtered index to allow multiple NULLs
-- Issue: UNIQUE constraint doesn't allow multiple NULL values
-- Solution: Use filtered unique index WHERE EmployeeCode IS NOT NULL
-- ===================================================

USE QuanLyNhanSu;
GO

PRINT '?? Fixing EmployeeCode UNIQUE Constraint...';
PRINT '==========================================';
PRINT '';

-- Step 1: Find and drop existing UNIQUE constraint on EmployeeCode
DECLARE @ConstraintName NVARCHAR(200);
DECLARE @SQL NVARCHAR(500);

-- Find UNIQUE constraint on EmployeeCode column
SELECT TOP 1 @ConstraintName = kc.name
FROM sys.key_constraints kc
INNER JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE kc.type = 'UQ'
  AND kc.parent_object_id = OBJECT_ID('dbo.Employees')
  AND c.name = 'EmployeeCode';

-- Drop constraint if exists
IF @ConstraintName IS NOT NULL
BEGIN
    SET @SQL = 'ALTER TABLE Employees DROP CONSTRAINT ' + QUOTENAME(@ConstraintName);
    PRINT '? Dropping existing UNIQUE constraint: ' + @ConstraintName;
    EXEC sp_executesql @SQL;
    PRINT '? Constraint dropped successfully!';
END
ELSE
BEGIN
    -- Check for unique index not tied to a constraint
    DECLARE @IndexName NVARCHAR(200);
    
    SELECT TOP 1 @IndexName = i.name
    FROM sys.indexes i
    INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
    WHERE i.object_id = OBJECT_ID('dbo.Employees')
      AND c.name = 'EmployeeCode'
    AND i.is_unique = 1
      AND i.is_primary_key = 0
      AND i.is_unique_constraint = 0; -- Only get indexes that are NOT constraint-based
    
    IF @IndexName IS NOT NULL
 BEGIN
        SET @SQL = 'DROP INDEX ' + QUOTENAME(@IndexName) + ' ON Employees';
        PRINT '? Dropping existing unique index: ' + @IndexName;
 EXEC sp_executesql @SQL;
        PRINT '? Index dropped successfully!';
    END
    ELSE
    BEGIN
        PRINT '? No existing constraint or index found to drop.';
    END
END

PRINT '';

-- Step 2: Create filtered unique index to allow multiple NULLs
PRINT '?? Creating filtered unique index...';

IF NOT EXISTS (
    SELECT 1 
    FROM sys.indexes 
    WHERE name = 'UQ_Employees_EmployeeCode' 
      AND object_id = OBJECT_ID('dbo.Employees')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UQ_Employees_EmployeeCode
    ON Employees(EmployeeCode)
 WHERE EmployeeCode IS NOT NULL;
    
    PRINT '? Filtered unique index created successfully!';
    PRINT '  - Index Name: UQ_Employees_EmployeeCode';
    PRINT '  - Column: EmployeeCode';
    PRINT '  - Filter: WHERE EmployeeCode IS NOT NULL';
    PRINT '  - Allows: Multiple NULL values ?';
    PRINT '  - Prevents: Duplicate non-NULL values ?';
END
ELSE
BEGIN
    PRINT '? Filtered unique index already exists!';
END

PRINT '';
PRINT '?? FIX COMPLETED SUCCESSFULLY!';
PRINT '==========================================';
PRINT '';
PRINT '?? Testing the fix...';
PRINT '';

-- Test: Show current EmployeeCode values
SELECT 
    Id,
    EmployeeCode,
  FullName,
    CASE 
        WHEN EmployeeCode IS NULL THEN '(NULL)'
        ELSE EmployeeCode
    END AS [EmployeeCode_Display]
FROM Employees
ORDER BY 
    CASE WHEN EmployeeCode IS NULL THEN 1 ELSE 0 END,
    EmployeeCode;

PRINT '';
PRINT '? You can now update employees with NULL EmployeeCode without errors!';
PRINT '? Duplicate non-NULL EmployeeCodes are still prevented.';
PRINT '';
