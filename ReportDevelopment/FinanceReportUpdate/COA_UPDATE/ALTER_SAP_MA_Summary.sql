ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_SAP_InsertMASummary]
AS 
--***************************************************************************
--MA
--***************************************************************************
--Direct Material
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Direct Material'
WHERE i.ItemName = '(MA)DM-Raw Materials'
or i.ItemName = '(MA)DM-Parts & Access'
or i.ItemName = '(MA)DM-Packing & Label Materials'
or i.ItemName = '(MA)DM-Others'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Direct Labour
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Direct Labor'
WHERE i.ItemName = '(MA)DL-Salary & Leave'
or i.ItemName = '(MA)DL-Bonus'
or i.ItemName = '(MA)DL-Foreign Workers/Casual/Temp'
or i.ItemName = '(MA)DL-Overtime'
or i.ItemName = '(MA)DL-Shift Allowance & Incentive'
or i.ItemName = '(MA)DL-Staff Transport/Vehicle Rental'
or i.ItemName = '(MA)DL-Social Security Contribution'
or i.ItemName = '(MA)DL-Workers` Levy'
or i.ItemName = '(MA)DL-Others Direct Labor Costs'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Prime Cost
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Prime Cost'
WHERE i.ItemName = '(MA)Direct Material'
or i.ItemName = '(MA)Direct Labor'
or i.ItemName = '(MA)Direct Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Indirect Labour
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Indirect Labor'
WHERE i.ItemName = '(MA)IL-Salary & Leave'
or i.ItemName = '(MA)IL-Bonus'
or i.ItemName = '(MA)IL-Foreign Workers/Casual/Temp'
or i.ItemName = '(MA)IL-Overtime'
or i.ItemName = '(MA)IL-Shift Allowance & Incentive'
or i.ItemName = '(MA)IL-Staff Transport/Vehicle Rental'
or i.ItemName = '(MA)IL-Social Security Contribution'
or i.ItemName = '(MA)IL-Workers` Levy'
or i.ItemName = '(MA)IL-Expats` Expenses'
or i.ItemName = '(MA)IL-Other Indirect Labor Costs'
or i.ItemName = '(MA)IL-QA Allocated Cost'
or i.ItemName = '(MA)IL-PM Allocated Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Other Variable Overhead 
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Other Variable Overhead'
WHERE i.ItemName = '(MA)Utilities - Elec, Water, Gas'
OR i.ItemName = '(MA)Product Development/Testing'
OR i.ItemName = '(MA)Carriage/Transport/Storage'
OR i.ItemName = '(MA)Manufacturing Loss/Wastage'
OR i.ItemName = '(MA)Rejects/Reworks/Scrap'
OR i.ItemName = '(MA)Disposal of Slurge/Others'
OR i.ItemName = '(MA)VO-QA Allocated Cost'
OR i.ItemName = '(MA)VO-PM Allocated Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Variable Overhead
/*
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Variable Overhead'
WHERE i.ItemName = '(MA)Indirect Material'
or i.ItemName = '(MA)Indirect Labor'
or i.ItemName = '(MA)Utilities - Elec, Water, Gas'
or i.ItemName = '(MA)Product Development/Testing'
or i.ItemName = '(MA)Carriage/Transport'
or i.ItemName = '(MA)Manufacturing Loss/Wastage'
or i.ItemName = '(MA)Rejects/Reworks/Scrap'
or i.ItemName = '(MA)Sales of Scrap Production'
or i.ItemName = '(MA)Disposal of Slurge'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID
*/

--Total Variable Costs
/*
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Total Variable Costs'
WHERE i.ItemName = '(MA)Variable Overhead'
or i.ItemName = '(MA)Work in Progress'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID
*/

--Total Variable Overhead
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Total Variable Overhead'
WHERE i.ItemName = '(MA)Indirect Material'
or i.ItemName = '(MA)Indirect Labor'
or i.ItemName = '(MA)Indirect Labor'
or i.ItemName = '(MA)Other Variable Overhead'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID


--Fixed Overhead
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Fixed Overhead'
WHERE i.ItemName = '(MA)Factory Rental/Upkeep'
or i.ItemName = '(MA)Property Tax for Factory'
or i.ItemName = '(MA)Repair & Maintainance/Upkeep'
or i.ItemName = '(MA)Leasing of Mac & Equip/Vehicles/Cyl'
or i.ItemName = '(MA)Depreciation'
or i.ItemName = '(MA)Licenses & Insurance'
or i.ItemName = '(MA)Non Capitalised Purchases'
or i.ItemName = '(MA)Others Fixed Overhead'
or i.ItemName = '(MA)QA Allocated Cost'
or i.ItemName = '(MA)PM Allocated Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Cost of Goods Manufactured
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MA)Cost of Goods Manufactured'
WHERE i.ItemName = '(MA)Prime Cost'
or i.ItemName = '(MA)Total Variable Overhead'
or i.ItemName = '(MA)Fixed Overhead'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID






