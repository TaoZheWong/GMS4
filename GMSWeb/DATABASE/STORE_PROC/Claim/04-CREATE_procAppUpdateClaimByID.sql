CREATE PROCEDURE [dbo].[procAppUpdateClaimByID]
@ClaimID int,
@dim1 smallint,
@dim2 smallint,
@dim3 smallint,
@dim4 smallint,
@Cust1 nvarchar(max),
@Cust2 nvarchar(max),
@Cust3 nvarchar(max),
@Person1 nvarchar(200),
@Person2 nvarchar(200),
@Person3 nvarchar(200),
@Desig1 nvarchar(200),
@Desig2 nvarchar(200),
@Desig3 nvarchar(200),
@Phone1 nvarchar(20),
@Phone2 nvarchar(20),
@Phone3 nvarchar(20),
@ClaimDate nvarchar(50),
@Description nvarchar(max)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tbEntertainmentClaim
	Set 
	dim1 = @dim1,
	dim2 = @dim2,
	dim3 = @dim3,
	dim4 = @dim4,
	Cust1 = @Cust1,
	Cust2 = @Cust2,
	Cust3 = @Cust3,
	Person1 = @Person1,
	Person2 = @Person2,
	Person3 = @Person3,
	Phone1 = @Phone1,
	Phone2 = @Phone2,
	Phone3 = @Phone3,
	Desig1 = @Desig1,
	Desig2 = @Desig2,
	Desig3 = @Desig3,
	Description = @Description,
	ClaimDate = CONVERT(datetime,@ClaimDate,104),
	ModifiedDate = GETDATE()
	WHERE ClaimID = @ClaimID
END
GO
