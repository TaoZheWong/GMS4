using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic; 
using System.Xml.Serialization;
using GMSCore.Entity;
using GMSCore; 

namespace GMSWeb.Common
{
    /// <summary>
    /// Summary description for GMSQuotation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class GMSQuotation : System.Web.Services.WebService
    {
        [WebMethod]
        [XmlInclude(typeof(Quotation))]
        public Quotation GetQuotation(short coyID, string quotationNo)
        {
            QuotationHeader qh = QuotationHeader.RetrieveByKey(coyID, quotationNo, 0);
            if (qh != null)
            {
                Quotation quotation = new Quotation(qh.QuotationNo);
                quotation.accountCode = qh.AccountCode;
                quotation.address1 = qh.Address1;
                quotation.address2 = qh.Address2;
                quotation.address3 = qh.Address3;
                quotation.address4 = qh.Address4;
                quotation.customerPONo = qh.CustomerPONo;                
                quotation.internalRemarks = qh.InternalRemarks;
                quotation.externalRemarks = qh.ExternalRemarks;
                quotation.contactPerson = qh.AttentionTo;
                quotation.mobilePhone = qh.MobilePhone;
                quotation.officePhone = qh.OfficePhone;
                quotation.fax = qh.Fax;
                quotation.salesPersonID = qh.SalesPersonID;
                quotation.currencyCode = qh.Currency;
                decimal currencyRate;
                Decimal.TryParse(qh.CurrencyRate.ToString(), out currencyRate);  
                quotation.currencyRate = currencyRate; 
                quotation.taxType = qh.TaxTypeID;
                decimal taxRate;
                Decimal.TryParse(qh.TaxRate.ToString(), out taxRate);
                quotation.taxRate = taxRate; 
                quotation.subTotal = (decimal) qh.SubTotal;
                quotation.grandTotal = (decimal) qh.GrandTotal;
                quotation.contactPerson = qh.ContactPerson;
                quotation.orderedBy = qh.OrderedBy;
                quotation.CustomerSO = qh.CustomerSO;
                quotation.TransportZone = qh.TransportZone;                
                if (qh.RequiredDate == null)
                    quotation.RequiredDate = null; 
                else
                    quotation.RequiredDate = Convert.ToDateTime(qh.RequiredDate); 
                if(qh.IsSelfCollect.ToString() == "True")
                    quotation.IsSelfCollect = true;
                else
                    quotation.IsSelfCollect = false;
                quotation.AddressCode = qh.AddressCode;
                quotation.AddressID = (short)qh.AddressID;
                quotation.StatusID = qh.QuotationStatusID;

                if (qh.IsOutright.ToString() == "True")
                    quotation.IsOutright = true;
                else
                    quotation.IsOutright = false;

                if (qh.IsCOP.ToString() == "True")
                    quotation.IsCOP = true;
                else
                    quotation.IsCOP = false;

                if (qh.ConvertLabFile.ToString() == "True")
                    quotation.ConvertLabFile = true;
                else
                    quotation.ConvertLabFile = false;

                return quotation;
            }
            else
                return null;
        }

        [WebMethod]
        [XmlInclude(typeof(List<QuotationItem>))]
        public List<QuotationItem> GetQuotationItems(short coyID, string quotationNo)
        {
            List<QuotationItem> qil = new List<QuotationItem>();
            DataSet dsTemp = new DataSet();
            (new QuotationDataDALC()).GetQuotationDetailByQuotationNoSelect(coyID, quotationNo, 1, 0, ref dsTemp);
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTemp.Tables[0].Rows)
                {
                    QuotationItem qi = new QuotationItem();
                    qi.quotationNo = dr["QuotationNo"].ToString();
                    qi.prodCode = dr["ProductCode"].ToString();
                    qi.prodName = dr["ProductDescription"].ToString();
                    qi.qty = Convert.ToDecimal(dr["Quantity"]);
                    qi.unitPrice = Convert.ToDecimal(dr["UnitPrice"]);
                    qi.prodNotes = "";
                    qi.recipeNo = dr["RecipeNo"].ToString();
                    qi.batchSize = Convert.ToInt32(dr["BatchSize"].ToString());
                    qi.tagNo = dr["TagNo"].ToString();
                    qil.Add(qi); 
                }
                return qil; 
            }
            else
                return null; 
        }
    }
}
