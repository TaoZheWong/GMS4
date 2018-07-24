using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore
{
    public class SAPPurchaseOrder_Detail
    {
        public string GMSDocumentNumber { get; set; }
        public string GMSLineNumber { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ItemCode { get; set; }
        public string Warehouse { get; set; }
        public string TaxCode { get; set; }
        public string LineDiscountPercent { get; set; }
        public string DimensionL1 { get; set; }
        public string DimensionL2 { get; set; }
        public string DimensionL3 { get; set; }
        public string DimensionL4 { get; set; }
        public string UOMQuantity { get; set; }
        public string UOMPrice { get; set; }
        public string UOMSales { get; set; }
        public string Requestor { get; set; }
        public string Approver1 { get; set; }
        public string Approver2 { get; set; }
        public string Approver3 { get; set; }
        public string Approver4 { get; set; }
        public string GMSMRNo { get; set; }
        public string GMSProject { get; set; }
        public string ItemName { get; set; }
    }
}
