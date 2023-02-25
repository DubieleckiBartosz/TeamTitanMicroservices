
CREATE OR ALTER PROCEDURE user_initAccount_I 
	@departmentCode VARCHAR(MAX),
	@companyId VARCHAR(MAX),
	@employeeCode VARCHAR(MAX), 
	@isConfirmed BIT,
	@roleId INT
AS
BEGIN  
	BEGIN TRY
		BEGIN TRANSACTION; 
	 
			IF EXISTS (SELECT* FROM Roles WHERE Id = @roleId) 
			BEGIN 
				DECLARE @newIdentity INT;
					
				INSERT INTO ApplicationUsers(IsConfirmed, EmployeeCode, DepartmentCode, CompanyId)
				VALUES(@isConfirmed, @employeeCode, @departmentCode, @companyId) 

				SET @newIdentity = CAST(SCOPE_IDENTITY() AS INT)
			
				INSERT INTO UserRoles(RoleId, UserId) VALUES(@roleId, @newIdentity)
			END

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

CREATE OR ALTER PROCEDURE user_completeData_U
	@identifier INT, 
	@verificationToken VARCHAR(MAX),
	@verificationTokenExpirationDate DATETIME,
	@isConfirmed BIT, 
	@userName VARCHAR(50),
	@email VARCHAR(50),
	@phoneNumber VARCHAR(50),
	@passwordHash VARCHAR(MAX)
AS
BEGIN
	UPDATE ApplicationUsers 
		SET VerificationToken = @verificationToken, 
			VerificationTokenExpirationDate = @verificationTokenExpirationDate,
			IsConfirmed = @isConfirmed, UserName = @userName, 
			Email = @email, PhoneNumber = @phoneNumber,
			PasswordHash = @passwordHash,
			Completed = 1
	WHERE Id = @identifier AND Completed = 0
END
GO


CREATE OR ALTER PROCEDURE user_getUserByCode  
	@uniqueCode VARCHAR(MAX) 
AS
BEGIN  
	SELECT 
		au.Id,
		au.CompanyId, 
		au.DepartmentCode,
		au.EmployeeCode,
		ur.RoleId 
	FROM ApplicationUsers AS au
	INNER JOIN UserRoles AS ur ON ur.UserId = au.Id 
	WHERE au.EmployeeCode = @uniqueCode AND au.Completed = 0
END
GO


CREATE OR ALTER PROCEDURE user_codeExists_S
	@code VARCHAR(MAX)
AS
BEGIN
	SELECT 1 FROM ApplicationUsers WHERE EmployeeCode = @code
END
GO

 CREATE OR ALTER PROCEDURE [dbo].[user_createNewUser_I]
    @roleId INT,
	@verificationToken VARCHAR(MAX),
	@verificationTokenExpirationDate DATETIME,
	@isConfirmed BIT, 
	@userName VARCHAR(50),
	@email VARCHAR(50),
	@phoneNumber VARCHAR(50),
	@passwordHash VARCHAR(MAX),
	@newIdentity INT OUTPUT
AS
BEGIN 
	BEGIN TRY
		BEGIN TRANSACTION; 
	 
			IF EXISTS (SELECT* FROM Roles WHERE Id = @roleId) 
			BEGIN
				INSERT INTO ApplicationUsers(IsConfirmed,
							UserName, Email, PhoneNumber, PasswordHash, VerificationToken, 
							VerificationTokenExpirationDate, ResetToken, ResetTokenExpirationDate, Completed) 
					VALUES (@isConfirmed, @userName, 
							@email, @phoneNumber, @passwordHash, @verificationToken, 
							@verificationTokenExpirationDate, NULL, NULL, 1) 
				
					SET @newIdentity = CAST(SCOPE_IDENTITY() AS INT)
			
					INSERT INTO UserRoles(RoleId, UserId) VALUES(@roleId, @newIdentity)
			END

		COMMIT TRANSACTION;

		SELECT @newIdentity;
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

