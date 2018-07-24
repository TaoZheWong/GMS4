using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore.Entity
{
    public class MRItem
    {
        string _mrNo;
        Nullable<DateTime> _mrDate;
        string _projectNo;
        string _vendorName;
        string _requestor;
        string _purchaser;
        string _purchaseCurrency;
        decimal _purchasePrice;
        string _status;
        string _remarks;              

        public MRItem()
        {}

        public MRItem(string mrNo, string projectNo)
        {
            this._mrNo = mrNo;
            this._projectNo = projectNo;
        }

        public string mrNo
        {
            get { return _mrNo; }
            set { _mrNo = value; }
        }

        public Nullable<DateTime> MRDate
        {
            get { return _mrDate; }
            set { _mrDate = value; }
        }

        public string vendorName
        {
            get { return _vendorName; }
            set { _vendorName = value; }
        }

        public string requestor
        {
            get { return _requestor; }
            set { _requestor = value; }
        }

        public string purchaser
        {
            get { return _purchaser; }
            set { _purchaser = value; }
        }

        public string purchaseCurrency
        {
            get { return _purchaseCurrency; }
            set { _purchaseCurrency = value; }
        }

        public string status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        public decimal purchasePrice
        {
            get { return _purchasePrice; }
            set { _purchasePrice = value; }
        }       
    }
}
