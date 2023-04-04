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


