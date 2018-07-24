SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[procAppSelectionGetCustomerListByID]
@CoyID smallint,
@Name nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

    SELECT TOP 100 AccountCode As 'value', AccountName As 'label' FROM tbAccount 
    WHERE CoyID = @CoyID
    AND AccountName like @Name
    
END
GO
