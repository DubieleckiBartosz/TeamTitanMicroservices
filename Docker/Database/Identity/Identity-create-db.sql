IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanIdentity')
BEGIN
	CREATE DATABASE TeamTitanIdentity
END

GO

IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanIdentityTests')
BEGIN
	CREATE DATABASE TeamTitanIdentityTests 
END

GO 
