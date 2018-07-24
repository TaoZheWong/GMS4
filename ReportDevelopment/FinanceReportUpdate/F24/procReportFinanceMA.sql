ALTER PROCEDURE [dbo].[procReportFinanceMA]
@CoyID smallint, 
@ProjectID smallint,
@DepartmentID smallint,
@SectionID smallint,
@Year smallint,
@Month smallint,
@UnitID smallint
AS 

DECLARE @StatusType char(1)

SELECT @StatusType = StatusType FROM tbCompany WHERE CoyID = @CoyID 

IF (@StatusType = 'L' OR @StatusType = 'S') 
BEGIN
	EXEC procReportFinanceSAPMA @CoyID, @ProjectID, @DepartmentID, @SectionID, @Year, @Month, @UnitID
	RETURN 
END

SET @UnitID = -1

DECLARE @CurrencyCode nvarchar(3), @Company nvarchar(100), @Unit nvarchar(100),
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
@CurrencyCode+' 000s' as CurrencyCode, @Conversion as Conversion,
F.ItemSN, REPLACE(F.ItemName,'(MA)','') as ItemName, F.MTD as MTD, F.YTD as YTD, F.CreatedDate,
LM.MTD AS LM_MTD, MTDBudgetTotal, 
CASE WHEN YTDBudgetTotal IS NULL THEN 0 ELSE YTDBudgetTotal END AS YTDBudgetTotal, 
L.MTD as LastYearMTD, L.YTD as LastYearYTD,
F.MTD - MTDBudgetTotal as MTDVariance,
CASE WHEN YTDBudgetTotal IS NULL THEN F.YTD ELSE F.YTD - YTDBudgetTotal end as YTDVariance,
CASE WHEN L.YTD IS NULL THEN F.YTD ELSE F.YTD - L.YTD end as LY_YTDVariance
FROM 
	(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
	CASE WHEN f.MTD IS NULL THEN 0 ELSE f.MTD end as MTD, 
	CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as YTD, 
	(SELECT MAX(CreatedDate) FROM tbFinanceData WITH (NOLOCK) WHERE CoyID = @CoyID AND tbYear = @Year AND tbMonth = @Month) AS CreatedDate
	FROM tbFinanceItemSeq p WITH (NOLOCK) 
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
	LEFT OUTER JOIN tbFinanceData f WITH (NOLOCK) ON p.ItemID = f.ItemID 
	AND f.CoyID = @CoyID
	AND f.ProjectID = @ProjectID
	AND f.DepartmentID = @DepartmentID
	AND f.SectionID = @SectionID
	AND f.tbYear = @Year
	AND f.tbMonth = @Month
	WHERE p.Report='MA')as F,

	(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
	CASE WHEN f.MTD IS NULL THEN 0 ELSE f.MTD end as MTD, 
	CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as YTD, 
	f.CreatedDate
	FROM tbFinanceItemSeq p WITH (NOLOCK) 
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID
	LEFT OUTER JOIN tbFinanceData f WITH (NOLOCK) ON p.ItemID = f.ItemID
	AND f.CoyID = @CoyID
	AND f.ProjectID = @ProjectID
	AND f.DepartmentID = @DepartmentID
	AND f.SectionID = @SectionID
	AND f.tbYear = @LastMonthYear
	AND f.tbMonth = @LastMonthMonth
	WHERE p.Report='MA')as LM,

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
	WHERE p.Report='MA')as B,

	(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName, 
	CASE WHEN f.MTD IS NULL THEN 0 ELSE f.MTD end as MTD, 
	CASE WHEN f.YTD IS NULL THEN 0 ELSE f.YTD end as YTD
	FROM tbFinanceItemSeq p WITH (NOLOCK) 
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID
	LEFT OUTER JOIN tbFinanceData f WITH (NOLOCK) ON p.ItemID = f.ItemID
	AND f.CoyID = @CoyID
	AND f.ProjectID = @ProjectID
	AND f.DepartmentID = @DepartmentID
	AND f.SectionID = @SectionID
	AND f.tbYear = @Year - 1
	AND f.tbMonth = @Month
	WHERE p.Report='MA')as L,

	(SELECT b.CoyID, @Year as BudgetYear, p.ItemSeqID, 
	b.Total AS YTDBudgetTotal
	FROM tbFinanceItemSeq p WITH (NOLOCK)
	LEFT OUTER JOIN 
		(SELECT b.CoyID, b.ItemID, SUM(b.Total) as Total
		FROM tbFinanceItem i WITH (NOLOCK)
		LEFT OUTER JOIN tbBudgetForFinance b WITH (NOLOCK) ON i.ItemID = b.ItemID 
		AND b.CoyID = @CoyID
		AND b.ProjectID = @ProjectID
		AND b.DepartmentID = @DepartmentID
		AND b.SectionID = @SectionID
		AND ((@Month BETWEEN 4 AND 12 AND ((BudgetMonth>=4 AND BudgetMonth<=@Month AND BudgetYear=@Year) 
					OR (BudgetMonth BETWEEN 1 AND 3 AND BudgetYear=@Year+1)))
				OR (@Month BETWEEN 1 AND 3 AND ((BudgetMonth BETWEEN 4 AND 12 AND BudgetYear=@Year-1) 
					OR (BudgetMonth<=@Month AND BudgetYear=@Year))))
		GROUP BY b.CoyID, b.ItemID
		) AS b ON p.ItemID = b.ItemID
	WHERE p.Report='MA') AS YB
WHERE F.ItemSeqID = B.ItemSeqID
AND B.ItemSeqID = L.ItemSeqID
AND L.ItemSeqID = YB.ItemSeqID
AND YB.ItemSeqID = LM.ItemSeqID 
ORDER BY F.ItemSeqID
