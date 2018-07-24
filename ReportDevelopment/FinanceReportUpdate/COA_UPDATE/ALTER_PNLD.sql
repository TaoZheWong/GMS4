ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_InsertPNLDetail]
AS 
--****************************************
--COA 2016
--****************************************

--******************************
--External Sales [5001 to 5029]
--******************************
--External Sales-Goods
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Goods'
WHERE (c.coasn >= 5001 AND c.coasn <= 5001.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External Sales-Project Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Project Sales'
WHERE (c.coasn >= 5002 AND c.coasn <= 5002.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External Sales-Cylinder Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Cylinder Rental'
WHERE (c.coasn >= 5003 AND c.coasn <= 5003.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External Sales-Equip Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Equip Rental'
WHERE (c.coasn >= 5004 AND c.coasn <= 5004.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External Sales-Repair & Service
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Repair & Service'
WHERE (c.coasn >= 5005 AND c.coasn <= 5005.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External Sales-Delivery Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Delivery Charges'
WHERE (c.coasn >= 5006 AND c.coasn <= 5006.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External Sales-Other Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Other Charges'
WHERE (c.coasn >= 5007 AND c.coasn <= 5007.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External Sales-Mgmt Fee
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External Sales-Mgmt Fee'
WHERE (c.coasn >= 5021 AND c.coasn <= 5021.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Enternal Sales-Property Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Enternal Sales-Property Rental'
WHERE (c.coasn >= 5022 AND c.coasn <= 5022.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Interco Sales [5031 to 5049]
--******************************
--Interco Sales-Goods
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Goods'
WHERE (c.coasn >= 5031 and c.coasn <= 5031.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Cylinder Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Cylinder Rental'
WHERE (c.coasn >= 5032 and c.coasn <= 5032.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Equip Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Equip Rental'
WHERE (c.coasn >= 5033 and c.coasn <= 5033.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Repair & Service
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Repair & Service'
WHERE (c.coasn >= 5034 and c.coasn <= 5034.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Delivery Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Delivery Charges'
WHERE (c.coasn >= 5035 and c.coasn <= 5035.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Other Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Other Charges'
WHERE (c.coasn >= 5036 and c.coasn <= 5036.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Project Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Project Sales'
WHERE (c.coasn >= 5037 and c.coasn <= 5037.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Mgmt Fee
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Mgmt Fee'
WHERE (c.coasn >= 5041 and c.coasn <= 5041.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco Sales-Property Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco Sales-Property Rental'
WHERE (c.coasn >= 5042 and c.coasn <= 5042.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Assoc/JV/Rel Sales [5051 to 5099]
--******************************
--Assoc/JV/Rel Sales-Goods
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Goods'
WHERE (c.coasn >= 5051 and c.coasn <= 5051.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Cylinder Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Cylinder Rental'
WHERE (c.coasn >= 5052 and c.coasn <= 5052.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Equip Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Equip Rental'
WHERE (c.coasn >= 5053 and c.coasn <= 5053.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Repair & Service
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Repair & Service'
WHERE (c.coasn >= 5054 and c.coasn <= 5054.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Delivery Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Delivery Charges'
WHERE (c.coasn >= 5055 and c.coasn <= 5055.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Other Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Other Charges'
WHERE (c.coasn >= 5056 and c.coasn <= 5056.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Project Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Project Sales'
WHERE (c.coasn >= 5057 and c.coasn <= 5057.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Mgmt Fee
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Mgmt Fee'
WHERE (c.coasn >= 5061 and c.coasn <= 5061.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel Sales-Property Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel Sales-Property Rental'
WHERE (c.coasn >= 5062 and c.coasn <= 5062.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Cost of External Sales [5101 to 5119]
--******************************
--External COS-Goods
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Goods'
WHERE (c.coasn >= 5101 AND c.coasn <= 5101.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External COS-Cylinder Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Cylinder Rental'
WHERE (c.coasn >= 5102 AND c.coasn <= 5102.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External COS-Equip Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Equip Rental'
WHERE (c.coasn >= 5103 AND c.coasn <= 5103.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External COS-Repair & Service
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Repair & Service'
WHERE(c.coasn >= 5104 AND c.coasn <= 5104.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External COS-Delivery Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Delivery Charges'
WHERE (c.coasn >= 5105 and c.coasn <= 5105.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External COS-Other Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Other Charges'
WHERE (c.coasn >= 5106 and c.coasn <= 5106.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External COS-Project Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Project Sales'
WHERE (c.coasn >= 5107 AND c.coasn <= 5107.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--External COS-Prov for Bill Loss
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)External COS-Prov for Bill Loss'
WHERE (c.coasn >= 5108 AND c.coasn <= 5108.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Manufacturing Cost
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Manufacturing Cost'
WHERE (c.coasn >= 6000 AND c.coasn <= 6999.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--******************************
--Cost of Interco Sales [5121 to 5139]
--******************************
--Interco COS-Goods
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Goods'
WHERE (c.coasn >= 5121 and c.coasn <= 5121.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco COS-Cylinder Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Cylinder Rental'
WHERE (c.coasn >= 5122 and c.coasn <= 5122.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco COS-Equip Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Equip Rental'
WHERE (c.coasn >= 5123 and c.coasn <= 5123.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco COS-Repair & Service
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Repair & Service'
WHERE (c.coasn >= 5124 and c.coasn <= 5124.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco COS-Delivery Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Delivery Charges'
WHERE (c.coasn >= 5125 and c.coasn <= 5125.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco COS-Other Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Other Charges'
WHERE (c.coasn >= 5126 and c.coasn <= 5126.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco COS-Project Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Project Sales'
WHERE (c.coasn >= 5127 and c.coasn <= 5127.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interco COS-Prov for Bill Loss
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interco COS-Prov for Bill Loss'
WHERE (c.coasn >= 5128 and c.coasn <= 5128.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Cost of Assoc/JV/Rel Sales [5101 to 5119]
--******************************
--Assoc/JV/Rel COS-Goods
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel COS-Goods'
WHERE (c.coasn >= 5141 and c.coasn <= 5141.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel COS-Cylinder Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel COS-Cylinder Rental'
WHERE (c.coasn >= 5142 and c.coasn <= 5142.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel COS-Equip Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel COS-Equip Rental'
WHERE (c.coasn >= 5143 and c.coasn <= 5143.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel COS-Repair & Service
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel COS-Repair & Service'
WHERE (c.coasn >= 5144 and c.coasn <= 5144.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel COS-Delivery Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel COS-Delivery Charges'
WHERE (c.coasn >= 5145 and c.coasn <= 5145.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel COS-Other Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel COS-Other Charges'
WHERE (c.coasn >= 5146 and c.coasn <= 5146.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Assoc/JV/Rel COS-Project Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Assoc/JV/Rel COS-Project Sales'
WHERE (c.coasn >= 5147 and c.coasn <= 5147.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--[5201 to 5223]
--******************************
--Carriage Inwards
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Carriage Inwards'
WHERE (c.coasn >= 5201 and c.coasn <= 5201.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Insurance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Insurance'
WHERE (c.coasn >= 5202 and c.coasn <= 5202.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Handling Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Handling Charges'
WHERE (c.coasn >= 5203 and c.coasn <= 5203.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Import/Custom Duties
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Import/Custom Duties'
WHERE (c.coasn >= 5204 and c.coasn <= 5204.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Rental Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Dep-Rental Equipment'
WHERE (c.coasn >= 5211 and c.coasn <= 5211.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Onsite Plant
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Dep-Onsite Plant'
WHERE (c.coasn >= 5221 and c.coasn <= 5221.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Onsite Storage Tanks
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Dep-Onsite Storage Tanks'
WHERE (c.coasn >= 5222 and c.coasn <= 5222.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dep-Racks & Cylinders
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Dep-Racks & Cylinders'
WHERE (c.coasn >= 5223 and c.coasn <= 5223.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Other Operating Income [5500 to 5599]
--******************************
--Commission Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Commission Income'
WHERE (c.coasn >= 5501 and c.coasn <= 5501.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Property Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Property Rental Income'
WHERE (c.coasn >= 5502 and c.coasn <= 5502.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Admin/Management Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Admin/Management Income'
WHERE (c.coasn >= 5503 and c.coasn <= 5503.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain/(Loss) on Disposal of FA
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain/(Loss) on Disposal of FA'
WHERE (c.coasn >= 9101 and c.coasn <= 9101.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Income on Disposal of Scrap
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Income on Disposal of Scrap'
WHERE (c.coasn >= 5511 and c.coasn <= 5511.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Insurance Claim - Op
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Insurance Claim - Op'
WHERE (c.coasn = 5512 and c.coasn <= 5512.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Insurance Claim
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Insurance Claim'
WHERE (c.coasn >= 5512 and c.coasn <= 5512.999)
OR (c.coasn >= 9011 and c.coasn <= 9011.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Logistic Service Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Logistic Service Income'
WHERE (c.coasn >= 5513 and c.coasn <= 5513.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Other Misc Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Other Misc Income'
WHERE (c.coasn >= 5519 and c.coasn <= 5519.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Government Grant
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Government Grant'
WHERE (c.coasn >= 9001 and c.coasn <= 9001.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Insurance Claim - Non-Op
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Insurance Claim - Non-Op'
WHERE (c.coasn = 9011 and c.coasn <= 9011.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Share of Assoc/JV’s P&L
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Share of Assoc/JV’s P&L'
WHERE (c.coaid = '9041')
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Share of Assoc/JV’s P&L Credit
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Share of Assoc/JV’s P&L Credit'
WHERE t.YTDTotal > 0 AND (c.coasn >= 9041 and c.coasn <= 9041.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain on Disposal of Investment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain on Disposal of Investment'
WHERE t.YTDTotal > 0 AND (c.coasn >= 9102 and c.coasn <= 9102.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain on Asset Held for Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain on Asset Held for Sales'
WHERE t.YTDTotal > 0 AND (c.coasn >= 9103 and c.coasn <= 9103.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--******************************
--Other Operating Expenses [5601 to 5999]
--******************************

--Other Misc Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Other Misc Expenses'
WHERE (c.coasn >= 5629 and c.coasn <= 5629.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Other Non-Operating Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Other Non-Operating Exps'
WHERE (c.coasn >= 9199 and c.coasn <= 9199.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Share of Assoc/JV’s P&L Debit
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Share of Assoc/JV’s P&L Debit'
WHERE t.YTDTotal < 0 AND (c.coasn >= 9041 and c.coasn <= 9041.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--Loss on Disposal of Investment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Loss on Disposal of Investment'
WHERE t.YTDTotal < 0 AND (c.coasn >= 9102 and c.coasn <= 9102.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Loss on Asset Held for Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Loss on Asset Held for Sales'
WHERE  t.YTDTotal < 0 AND (c.coasn >= 9103 and c.coasn <= 9103.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Impairm Loss on Subsidiaries
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Impairm Loss on Subsidiaries'
WHERE (c.coasn >= 9111 and c.coasn <= 9111.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Impairm Loss on Assoc/JV/Rel Co
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Impairm Loss on Assoc/JV/Rel Co'
WHERE (c.coasn >= 9112 and c.coasn <= 9112.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Impairm Loss-Property
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Impairm Loss-Property'
WHERE (c.coasn >= 9113 and c.coasn <= 9113.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Impairm Loss-Plant & Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Impairm Loss-Plant & Equip'
WHERE (c.coasn >= 9114 and c.coasn <= 9114.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Impairm Loss-Investment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Impairm Loss-Investment'
WHERE (c.coasn >= 9115 and c.coasn <= 9115.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Impairm Loss-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Impairm Loss-Others'
WHERE (c.coasn >= 9117 and c.coasn <= 9117.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Real Property Gain Tax
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Real Property Gain Tax'
WHERE (c.coasn >= 9121 and c.coasn <= 9121.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Foreign Withholding Tax
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Foreign Withholding Tax'
WHERE (c.coasn >= 9122 and c.coasn <= 9122.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Amortisation-Trade Name
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Amortisation-Trade Name'
WHERE (c.coasn = 5601)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Amortisation-Product License
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Amortisation-Product License'
WHERE (c.coasn >= 9131 and c.coasn <= 9131.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Amortisation-Club Membership
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Amortisation-Club Membership'
WHERE (c.coasn >= 9132 and c.coasn <= 9132.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Amortisation-Trademark & Patent
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Amortisation-Trademark & Patent'
WHERE (c.coasn >= 9133 and c.coasn <= 9133.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Amortisation-Cust Relationship
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Amortisation-Cust Relationship'
WHERE (c.coasn = 5602 )
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Written Off-Trade Name
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Written Off-Trade Name'
WHERE (c.coasn = 5611)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Written Off-Cust Relationship
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Written Off-Cust Relationship'
WHERE (c.coasn = 5612)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Written Off-Trademark & Patent
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Written Off-Trademark & Patent'
WHERE (c.coasn = 5613)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Written Off-Product License
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Written Off-Product License'
WHERE (c.coasn = 5614)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--Written Off-Club Membership
/*
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Written Off-Club Membership'
WHERE (c.coasn = 5615)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
*/

--******************************
--S&D Expenses [7001 to 7999]
--******************************
--S&D Salary & Leave
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Salary & Leave'
WHERE (c.coasn >= 7001 and c.coasn <= 7001.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Provision for Bonus
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Provision for Bonus'
WHERE (c.coasn >= 7002 and c.coasn <= 7002.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Comms & Incentives
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Comms & Incentives'
WHERE (c.coasn >= 7003 and c.coasn <= 7003.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Foreign Workers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Foreign Workers'
WHERE (c.coasn >= 7004 and c.coasn <= 7004.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Casual/Temp Staff
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Casual/Temp Staff'
WHERE (c.coasn >= 7005 and c.coasn <= 7005.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Overtime
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Overtime'
WHERE (c.coasn >= 7006 and c.coasn <= 7006.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Allow & Incentive
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Allow & Incentive'
WHERE (c.coasn >= 7007 and c.coasn <= 7007.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Shift Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Shift Allowance'
WHERE (c.coasn >= 7008 and c.coasn <= 7008.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Transport
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Transport'
WHERE (c.coasn >= 7009 and c.coasn <= 7009.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Vehicle Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Vehicle Expenses'
WHERE (c.coasn >= 7010 and c.coasn <= 7010.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Vehicle Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Vehicle Rental'
WHERE (c.coasn >= 7011 and c.coasn <= 7011.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Social Securities
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Social Securities'
WHERE (c.coasn >= 7012 and c.coasn <= 7012.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Worker`s Levy
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Worker`s Levy'
WHERE (c.coasn >= 7013 and c.coasn <= 7013.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Recruitment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Recruitment'
WHERE (c.coasn >= 7014 and c.coasn <= 7014.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Training
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Training'
WHERE (c.coasn >= 7015 and c.coasn <= 7015.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Insurance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Insurance'
WHERE (c.coasn >= 7016 and c.coasn <= 7016.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Medical
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Medical'
WHERE (c.coasn >= 7017 and c.coasn <= 7017.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Welfare
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Welfare'
WHERE (c.coasn >= 7018 and c.coasn <= 7018.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Uniforms/PPE
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Uniforms/PPE'
WHERE (c.coasn >= 7019 and c.coasn <= 7019.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Staff Accommodation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Staff Accommodation'
WHERE (c.coasn >= 7020 and c.coasn <= 7020.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Other Staff Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Other Staff Expenses'
WHERE (c.coasn >= 7021 and c.coasn <= 7021.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Expats` Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Expats` Allowance'
WHERE (c.coasn >= 7031 and c.coasn <= 7031.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Expats` Housing
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Expats` Housing'
WHERE (c.coasn >= 7032 and c.coasn <= 7032.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Expats` Transportation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Expats` Transportation'
WHERE (c.coasn >= 7033 and c.coasn <= 7033.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Expats` Education
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Expats` Education'
WHERE (c.coasn >= 7034 and c.coasn <= 7034.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Expats` Phone, Internet, Etc.
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Expats` Phone, Internet, Etc.'
WHERE (c.coasn >= 7035 and c.coasn <= 7035.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Expats` Other Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Expats` Other Expenses'
WHERE (c.coasn >= 7036 and c.coasn <= 7036.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Carriage Outwards
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Carriage Outwards'
WHERE (c.coasn >= 7101 and c.coasn <= 7101.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Transport-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Transport-Others'
WHERE (c.coasn >= 7102 and c.coasn <= 7102.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Road Tax-Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Road Tax-Motor Vehicle'
WHERE (c.coasn >= 7103 and c.coasn <= 7103.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Inspection Fee-Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Inspection Fee-Motor Vehicle'
WHERE (c.coasn >= 7104 and c.coasn <= 7104.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Leasing of Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Leasing of Motor Vehicle'
WHERE (c.coasn >= 7105 and c.coasn <= 7105.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Upkeep of Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Upkeep of Motor Vehicle'
WHERE (c.coasn >= 7106 and c.coasn <= 7106.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Endorsement Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Endorsement Fees'
WHERE (c.coasn >= 7107 and c.coasn <= 7107.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Export Packaging Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Export Packaging Exps'
WHERE (c.coasn >= 7108 and c.coasn <= 7108.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D A&P Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D A&P Expenses'
WHERE (c.coasn >= 7201 AND c.coasn <= 7201.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Exhibition Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Exhibition Expenses'
WHERE (c.coasn >= 7202 AND c.coasn <= 7202.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Business Development
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Business Development'
WHERE (c.coasn >= 7203 AND c.coasn <= 7203.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Product Development
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Product Development'
WHERE (c.coasn >= 7204 AND c.coasn <= 7204.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Overseas Travelling
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Overseas Travelling'
WHERE (c.coasn >= 7301 AND c.coasn <= 7301.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Entertainment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Entertainment'
WHERE (c.coasn >= 7302 AND c.coasn <= 7302.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Non-capitalised Purchases
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Non-capitalised Purchases'
WHERE (c.coasn >= 7401 AND c.coasn <= 7401.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Leasing of Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Leasing of Equipment'
WHERE (c.coasn >= 7402 AND c.coasn <= 7402.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D R&M-Equipment 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D R&M-Equipment '
WHERE (c.coasn >= 7403 AND c.coasn <= 7403.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D R&M-Machinery
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D R&M-Machinery'
WHERE (c.coasn >= 7404 AND c.coasn <= 7404.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D R&M-F&F
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D R&M-F&F'
WHERE (c.coasn >= 7405 AND c.coasn <= 7405.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D R&M-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D R&M-Office Equip'
WHERE (c.coasn >= 7406 AND c.coasn <= 7406.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D R&M-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D R&M-Computers'
WHERE (c.coasn >= 7407 AND c.coasn <= 7407.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D R&M-Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D R&M-Motor Vehicles'
WHERE (c.coasn >= 7408 AND c.coasn <= 7408.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Insurance of Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Insurance of Equipment'
WHERE (c.coasn >= 7409 AND c.coasn <= 7409.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Insurance of Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Insurance of Vehicles'
WHERE (c.coasn >= 7410 AND c.coasn <= 7410.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Upkeep-Machinery
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Upkeep-Machinery'
WHERE (c.coasn >= 7411 AND c.coasn <= 7411.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Upkeep-Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Upkeep-Equipment'
WHERE (c.coasn >= 7412 AND c.coasn <= 7412.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Upkeep-Furniture & Fitting
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Upkeep-Furniture & Fitting'
WHERE (c.coasn >= 7413 AND c.coasn <= 7413.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Upkeep-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Upkeep-Office Equip'
WHERE (c.coasn >= 7414 AND c.coasn <= 7414.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Upkeep-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Upkeep-Computers'
WHERE (c.coasn >= 7415 AND c.coasn <= 7415.999) 
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Rental'
WHERE (c.coasn >= 7501 and c.coasn <= 7501.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Upkeep of Premises
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Upkeep of Premises'
WHERE (c.coasn >= 7502 and c.coasn <= 7502.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Waste Disposal
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Waste Disposal'
WHERE (c.coasn >= 7503 and c.coasn <= 7503.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Electricity Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Electricity Charges'
WHERE (c.coasn >= 7504 and c.coasn <= 7504.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Water Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Water Charges'
WHERE (c.coasn >= 7505 and c.coasn <= 7505.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Gas Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Gas Charges'
WHERE (c.coasn >= 7506 and c.coasn <= 7506.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Property Tax
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Property Tax'
WHERE (c.coasn >= 7507 and c.coasn <= 7507.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Licenses Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Licenses Fees'
WHERE (c.coasn >= 7508 and c.coasn <= 7508.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Insurance-General 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Insurance-General'
WHERE (c.coasn >= 7509 and c.coasn <= 7509.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Leasehold Land
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Leasehold Land'
WHERE (c.coasn >= 7601 and c.coasn <= 7601.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Land Rights Use
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Land Rights Use'
WHERE (c.coasn >= 7602 and c.coasn <= 7602.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Buildings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Buildings'
WHERE (c.coasn >= 7603 and c.coasn <= 7603.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Renovations
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Renovations'
WHERE (c.coasn >= 7604 and c.coasn <= 7604.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Machinery
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Machinery'
WHERE (c.coasn >= 7605 and c.coasn <= 7605.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Equipment'
WHERE (c.coasn >= 7606 and c.coasn <= 7606.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Onsite Pipelines 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Onsite Pipelines'
WHERE (c.coasn >= 7607 and c.coasn <= 7607.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-ISO Tanks
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-ISO Tanks'
WHERE (c.coasn >= 7608 and c.coasn <= 7608.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Tools
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Tools'
WHERE (c.coasn >= 7609 and c.coasn <= 7609.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Furniture & Fittings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Furniture & Fittings'
WHERE (c.coasn >= 7610 and c.coasn <= 7610.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Office Equip'
WHERE (c.coasn >= 7611 and c.coasn <= 7611.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Computers'
WHERE (c.coasn >= 7612 and c.coasn <= 7612.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Dep-Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Dep-Motor Vehicles'
WHERE (c.coasn >= 7613 and c.coasn <= 7613.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Postage & Mail Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Postage & Mail Charges'
WHERE (c.coasn >= 7701 and c.coasn <= 7701.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Printing & Stationaries
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Printing & Stationaries'
WHERE (c.coasn >= 7702 and c.coasn <= 7702.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Telephone/Fax/Videoconference
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Telephone/Fax/Videoconference'
WHERE (c.coasn >= 7703 and c.coasn <= 7703.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Internet
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Internet'
WHERE (c.coasn >= 7704 and c.coasn <= 7704.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Subscription-Prof Bodies (S&D)
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Subscription-Prof Bodies (S&D)'
WHERE (c.coasn >= 7705 and c.coasn <= 7705.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Credit Card Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Credit Card Charges'
WHERE (c.coasn >= 7706 and c.coasn <= 7706.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Donations
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Donations'
WHERE (c.coasn >= 7707 and c.coasn <= 7707.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Other Misc Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Other Misc Expenses'
WHERE (c.coasn >= 7708 and c.coasn <= 7708.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D A&P Marcom Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D A&P Marcom Exps'
WHERE (c.coasn >= 7901 and c.coasn <= 7901.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Warehouse & Logistic Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Warehouse & Logistic Exps'
WHERE (c.coasn >= 7903 and c.coasn <= 7903.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Product Support Welding
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Product Support Welding'
WHERE (c.coasn >= 7910 and c.coasn <= 7910.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Product Support Safety
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Product Support Safety'
WHERE (c.coasn >= 7920 and c.coasn <= 7920.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Recovery to/from Interco
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Recovery to/from Interco'
WHERE (c.coasn >= 7998 and c.coasn <= 7998.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--S&D Head Office Costs/Alloc
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)S&D Head Office Costs/Alloc'
WHERE (c.coasn >= 7999 and c.coasn <= 7999.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--A&G Expenses [8001 to 8999]
--******************************
--A&G Salary & Leave
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Salary & Leave'
WHERE (c.coasn >= 8001 and c.coasn <= 8001.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Provision for Bonus
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Provision for Bonus'
WHERE (c.coasn >= 8002 and c.coasn <= 8002.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Comms & Incentives
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Comms & Incentives'
WHERE (c.coasn >= 8003 and c.coasn <= 8003.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Foreign Workers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Foreign Workers'
WHERE (c.coasn >= 8004 and c.coasn <= 8004.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Casual/Temp Staff
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Casual/Temp Staff'
WHERE (c.coasn >= 8005 and c.coasn <= 8005.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Overtime
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Overtime'
WHERE (c.coasn >= 8006 and c.coasn <= 8006.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Allow & Incentive
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Allow & Incentive'
WHERE (c.coasn >= 8007 and c.coasn <= 8007.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Shift Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Shift Allowance'
WHERE (c.coasn >= 8008 and c.coasn <= 8008.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Transport
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Transport'
WHERE (c.coasn >= 8009 and c.coasn <= 8009.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Vehicle Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Vehicle Expenses'
WHERE (c.coasn >= 8010 and c.coasn <= 8010.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Vehicle Rental
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Vehicle Rental'
WHERE (c.coasn >= 8011 and c.coasn <= 8011.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Social Securities
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Social Securities'
WHERE (c.coasn >= 8012 and c.coasn <= 8012.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Worker`s Levy
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Workers` Levy'
WHERE (c.coasn >= 8013 and c.coasn <= 8013.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Recruitment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Recruitment'
WHERE (c.coasn >= 8014 and c.coasn <= 8014.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Training
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Training'
WHERE (c.coasn >= 8015 and c.coasn <= 8015.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Insurance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Insurance'
WHERE (c.coasn >= 8016 and c.coasn <= 8016.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Medical
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Medical'
WHERE (c.coasn >= 8017 and c.coasn <= 8017.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Welfare
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Welfare'
WHERE (c.coasn >= 8018 and c.coasn <= 8018.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Uniforms/PPE
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Uniforms/PPE'
WHERE (c.coasn >= 8019 and c.coasn <= 8019.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Staff Accommodation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Staff Accommodation'
WHERE (c.coasn >= 8020 and c.coasn <= 8020.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Other Staff Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Other Staff Expenses'
WHERE (c.coasn >= 8021 and c.coasn <= 8021.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Expats` Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Expats` Allowance'
WHERE (c.coasn >= 8031 and c.coasn <= 8031.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Expats` Housing
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Expats` Housing'
WHERE (c.coasn >= 8032 and c.coasn <= 8032.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Expats` Transportation
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Expats` Transportation'
WHERE (c.coasn >= 8033 and c.coasn <= 8033.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Expats` Education
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Expats` Education'
WHERE (c.coasn >= 8034 and c.coasn <= 8034.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Expats` Phone, Internet, Etc.
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Expats` Phone, Internet, Etc.'
WHERE (c.coasn >= 8035 and c.coasn <= 8035.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Expats` Other Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Expats` Other Expenses'
WHERE (c.coasn >= 8036 and c.coasn <= 8036.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Directors Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Directors Fees'
WHERE (c.coasn >= 8041 and c.coasn <= 8041.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Directors Remuneration
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Directors Remuneration'
WHERE (c.coasn >= 8042 and c.coasn <= 8042.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Directors Allowance
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Directors Allowance'
WHERE (c.coasn >= 8043 and c.coasn <= 8043.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Overseas Travelling
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Overseas Travelling'
WHERE (c.coasn >= 8101 and c.coasn <= 8101.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Transport-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Transport-Others'
WHERE (c.coasn >= 8102 and c.coasn <= 8102.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Road Tax-Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Road Tax-Motor Vehicle'
WHERE (c.coasn >= 8103 and c.coasn <= 8103.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Inspection Fee-Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Inspection Fee-Motor Vehicle'
WHERE (c.coasn >= 8104 and c.coasn <= 8104.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Leasing of Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Leasing of Motor Vehicle'
WHERE (c.coasn >= 8105 and c.coasn <= 8105.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Upkeep of Motor Vehicle
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Upkeep of Motor Vehicle'
WHERE (c.coasn >= 8106 and c.coasn <= 8106.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Entertainment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Entertainment'
WHERE (c.coasn >= 8151 and c.coasn <= 8151.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Donations
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Donations'
WHERE (c.coasn >= 8152 and c.coasn <= 8152.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Leasing of Machinery & Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Leasing of Machinery & Equip'
WHERE (c.coasn >= 8201 and c.coasn <= 8201.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Non-Capitalised Purchases
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Non-Capitalised Purchases'
WHERE (c.coasn >= 8202 and c.coasn <= 8202.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G R&M-Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G R&M-Equipment'
WHERE (c.coasn >= 8203 and c.coasn <= 8203.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G R&M-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G R&M-Office Equip'
WHERE (c.coasn >= 8204 and c.coasn <= 8204.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G R&M-F&F
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G R&M-F&F'
WHERE (c.coasn >= 8205 and c.coasn <= 8205.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G R&M-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G R&M-Computers'
WHERE (c.coasn >= 8206 and c.coasn <= 8206.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G R&M-Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G R&M-Motor Vehicles'
WHERE (c.coasn >= 8207 and c.coasn <= 8207.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Upkeep-Equipment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Upkeep-Equipment'
WHERE (c.coasn >= 8207 and c.coasn <= 8207.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Upkeep-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Upkeep-Office Equip'
WHERE (c.coasn >= 8207 and c.coasn <= 8207.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Rental of Premises
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Rental of Premises'
WHERE (c.coasn >= 8301 and c.coasn <= 8301.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Upkeep of Premises
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Upkeep of Premises'
WHERE (c.coasn >= 8302 and c.coasn <= 8302.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Waste Disposal
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Waste Disposal'
WHERE (c.coasn >= 8303 and c.coasn <= 8303.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Electricity Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Electricity Charges'
WHERE (c.coasn >= 8304 and c.coasn <= 8304.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Water Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Water Charges'
WHERE (c.coasn >= 8305 and c.coasn <= 8305.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Gas Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Gas Charges'
WHERE (c.coasn >= 8306 and c.coasn <= 8306.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Property Tax
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Property Tax'
WHERE (c.coasn >= 8307 and c.coasn <= 8307.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Assessment Fee
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Assessment Fee'
WHERE (c.coasn >= 8308 and c.coasn <= 8308.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Stamp Duty 
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Stamp Duty'
WHERE (c.coasn >= 8309 and c.coasn <= 8309.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Licenses Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Licenses Fees'
WHERE (c.coasn >= 8310 and c.coasn <= 8310.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Leasehold Land
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Leasehold Land'
WHERE (c.coasn >= 8401 and c.coasn <= 8401.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Land Rights Use
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Land Rights Use'
WHERE (c.coasn >= 8402 and c.coasn <= 8402.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Buildings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Buildings'
WHERE (c.coasn >= 8403 and c.coasn <= 8403.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Renovations
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Renovations'
WHERE (c.coasn >= 8404 and c.coasn <= 8404.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Investment Property
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Investment Property'
WHERE (c.coasn >= 8405 and c.coasn <= 8405.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Furniture & Fittings
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Furniture & Fittings'
WHERE (c.coasn >= 8406 and c.coasn <= 8406.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Office Equip
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Office Equip'
WHERE (c.coasn >= 8407 and c.coasn <= 8407.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Computers
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Computers'
WHERE (c.coasn >= 8408 and c.coasn <= 8408.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Dep-Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Dep-Motor Vehicles'
WHERE (c.coasn >= 8409 and c.coasn <= 8409.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Postage & Mail Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Postage & Mail Charges'
WHERE (c.coasn >= 8501 and c.coasn <= 8501.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Printing & Stationaries
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Printing & Stationaries'
WHERE (c.coasn >= 8502 and c.coasn <= 8502.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Telephone/Fax/Teleconference
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Telephone/Fax/Teleconference'
WHERE (c.coasn >= 8503 and c.coasn <= 8503.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Internet
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Internet'
WHERE (c.coasn >= 8504 and c.coasn <= 8504.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Subscription
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Subscription'
WHERE (c.coasn >= 8505 and c.coasn <= 8505.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Directors` Fee
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Directors` Fee'
WHERE (c.coasn >= 8601 and c.coasn <= 8601.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Audit Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Audit Fees'
WHERE (c.coasn >= 8602 and c.coasn <= 8602.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Secretarial Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Secretarial Fees'
WHERE (c.coasn >= 8603 and c.coasn <= 8603.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Tax Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Tax Fees'
WHERE (c.coasn >= 8604 and c.coasn <= 8604.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Legal Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Legal Fees'
WHERE (c.coasn >= 8605 and c.coasn <= 8605.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Professional Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Professional Fees'
WHERE (c.coasn >= 8606 and c.coasn <= 8606.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Consultancy Fees
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Consultancy Fees'
WHERE (c.coasn >= 8607 and c.coasn <= 8607.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Security Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Security Charges'
WHERE (c.coasn >= 8608 and c.coasn <= 8608.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Safety Supervision
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Safety Supervision'
WHERE (c.coasn >= 8610 and c.coasn <= 8610.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Insurance-General
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Insurance-General'
WHERE (c.coasn >= 8621 and c.coasn <= 8621.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Insurance-Motor Vehicles
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Insurance-Motor Vehicles'
WHERE (c.coasn >= 8622 and c.coasn <= 8622.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Insurance-Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Insurance-Others'
WHERE (c.coasn >= 8623 and c.coasn <= 8623.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Bank Charges
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Bank Charges'
WHERE (c.coasn >= 8631 and c.coasn <= 8631.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G IT System Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G IT System Exps'
WHERE (c.coasn >= 8651 and c.coasn <= 8651.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Purchasing Exps
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Purchasing Exps'
WHERE (c.coasn >= 8653 and c.coasn <= 8653.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Prov for Stock Obsolescence
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Prov for Stock Obsolescence'
WHERE (c.coasn >= 8701 and c.coasn <= 8701.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Prov for Pilferage
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Prov for Pilferage'
WHERE (c.coasn >= 8702 and c.coasn <= 8702.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Prov for Doubtful Debt
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Prov for Doubtful Debt'
WHERE (c.coasn >= 8703 and c.coasn <= 8703.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Bad Debt Written Off/Recovered
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Bad Debt Written Off/Recovered'
WHERE (c.coasn >= 8704 and c.coasn <= 8704.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Stocks Written Down/Written Off
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Stocks Written Down/Written Off'
WHERE (c.coasn >= 8705 and c.coasn <= 8705.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G GST/VAT Expense Off
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G GST/VAT Expense Off'
WHERE (c.coasn >= 8711 and c.coasn <= 8711.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Foreign GST/VAT Expense Off-Interco
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Foreign GST/VAT Expense Off-Interco'
WHERE (c.coasn >= 8712 and c.coasn <= 8712.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Recovery to/(from) Interco
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Recovery to/(from) Interco'
WHERE (c.coasn >= 8798 and c.coasn <= 8798.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--A&G Head Office Costs/Alloc
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)A&G Head Office Costs/Alloc'
WHERE (c.coasn >= 8799 and c.coasn <= 8799.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Finance Expenses [8801 to 8999]
--******************************
--Interest-Bank Overdraft
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Bank Overdraft'
WHERE (c.coasn >= 8801 and c.coasn <= 8801.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Factoring/TR/Bill Payback
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Factoring/TR/Bill Payback'
WHERE (c.coasn >= 8802 and c.coasn <= 8802.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Short Term Loan
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Short Term Loan'
WHERE (c.coasn >= 8803 and c.coasn <= 8803.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Term Loan
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Term Loan'
WHERE (c.coasn >= 8804 and c.coasn <= 8804.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Hire Purchase
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Hire Purchase'
WHERE (c.coasn >= 8805 and c.coasn <= 8805.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Loan from Interco
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Loan from Interco'
WHERE (c.coasn >= 8806 and c.coasn <= 8806.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Loan from Assoc/Rel
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Loan from Assoc/Rel'
WHERE (c.coasn >= 8807 and c.coasn <= 8807.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Loan from Shareholders
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Loan from Shareholders'
WHERE (c.coasn >= 8808 and c.coasn <= 8808.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest-Loan from Others
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest-Loan from Others'
WHERE (c.coasn >= 8809 and c.coasn <= 8809.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Non Operating Income/Expenses [9001 to 9399]
--******************************
--Gain/(Loss) on Derivatives-Reald
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain/(Loss) on Derivatives-Reald'
WHERE (c.coasn >= 8821 and c.coasn <= 8821.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain/(Loss) on Derivatives-Unreald
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain/(Loss) on Derivatives-Unreald'
WHERE (c.coasn >= 8822 and c.coasn <= 8822.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain/(loss) on Exch Diff-Reald
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain/(loss) on Exch Diff-Reald'
WHERE (c.coasn >= 8831 and c.coasn <= 8831.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain/(Loss) on Exch Diff-Unreald
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain/(Loss) on Exch Diff-Unreald'
WHERE (c.coasn >= 8832 and c.coasn <= 8832.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dividend Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Dividend Income'
WHERE (c.coasn >= 9021 and c.coasn <= 9021.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dividend Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Dividend Expenses'
WHERE (c.coasn >= 9022 and c.coasn <= 9022.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Interest Income
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Interest Income'
WHERE (c.coasn >= 9023 and c.coasn <= 9023.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Minority Interest
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Minority Interest'
WHERE (c.coasn >= 9031 and c.coasn <= 9031.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain/(Loss) on Disposal of Investment
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain/(Loss) on Disposal of Investment'
WHERE (c.coasn >= 9102 and c.coasn <= 9102.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Gain/(Loss) on Asset Held for Sales
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Gain/(Loss) on Asset Held for Sales'
WHERE (c.coasn >= 9103 and c.coasn <= 9103.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Impairm Loss-Goodwill
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Impairm Loss-Goodwill'
WHERE (c.coasn >= 9116 and c.coasn <= 9116.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Negative Goodwill
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Negative Goodwill'
WHERE (c.coasn >= 9201 and c.coasn <= 9201.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--ESOS Expenses
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)ESOS Expenses'
WHERE (c.coasn >= 9211 and c.coasn <= 9211.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId


--******************************
--Less: Taxation [9401 to 9999]
--******************************
--Taxation-Current Year
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Taxation-Current Year'
WHERE (c.coasn >= 9401 and c.coasn <= 9401.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Taxation-Prior Year
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Taxation-Prior Year'
WHERE (c.coasn >= 9402 and c.coasn <= 9402.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Deferred Tax-Current Year
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Deferred Tax-Current Year'
WHERE (c.coasn >= 9403 and c.coasn <= 9403.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Deferred Tax-Prior Year
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Deferred Tax-Prior Year'
WHERE (c.coasn >= 9404 and c.coasn <= 9404.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId

--Dividends Declared
Insert INTO tbFinanceData
SELECT t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId, 
isnull(sum(MTDTotal),0)/-1000 AS MTD, isnull(sum(YTDTotal), 0)/-1000 AS YTD, 
getdate() as CreatedDate  
FROM tbTempTransfer t WITH (NOLOCK)
INNER JOIN tbChartOfAccounts c WITH (NOLOCK) ON t.COAID = c.COAID
INNER JOIN tbFinanceItem i WITH (NOLOCK) ON i.itemname = '(PNLD)Dividends Declared'
WHERE (c.coasn >= 4902 and c.coasn <= 4902.999)
GROUP BY t.CoyID, ProjectID, DepartmentID, SectionID, TBYear, TBMonth, i.ItemId
