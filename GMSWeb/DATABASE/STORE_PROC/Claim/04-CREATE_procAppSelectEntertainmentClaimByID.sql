CREATE PROCEDURE procAppSelectEntertainmentClaimByID
@ClaimID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT c.ClaimID, u.UserRealName, company.Name as 'CompanyName', c.ClaimNo, CONVERT(varchar,c.claimDate,103) as 'ClaimDate',
	c.dim1,
	c.dim2,
	c.dim3,
	c.dim4,
	c.Cust1,
	c.Cust2,
	c.Cust3,
	c.Person1,
	c.Person2,
	c.Person3,
	c.Desig1,
	c.Desig2,
	c.Desig3,
	c.Phone1,
	c.Phone2,
	c.Phone3,
	c.Description,
	c.Status ,
	c.CreatedBy,
	CASE 
		WHEN c.Status = 0 THEN 'Open' 
		WHEN c.Status = 1 THEN 'Pending' 
		WHEN c.Status = 2 THEN 'Approved' 
		WHEN c.Status = 3 THEN 'Rejected' 
	End As StatusName
	FROM tbEntertainmentClaim c WITH(NOLOCK) 
	INNER JOIN aspnet_Users u WITH(NOLOCK)  on u.UserNumId = c.CreatedBy
	INNER JOIN tbCompany company WITH(NOLOCK) on company.CoyID = c.CoyID 
	where ClaimID = @ClaimID
END
GO
