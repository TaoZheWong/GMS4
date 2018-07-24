ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_SAP_InsertPNLSummary]
AS 
--====================================================================================================
-- COA 2016 
--====================================================================================================
--Total Sales
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Total Sales'
WHERE i.ItemName = '(PNL)External Sales'
or i.ItemName = '(PNL)Interco Sales'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Cost of Sales
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Total Cost of Sales'
WHERE i.ItemName = '(PNL)Cost of External Sales'
or i.ItemName = '(PNL)Cost of Interco Sales'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Gross Profit
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Gross Profit'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)Total Cost of Sales'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Other Op Income/ (Exp)
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Total Other Op Income/ (Exp)'
WHERE i.ItemName = '(PNL)Commission Income/Mgt Income'
or i.ItemName = '(PNL)Other Op Income'
or i.ItemName = '(PNL)Other Op Expense'
or i.ItemName = '(PNL)Gain/(Loss) Disposal of FA'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total S&D Expenses
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Total S&D Expenses'
WHERE i.ItemName = '(PNL)S&D Personnel Related Cost'
or i.ItemName = '(PNL)S&D Carriage/Transportation'
or i.ItemName = '(PNL)S&D Advertising/Promotion'
or i.ItemName = '(PNL)S&D Overseas Travelling'
or i.ItemName = '(PNL)S&D Entertainment'
or i.ItemName = '(PNL)S&D Equipment Expenses'
or i.ItemName = '(PNL)S&D Rental/Property Related'
or i.ItemName = '(PNL)S&D Depn of Fixed Assets'
or i.ItemName = '(PNL)S&D Stationery/Printing/Telephone'
or i.ItemName = '(PNL)HO/Interco and Other S&D'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Direct Expenses
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)A&G Direct Expenses'
WHERE i.ItemName = '(PNL)A&G Personnel Related Cost'
or i.ItemName = '(PNL)A&G Overseas Travelling'
or i.ItemName = '(PNL)A&G Transportation'
or i.ItemName = '(PNL)A&G Entertainment & Donations'
or i.ItemName = '(PNL)A&G Equipment Expenses'
or i.ItemName = '(PNL)A&G Rental/Property Related'
or i.ItemName = '(PNL)A&G Depn of Fixed Assets'
or i.ItemName = '(PNL)A&G Stationery/Printing/Telephone'
or i.ItemName = '(PNL)A&G Legal/Prof Fees/Insurance'
or i.ItemName = '(PNL)A&G IT System Exps'
or i.ItemName = '(PNL)A&G Purchasing Exps'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Other Expenses
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)A&G Other Expenses'
WHERE i.ItemName = '(PNL)Prov for Stock Obso/Pilferage'
or i.ItemName = '(PNL)Prov for Doubtful Debts'
or i.ItemName = '(PNL)Bad Debts Recovered'
or i.ItemName = '(PNL)Stocks Written Down/Written Off'
or i.ItemName = '(PNL)GST/VAT Expense'
or i.ItemName = '(PNL)HO/Interco Alloc'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total A&G Expenses
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Total A&G Expenses'
WHERE i.ItemName = '(PNL)A&G Direct Expenses'
or i.ItemName = '(PNL)A&G Other Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Profit from Operations
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Profit from Operations'
WHERE i.ItemName = '(PNL)Gross Profit'
or i.ItemName = '(PNL)Total Oth Op Income/ (Exp)'
or i.ItemName = '(PNL)Total S&D Expenses'
or i.ItemName = '(PNL)Total A&G Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non-Op Income/(Exp)
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Total Non-Op Income/(Exp)'
WHERE i.ItemName = '(PNL)Interest Income'
or i.ItemName = '(PNL)Interest Expense'
or i.ItemName = '(PNL)Derivatives Gain/(Loss)'
or i.ItemName = '(PNL)Forex Gain/(Loss)'
or i.ItemName = '(PNL)Dividend Income'
or i.ItemName = '(PNL)Other Non-Op Income/(Exp)'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Profit Before Taxation
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Profit Before Taxation'
WHERE i.ItemName = '(PNL)Profit from Operations'
or i.ItemName = '(PNL)Total Non-Op Income/(Exp)'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Profit After Taxation
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth 
FROM tbTempTransfer4a t
)AS t1  
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNL)Profit After Taxation'
WHERE i.ItemName = '(PNL)Profit Before Taxation'
or i.ItemName = '(PNL)Less: Taxation'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID


