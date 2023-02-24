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
			EmployeeCode VARCHAR(MAX) NULL,
			DepartmentCode VARCHAR(MAX) NULL,
			CompanyId VARCHAR(MAX) NULL,
			ResetToken VARCHAR(MAX) NULL,
			ResetTokenExpirationDate DATETIME NULL,
			UniqueUserCode VARCHAR(MAX) NULL, 
			Created DATETIME NOT NULL DEFAULT GETDATE(),
			Modified DATETIME NOT NULL DEFAULT GETDATE()
		)
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
			UserId INT NOT NULL FOREIGN KEY REFERENCES ApplicationUsers(Id) ON DELETE CASCADE,
			RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(Id) ON DELETE CASCADE,
		    CONSTRAINT PK_Users_Roles PRIMARY KEY(UserId, RoleId)
		)
END