CREATE OR ALTER PROCEDURE user_confirmAccount_U
	@userId INT,
	@isConfirmed BIT
AS
BEGIN
	UPDATE ApplicationUsers SET IsConfirmed = 1
	WHERE Id = @userId
END
GO


CREATE OR ALTER PROCEDURE [dbo].[user_addToRole_I] 
	@userId INT, 
	@role INT,
	@companyId VARCHAR(MAX) = NULL,
	@trainerYearsExperience INT = NULL
AS 
BEGIN 
	BEGIN TRY  
		BEGIN TRANSACTION;

			IF(@companyId IS NOT NULL)
			BEGIN
				UPDATE ApplicationUsers SET CompanyId = @companyId
				WHERE Id = @userId;
			END;

			INSERT INTO UserRoles(UserId, RoleId) 
			VALUES (@userId, @role)
	
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

CREATE OR ALTER PROCEDURE [dbo].[user_clearResetToken_U] 
	@userId INT 
AS 
BEGIN 
	UPDATE ApplicationUsers 
	SET ResetToken = NULL, ResetTokenExpirationDate = NULL  
	WHERE Id = @userId
END
GO

CREATE OR ALTER PROCEDURE [dbo].[user_clearRevokedTokens_D]
AS
BEGIN
	DELETE FROM RefreshTokens
	WHERE Revoked IS NOT NULL
END
GO

CREATE OR ALTER PROCEDURE [dbo].[user_getUserByEmail_S]
	@email VARCHAR(50)
AS
BEGIN 
	SELECT 
	au.Id AS Id, 
	au.UserName,
	au.Email,
	au.PhoneNumber,
	au.PasswordHash,
	au.IsConfirmed,
	au.VerificationToken,
	au.VerificationTokenExpirationDate,
	au.ResetToken,
	au.ResetTokenExpirationDate,
	au.CompanyId,
	au.EmployeeCode,
	au.DepartmentCode,
	rt.Id,
	rt.Token,
	rt.TokenExpirationDate,
	rt.Created,
	rt.Revoked,
	r.RoleId
	FROM ApplicationUsers AS au 
	INNER JOIN UserRoles AS r ON r.UserId = au.Id
	LEFT JOIN RefreshTokens AS rt ON rt.UserId = r.UserId
	WHERE au.Email = @email 
END
GO 

CREATE OR ALTER PROCEDURE [dbo].[user_getUserByToken_S]
	@tokenKey VARCHAR(MAX)
AS
BEGIN 
	SELECT
	au.Id, 
	au.UserName,
	au.Email,
	au.PhoneNumber,
	au.PasswordHash,
	au.IsConfirmed,
	au.VerificationToken,
	au.VerificationTokenExpirationDate,
	au.ResetToken,
	au.ResetTokenExpirationDate,
	au.CompanyId,
	au.EmployeeCode,
	au.DepartmentCode,
	rt.Id,
	rt.Token,
	rt.TokenExpirationDate,
	rt.Created,
	rt.Revoked,
	r.RoleId
	FROM RefreshTokens AS rt
	INNER JOIN ApplicationUsers AS au ON au.Id = rt.UserId
	INNER JOIN UserRoles AS r ON r.UserId = au.Id
	WHERE rt.Token = @tokenKey AND au.IsConfirmed = 1 
END
GO

CREATE OR ALTER PROCEDURE [dbo].[user_getUserByVerificationToken_S]
	@tokenKey VARCHAR(MAX)
AS
BEGIN 
	SELECT
	au.Id, 
	au.UserName,
	au.Email,
	au.PhoneNumber,
	au.PasswordHash,
	au.IsConfirmed,
	au.VerificationToken,
	au.VerificationTokenExpirationDate,
	au.ResetToken,
	au.ResetTokenExpirationDate,
	au.CompanyId,
	au.EmployeeCode,
	au.DepartmentCode,
	rt.Id,
	rt.Token,
	rt.TokenExpirationDate,
	rt.Created,
	rt.Revoked,
	r.RoleId
	FROM ApplicationUsers AS au
	INNER JOIN UserRoles AS r ON r.UserId = au.Id
	LEFT JOIN RefreshTokens AS rt ON rt.UserId = r.UserId 
	WHERE au.VerificationToken = @tokenKey
