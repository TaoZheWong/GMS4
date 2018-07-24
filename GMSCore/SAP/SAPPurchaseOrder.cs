using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore
{
    public class SAPPurchaseOrder
    {
        public string GMSDocumentNumber { get; set; }
        public string PostingDate { get; set; }
        public string Vendor { get; set; }
        public string DocumentCurency { get; set; }
        public string DocumentRate { get; set; }
        public string CreatedBy { get; set; }
        public string DocumentDiscount { get; set; }
        public string Remarks { get; set; }
        public string ContactPerson { get; set; }
        public string GMSCoyID { get; set; }
        public string DeliveryMode { get; set; }
        public string Purchaser { get; set; }
        public virtual List<SAPPurchaseOrder_Detail> SAPPurchaseOrder_Detail { get; set; }
    }
}
