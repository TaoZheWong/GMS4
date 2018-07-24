ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertPerformanceIndicators]
AS 
--********************************************************
--BS
--====================================================================================================
--COA  2016
--=====================================================================================================
--Prov for Doubtful Debts
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
INNER JOIN tbFinanceItem i2 on i2.ItemName = 'Prov for Doubtful Debts'
WHERE i.ItemName = '(BSD)Prov for Doubtful Debt'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Provision for Stock Obso/Pilferage
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Provision for Stock Obso/Pilferage'
WHERE i.ItemName = '(BSD)Prov for Raw Materials'
or i.ItemName = '(BSD)Prov for Packing & Label Material'
or i.ItemName = '(BSD)Prov for Finished Goods'
or i.ItemName = '(BSD)Prov for Trading Stocks'
or i.ItemName = '(BSD)Prov for Stock Pilferage'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Provision for Plant Maintenance
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
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Provision for Plant Maintenance'
WHERE i.ItemName = '(BSD)Prov for Impairment-Plant'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID

--Debtors Turnover (Days)
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)External Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(BS)Trade Debtors' or i.itemname = 'Prov for Doubtful Debts' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)External Sales' then f.mtd else 0 end) *  
(datediff(day,
case when (f.tbYear > 2016 and f.tbMonth < 4) then cast(cast (f.tbYear-1 as nvarchar(4))+ '-4-1' as smalldatetime)
else cast(cast (f.tbYear as nvarchar(4))+ '-4-1' as smalldatetime) end, dateadd(day, -1, dateadd(m, 1, cast(cast (f.tbYear as nvarchar(4))+ '-'+cast(f.tbMonth as nvarchar(2)) +'-1' as smalldatetime)))) + 1 )
end as 'MTD',
case when sum(case when i.itemname = '(PNL)External Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(BS)Trade Debtors' or i.itemname = 'Prov for Doubtful Debts' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)External Sales' then f.ytd else 0 end) *
(datediff(day,case when (f.tbYear > 2016 and f.tbMonth < 4) then cast(cast (f.tbYear-1 as nvarchar(4))+ '-4-1' as smalldatetime)
else cast(cast (f.tbYear as nvarchar(4))+ '-4-1' as smalldatetime) end, dateadd(day, -1, dateadd(m, 1, cast(cast (f.tbYear as nvarchar(4))+ '-'+cast(f.tbMonth as nvarchar(2)) +'-1' as smalldatetime)))) + 1 )
end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Debtors Turnover (Days)'
WHERE i.ItemName = '(PNL)External Sales'
or i.ItemName = '(BS)Trade Debtors'
or i.ItemName = 'Prov for Doubtful Debts'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Stocks Turnover (Days)
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Total Cost Of Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(BS)Stocks' or i.itemname = '(PNLD)A&G Prov for Stock Obsolescence' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Cost Of Sales' then f.mtd else 0 end) * -1 * 
(datediff(day,case when (f.tbYear > 2016 and f.tbMonth < 4) then cast(cast (f.tbYear-1 as nvarchar(4))+ '-4-1' as smalldatetime)
else cast(cast (f.tbYear as nvarchar(4))+ '-4-1' as smalldatetime) end, dateadd(day, -1, dateadd(m, 1, cast(cast (f.tbYear as nvarchar(4))+ '-'+cast(f.tbMonth as nvarchar(2)) +'-1' as smalldatetime)))) + 1 )
end as 'MTD',
case when sum(case when i.itemname = '(PNL)Total Cost Of Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(BS)Stocks' or i.itemname = '(PNLD)A&G Prov for Stock Obsolescence' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Cost Of Sales' then f.ytd else 0 end) * -1 * 
(datediff(day,case when (f.tbYear > 2016 and f.tbMonth < 4) then cast(cast (f.tbYear-1 as nvarchar(4))+ '-4-1' as smalldatetime)
else cast(cast (f.tbYear as nvarchar(4))+ '-4-1' as smalldatetime) end, dateadd(day, -1, dateadd(m, 1, cast(cast (f.tbYear as nvarchar(4))+ '-'+cast(f.tbMonth as nvarchar(2)) +'-1' as smalldatetime)))) + 1 )
end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Stocks Turnover (Days)'
WHERE i.ItemName = '(PNL)Total Cost Of Sales'
or i.ItemName = '(BS)Stocks'
or i.ItemName = '(PNLD)A&G Prov for Stock Obsolescence'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Creditors Turnover (Days)
--Todo 

--Liabilities To Equity (Leverage)
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(BS)TOTAL LIABILITIES' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(BS)TOTAL LIABILITIES' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Liabilities To Equity (Leverage)'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(BS)TOTAL LIABILITIES'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Debt To Equity
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(BS)TOTAL EQUITY' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.ItemName = '(BS)OD/Revolving Credit'
or i.ItemName = '(BS)Trust Receipts/Notes Payable/Factoring'
or i.ItemName = '(BS)Term Loans - Current Portion'
or i.ItemName = '(BS)Hire Purchase Creditors - Current Li'
or i.ItemName = '(BS)Long Term Loans'
or i.ItemName = '(BS)Hire Purchase Creditors - Non Current Li' then f.mtd else 0 end)/
sum(case when i.itemname = '(BS)TOTAL EQUITY' then f.mtd else 0 end) end as 'MTD', 
case when sum(case when i.itemname = '(BS)TOTAL EQUITY' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.ItemName = '(BS)OD/Revolving Credit'
or i.ItemName = '(BS)Trust Receipts/Notes Payable/Factoring'
or i.ItemName = '(BS)Term Loans - Current Portion'
or i.ItemName = '(BS)Hire Purchase Creditors - Current Li'
or i.ItemName = '(BS)Long Term Loans'
or i.ItemName = '(BS)Hire Purchase Creditors - Non Current Li' then f.ytd else 0 end)/
sum(case when i.itemname = '(BS)TOTAL EQUITY' then f.ytd else 0 end) end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT t.CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer t
)AS t1  
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Debt To Equity'
WHERE i.ItemName = '(BS)TOTAL EQUITY'
or i.ItemName = '(BS)OD/Revolving Credit'
or i.ItemName = '(BS)Trust Receipts/Notes Payable/Factoring'
or i.ItemName = '(BS)Term Loans - Current Portion'
or i.ItemName = '(BS)Hire Purchase Creditors - Current Li'
or i.ItemName = '(BS)Long Term Loans'
or i.ItemName = '(BS)Hire Purchase Creditors - Non Current Li'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName




