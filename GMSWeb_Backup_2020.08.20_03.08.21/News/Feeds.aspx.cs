using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using System.Web.Services;

namespace GMSWeb.News
{
    public partial class Feeds : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            LogSession session = GetSessionInfo();

            session.UserId.ToString();
            if (session.UserId == 1) {
                createForm.Visible = true;
                editForm.Visible = true;
            }
                
        }

        public class ResponseModel
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public IDictionary<string, object> Params { get; set; }

            public ResponseModel()
            {
                Status = 0; // 0=success; 1=error
                Message = "Success";
            }

            public ResponseModel(int status, string message)
            {
                Status = status;
                Message = message;
            }
        }

        [WebMethod]
        public static ResponseModel GetFeeds(short CompanyID, short UserID, string Status)
        {
            var m = new ResponseModel();

            try
            {
                DataSet dsTemp = new DataSet();
                new GMSGeneralDALC().GetNewFeeds(CompanyID, UserID, Status, ref dsTemp);
                m.Params = new Dictionary<string, object> { { "data" , GMSUtil.ToJson(dsTemp, 0) } };
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel ReadFeed(short FeedID, short UserID)
        {
            var m = new ResponseModel();

            try
            {
                new GMSGeneralDALC().ReadNewFeed(FeedID, UserID);
                m.Message = "Successfully Update";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel SetFeed(short CoyID, short UserID, string Title, string Desc, string Content)
        {
            var m = new ResponseModel();

            try
            {
                new GMSGeneralDALC().InsertNewFeed(CoyID, UserID, Title, Desc, Content);
                m.Message = "Successfully Insert";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }

        [WebMethod]
        public static ResponseModel UpdateFeed(short ID, short CoyID, short UserID, string Title, string Desc, string Content)
        {
            var m = new ResponseModel();

            try
            {
                new GMSGeneralDALC().UpdateNewFeed(ID,CoyID, UserID, Title, Desc, Content);
                m.Message = "Successfully Insert";
            }
            catch (Exception e)
            {
                m.Status = 1;
                m.Message = e.Message;

            }

            return m;
        }




    }
}