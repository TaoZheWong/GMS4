<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" CodeBehind="Feeds.aspx.cs" Inherits="GMSWeb.News.Feeds" Title="New Feeds" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <!--
    <div class="row">
        <div class="col-md-3">
            <div class="panel panel-default">
				<div class="panel-heading">
					<h4 class="panel-title">Feeds</h4>
				</div>
				<div class="panel-body">
                    <div class="list-group m-b-0">
				        <a href="#" class="list-group-item active">Active Link Item</a>
				        <a href="#" class="list-group-item">Normal Link Item</a>
				        <a href="#" class="list-group-item"><span class="badge pull-right">New</span>Item with Badge</a>
			        </div>
                </div>
            </div>
        </div>
        <div class="col-md-8">
            <div class="panel panel-default">
				<div class="panel-heading">
					<h4 class="panel-title">MARKED TEXT</h4>
				</div>
				<div class="panel-body">
					<p class="desc">For highlighting a run of text due to its relevance in another context, use the <code>&lt;mark&gt;</code> tag.</p>
					<p>
						You can use the mark tag to <mark>highlight</mark> text.
					</p>
				</div>
			</div>
        </div>
       
    </div>
    -->

<div class=""  data-ng-controller="NewsFeedController as newsFeedList" data-ng-cloak="">
    <div id="feedForm" class="modal fade" role="dialog">
        <div class="modal-dialog modal-full">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" data-ng-show="newsFeedList.selectedFeed == ''">New Feed</h4>
                    <h4 class="modal-title" data-ng-show="newsFeedList.selectedFeed != ''">Update Feed</h4>
                    <button type="button" class="close" data-dismiss="modal"><span>×</span></button>
                </div>
                <div class="modal-body" ng-form name="theForm">
                        <div class="form-group">
					        <label class="control-label" for="feed_title">Title</label>
					        <input type="text" class="form-control" id="feed_title" name="feed_title" data-ng-model="newsFeedList.feed.Title" placeholder="Title" required />
				            <span data-ng-show="theForm.feed_title.$invalid" class="help-block text-danger">This is required.</span>
                        </div>
                        <div class="form-group">
					        <label class="control-label" for="feed_description">Description</label>
					        <input type="text" class="form-control" id="feed_description" name="feed_description" data-ng-model="newsFeedList.feed.Desc" placeholder="Description" required />
				            <span data-ng-show="theForm.feed_description.$invalid" class="help-block text-danger">This is required.</span>
                        </div>
                        <div class="form-group" data-ng-show="newsFeedList.selectedFeed == ''">
					        <label class="control-label" for="feed_company">Company</label>
                            <select class="form-control" data-ng-model="newsFeedList.feed.CoyID" name="feed_company" required>
                                <option value="-1">All</option>
					        </select>
                            <span data-ng-show="theForm.feed_company.$invalid" class="help-block text-danger">This is required.</span>
				        </div>
                        <div class="form-group">
					        <label class="control-label" for="feed_content">Content</label>
					        <div id="feed_content" data-ng-model="newsFeedList.feed.content" name="feed_content"></div>
				        </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" data-ng-click="newsFeedList.newFeed()" data-ng-show="newsFeedList.selectedFeed == ''">Save changes</button>
                    <button type="button" class="btn btn-primary" data-ng-click="newsFeedList.updateFeed()" data-ng-show="newsFeedList.selectedFeed != ''">Update changes</button>
                </div>
            </div>
        </div>
    </div>

  

    <!-- BEGIN mail-box-->
    <div class="mail-box">
        <div class="mail-box-content" style="top:69px;left:0;border-top:1px solid #d1d1d4" data-ng-cloak="" >
            <div class="mail-box-toolbar">
                <div class="pull-left">
                    <a href="#" data-ng-show="newsFeedList.page == 'detail'" data-ng-click="newsFeedList.goBack()" class="btn btn-default btn-sm"><i class="ti-arrow-left"></i></a>
                    <LinkButton id="editForm" runat="server" visible="false" data-ng-click="newsFeedList.editForm()" class="btn btn-default btn-sm" data-ng-show="newsFeedList.selectedFeed != '' "><i class="ti-pencil"></i></LinkButton>
                    <LinkButton id="createForm" runat="server" visible="false" href="#feedForm" data-ng-click="newsFeedList.openForm()" class="btn btn-default btn-sm" data-ng-show="newsFeedList.selectedFeed == '' "><i class="ti-plus"></i></LinkButton>
                </div>
            </div>
            <div class="mail-box-container">
                <div data-scrollbar="true" data-height="100%">
                    <ul class="email-list" data-ng-show="newsFeedList.page == 'main'">
                        <li data-ng-class="{'has-important': feed.Type=='system_update' , 'unread':feed.Read_At==''}" data-ng-repeat="feed in newsFeedList.feedList" data-ng-click="newsFeedList.viewDetail(feed)">
                            <div class="email-message">
                                <a>
                                    <div class="email-sender">
                                        <span class="email-time">{{feed.Created_At}}</span>
                                        {{feed.Created_By}}
                                    </div>
                                    <div class="email-title">{{feed.Title}}</div>
                                    <div class="email-desc">{{feed.Feed_Description}}</div>
                                </a>
                            </div>
                        </li>
                    </ul>
                    <div class="mail-detail" data-ng-show="newsFeedList.page == 'detail'">
                        <div class="email-detail-header">
							<h4 class="email-subject">{{newsFeedList.detail.Created_By}}</h4>
                            <div class="email-sender-info" style="margin-left:0;">
								<h4 class="title">{{newsFeedList.detail.Title}}</h4>
								<div class="time">{{newsFeedList.detail.Created_At}}</div>
							</div>
						</div>
                        <div class="email-detail-content">
							<div class="email-detail-body" data-ng-bind-html="newsFeedList.detail.Feed_Content">
							</div>
						</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
     <script type="text/javascript" src="Feeds.js?now=<%=DateTime.Now.Millisecond %>"></script>
</asp:Content>
