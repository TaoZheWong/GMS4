ALTER PROCEDURE [dbo].[procReportFinanceSAPPNLDetailByMonth]
@CoyID smallint, 
@ProjectID smallint,
@DepartmentID smallint,
@SectionID smallint,
@Year smallint,
@UnitID smallint
AS 

DECLARE @CurrencyCode nvarchar(15), @Company nvarchar(100), @Less Int, @Unit nvarchar(100),
@Division nvarchar(100), @Department nvarchar(100), @Section nvarchar(100)

--Get the latest year month
DECLARE @BIGYEAR smallint, @BIGMONTH smallint
SELECT TOP 1 @BIGYEAR=tbYear, @BIGMONTH=tbMonth
FROM tbFinanceDataSAP 
WHERE CoyID = @CoyID AND ProjectID = @ProjectID AND DepartmentID = @DepartmentID	AND SectionID = @SectionID
	AND tbYear IN (@Year, @Year+1)
	AND 
	(
	(tbYear = 2017 AND tbMonth >= 4 AND tbMonth <=12) OR 	
	(tbYear = 2017+1 AND tbMonth >= 1 AND tbMonth <=3)
	)
ORDER BY tbYear DESC, tbMonth DESC

IF (@BIGYEAR=@Year+1 AND @BIGMONTH>3)
	SET @BIGMONTH=3

--Company Details
SELECT @CurrencyCode=C.DefaultCurrencyCode, @Company=C.Name, 
@Division=UPPER(ISNULL(cp.ProjectName,'Company')), @Department=UPPER(ISNULL(cd.DepartmentName,'None')), 
@Section=UPPER(ISNULL(cs.SectionName,'None')), @Unit=UPPER(ISNULL(cu.UnitName,'None'))
FROM tbCompany C WITH (NOLOCK)
LEFT JOIN tbCompanyProject cp WITH (NOLOCK) on cp.coyid=c.coyid AND cp.ProjectID = @ProjectID
LEFT JOIN tbCompanyDepartment cd WITH (NOLOCK) on cd.coyid=c.coyid AND cd.DepartmentID=@DepartmentID
LEFT JOIN tbCompanySection cs WITH (NOLOCK) on cs.coyid=c.coyid AND cs.sectionid=@SectionID
LEFT JOIN tbCompanyUnit cu WITH (NOLOCK) on cu.coyid=c.coyid AND cu.UnitID=@UnitID
WHERE C.CoyID = @CoyID

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


SELECT @Company as Company, @Division as Division, @Department as Department, @Section as Section, @Unit as Unit,
@CurrencyCode as CurrencyCode,
ItemSeqID, ItemSN, REPLACE(ItemName,'(PNLD)','') as ItemName,
	CASE WHEN SUM(JAN) IS NULL THEN 0 ELSE SUM(JAN)/@Less END AS JAN, 
	CASE WHEN SUM(FEB) IS NULL THEN 0 ELSE SUM(FEB)/@Less END AS FEB, 
	CASE WHEN SUM(MAR) IS NULL THEN 0 ELSE SUM(MAR)/@Less END AS MAR, 
	CASE WHEN SUM(APR) IS NULL THEN 0 ELSE SUM(APR)/@Less END AS APR, 
	CASE WHEN SUM(MAY) IS NULL THEN 0 ELSE SUM(MAY)/@Less END AS MAY, 
	CASE WHEN SUM(JUN) IS NULL THEN 0 ELSE SUM(JUN)/@Less END AS JUN, 
	CASE WHEN SUM(JUL) IS NULL THEN 0 ELSE SUM(JUL)/@Less END AS JUL, 
	CASE WHEN SUM(AUG) IS NULL THEN 0 ELSE SUM(AUG)/@Less END AS AUG, 
	CASE WHEN SUM(SEP) IS NULL THEN 0 ELSE SUM(SEP)/@Less END AS SEP, 
	CASE WHEN SUM(OCT) IS NULL THEN 0 ELSE SUM(OCT)/@Less END AS OCT, 
	CASE WHEN SUM(NOV) IS NULL THEN 0 ELSE SUM(NOV)/@Less END AS NOV, 
	CASE WHEN SUM(DEC) IS NULL THEN 0 ELSE SUM(DEC)/@Less END AS DEC, 
	CASE WHEN SUM(TOTAL) IS NULL THEN 0 ELSE SUM(TOTAL)/@Less END AS TOTAL,
	CASE WHEN SUM(LASTYEAR) IS NULL THEN 0 ELSE SUM(LASTYEAR)/@Less END AS LASTYEAR, 
	MAX(CreateDDate) As CreatedDate,
    CASE WHEN SUM(TOTAL) IS NOT NULL AND SUM(LASTYEAR)IS NOT NULL THEN -((SUM(TOTAL) - SUM(LASTYEAR)) / ISNULL(NULLIF(SUM(LASTYEAR),0),1*-100)) * 100 ELSE 0 END AS VARIANCE
	FROM
	(SELECT f.CoyID, f.tbYear as Year, f.tbMonth as Month, p.ItemSeqID, p.ItemSN, i.ItemName,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 4 THEN f.MTD end as APR,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 5 THEN f.MTD end as MAY,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 6 THEN f.MTD end as JUN,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 7 THEN f.MTD end as JUL,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 8 THEN f.MTD end as AUG,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 9 THEN f.MTD end as SEP,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 10 THEN f.MTD end as OCT,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 11 THEN f.MTD end as NOV,
	CASE WHEN f.tbYear = @Year AND f.tbMonth = 12 THEN f.MTD end as DEC, 
	CASE WHEN f.tbYear = @Year+1 AND f.tbMonth = 1 THEN f.MTD end as JAN,
	CASE WHEN f.tbYear = @Year+1 AND f.tbMonth = 2 THEN f.MTD end as FEB,
	CASE WHEN f.tbYear = @Year+1 AND f.tbMonth = 3 THEN f.MTD end as MAR,
	CASE WHEN (f.tbYear = @BIGYEAR AND F.tbMonth=@BIGMONTH) THEN f.YTD end as TOTAL,
	CASE WHEN ((f.tbYear = @Year-1 AND F.tbMonth>=4) OR (f.tbYear = @Year AND F.tbMonth<=3)) THEN f.MTD end as LASTYEAR,
	f.CreatedDate
	FROM tbFinanceItemSeq p WITH (NOLOCK)
	LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
	LEFT OUTER JOIN tbFinanceDataSAP f WITH (NOLOCK) ON p.ItemID = f.ItemID
	AND f.CoyID = @CoyID
	AND f.ProjectID = @ProjectID
	AND f.DepartmentID = @DepartmentID
	AND f.SectionID = @SectionID
	AND f.UnitID = @UnitID
	AND f.tbYear IN (@Year-1, @Year, @Year+1)
	where P.Report ='PNLD'
	)AS A 
	GROUP BY CoyID, ItemSeqID, ItemSN, ItemName
	ORDER BY ItemSeqID

