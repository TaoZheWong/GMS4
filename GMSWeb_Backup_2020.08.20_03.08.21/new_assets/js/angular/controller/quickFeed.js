app.controller('QuickNewsFeedController', function (newsFeedFactory,$q) {
    var quickfeed = this;
    quickfeed.list = [];
    //quickfeed.source = [];

    var timeoutCanceler = $q.defer();

    quickfeed.loadList = function () {

        newsFeedFactory.get(globalCoyID, globalUserID, "UNREAD", timeoutCanceler).then(function (resp) {
            //console.log(resp.data.Params.data)
            quickfeed.source = resp.data.Params.data;
            var importantFeedCount = 0;
            
            if (quickfeed.source.length > 0) {
                quickfeed.source.forEach(function (feed, index) {
                    console.log(feed);
                    quickfeed.list.push(feed);

                    if(feed.Type == "system_update")
                        importantFeedCount++

                });
                
                if (importantFeedCount>0)
                    $.notification({ title: "Important New (" + importantFeedCount + ")", content: "View for more details", icon: "ti-announcement", iconClass: "bg-gradient-red", btn: true, btnText: "View",btnUrl:Path + "/News/Feeds.aspx" });
            }
        });
    }

    quickfeed.view = function (data) {
        window.location = Path + "/News/Feeds.aspx#!?feedId=" + data.ID;
    }

    if(window.location.pathname.indexOf("Feeds.aspx")<0)
        quickfeed.loadList();


    $(window).bind('beforeunload', function () {
        timeoutCanceler.resolve();
    });
});