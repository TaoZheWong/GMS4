DELETE FROM tbFinanceItemSeq where Report = 'PNLS'

INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',1,1,1081)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',2,2,1082)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',3,3,1083)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',4,4,1084)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',5,5,1085)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',6,6,1086)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',7,7,1087)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',8,8,1088)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',9,9,1126)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',10,10,1089)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',11,11,1090)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',12,12,1091)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',13,13,(SELECT TOP 1 ItemID FROM tbFinanceItem where ItemName = '(PNL)Selling & Distribution'))

INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',14,14,1112)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',15,15,1118)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',16,16,1120)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',17,17,1121)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',18,18,1122)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',19,19,(SELECT ItemID FROM tbFinanceItem where ItemName = '(PNL)Derivatives Gain/(Loss)'))
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',20,20,(SELECT ItemID FROM tbFinanceItem where ItemName = '(PNL)Forex Gain/(Loss)'))


INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',21,21,1125)

INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',22,22,1127)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',23,23,1128)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',24,24,1129)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',25,25,1130)

INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',26,26,1131)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',27,27,1474)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',28,0,46)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',29,1,1135)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',30,2,1136)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',31,3,1137)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',32,4,50)

INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',33,5,1139)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',34,6,53)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',35,7,52)
INSERT INTO tbFinanceItemSeq(Report,ItemSeqID,ItemSN,ItemID) VALUES('PNLS',36,8,1140)