END
GO

CREATE OR ALTER PROCEDURE [dbo].[user_getUserByResetToken_S]
	@tokenKey VARCHAR(MAX)
AS
BEGIN 
	SELECT
	au.Id, 
	au.UserName,
	au.Email,
	au.PhoneNumber,
	au.PasswordHash,
	au.IsConfirmed,
	au.VerificationToken,
	au.VerificationTokenExpirationDate,
	au.ResetToken,
	au.ResetTokenExpirationDate,
	au.CompanyId,
	au.EmployeeCode,
	au.DepartmentCode,
	rt.Id,
	rt.Token,
	rt.TokenExpirationDate,
	rt.Created,
	rt.Revoked,
	r.RoleId
	FROM ApplicationUsers AS au
	INNER JOIN UserRoles AS r ON r.UserId = au.Id
	LEFT JOIN RefreshTokens AS rt ON rt.UserId = r.UserId 
	WHERE au.ResetToken = @tokenKey 
END
GO

CREATE OR ALTER PROCEDURE [dbo].[user_getUserRoles_S]
@userId INT
AS
BEGIN
	SELECT r.RoleName FROM UserRoles AS ur
	INNER JOIN Roles AS r ON r.Id = ur.RoleId
	WHERE ur.UserId = @userId
END
GO

CREATE OR ALTER PROCEDURE [dbo].[user_updatePassword_U]
	@userId INT,
	@newPassword VARCHAR(MAX)
AS
BEGIN
	UPDATE ApplicationUsers SET PasswordHash = @newPassword
	WHERE Id = @userId
END
GO

CREATE OR ALTER PROCEDURE [dbo].[user_updateUserData_U]
	@userId INT,
	@email VARCHAR(50) = NULL, 
	@password VARCHAR(MAX) = NULL,
	@phoneNumber VARCHAR(50) = NULL,
	@resetToken VARCHAR(MAX) = NULL,
	@resetTokenExpirationDate DATETIME = NULL,
	@refreshTokens UserRefreshTokensTableType READONLY 
AS
BEGIN 
	BEGIN TRY  
		BEGIN TRANSACTION --Add conditions

			MERGE RefreshTokens AS target
			USING (SELECT Token, TokenExpirationDate, Created, ReplacedByToken, Revoked FROM @refreshTokens) AS source
			ON (target.UserId = @userId AND target.Token = source.Token)
			WHEN MATCHED THEN 
				UPDATE SET Token = COALESCE(source.Token, target.Token),
						   TokenExpirationDate = CONVERT(DATETIME, source.TokenExpirationDate, 120), 
						   Created = CONVERT(DATETIME, source.Created, 120),
						   ReplacedByToken = COALESCE(source.ReplacedByToken, target.ReplacedByToken),
						   Revoked = COALESCE(source.Revoked, target.Revoked)
			WHEN NOT MATCHED THEN 
				INSERT(UserId, Token, TokenExpirationDate, Created, ReplacedByToken, Revoked) 
				VALUES(@userId, source.Token, source.TokenExpirationDate, source.Created,
				source.ReplacedByToken, source.Revoked);

			UPDATE ApplicationUsers 
			SET Email = COALESCE(@email, Email) , 
			PhoneNumber = COALESCE(@phoneNumber, PhoneNumber),
			PasswordHash = COALESCE(@password, PasswordHash),
			ResetToken = @resetToken,
			ResetTokenExpirationDate = @resetTokenExpirationDate,
			Modified = GETDATE()
			WHERE Id = @userId

		COMMIT
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