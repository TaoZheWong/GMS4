
CREATE TABLE [dbo].[tbEntertainmentClaim](
	[ClaimID] [int] IDENTITY(1,1) NOT NULL,
	[ClaimNo] [nvarchar](50) NULL,
	[CoyID] [smallint] NOT NULL,
	[CreatedBy] [smallint] NOT NULL,
	[ModifiedBy] [smallint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Status] [smallint] NOT NULL,
	[dim1] [smallint] NULL,
	[dim2] [smallint] NULL,
	[dim3] [smallint] NULL,
	[dim4] [smallint] NULL,
	[Cust1] [nvarchar](max) NULL,
	[Person1] [nvarchar](200) NULL,
	[Desig1] [nvarchar](200) NULL,
	[Cust2] [nvarchar](max) NULL,
	[Phone1] [nvarchar](20) NULL,
	[Person2] [nvarchar](200) NULL,
	[Desig2] [nvarchar](200) NULL,
	[Phone2] [nvarchar](20) NULL,
	[Cust3] [nvarchar](max) NULL,
	[Person3] [nvarchar](200) NULL,
	[Desig3] [nvarchar](200) NULL,
	[Phone3] [nvarchar](20) NULL,
	[Description] [nvarchar](max) NULL,
	[ClaimDate] [datetime] NULL,
 CONSTRAINT [PK_tbEntertainmentClaim] PRIMARY KEY CLUSTERED 
(
	[ClaimID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbEntertainmentClaim] ADD  CONSTRAINT [DF_tbEntertainmentClaim_Status]  DEFAULT ((0)) FOR [Status]
GO


