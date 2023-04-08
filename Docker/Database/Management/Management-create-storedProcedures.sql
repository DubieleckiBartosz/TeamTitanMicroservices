SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE company_initComany_I
	@companyStatus INT,
	@isConfirmed BIT,
	@companyCode VARCHAR(50),
	@ownerCode VARCHAR(50),
	@ownerId INT
AS
BEGIN
	INSERT INTO [dbo].[Companies]
			   ([CompanyStatus],
				[CompanyCode],  
				[IsConfirmed],
				[OwnerCode],
				[OwnerId]) 
		 VALUES
			   (@companyStatus, 
			    @companyCode,  
			    @isConfirmed,  
				@ownerCode,
			    @ownerId)
END 
GO


CREATE OR ALTER PROCEDURE company_nameExists_S 
	@companyName VARCHAR (250)
AS
BEGIN 
	SELECT 1 FROM Companies 
	WHERE CompanyName = @companyName
END 
GO

CREATE OR ALTER PROCEDURE company_codeExists_S 
	@companyCode VARCHAR (50)
AS
BEGIN 
	SELECT 1 FROM Companies 
	WHERE CompanyCode = @companyCode
END 
GO

CREATE OR ALTER PROCEDURE [dbo].[company_getByOwnerCode_S] 
	@ownerCode VARCHAR (50)
AS
BEGIN 
	SELECT c.[Id],
		   c.[CompanyStatus],
		   c.[CompanyCode],
		   c.[CompanyName],
		   c.[IsConfirmed],
		   c.[OpenFrom],
		   c.[OpenTo],
		   c.[OwnerCode],
		   c.[OwnerId], 
		   c.[Version],
		   cd.[Id],
           cd.[EmployeeId],
           cd.[CompanyId],
           cd.[City],
           cd.[Street],
           cd.[NumberStreet],
           cd.[PostalCode],
           cd.[PhoneNumber],
           cd.[Email],
		   cd.[Version]
	  FROM [TeamTitanManagement].[dbo].[Companies] AS c
	  LEFT JOIN ContactDetails AS cd ON cd.CompanyId = c.Id
	  WHERE OwnerCode = @ownerCode
END 
GO

CREATE OR ALTER PROCEDURE company_getByCode_S 
	@companyCode VARCHAR (50)
AS
BEGIN 
	SELECT c.[Id],
		   c.[CompanyStatus],
		   c.[CompanyCode],
		   c.[CompanyName],
		   c.[IsConfirmed],
		   c.[OpenFrom],
		   c.[OpenTo],
		   c.[OwnerCode],
		   c.[OwnerId],
		   c.[Version],
		   cd.[Id], 
           cd.[CompanyId],
           cd.[City],
           cd.[Street],
           cd.[NumberStreet],
           cd.[PostalCode],
           cd.[PhoneNumber],
           cd.[Email],
		   cd.[Version]
	  FROM [TeamTitanManagement].[dbo].[Companies] AS c
	  INNER JOIN ContactDetails AS cd ON cd.CompanyId = c.Id
	  WHERE CompanyCode = @companyCode
END 
GO

CREATE OR ALTER PROCEDURE company_getWithDepartmentsByCode_S
	@companyCode VARCHAR(50)
AS 
BEGIN
	SELECT c.[Id],
		   c.[CompanyStatus],
		   c.[CompanyCode],
		   c.[CompanyName],
		   c.[IsConfirmed],
		   c.[OpenFrom],
		   c.[OpenTo],
		   c.[OwnerCode],
		   c.[OwnerId],
		   c.[Version],
		   cd.[Id], 
           cd.[CompanyId],
           cd.[City],
           cd.[Street],
           cd.[NumberStreet],
           cd.[PostalCode],
           cd.[PhoneNumber],
           cd.[Email],
		   cd.[Version],
		   d.[Id],
		   d.[DepartmentName],
		   d.[Version]
	  FROM [TeamTitanManagement].[dbo].[Companies] AS c
	  INNER JOIN ContactDetails AS cd ON cd.CompanyId = c.Id
	  LEFT JOIN Departments AS d ON d.CompanyId = c.Id
	  WHERE c.[CompanyCode] = @companyCode
