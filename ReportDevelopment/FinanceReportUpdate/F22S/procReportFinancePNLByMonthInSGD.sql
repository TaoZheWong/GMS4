ALTER PROCEDURE [dbo].[procReportFinancePNLByMonthInSGD]
@CoyID smallint, 
@ProjectID smallint,
@DepartmentID smallint,
@SectionID smallint,
@Year smallint,
@UnitID smallint
AS 

DECLARE @StatusType char(1)

SELECT @StatusType = StatusType FROM tbCompany WHERE CoyID = @CoyID 

IF (@StatusType = 'L' OR @StatusType = 'S') 
BEGIN
	EXEC procReportFinanceSAPPNLByMonth @CoyID, @ProjectID, @DepartmentID, @SectionID, @Year, @UnitID
	RETURN 
END

--Default value for non SAP
SET @UnitID = -1

DECLARE @CurrencyCode nvarchar(15),@DefaultCurrencyCode nvarchar(15), @Company nvarchar(100), @Less Int, @Unit nvarchar(100),
@Division nvarchar(100), @Department nvarchar(100), @Section nvarchar(100)

--Get the latest year month
DECLARE @BIGYEAR smallint, @BIGMONTH smallint
SELECT TOP 1 @BIGYEAR=tbYear, @BIGMONTH=tbMonth
FROM tbFinanceData 
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

--Fields required by Total Column
declare @GPM float, @ESGP float, @ISGP float, @SDM float, 
@AGMD float, @AGMT float, @PFO float, @PBT float, @PAT float

--Fields required by LastYear Column
declare @LY_GPM float, @LY_ESGP float, @LY_ISGP float, @LY_SDM float, 
@LY_AGMD float, @LY_AGMT float, @LY_PFO float, @LY_PBT float, @LY_PAT float

--To calculate the ratio for Total Column 
select top 1 @GPM = GPM, @ESGP = ESGP, @ISGP= ISGP, @SDM = SDM, 
@AGMD = AGMD, @AGMT = AGMT, @PFO = PFO, @PBT = PBT, @PAT=PAT
from
(select 
	case when isnull(sum(TotalSales),0) = 0 then 0 
	else isnull(sum(GrossProfits),0) * 100 / isnull(sum(TotalSales),0) end as GPM, 
	
	case when isnull(sum(ExternalSales),0) = 0 then 0 
	else (isnull(sum(ExternalSales),0) + isnull(sum(CostOfExternalSales),0)) * 100 / 
		isnull(sum(ExternalSales),0) end as ESGP, 
	
	case when isnull(sum(IntercoSales),0) = 0 then 0 
	else (isnull(sum(IntercoSales),0) + isnull(sum(CostOfIntercoSales),0)) * 100 / 
		isnull(sum(IntercoSales),0) end as ISGP,

	case when isnull(sum(TotalSales),0) = 0 then 0 
	else isnull(sum(TotalSDExpenses),0) * 100 / isnull(sum(TotalSales),0) end as SDM,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(AGDirectExpenses),0) * 100 / isnull(sum(TotalSales),0) end as AGMD,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(TotalAGExpenses),0) * 100 / isnull(sum(TotalSales),0) end as AGMT,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(ProfitsFromOperations),0) * 100 / isnull(sum(TotalSales),0) end as PFO,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(ProfitBeforeTaxation),0) / isnull(sum(TotalSales),0) end as PBT,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(ProfitAfterTaxation),0) / isnull(sum(TotalSales),0) end as PAT
from
(select 
case when itemname = '(PNL)Total Sales' then sum(f.ytd) end as 'TotalSales', 
case when itemname = '(PNL)Gross Profit' then sum(f.ytd) end as 'GrossProfits', 
case when itemname = '(PNL)External Sales' then sum(f.ytd) end as 'ExternalSales', 
case when itemname = '(PNL)Cost Of External Sales' then sum(f.ytd) end as 'CostOfExternalSales', 
case when itemname = '(PNL)Interco Sales' then sum(f.ytd) end as 'IntercoSales', 
case when itemname = '(PNL)Cost Of Interco Sales' then sum(f.ytd) end as 'CostOfIntercoSales',
case when itemname = '(PNL)Total S&D Expenses' then sum(f.ytd) end as 'TotalSDExpenses',
case when itemname = '(PNL)Total A&G Expenses' then sum(f.ytd) end as 'TotalAGExpenses',
case when itemname = '(PNL)A&G Direct Expenses' then sum(f.ytd) end as 'AGDirectExpenses',
case when itemname = '(PNL)Profit from Operations' then sum(f.ytd) end as 'ProfitsFromOperations', 
case when itemname = '(PNL)Profit Before Taxation' then sum(f.ytd) end as 'ProfitBeforeTaxation',
case when itemname = '(PNL)Profit After Taxation' then sum(f.ytd) end as 'ProfitAfterTaxation'
from tbFinanceData f WITH (NOLOCK) left outer join tbFinanceItem i WITH (NOLOCK) on f.ItemID = i.ItemID 
where f.CoyID = @CoyID and f.ProjectID = @ProjectID and f.DepartmentID = @DepartmentID and f.SectionID = @SectionID 
and F.tbYear = @BIGYEAR AND F.tbMonth=@BIGMONTH
and i.ItemName in 
('(PNL)Total Sales','(PNL)Gross Profit',
'(PNL)External Sales','(PNL)Cost Of External Sales',
'(PNL)Interco Sales','(PNL)Cost Of Interco Sales',
'(PNL)Total S&D Expenses','(PNL)Total A&G Expenses',
'(PNL)A&G Direct Expenses',
'(PNL)Profit from Operations',
'(PNL)Profit Before Taxation',
'(PNL)Profit After Taxation')
group by F.tbYear, i.itemname)as a)as b

--To calculate the ratio for LastYear Column 
select top 1 @LY_GPM = GPM, @LY_ESGP = ESGP, @LY_ISGP= ISGP, @LY_SDM = SDM, 
@LY_AGMD = AGMD, @LY_AGMT = AGMT, @LY_PFO = PFO, @LY_PBT = PBT, @LY_PAT=PAT
from
(select 
	case when isnull(sum(TotalSales),0) = 0 then 0 
	else isnull(sum(GrossProfits),0) * 100 / isnull(sum(TotalSales),0) end as GPM, 
	
	case when isnull(sum(ExternalSales),0) = 0 then 0 
	else (isnull(sum(ExternalSales),0) + isnull(sum(CostOfExternalSales),0)) * 100 / 
		isnull(sum(ExternalSales),0) end as ESGP, 
	
	case when isnull(sum(IntercoSales),0) = 0 then 0 
	else (isnull(sum(IntercoSales),0) + isnull(sum(CostOfIntercoSales),0)) * 100 / 
		isnull(sum(IntercoSales),0) end as ISGP,

	case when isnull(sum(TotalSales),0) = 0 then 0 
	else isnull(sum(TotalSDExpenses),0) * 100 / isnull(sum(TotalSales),0) end as SDM,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(AGDirectExpenses),0) * 100 / isnull(sum(TotalSales),0) end as AGMD,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(TotalAGExpenses),0) * 100 / isnull(sum(TotalSales),0) end as AGMT,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(ProfitsFromOperations),0) * 100 / isnull(sum(TotalSales),0) end as PFO,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(ProfitBeforeTaxation),0) / isnull(sum(TotalSales),0) end as PBT,
	
	case when isnull(sum(TotalSales),0) = 0 then 0 	
	else isnull(sum(ProfitAfterTaxation),0) / isnull(sum(TotalSales),0) end as PAT
from
(select 
case when itemname = '(PNL)Total Sales' then sum(f.mtd) end as 'TotalSales', 
case when itemname = '(PNL)Gross Profit' then sum(f.mtd) end as 'GrossProfits', 
case when itemname = '(PNL)External Sales' then sum(f.mtd) end as 'ExternalSales', 
case when itemname = '(PNL)Cost Of External Sales' then sum(f.mtd) end as 'CostOfExternalSales', 
case when itemname = '(PNL)Interco Sales' then sum(f.mtd) end as 'IntercoSales', 
case when itemname = '(PNL)Cost Of Interco Sales' then sum(f.mtd) end as 'CostOfIntercoSales',
case when itemname = '(PNL)Total S&D Expenses' then sum(f.mtd) end as 'TotalSDExpenses',
case when itemname = '(PNL)Total A&G Expenses' then sum(f.mtd) end as 'TotalAGExpenses',
case when itemname = '(PNL)A&G Direct Expenses' then sum(f.mtd) end as 'AGDirectExpenses',
case when itemname = '(PNL)Profit from Operations' then sum(f.mtd) end as 'ProfitsFromOperations', 
case when itemname = '(PNL)Profit Before Taxation' then sum(f.mtd) end as 'ProfitBeforeTaxation',
case when itemname = '(PNL)Profit After Taxation' then sum(f.mtd) end as 'ProfitAfterTaxation'
from tbFinanceData f WITH (NOLOCK) left outer join tbFinanceItem i WITH (NOLOCK) on f.ItemID = i.ItemID 
where f.CoyID = @CoyID and f.ProjectID = @ProjectID and f.DepartmentID = @DepartmentID and f.SectionID = @SectionID 
and ((f.tbYear = @Year-1 AND F.tbMonth>=4) OR (f.tbYear = @Year AND F.tbMonth<=3))
and i.ItemName in 
('(PNL)Total Sales','(PNL)Gross Profit',
'(PNL)External Sales','(PNL)Cost Of External Sales',
'(PNL)Interco Sales','(PNL)Cost Of Interco Sales',
'(PNL)Total S&D Expenses','(PNL)Total A&G Expenses',
'(PNL)A&G Direct Expenses',
'(PNL)Profit from Operations',
'(PNL)Profit Before Taxation',
'(PNL)Profit After Taxation')
group by f.tbYear, i.itemname)as a)as b

--Company Details
SELECT @CurrencyCode=C.DefaultCurrencyCode, @DefaultCurrencyCode=C.DefaultCurrencyCode, @Company=C.Name, 
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
END
ELSE
BEGIN
	SET @Less=1
END

SET @CurrencyCode='SGD 000s'

SELECT @Company as Company, @Division as Division, @Department as Department, @Section as Section, @Unit as Unit,
@CurrencyCode as CurrencyCode,
ItemSeqID, ItemSN, REPLACE(ItemName,'(PNL)','') as ItemName,
	CASE WHEN SUM(JAN) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(JAN)/@LESS ELSE SUM(JAN) END AS JAN, 
	CASE WHEN SUM(FEB) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(FEB)/@LESS ELSE SUM(FEB) END AS FEB, 
	CASE WHEN SUM(MAR) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(MAR)/@LESS ELSE SUM(MAR) END AS MAR, 
	CASE WHEN SUM(APR) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(APR)/@LESS ELSE SUM(APR) END AS APR, 
	CASE WHEN SUM(MAY) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(MAY)/@LESS ELSE SUM(MAY) END AS MAY, 
	CASE WHEN SUM(JUN) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(JUN)/@LESS ELSE SUM(JUN) END AS JUN, 
	CASE WHEN SUM(JUL) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(JUL)/@LESS ELSE SUM(JUL) END AS JUL, 
	CASE WHEN SUM(AUG) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(AUG)/@LESS ELSE SUM(AUG) END AS AUG, 
	CASE WHEN SUM(SEP) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(SEP)/@LESS ELSE SUM(SEP) END AS SEP, 
	CASE WHEN SUM(OCT) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(OCT)/@LESS ELSE SUM(OCT) END AS OCT, 
	CASE WHEN SUM(NOV) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(NOV)/@LESS ELSE SUM(NOV) END AS NOV, 
	CASE WHEN SUM(DEC) IS NULL THEN 0 WHEN ItemSeqID <= 27 THEN SUM(DEC)/@LESS ELSE SUM(DEC) END AS DEC, 
	--Total Column
	CASE WHEN ItemName = 'Gross Profits Margin %' THEN @GPM 
		 WHEN ItemName = '- External Sales GP %' THEN @ESGP 
		 WHEN ItemName = '- Interco Sales GP %' THEN @ISGP
		 WHEN ItemName = 'Selling & Dist Margin %' THEN @SDM
		 WHEN ItemName = 'A&G Margin Total %' THEN @AGMT
		 WHEN ItemName = 'Operating Profits Margin %' THEN @PFO
		 WHEN ItemName = 'Profits Before Taxation %' THEN @PBT
		 WHEN ItemName = 'Profit After Taxation %' THEN @PAT
		 WHEN SUM(TOTAL) IS NULL THEN 0 ELSE SUM(TOTAL)/@LESS END AS TOTAL, 
	--LastYear Column
	CASE WHEN ItemName = 'Gross Profits Margin %' THEN @LY_GPM 
		 WHEN ItemName = '- External Sales GP %' THEN @LY_ESGP 
		 WHEN ItemName = '- Interco Sales GP %' THEN @LY_ISGP
		 WHEN ItemName = 'Selling & Dist Margin %' THEN @LY_SDM
		 WHEN ItemName = 'A&G Margin Total %' THEN @LY_AGMT
		 WHEN ItemName = 'Operating Profits Margin %' THEN @LY_PFO
		 WHEN ItemName = 'Profits Before Taxation %' THEN @LY_PBT
		 WHEN ItemName = 'Profit After Taxation %' THEN @LY_PAT
		 WHEN SUM(LASTYEAR) IS NULL THEN 0 ELSE SUM(LASTYEAR)/@LESS END AS LASTYEAR, 
	MAX(CreateDDate) As CreatedDate,
	CASE 
		WHEN SUM(TOTAL) IS NOT NULL AND SUM(LASTYEAR)IS NOT NULL THEN -((SUM(TOTAL) - SUM(LASTYEAR)) / ISNULL(NULLIF(SUM(LASTYEAR),0),1*-100)) * 100 END AS VARIANCE
	FROM
	(SELECT FormattedFinanceData.CoyID, FormattedFinanceData.tbYear as Year, FormattedFinanceData.tbMonth as Month, FormattedFinanceData.ItemSeqID, FormattedFinanceData.ItemSN, FormattedFinanceData.ItemName,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 4 THEN FormattedFinanceData.ConvertedMTD end as APR,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 5 THEN FormattedFinanceData.ConvertedMTD end as MAY,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 6 THEN FormattedFinanceData.ConvertedMTD end as JUN,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 7 THEN FormattedFinanceData.ConvertedMTD end as JUL,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 8 THEN FormattedFinanceData.ConvertedMTD end as AUG,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 9 THEN FormattedFinanceData.ConvertedMTD end as SEP,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 10 THEN FormattedFinanceData.ConvertedMTD end as OCT,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 11 THEN FormattedFinanceData.ConvertedMTD end as NOV,
	CASE WHEN FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth = 12 THEN FormattedFinanceData.ConvertedMTD end as DEC, 
	CASE WHEN FormattedFinanceData.tbYear = @Year+1 AND FormattedFinanceData.tbMonth = 1 THEN FormattedFinanceData.ConvertedMTD end as JAN,
	CASE WHEN FormattedFinanceData.tbYear = @Year+1 AND FormattedFinanceData.tbMonth = 2 THEN FormattedFinanceData.ConvertedMTD end as FEB,
	CASE WHEN FormattedFinanceData.tbYear = @Year+1 AND FormattedFinanceData.tbMonth = 3 THEN FormattedFinanceData.ConvertedMTD end as MAR,
	CASE WHEN (FormattedFinanceData.tbYear = @BIGYEAR AND FormattedFinanceData.tbMonth=@BIGMONTH) THEN FormattedFinanceData.YTD end as TOTAL,
	CASE WHEN ((FormattedFinanceData.tbYear = @Year-1 AND FormattedFinanceData.tbMonth>=4) OR (FormattedFinanceData.tbYear = @Year AND FormattedFinanceData.tbMonth<=3)) THEN FormattedFinanceData.ConvertedYTD end as LASTYEAR,
	FormattedFinanceData.CreatedDate
	FROM 
		(
			SELECT f.*,p.ItemSeqID, p.ItemSN,i.ItemName ,
				f.MTD / (SELECT ISNULL(MonthEndRate,1) FROM tbForeignExchangeRate WITH (NOLOCK) WHERE Year(CreatedDate) = f.tbYear and Month(CreatedDate) = f.tbMonth AND HomeCurrencyCode = @DefaultCurrencyCode and ForeignCurrencyCode = 'SGD') As ConvertedMTD,
				f.YTD / (SELECT ISNULL(MonthEndRate,1) FROM tbForeignExchangeRate WITH (NOLOCK) WHERE Year(CreatedDate) = f.tbYear and Month(CreatedDate) = f.tbMonth AND HomeCurrencyCode = @DefaultCurrencyCode and ForeignCurrencyCode = 'SGD') As ConvertedYTD
			FROM tbFinanceItemSeq p WITH (NOLOCK)
				LEFT OUTER JOIN tbFinanceItem i WITH (NOLOCK) ON p.ItemID = i.ItemID 
				LEFT OUTER JOIN tbFinanceData f WITH (NOLOCK) ON p.ItemID = f.ItemID
				AND f.CoyID = @CoyID
				AND f.ProjectID = @ProjectID
				AND f.DepartmentID = @DepartmentID
				AND f.SectionID = @SectionID
				AND f.tbYear IN (@Year-1, @Year, @Year+1)
				where P.Report ='PNLS'
		)FormattedFinanceData
	)AS A 
	GROUP BY CoyID, ItemSeqID, ItemSN, ItemName
	ORDER BY ItemSeqID

