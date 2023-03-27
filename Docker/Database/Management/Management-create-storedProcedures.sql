SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE company_newComany_I
	@companyStatus INT,
	@companyCode VARCHAR(50),
	@companyName VARCHAR(250),
	@openFrom INT,
	@openTo INT,
	@ownerId INT
AS
BEGIN
	INSERT INTO [dbo].[Companies]
			   ([CompanyStatus],
				[CompanyCode],
				[CompanyName],
				[OpenFrom],
				[OpenTo],
				[OwnerId]) 
		 VALUES
			   (@companyStatus, 
			    @companyCode,
			    @companyName, 
			    @openFrom, 
			    @openTo, 
			    @ownerId)
END 
GO


