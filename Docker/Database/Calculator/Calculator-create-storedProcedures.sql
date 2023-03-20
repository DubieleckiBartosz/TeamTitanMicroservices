--ACCOUNT

CREATE OR ALTER PROCEDURE account_createNew_I
	@accountId UNIQUEIDENTIFIER,
	@companyCode VARCHAR(50),
	@accountOwner VARCHAR(50),
	@createdBy VARCHAR(MAX),
	@isActive BIT
AS
BEGIN
	INSERT INTO Accounts(Id, CompanyCode, AccountOwner, CreatedBy, IsActive)
	VALUES (@accountId, @companyCode, @accountOwner, @createdBy, @isActive)
END
GO

CREATE OR ALTER PROCEDURE account_statusDeactivate_U
	@accountId UNIQUEIDENTIFIER, 
	@isActive BIT,
	@deactivatedBy VARCHAR(MAX),
	@accountStatus INT
AS
BEGIN 
	UPDATE Accounts SET 
		IsActive = @isActive,
		DeactivatedBy = @deactivatedBy,
		AccountStatus = @accountStatus
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_completeData_U
	@accountId UNIQUEIDENTIFIER,
	@countingType INT,
	@status INT,
	@workDayHours INT,
	@settlementDayMonth INT,
	@expirationDate DATETIME NULL
AS
BEGIN 
	UPDATE Accounts SET 
		ExpirationDate = @expirationDate,
		CountingType = @countingType,
		AccountStatus = @status,
		WorkDayHours = @workDayHours,
		SettlementDayMonth = @settlementDayMonth
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE  account_financialData_U
	@accountId UNIQUEIDENTIFIER, 
	@overtimeRate DECIMAL NULL,
	@hourlyRate DECIMAL NULL
AS
BEGIN 
	UPDATE Accounts SET 
		OvertimeRate = @overtimeRate,
		HourlyRate = @hourlyRate
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_newCountingType_U 
	@accountId UNIQUEIDENTIFIER,
	@newCountingType INT 
AS
BEGIN 
	UPDATE Accounts SET CountingType = @newCountingType
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_newWorkDayHours_U 
	@accountId UNIQUEIDENTIFIER,
	@newWorkDayHours INT 
AS
BEGIN 
	UPDATE Accounts SET WorkDayHours = @newWorkDayHours
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_newHourlyRate_U 
	@accountId UNIQUEIDENTIFIER,
	@newHourlyRate DECIMAL 
AS
BEGIN 
	UPDATE Accounts SET HourlyRate = @newHourlyRate
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_newOvertimeRate_U 
	@accountId UNIQUEIDENTIFIER,
	@newOvertimeRate DECIMAL 
AS
BEGIN 
	UPDATE Accounts SET OvertimeRate = @newOvertimeRate
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_statusActive_U
	@accountId UNIQUEIDENTIFIER, 
	@isActive BIT,
	@activatedBy VARCHAR(MAX),
	@accountStatus INT
AS
BEGIN 
	UPDATE Accounts SET 
		IsActive = @isActive,
		ActivatedBy = @activatedBy,
		AccountStatus = @accountStatus
	WHERE Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE day_createWorkDay_I
	@accountId UNIQUEIDENTIFIER, 
	@date DATETIME,
	@hoursWorked INT,
	@overtime INT,
	@isDayOff BIT,
	@createdBy VARCHAR(MAX),
	@balance DECIMAL
AS
BEGIN  
	BEGIN TRY  
		BEGIN TRANSACTION;
			
			UPDATE Accounts SET Balance = @balance
			WHERE Id = @accountId;

			INSERT INTO WorkDays(AccountId, [Date] ,HoursWorked ,Overtime ,IsDayOff ,CreatedBy)
			VALUES (@accountId, @date, @hoursWorked, @overtime, @isDayOff, @createdBy)
	
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


CREATE OR ALTER PROCEDURE product_createPieceworkProductItem_I 
	@pieceworkProductId UNIQUEIDENTIFIER, 
	@quantity DECIMAL,
	@currentPrice DECIMAL,
	@date DateTime,
	@isConsidered BIT,
	@accountId UNIQUEIDENTIFIER,
	@balance DECIMAL
AS
BEGIN  
	BEGIN TRY  
		BEGIN TRANSACTION;

			UPDATE Accounts SET Balance = @balance
			WHERE Id = @accountId;

			INSERT INTO ProductItems(PieceworkProductId, Quantity, CurrentPrice, AccountId, [Date], IsConsidered)
			VALUES(@pieceworkProductId, @quantity, @currentPrice, @accountId, @date, @isConsidered)
	
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

CREATE OR ALTER PROCEDURE bonus_createNew_I
	@accountId UNIQUEIDENTIFIER,
	@bonusCode VARCHAR(12),  
	@creator VARCHAR(MAX),
	@canceled BIT = 0,
	@settled BIT = 0,
	@created DATETIME,
	@balance DECIMAL
