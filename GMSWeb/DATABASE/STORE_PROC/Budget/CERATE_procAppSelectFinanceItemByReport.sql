CREATE PROCEDURE [dbo].[procAppSelectFinanceItemByReport]
@Report nvarchar(50)
AS 

BEGIN
	SELECT itemSN,itemName FROM tbFinanceItemSeq s
	INNER JOIN tbFinanceItem i on i.ItemID = s.ItemID
	WHERE s.Report = @Report
	ORDER BY ItemSeqID 
END
