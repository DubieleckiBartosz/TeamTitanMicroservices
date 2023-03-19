IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ApplicationUsers' and xtype='U')
BEGIN

	CREATE TABLE ApplicationUsers
		(
		    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,  
			UserName VARCHAR(50) NULL,
		    Email VARCHAR(50) NULL,
		    PasswordHash VARCHAR(MAX) NULL,
		    PhoneNumber VARCHAR(50) NULL,
			IsConfirmed BIT DEFAULT 0,
			VerificationToken VARCHAR(MAX) NULL,
			VerificationTokenExpirationDate DATETIME NULL,
			VerificationCode VARCHAR(50) NULL, 
			OrganizationCode VARCHAR(50) NULL, 
			ResetToken VARCHAR(MAX) NULL,
			ResetTokenExpirationDate DATETIME NULL, 
			Created DATETIME NOT NULL DEFAULT GETDATE(),
			Modified DATETIME NOT NULL DEFAULT GETDATE()
		)

			CREATE UNIQUE INDEX IX_ApplicationUsers_VerificationCode 
			ON ApplicationUsers (VerificationCode) 
			WHERE VerificationCode IS NOT NULL 
END
 
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RefreshTokens' and xtype='U')
BEGIN
		CREATE TABLE [dbo].[RefreshTokens]
		(
			Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
			UserId INT NOT NULL,
			Token VARCHAR(MAX) NULL,
			TokenExpirationDate DATETIME NOT NULL,
			Created DATETIME NOT NULL,
			ReplacedByToken VARCHAR(MAX) NULL,
			Revoked VARCHAR(250) NULL
		)
		
		ALTER TABLE [dbo].[RefreshTokens] ADD  CONSTRAINT [FK_RefreshToken_ApplicationUsers] FOREIGN KEY([UserId])
		REFERENCES [dbo].[ApplicationUsers] ([Id]) 

		
--TYPE
		
		CREATE TYPE [dbo].[UserRefreshTokensTableType] AS TABLE
		(
			Token VARCHAR(max) NULL,
			TokenExpirationDate DATETIME NOT NULL,
			Created DATETIME NOT NULL,
			ReplacedByToken VARCHAR(max) NULL,
			Revoked VARCHAR(250) NULL	
		)

END


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Roles' and xtype='U')
BEGIN
		CREATE TABLE [dbo].[Roles]
			(
				Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
				RoleName VARCHAR(25) NULL,
			)

		INSERT INTO Roles VALUES('Admin'), ('Owner'), ('Manager'), ('Employee'), ('User')
								
END


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserRoles' and xtype='U')
BEGIN
		CREATE TABLE [dbo].[UserRoles]
		(
			RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(Id) ON DELETE CASCADE,
			UserId INT NOT NULL FOREIGN KEY REFERENCES ApplicationUsers(Id) ON DELETE CASCADE,
			VerificationCode VARCHAR(50) NULL,
		    CONSTRAINT PK_Users_Roles PRIMARY KEY(UserId, RoleId)
		)
END


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TempUsers' and xtype='U')
BEGIN

	CREATE TABLE TempUsers
		(
		    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,   
			RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(Id) ON DELETE CASCADE, 
			VerificationCode VARCHAR(50) NOT NULL, 
			OrganizationCode VARCHAR(50) NOT NULL,  
			Created DATETIME NOT NULL DEFAULT GETDATE(),
			Modified DATETIME NOT NULL DEFAULT GETDATE(),
			CONSTRAINT UC_Organization_VerificationCode UNIQUE (VerificationCode, OrganizationCode)
		)
END