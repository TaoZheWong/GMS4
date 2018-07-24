ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertBSSummary]
AS 
--***************************************************************************
--BS
--====================================================================================================
--The below is COA from 2016 onwards where company (YEAR(Is2016COA)*100)+MONTH(Is2016COA)<=(TBYear*100)+TBMonth
--Eg. 201601 <= 201701 
--=====================================================================================================
--Total Current Assets
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Total Current Assets'
WHERE i.ItemName = '(BS)Cash and Bank Balances'
or i.ItemName = '(BS)Fixed Deposits'
or i.ItemName = '(BS)Stocks'
or i.ItemName = '(BS)Trade Debtors'
or i.ItemName = '(BS)Due From Assoc/JV/Related - Current'
or i.ItemName = '(BS)Due From Interco - Current'
or i.ItemName = '(BS)Short-Term Investment'
or i.ItemName = '(BS)Other Debtors/Deposits/Prepayment'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Net Fixed Assets
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Net Fixed Assets'
WHERE i.ItemName = '(BS)Freehold Land'
or i.ItemName = '(BS)Leasehold Properties'
or i.ItemName = '(BS)Construction In Progress'
or i.ItemName = '(BS)Renovations'
or i.ItemName = '(BS)Investment Property'
or i.ItemName = '(BS)Plant, Machinery & Equipment'
or i.ItemName = '(BS)Furniture and Fittings'
or i.ItemName = '(BS)Office Equipment & Computers'
or i.ItemName = '(BS)Motor Vehicles'
or i.ItemName = '(BS)Less:Accumulated Depreciation/Impairment'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non Current Assets
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Total Non Current Assets'
WHERE i.ItemName = '(BS)Investment in Subsidiaries'
or i.ItemName = '(BS)Investment in Assoc/JV/Related'
or i.ItemName = '(BS)Intangible Assets'
or i.ItemName = '(BS)Due From Interco - Non Current'
or i.ItemName = '(BS)Due From Assoc/JV/Related - Non Current'
or i.ItemName = '(BS)Due From Shareholders'
or i.ItemName = '(BS)Deferred Tax Assets'
or i.ItemName = '(BS)Derivatives - Non Current'
or i.ItemName = '(BS)Goodwill'
or i.ItemName = '(BS)Less: Impairment/Amortization/Prov'
or i.ItemName = '(BS)Net Fixed Assets'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Assets
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Total Assets'
WHERE i.ItemName = '(BS)Total Current Assets'
or i.ItemName = '(BS)Total Non Current Assets'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Current Liabilities
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Total Current Liabilities'
WHERE i.ItemName = '(BS)OD/Revolving Credit'
or i.ItemName = '(BS)Trust Receipts/Notes Payable/Factoring'
or i.ItemName = '(BS)Short Term Loans'
or i.ItemName = '(BS)Term Loans - Current Portion'
or i.ItemName = '(BS)Hire Purchase Creditors - Current Li'
or i.ItemName = '(BS)Trade Creditors'
or i.ItemName = '(BS)Due To Assoc/JV/Related - Current Li'
or i.ItemName = '(BS)Due To Interco - Current Li'
or i.ItemName = '(BS)Prov For Taxation'
or i.ItemName = '(BS)Other Creditors/Accruals'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non Current Liabilities
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Total Non Current Liabilities'
WHERE i.ItemName = '(BS)Long Term Loans'
or i.ItemName = '(BS)Hire Purchase Creditors - Non Current Li'
or i.ItemName = '(BS)Due To Assoc/JV/Related - Non Current Li'
or i.ItemName = '(BS)Due To Interco - Non Current Li'
or i.ItemName = '(BS)Due to Interbranch'
or i.ItemName = '(BS)Derivatives - Non Current Li'
or i.ItemName = '(BS)Deferred Tax Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Liabilities
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1  
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Total Liabilities'
WHERE i.ItemName = '(BS)Total Current Liabilities'
or i.ItemName = '(BS)Total Non Current Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Equity
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)Total Equity'
WHERE i.ItemName = '(BS)Share Capital/Premium/Treasury Shares'
or i.ItemName = '(BS)Quasi-Equity Loan'
or i.ItemName = '(BS)Reserves'
or i.ItemName = '(BS)Accumulated Profits/(Loss)'
or i.ItemName = '(BS)Minority Interest'
or i.ItemName = '(BS)Foreign Currency Translation Reserve'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Liabilities & Equity
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BS)TOTAL LIAB & EQUITY'
WHERE i.ItemName = '(BS)Total Equity'
or i.ItemName = '(BS)Total Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID





