--INSERT AND UPDATE FINANCE ITEM
--F21
UPDATE tbFinanceItem SET ItemName = '(PNL)Commission/Mgt Income' WHERE ItemName = '(PNL)Commission Income/Mgt Income'
UPDATE tbFinanceItem SET ItemName = '(PNL)HO/Interco and Other S&D' WHERE ItemName = '(PNL)Other S&D Expenses'
UPDATE tbFinanceItem SET ItemName = '(PNL)A&G Entertainment & Donations' WHERE ItemName = '(PNL)A&G Entertainment'
UPDATE tbFinanceItem SET ItemName = '(PNL)HO/Interco Alloc' WHERE ItemName = '(PNL)HO / Interco Allocation'
UPDATE tbFinanceItem SET ItemName = '(PNL)Dividend Declared' WHERE ItemName = '(PNL)Dividend Expenses'
UPDATE tbFinanceItem SET ItemName = '(PNL)Other A&G Expenses' WHERE ItemName = '(PNL)A&G Other Expenses'
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNL)Derivatives Gain/(Loss)');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNL)Forex Gain/(Loss)');

--TODO 
--F21R


--F22
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNL)Selling & Distribution');


--F30
UPDATE tbFinanceItem SET ItemName = '(BS)Less:Accumulated Depreciation/Impairment' WHERE ItemName = '(BS)Less:Accumulated Depreciation'
UPDATE tbFinanceItem SET ItemName = 'Provision for Doubtful Debts' WHERE ItemName = 'Prov for Doubtful Debts'
--Exist but never use id is (228) INSERT INTO tbFinanceItem(ItemName) VALUES('Creditors Turnover (Days)');

--PNLD F31
UPDATE tbFinanceItem SET ItemName = '(PNLD)Property Rental Income' WHERE ItemName = '(PNLD)Property Rental';
UPDATE tbFinanceItem SET ItemName = '(PNLD)Proft From Operations' WHERE ItemName = '(PNLD)Total Operating Profit';
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Gain on Disposal of Investment');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Gain on Asset Held for Sales');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Loss on Disposal of Investment');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Loss on Asset Held for Sales');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)S&D Upkeep-Machinery');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)S&D Upkeep-Equipment');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)S&D Upkeep-Furniture & Fitting');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)S&D Upkeep-Office Equip');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)S&D Upkeep-Computers');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)A&G Travelling/Transportation');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)A&G Entertainment/Donations');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)A&G Upkeep-Equipment');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)A&G Upkeep-Office Equip');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Interest (Expense)');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Taxation');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Share of Assoc/JV’s P&L Credit');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Share of Assoc/JV’s P&L Debit');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNLD)Insurance Claim');

--MAD F32
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Direct Exps-Others')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)IL-QA Allocated Cost')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)IL-PM Allocated Cost')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)VO-QA Allocated Cost')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)VO-PM Allocated Cost')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Other Variable Overhead')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)R&M-Factory')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Upkeep-Plant')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Upkeep-Machinery')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Upkeep-Equipment')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Upkeep-Racks & Cylinders')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Upkeep-Furniture & Fitting')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Upkeep-Office Equip')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)Upkeep-Computers')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)QA Allocated Cost')
INSERT INTO tbFinanceItem(ItemName) VALUES('(MAD)PM Allocated Cost')

--BSD F33
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Stocks Return')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)COGS Clearing Account')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Due from Interco-Non-Trade Clearing')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Due from Assoc-Non-Trade Clearing')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Due from Rel-Non-Trade Clearing')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Suspense-HR')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Freehold Land')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Leasehold Land')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Land Rights')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Building')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Renovations')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Investment Property')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Machinery')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Equipment')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Rental Equipment')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Pipelines')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Storage Tanks')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-ISO Tanks')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Tools')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Dies & Moulds')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Spare Parts')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-F&F')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Office Equip')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Computer')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Motor Vehicle')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Fixed Assets Clearing Account')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Subsidiary ')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-JV Co')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Assoc Co')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Rel Co')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for Impairment-Quoted Shares')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Amort-Trade Name')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Amort-Customer Relationship')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Amort-Trademark & Patent')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Amort-Product License')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Amort-Club Membership')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for DD-Interco - Non Current')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for DD-JV/Assoc - Non Current')
UPDATE tbFinanceItem SET ItemName = '(BSD)Due From Rel - Non Current' WHERE ItemName = '(BSD)Due From Rel Parties - Non Current';
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Prov for DD-Rel-Non Current')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Derivatives Asset - Non Current')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)HP Int in Suspense')
UPDATE tbFinanceItem SET ItemName = '(BSD)Trade Creditors' WHERE ItemName = '(BSD)Creditors';
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Overpayment For Bill Lost')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Accum Dep-F&F')
INSERT INTO tbFinanceItem(ItemName) VALUES('(BSD)Dividend Declared')






