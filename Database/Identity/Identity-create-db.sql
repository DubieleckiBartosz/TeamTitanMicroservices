sp_configure 'user options', 256
RECONFIGURE WITH OVERRIDE;
GO

IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanIdentity')
BEGIN
	CREATE DATABASE TeamTitanIdentity
END

GO 