END
GO 

CREATE OR ALTER PROCEDURE company_completeData_U
	@companyId INT,
	@companyName VARCHAR(250),
	@isConfirmed BIT,
	@from INT NULL,
	@to INT NULL,
	@city VARCHAR(50),
	@street VARCHAR(50),
	@numberStreet VARCHAR(10),
	@postalCode VARChAR(10),
	@phoneNumber VARCHAR(20),
	@email VARCHAR(50),
	@version INT
AS
BEGIN
	BEGIN TRY  
		BEGIN TRANSACTION;
		UPDATE Companies SET 
			CompanyName = @companyName,
			IsConfirmed = @isConfirmed,
			OpenFrom = @from,
			OpenTo = @to,
			[Version] = @version
		WHERE Id = @companyId
	
		INSERT INTO [dbo].[ContactDetails]
			   ([EmployeeId],
				[CompanyId],
				[City],
				[Street],
				[NumberStreet],
				[PostalCode],
				[PhoneNumber],
				[Email])
		 VALUES
			   (NULL, 
				@companyId,
				@city,
				@street,
				@numberStreet,
				@postalCode,
				@phoneNumber,
				@email )
		COMMIT TRANSACTION;
	END TRY  
	BEGIN CATCH
	    IF (XACT_STATE()) = -1
        BEGIN
			ROLLBACK TRANSACTION
		END
  
		IF (XACT_STATE()) = 1
        BEGIN
			COMMIT TRANSACTION
		END
		
	END CATCH
END
GO
 
CREATE OR ALTER PROCEDURE company_updateCommunicationData_U
	@companyId INT,
	@city VARCHAR(50),
	@street VARCHAR(50),
	@numberStreet VARCHAR(10),
	@postalCode VARChAR(10),
	@phoneNumber VARCHAR(20),
	@email VARCHAR(50),
	@version INT
AS
BEGIN
	 UPDATE [dbo].[ContactDetails]
	   SET [City] = @city,
		   [Street] = @street,
		   [NumberStreet] = @numberStreet,
		   [PostalCode] = @postalCode,
		   [PhoneNumber] = @phoneNumber,
		   [Email] = @email,
		   [Version] = @version
	 WHERE CompanyId = @companyId
END
GO

--Department scripts

CREATE OR ALTER PROCEDURE department_newDepartment_I
	@companyId INT,
	@departmentName VARCHAR(100),
	@version INT
AS
BEGIN 
	BEGIN TRY  
		BEGIN TRANSACTION;
			
			UPDATE Companies SET [Version] = @version
			WHERE Id = @companyId

			INSERT INTO [dbo].[Departments]
					   ([CompanyId],
						[DepartmentName])
				 VALUES
					   (@companyId,
						@departmentName)

		COMMIT TRANSACTION;
	END TRY  
	BEGIN CATCH
	    IF (XACT_STATE()) = -1
        BEGIN
			ROLLBACK TRANSACTION
		END
  
		IF (XACT_STATE()) = 1
        BEGIN
			COMMIT TRANSACTION
		END
		
	END CATCH
END
GO  

CREATE OR ALTER PROCEDURE department_getById_S 
	@departmentId INT
AS
BEGIN 
	SELECT [Id],
		   [CompanyId],
		   [DepartmentName],
		   [Version]
	  FROM [TeamTitanManagement].[dbo].[Departments]
	  WHERE Id = @departmentId
END 
GO
 
CREATE OR ALTER PROCEDURE department_getDepartmentsByCompanyId_S 
	@companyId INT
AS
BEGIN 
	SELECT [Id],
		   [DepartmentName],
		   [Version]
	  FROM [TeamTitanManagement].[dbo].[Departments]
	  WHERE CompanyId = @companyId
END 
GO

--DayOffRequest scripts

