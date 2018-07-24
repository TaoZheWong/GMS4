USE [GMS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[procAppReadFeed]
@FeedID smallint,
@UserID smallint
AS

	
SET NOCOUNT ON;


IF NOT EXISTS(SELECT 1 FROM tbfeed_user WHERE FeedID = @FeedID AND UserID = @UserID)
	BEGIN
		INSERT INTO tbfeed_user(FeedID, UserID, Created_At)VALUES(@FeedID,@UserID,GETDATE());
	END






