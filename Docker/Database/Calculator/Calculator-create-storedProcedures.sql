--ACCOUNT

CREATE OR ALTER PROCEDURE account_createNew_I
	@accountId UNIQUEIDENTIFIER,
	@departmentCode VARCHAR(MAX),
	@accountOwner VARCHAR(MAX),
	@createdBy VARCHAR(MAX),
	@isActive BIT
AS
BEGIN
	INSERT INTO Accounts(Id, DepartmentCode, AccountOwner, CreatedBy, IsActive)
	VALUES (@accountId, @departmentCode, @accountOwner, @createdBy, @isActive)
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
	@overtimeRate DECIMAL NULL,
	@hourlyRate DECIMAL NULL
AS
BEGIN 
	UPDATE Accounts SET 
		CountingType = @countingType,
		AccountStatus = @status,
		WorkDayHours = @workDayHours,
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