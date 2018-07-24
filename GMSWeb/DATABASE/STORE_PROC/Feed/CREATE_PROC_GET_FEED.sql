USE [GMS]
GO
/****** Object:  StoredProcedure [dbo].[procAppGetFeed]    Script Date: 11/25/2017 00:23:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[procAppGetFeed]
@CoyID smallint,
@UserID smallint,
@Status nvarchar(max)
AS

	
SET NOCOUNT ON;


IF (@Status = 'UNREAD')
	BEGIN
		SELECT TOP 5 tf.ID,tf.Coy_ID,tf.Type,tf.Title,au.UserRealName As Created_By,tf.Created_At,tf.Feed_Description,tf.Feed_Content, fu.Created_At As Read_At
		FROM [dbo].[tbFeed] tf
		INNER JOIN [dbo].[aspnet_Users] au on tf.Created_By = au.UserNumId
		LEFT JOIN [dbo].[tbFeed_User] fu on fu.FeedID = tf.ID and fu.UserID = @UserID
		WHERE (tf.Coy_ID = @CoyID OR tf.Coy_ID = -1)
		AND fu.Created_At is null
		ORDER BY tf.Created_At desc
	END

ELSE 
	BEGIN
		SELECT tf.ID,tf.Coy_ID,tf.Type,tf.Title,au.UserRealName As Created_By,tf.Created_At,tf.Feed_Description,tf.Feed_Content, fu.Created_At As Read_At
		FROM [dbo].[tbFeed] tf
		INNER JOIN [dbo].[aspnet_Users] au on tf.Created_By = au.UserNumId
		LEFT JOIN [dbo].[tbFeed_User] fu on fu.FeedID = tf.ID and fu.UserID = @UserID
		WHERE tf.Coy_ID = @CoyID OR tf.Coy_ID = -1
		ORDER BY tf.Created_At desc
	END


