ALTER PROCEDURE [dbo].[procFinanceTBProcess_COA2016_SAP_InsertBSDetailSummary]
AS 
--***************************************************************************
--BSD
--***************************************************************************
--Total Current Assets
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Current Assets'
WHERE i.ItemName = '(BSD)Cash and Bank Balances'
or i.ItemName = '(BSD)Fixed Deposits'
or i.ItemName = '(BSD)Raw Materials'
or i.ItemName = '(BSD)Prov for Raw Materials'
or i.ItemName = '(BSD)Semi-Finished Goods'
or i.ItemName = '(BSD)Work in Progress'
or i.ItemName = '(BSD)Packing & Labelling Materials'
or i.ItemName = '(BSD)Prov for Packing & Label Material'
or i.ItemName = '(BSD)Consumables'
or i.ItemName = '(BSD)Stocks In Transit'
or i.ItemName = '(BSD)Finished Goods'
or i.ItemName = '(BSD)Prov for Finished Goods'
or i.ItemName = '(BSD)Trading Stocks'
or i.ItemName = '(BSD)Prov for Trading Stocks'
or i.ItemName = '(BSD)Prov for Stock Pilferage'
or i.ItemName = '(BSD)Stocks Return'
or i.ItemName = '(BSD)COGS Clearing Account'
or i.ItemName = '(BSD)Stocks Adjustment Account'
or i.ItemName = '(BSD)Trade Debtors'
or i.ItemName = '(BSD)Prov for Doubtful Debt'
or i.ItemName = '(BSD)Due from Interco-Trade'
or i.ItemName = '(BSD)Prov for DD-Interco Trade'
or i.ItemName = '(BSD)Due from Interco-Non-Trade'
or i.ItemName = '(BSD)Prov for DD-Interco Non-Trade'
or i.ItemName = '(BSD)Due from Interco-Loan'
or i.ItemName = '(BSD)Prov for DD-Interco Loan'
or i.ItemName = '(BSD)Due from Interco-Non-Trade Clearing'
or i.ItemName = '(BSD)Due from JV Co-Trade'
or i.ItemName = '(BSD)Prov for DD-JV Co Trade'
or i.ItemName = '(BSD)Due from JV Co-Non-Trade'
or i.ItemName = '(BSD)Prov for DD-JV Co Non-Trade'
or i.ItemName = '(BSD)Due from JV Co-Loan'
or i.ItemName = '(BSD)Prov for DD-JV Co Loan'
or i.ItemName = '(BSD)Due from Assoc-Trade'
or i.ItemName = '(BSD)Prov for DD-Assoc Trade'
or i.ItemName = '(BSD)Due from Assoc-Non-Trade'
or i.ItemName = '(BSD)Prov for DD-Assoc Non-Trade'
or i.ItemName = '(BSD)Due from Assoc-Loan'
or i.ItemName = '(BSD)Prov for DD-Assoc Loan'
or i.ItemName = '(BSD)Due from Assoc-Non-Trade Clearing'
or i.ItemName = '(BSD)Due from Rel-Trade'
or i.ItemName = '(BSD)Prov for DD-Rel Trade'
or i.ItemName = '(BSD)Due from Rel-Non-Trade'
or i.ItemName = '(BSD)Prov for DD-Rel Non-Trade'
or i.ItemName = '(BSD)Due from Rel-Non-Trade Clearing'
or i.ItemName = '(BSD)Prov for Gain/Loss in FX (Debtors)'
or i.ItemName = '(BSD)Marketable Securities'
or i.ItemName = '(BSD)Prov for Gain/Loss In Mkt Sec'
or i.ItemName = '(BSD)Assets Held For Sale'
or i.ItemName = '(BSD)Tax Recoverable'
or i.ItemName = '(BSD)GST-Input'
or i.ItemName = '(BSD)Prepayment'
or i.ItemName = '(BSD)Deposit'
or i.ItemName = '(BSD)Advance to Supplier'
or i.ItemName = '(BSD)Staff Advance'
or i.ItemName = '(BSD)Staff Loan'
or i.ItemName = '(BSD)Interest Receivables'
or i.ItemName = '(BSD)Recoverable Exps'
or i.ItemName = '(BSD)Other Misc Debtors'
or i.ItemName = '(BSD)Sundry Debtors'
or i.ItemName = '(BSD)Suspense-GST'
or i.ItemName = '(BSD)Suspense-HR'
or i.ItemName = '(BSD)Suspense-Others'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Net Fixed Assets
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Net Fixed Assets'
WHERE i.ItemName = '(BSD)Land-Freehold'
or i.ItemName = '(BSD)Prov for Impairment-Freehold Land'
or i.ItemName = '(BSD)Land-Leasehold'
or i.ItemName = '(BSD)Accum Dep-Leasehold land'
or i.ItemName = '(BSD)Prov for Impairment-Leasehold Land'
or i.ItemName = '(BSD)Land-Rights'
or i.ItemName = '(BSD)Accum Dep-Land Rights'
or i.ItemName = '(BSD)Prov for Impairment-Land Rights'
or i.ItemName = '(BSD)Building'
or i.ItemName = '(BSD)Accum Dep-Building'
or i.ItemName = '(BSD)Prov for Impairment-Building'
or i.ItemName = '(BSD)Construction in Progress'
or i.ItemName = '(BSD)Renovations'
or i.ItemName = '(BSD)Accum Dep-Renovations'
or i.ItemName = '(BSD)Prov for Impairment-Renovations'
or i.ItemName = '(BSD)Investment Property'
or i.ItemName = '(BSD)Accum Dep-Investment Property'
or i.ItemName = '(BSD)Prov for Impairment-Investment Property'
or i.ItemName = '(BSD)Plant'
or i.ItemName = '(BSD)Accum Dep-Plant'
or i.ItemName = '(BSD)Prov for Impairment-Plant'
or i.ItemName = '(BSD)Machinery'
or i.ItemName = '(BSD)Accum Dep-Machinery'
or i.ItemName = '(BSD)Prov for Impairment-Machinery'
or i.ItemName = '(BSD)Equipment'
or i.ItemName = '(BSD)Accum Dep-Equipment'
or i.ItemName = '(BSD)Prov for Impairment-Equipment'
or i.ItemName = '(BSD)Rental Equipment'
or i.ItemName = '(BSD)Accum Dep-Rental Equipment'
or i.ItemName = '(BSD)Prov for Impairment-Rental Equipment'
or i.ItemName = '(BSD)Pipelines'
or i.ItemName = '(BSD)Accum Dep-Pipelines'
or i.ItemName = '(BSD)Prov for Impairment-Pipelines'
or i.ItemName = '(BSD)Storage Tanks'
or i.ItemName = '(BSD)Accum Dep-Storage Tanks'
or i.ItemName = '(BSD)Prov for Impairment-Storage Tanks'
or i.ItemName = '(BSD)ISO Tanks'
or i.ItemName = '(BSD)Accum Dep-ISO Tanks'
or i.ItemName = '(BSD)Prov for Impairment-ISO Tanks'
or i.ItemName = '(BSD)Racks & Cylinders'
or i.ItemName = '(BSD)Accum Dep-Racks & Cylinders'
or i.ItemName = '(BSD)Prov for Impairment–Racks & Cylinders'
or i.ItemName = '(BSD)Tools'
or i.ItemName = '(BSD)Accum Dep-Tools'
or i.ItemName = '(BSD)Prov for Impairment-Tools'
or i.ItemName = '(BSD)Dies & Moulds'
or i.ItemName = '(BSD)Accum Dep-Dies & Moulds'
or i.ItemName = '(BSD)Prov for Impairment-Dies & Moulds'
or i.ItemName = '(BSD)Spare Parts'
or i.ItemName = '(BSD)Accum Dep -Spare Parts'
or i.ItemName = '(BSD)Prov for Impairment-Spare Parts'
or i.ItemName = '(BSD)Furniture & Fittings'
or i.ItemName = '(BSD)Derivatives Liability'
or i.ItemName = '(BSD)Prov for Impairment-F&F'
or i.ItemName = '(BSD)Office Equipment'
or i.ItemName = '(BSD)Accum Dep-Office Equip'
or i.ItemName = '(BSD)Prov for Impairment-Office Equip'
or i.ItemName = '(BSD)Computer'
or i.ItemName = '(BSD)Accum Dep-Computer'
or i.ItemName = '(BSD)Prov for Impairment-Computer'
or i.ItemName = '(BSD)Motor Vehicle'
or i.ItemName = '(BSD)Accum Dep-Motor Vehicle'
or i.ItemName = '(BSD)Prov for Impairment-Motor Vehicle'
or i.ItemName = '(BSD)Fixed Assets Clearing Account'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non Current Assets
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Non Current Assets'
WHERE i.ItemName = '(BSD)Investment in Subsidiary'
or i.ItemName = '(BSD)Prov for Impairment-Subsidiary '
or i.ItemName = '(BSD)Investment in JV Co'
or i.ItemName = '(BSD)Prov for Impairment-JV Co'
or i.ItemName = '(BSD)Investment in Assoc Co'
or i.ItemName = '(BSD)Prov for Impairment-Assoc Co'
or i.ItemName = '(BSD)Investment in Rel Co'
or i.ItemName = '(BSD)Prov for Impairment-Rel Co'
or i.ItemName = '(BSD)Investment in Quoted shares'
or i.ItemName = '(BSD)Prov for Impairment-Quoted Shares'
or i.ItemName = '(BSD)Intangibles-Trade Name'
or i.ItemName = '(BSD)Amort-Trade Name'
or i.ItemName = '(BSD)Intangibles-Customer Relationship'
or i.ItemName = '(BSD)Amort-Customer Relationship'
or i.ItemName = '(BSD)Intangibles-Trademark & Patent'
or i.ItemName = '(BSD)Amort-Trademark & Patent'
or i.ItemName = '(BSD)Intangibles-Product License'
or i.ItemName = '(BSD)Amort-Product License'
or i.ItemName = '(BSD)Intangibles-Club Membership'
or i.ItemName = '(BSD)Amort-Club Membership'
or i.ItemName = '(BSD)Intangibles-Others'
or i.ItemName = '(BSD)Due From Interco - Non Current'
or i.ItemName = '(BSD)Prov for DD-Interco - Non Current'
or i.ItemName = '(BSD)Due From JV/Assoc - Non Current'
or i.ItemName = '(BSD)Prov for DD-JV/Assoc - Non Current'
or i.ItemName = '(BSD)Due From Rel - Non Current'
or i.ItemName = '(BSD)Prov for DD-Rel-Non Current'
or i.ItemName = '(BSD)Due from Shareholders - Non Current'
or i.ItemName = '(BSD)Derivatives Asset - Non Current'
or i.ItemName = '(BSD)Deferred Tax Assets'
or i.ItemName = '(BSD)Goodwill'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Assets
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Assets'
WHERE i.ItemName = '(BSD)Total Current Assets'
or i.ItemName = '(BSD)Total Non Current Assets'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Current Liabilities
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Current Liabilities'
WHERE i.ItemName = '(BSD)Bank Overdraft'
or i.ItemName = '(BSD)Revolving Credit'
or i.ItemName = '(BSD)Factoring/Bills Payable/TR'
or i.ItemName = '(BSD)Short-Term Loan'
or i.ItemName = '(BSD)Term Loan-Current'
or i.ItemName = '(BSD)HP Creditors-Current'
or i.ItemName = '(BSD)HP Int in Suspense'
or i.ItemName = '(BSD)Trade Creditors'
or i.ItemName = '(BSD)Accrued Purchases-External'
or i.ItemName = '(BSD)Accrued Purchases-Interco'
or i.ItemName = '(BSD)Due to Interco-Trade'
or i.ItemName = '(BSD)Due to Interco-Non-Trade'
or i.ItemName = '(BSD)Due to Interco-Loan - Current Li'
or i.ItemName = '(BSD)Accrued Purchases-JV'
or i.ItemName = '(BSD)Accrued Purchases-Assoc'
or i.ItemName = '(BSD)Accrued Purchases-Rel'
or i.ItemName = '(BSD)Due to JV-Trade'
or i.ItemName = '(BSD)Due to JV-Non-Trade'
or i.ItemName = '(BSD)Due to JV-Loan - Current Li'
or i.ItemName = '(BSD)Due to Assoc-Trade'
or i.ItemName = '(BSD)Due to Assoc-Non-Trade'
or i.ItemName = '(BSD)Due to Assoc-Loan - Current Li'
or i.ItemName = '(BSD)Due to Rel Parties-Trade'
or i.ItemName = '(BSD)Due to Rel Parties-Non-Trade'
or i.ItemName = '(BSD)Due to Shareholders - Current Li'
or i.ItemName = '(BSD)Prov for Gain/Loss in FX (Creditors)'
or i.ItemName = '(BSD)Prov for taxation (Current Year)'
or i.ItemName = '(BSD)Prov for taxation (Prior Year)'
or i.ItemName = '(BSD)Accruals-Salary and Leave'
or i.ItemName = '(BSD)Accruals-Social Securities'
or i.ItemName = '(BSD)Accruals-Other HR Exps'
or i.ItemName = '(BSD)Accruals-Staff Claims'
or i.ItemName = '(BSD)Accruals-Transportation'
or i.ItemName = '(BSD)Accruals-Audit Fees'
or i.ItemName = '(BSD)Accruals-Tax Fees'
or i.ItemName = '(BSD)Accruals-Professional Fees'
or i.ItemName = '(BSD)Accruals-Interest Payables'
or i.ItemName = '(BSD)Accruals-Others'
or i.ItemName = '(BSD)Customer Deposit'
or i.ItemName = '(BSD)Customer down payment'
or i.ItemName = '(BSD)Deferred Revenue-Cash Vouchers'
or i.ItemName = '(BSD)Overpayment For Bill Lost'
or i.ItemName = '(BSD)Landed Cost Clearing A/C'
or i.ItemName = '(BSD)Other Creditors-Sundry'
or i.ItemName = '(BSD)Other Creditors-Bill Lost Clear'
or i.ItemName = '(BSD)Other Creditors-W/H Tax'
or i.ItemName = '(BSD)Other Creditors-Others'
or i.ItemName = '(BSD)GST-output'
or i.ItemName = '(BSD)Proposed Dividend'
or i.ItemName = '(BSD)Dividend Payable'
or i.ItemName = '(BSD)Prov for Bonus'
or i.ItemName = '(BSD)Prov for Com & Incentives'
or i.ItemName = '(BSD)Prov for Gratuities'
or i.ItemName = '(BSD)Prov for A&P'
or i.ItemName = '(BSD)Prov for Cylinder Loss'
or i.ItemName = '(BSD)Prov for Plant Maintenance'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Non Current Liabilities
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Non Current Liabilities'
WHERE i.ItemName = '(BSD)Long-Term Loan'
or i.ItemName = '(BSD)HP Creditors'
or i.ItemName = '(BSD)Due to Interco-Loan - Non Current Li'
or i.ItemName = '(BSD)Due to JV-Loan - Non Current Li'
or i.ItemName = '(BSD)Due to Assoc-Loan - Non Current Li'
or i.ItemName = '(BSD)Due to Rel Parties-Loan'
or i.ItemName = '(BSD)Due to Shareholders - Non Current Li'
or i.ItemName = '(BSD)Due to Interbranch - Non Current Li'
or i.ItemName = '(BSD)Deferred Tax Liabilities'
or i.ItemName = '(BSD)Derivatives Liability'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Liabilities
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Liabilities'
WHERE i.ItemName = '(BSD)Total Current Liabilities'
or i.ItemName = '(BSD)Total Non Current Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Equity
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)Total Equity'
WHERE i.ItemName = '(BSD)Share Capital'
or i.ItemName = '(BSD)Calls in Arrears'
or i.ItemName = '(BSD)Treasury Shares'
or i.ItemName = '(BSD)Share Premium'
or i.ItemName = '(BSD)Convertible Loan'
or i.ItemName = '(BSD)Quasi-Equity Loan'
or i.ItemName = '(BSD)Capital Reserve'
or i.ItemName = '(BSD)Asset Revaluation Reserve'
or i.ItemName = '(BSD)Statutory Fund Reserve'
or i.ItemName = '(BSD)Investment Revaluation Reserve'
or i.ItemName = '(BSD)Fair Value Adjustment Reserve'
or i.ItemName = '(BSD)Amalgamation Reserve'
or i.ItemName = '(BSD)General Reserve'
or i.ItemName = '(BSD)ESOS Reserve'
or i.ItemName = '(BSD)Accumulated Profit/(Loss)'
or i.ItemName = '(BSD)Dividend Declared'
or i.ItemName = '(BSD)Minority Interest'
or i.ItemName = '(BSD)Foreign Currency Trans Reserve'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID

--Total Liabilities & Equity
Insert INTO tbFinanceDataSAP
SELECT f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, 
i2.ItemID, SUM(f.MTD), SUM(f.YTD), getdate() 
FROM tbFinanceDataSAP f 
INNER JOIN tbFinanceItem i ON f.ItemID = i.ItemID
INNER JOIN 
(SELECT DISTINCT CoyID, ProjectID, DepartmentID, SectionID, UnitID, tbYear, tbMonth
FROM tbTempTransfer4a)AS t1 
ON f.CoyID = t1.CoyID and f.ProjectID = t1.ProjectID and f.DepartmentID = t1.DepartmentID 
and f.SectionID = t1.SectionID and f.UnitID = t1.UnitID and f.tbYear = t1.tbYear and f.tbMonth = t1.tbMonth
INNER JOIN tbFinanceItem i2 ON i2.ItemName = '(BSD)TOTAL LIAB & EQUITY'
WHERE i.ItemName = '(BSD)Total Equity'
or i.ItemName = '(BSD)Total Liabilities'
GROUP BY f.CoyID, f.ProjectID, f.DepartmentID, f.SectionID, f.UnitID, f.tbYear, f.tbMonth, i2.ItemID





