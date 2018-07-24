using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Wilson.ORMapper;


namespace GMSCore.Entity
{
	///<summary>Database mapping to table tbFinanceAttachmentType</summary>
	[DataObject(true)]
    public partial class FinanceAttachmentType : PersistBase<FinanceAttachmentType>, IObjectHelper
	{
		#region Properties
	
		private short _id;
		///<summary>Database mapping to column tbFinanceAttachmentType.ID</summary>
		public short Id
		{
			get { return _id; }
			set
			{
				if (_id != value)
				{
					_id = value;
					OnPropertyChanged("Id");
				}
			}
		}
			
		private string _financeAttachmentTypeName;
		///<summary>Database mapping to column tbFinanceAttachmentType.FinanceAttachmentTypeName</summary>
		public string FinanceAttachmentTypeName
		{
			get { return _financeAttachmentTypeName; }
			set
			{
				if (_financeAttachmentTypeName != value)
				{
                    _financeAttachmentTypeName = value;
					OnPropertyChanged("FinanceAttachmentTypeName");
				}
			}
		}
		
		#endregion
		
		///<summary>Initializes a new instance of this class</summary>
		public FinanceAttachmentType() : base()
		{
			// Default Constructor
		}
		
		#region IObjectHelper
		/// <summary>Indexer to update local member variables</summary>	
		/// <remarks>This indexer is used by the Wilson ORMapper</remarks>
		object IObjectHelper.this[string memberName]
		{
			get {
				switch (memberName) {
					case "_id": return _id;
                    case "_financeAttachmentTypeName": return _financeAttachmentTypeName;
									
					default: throw new Exception(string.Format("Mapping: IObjectHelper Get is missing member case {0}", memberName));
				}
			}
			set {
				//handle null values
				if(value == null)
					return;
					
				switch (memberName) {
					case "_id": _id = (short)value; break;
					case "_financeAttachmentTypeName": _financeAttachmentTypeName = (string)value; break;
				
					default: throw new Exception(string.Format("Mapping: IObjectHelper Set is missing member case {0}", memberName));
				}
			}
		}
		#endregion
		
		#region ToString
		///<summary>
		///Overwrites the base ToString method. This is used for AuditTrail purposes.
		///</summary>
		public override string ToString()
		{
			System.Text.StringBuilder stb = new System.Text.StringBuilder(500);
			
			stb.AppendFormat("_id={0}\n,", _id.ToString() );
			stb.AppendFormat("_financeAttachmentTypeName={0}\n,", _financeAttachmentTypeName.ToString() );
							
			
			return stb.ToString();
		}
		#endregion

	}
}
