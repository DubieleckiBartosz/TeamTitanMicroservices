SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE user_mergeCodes_U  
	@verificationCode VARCHAR(50), 
	@organizationCode VARCHAR(50), 
	@userId INT
AS
BEGIN  
	BEGIN TRY  
		BEGIN TRANSACTION; 

			DECLARE @userTempTable TABLE (RoleId INT, VerificationCode VARCHAR(50), OrganizationCode VARCHAR(50)); 

			INSERT INTO @userTempTable(RoleId, VerificationCode, OrganizationCode)
			SELECT RoleId, VerificationCode, OrganizationCode FROM TempUsers 
			WHERE VerificationCode = @verificationCode 
			AND OrganizationCode = @organizationCode

			UPDATE au
			SET au.VerificationCode = t.VerificationCode,
				au.OrganizationCode = t.OrganizationCode,
				au.Modified = GETDATE()
			FROM ApplicationUsers au INNER JOIN @userTempTable t 
			ON t.VerificationCode = @verificationCode 
			AND t.OrganizationCode = @organizationCode
			AND au.Id = @userId;
			 
			INSERT INTO UserRoles (UserId, RoleId, VerificationCode) 
			SELECT @userId, RoleId, VerificationCode FROM @userTempTable  

			DELETE FROM TempUsers 
			WHERE VerificationCode = @verificationCode 
			AND OrganizationCode = @organizationCode

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


CREATE OR ALTER PROCEDURE user_clearUserCodes_UD  
	@userId INT,
	@verificationCode VARCHAR(50)
AS
BEGIN   
	BEGIN TRY  
		BEGIN TRANSACTION; 

			UPDATE ApplicationUsers 
			SET VerificationCode = NULL,
				OrganizationCode = NULL
			WHERE Id = @userId

			DELETE FROM UserRoles
			WHERE VerificationCode = @verificationCode

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


CREATE OR ALTER PROCEDURE user_getUserByCode_S  
	@uniqueCode VARCHAR(50) 
AS
BEGIN  
	SELECT 
		au.Id, 
		au.VerificationCode,
		au.OrganizationCode,
		au.Email,
		ur.RoleId AS [Role]
	FROM ApplicationUsers AS au
	INNER JOIN UserRoles AS ur ON ur.UserId = au.Id 
	WHERE au.VerificationCode = @uniqueCode 
END
GO


CREATE OR ALTER PROCEDURE user_getUserLessById_S  
	@userId INT
AS
BEGIN  
	SELECT 
		au.Id, 
		au.VerificationCode,
		au.OrganizationCode,
		au.Email,
		ur.RoleId AS [Role]
	FROM ApplicationUsers AS au
	INNER JOIN UserRoles AS ur ON ur.UserId = au.Id 
	WHERE au.Id = @userId
END
GO


CREATE OR ALTER PROCEDURE user_codeExists_S
	@code VARCHAR(50)
AS
BEGIN
	SELECT 1 FROM ApplicationUsers WHERE VerificationCode = @code
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
							VerificationTokenExpirationDate, ResetToken, ResetTokenExpirationDate) 
					VALUES (@isConfirmed, @userName, 
							@email, @phoneNumber, @passwordHash, @verificationToken, 
							@verificationTokenExpirationDate, NULL, NULL) 
				
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
	@role INT
AS 
BEGIN 
		INSERT INTO UserRoles(UserId, RoleId) 
		VALUES (@userId, @role) 
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
	au.VerificationCode, 
	au.OrganizationCode,
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
	au.VerificationCode,
	au.OrganizationCode,
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
	au.VerificationCode, 
	au.OrganizationCode,
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
	au.VerificationCode, 
	au.OrganizationCode,
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
 
CREATE OR ALTER PROCEDURE [dbo].[temp_createUserCodes_I]  
	@roleId INT,
    @organizationCode VARCHAR(50),
    @verificationCode VARCHAR(50)
AS
BEGIN 
	INSERT INTO TempUsers(RoleId, VerificationCode, OrganizationCode)
	VALUES(@roleId, @verificationCode, @organizationCode)
END
GO