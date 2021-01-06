/*
Decependency

Global Variable
===============
app = located at sitemaster , app for angular js module
globalCoyID = located at sitemaster , company id from session
globalUserID = located at sitemaster , user id from session
*/
app.controller('NewsFeedController', function ($location, $scope, $q, newsFeedFactory) {
    var newsFeedList = this;
    var timeoutCanceler = $q.defer();

    newsFeedList.feedId = $location.$$search.feedId;
    newsFeedList.feedList = [];
    newsFeedList.origin = {
        CoyID: "",
        UserID: globalUserID,
        Title: "",
        Desc:"",
        Content:""
    };
    newsFeedList.feed = angular.copy(newsFeedList.origin);
    newsFeedList.selectedFeed = "";

    newsFeedList.openForm = function () {
        newsFeedList.feed = angular.copy(newsFeedList.origin);
        $('#feed_content').summernote('code','');
        $("#feedForm").modal("show");
    }

    newsFeedList.editForm = function () {
        var feed = angular.copy(_.find(newsFeedList.feedList, function (o) {
            return o.ID == newsFeedList.selectedFeed;
        }));

        newsFeedList.feed = {
            ID:feed.ID,
            CoyID: feed.Coy_ID,
            UserID: globalUserID,
            Title: feed.Title,
            Desc: feed.Feed_Description,
            Content: feed.Feed_Content
        }

        $('#feed_content').summernote('code', newsFeedList.feed.Content);

        $("#feedForm").modal("show");
    }

    newsFeedList.newFeed = function () {
        if ($scope.theForm.$invalid) {
            alert("Please complete the form ");
            return false;
        }

        newsFeedList.feed.Content = $('#feed_content').summernote('code');
        
        newsFeedFactory.insert(newsFeedList.feed).then(function (resp) {
            if (resp.data.Status == 1) {
                alert("Error . " + resp.data.Message);
                return false;
            }

            $("#feedForm").modal("hide");
            newsFeedList.gotoMain();

        });
       
    }

    newsFeedList.updateFeed = function () {
        if ($scope.theForm.$invalid) {
            alert("Please complete the form ");
            return false;
        }

        newsFeedList.feed.Content = $('#feed_content').summernote('code');
        
        newsFeedFactory.update(newsFeedList.feed).then(function (resp) {
            if (resp.data.Status == 1) {
                alert("Error . " + resp.data.Message);
                return false;
            }

            $("#feedForm").modal("hide");
           
            newsFeedList.gotoMain();

        });
    }

    newsFeedList.loadFeeds = function (CompanyID, UserID) {

        newsFeedList.feedList = [];
        newsFeedFactory.get(CompanyID, UserID, "ALL", timeoutCanceler).then(function (resp) {
            newsFeedList.feedList = resp.data.Params.data;
        });
    }

    newsFeedList.SetReadNew = function (the_new) {
        newsFeedFactory.set(the_new.ID, globalUserID);
    }

    newsFeedList.goBack = function () {
        newsFeedList.page = "main";
        newsFeedList.selectedFeed = "";
        newsFeedList.loadFeeds(globalCoyID, globalUserID);
    }

    newsFeedList.viewDetail = function (data) {
        newsFeedList.detail = data;
        newsFeedList.SetReadNew(data);
        newsFeedList.page = "detail";
        newsFeedList.selectedFeed = data.ID;
    }

    newsFeedList.viewDetailById = function (id) {
        newsFeedList.viewDetail(newsFeedList.detail = _.find(newsFeedList.feedList, function (o) {
            return o.ID == id;
        }));
    }

    newsFeedList.gotoMain = function(){
        newsFeedList.loadFeeds(globalCoyID, globalUserID);
        newsFeedList.page = "main";
    }

    //Page Load Handling
    if (newsFeedList.feedId) {
        newsFeedFactory.get(globalCoyID, globalUserID, "ALL", timeoutCanceler).then(function (resp) {
            newsFeedList.feedList = resp.data.Params.data;
            newsFeedList.viewDetailById(newsFeedList.feedId);
        });
    } else
        newsFeedList.gotoMain();

    

});

$(document).ready(function () {
    $("#footer").hide();
    $("#notification_btn").hide();
    $('#feed_content').summernote({
        height: 200,
    });
});