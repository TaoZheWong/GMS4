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
    public partial class CalendarControl : System.Web.UI.UserControl
    {
        #region Property: IsValueRequired
        /// <summary>
        /// Bool value to determine if the Calendar date is required.
        /// </summary>
        public bool IsValueRequired
        {
            get
            {
                return this.calCalendarCtrl.RequiredDate;
            }
            set
            {
                this.calCalendarCtrl.RequiredDate = value;
            }
        }
        #endregion

        #region Property: Enabled
        /// <summary>
        /// Bool value to determine if the Calendar date is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.calCalendarCtrl.Enabled;
            }
            set
            {
                this.calCalendarCtrl.Enabled = value;
                this.txtCalendarField.Enabled = value;
            }
        }
        #endregion

        #region Property: SelectedDate
        /// <summary>
        /// DateTime value of the Selected Date
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                if (this.calCalendarCtrl.DateValue.Ticks == DateTime.MinValue.Ticks)
                    return GMSCore.GMSCoreBase.DEFAULT_NO_DATE;
                return this.calCalendarCtrl.DateValue;
            }
            set
            {
                if (value.Ticks == GMSCore.GMSCoreBase.DEFAULT_NO_DATE.Ticks)
                {
                    this.calCalendarCtrl.DateValue = DateTime.MinValue;
                    this.txtCalendarField.Text = "";
                }
                else
                    this.calCalendarCtrl.DateValue = value;
            }
        }
        #endregion

        #region Property: InvalidDateMessage
        /// <summary>
        /// Property for Invalid Date Message
        /// </summary>
        public string InvalidDateMessage
        {
            get
            {
                return this.calCalendarCtrl.TextMessage;
            }
            set
            {
                this.calCalendarCtrl.TextMessage = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //this.calCalendarCtrl.AddHoliday( 1 , 1 , 0 , "New Year's Day" );
                //this.calCalendarCtrl.AddHoliday( 1 , 5 , 0 , "Labour Day" );
                //this.calCalendarCtrl.AddHoliday( 9 , 8 , 0 , "National Day" );
                //this.calCalendarCtrl.AddHoliday( 25 , 12 , 0 , "Christmas Day" );
            }
        }
    }
}