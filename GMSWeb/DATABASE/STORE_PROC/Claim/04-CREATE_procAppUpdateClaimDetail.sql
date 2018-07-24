CREATE PROCEDURE [dbo].[procAppUpdateClaimDetail]
@ClaimDetailID int,
@type nvarchar(max),
@claimDate nvarchar(50),
@remark nvarchar(max),
@currencyCode nvarchar(50),
@currencyRate float,
@amount float

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tbEntertainmentClaim_Detail
	SET type = @type
	,claimDate = CONVERT(datetime,@claimDate,104),
	remark = @remark,
	currencyCode = @currencyCode ,
	currencyRate = @currencyRate,
	amount = @amount
	WHERE 
	id = @ClaimDetailID
	
	
END
GO