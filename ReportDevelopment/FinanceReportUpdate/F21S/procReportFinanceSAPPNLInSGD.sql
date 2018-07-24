ALTER PROCEDURE [dbo].[procReportFinanceSAPPNLInSGD]
@CoyID smallint, 
@ProjectID smallint,
@DepartmentID smallint,
@SectionID smallint,
@Year smallint,
@Month smallint,
@UnitID smallint
AS 

DECLARE @CurrencyCode nvarchar(5), @Company nvarchar(100), @Unit nvarchar(100),
@Division nvarchar(100), @Department nvarchar(100), @Section nvarchar(100),
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
set @Conversion=1.00
END
ELSE
BEGIN
SELECT @Conversion=MonthEndRate
FROM tbForeignExchangeRate WITH (NOLOCK) 
WHERE Year(CreatedDate) = @Year and Month(CreatedDate) = @Month 
AND HomeCurrencyCode = @CurrencyCode and ForeignCurrencyCode = 'SGD'
END

SELECT @Company as Company, @Division as Division, @Department as Department, @Section as Section, @Unit as Unit,
@Conversion as Conversion,
F.ItemSN, REPLACE(F.ItemName,'(PNL)','') as ItemName, F.CreatedDate,
CASE WHEN F.ItemSeqID < 56 THEN F.MTD/@Conversion ELSE F.MTD END as MTD, 
CASE WHEN F.ItemSeqID < 56 THEN F.YTD/@Conversion ELSE F.YTD END as YTD, 
CASE WHEN F.ItemSeqID < 56 THEN LM.MTD/@Conversion ELSE LM.MTD END as LM_MTD, 
CASE WHEN F.ItemSeqID < 56 THEN MTDBudgetTotal/@Conversion ELSE MTDBudgetTotal END as MTDBudgetTotal, 
CASE WHEN YTDBudgetTotal IS NULL THEN 0 
WHEN F.ItemSeqID < 56 THEN YTDBudgetTotal/@Conversion
ELSE YTDBudgetTotal END AS YTDBudgetTotal, 
CASE WHEN F.ItemSeqID < 56 THEN L.MTD/@Conversion ELSE L.MTD END as LastYearMTD, 
CASE WHEN F.ItemSeqID < 56 THEN L.YTD/@Conversion ELSE L.YTD END as LastYearYTD,
CASE WHEN F.ItemSeqID < 56 THEN F.MTD/@Conversion - MTDBudgetTotal/@Conversion ELSE F.MTD - MTDBudgetTotal END as MTDVariance,
CASE 
  WHEN YTDBudgetTotal IS NULL AND F.ItemSeqID < 56 THEN F.YTD/@Conversion 
  WHEN YTDBudgetTotal IS NOT NULL AND F.ItemSeqID < 56 THEN F.YTD/@Conversion - YTDBudgetTotal/@Conversion
  WHEN YTDBudgetTotal IS NULL AND F.ItemSeqID > 55 THEN F.YTD
  WHEN YTDBudgetTotal IS NOT NULL AND F.ItemSeqID > 55 THEN F.YTD - YTDBudgetTotal
  ELSE 0 END as YTDVariance,
CASE 
  WHEN L.YTD IS NULL AND F.ItemSeqID < 56 THEN F.YTD/@Conversion 
  WHEN L.YTD IS NOT NULL AND F.ItemSeqID < 56 THEN F.YTD/@Conversion - L.YTD/@Conversion
  WHEN L.YTD IS NULL AND F.ItemSeqID > 55 THEN F.YTD
  WHEN L.YTD IS NOT NULL AND F.ItemSeqID > 55 THEN F.YTD - L.YTD
  ELSE 0 END as LY_YTDVariance
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
	CASE WHEN b.Total IS NULL THEN 0 ELSE b.Total end as MTDBudgetTotal
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
	WHERE p.Report='PNL') as L,
	(SELECT b.CoyID, b.BudgetYear, p.ItemSeqID, 
	b.Total AS YTDBudgetTotal
	FROM tbFinanceItemSeq p WITH (NOLOCK)
	LEFT OUTER JOIN 
		(SELECT b.CoyID, @Year as BudgetYear, b.ItemID, SUM(b.Total) as Total
		FROM tbFinanceItem i WITH (NOLOCK)
		LEFT OUTER JOIN tbBudgetForFinance b WITH (NOLOCK) ON i.ItemID = b.ItemID 
		AND b.CoyID = @CoyID
		AND b.ProjectID = @ProjectID
		AND b.DepartmentID = @DepartmentID
		AND b.SectionID = @SectionID
		AND b.BudgetYear = @Year
		AND b.BudgetMonth = @Month
		--AND ((@Month BETWEEN 4 AND 12 AND ((BudgetMonth>=4 AND BudgetMonth<=@Month AND BudgetYear=@Year) 
		--			OR (BudgetMonth BETWEEN 1 AND 3 AND BudgetYear=@Year+1)))
		--		OR (@Month BETWEEN 1 AND 3 AND ((BudgetMonth BETWEEN 4 AND 12 AND BudgetYear=@Year-1) 
		--			OR (BudgetMonth<=@Month AND BudgetYear=@Year))))
		AND i.ItemName NOT IN
		('Gross Profits Margin %', '- External Sales GP %', '- Interco Sales GP %',
		'Selling & Dist Margin %', 'A&G Margin Direct %', 'A&G Margin Total %',
		'Operating Profit Margin %', 'Profit Before Taxation %', 'Profit After Taxation %') 
		GROUP BY b.CoyID,  b.ItemID
		UNION 
		SELECT b.CoyID, b.BudgetYear, b.ItemID, b.YTDTotal as Total
		FROM tbFinanceItemSeq p WITH (NOLOCK) 
		LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID
		LEFT OUTER JOIN tbBudgetForFinance b WITH (NOLOCK) ON p.ItemID = b.ItemID
		AND b.CoyID = @CoyID
		AND b.ProjectID = @ProjectID
		AND b.DepartmentID = @DepartmentID
		AND b.SectionID = @SectionID
		AND b.BudgetYear = @Year
		AND b.BudgetMonth = @Month
		AND i.ItemName IN
		('Gross Profits Margin %', '- External Sales GP %', '- Interco Sales GP %',
		'Selling & Dist Margin %', 'A&G Margin Direct %', 'A&G Margin Total %',
		'Operating Profit Margin %', 'Profit Before Taxation %', 'Profit After Taxation %') 
		WHERE p.Report='PNL') AS b ON p.ItemID = b.ItemID
	WHERE p.Report='PNL') AS YB
WHERE F.ItemSeqID = B.ItemSeqID
AND B.ItemSeqID = L.ItemSeqID
AND L.ItemSeqID = YB.ItemSeqID
AND YB.ItemSeqID = LM.ItemSeqID 
ORDER BY F.ItemSeqID
