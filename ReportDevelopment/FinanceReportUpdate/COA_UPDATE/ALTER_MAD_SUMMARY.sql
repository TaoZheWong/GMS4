ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertMADetailSummary]
AS 
--***************************************************************************
--MAD
--***************************************************************************
--Total Direct Material
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Total Direct Material'
WHERE i.ItemName = '(MAD)DM-Raw Materials'
or i.ItemName = '(MAD)DM-Parts & Access'
or i.ItemName = '(MAD)DM-Packaging Materials'
or i.ItemName = '(MAD)DM-Labelling Materials'
or i.ItemName = '(MAD)DM-Refill Materials'
or i.ItemName = '(MAD)DM-Others'
or i.ItemName = '(MAD)DM-Control A/C'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Direct Labour
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Total Direct Labour'
WHERE i.ItemName = '(MAD)DL-Salary & Leave'
or i.ItemName = '(MAD)DL-Salary Foreign Workers'
or i.ItemName = '(MAD)DL-Bonus'
or i.ItemName = '(MAD)DL-Bonus Foreign Workers'
or i.ItemName = '(MAD)DL-Coms & Incentive'
or i.ItemName = '(MAD)DL-Foreign Workers'
or i.ItemName = '(MAD)DL-Contractors'
or i.ItemName = '(MAD)DL-Casual/Temp Staff'
or i.ItemName = '(MAD)DL-Overtime'
or i.ItemName = '(MAD)DL-OT Foreign Workers'
or i.ItemName = '(MAD)DL-Shift Allow & Incentive'
or i.ItemName = '(MAD)DL-Worker Allowance'
or i.ItemName = '(MAD)DL-Worker Transport'
or i.ItemName = '(MAD)DL-Social Securities'
or i.ItemName = '(MAD)DL-Worker’s Levy'
or i.ItemName = '(MAD)DL-Worker Recruitment'
or i.ItemName = '(MAD)DL-Worker Training'
or i.ItemName = '(MAD)DL-Worker Insurance'
or i.ItemName = '(MAD)DL-Worker Medical'
or i.ItemName = '(MAD)DL-Worker Welfare'
or i.ItemName = '(MAD)DL-Worker Uniforms/PPE'
or i.ItemName = '(MAD)DL-Staff Accommodation'
or i.ItemName = '(MAD)DL-Others'
or i.ItemName = '(MAD)DL-Control A/C'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Prime Cost
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Prime Cost'
WHERE i.ItemName = '(MAD)Total Direct Material'
or i.ItemName = '(MAD)Total Direct Labour'
or i.ItemName = '(MAD)Direct Exps-Electricity'
or i.ItemName = '(MAD)Direct Exps-LPG'
or i.ItemName = '(MAD)Direct Exps-Welding Gases'
or i.ItemName = '(MAD)Direct Exps-Others'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Indirect Labour
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Total Indirect Labour'
WHERE i.ItemName = '(MAD)IL-Salary & Leave'
or i.ItemName = '(MAD)IL-Bonus'
or i.ItemName = '(MAD)IL-Coms & Incentive'
or i.ItemName = '(MAD)IL-Foreign Workers'
or i.ItemName = '(MAD)IL-Contractors'
or i.ItemName = '(MAD)IL-Casual/ Temp Staff'
or i.ItemName = '(MAD)IL-Overtime'
or i.ItemName = '(MAD)IL-Shift Allow & Incentive'
or i.ItemName = '(MAD)IL-Worker Allowance'
or i.ItemName = '(MAD)IL-Staff Transport'
or i.ItemName = '(MAD)IL-Social Securities'
or i.ItemName = '(MAD)IL-Worker`s Levy'
or i.ItemName = '(MAD)IL-Staff Recruitment'
or i.ItemName = '(MAD)IL-Staff Training'
or i.ItemName = '(MAD)IL-Staff Insurance'
or i.ItemName = '(MAD)IL-Staff Medical'
or i.ItemName = '(MAD)IL-Staff Welfare'
or i.ItemName = '(MAD)IL-Staff Uniforms/PPE'
or i.ItemName = '(MAD)IL-Staff Accommodation'
or i.ItemName = '(MAD)IL-Others'
or i.ItemName = '(MAD)Expats` Allowance'
or i.ItemName = '(MAD)Expats` Housing'
or i.ItemName = '(MAD)Expats` Transportation'
or i.ItemName = '(MAD)Expats` Education'
or i.ItemName = '(MAD)Expats` Phone, Internet, Etc.'
or i.ItemName = '(MAD)Expats` Other Expenses'
or i.ItemName = '(MAD)IL-QA Allocated Cost'
or i.ItemName = '(MAD)IL-PM Allocated Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Other Variable Overhead
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Other Variable Overhead'
WHERE i.ItemName = '(MAD)Electricity Charges'
or i.ItemName = '(MAD)Water Charges'
or i.ItemName = '(MAD)Gas Charges'
or i.ItemName = '(MAD)Carriage Inwards'
or i.ItemName = '(MAD)Import/Custom Duties'
or i.ItemName = '(MAD)Handling Charges'
or i.ItemName = '(MAD)Storage Charges'
or i.ItemName = '(MAD)Overseas Traveling'
or i.ItemName = '(MAD)Research & Development'
or i.ItemName = '(MAD)Product Certification'
or i.ItemName = '(MAD)Product Testing'
or i.ItemName = '(MAD)QC Testing'
or i.ItemName = '(MAD)Manufacturing Loss'
or i.ItemName = '(MAD)Manufacturing Wastage'
or i.ItemName = '(MAD)Rejects'
or i.ItemName = '(MAD)Rework'
or i.ItemName = '(MAD)Scrap'
or i.ItemName = '(MAD)Disposal of Sludge'
or i.ItemName = '(MAD)VO-PM Allocated Cost'
or i.ItemName = '(MAD)VO-PM Allocated Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Variable Overhead
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Total Variable Overhead'
WHERE i.ItemName = '(MAD)Indirect Material'
or i.ItemName = '(MAD)Total Indirect Labour'
or i.ItemName = '(MAD)Other Variable Overhead'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Fixed Overhead
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Total Fixed Overhead'
WHERE i.ItemName = '(MAD)Factory Rental'
or i.ItemName = '(MAD)Upkeep of Factory'
or i.ItemName = '(MAD)Property Tax'
or i.ItemName = '(MAD)Licenses'
or i.ItemName = '(MAD)Waste Disposal'
or i.ItemName = '(MAD)R&M-Factory'
or i.ItemName = '(MAD)R&M-Plant'
or i.ItemName = '(MAD)R&M-Machinery'
or i.ItemName = '(MAD)R&M-Equipment'
or i.ItemName = '(MAD)R&M-Pipelines'
or i.ItemName = '(MAD)R&M-Storage Tanks'
or i.ItemName = '(MAD)R&M-ISO Tanks'
or i.ItemName = '(MAD)R&M-Racks & Cylinders'
or i.ItemName = '(MAD)R&M-Tools'
or i.ItemName = '(MAD)R&M-Dies & Moulds'
or i.ItemName = '(MAD)R&M-Furniture & Fittings'
or i.ItemName = '(MAD)R&M-Office Equip'
or i.ItemName = '(MAD)R&M-Computers'
or i.ItemName = '(MAD)R&M-Vehicles'
or i.ItemName = '(MAD)Upkeep-Plant'
or i.ItemName = '(MAD)Upkeep-Machinery'
or i.ItemName = '(MAD)Upkeep-Equipment'
or i.ItemName = '(MAD)Upkeep-Racks & Cylinders'
or i.ItemName = '(MAD)Upkeep-Furniture & Fitting'
or i.ItemName = '(MAD)Upkeep-Office Equip'
or i.ItemName = '(MAD)Upkeep-Computers'
or i.ItemName = '(MAD)Leasing of Machinery & Equip'
or i.ItemName = '(MAD)Rental of Cylinders'
or i.ItemName = '(MAD)Dep-Leasehold Land'
or i.ItemName = '(MAD)Dep-Land Rights Use'
or i.ItemName = '(MAD)Dep-Buildings'
or i.ItemName = '(MAD)Dep-Renovations'
or i.ItemName = '(MAD)Dep-Plant'
or i.ItemName = '(MAD)Dep-Machinery'
or i.ItemName = '(MAD)Dep-Equipment'
or i.ItemName = '(MAD)Dep-Pipelines'
or i.ItemName = '(MAD)Dep-Storage Tanks'
or i.ItemName = '(MAD)Dep-Tools'
or i.ItemName = '(MAD)Dep-Dies & Moulds'
or i.ItemName = '(MAD)Dep-Spare Parts'
or i.ItemName = '(MAD)Dep-Furniture & Fittings'
or i.ItemName = '(MAD)Dep-Office Equip'
or i.ItemName = '(MAD)Dep-Computers'
or i.ItemName = '(MAD)Dep-Motor Vehicles'
or i.ItemName = '(MAD)Amort-Trademark & Patent'
or i.ItemName = '(MAD)Amort-Product License'
or i.ItemName = '(MAD)Insurance-General'
or i.ItemName = '(MAD)Insurance-Motor Vehicles'
or i.ItemName = '(MAD)Non-Capitalised Purchases'
or i.ItemName = '(MAD)Postage & Mail Charges'
or i.ItemName = '(MAD)Printing & Stationeries'
or i.ItemName = '(MAD)Telephone/Fax/Teleconference'
or i.ItemName = '(MAD)Internet'
or i.ItemName = '(MAD)Security Systems'
or i.ItemName = '(MAD)Subscription-Prof Bodies (Manufac)'
or i.ItemName = '(MAD)Miscellaneous Expense'
or i.ItemName = '(MAD)QA Allocated Cost'
or i.ItemName = '(MAD)PM Allocated Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Fixed & Variable Overhead
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Total Fixed & Variable Overhead'
WHERE i.ItemName = '(MAD)Total Variable Overhead'
or i.ItemName = '(MAD)Total Fixed Overhead'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Manufacturing Cost
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(MAD)Total Manufacturing Cost'
WHERE i.ItemName = '(MAD)Total Fixed & Variable Overhead'
or i.ItemName = '(MAD)Prime Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID





