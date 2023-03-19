IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanHangfire')
BEGIN
	CREATE DATABASE TeamTitanHangfire
END
GO