--BONUS
CREATE OR ALTER PROCEDURE program_createNew_I
	@bonusId UNIQUEIDENTIFIER,
	@bonusAmount DECIMAL,
	@createdBy VARCHAR(MAX),
	@companyCode VARCHAR(MAX),
	@expires DATETIME,
	@reason VARCHAR(MAX)
AS
BEGIN
	INSERT INTO BonusPrograms(Id, BonusAmount, CreatedBy, CompanyCode, Expires, Reason)
	VALUES(@bonusId, @bonusAmount, @createdBy, @companyCode, @expires, @reason)
END
GO 

CREATE OR ALTER PROCEDURE bonus_createNew_I
	@bonusId UNIQUEIDENTIFIER,
	@recipientCode VARCHAR(MAX),
	@bonusCode VARCHAR(12),
	@groupBonus BIT,
	@creator VARCHAR(MAX),
	@canceled BIT = 0,
	@settled BIT = 0,
	@created DATETIME
AS
BEGIN 
	INSERT INTO Bonuses(BonusProgramId, Recipient, BonusCode, GroupBonus, Creator, Canceled, Settled, Created)
	VALUES(@bonusId, @recipientCode, @bonusCode, @groupBonus, @creator, @canceled, @settled, @created)
END
GO

CREATE OR ALTER PROCEDURE bonus_finish_U
	@bonusId UNIQUEIDENTIFIER,
	@canceled BIT,
	@settled BIT
AS
BEGIN 
	UPDATE Bonuses SET 
		Settled = @settled,
		Canceled = @canceled
	WHERE Id = @bonusId		
END
GO

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
	@createdBy VARCHAR(MAX)
AS
BEGIN  
	INSERT INTO WorkDays(AccountId, [Date] ,HoursWorked ,Overtime ,IsDayOff ,CreatedBy)
	VALUES (@accountId, @date, @hoursWorked, @overtime, @isDayOff, @createdBy)
END
GO


CREATE OR ALTER PROCEDURE product_createPieceworkProductItem_I
	@pieceworkProductId UNIQUEIDENTIFIER, 
	@quantity DECIMAL,
	@currentPrice DECIMAL,
	@accountId UNIQUEIDENTIFIER
AS
BEGIN  
	INSERT INTO ProductItems(PieceworkProductId, Quantity, CurrentPrice, AccountId)
	VALUES(@pieceworkProductId, @quantity, @currentPrice, @accountId)
END
GO