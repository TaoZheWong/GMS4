insert into tbReportCategory(Name,SeqID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) Values('Finance After 2016',1.1,null,null,null,null)

update tbReport set ReportCategoryID = (select ReportCategoryID from tbReportCategory where Name = 'Finance After 2016')
where ReportID IN (select ReportID from tbReport where Description like '%2016%' And ReportCategoryID = 1 )