CREATE OR ALTER PROCEDURE dayOff_newDayOffRequest_I
	@employeeId INT,
	@createdBy VARCHAR(50),
	@currentStatus INT,
	@fromDate DATETIME,
	@toDate DATETIME,
	@reasonType INT,
	@description VARCHAR(MAX),
	@canceled BIT,
	@version INT
AS
BEGIN 
	BEGIN TRY  
		BEGIN TRANSACTION;

		UPDATE Employees SET [Version] = @version
		WHERE Id = @employeeId

		INSERT INTO [dbo].[DayOffRequests]
				   ([EmployeeId],
					[CreatedBy],
					[FromDate],
					[ToDate],
					[CurrentStatus],
					[ReasonType],
					[Description], 
					[Canceled])
			 VALUES
				   (@employeeId,
					@createdBy,
					@fromDate,
					@toDate,
					@currentStatus,
					@reasonType,
					@description,
					@canceled)
		COMMIT TRANSACTION;
	END TRY  
	BEGIN CATCH
	    IF (XACT_STATE()) = -1
        BEGIN
			ROLLBACK TRANSACTION
		END
  
		IF (XACT_STATE()) = 1
        BEGIN
			COMMIT TRANSACTION
		END
		
	END CATCH
END 
GO

CREATE OR ALTER PROCEDURE dayOff_getById_S 
	@dayOffRequestId INT
AS
BEGIN 
	SELECT [Id],
		   [EmployeeId],
		   [CreatedBy],
		   [FromDate],
		   [ToDate],
		   [CurrentStatus],
		   [ReasonType],
		   [Description],
		   [ConsideredBy],
		   [Canceled],
		   [Version]
	  FROM [TeamTitanManagement].[dbo].[DayOffRequests]
	  WHERE Id = @dayOffRequestId
END 
GO

CREATE OR ALTER PROCEDURE dayOff_cancel_U
	@dayOffRequestId INT,
	@isCanceled BIT,
	@version INT
AS
BEGIN
	UPDATE DayOffRequests SET 
			Canceled = @isCanceled,
			[Version] = @version
	WHERE Id = @dayOffRequestId
END
GO

CREATE OR ALTER PROCEDURE dayOff_considerRequest_U
	@dayOffRequestId INT,
	@status INT,
	@consideredBy VARCHAR(50),
	@version INT
AS
BEGIN
	UPDATE DayOffRequests SET 
		CurrentStatus = @status,
		ConsideredBy = @consideredBy,
		[Version] = @version
	WHERE Id = @dayOffRequestId
END
GO

--Employee scripts

CREATE OR ALTER PROCEDURE employee_newEmployee_I
	@departmentId INT,
	@employeeCode VARCHAR(50),
	@name VARCHAR(50),
	@surname VARCHAR(50),
	@birthday DATETIME,
	@personIdentifier VARCHAR(11) NULL,
	@city VARCHAR(50),
	@street VARCHAR(50),
	@numberStreet VARCHAR(10),
	@postalCode VARChAR(10),
	@phoneNumber VARCHAR(20),
	@email VARCHAR(50),
	@leader VARCHAR(50)
AS
BEGIN
	BEGIN TRY  
		BEGIN TRANSACTION; 
			
			UPDATE Departments SET [Version] = @version
			WHERE Id = @departmentId

			INSERT INTO [dbo].[Employees]
					   ([DepartmentId],
						[EmployeeCode],
						[Name],
						[Surname],
						[Birthday],
						[PersonIdentifier],
						[Leader]) 
				 VALUES
					   (@departmentId,
						@employeeCode, 
						@name, 
						@surname, 
						@birthday,
						@personIdentifier,
						@leader)
		
		    DECLARE @employeeId INT = SCOPE_IDENTITY();

			INSERT INTO [dbo].[ContactDetails]
				   ([EmployeeId],
					[CompanyId],
					[City],
					[Street],
					[NumberStreet],
					[PostalCode],
					[PhoneNumber],
					[Email])
			 VALUES
				   (@employeeId, 
					NULL,
					@city,
					@street,
					@numberStreet,
					@postalCode,
					@phoneNumber,
					@email )
		COMMIT TRANSACTION;
	END TRY  
	BEGIN CATCH
	    IF (XACT_STATE()) = -1
        BEGIN
			ROLLBACK TRANSACTION
		END
  
		IF (XACT_STATE()) = 1
        BEGIN
			COMMIT TRANSACTION
		END 
	END CATCH
