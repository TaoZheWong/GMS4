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

namespace GMSWeb.CustomCtrl
{
    public partial class MessagePanelControl : System.Web.UI.UserControl
    {
        public enum MessageEnumType
        {
            Info = 1,
            Alert = 2,
            Notice = 3
        }

        private MessageEnumType _msgType = MessageEnumType.Info;
        private string _message = "";

        public MessageEnumType MessageType
        {
            get { return this._msgType; }
            set
            {
                this._msgType = value;
                switch (this._msgType)
                {
                    case MessageEnumType.Alert:
                        this.pnlMessagePanel.CssClass = "R_MsgBox";
                        break;
                    case MessageEnumType.Notice:
                        this.pnlMessagePanel.CssClass = "B_MsgBox";
                        break;
                    case MessageEnumType.Info:
                    default:
                        this.pnlMessagePanel.CssClass = "P_MsgBox";
                        break;
                }
            }
        }

        public string Message
        {
            get { return this._message; }
            set
            {
                this._message = value;
                this.lblMessagePanelMsg.Text = this._message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Function to toggle the Message Panel
        /// </summary>
        /// <param name="message">string value of the message</param>
        /// <param name="msgType">MessageEnumType of the type</param>
        public void ShowMessage(string message, MessageEnumType msgType)
        {
            this.Message = message;
            this.MessageType = msgType;
            this.pnlMessagePanel.Visible = true;
        }
    }
}