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
WHERE t.COAID = '1201Z'
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
WHERE t.COAID = '1221Z'
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
WHERE t.COAID = '1241Z'
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
WHERE t.COAID = '1242Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Stock Pilferage
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Stock Pilferage'
WHERE t.COAID = '1242L'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Stocks Return
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Stocks Return'
WHERE c.coasn = 1297
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--COGS Clearing Account
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)COGS Clearing Account'
WHERE c.coasn = 1298
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
WHERE t.COAID = '1301Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Interco-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Interco-Trade'
WHERE c.coasn = 1401
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Interco Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Interco Trade'
WHERE t.COAID = '1401Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Interco-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Interco-Non-Trade'
WHERE c.coasn = 1402
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Interco Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Interco Non-Trade'
WHERE t.COAID = '1402Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Interco-Loan 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Interco-Loan'
WHERE c.coasn = 1403 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Interco Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Interco Loan'
WHERE t.COAID = '1403Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Interco-Non-Trade Clearing
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Interco-Non-Trade Clearing'
WHERE c.coasn = 1404
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from JV Co-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from JV Co-Trade'
WHERE c.coasn = 1411
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-JV Co Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-JV Co Trade'
WHERE t.COAID = '1411Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from JV Co-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from JV Co-Non-Trade'
WHERE (c.coasn >= 1412 AND c.coasn <= 1412.999)
or (c.coasn >= 1414 AND c.coasn <= 1414.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-JV Co Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-JV Co Non-Trade'
WHERE t.COAID = '1412Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from JV Co-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from JV Co-Loan'
WHERE (c.coasn >= 1413 AND c.coasn <= 1413.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-JV Co Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-JV Co Non-Trade'
WHERE t.COAID = '1413Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due from Assoc-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Assoc-Trade'
WHERE (c.coasn >= 1421 AND c.coasn <= 1421.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Assoc Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Assoc Trade'
WHERE t.COAID = '1421Z'
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

--Due from Assoc-Non-Trade Clearing
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Assoc-Non-Trade Clearing'
WHERE c.coasn = 1424
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

--Due from Rel-Non-Trade Clearing
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due from Rel-Non-Trade Clearing'
WHERE c.coasn = 1433
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
WHERE t.COAID = '1501Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Derivatives - Current
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Current'
WHERE c.coasn = 1502
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

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
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)HP Int in Suspense'
WHERE c.coasn = 1821
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Interest Receivables
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Interest Receivables'
WHERE c.coasn = 1822
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

--Sundry Debtors
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Sundry Debtors'
WHERE c.coasn = 1851 --TBD COA
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

--Suspense-HR
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Suspense-HR'
WHERE c.coasn = 1908
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Suspense-Others
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Suspense-Others'
WHERE c.coasn = 1909
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

--Prov for Impairment-Freehold Land
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Freehold Land'
WHERE t.COAID = '2001Z'
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
WHERE t.COAID = '2002D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Leasehold Land
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Leasehold Land'
WHERE t.COAID = '2002Z'
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
WHERE t.COAID = '2003D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Land Rights
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Land Rights'
WHERE t.COAID = '2003Z'
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
WHERE t.COAID = '2011D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Building
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Building'
WHERE t.COAID = '2011Z'
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
WHERE t.COAID = '2022D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Renovations
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Renovations'
WHERE t.COAID = '2022Z'
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
WHERE t.COAID = '2031D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Investment Property
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Investment Property'
WHERE t.COAID = '2031Z'
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
WHERE t.COAID = '2101D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Plant
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Plant'
WHERE t.COAID = '2101Z'
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
WHERE t.COAID = '2102D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Machinery
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Machinery'
WHERE t.COAID = '2102Z'
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
WHERE t.COAID = '2103D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--(BSD)Prov for Impairment-Equipment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Equipment'
WHERE t.COAID = '2103Z'
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
WHERE t.COAID = '2104D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Equipment
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Equipment'
WHERE t.COAID = '2104Z'
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
WHERE t.COAID = '2111D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Pipelines
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Pipelines'
WHERE t.COAID = '2111Z'
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
WHERE t.COAID = '2112D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Storage Tanks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Storage Tanks'
WHERE t.COAID = '2112Z'
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
WHERE t.COAID = '2113D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-ISO Tanks
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = 'Prov for Impairment-ISO Tanks'
WHERE t.COAID = '2113Z'
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
WHERE t.COAID = '2114D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId


--Prov for Impairment–Racks & Cylinders
--2016-09-09 Added BY Kim Yoong
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment–Racks & Cylinders'
WHERE t.COAID = '2114Z'
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
WHERE t.COAID = '2121D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Tools
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Tools'
WHERE t.COAID = '2121Z'
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
WHERE t.COAID = '2122D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--(BSD)Prov for Impairment-Dies & Moulds
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Dies & Moulds'
WHERE t.COAID = '2122Z'
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
WHERE t.COAID = '2123D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Spare Parts
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Spare Parts'
WHERE t.COAID = '2123Z'
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

--Derivatives Liability
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives Liability'
WHERE t.COAID = '2201D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-F&F
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-F&F'
WHERE t.COAID = '2201Z'
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
WHERE t.COAID = '2301D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Office Equip
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Office Equip'
WHERE t.COAID = '2301Z'
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
WHERE t.COAID = '2302D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Computer
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Computer'
WHERE t.COAID = '2302Z'
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
WHERE t.COAID = '2401D'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Motor Vehicle
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Motor Vehicle'
WHERE t.COAID = '2401Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Fixed Assets Clearing Account
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Fixed Assets Clearing Account'
WHERE c.coasn = 2499
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
WHERE (c.coasn = 2501)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Subsidiary 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Subsidiary '
WHERE t.COAID = '2501Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment in JV Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in JV Co'
WHERE c.coasn = 2502
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-JV Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-JV Co'
WHERE t.COAID = '2502Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment in Assoc Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in Assoc Co'
WHERE c.coasn = 2503
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Assoc Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Assoc Co'
WHERE t.COAID = '2503Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Investment in Rel Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Investment in Rel Co'
WHERE c.coasn = 2504
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for Impairment-Rel Co
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Rel Co'
WHERE t.COAID = '2504Z'
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

--Prov for Impairment-Quoted Shares
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for Impairment-Quoted Shares'
WHERE t.COAID = '2601Z'
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

--Amort-Trade Name
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Amort-Trade Name'
WHERE t.COAID = '2701A'
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

--Amort-Customer Relationship
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Amort-Customer Relationship'
WHERE t.COAID = '2702A'
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

--Amort-Trademark & Patent
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Amort-Trademark & Patent'
WHERE t.COAID = '2703A'
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

--Amort-Product License
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Amort-Product License'
WHERE t.COAID = '2704A'
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

--Amort-Club Membership
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Amort-Club Membership'
WHERE t.COAID = '2711A'
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
WHERE c.coasn = 2721
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Interco - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Interco - Non Current'
WHERE t.COAID = '2721Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due From JV/Assoc - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From JV/Assoc - Non Current'
WHERE c.coasn = 2722
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-JV/Assoc - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-JV/Assoc - Non Current'
WHERE t.COAID = '2722Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due From Rel Parties - Non Current
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From Rel Parties - Non Current'
WHERE (c.coasn >= 2723 AND c.coasn <= 2723.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Due From Rel - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From Rel - Non Current'
WHERE c.coasn = 2723
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Prov for DD-Rel-Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Prov for DD-Rel-Non Current'
WHERE t.COAID = '2723Z'
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due From Shareholders - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due From Shareholders - Non Current'
WHERE c.coasn = 2724
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Derivatives - Non Current
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Non Current'
WHERE c.coasn = 2801
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Derivatives Asset - Non Current
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives Asset - Non Current'
WHERE c.coasn = 1502
or c.coasn = 2801
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
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/1000 AS MTD, isnull(sum(YTDTotal), 0)/1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Less: Impairment/Amortization/Prov'
WHERE (c.coasn >= 2501 AND c.coasn <= 2504.999) or c.coasn in (2601.26, 2701.01, 2702.01, 2703.01, 2704.01, 2711.01)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/


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
WHERE t.YTDTotal<0 and ((c.coasn >= 1000 AND c.coasn <= 1099.999) 
or (c.coasn >= 3000 AND c.coasn <= 3099.999))
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
WHERE c.coasn = 3501 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--HP Int in Suspense
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)HP Int in Suspense'
WHERE c.coasn = 3599
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Creditors
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Creditors'
WHERE c.coasn = 3601
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Trade Creditors
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Trade Creditors'
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

--Due to Interco-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interco-Trade'
WHERE c.coasn = 3701
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interco-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interco-Non-Trade'
WHERE c.coasn = 3702
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interco-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interco-Loan - Current Li'
WHERE c.coasn = 3703
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-JV
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-JV'
WHERE c.coasn = 3613
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-Assoc
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-Assoc'
WHERE c.coasn = 3614
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Accrued Purchases-Rel
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Accrued Purchases-Rel'
WHERE c.coasn = 3615
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Trade'
WHERE c.coasn = 3704
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Non-Trade'
WHERE c.coasn = 3705
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Loan - Current Li'
WHERE c.coasn = 3706
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Trade'
WHERE c.coasn = 3707 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Non-Trade'
WHERE c.coasn = 3708
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Loan - Current Li'
WHERE c.coasn = 3709
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Rel Parties-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Rel Parties-Trade'
WHERE c.coasn = 3710 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Rel Parties-Non-Trade
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Rel Parties-Non-Trade'
WHERE c.coasn = 3711
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interbranch - Current Li
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interbranch - Current Li'
WHERE (c.coasn >= 3721 AND c.coasn <= 3721.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

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

--Overpayment For Bill Lost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Overpayment For Bill Lost'
WHERE c.coasn = 3934
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Other Creditors-Landed Cost 
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Other Creditors-Landed Cost'
WHERE c.coasn = 3941
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Landed Cost Clearing A/C
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Landed Cost Clearing A/C'
WHERE c.coasn = 3941
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
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Current Li'
WHERE c.coasn = 3970
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

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
WHERE c.coasn = 4301
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to JV-Loan - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to JV-Loan - Non Current Li'
WHERE c.coasn = 4302 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Assoc-Loan - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Assoc-Loan - Non Current Li'
WHERE c.coasn = 4303
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Rel Parties-Loan
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Rel Parties-Loan'
WHERE c.coasn >= 4304
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Shareholders - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Shareholders - Non Current Li'
WHERE c.coasn = 4305
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Due to Interbranch - Non Current Li
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Due to Interbranch - Non Current Li'
WHERE c.coasn = 4306 
OR  c.coasn = 3721
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
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Derivatives - Non Current Li'
WHERE c.coasn = 4450
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

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
WHERE c.coasn = 4602
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

--Dividend Declared
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(BSD)Dividend Declared'
WHERE c.coasn = 4902
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