END
GO


CREATE OR ALTER PROCEDURE employee_getEmployeeById_S 
	@employeeId INT
AS
BEGIN 
	SELECT e.[Id],
		   e.[AccountId],
		   e.[DepartmentId],
		   e.[Leader],
		   e.[EmployeeCode],
		   e.[Name],
		   e.[Surname],
		   e.[Birthday],
		   e.[PersonIdentifier],
		   e.[Version],
           cd.[City],
           cd.[Street],
           cd.[NumberStreet],
           cd.[PostalCode],
           cd.[PhoneNumber],
           cd.[Email],
		   cd.[Version],
		   ec.[Id],
		   ec.[EmployeeId],
		   ec.[Position],
		   ec.[ContractType],
		   ec.[SettlementType],
		   ec.[Salary],
		   ec.[StartContract],
		   ec.[EndContract],
		   ec.[NumberHoursPerDay],
		   ec.[FreeDaysPerYear],
		   ec.[BankAccountNumber],
		   ec.[CreatedBy],
		   ec.[PaidIntoAccount],
		   ec.[PaymentMonthDay],
		   ec.[Version],
		   dor.[Id],
		   dor.[EmployeeId],
		   dor.[CreatedBy],
		   dor.[FromDate],
		   dor.[ToDate],
		   dor.[CurrentStatus],
		   dor.[ReasonType],
		   dor.[Description],
		   dor.[ConsideredBy],
		   dor.[Canceled],
		   dor.[Version]
	  FROM [TeamTitanManagement].[dbo].[Employees] AS e
	  INNER JOIN ContactDetails AS cd ON cd.EmployeeId = e.Id
	  LEFT JOIN EmployeeContracts AS ec ON ec.EmployeeId = e.Id 
	  LEFT JOIN DayOffRequests AS dor ON dor.EmployeeId = e.Id
	  WHERE e.Id = @employeeId
END 
GO

CREATE OR ALTER PROCEDURE employee_getEmployeeByCode_S 
	@code VARCHAR(50)
AS
BEGIN 
	SELECT e.[Id],
		   e.[AccountId],
		   e.[DepartmentId],
		   e.[EmployeeCode],
		   e.[Leader],
		   e.[Name],
		   e.[Surname],
		   e.[Birthday],
		   e.[PersonIdentifier], 
		   e.[Version],
		   cd.[City],
           cd.[Street],
           cd.[NumberStreet],
           cd.[PostalCode],
           cd.[PhoneNumber],
           cd.[Email],
		   cd.[Version],
		   ec.[Id],
		   ec.[EmployeeId],
		   ec.[Position],
		   ec.[ContractType],
		   ec.[SettlementType],
		   ec.[Salary],
		   ec.[StartContract],
		   ec.[EndContract],
		   ec.[NumberHoursPerDay],
		   ec.[FreeDaysPerYear],
		   ec.[BankAccountNumber],
		   ec.[CreatedBy],
		   ec.[PaidIntoAccount],
		   ec.[PaymentMonthDay],
		   ec.[Version],
		   dor.[Id],
		   dor.[EmployeeId],
		   dor.[CreatedBy],
		   dor.[FromDate],
		   dor.[ToDate],
		   dor.[CurrentStatus],
		   dor.[ReasonType],
		   dor.[Description],
		   dor.[ConsideredBy],
		   dor.[Canceled],
		   dor.[Version]
	  FROM [TeamTitanManagement].[dbo].[Employees] AS e
	  INNER JOIN ContactDetails AS cd ON cd.EmployeeId = e.Id
	  LEFT JOIN EmployeeContracts AS ec ON ec.EmployeeId = e.Id 
	  LEFT JOIN DayOffRequests AS dor ON dor.EmployeeId = e.Id
	  WHERE e.EmployeeCode = @code
