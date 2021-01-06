CREATE PROCEDURE procAppDeleteEntertainmentClaimDetailAttachment
@ClaimAttachmentID int
AS


SET NOCOUNT ON

BEGIN
	DELETE FROM tbEntertainmentClaim_Attachment
	WHERE ID = @ClaimAttachmentID
END


GO
