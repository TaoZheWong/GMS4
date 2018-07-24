CREATE PROCEDURE [dbo].[procAppDeleteClaimDetailByID]
@ClaimDetailID int

AS
BEGIN
	SET NOCOUNT ON;

	DELETE tbEntertainmentClaim_Detail WHERE id = @ClaimDetailID
END
GO