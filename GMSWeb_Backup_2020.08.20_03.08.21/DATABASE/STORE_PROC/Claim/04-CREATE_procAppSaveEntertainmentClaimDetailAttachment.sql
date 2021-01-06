CREATE PROCEDURE procAppSaveEntertainmentClaimDetailAttachment
@ClaimAttachmentID int,
@ClaimDetailID int,
@Attachment nvarchar(max)
AS


SET NOCOUNT ON

if (@ClaimAttachmentID > 0) 
BEGIN
	UPDATE tbEntertainmentClaim_Attachment
	SET attachment = @Attachment
	WHERE
	ID = @ClaimAttachmentID
END
ELSE
BEGIN
	INSERT INTO tbEntertainmentClaim_Attachment(ClaimDetailID , attachment)
	VALUES(@ClaimDetailID,@Attachment)
END


GO
