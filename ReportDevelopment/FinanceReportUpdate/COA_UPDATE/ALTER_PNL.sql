ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertPNL]
AS 
--****************************************
--COA 2016
--****************************************

--External Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)External Sales'
WHERE (c.coasn >= 5001 AND c.coasn <= 5029.999) OR (c.coasn >= 5051 AND c.coasn <= 5099.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Interco Sales'
WHERE (c.coasn >= 5031 AND c.coasn <= 5049.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Cost of External Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Cost Of External Sales'
WHERE (c.coasn >= 5101 AND c.coasn <= 5119.999) OR (c.coasn >= 5141 AND c.coasn <= 5499.999) OR (c.coasn >= 6000 AND c.coasn <= 6999.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Cost of Interco Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Cost Of Interco Sales'
WHERE (c.coasn >= 5121 AND c.coasn <= 5139.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Commission Income/Mgt Fee
/**
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Commission Income/Mgt Income'
WHERE (c.coasn >= 5501 AND c.coasn <= 5509.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
**/

--Commission/Mgt Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Commission/Mgt Income'
WHERE (c.coasn >= 5501 AND c.coasn <= 5509.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--Selling & Distribution
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Selling & Distribution'
WHERE (c.coasn >= 7000 AND c.coasn <= 7999.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--Other Op Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Other Op Income'
WHERE (c.coasn >= 5511 and c.coasn <= 5599.999)
OR (c.coasn >= 9001 and c.coasn <= 9019.999)
OR (c.coasn >= 9102 and c.coasn <= 9109.999)
OR c.coaid = '9041Credit'
OR c.coaid = '9109Credit'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Other Op Expense
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Other Op Expense'
WHERE (c.coasn >= 5601 and c.coasn <= 5999.999)
OR (c.coasn >= 9102 and c.coasn <= 9019.999)
OR (c.coasn >= 9111 and c.coasn <= 9115.999)
OR (c.coasn >= 9117 and c.coasn <= 9199.999)
OR c.coaid = '9041Debit'
OR c.coaid = '9109Debit'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Personnel Related Cost
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Personnel Related Cost'
WHERE (c.coasn >= 7000 and c.coasn <= 7099.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Carriage/Transportation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Carriage/Transportation'
WHERE (c.coasn >= 7100 and c.coasn <= 7199.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Advertising/Promotion
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Advertising/Promotion'
WHERE (c.coasn >= 7200 and c.coasn <= 7299.999)
or (c.coasn >= 7901 and c.coasn <= 7901.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Overseas Travelling
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Overseas Travelling'
WHERE (c.coasn >= 7301 and c.coasn <= 7301.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Entertainment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Entertainment'
WHERE (c.coasn >= 7302 and c.coasn <= 7399.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Equipment Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Equipment Expenses'
WHERE (c.coasn >= 7400 and c.coasn <= 7499.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Rental/Property Related
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Rental/Property Related'
WHERE (c.coasn >= 7500 and c.coasn <= 7599.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Depn of Fixed Assets
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Depn of Fixed Assets'
WHERE (c.coasn >= 7600 and c.coasn <= 7699.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Stationery/Printing/Telephone
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)S&D Stationery/Printing/Telephone'
WHERE (c.coasn >= 7701 and c.coasn <= 7705.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Other S&D Expenses
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Other S&D Expenses'
WHERE (c.coasn >= 7706 and c.coasn <= 7999.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--HO/Interco and Other S&D
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)HO/Interco and Other S&D'
WHERE (c.coasn >= 7706 and c.coasn <= 7900.999)
or (c.coasn >= 7902 and c.coasn <= 7999.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Personnel Related Cost
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Personnel Related Cost'
WHERE (c.coasn >= 8000 and c.coasn <= 8099.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Overseas Travelling
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Overseas Travelling'
WHERE (c.coasn >= 8101 and c.coasn <= 8101.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Transportation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Transportation'
WHERE (c.coasn >= 8102 and c.coasn <= 8149.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Entertainment
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Entertainment'
WHERE (c.coasn >= 8150 and c.coasn <= 8199.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--A&G Entertainment & Donations
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Entertainment & Donations'
WHERE (c.coasn >= 8150 and c.coasn <= 8199.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Equipment Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Equipment Expenses'
WHERE (c.coasn >= 8200 and c.coasn <= 8299.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Rental/Property Related
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Rental/Property Related'
WHERE (c.coasn >= 8300 and c.coasn <= 8399.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Depn of Fixed Assets
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Depn of Fixed Assets'
WHERE (c.coasn >= 8400 and c.coasn <= 8499.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Stationery/Printing/Telephone
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Stationery/Printing/Telephone'
WHERE (c.coasn >= 8500 and c.coasn <= 8599.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Legal/Prof Fees/Insurance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Legal/Prof Fees/Ins/Safety'
WHERE (c.coasn >= 8600 and c.coasn <= 8649.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G IT System Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G IT System Exps'
WHERE (c.coasn >= 8651 and c.coasn <= 8651.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Purchasing Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)A&G Purchasing Exps'
WHERE (c.coasn >= 8653 and c.coasn <= 8653.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Prov for Stock Obso/Pilferage
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Prov for Stock Obso/Pilferage'
WHERE (c.coasn >= 8701 and c.coasn <= 8702.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Prov for Doubtful Debts
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Prov for Doubtful Debts'
WHERE (c.coasn >= 8703 and c.coasn <= 8703.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Bad Debts Recovered
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Bad Debts Recovered'
WHERE (c.coasn >= 8704 and c.coasn <= 8704.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Stocks Written Down/Written Off
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Stocks Written Down/Written Off'
WHERE (c.coasn >= 8705 and c.coasn <= 8705.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--GST/VAT Expense
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)GST/VAT Expense'
WHERE (c.coasn >= 8711 and c.coasn <= 8711.999) or (c.coasn >= 8712 and c.coasn <= 8712.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--HO / Interco Allocation
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)HO / Interco Allocation'
WHERE (c.coasn >= 8798 and c.coasn <= 8799.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--HO/Interco Alloc
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)HO/Interco Alloc'
WHERE (c.coasn >= 8798 and c.coasn <= 8799.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Interest Income'
WHERE (c.coasn >= 9023 and c.coasn <= 9023.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest Expense
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Interest Expense'
WHERE (c.coasn >= 8801 and c.coasn <= 8809.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Derivatives Gain/(Loss)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Derivatives Gain/(Loss)'
WHERE (c.coasn >= 8821 and c.coasn <= 8822.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Forex Gain/(Loss)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Forex Gain/(Loss)'
WHERE (c.coasn >= 8831 and c.coasn <=8832.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Realised Forex Gain/(Loss)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Realised Forex Gain/(Loss)'
WHERE (c.coasn >= 8831 and c.coasn <=8831.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Unrealised Forex Gain/(Loss)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Unrealised Forex Gain/(Loss)'
WHERE (c.coasn >= 8832 and c.coasn <=8832.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dividend Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Dividend Income'
WHERE (c.coasn >= 9021 AND c.coasn <= 9021.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain/(Loss) Disposal of FA
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Gain/(Loss) Disposal of FA'
WHERE (c.coasn >= 9101 AND c.coasn <= 9101.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Other Non-Op Income/(Exp)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Other Non-Op Income/(Exp)'
WHERE (c.coasn >= 9031 AND c.coasn <= 9031.999) 
OR (c.coasn >= 9116 AND c.coasn <= 9116.999) 
OR (c.coasn >= 9200 AND c.coasn <= 9211.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Less: Taxation 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Less: Taxation'
WHERE (c.coasn >= 9400 AND c.coasn <= 9999.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dividend Expenses 
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Dividend Expenses'
WHERE (c.coasn >= 9022 AND c.coasn <= 9022.999) 
or (c.coasn >= 4902 AND c.coasn <= 4902.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Dividend Declared
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(PNL)Dividend Declared'
WHERE (c.coasn >= 9022 AND c.coasn <= 9022.999) 
or (c.coasn >= 4902 AND c.coasn <= 4902.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId



