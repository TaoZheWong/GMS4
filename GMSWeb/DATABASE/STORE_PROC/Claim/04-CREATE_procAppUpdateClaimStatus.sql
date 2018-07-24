CREATE PROCEDURE [dbo].[procAppUpdateClaimStatus]
@ClaimID int,
@Status int

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tbEntertainmentClaim
	SET Status = @Status
	WHERE 
	ClaimID = @ClaimID
	
	
END
GO