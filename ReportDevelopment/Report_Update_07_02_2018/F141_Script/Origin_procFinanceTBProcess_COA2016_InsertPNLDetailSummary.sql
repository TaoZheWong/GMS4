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
('(PNLD)Commission Income','(PNLD)Property Rental','(PNLD)Admin/Management Income',
'(PNLD)Income on Disposal of Scrap','(PNLD)Insurance Claim - Op','(PNLD)Logistic Service Income',
'(PNLD)Other Misc Income')
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
('(PNLD)Amortisation-Trade Name','(PNLD)Amortisation-Cust Relationship','(PNLD)Amortisation-Trademark & Patent',
'(PNLD)Amortisation-Product License','(PNLD)Amortisation-Club Membership','(PNLD)Written Off-Trade Name',
'(PNLD)Written Off-Cust Relationship','(PNLD)Written Off-Trademark & Patent','(PNLD)Written Off-Product License',
'(PNLD)Written Off-Club Membership','(PNLD)Other Misc Expenses')
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
WHERE i.ItemName = '(PNL)S&D Personnel Related Cost'
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
WHERE i.ItemName = '(PNL)S&D Carriage/Transportation'
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
WHERE i.ItemName = '(PNL)S&D Advertising/Promotion'
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
WHERE i.ItemName IN ('(PNL)S&D Overseas Travelling', '(PNL)S&D Entertainment')
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
WHERE i.ItemName = '(PNL)S&D Equipment Expenses'
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
WHERE i.ItemName = '(PNL)S&D Rental/Property Related'
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
WHERE i.ItemName = '(PNL)S&D Depn of Fixed Assets'
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
WHERE i.ItemName = '(PNL)S&D Stationery/Printing/Telephone'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--S&D Other Expenses
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
WHERE i.ItemName = '(PNL)S&D Personnel Related Cost'
or i.ItemName = '(PNL)S&D Carriage/Transportation'
or i.ItemName = '(PNL)S&D Advertising/Promotion'
or i.ItemName = '(PNL)S&D Overseas Travelling'
or i.ItemName = '(PNL)S&D Entertainment'
or i.ItemName = '(PNL)S&D Equipment Expenses'
or i.ItemName = '(PNL)S&D Rental/Property Related'
or i.ItemName = '(PNL)S&D Depn of Fixed Assets'
or i.ItemName = '(PNL)S&D Stationery/Printing/Telephone'
or i.ItemName = '(PNL)Other S&D Expenses'
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
WHERE i.ItemName = '(PNL)A&G Personnel Related Cost'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--A&G Travelling/Entertainment
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)A&G Travelling/Entertainment'
WHERE i.ItemName IN ('(PNL)A&G Overseas Travelling','(PNL)A&G Transportation','(PNL)A&G Entertainment')
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
WHERE i.ItemName = '(PNL)A&G Equipment Expenses'
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
WHERE i.ItemName = '(PNL)A&G Rental/Property Related'
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
WHERE i.ItemName = '(PNL)A&G Depn of Fixed Assets'
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
WHERE i.ItemName = '(PNL)A&G Stationery/Printing/Telephone'
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
WHERE i.ItemName = '(PNL)A&G Legal/Prof Fees/Insurance'
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
WHERE i.ItemName = '(PNL)A&G Other Expenses'
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
WHERE i.ItemName = '(PNL)Total A&G Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Total Operating Profit
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(PNLD)Total Operating Profit'
WHERE i.ItemName = '(PNLD)Gross Profit'
or i.ItemName = '(PNLD)Total Other Op Income/(Exp)'
or i.ItemName = '(PNLD)Total S&D Expenses'
or i.ItemName = '(PNLD)Total A&G Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Finance Expenses
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
('(PNLD)Gain/(Loss) on Exch Diff-Unreald','(PNLD)Gain/(loss) on Exch Diff-Reald',
'(PNLD)Interest-Loan from Others','(PNLD)Interest-Loan from Shareholders',
'(PNLD)Interest-Loan from Assoc/Rel','(PNLD)Interest-Loan from Interco','(PNLD)Interest-Hire Purchase',
'(PNLD)Interest-Term Loan','(PNLD)Interest-Short Term Loan','(PNLD)Interest-Factoring/TR/Bill Payback',
'(PNLD)Interest-Bank Overdraft')
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
('(PNLD)ESOS Expenses','(PNLD)Negative Goodwill','(PNLD)Foreign Withholding Tax',
'(PNLD)Real Property Gain Tax','(PNLD)Other Non-Operating Exps','(PNLD)Impairm Loss-Others',
'(PNLD)Impairm Loss-Goodwill','(PNLD)Impairm Loss-Investment','(PNLD)Impairm Loss-Plant & Equip',
'(PNLD)Impairm Loss-Property','(PNLD)Impairm Loss on Assoc/JV/Rel Co','(PNLD)Impairm Loss on Subsidiaries',
'(PNLD)Gain/(Loss) on Asset Held for Sales','(PNLD)Gain/(Loss) on Disposal of Investment','(PNLD)Gain/(Loss) on Disposal of FA',
'(PNLD)Share of Assoc/JV’s P&L','(PNLD)Minority Interest','(PNLD)Interest Income',
'(PNLD)Dividend Expenses','(PNLD)Dividend Income','(PNLD)Insurance Claim - Non-Op',
'(PNLD)Government Grant','(PNLD)Gain/(Loss) on Derivatives-Unreald','(PNLD)Gain/(Loss) on Derivatives-Reald')
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
WHERE i.ItemName IN ('(PNLD)Finance Expenses','(PNLD)Non Operating Income/Expenses')
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
WHERE i.ItemName = '(PNLD)Total Operating Profit'
or i.ItemName = '(PNLD)Total Non-Op Income/(Exp)'
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
or i.ItemName = '(PNL)Less: Taxation'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID


