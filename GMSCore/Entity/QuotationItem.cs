using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore.Entity
{
    public class QuotationItem
    {
        string _quotationNo;
        string _prodCode;
        string _prodName;
        decimal _qty;
        decimal _unitPrice;
        string _prodNotes;
        string _recipeNo;
        int _batchSize;
        string _tagNo;

        public QuotationItem()
        {}

        public QuotationItem(string quotationNo, string prodCode, string prodName, decimal qty, decimal unitPrice, string prodNotes)
        {
            this.quotationNo = quotationNo;
            this.prodCode = prodCode;
            this.prodName = prodName;
            this.qty = qty;
            this.unitPrice = unitPrice;
            this.prodNotes = prodNotes; 
        }

        public string quotationNo
        {
            get { return _quotationNo; }
            set { _quotationNo = value; }
        }

        public string prodCode
        {
            get { return _prodCode; }
            set { _prodCode = value; }
        }

        public string prodName
        {
            get { return _prodName; }
            set { _prodName = value; }
        }

        public decimal qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        public decimal unitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        public string prodNotes
        {
            get { return _prodNotes; }
            set { _prodNotes = value; }
        }

        public string recipeNo
        {
            get { return _recipeNo; }
            set { _recipeNo = value; }
        }
       
        public int batchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        public string tagNo
        {
            get { return _tagNo; }
            set { _tagNo = value; }
        }
    }
}
