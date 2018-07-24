CREATE PROCEDURE procAppSelectCompanyProjectByCompanyID
@CoyID int
AS
BEGIN
	SET NOCOUNT ON;
	
	select * from tbCompanyProject WITH(NOLOCK) 
	WHERE CoyID = @CoyID
END
GO
