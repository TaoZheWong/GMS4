ALTER PROCEDURE [dbo].[procReportFinanceSAPPNL]
@CoyID smallint, 
@ProjectID smallint,
@DepartmentID smallint,
@SectionID smallint,
@Year smallint,
@Month smallint,
@UnitID smallint
AS 

DECLARE @CurrencyCode nvarchar(15), @Company nvarchar(100), @Unit nvarchar(100),
@Division nvarchar(100), @Department nvarchar(100), @Section nvarchar(100), @Less float,
@Conversion float, @LastMonthYear smallint, @LastMonthMonth smallint, @LastMonthDate datetime 

SET @LastMonthDate = dateadd(m,-1,convert(datetime,cast(@Month as nvarchar(2))+'/1/'+cast(right(@Year,2) as nvarchar(2)),1)) 
SET @LastMonthYear = year(@LastMonthDate) 
SET @LastMonthMonth = month(@LastMonthDate) 

SELECT @CurrencyCode=C.DefaultCurrencyCode, @Company=C.Name,
@Division=UPPER(ISNULL(cp.ProjectName,'Company')), @Department=UPPER(ISNULL(cd.DepartmentName,'None')), 
@Section=UPPER(ISNULL(cs.SectionName,'None')), @Unit=UPPER(ISNULL(cu.UnitName,'None'))
FROM tbCompany C WITH (NOLOCK)
LEFT JOIN tbCompanyProject cp WITH (NOLOCK) on cp.coyid=c.coyid AND cp.ProjectID = @ProjectID
LEFT JOIN tbCompanyDepartment cd WITH (NOLOCK) on cd.coyid=c.coyid AND cd.DepartmentID=@DepartmentID
LEFT JOIN tbCompanySection cs WITH (NOLOCK) on cs.coyid=c.coyid AND cs.sectionid=@SectionID
LEFT JOIN tbCompanyUnit cu WITH (NOLOCK) on cu.coyid=c.coyid AND cu.UnitID=@UnitID
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

SELECT @Company as Company, @Division as Division, @Department as Department, @Section as Section, @Unit as Unit,
@CurrencyCode as CurrencyCode, @Conversion as Conversion,
F.ItemSN, REPLACE(F.ItemName,'(PNL)','') as ItemName, F.CreatedDate,
CASE WHEN F.ItemSeqID > 54 THEN F.MTD ELSE F.MTD/@Less END AS MTD, 
CASE WHEN F.ItemSeqID > 54 THEN F.YTD ELSE F.YTD/@Less END AS YTD, 
CASE WHEN F.ItemSeqID > 54 THEN LM.MTD ELSE LM.MTD/@Less END AS LM_MTD, 
CASE WHEN F.ItemSeqID > 54 THEN MTDBudgetTotal ELSE MTDBudgetTotal/@Less END AS MTDBudgetTotal, 
CASE WHEN F.ItemSeqID > 54 THEN YTDBudgetTotal ELSE YTDBudgetTotal/@Less END AS YTDBudgetTotal, 
CASE WHEN F.ItemSeqID > 54 THEN L.MTD ELSE L.MTD/@Less END AS LastYearMTD, 
CASE WHEN F.ItemSeqID > 54 THEN L.YTD ELSE L.YTD/@Less END AS LastYearYTD,
CASE WHEN F.ItemSeqID > 54 THEN F.MTD - MTDBudgetTotal ELSE (F.MTD - MTDBudgetTotal)/@Less END AS MTDVariance,
CASE WHEN F.ItemSeqID > 54 THEN CASE WHEN YTDBudgetTotal IS NULL THEN F.YTD ELSE F.YTD - YTDBudgetTotal end
	ELSE (CASE WHEN YTDBudgetTotal IS NULL THEN F.YTD ELSE F.YTD - YTDBudgetTotal end)/@Less END AS YTDVariance,
CASE WHEN F.ItemSeqID > 54 THEN CASE WHEN L.YTD IS NULL THEN F.YTD ELSE F.YTD - L.YTD end 
	ELSE (CASE WHEN L.YTD IS NULL THEN F.YTD ELSE F.YTD - L.YTD end)/@Less END AS LY_YTDVariance
FROM 
	(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
	CASE WHEN f.MTD IS NULL THEN 0 ELSE f.MTD end as MTD, 
	CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as YTD, 
	(SELECT MAX(CreatedDate) FROM tbFinanceDataSAP WITH (NOLOCK) WHERE CoyID = @CoyID AND tbYear = @Year AND tbMonth = @Month) AS CreatedDate
	FROM tbFinanceItemSeq p WITH (NOLOCK) 
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
	LEFT OUTER JOIN tbFinanceDataSAP f WITH (NOLOCK) ON p.ItemID = f.ItemID 
	AND f.CoyID = @CoyID
	AND f.ProjectID = @ProjectID
	AND f.DepartmentID = @DepartmentID
	AND f.SectionID = @SectionID
	AND f.UnitID = @UnitID
	AND f.tbYear = @Year
	AND f.tbMonth = @Month
	WHERE p.Report='PNL')as F,

	(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
	CASE WHEN f.MTD IS NULL THEN 0 ELSE f.MTD end as MTD, 
	CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as YTD, 
	f.CreatedDate
	FROM tbFinanceItemSeq p WITH (NOLOCK) 
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID
	LEFT OUTER JOIN tbFinanceDataSAP f WITH (NOLOCK) ON p.ItemID = f.ItemID
	AND f.CoyID = @CoyID
	AND f.ProjectID = @ProjectID
	AND f.DepartmentID = @DepartmentID
	AND f.SectionID = @SectionID
	AND f.UnitID = @UnitID
	AND f.tbYear = @LastMonthYear
	AND f.tbMonth = @LastMonthMonth
	WHERE p.Report='PNL')as LM,

	(SELECT b.CoyID, b.BudgetYear as Year, b.BudgetMonth as Month, p.ItemSeqID,
	CASE WHEN b.Total IS NULL THEN 0 ELSE b.Total end as MTDBudgetTotal,
	CASE WHEN b.YTDTotal IS NULL THEN 0 ELSE b.YTDTotal end as YTDBudgetTotal
	FROM tbFinanceItemSeq p WITH (NOLOCK) 
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID
	LEFT OUTER JOIN tbBudgetForFinance b WITH (NOLOCK) ON p.ItemID = b.ItemID
	AND b.CoyID = @CoyID
	AND b.ProjectID = @ProjectID
	AND b.DepartmentID = @DepartmentID
	AND b.SectionID = @SectionID
	AND b.BudgetYear = @Year
	AND b.BudgetMonth = @Month
	WHERE p.Report='PNL')as B,

	(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
	CASE WHEN f.MTD IS NULL THEN 0 ELSE f.MTD end as MTD, 
	CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as YTD
	FROM tbFinanceItemSeq p WITH (NOLOCK) 
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID
	LEFT OUTER JOIN tbFinanceDataSAP f WITH (NOLOCK) ON p.ItemID = f.ItemID
	AND f.CoyID = @CoyID
	AND f.ProjectID = @ProjectID
	AND f.DepartmentID = @DepartmentID
	AND f.SectionID = @SectionID
	AND f.UnitID = @UnitID
	AND f.tbYear = @Year - 1
	AND f.tbMonth = @Month
	WHERE p.Report='PNL')as L
WHERE F.ItemSeqID = B.ItemSeqID
AND B.ItemSeqID = L.ItemSeqID
AND L.ItemSeqID = LM.ItemSeqID 
ORDER BY F.ItemSeqID
