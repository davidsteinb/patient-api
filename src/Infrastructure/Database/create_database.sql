-- 📌 STEP 1: Create the Database
-- ==============================
CREATE DATABASE PatientRecords;
GO

-- Use the new database
USE PatientRecords;
GO

-- ==============================
-- 📌 STEP 2: Create Patients Table
-- ==============================
CREATE TABLE Patients (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FirstName NVARCHAR(512) NOT NULL, -- ✅ Encrypted before storing
    LastName NVARCHAR(512) NOT NULL, -- ✅ Encrypted before storing
    Email NVARCHAR(512) NOT NULL, -- ✅ Encrypted before storing
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO