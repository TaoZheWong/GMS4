SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[procAppSelectionGetCurrencyRateListByCodeInSGD]
@CoyID smallint,
@DefaultCurrency nvarchar(50),
@Currency nvarchar(50),
@Year smallint,
@Month smallint

AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT @DefaultCurrency As 'label', '1' As 'value'
	UNION
    SELECT ForeignCurrencyCode As 'label', MonthEndRate As 'value' from tbForeignExchangeRate WITH(NOLOCK) 
	WHERE HomeCurrencyCode = @DefaultCurrency
	AND Year(CreatedDate) = ISNULL(@Year,YEAR(GETDATE()))
	AND Month(CreatedDate) = ISNULL(@Month,MONTH(GETDATE()))
	AND ForeignCurrencyCode like @Currency
    
END
GO



