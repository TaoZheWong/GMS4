CREATE PROCEDURE [dbo].[procAppInsertClaim]
@ClaimNo nvarchar(50),
@CoyID	smallint,
@ClaimDate datetime,
@CreatedBy smallint,
@Description nvarchar(max)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tbEntertainmentClaim
	(ClaimNo,CoyID,Description,ClaimDate,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Status,dim1,dim2,dim3,dim4)
	VALUES
	(@ClaimNo,@CoyID,@Description,@ClaimDate,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),0,-1,-1,-1,-1)
	
END
GO
