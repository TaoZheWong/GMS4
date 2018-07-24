CREATE PROCEDURE [dbo].[procAppGetAccessRight]
@userId int,
@claimantID int,
@Action int
	
AS 

SELECT m.Name , 'true' As 'Access' 
FROM tbUserAccessModule u WITH (NOLOCK)
INNER JOIN tbModule m WITH (NOLOCK) on m.ModuleID = u.ModuleID AND m.ModuleID = @Action
INNER JOIN tbEntertainmentClaim_Approve a on a.UserNumID = u.UserNumID and a.OnBehalfUserNumID = @claimantID 
WHERE u.UserNumID = @UserID






