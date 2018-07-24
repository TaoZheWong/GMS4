ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertPNLDetailSummary]
AS 
--****************************************
--COA 2016
--****************************************
--External Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)External Sales'
WHERE i.ItemName IN
('(PNLD)External Sales-Goods','(PNLD)External Sales-Project Sales','(PNLD)External Sales-Cylinder Rental',
'(PNLD)External Sales-Equip Rental','(PNLD)External Sales-Repair & Service','(PNLD)External Sales-Delivery Charges',
'(PNLD)External Sales-Other Charges','(PNLD)External Sales-Mgmt Fee','(PNLD)Enternal Sales-Property Rental')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Interco Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Interco Sales'
WHERE i.ItemName IN 
('(PNLD)Interco Sales-Goods','(PNLD)Interco Sales-Cylinder Rental','(PNLD)Interco Sales-Equip Rental',
'(PNLD)Interco Sales-Repair & Service','(PNLD)Interco Sales-Delivery Charges','(PNLD)Interco Sales-Other Charges',
'(PNLD)Interco Sales-Project Sales','(PNLD)Interco Sales-Mgmt Fee','(PNLD)Interco Sales-Property Rental')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Assoc/JV/Rel Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Assoc/JV/Rel Sales'
WHERE i.ItemName IN 
('(PNLD)Assoc/JV/Rel Sales-Goods','(PNLD)Assoc/JV/Rel Sales-Cylinder Rental','(PNLD)Assoc/JV/Rel Sales-Equip Rental',
'(PNLD)Assoc/JV/Rel Sales-Repair & Service','(PNLD)Assoc/JV/Rel Sales-Delivery Charges','(PNLD)Assoc/JV/Rel Sales-Other Charges',
'(PNLD)Assoc/JV/Rel Sales-Project Sales','(PNLD)Assoc/JV/Rel Sales-Mgmt Fee','(PNLD)Assoc/JV/Rel Sales-Property Rental')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Total Sales'
WHERE i.ItemName IN ('(PNLD)External Sales','(PNLD)Interco Sales','(PNLD)Assoc/JV/Rel Sales')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Cost of External Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Cost of External Sales'
WHERE i.ItemName IN
('(PNLD)External COS-Goods','(PNLD)External COS-Cylinder Rental','(PNLD)External COS-Equip Rental',
'(PNLD)External COS-Repair & Service','(PNLD)External COS-Delivery Charges','(PNLD)External COS-Other Charges',
'(PNLD)External COS-Project Sales','(PNLD)External COS-Prov for Bill Loss','(PNLD)Manufacturing Cost')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Cost of Interco Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Cost of Interco Sales'
WHERE i.ItemName IN
('(PNLD)Interco COS-Goods','(PNLD)Interco COS-Cylinder Rental','(PNLD)Interco COS-Equip Rental',
'(PNLD)Interco COS-Repair & Service','(PNLD)Interco COS-Delivery Charges','(PNLD)Interco COS-Other Charges',
'(PNLD)Interco COS-Project Sales','(PNLD)Interco COS-Prov for Bill Loss')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Cost of Assoc/JV/Rel Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Cost of Assoc/JV/Rel Sales'
WHERE i.ItemName IN
('(PNLD)Assoc/JV/Rel COS-Goods','(PNLD)Assoc/JV/Rel COS-Cylinder Rental','(PNLD)Assoc/JV/Rel COS-Equip Rental',
'(PNLD)Assoc/JV/Rel COS-Repair & Service','(PNLD)Assoc/JV/Rel COS-Delivery Charges','(PNLD)Assoc/JV/Rel COS-Other Charges',
'(PNLD)Assoc/JV/Rel COS-Project Sales')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Cost of Sales
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Total Cost of Sales'
WHERE i.ItemName IN 
('(PNLD)Cost of External Sales','(PNLD)Cost of Interco Sales','(PNLD)Cost of Assoc/JV/Rel Sales',
'(PNLD)Carriage Inwards','(PNLD)Insurance','(PNLD)Handling Charges',
'(PNLD)Import/Custom Duties','(PNLD)Dep-Rental Equipment','(PNLD)Dep-Onsite Plant',
'(PNLD)Dep-Onsite Storage Tanks','(PNLD)Dep-Racks & Cylinders')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Gross Profit
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Gross Profit'
WHERE i.ItemName = '(PNLD)Total Sales'
or i.ItemName = '(PNLD)Total Cost of Sales'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Other Operating Income
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Other Operating Income'
WHERE i.ItemName IN 
('(PNLD)Commission Income','(PNLD)Property Rental Income','(PNLD)Admin/Management Income',
'(PNLD)Gain/(Loss) on Disposal of FA',
'(PNLD)Income on Disposal of Scrap','(PNLD)Insurance Claim','(PNLD)Logistic Service Income',
'(PNLD)Other Misc Income','(PNLD)Government Grant','(PNLD)Gain/(Loss) on Derivatives-Unreald',
'(PNLD)Share of Assoc/JV’s P&L Credit','(PNLD)Gain on Disposal of Investment','(PNLD)Gain on Asset Held for Sales'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Other Operating Expenses
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Other Operating Expenses'
WHERE i.ItemName IN 
('(PNLD)Other Misc Expenses',
'(PNLD)Other Non-Operating Exps',
'(PNLD)Share of Assoc/JV’s P&L Debit',
'(PNLD)Loss on Disposal of Investment',
'(PNLD)Loss on Asset Held for Sales',
'(PNLD)Impairm Loss on Subsidiaries',
'(PNLD)Impairm Loss on Assoc/JV/Rel Co',
'(PNLD)Impairm Loss-Plant & Equip',
'(PNLD)Impairm Loss-Investment',
'(PNLD)Impairm Loss-Others',
'(PNLD)Real Property Gain Tax',
'(PNLD)Foreign Withholding Tax',
'(PNLD)Amortisation-Product License',
'(PNLD)Amortisation-Trademark & Patent')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Other Op Income/(Exp)
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Total Other Op Income/(Exp)'
WHERE i.ItemName IN ('(PNLD)Other Operating Income', '(PNLD)Other Operating Expenses')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Personnel Related Cost
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Personnel Related Cost'
WHERE i.ItemName IN 
('(PNLD)S&D Salary & Leave',
'(PNLD)S&D Provision for Bonus',
'(PNLD)S&D Comms & Incentives',
'(PNLD)S&D Foreign Workers',
'(PNLD)S&D Casual/Temp Staff',
'(PNLD)S&D Overtime',
'(PNLD)S&D Staff Allow & Incentive',
'(PNLD)S&D Shift Allowance',
'(PNLD)S&D Staff Transport',
'(PNLD)S&D Staff Vehicle Expenses',
'(PNLD)S&D Staff Vehicle Rental',
'(PNLD)S&D Social Securities',
'(PNLD)S&D Worker`s Levy',
'(PNLD)S&D Staff Recruitment',
'(PNLD)S&D Staff Training',
'(PNLD)S&D Staff Insurance',
'(PNLD)S&D Staff Medical',
'(PNLD)S&D Staff Welfare',
'(PNLD)S&D Staff Uniforms/PPE',
'(PNLD)S&D Staff Accommodation',
'(PNLD)S&D Other Staff Expenses',
'(PNLD)S&D Expats` Allowance',
'(PNLD)S&D Expats` Housing',
'(PNLD)S&D Expats` Transportation',
'(PNLD)S&D Expats` Education',
'(PNLD)S&D Expats` Phone, Internet, Etc.',
'(PNLD)S&D Expats` Other Expenses')
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Carriage/Transportation
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Carriage/Transportation'
WHERE i.ItemName IN 
(
'(PNLD)S&D Carriage Outwards',
'(PNLD)S&D Transport-Others',
'(PNLD)S&D Road Tax-Motor Vehicle',
'(PNLD)S&D Inspection Fee-Motor Vehicle',
'(PNLD)S&D Leasing of Motor Vehicle',
'(PNLD)S&D Upkeep of Motor Vehicle',
'(PNLD)S&D Endorsement Fees',
'(PNLD)S&D Export Packaging Exps'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Advertising/Promotion
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Advertising/Promotion'
WHERE i.ItemName IN 
(
'(PNLD)S&D A&P Expenses',
'(PNLD)S&D A&P Marcom Exps',
'(PNLD)S&D Exhibition Expenses',
'(PNLD)S&D Business Development',
'(PNLD)S&D Product Development'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Travelling/Entertainment
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Travelling/Entertainment'
WHERE i.ItemName IN 
(
  '(PNL)S&D Overseas Travelling', 
  '(PNL)S&D Entertainment'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Equipment Expenses
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Equipment Expenses'
WHERE i.ItemName IN 
(
  '(PNLD)S&D Non-capitalised Purchases',
  '(PNLD)S&D Leasing of Equipment',
  '(PNLD)S&D R&M-Equipment ',
  '(PNLD)S&D R&M-Machinery',
  '(PNLD)S&D R&M-F&F',
  '(PNLD)S&D R&M-Office Equip',
  '(PNLD)S&D R&M-Computers',
  '(PNLD)S&D R&M-Motor Vehicles',
  '(PNLD)S&D Insurance of Equipment',
  '(PNLD)S&D Insurance of Vehicles',
  '(PNLD)S&D Upkeep-Machinery',
  '(PNLD)S&D Upkeep-Equipment',
  '(PNLD)S&D Upkeep-Furniture & Fitting',
  '(PNLD)S&D Upkeep-Office Equip',
  '(PNLD)S&D Upkeep-Computers'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Rental/Property Related
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Rental/Property Related'
WHERE i.ItemName IN 
(
  '(PNLD)S&D Rental',
  '(PNLD)S&D Upkeep of Premises',
  '(PNLD)S&D Waste Disposal',
  '(PNLD)S&D Electricity Charges',
  '(PNLD)S&D Water Charges',
  '(PNLD)S&D Gas Charges',
  '(PNLD)S&D Property Tax',
  '(PNLD)S&D Licenses Fees',
  '(PNLD)S&D Insurance-General'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Depn of Fixed Assets
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Depn of Fixed Assets'
WHERE i.ItemName IN 
(
  '(PNLD)S&D Dep-Leasehold Land',
  '(PNLD)S&D Dep-Land Rights Use',
  '(PNLD)S&D Dep-Buildings',
  '(PNLD)S&D Dep-Renovations',
  '(PNLD)S&D Dep-Machinery',
  '(PNLD)S&D Dep-Equipment',
  '(PNLD)S&D Dep-Onsite Pipelines',
  '(PNLD)S&D Dep-ISO Tanks',
  '(PNLD)S&D Dep-Tools',
  '(PNLD)S&D Dep-Furniture & Fittings',
  '(PNLD)S&D Dep-Office Equip',
  '(PNLD)S&D Dep-Computers',
  '(PNLD)S&D Dep-Motor Vehicles'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Stationery/Printing/Telephone
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Stationery/Printing/Telephone'
WHERE i.ItemName IN
(
  '(PNLD)S&D Postage & Mail Charges',
  '(PNLD)S&D Printing & Stationaries',
  '(PNLD)S&D Telephone/Fax/Videoconference',
  '(PNLD)S&D Internet',
  '(PNLD)S&D Subscription-Prof Bodies (S&D)'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Other Expenses
/*
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)S&D Other Expenses'
WHERE i.ItemName = '(PNL)Other S&D Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID
*/

--Total S&D Exp
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Total S&D Expenses'
WHERE i.ItemName = '(PNLD)S&D Personnel Related Cost'
or i.ItemName = '(PNLD)S&D Carriage/Transportation'
or i.ItemName = '(PNLD)S&D Advertising/Promotion'
or i.ItemName = '(PNLD)S&D Overseas Travelling'
or i.ItemName = '(PNLD)S&D Entertainment'
or i.ItemName = '(PNLD)S&D Equipment Expenses'
or i.ItemName = '(PNLD)S&D Rental/Property Related'
or i.ItemName = '(PNLD)S&D Depn of Fixed Assets'
or i.ItemName = '(PNLD)S&D Stationery/Printing/Telephone'
or i.ItemName = '(PNLD)S&D Warehouse & Logistic Exps'
or i.ItemName = '(PNLD)S&D Product Support Welding'
or i.ItemName = '(PNLD)S&D Product Support Safety'
or i.ItemName = '(PNLD)S&D Recovery to/from Interco'
or i.ItemName = '(PNLD)S&D Head Office Costs/Alloc'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Personnel Related Cost
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Personnel Related Cost'
WHERE i.ItemName IN (
  '(PNLD)A&G Salary & Leave',
  '(PNLD)A&G Provision for Bonus',
  '(PNLD)A&G Comms & Incentives',
  '(PNLD)A&G Foreign Workers',
  '(PNLD)A&G Casual/Temp Staff',
  '(PNLD)A&G Overtime',
  '(PNLD)A&G Staff Allow & Incentive',
  '(PNLD)A&G Shift Allowance',
  '(PNLD)A&G Staff Transport',
  '(PNLD)A&G Staff Vehicle Expenses',
  '(PNLD)A&G Staff Vehicle Rental',
  '(PNLD)A&G Social Securities',
  '(PNLD)A&G Workers` Levy',
  '(PNLD)A&G Staff Recruitment',
  '(PNLD)A&G Staff Training',
  '(PNLD)A&G Staff Insurance',
  '(PNLD)A&G Staff Medical',
  '(PNLD)A&G Staff Welfare',
  '(PNLD)A&G Staff Uniforms/PPE',
  '(PNLD)A&G Staff Accommodation',
  '(PNLD)A&G Other Staff Expenses',
  '(PNLD)A&G Expats` Allowance',
  '(PNLD)A&G Expats` Housing',
  '(PNLD)A&G Expats` Transportation',
  '(PNLD)A&G Expats` Education',
  '(PNLD)A&G Expats` Phone, Internet, Etc.',
  '(PNLD)A&G Expats` Other Expenses',
  '(PNLD)A&G Directors Fees',
  '(PNLD)A&G Directors Remuneration',
  '(PNLD)A&G Directors Allowance'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Travelling/Transportation
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Travelling/Transportation'
WHERE i.ItemName IN 
(
  '(PNLD)A&G Overseas Travelling',
  '(PNLD)A&G Transport-Others',
  '(PNLD)A&G Road Tax-Motor Vehicle',
  '(PNLD)A&G Inspection Fee-Motor Vehicle',
  '(PNLD)A&G Leasing of Motor Vehicle',
  '(PNLD)A&G Upkeep of Motor Vehicle'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Entertainment/Donations
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Entertainment/Donations'
WHERE i.ItemName IN 
(
  '(PNLD)A&G Entertainment',
  '(PNLD)A&G Donations'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Equipment Expenses
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Equipment Expenses'
WHERE i.ItemName IN (
  '(PNLD)A&G Leasing of Machinery & Equip',
  '(PNLD)A&G Non-Capitalised Purchases',
  '(PNLD)A&G R&M-Equipment',
  '(PNLD)A&G R&M-Office Equip',
  '(PNLD)A&G R&M-F&F',
  '(PNLD)A&G R&M-Computers',
  '(PNLD)A&G R&M-Motor Vehicles',
  '(PNLD)A&G Upkeep-Equipment',
  '(PNLD)A&G Upkeep-Office Equip'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Rental/Property Related
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Rental/Property Related'
WHERE i.ItemName IN 
(
  '(PNLD)A&G Rental of Premises',
  '(PNLD)A&G Upkeep of Premises',
  '(PNLD)A&G Waste Disposal',
  '(PNLD)A&G Electricity Charges',
  '(PNLD)A&G Water Charges',
  '(PNLD)A&G Gas Charges',
  '(PNLD)A&G Property Tax',
  '(PNLD)A&G Assessment Fee',
  '(PNLD)A&G Stamp Duty',
  '(PNLD)A&G Licenses Fees'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Depn of Fixed Assets
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Depn of Fixed Assets'
WHERE i.ItemName IN 
(
  '(PNLD)A&G Dep-Leasehold Land',
  '(PNLD)A&G Dep-Land Rights Use',
  '(PNLD)A&G Dep-Buildings',
  '(PNLD)A&G Dep-Renovations',
  '(PNLD)A&G Dep-Investment Property',
  '(PNLD)A&G Dep-Furniture & Fittings',
  '(PNLD)A&G Dep-Office Equip',
  '(PNLD)A&G Dep-Computers',
  '(PNLD)A&G Dep-Motor Vehicles'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Stationery/Printing/Telephone
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Stationery/Printing/Telephone'
WHERE i.ItemName IN
(
  '(PNLD)A&G Postage & Mail Charges',
  '(PNLD)A&G Printing & Stationaries',
  '(PNLD)A&G Telephone/Fax/Teleconference',
  '(PNLD)A&G Internet',
  '(PNLD)A&G Subscription'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Legal/Professional Fee
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Legal/Professional Fee'
WHERE i.ItemName IN 
(
  '(PNLD)A&G Directors` Fee',
  '(PNLD)A&G Audit Fees',
  '(PNLD)A&G Secretarial Fees',
  '(PNLD)A&G Tax Fees',
  '(PNLD)A&G Legal Fees',
  '(PNLD)A&G Professional Fees',
  '(PNLD)A&G Consultancy Fees',
  '(PNLD)A&G Security Charges',
  '(PNLD)A&G Safety Supervision',
  '(PNLD)A&G Insurance-General',
  '(PNLD)A&G Insurance-Motor Vehicles',
  '(PNLD)A&G Insurance-Others',
  '(PNLD)A&G Bank Charges'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Other Expenses
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Other Expenses'
WHERE i.ItemName IN
(
  '(PNLD)A&G Prov for Stock Obsolescence',
  '(PNLD)A&G Prov for Pilferage',
  '(PNLD)A&G Prov for Doubtful Debt',
  '(PNLD)A&G Bad Debt Written Off/Recovered',
  '(PNLD)A&G Stocks Written Down/Written Off',
  '(PNLD)A&G GST/VAT Expense Off',
  '(PNLD)A&G Foreign GST/VAT Expense Off-Interco',
  '(PNLD)A&G Recovery to/(from) Interco',
  '(PNLD)A&G Head Office Costs/Alloc'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total A&G Expenses
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Total A&G Expenses'
WHERE i.ItemName IN (
  '(PNLD)A&G Travelling/Transportation',
  '(PNLD)A&G Entertainment/Donations',
  '(PNLD)A&G Equipment Expenses',
  '(PNLD)A&G Rental/Property Related',
  '(PNLD)A&G Depn of Fixed Assets',
  '(PNLD)A&G Stationery/Printing/Telephone',
  '(PNLD)A&G Legal/Professional Fee',
  '(PNLD)A&G IT System Exps',
  '(PNLD)A&G Purchasing Exps',
  '(PNLD)A&G Other Expenses'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Profit From Operations
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Profit From Operations'
WHERE i.ItemName = '(PNLD)Gross Profit'
or i.ItemName = '(PNLD)Total Other Op Income/(Exp)'
or i.ItemName = '(PNLD)Total S&D Expenses'
or i.ItemName = '(PNLD)Total A&G Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Finance Expenses
/*
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Finance Expenses'
WHERE i.ItemName IN 
(
  '(PNLD)Gain/(Loss) on Exch Diff-Unreald',
  '(PNLD)Gain/(loss) on Exch Diff-Reald',
  '(PNLD)Interest-Loan from Others',
  '(PNLD)Interest-Loan from Shareholders',
  '(PNLD)Interest-Loan from Assoc/Rel',
  '(PNLD)Interest-Loan from Interco',
  '(PNLD)Interest-Hire Purchase',
  '(PNLD)Interest-Term Loan',
  '(PNLD)Interest-Short Term Loan',
  '(PNLD)Interest-Factoring/TR/Bill Payback',
  '(PNLD)Interest-Bank Overdraft'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID
*/

--Interest (Expense)
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Interest (Expense)'
WHERE i.ItemName IN 
(
  '(PNLD)Interest-Bank Overdraft',
  '(PNLD)Interest-Factoring/TR/Bill Payback',
  '(PNLD)Interest-Short Term Loan',
  '(PNLD)Interest-Term Loan',
  '(PNLD)Interest-Hire Purchase',
  '(PNLD)Interest-Loan from Interco',
  '(PNLD)Interest-Loan from Assoc/Rel',
  '(PNLD)Interest-Loan from Shareholders',
  '(PNLD)Interest-Loan from Others'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Non Operating Income/Expenses
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Non Operating Income/Expenses'
WHERE i.ItemName IN 
(
  '(PNLD)Gain/(Loss) on Derivatives-Reald',
  '(PNLD)Gain/(Loss) on Derivatives-Unreald',
  '(PNLD)Gain/(loss) on Exch Diff-Reald',
  '(PNLD)Gain/(Loss) on Exch Diff-Unreald',
  '(PNLD)Dividend Income',
  '(PNLD)Interest Income',
  '(PNLD)Minority Interest',
  '(PNLD)Impairm Loss-Goodwill',
  '(PNLD)Negative Goodwill',
  '(PNLD)ESOS Expenses'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non-Op Income/(Exp)
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Total Non-Op Income/(Exp)'
WHERE i.ItemName IN 
(
  '(PNLD)Interest (Expense)',
  '(PNLD)Non Operating Income/Expenses'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Net Profit Before Taxation
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Net Profit Before Taxation'
WHERE i.ItemName = '(PNLD)Profit From Operations'
or i.ItemName = '(PNLD)Total Non-Op Income/(Exp)'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Taxation
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Taxation'
WHERE i.ItemName IN 
(
  '(PNLD)Taxation-Current Year',
  '(PNLD)Taxation-Prior Year',
  '(PNLD)Deferred Tax-Current Year',
  '(PNLD)Deferred Tax-Prior Year'
)
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Net Profit After Taxation
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Net Profit After Taxation'
WHERE i.ItemName = '(PNLD)Net Profit Before Taxation'
or i.ItemName = '(PNLD)Taxation'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID


