ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertBS]
AS 
--****************************************
--COA 2016
--****************************************

--NET FIXED ASSETS
--****************************************
--Cash and Bank Balances
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Cash and Bank Balances'
WHERE ((c.coasn >= 1000 AND c.coasn <= 1099.999 and YTDTotal >= 0) 
or (c.coasn >= 3000 AND c.coasn <= 3099.999 and YTDTotal >= 0))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Fixed Deposits
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Fixed Deposits'
WHERE (c.coasn >= 1100 AND c.coasn <= 1199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Stocks
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Stocks'
WHERE (c.coasn >= 1200 AND c.coasn <= 1299.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Trade Debtors
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Trade Debtors'
WHERE (c.coasn >= 1300 AND c.coasn <= 1399) 
OR (c.coasn >= 1499 AND c.coasn <= 1499.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due From Assoc/JV/Related - Current
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due From Assoc/JV/Related - Current'
WHERE (c.coasn >= 1411 AND c.coasn <= 1433.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due From Interco - Current
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due From Interco - Current'
WHERE (c.coasn >= 1401 AND c.coasn <= 1404.999) and not (c.coasn >= 1411 AND c.coasn <= 1432.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Short-Term Investment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Short-Term Investment'
WHERE (c.coasn >= 1501 AND c.coasn <= 1699.999) and not (c.coasn = 1502)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Other Debtors/Deposits/Prepayment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Other Debtors/Deposits/Prepayment'
WHERE (c.coasn >= 1700 AND c.coasn <= 1700.999) 
Or (c.coasn >= 1800 AND c.coasn <= 1999.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Freehold Land
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Freehold Land'
WHERE c.coasn = 2001 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Freehold Building [Merge to Leasehold Properties on 07-06-2016]
--Insert INTO tbFinanceData
--SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
--isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
--getdate() as CreatedDate  
--FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
--INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Freehold Building'
--WHERE (c.coasn >= 2011 AND c.coasn <= 2019) AND t.COAID NOT LIKE '____D%'
--GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Leasehold Properties
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Leasehold Properties'
WHERE (c.coasn >= 2011 AND c.coasn <= 2019) 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Construction in Progress
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Construction in Progress'
WHERE c.coasn = 2021
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Renovations
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Renovations'
WHERE c.coasn = 2022 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Investment Property
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Investment Property'
WHERE (c.coasn >= 2031 AND c.coasn <= 2099) 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Plant, Machinery & Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Plant, Machinery & Equipment'
WHERE (c.coasn >= 2101 AND c.coasn <= 2200) 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Furniture and Fittings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Furniture and Fittings'
WHERE (c.coasn >= 2201 AND c.coasn <= 2299) 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Office Equipment & Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Office Equipment & Computers'
WHERE (c.coasn >= 2301 AND c.coasn <= 2399) 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Motor Vehicles'
WHERE  (c.coasn >= 2401 AND c.coasn <= 2499) 
AND t.COAID NOT LIKE '____A%'
AND t.COAID NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
/*
--Less:Accumulated Depreciation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Less:Accumulated Depreciation'
WHERE c.coaid in 
('2001Z','2002D','2002Z','2003D','2003Z','2011D','2011Z','2022D','2022Z',
'2031D','2031Z','2101D','2101Z','2102D','2102Z','2103D','2103Z','2104D','2104Z',
'2111D','2111Z','2112D','2112Z','2113D','2113Z','2114D','2114Z','2121D','2121Z',
'2122D','2122Z','2123D','2123Z','2201D','2201Z','2301D','2301Z','2302D','2302Z','2401D','2401Z') 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/
--Less:Accumulated Depreciation/Impairment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Less:Accumulated Depreciation/Impairment'
WHERE  (c.coasn >= 2001 AND c.coasn <= 2499)
AND (t.COAID NOT LIKE '____A%' OR t.COAID NOT LIKE '____Z%' )
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--****************************************
--NON CURRENT ASSETS
--****************************************
--Investment in Subsidiaries
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Investment in Subsidiaries'
WHERE (c.coasn >= 2501 AND c.coasn <= 2501.999) AND t.coaid NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Investment in Assoc/JV/Related
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Investment in Assoc/JV/Related'
WHERE (c.coasn >= 2502 AND c.coasn <= 2699.999) AND t.coaid NOT LIKE '____Z%'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Intangible Assets
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Intangible Assets'
WHERE (c.coasn >= 2701 AND c.coasn <= 2719.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due From Interco - Non Current
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due From Interco - Non Current'
WHERE (c.coasn >= 2721 AND c.coasn <= 2721.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due From Assoc/JV/Related
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due From Assoc/JV/Related'
WHERE (c.coasn >= 2722 AND c.coasn <= 2723.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due From Shareholders
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due From Shareholders'
WHERE (c.coasn >= 2724 AND c.coasn <= 2799)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Deferred Tax Assets
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Deferred Tax Assets'
WHERE (c.coasn >= 2811 AND c.coasn <= 2899)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Derivatives - Non Current
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Derivatives - Non Current'
WHERE c.coasn=1502 or (c.coasn >= 2801 AND c.coasn <= 2809.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Goodwill
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Goodwill'
WHERE (c.coasn >= 2901 AND c.coasn <= 2999.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Less: Impairment/Amortization/Prov
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Less: Impairment/Amortization/Prov'
WHERE (c.coasn >= 2501 AND c.coasn <= 2601.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--****************************************
--CURRENT LIABILITIES  - Current Li
--****************************************
--OD/Revolving Credit
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)OD/Revolving Credit'
WHERE ((c.coasn >= 1000 AND c.coasn <= 1099.999 and YTDTotal <= 0) 
or (c.coasn >= 3000 AND c.coasn <= 3099.999 and YTDTotal <= 0)
or (c.coasn >= 3100 AND c.coasn <= 3199.999))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Trust Receipts/Notes Payable/Factoring
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Trust Receipts/Notes Payable/Factoring'
WHERE (c.coasn >= 3200 AND c.coasn <= 3299.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Short Term Loans
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Short Term Loans'
WHERE (c.coasn >= 3300 AND c.coasn <= 3399.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Term Loans - Current Portion
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Term Loans - Current Portion'
WHERE (c.coasn >= 3400 AND c.coasn <= 3499.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Hire Purchase Creditors - Current Li
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Hire Purchase Creditors - Current Li'
WHERE (c.coasn >= 3500 AND c.coasn <= 3599.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Trade Creditors
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Trade Creditors'
WHERE (c.coasn >= 3601 AND c.coasn <= 3611.999)
OR (c.coasn >= 3799 AND c.coasn <= 3799.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due to Assoc/JV/Related - Current Li
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due to Assoc/JV/Related - Current Li'
WHERE (c.coasn >= 3704 AND c.coasn <= 3719.999) 
or (c.coasn >= 3613 AND c.coasn <= 3615.999) 
or (c.coasn =3731 AND c.coasn <= 3731.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due to Interco - Current Li
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due to Interco - Current Li'
WHERE ((c.coasn >= 3701 AND c.coasn <= 3703.999) OR (c.coasn >= 3612 AND c.coasn <= 3612.999))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Prov for Taxation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Prov for Taxation'
WHERE (c.coasn >= 3801 AND c.coasn <= 3899.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Other Creditors/Accruals
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Other Creditors/Accruals'
WHERE (c.coasn >= 3900 AND c.coasn <= 3999) and not (c.coasn >= 3970 AND c.coasn <= 3970.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--****************************************
--NON CURRENT LIABILITIES - Non Current Li
--****************************************
--Long Term Loans
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Long Term Loans'
WHERE (c.coasn >= 4100 AND c.coasn <= 4199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Hire Purchase Creditors - Non Current Li
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Hire Purchase Creditors - Non Current Li'
WHERE (c.coasn >= 4200 AND c.coasn <= 4299.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due to Assoc/JV/Related - Non Current Li
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due to Assoc/JV/Related - Non Current Li'
WHERE (c.coasn >= 4302 AND c.coasn <= 4305.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due to Interco - Non Current Li
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due to Interco - Non Current Li'
WHERE (c.coasn >= 4301 AND c.coasn <= 4301.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Due to Interbranch
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Due to Interbranch'
WHERE (c.coasn >= 4306 AND c.coasn <= 4306.999)
or (c.coasn >= 3721 AND c.coasn <= 3721.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Derivatives - Non Current Li
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Derivatives - Non Current Li'
WHERE (c.coasn >= 3970 AND c.coasn <= 3970.999) 
or (c.coasn >= 4450 AND c.coasn <= 4450.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Deferred Tax Liabilities
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Deferred Tax Liabilities'
WHERE (c.coasn >= 4401 AND c.coasn <= 4401.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--****************************************
--EQUITY
--****************************************
--Share Capital/Premium/Treasury Shares
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Share Capital/Premium/Treasury Shares'
WHERE (c.coasn >= 4501 AND c.coasn <= 4599.999) or (c.coasn >= 4801 AND c.coasn <= 4801.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Quasi-Equity Loan
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Quasi-Equity Loan'
WHERE (c.coasn >= 4601 AND c.coasn <= 4700.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Reserves
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Reserves'
WHERE (c.coasn >= 4701 AND c.coasn <= 4899) and not (c.coasn >= 4801 AND c.coasn <= 4801.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Accumulated Profits/(Loss)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Accumulated Profits/(Loss)'
WHERE ((c.coasn >= 4901 AND c.coasn <= 4909.999) OR (c.coasn >= 5000 AND c.coasn <= 9999.999)) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Minority Interest
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Minority Interest'
WHERE (c.coasn >= 4910 AND c.coasn <= 4919.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Foreign Currency Translation Reserve
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BS)Foreign Currency Translation Reserve'
WHERE (c.coasn >= 4920 AND c.coasn <= 4999.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