AS
BEGIN 
		BEGIN TRY  
		BEGIN TRANSACTION;
			
			UPDATE Accounts SET Balance = @balance
			WHERE Id = @accountId;
			
			INSERT INTO Bonuses(AccountId, BonusCode, Creator, Canceled, Settled, Created)
			VALUES(@accountId, @bonusCode, @creator, @canceled, @settled, @created)
	
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

CREATE OR ALTER PROCEDURE account_finishBonusLife_U 
	@accountId UNIQUEIDENTIFIER,
	@bonusCode VARCHAR(12),
	@canceled BIT,
	@settled BIT,
	@balance DECIMAL
AS
BEGIN 
		BEGIN TRY  
		BEGIN TRANSACTION;

			UPDATE Accounts SET Balance = @balance
			WHERE Id = @accountId;

			UPDATE Bonuses SET 
				Settled = @settled,
				Canceled = @canceled
			WHERE BonusCode = @bonusCode
	
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

CREATE OR ALTER PROCEDURE settlement_createSettlement_I
	@accountId UNIQUEIDENTIFIER,
	@from DATETIME,
	@to DATETIME,
	@value DECIMAL 
AS
BEGIN
	
INSERT INTO dbo.Settlements(AccountId, [From], [To], [Value])
     VALUES (@accountId, @from, @to, @value )
END
GO


CREATE OR ALTER PROCEDURE account_getById_S
@accountId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT 
	   [Id]
       [CompanyCode],
       [AccountOwner],
       [Balance],
       [AccountStatus],
       [CountingType],
       [ActivatedBy],
       [CreatedBy],
       [DeactivatedBy],
       [WorkDayHours],
       [HourlyRate],
       [OvertimeRate],
       [IsActive],
       [ExpirationDate],
       [SettlementDayMonth]
  FROM [TeamTitanCalculator].[dbo].[Accounts]
END
GO

CREATE OR ALTER PROCEDURE account_getByIdWithBonuses_S
@accountId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT 
	   a.Id,
       a.CompanyCode,
       a.AccountOwner,
       a.Balance,
       a.AccountStatus,
       a.CountingType,
       a.ActivatedBy,
       a.CreatedBy,
       a.DeactivatedBy,
       a.WorkDayHours,
       a.HourlyRate,
       a.OvertimeRate,
       a.IsActive,
       a.ExpirationDate,
       a.SettlementDayMonth,
	   b.AccountId,
       b.BonusCode,
       b.Creator,
       b.Settled,
       b.Canceled
  FROM TeamTitanCalculator.dbo.Accounts AS a
  INNER JOIN Bonuses AS b ON b.AccountId = a.Id
  WHERE a.Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_getByIdWithWorkDays_s
@accountId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT 
	   a.Id,
       a.CompanyCode,
       a.AccountOwner,
       a.Balance,
       a.AccountStatus,
       a.CountingType,
       a.ActivatedBy,
       a.CreatedBy,
       a.DeactivatedBy,
       a.WorkDayHours,
       a.HourlyRate,
       a.OvertimeRate,
       a.IsActive,
       a.ExpirationDate,
       a.SettlementDayMonth, 
	   w.AccountId,
       w.[Date],
       w.HoursWorked,
       w.Overtime,
       w.IsDayOff,
       w.CreatedBy
  FROM TeamTitanCalculator.dbo.Accounts AS a
  INNER JOIN WorkDays AS w ON w.AccountId = a.Id
  WHERE a.Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_getByIdWithProducts_s
@accountId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT 
	   a.Id,
       a.CompanyCode,
       a.AccountOwner,
       a.Balance,
       a.AccountStatus,
       a.CountingType,
       a.ActivatedBy,
       a.CreatedBy,
       a.DeactivatedBy,
       a.WorkDayHours,
       a.HourlyRate,
       a.OvertimeRate,
       a.IsActive,
       a.ExpirationDate,
       a.SettlementDayMonth,
	   p.AccountId,
       p.PieceworkProductId,
       p.CurrentPrice,
       p.Quantity,
       p.IsConsidered,
       p.[Date]
  FROM TeamTitanCalculator.dbo.Accounts AS a
  INNER JOIN ProductItems AS p ON p.AccountId = a.Id
  WHERE a.Id = @accountId
END
GO

CREATE OR ALTER PROCEDURE account_getDetailsById_s
@accountId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT 
	   a.Id,
       a.CompanyCode,
       a.AccountOwner,
       a.Balance,
       a.AccountStatus,
       a.CountingType,
       a.ActivatedBy,
       a.CreatedBy,
       a.DeactivatedBy,
       a.WorkDayHours,
       a.HourlyRate,
       a.OvertimeRate,
       a.IsActive,
       a.ExpirationDate,
       a.SettlementDayMonth,
	   w.AccountId,
       w.[Date],
       w.HoursWorked,
       w.Overtime,
       w.IsDayOff,
       w.CreatedBy,
	   p.AccountId,
       p.PieceworkProductId,
       p.CurrentPrice,
       p.Quantity,
       p.IsConsidered,
       p.[Date]
  FROM TeamTitanCalculator.dbo.Accounts AS a
  LEFT JOIN WorkDays AS w ON w.AccountId = a.Id
  LEFT JOIN ProductItems AS p ON p.AccountId = a.Id
  WHERE a.Id = @accountId
