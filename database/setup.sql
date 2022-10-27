CREATE DATABASE SecurityDB;
GO
USE SecurityDB;
GO

CREATE TABLE PermissionTypes(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Description NVARCHAR(MAX) NOT NULL
);

INSERT INTO PermissionTypes(Description)
VALUES('Owner');
INSERT INTO PermissionTypes(Description)
VALUES('Read');
INSERT INTO PermissionTypes(Description)
VALUES('Write');
INSERT INTO PermissionTypes(Description)
VALUES('Delete');

GO

CREATE TABLE Permissions(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeForename NVARCHAR(MAX) NOT NULL,
    EmployeeSurname NVARCHAR(MAX) NOT NULL,
    PermissionType INT NOT NULL,
    PermissionDate DATE NOT NULL,
    CONSTRAINT FK_Permissions_PermissionTypes FOREIGN KEY (PermissionType)
        REFERENCES dbo.PermissionTypes(Id)
);

GO