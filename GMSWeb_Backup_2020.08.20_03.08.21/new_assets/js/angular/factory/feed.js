app.factory('newsFeedFactory', function ($http) {
    return {
        set: function (FeedID, UserID) {
            return $http.post(Path + '/News/Feeds.aspx/ReadFeed', {
                'FeedID': FeedID,
                'UserID': UserID
            });
        },
        insert: function (feed) {
            return $http.post(Path + '/News/Feeds.aspx/SetFeed', {
                'CoyID': feed.CoyID,
                'UserID': feed.UserID,
                'Title': feed.Title,
                'Desc': feed.Desc,
                'Content': feed.Content,
            });
        },
        update: function (feed) {
            return $http.post(Path + '/News/Feeds.aspx/UpdateFeed', {
                'ID':feed.ID,
                'CoyID': feed.CoyID,
                'UserID': feed.UserID,
                'Title': feed.Title,
                'Desc': feed.Desc,
                'Content': feed.Content,
            });
        },
        get: function (CompanyID, UserID, status, timeoutCanceler) {
            var action = $http({
                url: Path + '/News/Feeds.aspx/GetFeeds',
                data: {
                    'CompanyID': CompanyID,
                    'UserID': UserID,
                    'Status': status
                },
                timeout: timeoutCanceler.promise,
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action
        }
    };
});