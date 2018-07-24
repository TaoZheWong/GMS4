ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_SAP_InsertMA]
AS 
--****************************************
--COA 2016
--****************************************

--=============================
--Direct Material
--=============================
--DM-Raw Materials
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DM-Raw Materials'
WHERE (c.coasn >= 6101 and c.coasn <= 6101.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DM-Parts & Access
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DM-Parts & Access'
WHERE (c.coasn >= 6102 and c.coasn <= 6102.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DM-Packing & Label Materials
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DM-Packing & Label Materials'
WHERE (c.coasn >= 6103 and c.coasn <= 6104.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DM-Others
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DM-Others'
WHERE (c.coasn >= 6105 and c.coasn <= 6199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--=============================
--Direct Labour
--=============================
--DL-Salary & Leave
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Salary & Leave'
WHERE (c.coasn >= 6201 and c.coasn <= 6202.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Bonus
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Bonus'
WHERE (c.coasn >= 6203 and c.coasn <= 6204.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Foreign Workers/Casual/Temp 
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Foreign Workers/Casual/Temp'
WHERE (c.coasn >= 6206 and c.coasn <= 6206.999)
or (c.coasn >= 6208 and c.coasn <= 6208.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Contractors
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Contractors'
WHERE c.coasn = 6207
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--DL-Overtime
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Overtime'
WHERE (c.coasn >= 6209 and c.coasn <= 6209.999)
or (c.coasn >= 6210 and c.coasn <= 6210.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Shift Allowance & Incentive
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Shift Allowance & Incentive'
WHERE (c.coasn >= 6205 and c.coasn <= 6205.999)
or (c.coasn >= 6211 and c.coasn <= 6212.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Staff Transport/Vehicle Rental
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Staff Transport/Vehicle Rental'
WHERE (c.coasn >= 6213 and c.coasn <= 6213.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Social Security Contribution
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Social Security Contribution'
WHERE (c.coasn >= 6214 and c.coasn <= 6214.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Worker`s Levy
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Worker`s Levy'
WHERE(c.coasn >= 6215 and c.coasn <= 6215.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--DL-Others Direct Labor Costs
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)DL-Others Direct Labor Costs'
WHERE (c.coasn >= 6207 and c.coasn <= 6207.999)
or (c.coasn >= 6216 and c.coasn <= 6229.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Direct Expenses
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Direct Expenses'
WHERE (c.coasn >= 6251 and c.coasn <= 6299.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Indirect Material
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Indirect Material'
WHERE (c.coasn >= 6301 and c.coasn <= 6310.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--=============================
--Indirect labour
--=============================
--IL-Salary & Leave
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Salary & Leave'
WHERE (c.coasn >= 6311 and c.coasn <= 6311.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Bonus
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Bonus'
WHERE (c.coasn >= 6312 and c.coasn <= 6312.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Foreign Workers/Casual/Temp
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Foreign Workers/Casual/Temp'
WHERE (c.coasn >= 6314 and c.coasn <= 6314.999)
or (c.coasn >= 6316 and c.coasn <= 6316.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Overtime
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Overtime'
WHERE (c.coasn >= 6317 and c.coasn <= 6317.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Shift Allowance & Incentive
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Shift Allowance & Incentive'
WHERE (c.coasn >= 6313 and c.coasn <= 6313.999)
or (c.coasn >= 6318 and c.coasn <= 6319.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Staff Transport/Vehicle Rental
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Staff Transport/Vehicle Rental'
WHERE (c.coasn >= 6320 and c.coasn <= 6320.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Social Security Contribution
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Social Security Contribution'
WHERE (c.coasn >= 6321 and c.coasn <= 6321.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Worker`s Levy
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Worker`s Levy'
WHERE(c.coasn >= 6322 and c.coasn <= 6322.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Expats` Expenses
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Expats` Expenses'
WHERE (c.coasn >= 6351 and c.coasn <= 6399.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--IL-Other Indirect Labor Costs
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-Other Indirect Labor Costs'
WHERE (c.coasn >= 6315 and c.coasn <= 6315.999) 
OR (c.coasn >= 6323 and c.coasn <= 6349.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Todo
--IL-QA Allocated Cost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-QA Allocated Cost'
WHERE (c.coasn >= 0 and c.coasn <= 0.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Todo
--IL-PM Allocated Cost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)IL-PM Allocated Cost'
WHERE (c.coasn >= 0 and c.coasn <= 0.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Utilities - Elec, Water, Gas
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Utilities - Elec, Water, Gas'
WHERE (c.coasn >= 6401 and c.coasn <= 6410.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Product Development/Testing
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Product Development/Testing'
WHERE (c.coasn >= 6421 and c.coasn <= 6430.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Carriage/Transport/Storage
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Carriage/Transport/Storage'
WHERE (c.coasn >= 6411 and c.coasn <= 6420.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Manufacturing Loss/Wastage
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Manufacturing Loss/Wastage'
WHERE (c.coasn >= 6431 and c.coasn <= 6431.999)
OR (c.coasn >= 6432 and c.coasn <= 6432.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Rejects/Reworks/Scrap
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Rejects/Reworks/Scrap'
WHERE (c.coasn >= 6433 and c.coasn <= 6435.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Sales of Scrap Production
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Sales of Scrap Production'
WHERE c.coasn = 6436
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Disposal of Slurge
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Disposal of Slurge'
WHERE (c.coasn >= 6437 and c.coasn <= 6499)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Disposal of Slurge/Others
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Disposal of Slurge/Others'
WHERE (c.coasn >= 6436 and c.coasn <= 6499.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Work in Progress
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Work in Progress'
WHERE (c.coasn >= 1212 and c.coasn <= 1220)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Todo
--VO-QA Allocated Cost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)VO-QA Allocated Cost'
WHERE (c.coasn >= 0 and c.coasn <= 0.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Todo
--VO-PM Allocated Cost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)VO-PM Allocated Cost'
WHERE (c.coasn >= 0 and c.coasn <= 0.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Factory Rental/Upkeep
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Factory Rental/Upkeep'
WHERE (c.coasn >= 6501 and c.coasn <= 6502.999)
OR (c.coasn >= 6506 and c.coasn <= 6506.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Property Tax for Factory
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Property Tax for Factory'
WHERE (c.coasn >= 6503 and c.coasn <= 6503.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Repair & Maintainance
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Repair & Maintainance'
WHERE (c.coasn >= 6601 and c.coasn <= 6619)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Repair & Maintainance/Upkeep
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Repair & Maintainance/Upkeep'
WHERE (c.coasn >= 6601 and c.coasn <= 6620.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Leasing of Mac & Equip/Vehicles
/*
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Leasing of Mac & Equip/Vehicles'
WHERE (c.coasn >= 6621 and c.coasn <= 6622)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId
*/

--Leasing of Mac & Equip/Vehicles/Cyl
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Leasing of Mac & Equip/Vehicles/Cyl'
WHERE (c.coasn >= 6621 and c.coasn <= 6623.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Depreciation
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Depreciation'
WHERE (c.coasn >= 6701 and c.coasn <= 6749.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Licenses & Insurance
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Licenses & Insurance'
WHERE (c.coasn >= 6504 and c.coasn <= 6504.999)
OR (c.coasn >= 6751 and c.coasn <= 6759.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Non Capitalised Purchases
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Non Capitalised Purchases'
WHERE (c.coasn >= 6761 and c.coasn <= 6769.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--Others Fixed Overhead
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)Others Fixed Overhead'
WHERE (c.coasn >= 6505 and c.coasn <= 6505.999)
OR (c.coasn >= 6771 and c.coasn <= 6799.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--QA Allocated Cost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)QA Allocated Cost'
WHERE (c.coasn >= 0 and c.coasn <= 0.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

--PM Allocated Cost
Insert INTO tbFinanceDataSAP
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer4a t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MA)PM Allocated Cost'
WHERE (c.coasn >= 0 and c.coasn <= 0.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, UnitID, TBYear, TBMonth, i.ItemId

