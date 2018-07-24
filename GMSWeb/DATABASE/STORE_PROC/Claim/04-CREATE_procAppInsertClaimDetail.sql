CREATE PROCEDURE [dbo].[procAppInsertClaimDetail]
@ClaimID int,
@type nvarchar(max),
@claimDate nvarchar(50),
@remark nvarchar(max),
@currencyCode nvarchar(50),
@currencyRate float,
@amount float

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tbEntertainmentClaim_Detail
	(
	ClaimID,
	type,
	claimDate,
	remark,
	currencyCode,
	currencyRate,
	amount,
	CreatedDate
	)
	VALUES
	(
	@ClaimID ,
	@type ,
	CONVERT(datetime,@claimDate,104) ,
	@remark ,
	@currencyCode ,
	@currencyRate ,
	@amount,
	GETDATE()
	)
END
GO