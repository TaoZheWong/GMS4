using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore.Entity
{
    public class Quotation
    {
        string _quotationNo;
        string _accountCode;
        string _address1;
        string _address2;
        string _address3;
        string _address4;
        string _customerPONo;
        string _internalRemarks;
        string _externalRemarks;
        string _contactPerson;
        string _mobilePhone;
        string _officePhone;
        string _fax;
        string _salesPersonID; 
        string _currencyCode;
        decimal _currencyRate; 
        string _taxType; 
        decimal _taxRate; 
        decimal _subTotal; 
        decimal _grandTotal;
        string _orderedBy;
        string _customerSO;
        string _transportZone;
        bool _isSelfCollect;
        Nullable<DateTime> _requiredDate;
        short _addressID;
        string _addressCode;
        string _statusID;
        bool _isOutright;
        bool _isCOP;
        bool _convertLabFile;

        public Quotation()
        {}

        public Quotation(string quotationNo)
        {
            this.quotationNo = quotationNo;           
        }

        public string quotationNo
        {
            get { return _quotationNo; }
            set { _quotationNo = value; }
        }

        public string accountCode
        {
            get { return _accountCode; }
            set { _accountCode = value; }
        }

        public string address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public string address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        public string address3
        {
            get { return _address3; }
            set { _address3 = value; }
        }

        public string address4
        {
            get { return _address4; }
            set { _address4 = value; }
        }

        public string customerPONo
        {
            get { return _customerPONo; }
            set { _customerPONo = value; }
        }

        public string internalRemarks
        {
            get { return _internalRemarks; }
            set { _internalRemarks = value; }
        }

        public string externalRemarks
        {
            get { return _externalRemarks; }
            set { _externalRemarks = value; }
        }

        public string contactPerson
        {
            get { return _contactPerson; }
            set { _contactPerson = value; }
        }

        public string mobilePhone
        {
            get { return _mobilePhone; }
            set { _mobilePhone = value; }
        }

        public string officePhone
        {
            get { return _officePhone; }
            set { _officePhone = value; }
        }

        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string salesPersonID
        {
            get { return _salesPersonID; }
            set { _salesPersonID = value; }
        }

        public string currencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

        public decimal currencyRate
        {
            get { return _currencyRate; }
            set { _currencyRate = value; }
        }

        public string taxType
        {
            get { return _taxType; }
            set { _taxType = value; }
        }

        public decimal taxRate
        {
            get { return _taxRate; }
            set { _taxRate = value; }
        }

        public decimal subTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; }
        }
        
        public decimal grandTotal
        {
            get { return _grandTotal; }
            set { _grandTotal = value; }
        }       
       
        public string orderedBy
        {
            get { return _orderedBy; }
            set { _orderedBy = value; }            
        }
       
        public string CustomerSO
        {
            get { return _customerSO; }
            set { _customerSO = value; }
        }

        public string TransportZone
        {
            get { return _transportZone; }
            set { _transportZone = value;}
        }
        
        public bool IsSelfCollect
        {
            get { return _isSelfCollect; }
            set {_isSelfCollect = value; }
        }
       
        public Nullable<DateTime> RequiredDate
        {
            get { return _requiredDate; }
            set {   _requiredDate = value; }
        }

        public string AddressCode
        {
            get { return _addressCode; }
            set { _addressCode = value; }
        }

        public short AddressID
        {
            get { return _addressID; }
            set { _addressID = value; }
        }

        public string StatusID
        {
            get { return _statusID; }
            set { _statusID = value; }
        }

       

        public bool IsOutright
        {
            get { return _isOutright; }
            set { _isOutright = value; }
        }

        public bool IsCOP
        {
            get { return _isCOP; }
            set { _isCOP = value; }
        }

        public bool ConvertLabFile
        {
            get { return _convertLabFile; }
            set { _convertLabFile = value; }
        }
    }
}
