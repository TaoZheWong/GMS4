ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertMADetail]
AS 
--****************************************
--COA 2016
--****************************************

--=============================
--Direct Material
--=============================
--DM-Raw Materials
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DM-Raw Materials'
WHERE (c.coasn = 6101 and c.coasn <= 6101.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DM-Parts & Access
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DM-Parts & Access'
WHERE (c.coasn = 6102 and c.coasn <= 6102.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DM-Packaging Materials
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DM-Packaging Materials'
WHERE (c.coasn = 6103 and c.coasn <= 6103.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DM-Labelling Materials
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DM-Labelling Materials'
WHERE (c.coasn = 6104 and c.coasn <= 6104.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DM-Refill Materials
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DM-Refill Materials'
WHERE (c.coasn = 6105 and c.coasn <= 6105.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DM-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DM-Others'
WHERE (c.coasn >= 6106 and c.coasn <= 6198.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DM-Control A/C
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DM-Control A/C'
WHERE (c.coasn = 6199 and c.coasn <= 6199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId



--=============================
--Direct Labour
--=============================
--DL-Salary & Leave
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Salary & Leave'
WHERE (c.coasn = 6201 and c.coasn <= 6201.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Salary Foreign Workers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Salary Foreign Workers'
WHERE (c.coasn = 6202 and c.coasn <= 6202.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Bonus
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Bonus'
WHERE (c.coasn = 6203 and c.coasn <= 6203.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Bonus Foreign Workers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Bonus Foreign Workers'
WHERE (c.coasn = 6204 and c.coasn <= 6204.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Coms & Incentive
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Coms & Incentive'
WHERE (c.coasn = 6205 and c.coasn <= 6205.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Foreign Workers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Foreign Workers'
WHERE (c.coasn = 6206 and c.coasn <= 6206.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Contractors
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Contractors'
WHERE (c.coasn = 6207 and c.coasn <= 607.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Casual/Temp Staff
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Casual/Temp Staff'
WHERE (c.coasn = 6208 and c.coasn <= 6208.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Overtime
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Overtime'
WHERE (c.coasn = 6209 and c.coasn <= 6209.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-OT Foreign Workers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-OT Foreign Workers'
WHERE (c.coasn = 6210 and c.coasn <= 6210.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Shift Allow & Incentive
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Shift Allow & Incentive'
WHERE (c.coasn = 6211 and c.coasn <= 6211.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Allowance'
WHERE (c.coasn = 6212 and c.coasn <= 6212.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Transport
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Transport'
WHERE (c.coasn = 6213 and c.coasn <= 6213.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Social Securities
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Social Securities'
WHERE (c.coasn = 6214 and c.coasn <= 6214.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker`s Levy
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker`s Levy'
WHERE (c.coasn = 6215 and c.coasn <= 6215.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Recruitment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Recruitment'
WHERE (c.coasn = 6216 and c.coasn <= 6216.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Training
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Training'
WHERE (c.coasn = 6217 and c.coasn <= 6217.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Insurance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Insurance'
WHERE (c.coasn = 6218 and c.coasn <= 6218.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Medical
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Medical'
WHERE (c.coasn = 6219 and c.coasn <= 6219.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Welfare 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Welfare'
WHERE (c.coasn = 6220 and c.coasn <= 6220.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Worker Uniforms/PPE 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Worker Uniforms/PPE'
WHERE (c.coasn = 6221 and c.coasn <= 6221.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Staff Accommodation 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Staff Accommodation'
WHERE (c.coasn = 6222 and c.coasn <= 6222.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Others'
WHERE (c.coasn = 6223 and c.coasn <= 6223.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--DL-Control A/C
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)DL-Control A/C'
WHERE (c.coasn = 6229 and c.coasn <= 6229.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Direct Exps-Electricity
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Direct Exps-Electricity'
WHERE (c.coasn = 6251 and c.coasn <= 6251.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Direct Exps-LPG
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Direct Exps-LPG'
WHERE (c.coasn = 6252 and c.coasn <= 6252.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Direct Exps-Welding Gases
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Direct Exps-Welding Gases'
WHERE (c.coasn >= 6253 and c.coasn <= 6253.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Direct Exps-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Direct Exps-Others'
WHERE (c.coasn >= 6254 and c.coasn <= 6254.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Indirect Material
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Indirect Material'
WHERE (c.coasn >= 6301 and c.coasn <= 6310.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--=============================
--Indirect labour
--=============================
--IL-Salary & Leave
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Salary & Leave'
WHERE (c.coasn >= 6311 and c.coasn <= 6311.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Bonus
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Bonus'
WHERE (c.coasn >= 6312 and c.coasn <= 6312.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Coms & Incentive
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Coms & Incentive'
WHERE (c.coasn >= 6313 and c.coasn <= 6313.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Foreign Workers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Foreign Workers'
WHERE (c.coasn >= 6314 and c.coasn <= 6314.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Contractors
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Contractors'
WHERE (c.coasn >= 6315 and c.coasn <= 6315.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Casual/ Temp Staff
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Casual/ Temp Staff'
WHERE (c.coasn >= 6316 and c.coasn <= 6316.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Overtime
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Overtime'
WHERE (c.coasn >= 6317 and c.coasn <= 6317.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Shift Allow & Incentive
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Shift Allow & Incentive'
WHERE (c.coasn >= 6318 and c.coasn <= 6318.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Worker Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Worker Allowance'
WHERE (c.coasn >= 6319 and c.coasn <= 6319.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Transport
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Transport'
WHERE (c.coasn >= 6320 and c.coasn <= 6320.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Social Securities
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Social Securities'
WHERE (c.coasn >= 6321 and c.coasn <= 6321.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Worker`s Levy
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Worker`s Levy'
WHERE (c.coasn >= 6322 and c.coasn <= 6322.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Recruitment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Recruitment'
WHERE (c.coasn >= 6323 and c.coasn <= 6323.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Training
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Training'
WHERE (c.coasn >= 6324 and c.coasn <= 6324.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Insurance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Insurance'
WHERE (c.coasn >= 6325 and c.coasn <= 6325.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Medical
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Medical'
WHERE (c.coasn >= 6326 and c.coasn <= 6326.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Welfare
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Welfare'
WHERE (c.coasn >= 6327 and c.coasn <= 6327.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Uniforms/PPE
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Uniforms/PPE'
WHERE (c.coasn >= 6328 and c.coasn <= 6328.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Staff Accommodation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Staff Accommodation'
WHERE (c.coasn >= 6329 and c.coasn <= 6329.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--IL-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)IL-Others'
WHERE (c.coasn >= 6330 and c.coasn <= 6330.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Expats` Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Expats` Allowance'
WHERE (c.coasn >= 6351 and c.coasn <= 6351.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Expats` Housing
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Expats` Housing'
WHERE (c.coasn >= 6352 and c.coasn <= 6352.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Expats` Transportation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Expats` Transportation'
WHERE (c.coasn >= 6353 and c.coasn <= 6353.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Expats` Education
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Expats` Education'
WHERE (c.coasn >= 6354 and c.coasn <= 6354.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Expats` Phone, Internet, Etc.
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Expats` Phone, Internet, Etc.'
WHERE (c.coasn >= 6355 and c.coasn <= 6355.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Expats` Other Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Expats` Other Expenses'
WHERE (c.coasn >= 6356 and c.coasn <= 6356.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Todo
--(MAD)QA Allocated Cost
--(MAD)PM Allocated Cost

--Electricity Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Electricity Charges'
WHERE (c.coasn >= 6401 and c.coasn <= 6401.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Water Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Water Charges'
WHERE (c.coasn >= 6402 and c.coasn <= 6402.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gas Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Gas Charges'
WHERE (c.coasn >= 6403 and c.coasn <= 6403.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Carriage Inwards
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Carriage Inwards'
WHERE (c.coasn >= 6411 and c.coasn <= 6411.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Import/Custom Duties
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Import/Custom Duties'
WHERE (c.coasn >= 6412 and c.coasn <= 6412.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Handling Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Handling Charges'
WHERE (c.coasn >= 6413 and c.coasn <= 6413.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Storage Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Storage Charges'
WHERE (c.coasn >= 6414 and c.coasn <= 6414.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Overseas Traveling
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Overseas Traveling'
WHERE (c.coasn >= 6415 and c.coasn <= 6415.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Research & Development
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Research & Development'
WHERE (c.coasn >= 6421 and c.coasn <= 6421.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Product Certification
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Product Certification'
WHERE (c.coasn >= 6422 and c.coasn <= 6422.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Product Testing
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Product Testing'
WHERE (c.coasn >= 6423 and c.coasn <= 6423.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--QC Testing
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)QC Testing'
WHERE (c.coasn >= 6424 and c.coasn <= 6424.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Manufacturing Loss
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Manufacturing Loss'
WHERE (c.coasn >= 6424 and c.coasn <= 6431.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Manufacturing Wastage
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Manufacturing Wastage'
WHERE (c.coasn >= 6432 and c.coasn <= 6432.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Rejects
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Rejects'
WHERE (c.coasn >= 6433 and c.coasn <= 6433.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Rework
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Rework'
WHERE (c.coasn >= 6434 and c.coasn <= 6434.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Scrap
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Scrap'
WHERE (c.coasn >= 6435 and c.coasn <= 6435.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Disposal of Sludge
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Disposal of Sludge'
WHERE (c.coasn >= 6436 and c.coasn <= 6436.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Todo
--(MAD)QA Allocated Cost
--(MAD)PM Allocated Cost

--Factory Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Factory Rental'
WHERE (c.coasn >= 6501 and c.coasn <= 6501.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep of Factory
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Upkeep of Factory'
WHERE (c.coasn >= 6502 and c.coasn <= 6502.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Property Tax
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Property Tax'
WHERE (c.coasn >= 6503 and c.coasn <= 6503.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Licenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Licenses'
WHERE (c.coasn >= 6504 and c.coasn <= 6504.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Waste Disposal
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Waste Disposal'
WHERE (c.coasn >= 6505 and c.coasn <= 6505.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Factory
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Factory'
WHERE (c.coasn >= 6506 and c.coasn <= 6506.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Plant
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Plant'
WHERE (c.coasn >= 6601 and c.coasn <= 6601.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Machinery 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Machinery'
WHERE (c.coasn >= 6602 and c.coasn <= 6602.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Equipment'
WHERE (c.coasn >= 6603 and c.coasn <= 6603.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Pipelines
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Pipelines'
WHERE (c.coasn >= 6604 and c.coasn <= 6604.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Storage Tanks
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Storage Tanks'
WHERE (c.coasn >= 6605 and c.coasn <= 6605.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-ISO Tanks
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-ISO Tanks'
WHERE (c.coasn >= 6606 and c.coasn <= 6606.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Racks & Cylinders
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Racks & Cylinders'
WHERE (c.coasn >= 6607 and c.coasn <= 6607.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Tools
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Tools'
WHERE (c.coasn >= 6608 and c.coasn <= 6608.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Dies & Moulds
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Dies & Moulds'
WHERE (c.coasn >= 6609 and c.coasn <= 6609.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Furniture & Fittings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Furniture & Fittings'
WHERE (c.coasn >= 6610 and c.coasn <= 6610.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Office Equip'
WHERE (c.coasn >= 6611 and c.coasn <= 6611.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Computers'
WHERE (c.coasn >= 6612 and c.coasn <= 6612.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--R&M-Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)R&M-Vehicles'
WHERE (c.coasn >= 6613 and c.coasn <= 6613.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep-Plant
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Upkeep-Plant'
WHERE (c.coasn >= 6614 and c.coasn <= 6614.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep-Machinery
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Upkeep-Machinery'
WHERE (c.coasn >= 6615 and c.coasn <= 6615.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep-Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Upkeep-Equipment'
WHERE (c.coasn >= 6616 and c.coasn <= 6616.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep-Racks & Cylinders
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Upkeep-Racks & Cylinders'
WHERE (c.coasn >= 6617 and c.coasn <= 6617.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep-Furniture & Fitting
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Upkeep-Furniture & Fitting'
WHERE (c.coasn >= 6618 and c.coasn <= 6618.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = 'Upkeep-Office Equip'
WHERE (c.coasn >= 6619 and c.coasn <= 6619.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Upkeep-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Upkeep-Computers'
WHERE (c.coasn >= 6620 and c.coasn <= 6620.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Leasing of Machinery & Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Leasing of Machinery & Equip'
WHERE (c.coasn >= 6621 and c.coasn <= 6621.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Leasing of Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Leasing of Vehicles'
WHERE (c.coasn >= 6622 and c.coasn <= 6622.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Rental of Cylinders
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Rental of Cylinders'
WHERE (c.coasn >= 6623 and c.coasn <= 6623.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Leasehold Land
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Leasehold Land'
WHERE (c.coasn >= 6701 and c.coasn <= 6701.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Land Rights Use
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Land Rights Use'
WHERE (c.coasn >= 6702 and c.coasn <= 6702.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Buildings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Buildings'
WHERE (c.coasn >= 6703 and c.coasn <= 6703.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Renovations
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Renovations'
WHERE (c.coasn >= 6704 and c.coasn <= 6704.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Plant
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Plant'
WHERE (c.coasn >= 6705 and c.coasn <= 6705.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Machinery
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Machinery'
WHERE (c.coasn >= 6706 and c.coasn <= 6706.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Equipment 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Equipment'
WHERE (c.coasn >= 6707 and c.coasn <= 6707.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-­Pipelines
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-­Pipelines'
WHERE (c.coasn >= 6708 and c.coasn <= 6708.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Storage Tanks
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Storage Tanks'
WHERE (c.coasn >= 6709 and c.coasn <= 6709.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Tools
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Tools'
WHERE (c.coasn >= 6710 and c.coasn <= 6710.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Dies & Moulds
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Dies & Moulds'
WHERE (c.coasn >= 6711 and c.coasn <= 6711.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Spare Parts
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Spare Parts'
WHERE (c.coasn >= 6712 and c.coasn <= 6712.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Furniture & Fittings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Furniture & Fittings'
WHERE (c.coasn >= 6731 and c.coasn <= 6731.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Office Equip'
WHERE (c.coasn >= 6732 and c.coasn <= 6732.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Computers'
WHERE (c.coasn >= 6733 and c.coasn <= 6733.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Dep-Motor Vehicles'
WHERE (c.coasn >= 6734 and c.coasn <= 6734.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Amort-Trademark & Patent
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Amort-Trademark & Patent'
WHERE (c.coasn >= 6741 and c.coasn <= 6741.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Amort-Product License
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Amort-Product License'
WHERE (c.coasn >= 6742 and c.coasn <= 6742.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Insurance-General
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Insurance-General'
WHERE (c.coasn >= 6751 and c.coasn <= 6751.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Insurance-Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Insurance-Motor Vehicles'
WHERE (c.coasn >= 6752 and c.coasn <= 6752.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Non-Capitalised Purchases
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Non-Capitalised Purchases'
WHERE (c.coasn >= 6761 and c.coasn <= 6761.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Postage & Mail Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Postage & Mail Charges'
WHERE (c.coasn >= 6771 and c.coasn <= 6771.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Printing & Stationeries
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Printing & Stationeries'
WHERE (c.coasn >= 6772 and c.coasn <= 6772.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Telephone/Fax/Teleconference 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Telephone/Fax/Teleconference '
WHERE (c.coasn >= 6773 and c.coasn <= 6773.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Internet
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Internet'
WHERE (c.coasn >= 6774 and c.coasn <= 6774.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Security Systems
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Security Systems'
WHERE (c.coasn >= 6775 and c.coasn <= 6775.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Subscription-Prof Bodies (Manufac)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Subscription-Prof Bodies (Manufac)'
WHERE (c.coasn >= 6776 and c.coasn <= 6776.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Miscellaneous Expense
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t INNER JOIN tbChartOfAccounts c ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i ON i.itemname = '(MAD)Miscellaneous Expense'
WHERE (c.coasn >= 6799 and c.coasn <= 6799.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