END 
GO

CREATE OR ALTER PROCEDURE employee_addAccountToEmployee_U
	@employeeId INT,
	@accountId UNIQUEIDENTIFIER,
	@version INT
AS
BEGIN
	UPDATE Employees SET
		AccountId = @accountId,
		[Version] = @version
	WHERE Id = @employeeId
END
GO

CREATE OR ALTER PROCEDURE employee_contactData_U
	@employeeId INT,
	@phoneNumber VARCHAR(20),
	@email VARCHAR(50),
	@version INT
AS
BEGIN
	UPDATE ContactDetails SET 
		PhoneNumber = @phoneNumber,
		Email = @email,
		[Version] = @version
	WHERE EmployeeId = @employeeId
END
GO

CREATE OR ALTER PROCEDURE employee_address_U
	@employeeId INT,
	@city VARCHAR(50),
	@street VARCHAR(50),
	@numberStreet VARCHAR(10),
	@postalCode VARChAR(10),
	@version INT
AS
BEGIN
	UPDATE ContactDetails SET 
		City = @city,
		Street = @street,
		NumberStreet = @numberStreet,
		PostalCode = @postalCode,
		[Version] = @version
	WHERE EmployeeId = @employeeId
END
GO

CREATE OR ALTER PROCEDURE employee_newLeader_U
	@employeeId INT,
	@newLeader VARCHAR(50),
	@version INT
AS
BEGIN
	UPDATE Employees SET
		Leader = @newLeader,
		[Version] = @version
	WHERE Id = @employeeId
END
GO

CREATE OR ALTER PROCEDURE employee_getNecessaryDataById_S
	@employeeId INT
AS
BEGIN
	SELECT [Id],
		   [AccountId] ,
		   [EmployeeCode],
		   [Name],
		   [Surname],
		   [PersonIdentifier],
		   [Leader],
		   [Version]
	  FROM [TeamTitanManagement].[dbo].[Employees]
	  WHERE Id = @employeeId
END
GO

--Contract scripts

CREATE OR ALTER PROCEDURE contract_newContract_I
	@employeeId INT,
	@version INT,
	@position VARCHAR(100),
	@contractType INT,
	@settlementType INT,
	@salary DECIMAL, 
	@startContract DATETIME,
	@endContract DATETIME,
	@numberHoursPerDay INT,
	@freeDaysPerYear INT,
	@bankAccountNumber VARCHAR(MAX),
	@paidIntoAccount BIT,
	@hourlyRate DECIMAL,
	@overtimeRate DECIMAL,
	@paymentMonthDay DECIMAL,
	@createdBy VARCHAR(50)
AS
BEGIN
	BEGIN TRY  
		BEGIN TRANSACTION; 

			UPDATE Employees SET [Version] = @version
			WHERE Id = @employeeId

			INSERT INTO [dbo].[EmployeeContracts]
					   ([EmployeeId]
					   ,[Position]
					   ,[ContractType]
					   ,[SettlementType]
					   ,[Salary]
					   ,[StartContract]
					   ,[EndContract]
					   ,[NumberHoursPerDay]
					   ,[FreeDaysPerYear]
					   ,[BankAccountNumber]
					   ,[CreatedBy]
					   ,[PaidIntoAccount]
					   ,[PaymentMonthDay])
				 VALUES
						(@employeeId, 
						 @position, 
						 @contractType, 
						 @settlementType, 
						 @salary, 
						 @startContract, 
						 @endContract, 
						 @numberHoursPerDay,
						 @freeDaysPerYear, 
						 @bankAccountNumber, 
						 @createdBy, 
						 @paidIntoAccount, 
						 @paymentMonthDay)

		COMMIT TRANSACTION;
	END TRY  
	BEGIN CATCH
	    IF (XACT_STATE()) = -1
        BEGIN
			ROLLBACK TRANSACTION
		END
  
		IF (XACT_STATE()) = 1
        BEGIN
			COMMIT TRANSACTION
		END 
	END CATCH
END
GO
 
