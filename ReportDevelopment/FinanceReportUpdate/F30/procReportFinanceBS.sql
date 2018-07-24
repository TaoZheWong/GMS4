ALTER PROCEDURE [dbo].[procReportFinanceBS]
@CoyID smallint, 
@Year smallint,
@Month smallint
AS 

DECLARE @StatusType char(1)

SELECT @StatusType = StatusType FROM tbCompany WHERE CoyID = @CoyID 

IF (@StatusType = 'L' OR @StatusType = 'S') 
BEGIN
	EXEC procReportFinanceSAPBS @CoyID, @Year, @Month
	RETURN 
END

DECLARE @CurrencyCode nvarchar(15),@CurrencySign nvarchar(5), @Company nvarchar(100), @Less Int,
@Conversion float, @LastMonthYear smallint, @LastMonthMonth smallint, @LastMonthDate datetime,
@LastLastMonthDate datetime 

SET @LastMonthDate = dateadd(m,-1,convert(datetime,cast(@Month as nvarchar(2))+'/1/'+cast(right(@Year,2) as nvarchar(2)),1)) 
SET @LastMonthYear = year(@LastMonthDate) 
SET @LastMonthMonth = month(@LastMonthDate) 
SET @LastLastMonthDate = dateadd(m,-2,convert(datetime,cast(@Month as nvarchar(2))+'/1/'+cast(right(@Year,2) as nvarchar(2)),1)) 

SELECT @CurrencyCode=C.DefaultCurrencyCode, @Company=C.Name, @CurrencySign=CurrencySign
FROM tbCompany C WITH (NOLOCK)
left join tbCurrency cr on c.DefaultCurrencyCode=cr.CurrencyCode
WHERE C.CoyID = @CoyID

IF @CurrencyCode = 'SGD' 
BEGIN
SET @Conversion=1.00
SET @Less=1
SET @CurrencyCode=@CurrencyCode+' 000s'
END
ELSE
BEGIN
SELECT @Conversion=MonthEndRate
FROM tbForeignExchangeRate WITH (NOLOCK) 
WHERE Year(CreatedDate) = @Year and Month(CreatedDate) = @Month 
AND HomeCurrencyCode = @CurrencyCode and ForeignCurrencyCode = 'SGD'

	IF (@CurrencyCode='IDR')
	BEGIN
		SET @Less=1000
		SET @CurrencyCode=@CurrencyCode+' 000,000s'
	END
	ELSE
	BEGIN
		SET @Less=1
		SET @CurrencyCode=@CurrencyCode+' 000s'
	END
END


SELECT @Company as Company,
@CurrencyCode as CurrencyCode, @Conversion as Conversion, @CurrencySign as CurrencySign,
F.ItemSN, REPLACE(F.ItemName,'(BS)','') as ItemName, F.CreatedDate,
CASE WHEN F.ItemSeqID > 61 THEN F.MTD ELSE F.MTD/@Less END AS MTD, 
CASE WHEN F.ItemSeqID > 61 THEN LM_MTD ELSE LM_MTD/@Less END AS LM_MTD, 
CASE WHEN F.ItemSeqID > 61 THEN LLM_MTD ELSE LLM_MTD/@Less END AS LLM_MTD, 
CASE WHEN F.ItemSeqID > 61 THEN LY_MTD ELSE LY_MTD/@Less END AS LY_MTD, 
CASE WHEN F.ItemSeqID > 61 THEN LLY_MTD ELSE LLY_MTD/@Less END AS LLY_MTD, 
CASE WHEN @CurrencyCode = 'SGD' OR @Conversion IS NULL THEN 0 ELSE F.MTD / @Conversion END AS MTD_SGD
FROM 
(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as MTD,
f.CreatedDate
FROM [tbFinanceItemSeq] p WITH (NOLOCK) LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
LEFT OUTER JOIN tbFinanceData f WITH (NOLOCK) ON p.ItemID = f.ItemID 
AND f.CoyID = @CoyID
AND f.ProjectID = -1 
AND f.DepartmentID = -1
AND f.SectionID = -1
AND f.tbYear = @Year
AND f.tbMonth = @Month
WHERE Report='BS'
)as F,
(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as LM_MTD
FROM [tbFinanceItemSeq] p WITH (NOLOCK) LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
LEFT OUTER JOIN tbFinanceData f WITH (NOLOCK) ON p.ItemID = f.ItemID 
AND f.CoyID = @CoyID
AND f.ProjectID = -1 
AND f.DepartmentID = -1
AND f.SectionID = -1
AND f.tbYear = Year(@LastMonthDate)
AND f.tbMonth = Month(@LastMonthDate)
WHERE Report='BS')as LM,
(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as LLM_MTD
FROM [tbFinanceItemSeq] p LEFT OUTER JOIN tbFinanceItem i ON p.ItemID = i.ItemID 
LEFT OUTER JOIN tbFinanceData f ON p.ItemID = f.ItemID 
AND f.CoyID = @CoyID
AND f.ProjectID = -1 
AND f.DepartmentID = -1
AND f.SectionID = -1
AND f.tbYear = Year(@LastLastMonthDate)
AND f.tbMonth = Month(@LastLastMonthDate)
WHERE Report='BS')as LLM,
(SELECT f.CoyID, f.tbYear as Year, p.ItemSeqID, p.ItemSN, i.ItemName, 
CASE WHEN f.Total IS NULL THEN 0 ELSE f.Total end as LY_MTD
FROM [tbFinanceItemSeq] p WITH (NOLOCK) LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
LEFT OUTER JOIN tbFinanceAuditData f WITH (NOLOCK) ON p.ItemID = f.ItemID 
AND f.CoyID = @CoyID
AND f.tbYear = Year(@Year-1)
WHERE Report='BS')as LY,
(SELECT f.CoyID, f.tbYear as Year, p.ItemSeqID, p.ItemSN, i.ItemName, 
CASE WHEN f.Total IS NULL THEN 0 ELSE f.Total end as LLY_MTD
FROM [tbFinanceItemSeq] p WITH (NOLOCK) LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
LEFT OUTER JOIN tbFinanceAuditData f WITH (NOLOCK) ON p.ItemID = f.ItemID 
AND f.CoyID = @CoyID
AND f.tbYear = Year(@Year-2)
WHERE Report='BS')as LLY
WHERE F.ItemSeqID = LM.ItemSeqID
AND LM.ItemSeqID = LLM.ItemSeqID
AND LLM.ItemSeqID = LY.ItemSeqID
AND LY.ItemSeqID = LLY.ItemSeqID 
ORDER BY F.ItemSeqID