--********************************************************
--PNL
--Gross Profit Margin %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Gross Profit' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Gross Profit' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Gross Profits Margin %'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)Gross Profit'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--External Sales GP %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, 
case when sum(case when i.itemname = '(PNL)External Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)External Sales' or i.itemname = '(PNL)Cost Of External Sales' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)External Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)External Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)External Sales' or i.itemname = '(PNL)Cost Of External Sales' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)External Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer 
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '- External Sales GP %'
WHERE i.ItemName = '(PNL)External Sales'
or i.ItemName = '(PNL)Cost Of External Sales'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Interco Sales GP %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Interco Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Interco Sales' or i.itemname = '(PNL)Cost Of Interco Sales' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Interco Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Interco Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Interco Sales' or i.itemname = '(PNL)Cost Of Interco Sales' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Interco Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer
)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '- Interco Sales GP %'
WHERE i.ItemName = '(PNL)Interco Sales'
or i.ItemName = '(PNL)Cost Of Interco Sales'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Selling & Dist Margin %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Total S&D Expenses' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Total S&D Expenses' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer WHERE ProjectID = -1 AND DepartmentID = -1 AND SectionID = -1)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Selling & Dist Margin %'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)Total S&D Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--A&G Margin Direct %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)A&G Direct Expenses' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)A&G Direct Expenses' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer WHERE ProjectID = -1 AND DepartmentID = -1 AND SectionID = -1)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'A&G Margin Direct %'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)A&G Direct Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--A&G Margin Total %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Total A&G Expenses' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Total A&G Expenses' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer WHERE ProjectID = -1 AND DepartmentID = -1 AND SectionID = -1)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'A&G Margin Total %'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)Total A&G Expenses'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Operating Profit Margin %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID, 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Profit from Operations' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Profit from Operations' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer WHERE ProjectID = -1 AND DepartmentID = -1 AND SectionID = -1)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Operating Profit Margin %'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)Profit from Operations'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Profit Before Taxation %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Profit Before Taxation' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Profit Before Taxation' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer WHERE ProjectID = -1 AND DepartmentID = -1 AND SectionID = -1)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Profit Before Taxation %'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)Profit Before Taxation'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

--Profit After Taxation %
Insert INTO tbFinanceData
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, 
i2.ItemID,
case when sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Profit After Taxation' then f.mtd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.mtd else 0 end)*100 end as 'MTD', 
case when sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end) = 0 then 0 
else sum(case when i.itemname = '(PNL)Profit After Taxation' then f.ytd else 0 end)/
sum(case when i.itemname = '(PNL)Total Sales' then f.ytd else 0 end)*100 end as 'YTD',
getdate()
FROM tbFinanceData f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, tbYear, tbMonth
FROM tbTempTransfer WHERE ProjectID = -1 AND DepartmentID = -1 AND SectionID = -1)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth 
INNER JOIN tbFinanceItem i2 ON i2.ItemName = 'Profit After Taxation %'
WHERE i.ItemName = '(PNL)Total Sales'
or i.ItemName = '(PNL)Profit After Taxation'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.tbYear, f.tbMonth, i2.ItemID, i2.ItemName

