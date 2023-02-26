IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanManagement')
BEGIN
	CREATE DATABASE TeamTitanManagement
END
GO

IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanManagementTests')
BEGIN
	CREATE DATABASE TeamTitanManagementTests 
END
GO 
 
