SET QUOTED_IDENTIFIER ON;

--Companies table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Companies' and xtype='U')
BEGIN
	CREATE TABLE Companies(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	CompanyStatus INT NOT NULL,
	CompanyCode VARCHAR(50) NOT NULL,
	CompanyName VARCHAR (250) NULL,
	IsConfirmed BIT NOT NULL,
	[OpenFrom] INT NULL,
	[OpenTo] INT NULL, 
	OwnerCode VARCHAR(50) NOT NULL,
	[OwnerId] INT NOT NULL,
	Created DATETIME DEFAULT GETDATE(),
	Modified DATETIME DEFAULT GETDATE()
)
END


--Departments table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Departments' and xtype='U')
BEGIN
	CREATE TABLE Departments(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	CompanyId INT NOT NULL FOREIGN KEY REFERENCES Companies(Id),
	DepartmentName VARCHAR (100) NOT NULL,  
	Created DATETIME DEFAULT GETDATE(),
	Modified DATETIME DEFAULT GETDATE()
) 
END


--Employees table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' and xtype='U')
BEGIN
	CREATE TABLE Employees(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	AccountId UNIQUEIDENTIFIER NULL, 
	DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id), 
	EmployeeCode VARCHAR(50) NULL, 
	[Name] VARCHAR (50) NOT NULL,
	Surname VARCHAR (50) NOT NULL,
	Birthday DATETIME NOT NULL,
	PersonIdentifier VARCHAR(11) NULL,
	Created DATETIME DEFAULT GETDATE(),
	Modified DATETIME DEFAULT GETDATE()
) 
END

--ContactDetails
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ContactDetails' and xtype='U')
BEGIN
CREATE TABLE ContactDetails(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NULL FOREIGN KEY REFERENCES Employees(Id),
	CompanyId INT NULL FOREIGN KEY REFERENCES Companies(Id), 
	City VARCHAR(50) NOT NULL,
	Street VARCHAR(50) NOT NULL,
	NumberStreet VARCHAR(10) NOT NULL,
	PostalCode VARCHAR(10) NOT NULL,
	PhoneNumber VARCHAR(20) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	Created DATETIME DEFAULT GETDATE(),
	Modified DATETIME DEFAULT GETDATE()
)
END 

--EmployeeContracts table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='EmployeeContracts' and xtype='U')
BEGIN
	CREATE TABLE EmployeeContracts(
	Id INT IDENTITY(1, 1) PRIMARY KEY,  
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	Position VARCHAR(100) NOT NULL, 
	ContractType INT NOT NULL,
	SettlementType INT NOT NULL, --If we have ContractType, is it necessary?
	Salary DECIMAL DEFAULT 0.00, 
	StartContract DATETIME NOT NULL,
	EndContract DATETIME NULL,
	NumberHoursPerDay INT NULL,
	FreeDaysPerYear INT NOT NULL,
	BankAccountNumber VARCHAR(MAX) NULL,
	CreatedBy VARCHAR(MAX) NULL,
	PaidIntoAccount BIT NOT NULL,
	PaymentMonthDay BIT NOT NULL,
	Created DATETIME DEFAULT GETDATE(),
	Modified DATETIME DEFAULT GETDATE()
) 
END
 
  
--DayOffRequests
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DayOffRequests' and xtype='U')
BEGIN
	CREATE TABLE DayOffRequests(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL REFERENCES Employees(Id),
	CreatedBy VARCHAR(MAX) NOT NULL,
	FromDate DATETIME NOT NULL,
	ToDate DATETIME NOT NULL,
	CurrentStatus INT NOT NULL,
	ReasonType INT NOT NULL, 
	[Description] VARCHAR(MAX) NULL,
	ConsideredBy VARCHAR(MAX) NULL,
	Canceled BIT NOT NULL DEFAULT 0,
	Created DATETIME DEFAULT GETDATE(),
	Modified DATETIME DEFAULT GETDATE()
) 
END

