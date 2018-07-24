CREATE PROCEDURE procAppSelectCompanyDepartmentByCoyIDAndProjectID
@CoyID int,
@ProjectID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT d.* FROM tbCompanyDepartment d WITH (NOLOCK) 
	WHERE d.CoyID = @CoyID
	and d.ProjectID = @ProjectID
END
GO