END
GO


 CREATE OR ALTER PROCEDURE [dbo].[account_getBySearch_S]
	@accountId UNIQUEIDENTIFIER NULL,
	@countingType INT NULL,
	@accountStatus INT NULL,
	@expirationDateFrom DATETIME NULL,
	@expirationDateTo DATETIME NULL,
	@activatedBy VARCHAR(50) NULL,
	@deactivatedBy VARCHAR(50) NULL,
	@hourlyRateFrom DECIMAL NULL,
	@hourlyRateTo DECIMAL NULL,
	@settlementDayMonth INT NULL,
	@balanceFrom DECIMAL NULL,
	@balanceTo DECIMAL NULL,
	@type VARCHAR(10),
	@name VARCHAR(50),
	@pageNumber INT,
	@pageSize INT,
	@companyCode VARCHAR(50) 
AS
BEGIN
	 WITH Data_CTE 
	 AS
	 (
		 SELECT 
			  Id,
			  CompanyCode,
			  AccountOwner,
			  Balance,
			  AccountStatus,
			  CountingType,
			  ActivatedBy,
			  CreatedBy,
			  DeactivatedBy,
			  WorkDayHours,
			  HourlyRate,
			  OvertimeRate,
			  IsActive,
			  ExpirationDate,
			  SettlementDayMonth,
			  Created
		 FROM TeamTitanCalculator.dbo.Accounts 
		 WHERE CompanyCode = @companyCode 
		 AND (@accountId IS NULL OR Id = @accountId)
		 AND (@countingType IS NULL OR CountingType = @countingType)
		 AND (@accountStatus IS NULL OR AccountStatus = @accountStatus)
		 AND (@expirationDateFrom IS NULL OR ExpirationDate >= @expirationDateFrom)
		 AND (@expirationDateTo IS NULL OR ExpirationDate <= @expirationDateTo)
		 AND (@activatedBy IS NULL OR ActivatedBy = @activatedBy)
		 AND (@deactivatedBy IS NULL OR DeactivatedBy = @deactivatedBy)
		 AND (@hourlyRateFrom IS NULL OR HourlyRate >= @hourlyRateFrom)
		 AND (@hourlyRateTo IS NULL OR HourlyRate <= @hourlyRateTo)
		 AND (@settlementDayMonth IS NULL OR SettlementDayMonth = @settlementDayMonth)
		 AND (@balanceFrom IS NULL OR Balance >= @balanceFrom)
		 AND (@balanceTo IS NULL OR Balance <= @balanceTo)
	 ), 
	 Count_CTE 
	 AS 
	 (
	 	SELECT COUNT(*) AS TotalCount FROM Data_CTE
	 )
	 SELECT 
	 	   d.Id,
		   d.CompanyCode,
		   d.AccountOwner,
		   d.Balance,
		   d.AccountStatus,
		   d.CountingType,
		   d.ActivatedBy,
		   d.CreatedBy,
		   d.DeactivatedBy,
		   d.WorkDayHours,
		   d.HourlyRate,
		   d.OvertimeRate,
		   d.IsActive,
		   d.ExpirationDate,
		   d.SettlementDayMonth,
		   c.TotalCount
	 FROM Data_CTE AS d
	 CROSS JOIN Count_CTE AS c
	 ORDER BY 
	 CASE WHEN @name = 'Id' AND @type = 'asc' THEN Id END ASC,  
	 CASE WHEN @name = 'Id' AND @type = 'desc' THEN Id END DESC,

	 CASE WHEN @name = 'HourlyRate' AND @type = 'asc' THEN HourlyRate END ASC,  
	 CASE WHEN @name = 'HourlyRate' AND @type = 'desc' THEN HourlyRate END DESC,

	 CASE WHEN @name = 'ExpirationDate' AND @type = 'asc' THEN ExpirationDate END ASC,  
	 CASE WHEN @name = 'ExpirationDate' AND @type = 'desc' THEN ExpirationDate END DESC,

	 CASE WHEN @name = 'Balance' AND @type = 'asc' THEN Balance END ASC,  
	 CASE WHEN @name = 'Balance' AND @type = 'desc' THEN Balance END DESC,

	 CASE WHEN @name = 'SettlementDayMonth' AND @type = 'asc' THEN SettlementDayMonth END ASC,  
	 CASE WHEN @name = 'SettlementDayMonth' AND @type = 'desc' THEN SettlementDayMonth END DESC,

	 CASE WHEN @name = 'Created' AND @type = 'asc' THEN Created END ASC,  
	 CASE WHEN @name = 'Created' AND @type = 'desc' THEN Created END DESC

	 OFFSET (@pageNumber - 1)* @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY;   
END
GO