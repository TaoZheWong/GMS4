CREATE PROCEDURE procAppSelectEntertainmentClaimDetailByClaimID
@ID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	COUNT(a.ClaimDetailID) As 'Count',
	d.id,
	CONVERT(varchar,d.claimDate,103) as 'date',
	d.type,
	d.amount,
	d.currencyCode,
	d.remark,
	d.currencyRate,
	convert(float,d.amount),
	convert(float,d.currencyRate * d.amount) As 'amountSGD'
	FROM tbEntertainmentClaim_Detail d WITH(NOLOCK)
	LEFT JOIN tbEntertainmentClaim_Attachment a on d.id = a.ClaimDetailID
	WHERE d.ClaimID = @ID
	GROUP BY d.id, d.claimDate, d.type, d.amount, d.currencyCode, d.remark, d.currencyRate, d.amount

	
END
GO
