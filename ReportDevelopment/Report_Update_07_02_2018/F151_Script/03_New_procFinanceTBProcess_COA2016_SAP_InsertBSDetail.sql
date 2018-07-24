ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_SAP_InsertBSDetail]
AS 
--====================================================================================================
-- COA 2016
--=====================================================================================================

--******************************
--CURRENT ASSETS
--******************************
--Cash and Bank Balances
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Cash and Bank Balances'
WHERE ((c.coasn >= 1000 AND c.coasn <= 1099.999 and YTDTotal >= 0) 
or (c.coasn >= 3000 AND c.coasn <= 3099.999 and YTDTotal >= 0))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Fixed Deposits
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Fixed Deposits'
WHERE (c.coasn >= 1100 AND c.coasn <= 1199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Raw Materials
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Raw Materials'
WHERE c.coasn = 1201
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Raw Materials
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Raw Materials'
WHERE c.coasn = 1201.26
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Semi-Finished Goods
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Semi-Finished Goods'
WHERE c.coasn = 1211
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Work in Progress
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Work in Progress'
WHERE c.coasn = 1212
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Packing & Labelling Materials
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Packing & Labelling Materials'
WHERE c.coasn = 1221
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Packing & Label Material
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Packing & Label Material'
WHERE c.coasn = 1221.26
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Consumables 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Consumables'
WHERE c.coasn = 1222
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Stocks In Transit 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Stocks In Transit'
WHERE c.coasn = 1231
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Finished Goods
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Finished Goods'
WHERE c.coasn = 1241
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Finished Goods
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Finished Goods'
WHERE c.coasn = 1241.26
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Trading Stocks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Trading Stocks'
WHERE c.coasn = 1242
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Trading Stocks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Trading Stocks'
WHERE c.coasn = 1242.26
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Stock Pilferage
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Stock Pilferage'
WHERE c.coasn = 1242.12
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Stocks Adjustment Account
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Stocks Adjustment Account'
WHERE c.coasn = 1299
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Trade Debtors
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Trade Debtors'
WHERE c.coasn = 1301
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Doubtful Debt
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Doubtful Debt'
WHERE c.coasn = 1301.26
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Interco-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Interco-Trade'
WHERE (c.coasn >= 1401 AND c.coasn <= 1401.999 
	AND NOT (c.coasn >= 1401.260101 AND c.coasn <= 1401.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Interco Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Interco Trade'
WHERE (c.coasn >= 1401.260101 AND c.coasn <= 1401.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Interco-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Interco-Non-Trade'
WHERE (c.coasn >= 1402 AND c.coasn <= 1402.999 
	AND NOT (c.coasn >= 1402.260101 AND c.coasn <= 1402.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Interco Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Interco Non-Trade'
WHERE (c.coasn >= 1402.260101 AND c.coasn <= 1402.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Interco-Loan 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Interco-Loan'
WHERE (c.coasn >= 1403 AND c.coasn <= 1403.999 
	AND NOT (c.coasn >= 1403.260101 AND c.coasn <= 1403.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Interco Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Interco Loan'
WHERE (c.coasn >= 1403.260101 AND c.coasn <= 1403.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from JV Co-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from JV Co-Trade'
WHERE (c.coasn >= 1411 AND c.coasn <= 1411.999 
	AND NOT (c.coasn >= 1411.260101 AND c.coasn <= 1411.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-JV Co Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-JV Co Trade'
WHERE (c.coasn >= 1411.260101 AND c.coasn <= 1411.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from JV Co-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from JV Co-Non-Trade'
WHERE (c.coasn >= 1412 AND c.coasn <= 1412.999 
	AND NOT (c.coasn >= 1412.260101 AND c.coasn <= 1412.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-JV Co Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-JV Co Non-Trade'
WHERE (c.coasn >= 1412.260101 AND c.coasn <= 1412.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from JV Co-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from JV Co-Loan'
WHERE (c.coasn >= 1413 AND c.coasn <= 1413.999 
	AND NOT (c.coasn >= 1413.260101 AND c.coasn <= 1413.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-JV Co Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-JV Co Non-Trade'
WHERE (c.coasn >= 1413.260101 AND c.coasn <= 1413.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Assoc-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Assoc-Trade'
WHERE (c.coasn >= 1421 AND c.coasn <= 1421.999 
	AND NOT (c.coasn >= 1421.260101 AND c.coasn <= 1421.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Assoc Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Assoc Trade'
WHERE (c.coasn >= 1421.260101 AND c.coasn <= 1421.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Assoc-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Assoc-Non-Trade'
WHERE (c.coasn >= 1422 AND c.coasn <= 1422.999 
	AND NOT (c.coasn >= 1422.260101 AND c.coasn <= 1422.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Assoc Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Assoc Non-Trade'
WHERE (c.coasn >= 1422.260101 AND c.coasn <= 1422.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Assoc-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Assoc-Loan'
WHERE (c.coasn >= 1423 AND c.coasn <= 1423.999 
	AND NOT (c.coasn >= 1423.260101 AND c.coasn <= 1423.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Assoc Loan 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Assoc Loan'
WHERE (c.coasn >= 1423.260101 AND c.coasn <= 1423.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Rel-Trade 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Rel-Trade'
WHERE (c.coasn >= 1431 AND c.coasn <= 1431.999 
	AND NOT (c.coasn >= 1431.260101 AND c.coasn <= 1431.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Rel Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Rel Trade'
WHERE (c.coasn >= 1431.260101 AND c.coasn <= 1431.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Rel-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Rel-Non-Trade'
WHERE (c.coasn >= 1432 AND c.coasn <= 1432.999 
	AND NOT (c.coasn >= 1432.260101 AND c.coasn <= 1432.262626))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Rel Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Rel Non-Trade'
WHERE (c.coasn >= 1432.260101 AND c.coasn <= 1432.262626)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Gain/Loss in FX (Debtors)
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Gain/Loss in FX (Debtors)'
WHERE c.coasn = 1499
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Marketable Securities
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Marketable Securities'
WHERE c.coasn = 1501
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Gain/Loss In Mkt Sec
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Gain/Loss In Mkt Sec'
WHERE c.coasn = 1501.16
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Derivatives - Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Current'
WHERE c.coasn = 1502
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Assets Held For Sale
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Assets Held For Sale'
WHERE c.coasn = 1600
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Tax Recoverable
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Tax Recoverable'
WHERE (c.coasn >= 1700 AND c.coasn <= 1700.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--GST-Input
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)GST-Input'
WHERE (c.coasn >= 1801 AND c.coasn <= 1801.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prepayment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prepayment'
WHERE c.coasn = 1802
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Deposit
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Deposit'
WHERE c.coasn = 1803
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Advance to Supplier
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Advance to Supplier'
WHERE c.coasn = 1804
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Staff Advance
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Staff Advance'
WHERE c.coasn = 1811
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Staff Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Staff Loan'
WHERE c.coasn = 1812
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--HP Int in Suspense
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)HP Int in Suspense'
WHERE c.coasn = 1821
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Interest Receivables
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Interest Receivables'
WHERE c.coasn = 1822
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Sundry Debtors
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Sundry Debtors'
WHERE c.coasn = 1842
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--Recoverable Exps
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Recoverable Exps'
WHERE c.coasn = 1831
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Misc Debtors
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Misc Debtors'
WHERE c.coasn = 1841
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Suspense-GST
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Suspense-GST'
WHERE c.coasn = 1901
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Suspense-Others
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Suspense-Others'
WHERE c.coasn in (1908,1909)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--******************************
--FIXED ASSETS 
--******************************
--Land-Freehold
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Land-Freehold'
WHERE c.coasn = 2001
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Land-Leasehold
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Land-Leasehold'
WHERE c.coasn = 2002
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Leasehold land
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Leasehold land'
WHERE c.coasn = 2002.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Land-Rights
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Land-Rights'
WHERE c.coasn = 2003
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Land Rights
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Land Rights'
WHERE c.coasn = 2003.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Building
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Building'
WHERE c.coasn = 2011
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Land Rights
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Building'
WHERE c.coasn = 2011.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Construction in Progress
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Construction in Progress'
WHERE c.coasn = 2021
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Renovations
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Renovations'
WHERE c.coasn = 2022
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Renovations
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Renovations'
WHERE c.coasn = 2022.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment Property
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment Property'
WHERE c.coasn = 2031
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Investment Property
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Investment Property'
WHERE c.coasn = 2031.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Plant
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Plant'
WHERE c.coasn = 2101
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Plant
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Plant'
WHERE c.coasn = 2101.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Plant
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Plant'
WHERE c.coasn = 2101.26
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Machinery
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Machinery'
WHERE c.coasn = 2102
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Machinery
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Machinery'
WHERE c.coasn = 2102.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Equipment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Equipment'
WHERE c.coasn = 2103
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Equipment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Equipment'
WHERE c.coasn = 2103.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Rental Equipment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Rental Equipment'
WHERE c.coasn = 2104
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Rental Equipment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Rental Equipment'
WHERE c.coasn = 2104.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Pipelines
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Pipelines'
WHERE c.coasn = 2111
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Pipelines
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Pipelines'
WHERE c.coasn = 2111.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Storage Tanks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Storage Tanks'
WHERE c.coasn = 2112
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Storage Tanks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Storage Tanks'
WHERE c.coasn = 2112.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--ISO Tanks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)ISO Tanks'
WHERE c.coasn = 2113
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-ISO Tanks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-ISO Tanks'
WHERE c.coasn = 2113.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Racks & Cylinders
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Racks & Cylinders'
WHERE c.coasn = 2114
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Racks & Cylinders
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Racks & Cylinders'
WHERE c.coasn = 2114.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--Prov for Impairment–Racks & Cylinders
--2016-09-09 Added BY Kim Yoong
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment–Racks & Cylinders'
WHERE c.coasn = 2114.26
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Tools
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Tools'
WHERE c.coasn = 2121
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Tools
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Tools'
WHERE c.coasn = 2121.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Dies & Moulds
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Dies & Moulds'
WHERE c.coasn = 2122
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Dies & Moulds
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Dies & Moulds'
WHERE c.coasn = 2122.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Spare Parts
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Spare Parts'
WHERE c.coasn = 2123
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Spare Parts
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Spare Parts'
WHERE c.coasn = 2123.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Furniture & Fittings
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Furniture & Fittings'
WHERE c.coasn = 2201
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-F&F
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-F&F'
WHERE c.coasn = 2201.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Office Equipment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Office Equipment'
WHERE c.coasn = 2301
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Office Equip
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Office Equip'
WHERE c.coasn = 2301.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Computer
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Computer'
WHERE c.coasn = 2302
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Computer
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Computer'
WHERE c.coasn = 2302.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Motor Vehicle
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Motor Vehicle'
WHERE c.coasn = 2401
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accum Dep-Motor Vehicle
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accum Dep-Motor Vehicle'
WHERE c.coasn = 2401.04
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--******************************
--NON CURRENT ASSETS
--******************************
--Investment in Subsidiary
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in Subsidiary'
WHERE (c.coasn >= 2501 AND c.coasn <= 2501.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment in JV Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in JV Co'
WHERE (c.coasn >= 2502 AND c.coasn <= 2502.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment in Assoc Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in Assoc Co'
WHERE (c.coasn >= 2503 AND c.coasn <= 2503.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment in Rel Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in Rel Co'
WHERE (c.coasn >= 2504 AND c.coasn <= 2504.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment in Quoted shares
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in Quoted shares'
WHERE c.coasn = 2601
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Intangibles-Trade Name
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Intangibles-Trade Name'
WHERE c.coasn = 2701
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Intangibles-Customer Relationship
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Intangibles-Customer Relationship'
WHERE c.coasn = 2702
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Intangibles-Trademark & Patent
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Intangibles-Trademark & Patent'
WHERE c.coasn = 2703
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Intangibles-Product License
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Intangibles-Product License'
WHERE c.coasn = 2704
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Intangibles-Club Membership
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Intangibles-Club Membership'
WHERE c.coasn = 2711
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Intangibles-Others
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Intangibles-Others'
WHERE c.coasn = 2719
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due From Interco - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From Interco - Non Current'
WHERE (c.coasn >= 2721 AND c.coasn <= 2721.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due From JV/Assoc - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From JV/Assoc - Non Current'
WHERE (c.coasn >= 2722 AND c.coasn <= 2722.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due From Rel Parties - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From Rel Parties - Non Current'
WHERE (c.coasn >= 2723 AND c.coasn <= 2723.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due From Shareholders - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID

INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From Shareholders - Non Current'
WHERE (c.coasn >= 2724 AND c.coasn <= 2724.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Derivatives - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Non Current'
WHERE c.coasn = 2801
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Deferred Tax Assets
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Deferred Tax Assets'
WHERE c.coasn = 2811
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Goodwill
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Goodwill'
WHERE c.coasn = 2901
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Less: Impairment/Amortization/Prov
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Less: Impairment/Amortization/Prov'
WHERE (c.coasn >= 2501 AND c.coasn <= 2504.999) or c.coasn in (2601.26, 2701.01, 2702.01, 2703.01, 2704.01, 2711.01)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--******************************
--CURRENT LIABILITIES
--******************************
--Bank Overdraft
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Bank Overdraft'
WHERE t.YTDTotal<0 and ((c.coasn >= -1000 AND c.coasn <= 1099.999) or
						(c.coasn >= 3000 AND c.coasn <= 3099.999))
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Revolving Credit
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Revolving Credit'
WHERE (c.coasn >= 3100 AND c.coasn <= 3199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Factoring/Bills Payable/TR
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Factoring/Bills Payable/TR'
WHERE (c.coasn >= 3200 AND c.coasn <= 3299.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Short-Term Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Short-Term Loan'
WHERE (c.coasn >= 3300 AND c.coasn <= 3399.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Term Loan-Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Term Loan-Current'
WHERE (c.coasn >= 3400 AND c.coasn <= 3499.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--HP Creditors-Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)HP Creditors-Current'
WHERE (c.coasn >= 3500 AND c.coasn <= 3599.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Creditors
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Creditors'
WHERE c.coasn = 3601
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-External
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-External'
WHERE c.coasn = 3611
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-Interco
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-Interco'
WHERE (c.coasn >= 3612 AND c.coasn <= 3612.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-JV
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-JV'
WHERE (c.coasn >= 3613 AND c.coasn <= 3613.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-Assoc
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-Assoc'
WHERE (c.coasn >= 3614 AND c.coasn <= 3614.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-Rel
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-Rel'
WHERE (c.coasn >= 3615 AND c.coasn <= 3615.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interco-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interco-Trade'
WHERE (c.coasn >= 3701 AND c.coasn <= 3701.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interco-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interco-Non-Trade'
WHERE (c.coasn >= 3702 AND c.coasn <= 3702.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interco-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interco-Loan - Current Li'
WHERE (c.coasn >= 3703 AND c.coasn <= 3703.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Trade'
WHERE (c.coasn >= 3704 AND c.coasn <= 3704.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Non-Trade'
WHERE (c.coasn >= 3705 AND c.coasn <= 3705.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Loan - Current Li'
WHERE (c.coasn >= 3706 AND c.coasn <= 3706.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Trade'
WHERE (c.coasn >= 3707 AND c.coasn <= 3707.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Non-Trade'
WHERE (c.coasn >= 3708 AND c.coasn <= 3708.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Loan - Current Li'
WHERE (c.coasn >= 3709 AND c.coasn <= 3709.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Rel Parties-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Rel Parties-Trade'
WHERE (c.coasn >= 3710 AND c.coasn <= 3710.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Rel Parties-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Rel Parties-Non-Trade'
WHERE (c.coasn >= 3711 AND c.coasn <= 3711.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interbranch - Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interbranch - Current Li'
WHERE (c.coasn >= 3721 AND c.coasn <= 3721.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Shareholders - Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Shareholders - Current Li'
WHERE c.coasn = 3731
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Gain/Loss in FX (Creditors)
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Gain/Loss in FX (Creditors)'
WHERE c.coasn = 3799
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for taxation (Current Year)
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for taxation (Current Year)'
WHERE c.coasn = 3801
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for taxation (Prior Year)
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for taxation (Prior Year)'
WHERE c.coasn = 3802
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Salary and Leave
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Salary and Leave'
WHERE c.coasn = 3901
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Social Securities
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Social Securities'
WHERE c.coasn = 3902
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Other HR Exps
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Other HR Exps'
WHERE c.coasn = 3903
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Staff Claims
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Staff Claims'
WHERE c.coasn = 3904
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Transportation
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Transportation'
WHERE c.coasn = 3911
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Audit Fees
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Audit Fees'
WHERE c.coasn = 3921
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Tax Fees
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Tax Fees'
WHERE c.coasn = 3922
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Professional Fees
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Professional Fees'
WHERE c.coasn = 3923
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Interest Payables
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Interest Payables'
WHERE c.coasn = 3924
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accruals-Others
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accruals-Others'
WHERE c.coasn = 3929
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Customer Deposit
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Customer Deposit'
WHERE c.coasn = 3931
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Customer down payment 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Customer down payment'
WHERE c.coasn = 3932
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Added By KimYoong 2017-03-09
--Deferred revenue-cash vouchers
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Deferred Revenue-Cash Vouchers'
WHERE c.coasn = 3933
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Creditors-Landed Cost 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Creditors-Landed Cost'
WHERE c.coasn = 3941
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Landed Cost Clearing A/C
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Landed Cost Clearing A/C'
WHERE c.coasn = 3942
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Creditors-Sundry
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Creditors-Sundry'
WHERE c.coasn = 3943
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Creditors-Bill Lost Clear
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Creditors-Bill Lost Clear'
WHERE c.coasn = 3944
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Creditors-W/H Tax
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Creditors-W/H Tax'
WHERE c.coasn = 3945
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Creditors-Overpayment For Bill Lost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Creditors-Overpayment For Bill Lost'
WHERE c.coasn = 3934
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Creditors-Others
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Creditors-Others'
WHERE c.coasn = 3946
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--GST-output
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)GST-output'
WHERE c.coasn = 3950
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Proposed Dividend
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Proposed Dividend'
WHERE c.coasn = 3960
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Dividend Payable
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Dividend Payable'
WHERE c.coasn = 3961
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Derivatives - Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Current Li'
WHERE c.coasn = 3970
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Bonus
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Bonus'
WHERE c.coasn = 3981
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Com & Incentives
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Com & Incentives'
WHERE c.coasn = 3982
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Gratuities
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Gratuities'
WHERE c.coasn = 3983
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for A&P
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for A&P'
WHERE c.coasn = 3991
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Cylinder Loss
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Cylinder Loss'
WHERE c.coasn = 3992
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Plant Maintenance
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Plant Maintenance'
WHERE c.coasn = 3993
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--******************************
--NON CURRENT LIABILITIES
--******************************
--Long-Term Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Long-Term Loan'
WHERE (c.coasn >= 4100 AND c.coasn <= 4199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--HP Creditors
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)HP Creditors'
WHERE (c.coasn >= 4200 AND c.coasn <= 4299.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interco-Loan - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interco-Loan - Non Current Li'
WHERE (c.coasn >= 4301 AND c.coasn <= 4301.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Loan - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Loan - Non Current Li'
WHERE (c.coasn >= 4302 AND c.coasn <= 4302.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Loan - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Loan - Non Current Li'
WHERE (c.coasn >= 4303 AND c.coasn <= 4303.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Rel Parties-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Rel Parties-Loan'
WHERE (c.coasn >= 4304 AND c.coasn <= 4304.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Shareholders - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Shareholders - Non Current Li'
WHERE (c.coasn >= 4305 AND c.coasn <= 4305.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interbranch - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interbranch - Non Current Li'
WHERE (c.coasn >= 4306 AND c.coasn <= 4306.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Deferred Tax Liabilities
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Deferred Tax Liabilities'
WHERE c.coasn = 4401
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Derivatives - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Non Current Li'
WHERE c.coasn = 4450
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--******************************
--EQUITY
--******************************
--Share Capital
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Share Capital'
WHERE c.coasn = 4501
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Calls in Arrears
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Calls in Arrears'
WHERE c.coasn = 4502
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Treasury Shares
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Treasury Shares'
WHERE c.coasn = 4510
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Share Premium
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Share Premium'
WHERE c.coasn = 4520
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Convertible Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Convertible Loan'
WHERE c.coasn = 4601
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Quasi-Equity Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Quasi-Equity Loan'
WHERE (c.coasn >= 4602 AND c.coasn <= 4602.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Capital Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Capital Reserve'
WHERE c.coasn = 4701
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Asset Revaluation Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Asset Revaluation Reserve'
WHERE c.coasn = 4710
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Statutory Fund Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Statutory Fund Reserve'
WHERE c.coasn = 4720
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment Revaluation Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment Revaluation Reserve'
WHERE c.coasn = 4730
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Fair Value Adjustment Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Fair Value Adjustment Reserve'
WHERE c.coasn = 4740
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Amalgamation Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Amalgamation Reserve'
WHERE c.coasn = 4750
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--General Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)General Reserve'
WHERE c.coasn = 4801
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--ESOS Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)ESOS Reserve'
WHERE c.coasn = 4810
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accumulated Profit/(Loss)
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accumulated Profit/(Loss)'
WHERE ((c.coasn >= 4901 AND c.coasn <= 4909.999) OR (c.coasn >= 5000 AND c.coasn <= 9999.999)) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Minority Interest
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Minority Interest'
WHERE c.coasn = 4910
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Foreign Currency Trans Reserve
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Foreign Currency Trans Reserve'
WHERE c.coasn = 4920
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


