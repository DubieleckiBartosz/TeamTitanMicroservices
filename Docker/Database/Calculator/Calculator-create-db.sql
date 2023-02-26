IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanCalculator')
BEGIN
	CREATE DATABASE TeamTitanCalculator
END
GO

IF NOT EXISTS(SELECT*FROM sys.databases WHERE name = 'TeamTitanCalculatorTests')
BEGIN
	CREATE DATABASE TeamTitanCalculatorTests 
END
GO 
 
