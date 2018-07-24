UPDATE tbFinanceItem SET ItemName = '(PNL)Commission/Mgt Income' WHERE ItemName = '(PNL)Commission Income/Mgt Income'
UPDATE tbFinanceItem SET ItemName = '(PNL)HO/Interco and Other S&D' WHERE ItemName = '(PNL)Other S&D Expenses'
UPDATE tbFinanceItem SET ItemName = '(PNL)A&G Entertainment & Donations' WHERE ItemName = '(PNL)A&G Entertainment'
UPDATE tbFinanceItem SET ItemName = '(PNL)HO/Interco Alloc' WHERE ItemName = '(PNL)HO / Interco Allocation'
UPDATE tbFinanceItem SET ItemName = '(PNL)Dividend Declared' WHERE ItemName = '(PNL)Dividend Expenses'
UPDATE tbFinanceItem SET ItemName = '(PNL)Other A&G Expenses' WHERE ItemName = '(PNL)A&G Other Expenses'
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNL)Derivatives Gain/(Loss)');
INSERT INTO tbFinanceItem(ItemName) VALUES('(PNL)Forex Gain/(Loss)');
