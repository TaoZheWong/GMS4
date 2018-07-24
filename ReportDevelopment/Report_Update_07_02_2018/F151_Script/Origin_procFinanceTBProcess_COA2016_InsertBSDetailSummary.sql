ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertBSDetailSummary]
AS 
--***************************************************************************
--BSD
--***************************************************************************
--Total Current Assets
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Current Assets'
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
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Net Fixed Assets'
WHERE i.ItemName = '(BS)Freehold Land'
or i.ItemName = '(BS)Freehold Building'
or i.ItemName = '(BS)Leasehold Properties'
or i.ItemName = '(BS)Construction In Progress'
or i.ItemName = '(BS)Renovations'
or i.ItemName = '(BS)Investment Property'
or i.ItemName = '(BS)Plant, Machinery & Equipment'
or i.ItemName = '(BS)Furniture and Fittings'
or i.ItemName = '(BS)Office Equipment & Computers'
or i.ItemName = '(BS)Motor Vehicles'
or i.ItemName = '(BS)Less:Accumulated Depreciation'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non Current Assets
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Non Current Assets'
WHERE i.ItemName = '(BS)Investment in Subsidiaries'
or i.ItemName = '(BS)Investment in Assoc/JV/Related'
or i.ItemName = '(BS)Intangible Assets'
or i.ItemName = '(BS)Due From Interco - Non Current'
or i.ItemName = '(BS)Due From Assoc/JV/Related - Non Current'
or i.ItemName = '(BS)Due From Shareholders'
or i.ItemName = '(BS)Deferred Tax Assets'
or i.ItemName = '(BS)Goodwill'
or i.ItemName = '(BS)Less: Impairment/Amortization/Prov'
or i.ItemName = '(BSD)Net Fixed Assets'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Assets
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Assets'
WHERE i.ItemName = '(BSD)Total Current Assets'
or i.ItemName = '(BSD)Total Non Current Assets'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Current Liabilities
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Current Liabilities'
WHERE i.ItemName = '(BS)Total Current Liabilities'
WHERE i.ItemName IN ('(BS)Total Current Liabilities', '(BSD)Other Creditors-Overpayment For Bill Lost')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non Current Liabilities
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Non Current Liabilities'
WHERE i.ItemName = '(BS)Total Non Current Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Liabilities
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Liabilities'
WHERE i.ItemName = '(BSD)Total Current Liabilities'
or i.ItemName = '(BSD)Total Non Current Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Equity
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Equity'
WHERE i.ItemName = '(BS)Total Equity'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Liabilities & Equity
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)TOTAL LIAB & EQUITY'
WHERE i.ItemName = '(BSD)Total Equity'
or i.ItemName = '(BSD)Total Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID





