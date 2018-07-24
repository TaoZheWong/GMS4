using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Configuration;

namespace GMSWebService
{
	/// <summary>
	/// Summary description for Service1
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	public class GMSWebService : System.Web.Services.WebService
	{
		string A21Pwd = "amdynamics";

		#region GetProduct
		[WebMethod]
		public DataSet GetProduct(short companyId, string productCode, string productName, string productGroupCode, string productGroup)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.Product as ProductCode, a.PName as ProductName, " + 
									" b.Code + ' ' + b.[Description] as ProductGroupName, a.purchaseunit as UOM " + 
									" from mstproduct a " + 
									" inner join SUProductGroup b on a.class1 = b.Code " + 
									" where a.Product like '" + productCode + "' and a.PName like '" + productName +
									"' and b.[Description] like '" + productGroup + "' " +
									" and a.class1 like '" + productGroupCode + "'";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procProductStockStatusSelectByProductCode";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@ProductCode", productCode);
			//SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
			//DataTable dt = new DataTable("StockStatus");
			//dataAdapter.Fill(dt);

			//ds.Tables.Add(dt);

			//serverConn.Close();
			//return ds;
		}
		#endregion

		#region GetProductFullDetail 
		[WebMethod]
		public DataSet GetProductFullDetail(short companyId, string productCode, string productName, string productGroupCode, string productGroup)
		{
			//16 Jan 2012 - Ong Siew Siew - Include OnHand, OnOrder, OnPO fields to display in the Search Page. 
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string prodNameSQL = "";
				productName = productName.Substring(1, productName.Length-2); 
				string[] words = productName.Split(' ');
				foreach (string word in words)
				{
					//construct productName conditions 
					prodNameSQL += " and a.PName like '%" + word + "%'"; 
				}
				string cmdString = "select a.Product as ProductCode, a.PName as ProductName, " +
									" b.Code + ' ' + b.[Description] as ProductGroupName, a.purchaseunit as UOM, " +
									" OnHandQuantity, ISNULL(d.SO,0) as OnOrderQuantity, " +
									" ISNULL(c.BO,0) as OnBOQuantity, OnPOQuantity, OnHandQuantity - ISNULL(d.SO,0) As AvailableQuantity, " +
									" a.CostWt as WeightedCost" +
									" from mstproduct a WITH (NOLOCK) " +
									" inner join SUProductGroup b WITH (NOLOCK) on a.class1 = b.Code " +
									" left join (select I.Product, SUM(isnull(I.orderquantity,0) - isnull(I.delquantity,0)) AS BO " +
									" FROM A21OPOrderDetail I WITH (NOLOCK) " +
									" INNER JOIN a21oporderheader OH WITH (NOLOCK) ON I.company = OH.company and I.trntype = OH.trntype and I.trnno = OH.trnno " +
									" where clearflag = 'N' and isNULL(OH.Narration,'') like '%BLANKET ORDER%' " +
									" GROUP BY I.Product " +
									" ) c on c.Product = a.Product" +
									" left join (select I.Product, SUM(isnull(I.orderquantity,0) - isnull(I.delquantity,0)) AS SO " +
									" FROM A21OPOrderDetail I WITH (NOLOCK) " +
									" INNER JOIN a21oporderheader OH WITH (NOLOCK) ON I.company = OH.company and I.trntype = OH.trntype and I.trnno = OH.trnno " +
									" where clearflag = 'N' and isNULL(OH.Narration,'') NOT like '%BLANKET ORDER%' " +
									" GROUP BY I.Product " +
									" ) d on d.Product = a.Product" +
									" where a.Product like '" + productCode + "'" + prodNameSQL +  
									" and b.[Description] like '" + productGroup + "' " +
									" and a.class1 like '" + productGroupCode + "'";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		#region GetProductDetail
		[WebMethod]
		public DataSet GetProductDetail(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.Product as ProductCode, a.PName as ProductName, b.Code as ProductGroupCode, b.Code + ' ' + b.[Description] as ProductGroupName, " + 
								   " a.PurchaseUnit as UOM, a.CostWt as WeightedCost, " + 
								   " a.OnOrderQuantity as OnSOQty, a.OnPOQuantity as OnPOQty " + 
								   " from mstproduct a " + 
								   " inner join SUProductGroup b on a.class1 = b.Code " + 
								   " where a.Product = '" + productCode + "'" ;
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procProductStockStatusSelectByProductCode";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@ProductCode", productCode);
			//SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
			//DataTable dt = new DataTable("StockStatus");
			//dataAdapter.Fill(dt);

			//ds.Tables.Add(dt);

			//serverConn.Close();
			//return ds;
		}
		#endregion

		#region GetProductStockStatus
		[WebMethod]
		public DataSet GetProductStockStatus(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString =  "select Warehouse, w.Description as WarehouseName, Quantity " + 
									"from A21IMWarehouseStock a, SMWarehouse w " + 
									"where a.warehouse = w.code " + 
									"and Product = '" + productCode + "' ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procProductStockStatusSelectByProductCode";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@ProductCode", productCode);
			//SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
			//DataTable dt = new DataTable("StockStatus");
			//dataAdapter.Fill(dt);

			//ds.Tables.Add(dt);

			//serverConn.Close();
			//return ds;
		}
		#endregion

		#region GetProductStockStatusForPanel
		[WebMethod]
		public DataSet GetProductStockStatusForPanel(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select Warehouse, w.Description as WarehouseName, Quantity " +
									"from A21IMWarehouseStock a, SMWarehouse w " +
									"where a.warehouse = w.code " +
									"and Product = '" + productCode + "' ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procProductStockStatusSelectByProductCode";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@ProductCode", productCode);
			//SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
			//DataTable dt = new DataTable("StockStatus");
			//dataAdapter.Fill(dt);

			//ds.Tables.Add(dt);

			//serverConn.Close();
			//return ds;
		}
		#endregion

		#region GetProductOnSODetail
		[WebMethod]
		public DataSet GetProductOnSODetail(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString =  "select a.TrnNo, b.TrnDate, a.OrderQuantity, a.DelQuantity, c.CName as AccountName, " +
									"isnull(a.orderquantity,0) - isnull(a.delquantity,0) as Qty, isNULL(b.Narration,'') as Narration " + 
									"from a21oporderdetail a " + 
									"inner join a21oporderheader b on a.trntype = b.trntype and a.trnno = b.trnno " + 
									"inner join mstcustomersupplier c on b.code = c.code " +
									"where clearflag = 'N' and isNULL(b.Narration,'') not like '%BLANKET ORDER%' and a.Product = '" + productCode + "' ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		#region GetProductOnBODetail
		[WebMethod]
		public DataSet GetProductOnBODetail(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.TrnNo, b.TrnDate, a.OrderQuantity, a.DelQuantity, c.CName as AccountName, " +
									"isnull(a.orderquantity,0) - isnull(a.delquantity,0) as Qty, isNULL(b.Narration,'') as Narration " +
									"from a21oporderdetail a " +
									"inner join a21oporderheader b on a.trntype = b.trntype and a.trnno = b.trnno " +
									"inner join mstcustomersupplier c on b.code = c.code " +
									"where clearflag = 'N' and isNULL(b.Narration,'') like '%BLANKET ORDER%' and a.Product = '" + productCode + "' ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		#region GetProductOnPODetail
		[WebMethod]
		public DataSet GetProductOnPODetail(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select b.docno as PONo, a.TrnNo, b.TrnDate, a.OrderQuantity, a.DelQuantity, c.CName as AccountName, " +
									"isnull(a.orderquantity,0) - isnull(a.delquantity,0) as Qty, b.DelDate, b.DelMode, b.CrtUser, b.Narration " + 
									"from a21poorderdetail a " + 
									"inner join a21poorderheader b on a.trntype = b.trntype and a.trnno = b.trnno " + 
									"inner join mstcustomersupplier c on b.code = c.code " +
									"where clearflag = 'N' and a.Product = '" + productCode + "' ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		//#region GetProductOnBODetail
		//[WebMethod]
		//public DataSet GetProductOnBODetail(short companyId, string Right, string listing)
		//{
		//	string serverName = "";
		//	string dbName = "";

		//	string connString = ConfigurationSettings.AppSettings["DBConnection"];
		//	SqlConnection serverConn = new SqlConnection(connString);
		//	SqlDataReader rdr = null;

		//	try
		//	{
		//		serverConn.Open();
		//		SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
		//		rdr = cmd.ExecuteReader();

		//		while (rdr.Read())
		//		{
		//			serverName = (string)rdr[0];
		//			dbName = (string)rdr[1];
		//			A21Pwd = (string)rdr[2];
		//		}
		//	}
		//	finally
		//	{
		//		if (rdr != null)
		//		{
		//			rdr.Close();
		//		}

		//		if (serverConn != null)
		//		{
		//			serverConn.Close();
		//		}
		//	}

		//	SqlConnection a21Conn = new SqlConnection(
		//	"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

		//	DataSet ds = new DataSet();
		//	try
		//	{
		//		a21Conn.Open();
		//		string cmdString = string.Empty;

		//		if (Right == "All")
		//		{
		//			cmdString = "SELECT P.Product, P.PName, P.CostWt, AVGPrice, QtyLY, RevLY, QtyYTD, RevYTD, " +
		//						"OnHandQuantity as OnHand, OnBO, " +
		//						"OnHandQuantity-OnOrderQuantity+OnPOQuantity as ActQty, QMEQ, HMEQ, " +
		//						"(case when (OnHandQuantity-OnOrderQuantity+OnPOQuantity)<QMEQ " +
		//						"then QMEQ-(OnHandQuantity-OnOrderQuantity+OnPOQuantity) else 0 end) as QMEQRec, " +
		//						"(case when (OnHandQuantity-OnOrderQuantity+OnPOQuantity)<HMEQ " +
		//						"then HMEQ-(OnHandQuantity-OnOrderQuantity+OnPOQuantity) else 0 end) as HMEQRec " +
		//						"FROM MSTProduct P " +
		//						"LEFT JOIN (SELECT I.Product, " +
		//						"AVG((I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * GL.ExchangeRate) as AVGPrice, " +
		//						"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) - 1 then I.Quantity else 0 end) as QtyLY, " +
		//						"SUM(case when YEAR(GL.trndate) = YEAR(getdate())-1 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as RevLY, " +
		//						"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) then I.Quantity else 0 end) as QtyYTD, " +
		//						"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as RevYTD, " +
		//						"SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as Last12MthTeam, " +
		//						"SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as Last12MthTotal, " +
		//						"SUM(case when Narration like '%BLANKET ORDER%' then I.Quantity else 0 end) as OnBO, " +
		//						"(SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then I.Quantity else 0 end)/12)*3 as QMEQ, " +
		//						"(SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then I.Quantity else 0 end)/12)*6 as HMEQ " +
		//						"FROM A21GLTransactionHeader GL " +
		//						"INNER JOIN a21opinvoicedetail I ON I.company = GL.company and I.trntype = GL.trntype and I.trnno = GL.trnno " +
		//						"INNER JOIN MSTCustomerSupplier CS ON CS.Code=GL.Code " +
		//						"WHERE GL.TrnType IN ('CA','CC','CD','CE','CI','CM','CR') AND YEAR(GL.trndate)> YEAR(GETDATE())-2 " +
		//						"GROUP BY I.Product " +
		//						") GLI ON P.Product = GLI.Product " +
		//						"WHERE P.Product not like 'Z%' ";
		//	}
		//		else if (Right == "Sales")
		//	{
		//		cmdString = "SELECT P.Product, P.PName, P.CostWt, AVGPrice, QtyLY, RevLY, QtyYTD, RevYTD, " +
		//					"(case when Last12MthTotal>0 then (Last12MthTeam/Last12MthTotal)*OnHandQuantity else 0 end) as OnHand, OnBO, " +
		//					"(case when Last12MthTotal>0 then (Last12MthTeam/Last12MthTotal)*OnHandQuantity else 0 end)-OnOrderQuantity+OnPOQuantity as ActQty, QMEQ, HMEQ, " +
		//					"(case when ((case when Last12MthTotal>0 then (Last12MthTeam/Last12MthTotal)*OnHandQuantity else 0 end)-OnOrderQuantity+OnPOQuantity)<QMEQ " +
		//					"then QMEQ-((case when Last12MthTotal>0 then (Last12MthTeam/Last12MthTotal)*OnHandQuantity else 0 end)-OnOrderQuantity+OnPOQuantity) else 0 end) as QMEQRec, " +
		//					"(case when ((case when Last12MthTotal>0 then (Last12MthTeam/Last12MthTotal)*OnHandQuantity else 0 end)-OnOrderQuantity+OnPOQuantity)<HMEQ " +
		//					"then HMEQ-((case when Last12MthTotal>0 then (Last12MthTeam/Last12MthTotal)*OnHandQuantity else 0 end)-OnOrderQuantity+OnPOQuantity) else 0 end) as HMEQRec " +
		//					"FROM MSTProduct P " +
		//					"LEFT JOIN (SELECT I.Product, " +
		//					"AVG((I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * GL.ExchangeRate) as AVGPrice, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) - 1 then I.Quantity else 0 end) as QtyLY, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate())-1 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as RevLY, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) then I.Quantity else 0 end) as QtyYTD, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as RevYTD, " +
		//					"SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 and CS.SalesPerson IN (" + listing + ") then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as Last12MthTeam, " +
		//					"SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate	else 0 end) as Last12MthTotal, " +
		//					"SUM(case when Narration like '%BLANKET ORDER%' then I.Quantity else 0 end) as OnBO, " +
		//					"(SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then I.Quantity else 0 end)/12)*3 as QMEQ, " +
		//					"(SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then I.Quantity else 0 end)/12)*6 as HMEQ " +
		//					"FROM A21GLTransactionHeader GL " +
		//					"INNER JOIN a21opinvoicedetail I ON I.company = GL.company and I.trntype = GL.trntype and I.trnno = GL.trnno " +
		//					"INNER JOIN MSTCustomerSupplier CS ON CS.Code=GL.Code " +
		//					"WHERE GL.TrnType IN ('CA','CC','CD','CE','CI','CM','CR') AND YEAR(GL.trndate)> YEAR(GETDATE())-2 " +
		//					"GROUP BY I.Product " +
		//					") GLI ON P.Product = GLI.Product " +
		//					"WHERE P.Product not like 'Z%'";
		//	}
		//	else //PM
		//	{
		//		cmdString = "SELECT P.Product, P.PName, P.CostWt, AVGPrice, QtyLY, RevLY, QtyYTD, RevYTD, " +
		//					"OnHandQuantity as OnHand, OnBO, " +
		//					"OnHandQuantity-OnOrderQuantity+OnPOQuantity as ActQty, QMEQ, HMEQ, " +
		//					"(case when (OnHandQuantity-OnOrderQuantity+OnPOQuantity)<QMEQ " +
		//					"then QMEQ-(OnHandQuantity-OnOrderQuantity+OnPOQuantity) else 0 end) as QMEQRec, " +
		//					"(case when (OnHandQuantity-OnOrderQuantity+OnPOQuantity)<HMEQ " +
		//					"then HMEQ-(OnHandQuantity-OnOrderQuantity+OnPOQuantity) else 0 end) as HMEQRec " +
		//					"FROM MSTProduct P " +
		//					"LEFT JOIN (SELECT I.Product, " +
		//					"AVG((I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * GL.ExchangeRate) as AVGPrice, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) - 1 then I.Quantity else 0 end) as QtyLY, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate())-1 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as RevLY, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) then I.Quantity else 0 end) as QtyYTD, " +
		//					"SUM(case when YEAR(GL.trndate) = YEAR(getdate()) then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as RevYTD, " +
		//					"SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as Last12MthTeam, " +
		//					"SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then (I.UnitPrice-(I.UnitPrice * I.LineDiscount1)) * I.Quantity * GL.ExchangeRate else 0 end) as Last12MthTotal, " +
		//					"SUM(case when Narration like '%BLANKET ORDER%' then I.Quantity else 0 end) as OnBO, " +
		//					"(SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then I.Quantity else 0 end)/12)*3 as QMEQ, " +
		//					"(SUM(case when DATEDIFF(month,GL.trndate, GETDATE()) <= 12 then I.Quantity else 0 end)/12)*6 as HMEQ " +
		//					"FROM A21GLTransactionHeader GL " +
		//					"INNER JOIN a21opinvoicedetail I ON I.company = GL.company and I.trntype = GL.trntype and I.trnno = GL.trnno " +
		//					"INNER JOIN MSTCustomerSupplier CS ON CS.Code=GL.Code " +
		//					"WHERE GL.TrnType IN ('CA','CC','CD','CE','CI','CM','CR') AND YEAR(GL.trndate)> YEAR(GETDATE())-2 " +
		//					"GROUP BY I.Product " +
		//					") GLI ON P.Product = GLI.Product " +
		//					"WHERE P.Product not like 'Z%' and P.Class1 IN (" + listing + ") ";
		//	}

		//	SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
		//		cmd.CommandType = CommandType.Text;
		//		SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
		//		dataAdapter.Fill(ds);
		//	}
		//	finally
		//	{
		//		if (a21Conn != null)
		//		{
		//			a21Conn.Close();
		//		}
		//	}
		//	return ds;

		//}
		//#endregion

		#region GetProductDetailByProductCode
		[WebMethod]
		public DataSet GetProductDetailByProductCode(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.Product as Product, ISNULL(b.SO,0) as OnOrderQuantity, ISNULL(c.BO,0) as OnBOQuantity, OnPOQuantity " +
									" from mstproduct a " +
									" left join (select I.Product, SUM(isnull(I.orderquantity,0) - isnull(I.delquantity,0)) AS SO " +
									" FROM A21OPOrderDetail I WITH (NOLOCK) " +
									" INNER JOIN a21oporderheader OH WITH (NOLOCK) ON I.company = OH.company and I.trntype = OH.trntype and I.trnno = OH.trnno " +
									" where clearflag = 'N' and isNULL(OH.Narration,'') NOT like '%BLANKET ORDER%' " +
									" GROUP BY I.Product " +
									" ) b on b.Product = a.Product " +
									" left join (select I.Product, SUM(isnull(I.orderquantity,0) - isnull(I.delquantity,0)) AS BO " +
									" FROM A21OPOrderDetail I WITH (NOLOCK) " +
									" INNER JOIN a21oporderheader OH WITH (NOLOCK) ON I.company = OH.company and I.trntype = OH.trntype and I.trnno = OH.trnno " +
									" where clearflag = 'N' and isNULL(OH.Narration,'') like '%BLANKET ORDER%' " +
									" GROUP BY I.Product " +
									" ) c on c.Product = a.Product " +
									" where a.Product like '" + productCode + "' ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		#region GetProductCostByProductCode
		[WebMethod]
		public Double GetProductCostByProductCode(short companyId, string prodCode)
		{
			double cost = 0;
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			try
			{
				a21Conn.Open();
				SqlCommand cmd = new SqlCommand("select costwt from mstproduct where product='" + prodCode + "'", a21Conn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					cost = (double)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			return cost;
		}
		#endregion

		#region GetProductStockMovement
		[WebMethod]
		public DataSet GetProductStockMovement(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";
			string cmdString = "";
			int i = 0;

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			try
			{

				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName, Seq from tbA21Database where coyId=" + companyId + " order by Seq", serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					if (i > 0)
						cmdString += " UNION ALL ";

					cmdString += "select g.TrnDate, s.TrnType, s.TrnNo, s.receiptquantity as ReceivedQty, s.issuequantity as IssuedQty, s.BalQty as BalanceQty, g.Narration , " + rdr[1] + " as DBVersion " +
								   " from " + (string)rdr[0] + ".dbo.a21imtransactiondetail s, " + (string)rdr[0] + ".dbo.a21gltransactionheader g " +
								   " where s.trntype = g.trntype and s.trnno = g.trnno " +
								   " and s.Product = '" + productCode + "' ";
								  

					i++;

				}

				//if (i > 0)
				//	cmdString += " order by DBVersion, g.TrnDate desc ";



			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}



			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
			   
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procProductStockStatusSelectByProductCode";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@ProductCode", productCode);
			//SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
			//DataTable dt = new DataTable("StockStatus");
			//dataAdapter.Fill(dt);

			//ds.Tables.Add(dt);

			//serverConn.Close();
			//return ds;
		}
		#endregion

		#region IsProductCodeValid
		[WebMethod]
		public bool IsProductCodeValid(short companyId, string prodCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			int count = 0;

			try
			{
				a21Conn.Open();
				SqlCommand cmd = new SqlCommand("select count(*) from mstproduct where product='" + prodCode + "'", a21Conn);
				count = (int)cmd.ExecuteScalar();
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}

			return count == 0 ? false : true;
		}
		#endregion

		#region InsertA21QuotationHeader
		[WebMethod]
		public int InsertA21QuotationHeader(short companyId, string accountCode, string currency, DateTime trnDate, decimal exchangeRate,
												string docNo, string add1, string add2, string add3, string add4, string gstType, decimal gst,
												string salesPerson, decimal billAmt)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			int trnNo = 0;

			try
			{
				a21Conn.Open();
				SqlCommand cmd = new SqlCommand("select LastNumber + 1 as TrnNo from U21TransactionNos where Company = 'nn' and TrnType = 'CQ' ", a21Conn);
				trnNo = (int)cmd.ExecuteScalar();
				cmd = new SqlCommand("insert into A21OPQuotationHeader " + 
						"(Company, TrnType, TrnNo, Code, Status, Currency, TrnDate, TrnTime, [Year], [Month], " + 
						"[Week], ExchangeRate, DocNo, TrnScreen, DAdd1, DAdd2, DAdd3, " + 
						"DAdd4, GSTType, GST, Discount, DelDate, SalesPerson, Project, Department, [Section], " + 
						"UpdUser, UpdDate, UpdTime, CrtUser, CrtDate, CrtTime, BillAmt, QuoNo, QuoRevision, OpenedStatus) " + 
						"Values " +
						"('nn', 'CQ', " + trnNo.ToString() + ", '" + accountCode + "', 'P', '" + currency + "', '" + trnDate.ToString("MM/dd/yyyy") + "', " +
						"getdate(), year('" + trnDate.ToString("MM/dd/yyyy") + "'), " +
						"month('" + trnDate.ToString("MM/dd/yyyy") + "'), DATEPART(week, '" + trnDate.ToString("MM/dd/yyyy") + "'), " + 
						exchangeRate.ToString() + ", '" + docNo + "', 'CL', '" + add1 + "', '" + add2 + "', '" + add3 + "', " +
						"'" + add4 + "', '" + gstType + "', " + gst.ToString() + ", 0, '" + trnDate.ToString("MM/dd/yyyy") + "', " + 
						"'" + salesPerson + "', 'None', 'NA', 'NA', " + 
						"'KSCHAN', getdate(), getdate(), 'KSCHAN', getdate(), getdate()," + billAmt.ToString() + ", " + trnNo.ToString() + ", 1, 1) ", a21Conn);
				cmd.ExecuteNonQuery();
				cmd = new SqlCommand("update U21TransactionNos set LastNumber = " + trnNo.ToString() +  
									" from U21TransactionNos " + 
									" where Company = 'nn' and TrnType = 'CQ'", a21Conn);
				cmd.ExecuteNonQuery();
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return trnNo;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procA21QuotationHeaderInsert";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@Code", accountCode);
			//cmd.Parameters.AddWithValue("@Currency", currency);
			//cmd.Parameters.AddWithValue("@TrnDate", trnDate);
			//cmd.Parameters.AddWithValue("@ExchangeRate", exchangeRate);
			//cmd.Parameters.AddWithValue("@DocNo", docNo);
			//cmd.Parameters.AddWithValue("@DAdd1", add1);
			//cmd.Parameters.AddWithValue("@DAdd2", add2);
			//cmd.Parameters.AddWithValue("@DAdd3", add3);
			//cmd.Parameters.AddWithValue("@DAdd4", add4);
			//cmd.Parameters.AddWithValue("@GSTType", gstType);
			//cmd.Parameters.AddWithValue("@GST", gst);
			//cmd.Parameters.AddWithValue("@SalesPerson", salesPerson);
			//cmd.Parameters.AddWithValue("@BillAmt", billAmt);
			//int trnNo = (int) cmd.ExecuteScalar();

			//serverConn.Close();
			//return trnNo;
		}
		#endregion

		#region InsertUpdateA21QuotationDetail
		[WebMethod]
		public void InsertUpdateA21QuotationDetail(short companyId, int trnNo, short srNo, string product, decimal orderQuantity,
												string uOM, decimal unitPrice, string prodDesc)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			try
			{
				a21Conn.Open();
				SqlCommand cmd = new SqlCommand("select * " + 
										"from A21OPQuotationDetail " + 
										"where Company = 'nn' and TrnType = 'CQ' and TrnNo = " + trnNo.ToString() + " " + 
										"and SrNo = " + srNo.ToString(), a21Conn);
				rdr= cmd.ExecuteReader();
				if (rdr.HasRows)
				{
					rdr.Close();
					cmd = new SqlCommand("update A21OPQuotationDetail " +
						"set Product =  '" + product + "', OrderQuantity = " + orderQuantity.ToString() + ", " +
						"UOM = '" + uOM + "', UnitPrice = " + unitPrice.ToString() + ", MQty = " + orderQuantity.ToString() + ", " +
						"CUnitPrice = " + unitPrice.ToString() + " " +
						"from A21OPQuotationDetail " +
						"where Company = 'nn' and TrnType = 'CQ' and TrnNo = " + trnNo.ToString() + " " +
						"and SrNo = " + srNo.ToString(), a21Conn);
					cmd.ExecuteNonQuery();
				}
				else
				{
					rdr.Close();
					cmd = new SqlCommand("insert into A21OPQuotationDetail " + 
						"(Company, TrnType, TrnNo, SrNo, Product, OrderQuantity, DelQuantity, ShortQuantity, UOM, UnitPrice, " + 
						"LineDiscount1, LineDiscount2, LineDiscount3, ProdDesc, ClearFlag, StockUnit, Conversion, " + 
						"StockQuantity, MQty, CUnitPrice, DSrNo) " + 
						"Values " + 
						"('nn', 'CQ', " + trnNo.ToString() + ", " + srNo.ToString() + ", '" + product + "', " + 
						orderQuantity.ToString() + ", 0, 0, '" + uOM.ToString() + "', " + unitPrice.ToString() + ", " +
						"0, 0, 0, '" + prodDesc.ToString() + "', 'N', '" + uOM.ToString() + "', 1, 0, " + orderQuantity.ToString() + ", " +
						unitPrice.ToString() + ", " + srNo.ToString() + ") ", a21Conn);
					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procA21QuotationDetailInsertUpdate";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@TrnNo", trnNo);
			//cmd.Parameters.AddWithValue("@SrNo", srNo);
			//cmd.Parameters.AddWithValue("@Product", product);
			//cmd.Parameters.AddWithValue("@OrderQuantity", orderQuantity);
			//cmd.Parameters.AddWithValue("@UOM", uOM);
			//cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
			//cmd.Parameters.AddWithValue("@ProdDesc", prodDesc);
		   
			//cmd.ExecuteNonQuery();

			//serverConn.Close();
		}
		#endregion

		#region UpdateA21QuotationHeader
		[WebMethod]
		public void UpdateA21QuotationHeader(short companyId, int a21TrnNo, string accountCode, string currency, DateTime trnDate, decimal exchangeRate,
												string docNo, string add1, string add2, string add3, string add4, string gstType, decimal gst,
												string salesPerson, decimal billAmt)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			try
			{
				a21Conn.Open();
				SqlCommand cmd = new SqlCommand("update A21OPQuotationHeader " + 
					"set Code = '" + accountCode + "', Currency = '" + currency + "', TrnDate = '" + trnDate.ToString("MM/dd/yyyy") + "', " + 
					"[Year] = year('" + trnDate.ToString("MM/dd/yyyy") + "'), [Month] = month('" + trnDate.ToString("MM/dd/yyyy") + "'), " + 
					"[Week] = DATEPART(week, '" + trnDate.ToString("MM/dd/yyyy") + "'), ExchangeRate = " + exchangeRate.ToString() + ", " + 
					"DocNo = '" + docNo + "', DAdd1 = '" + add1 + "', DAdd2 = '" + add2 + "', DAdd3 = '" + add3 + "', " + 
					"DAdd4 = '" + add4 + "', GSTType = '" + gstType + "', GST = " + gst.ToString() + ", DelDate = '" + 
					trnDate.ToString("MM/dd/yyyy") + "', " + "SalesPerson = '" + salesPerson + "', UpdDate = getdate(), UpdTime = getdate(), " + 
					"BillAmt = " + billAmt.ToString() + " from A21OPQuotationHeader " + 
					"where Company = 'nn' and TrnType = 'CQ' and TrnNo = " +  a21TrnNo.ToString(), a21Conn);
					cmd.ExecuteNonQuery();
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procA21QuotationHeaderUpdate";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@TrnNo", a21TrnNo);
			//cmd.Parameters.AddWithValue("@Code", accountCode);
			//cmd.Parameters.AddWithValue("@Currency", currency);
			//cmd.Parameters.AddWithValue("@TrnDate", trnDate);
			//cmd.Parameters.AddWithValue("@ExchangeRate", exchangeRate);
			//cmd.Parameters.AddWithValue("@DocNo", docNo);
			//cmd.Parameters.AddWithValue("@DAdd1", add1);
			//cmd.Parameters.AddWithValue("@DAdd2", add2);
			//cmd.Parameters.AddWithValue("@DAdd3", add3);
			//cmd.Parameters.AddWithValue("@DAdd4", add4);
			//cmd.Parameters.AddWithValue("@GSTType", gstType);
			//cmd.Parameters.AddWithValue("@GST", gst);
			//cmd.Parameters.AddWithValue("@SalesPerson", salesPerson);
			//cmd.Parameters.AddWithValue("@BillAmt", billAmt);
			//cmd.ExecuteNonQuery();

			//serverConn.Close();
		}
		#endregion

		#region DeleteA21QuotationDetail
		[WebMethod]
		public void DeleteA21QuotationDetail(short companyId, int trnNo, short srNo)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			try
			{
				a21Conn.Open();
				SqlCommand cmd = new SqlCommand("delete A21OPQuotationDetail " + 
					"from A21OPQuotationDetail " + 
					"where Company = 'nn' and TrnType = 'CQ' and TrnNo = " + trnNo.ToString() + " " + 
					"and SrNo = " + srNo.ToString(), a21Conn);
				cmd.ExecuteNonQuery();
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}


			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procA21QuotationDetailDelete";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@TrnNo", trnNo);
			//cmd.Parameters.AddWithValue("@SrNo", srNo);

			//cmd.ExecuteNonQuery();

			//serverConn.Close();
		}
		#endregion

		#region A21QuotationUpdateStatus
		[WebMethod]
		public DataTable A21QuotationUpdateStatus(short companyId)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataTable dt = new DataTable("QuotationHeader");
			try
			{
				a21Conn.Open();
				string cmdString = "select a.*, case when b.trnno is null then 1 else 0 end as Completed " + 
					"from " + 
					"(select distinct Company, TrnType, TrnNo " + 
					"from A21OPQuotationDetail " + 
					"where delquantity > 0) a left " + 
					"join ( " + 
					"select  distinct Company, TrnType, TrnNo " + 
					"from A21OPQuotationDetail " + 
					"where clearflag = 'N' " + 
					") b on a.company = b.company and a.trntype = b.trntype and a.trnno = b.trnno ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(dt);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}

			return dt;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procA21UpdateQuotationStatus";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);

			//cmd.ExecuteNonQuery();

			//serverConn.Close();
		}
		#endregion

		#region A21QuotationUpdateStatusByA21TrnNo
		[WebMethod]
		public DataTable A21QuotationUpdateStatusByA21TrnNo(short companyId, int a21TrnNo)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataTable dt = new DataTable("QuotationHeader");
			try
			{
				a21Conn.Open();
				string cmdString = "select a.*, case when b.trnno is null then 1 else 0 end as Completed " +
					"from " +
					"(select distinct Company, TrnType, TrnNo " +
					"from A21OPQuotationDetail " +
					"where delquantity > 0 and trnno = " + a21TrnNo.ToString() + ") a left " +
					"join ( " +
					"select  distinct Company, TrnType, TrnNo " +
					"from A21OPQuotationDetail " +
					"where clearflag = 'N'  and trnno = " + a21TrnNo.ToString() + 
					") b on a.company = b.company and a.trntype = b.trntype and a.trnno = b.trnno ";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(dt);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return dt;

			//DataSet ds = new DataSet();
			//string connString = ConfigurationSettings.AppSettings["DBConnection"];
			//SqlConnection serverConn = new SqlConnection(connString);
			//serverConn.Open();

			//string cmdString = "procA21UpdateQuotationStatusByA21TrnNo";
			//SqlCommand cmd = new SqlCommand(cmdString, serverConn);
			//cmd.CommandType = CommandType.StoredProcedure;
			//cmd.Parameters.Clear();
			//cmd.Parameters.AddWithValue("@CoyID", companyId);
			//cmd.Parameters.AddWithValue("@TrnNo", a21TrnNo);

			//cmd.ExecuteNonQuery();

			//serverConn.Close();
		}
		#endregion

		#region A21CurrencyRateByCurrencyCode
		[WebMethod]
		public decimal A21CurrencyRateByCurrencyCode(short companyId, string currencyCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			decimal currencyRate = 1;
			try
			{
				a21Conn.Open();
				string cmdString = "select RevRate " +
					"from SMCurrency " +
					"where code = '" + currencyCode + "'";
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				currencyRate = decimal.Parse(cmd.ExecuteScalar().ToString());
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}

			return currencyRate;
		}
		#endregion

		#region GetSalesOrder
		[WebMethod]
		public DataSet GetSalesOrder(short companyId, string accountCode, string accountName, string poNumber, DateTime dateFrom, DateTime dateTo, string salesPersonsID, string productCode, string productName, string productGroupCode, string productGroupName, string soNumber, string productDetailDesc)
		{
			string serverName = "";
			string dbName = "";
			string cmdString = "";
			int i = 0;

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}



			try
			{

				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName, Seq from tbA21Database where coyId=" + companyId + " order by Seq", serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					if (i > 0)
						cmdString += " UNION ALL ";

					cmdString += "select distinct b.TrnNo, b.TrnDate, b.status, b.crtuser, b.custponumber, c.CName as AccountName, c.Code as AccountCode, " + rdr[1] + " as DBVersion " +
								 "from " + (string)rdr[0] + ".dbo.a21oporderheader b WITH (NOLOCK) " +
								 "inner join " + (string)rdr[0] + " .dbo.a21oporderdetail a WITH (NOLOCK) on a.trntype = b.trntype and a.trnno = b.trnno " +
								 "inner join " + (string)rdr[0] + " .dbo.mstcustomersupplier c WITH (NOLOCK) on b.code = c.code " +
								 "inner join " + (string)rdr[0] + " .dbo.MSTProduct d WITH (NOLOCK) on a.product = d.product " +
								 "inner join " + (string)rdr[0] + " .dbo.SUProductGroup e WITH (NOLOCK) on e.Code = d.Class1 " +
								 "left join " + (string)rdr[0] + " .dbo.A21OPOrdProductDesc h WITH (NOLOCK) on h.trntype = a.trntype and h.trnno = a.trnno " +
								 "where c.Code LIKE '" + accountCode + "' AND a.trntype = 'CO' AND " +
								 "c.CName LIKE '" + accountName + "' AND " +
								 "a.product LIKE '" + productCode + "' AND " +
								 "a.proddesc LIKE '" + productName + "' AND " +
								 "d.Class1 LIKE '" + productGroupCode + "' AND " +
								 "e.Description LIKE '" + productGroupName + "' AND ";

					if (poNumber.Trim() != "%%")
						cmdString += "b.custponumber LIKE '" + poNumber + "' AND ";

					if (productDetailDesc.Trim() != "%%")
						cmdString += "h.Description LIKE '" + productDetailDesc + "' AND ";
					else
						cmdString += "(h.Description LIKE '" + productDetailDesc + "' OR h.Description is NULL ) AND ";

					cmdString += "b.TrnDate >= '" + dateFrom + "' AND b.TrnDate <= '" + dateTo + "' AND " +
					"b.TrnNo LIKE '" + soNumber + "' AND " +
					"c.salesperson IN (" + salesPersonsID + ")";

					i++;

				}


			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}





			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open(); 
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetSalesOrderByTrnNo
		[WebMethod]
		public DataSet GetSalesOrderByTrnNo(short companyId, string trnNo, string salesPersonsID, short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}



			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select e.ename as salespersonname, b.TrnNo, b.TrnDate, b.status, b.crtuser,b.upduser,b.salesperson,b.narration, b.custponumber, b.dadd1 as DeliveryAddress1, b.dadd2 as DeliveryAddress2, b.dadd3 as DeliveryAddress3, b.dadd4 as DeliveryAddress4, b.dadd1 as OrderBy, b.currency as Currency, b.docno as DocNo, b.gst, b.discount, b.billamt ,c.mobile, c.telephone1 as OfficePhone, c.fax, c.email,c.contact, c.CName as AccountName, c.Code as AccountCode " +
								   "from a21oporderheader b WITH (NOLOCK) " +
								   "inner join mstcustomersupplier c WITH (NOLOCK) on b.code = c.code " +
								   "inner join mstemployee e WITH (NOLOCK) on e.employee = b.salesperson " +
								   "where b.TrnNo = '" + trnNo + "' AND b.trntype = 'CO' AND " +
								   "c.salesperson IN (" + salesPersonsID + ")";                                   

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion
		
		#region GetSalesOrderDetails
		[WebMethod]
		public DataSet GetSalesOrderDetails(short companyId, string trnNo, string salesPersonsID, short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.product as ProductCode,a.proddesc as ProductDescription,a.clearflag, b.currency as Currency, a.uom, a.TrnNo, a.unitprice as UnitPrice,a.LineDiscount1 as Discount, b.TrnDate, a.OrderQuantity, b.SalesPerson, a.DelQuantity, c.CName as AccountName, a.SrNo " +
									"from a21oporderdetail a WITH (NOLOCK) " +
									"inner join a21oporderheader b WITH (NOLOCK) on a.trntype = b.trntype and a.trnno = b.trnno " +
									"inner join mstcustomersupplier c WITH (NOLOCK) on b.code = c.code " +
									"where a.TrnNo = '" + trnNo + "' AND a.trntype = 'CO' AND " +
									"c.salesperson IN (" + salesPersonsID + ")";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetSalesOrderProductDesc
		[WebMethod]
		public DataSet GetSalesOrderProductDesc(short companyId, string trnNo, string trnType, string srNo, short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select * from A21OPOrdProductDesc a WITH (NOLOCK) " +
									"where a.TrnNo = '" + trnNo + "' and a.TrnType = '" + trnType + "' and a.SrNo = '" + srNo + "' order by DescNo ";

			   
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetDeliveryOrder
		[WebMethod]
		public DataSet GetDeliveryOrder(short companyId, string accountCode, string accountName, DateTime dateFrom, DateTime dateTo, string salesPersonsID, string productCode, string productName, string productGroupCode, string productGroupName, string donumber, string customerPO, string productDetailDesc)
		{
			string serverName = "";
			string dbName = "";
			string cmdString = "";
			int i = 0;

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName, Seq from tbA21Database where coyId=" + companyId + " order by Seq", serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					if (i > 0)
						cmdString += " UNION ALL ";

					cmdString += "select distinct d.DocNo as DONo, b.TrnNo, b.trntype, d.trndate, c.CName as AccountName, c.Code as AccountCode, g.custponumber as Custponumber , " + rdr[1] + " as DBVersion " +
								   "from " + (string)rdr[0] + ".dbo.A21OPInvoiceHeader b WITH (NOLOCK) " +
								   "inner join " + (string)rdr[0] + ".dbo.A21OPInvoiceDetail a WITH (NOLOCK) on a.trntype = b.trntype and a.trnno = b.trnno " +
								   "inner join " + (string)rdr[0] + ".dbo.A21GLTransactionHeader d WITH (NOLOCK) on d.company = b.company and d.TrnType = b.TrnType and d.TrnNo = b.TrnNo " +
								   "inner join " + (string)rdr[0] + ".dbo.mstcustomersupplier c WITH (NOLOCK) on c.code = d.code " +
								   "inner join " + (string)rdr[0] + ".dbo.MSTProduct f WITH (NOLOCK) on a.product = f.product " +
								   "inner join " + (string)rdr[0] + ".dbo.SUProductGroup e WITH (NOLOCK) on e.Code = f.Class1 " +
								   "left join (select distinct do.trnno, custponumber from " + (string)rdr[0] + ".dbo.a21opinvoicedetail do WITH (NOLOCK) , " + (string)rdr[0] + ".dbo.a21oporderheader so WITH (NOLOCK) where do.trntype = 'CY' and do.potrntype = so.trntype and do.potrnno = so.trnno ) g on a.trnno = g.trnno and a.trntype = 'CY'   " +
								   "left join " + (string)rdr[0] + ".dbo.A21OPInvProductDesc h WITH (NOLOCK) on h.trntype = a.trntype and h.trnno = a.trnno " +
								   "where c.Code LIKE '" + accountCode + "' AND a.trntype IN ('CY','CU') AND " +
								   "c.CName LIKE '" + accountName + "' AND " +
								   "a.product LIKE '" + productCode + "' AND " +
								   "a.proddesc LIKE '" + productName + "' AND " +
								   "f.Class1 LIKE '" + productGroupCode + "' AND " +
								   "e.Description LIKE '" + productGroupName + "' AND " +
								   "d.trndate >= '" + dateFrom + "' AND d.trndate <= '" + dateTo + "' AND d.TrnType IN ('CY','CU') AND " +
								   "d.DocNo LIKE '" + donumber + "' AND ";

					if (customerPO.Trim() != "%%")
					{
						cmdString += "g.custponumber LIKE '" + customerPO + "' AND ";
					}

					if (productDetailDesc.Trim() != "%%")
						cmdString += "h.Description LIKE '" + productDetailDesc + "' AND ";
					else
						cmdString += "(h.Description LIKE '" + productDetailDesc + "' OR h.Description is NULL ) AND ";

					cmdString += "c.salesperson IN (" + salesPersonsID + ")";

					i++;

				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
			  
								   

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetDeliveryOrderByTrnNo
		[WebMethod]
		public DataSet GetDeliveryOrderByTrnNo(short companyId, string trnNo, string trnType,string salesPersonsID,short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.POTrnNo as SONo, e.ename as salespersonname, b.TrnNo, d.TrnDate, d.salesperson,d.narration, b.dadd1 as DeliveryAddress1, b.dadd2 as DeliveryAddress2, b.dadd3 as DeliveryAddress3, b.dadd4 as DeliveryAddress4, d.currency as Currency, d.docno as DocNo, d.gst, d.billamt ,c.mobile, c.telephone1 as OfficePhone, c.fax, c.email,c.contact, c.CName as AccountName, c.Code as AccountCode , g.custponumber as CustPONo " +
								   "from A21OPInvoiceHeader b WITH (NOLOCK) " +
								   "inner join (select distinct trntype, trnno, POTrnNo from A21OPInvoiceDetail WITH (NOLOCK) ) as a on a.trntype = b.trntype and a.trnno = b.trnno " +
								   "inner join A21GLTransactionHeader d WITH (NOLOCK) on d.company = b.company and d.TrnType = b.TrnType and d.TrnNo = b.TrnNo " +
								   "inner join mstcustomersupplier c WITH (NOLOCK) on c.code = d.code " +
								   "inner join mstemployee e WITH (NOLOCK) on e.employee = d.salesperson " +
								   "left join (select distinct do.trnno, custponumber from a21opinvoicedetail do WITH (NOLOCK) , a21oporderheader so WITH (NOLOCK) where do.trntype = 'CY' and do.potrntype = so.trntype and do.potrnno = so.trnno ) g on a.trnno = g.trnno and a.trntype = 'CY' " +
								   "where b.TrnNo = '" + trnNo + "' and b.TrnType = '" + trnType + "' AND " + 
								   "c.salesperson IN (" + salesPersonsID + ")";                

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetDeliveryOrderDetails
		[WebMethod]
		public DataSet GetDeliveryOrderDetails(short companyId, string trnNo, string trnType, string salesPersonsID, short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.product as ProductCode,a.proddesc as ProductDescription, d.currency as Currency, a.uom, a.TrnNo, a.unitprice as UnitPrice,a.LineDiscount1 as Discount, d.TrnDate, a.Quantity, d.SalesPerson, c.CName as AccountName, a.SrNo, a.DOType, a.DONo " +
									"from A21OPInvoiceDetail a WITH (NOLOCK) " +
									"inner join A21OPInvoiceHeader b WITH (NOLOCK) on a.trntype = b.trntype and a.trnno = b.trnno " +
									"inner join A21GLTransactionHeader d WITH (NOLOCK) on d.company = b.company and d.TrnType = b.TrnType and d.TrnNo = b.TrnNo " +
									"inner join mstcustomersupplier c WITH (NOLOCK) on c.code = d.code " +
									"where a.TrnNo = '" + trnNo + "' and b.TrnType = '"+ trnType + "' AND " + 
									"c.salesperson IN (" + salesPersonsID + ")";               


				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetDeliveryProductDesc
		[WebMethod]
		public DataSet GetDeliveryProductDesc(short companyId, string trnNo, string trnType, string srNo, short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select * from A21OPInvProductDesc a WITH (NOLOCK) " +
									"where a.TrnNo = '" + trnNo + "' and a.TrnType = '" + trnType + "' and a.SrNo = '" + srNo + "' order by DescNo ";
			  

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetInvoiceList
		[WebMethod]
		public DataSet GetInvoicelist(short companyId, string accountCode, string accountName, DateTime dateFrom, DateTime dateTo, string salesPersonsID, string productCode, string productName, string productGroupCode, string productGroupName, string invoiceNo, string productDetailDesc)
		{
			string serverName = "";
			string dbName = "";
			string cmdString = "";
			int i = 0;

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName, Seq from tbA21Database where coyId=" + companyId + " order by Seq", serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					if (i > 0)
						cmdString += " UNION ALL ";

					cmdString += "select distinct d.DocNo as DONo, b.TrnNo,b.TrnType, d.trndate, c.CName as AccountName, c.Code as AccountCode, " + rdr[1] + " as DBVersion " +
								   "from " + (string)rdr[0] + ".dbo.A21OPInvoiceHeader b WITH (NOLOCK) " +
								   "inner join " + (string)rdr[0] + ".dbo.A21OPInvoiceDetail a WITH (NOLOCK) on a.trntype = b.trntype and a.trnno = b.trnno " +
								   "inner join " + (string)rdr[0] + ".dbo.A21GLTransactionHeader d WITH (NOLOCK) on d.company = b.company and d.TrnType = b.TrnType and d.TrnNo = b.TrnNo " +
								   "inner join " + (string)rdr[0] + ".dbo.mstcustomersupplier c WITH (NOLOCK) on c.code = d.code " +
								   "inner join " + (string)rdr[0] + ".dbo.MSTProduct f WITH (NOLOCK) on a.product = f.product " +
								   "inner join " + (string)rdr[0] + ".dbo.SUProductGroup e WITH (NOLOCK) on e.Code = f.Class1 " +
								   "left join " + (string)rdr[0] + ".dbo.A21OPInvProductDesc g WITH (NOLOCK) on g.trntype = a.DOType and g.trnno = a.dono " +
								   "where c.Code LIKE '" + accountCode + "' AND " +
								   "c.CName LIKE '" + accountName + "' AND " +
								   "a.product LIKE '" + productCode + "' AND " +
								   "a.proddesc LIKE '" + productName + "' AND " +
								   "f.Class1 LIKE '" + productGroupCode + "' AND " +
								   "e.Description LIKE '" + productGroupName + "' AND " +
								   "d.trndate >= '" + dateFrom + "' AND d.trndate <= '" + dateTo + "' AND d.TrnType IN ('CI','CD','CC','CR') AND " +
								   "d.DocNo LIKE '" + invoiceNo + "' AND ";

					if (productDetailDesc.Trim() != "%%")
						cmdString += "g.Description LIKE '" + productDetailDesc + "' AND ";
					else
						cmdString += "(g.Description LIKE '" + productDetailDesc + "' OR g.Description is NULL ) AND ";

					cmdString += "c.salesperson IN (" + salesPersonsID + ")";

					i++;

				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
			   
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetInvoiceByTrnNo
		[WebMethod]
		public DataSet GetInvoiceByTrnNo(short companyId, string trnNo, string trnType,string salesPersonsID,short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select e.ename as salespersonname, b.TrnNo, d.TrnDate, d.salesperson,d.narration, b.dadd1 as DeliveryAddress1, b.dadd2 as DeliveryAddress2, b.dadd3 as DeliveryAddress3, b.dadd4 as DeliveryAddress4, d.currency as Currency, d.docno as DocNo, d.gst, d.billamt ,c.mobile, c.telephone1 as OfficePhone, c.fax, c.email,c.contact, c.CName as AccountName, c.Code as AccountCode " +
								   "from A21OPInvoiceHeader b WITH (NOLOCK) " +
								   "inner join A21GLTransactionHeader d WITH (NOLOCK) on d.company = b.company and d.TrnType = b.TrnType and d.TrnNo = b.TrnNo " +
								   "inner join mstcustomersupplier c WITH (NOLOCK) on c.code = d.code " +
								   "inner join mstemployee e WITH (NOLOCK) on e.employee = d.salesperson " +
								   "where b.TrnNo = '" + trnNo + "' and b.TrnType ='" + trnType + "' AND " +
								   "c.salesperson IN (" + salesPersonsID + ")";              


				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetInvoiceDetails
		[WebMethod]
		public DataSet GetInvoiceDetails(short companyId, string trnNo, string trnType, string salesPersonsID, short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select a.product as ProductCode,a.proddesc as ProductDescription, d.currency as Currency, a.uom, a.TrnNo, a.unitprice as UnitPrice,a.LineDiscount1 as Discount, d.TrnDate, a.Quantity, d.SalesPerson, c.CName as AccountName, a.SrNo, a.DOType, a.DONo " +
									"from A21OPInvoiceDetail a WITH (NOLOCK) " +
									"inner join A21OPInvoiceHeader b WITH (NOLOCK) on a.trntype = b.trntype and a.trnno = b.trnno " +
									"inner join A21GLTransactionHeader d WITH (NOLOCK) on d.company = b.company and d.TrnType = b.TrnType and d.TrnNo = b.TrnNo " +
									"inner join mstcustomersupplier c WITH (NOLOCK) on c.code = d.code " +
									"where a.TrnNo = '" + trnNo + "' and b.TrnType = '" + trnType + "' AND " +
									"c.salesperson IN (" + salesPersonsID + ")  ";             


				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion
		
		#region GetInvoiceProductDesc
		[WebMethod]
		public DataSet GetInvoiceProductDesc(short companyId, string trnNo, string trnType, string srNo, short dbVersion)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			try
			{
				// 2015-11-07 Construct Query              
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select DBName from tbA21Database where coyId=" + companyId + " AND Seq = " + dbVersion, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					dbName = (string)rdr[0];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}


			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select * from A21OPInvProductDesc a WITH (NOLOCK) " +
									"where a.TrnNo = '" + trnNo + "' and a.TrnType = '" + trnType + "' and a.SrNo = '" + srNo + "' ";               


				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion
		
		#region GetBankUtilisationFromA21
		[WebMethod]
		public DataSet GetBankUtilisationFromA21(short companyId, DateTime dateFrom, DateTime dateTo)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select i.trntype, i.trnno, SUM(i.creditamount) as creditamount ,SUM(i.debitamount) as debitamount, MAX(g.trndate) as trndate, MAX(p.cname) as cname , MAX(p.code) as acctcode , MAX(g.chqno) as mode, MAX(g.currency) as currency, i.accountcode as bankcoa, right(left(i.accountcode,6),2) as bankdenoted, max(g.exchangerate) as exchangerate " +
								   "from A21GLTransactionDetail i " +
								   "inner join A21GLTransactionHeader g on g.company = i.company and i.TrnType = g.TrnType and i.TrnNo = g.TrnNo " +
								   "inner join mstcustomersupplier p on p.code = g.code " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' ) AND " +
								   "g.trndate >= '" + dateFrom + "' AND g.trndate <= '" + dateTo + "' AND " +
								   "left(i.AccountCode,2) IN (10) " +
								   "group by i.trntype,i.trnno,i.accountcode  " +
								   "order by i.trnno"; 

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion
		
		#region GetBankAccountFromA21
		[WebMethod]
		public DataSet GetBankAccountFromA21(short companyId)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select distinct(i.accountcode) as bankcoa, left(i.AccountCode,2) as code, right(left(i.accountcode,6),2) as bankdenoted, right(left(i.accountcode,4),2) as BankNumericCode " +
								   "from A21GLTransactionDetail i " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO' ) AND " +
								   "left(i.AccountCode,2) IN (10)";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetBankAccountCOA10BalanceFromA21
		[WebMethod]
		public DataSet GetBankAccountCOA10BalanceFromA21(short companyId, string bankCOA, string strCurrency)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, (SUM(DebitAmount) - SUM(CreditAmount))as Amount, (SUM(DefaultDebitAmount) - SUM(DefaultCreditAmount))as DefaultAmount from ( " +
								   "select i.AccountCode as AccountCode, SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, SUM(DebitAmount*g.exchangerate) as DefaultDebitAmount, SUM(CreditAmount*g.exchangerate) as DefaultCreditAmount " +
								   "from A21GLTransactionDetail i " +
								   "inner join A21GLTransactionHeader g on g.company = i.company and i.TrnType = g.TrnType and i.TrnNo = g.TrnNo " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO') AND i.AccountCode = '" + bankCOA + "' AND g.currency = '" + strCurrency + "' " +
								   "group by i.AccountCode " +
								   "UNION " +
								   "select i.AccountCode as AccountCode, SUM(DebitAmount*g.exchangerate) as DebitAmount, SUM(CreditAmount*g.exchangerate) as CreditAmount, SUM(DebitAmount*g.exchangerate) as DefaultDebitAmount, SUM(CreditAmount*g.exchangerate) as DefaultCreditAmount " +
								   "from A21GLTransactionDetail i " +
								   "inner join A21GLTransactionHeader g on g.company = i.company and i.TrnType = g.TrnType and i.TrnNo = g.TrnNo " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO') AND i.AccountCode = '" + bankCOA + "' AND g.currency != '" + strCurrency + "' " +
								   "group by i.AccountCode " +
								   ") as x " +
								   "group by AccountCode";

				

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion
		
		#region GetBankAccountCOAUtilisedBalanceFromA21
		[WebMethod]
		public DataSet GetBankAccountCOAUtilisedBalanceFromA21(short companyId, string bankCOA, string strCurrency)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, (SUM(DebitAmount) - SUM(CreditAmount))as Amount,(SUM(DefaultDebitAmount) - SUM(DefaultCreditAmount))as DefaultAmount from ( " +
								   "select i.AccountCode as AccountCode, SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, SUM(DebitAmount*g.exchangerate) as DefaultDebitAmount, SUM(CreditAmount*g.exchangerate) as DefaultCreditAmount " +
								   "from A21GLTransactionDetail i " +
								   "inner join A21GLTransactionHeader g on g.company = i.company and i.TrnType = g.TrnType and i.TrnNo = g.TrnNo " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO') AND i.AccountCode = '" + bankCOA + "' AND g.currency = '" + strCurrency + "' " +
								   "group by i.AccountCode " +
								   "UNION " +
								   "select i.AccountCode as AccountCode, SUM(DebitAmount*g.exchangerate) as DebitAmount, SUM(CreditAmount*g.exchangerate) as CreditAmount, SUM(DebitAmount*g.exchangerate) as DefaultDebitAmount, SUM(CreditAmount*g.exchangerate) as DefaultCreditAmount " +
								   "from A21GLTransactionDetail i " +
								   "inner join A21GLTransactionHeader g on g.company = i.company and i.TrnType = g.TrnType and i.TrnNo = g.TrnNo " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO') AND i.AccountCode = '" + bankCOA + "' AND g.currency != '" + strCurrency + "' " +
								   "group by i.AccountCode " +
								   ") as x " +
								   "group by AccountCode";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetPODetailFromA21
		[WebMethod]
		public DataSet GetPODetailFromA21(short companyId, string poNO)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select b.SrNo,a.DocNo as PONo,a.trndate as PODate, a.TrnNo, a.Code as AccountCode, a.TrnDate, " +
									"b.Product as ProductCode, b.prodDesc as ProductDescription, b.UOM as uom,b.OrderQuantity as Quantity, b.DelQuantity, " +
									"b.ShortQuantity,  b.UnitPrice as UnitPrice, LineDiscount1 as Discount, " +
									"a.Currency, a.ExchangeRate, a.GST " +
									"from a21poorderheader a " +
									"inner join a21poorderdetail b on a.trnno = b.trnno " +
									"where a.DocNo = '" + poNO + "' " +
									"order by b.SrNo";


								   

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion
		
		#region GetGRNDetailFromA21
		[WebMethod]
		public DataSet GetGRNDetailFromA21(short companyId, string GRNNo)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select ph.docno as PONo, ph.trndate as PODate, g.docno as GRNNo, g.trndate as GRNDate," +
								   "p.product as ProductCode, p.prodDesc as ProductDescription, p.uom as uom, p.Warehouse as WH, g.Currency, g.ExchangeRate, p.UnitPrice, p.LanderCostUnitPrice, p.cost as TotalCost,  p.Quantity " +
								   "from a21opinvoicedetail p, a21gltransactionheader g, a21poorderheader ph " +
								   "where p.trntype = g.trntype and p.trnno = g.trnno " +
								   "and p.potrntype = ph.trntype and p.potrnno = ph.trnno " +
								   "and g.docno = '" + GRNNo + "'";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetGRNHeaderFromA21
		[WebMethod]
		public DataSet GetGRNHeaderFromA21(short companyId, string GRNNo)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select g.TrnType, g.TrnNo, g.TrnDate, g.DocNo, g.Code,m.CName, m.Add1, m.Add2, m.Add3, m.contact from a21opinvoiceheader p, a21gltransactionheader g, MSTCustomerSupplier m " + 
								   "where p.trntype = g.trntype and p.trnno = g.trnno " + 
								   "and m.Code = g.Code " +
								   "and g.docno = '" + GRNNo + "'";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetMRGRNDetailFromA21
		[WebMethod]
		public DataSet GetMRGRNDetailFromA21(short companyId)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select distinct ISNULL(p.product,'') as Product, ph.upduser as Purchaser, ISNULL(ph.docno,'') as PONo, ph.trndate as PODate, ISNULL(g.docno,'') as GRNNo, p.trntype, p.trnno , g.trndate as GRNDate, ISNULL(sum(p.Quantity),0) as Quantity " +
								   "from a21opinvoicedetail p, a21gltransactionheader g, a21poorderheader ph " +
								   "where p.trntype = g.trntype and p.trnno = g.trnno  " +
								   "and p.potrntype = ph.trntype and p.potrnno = ph.trnno  " +
								   "and p.product not like 'Z%' " +
								   "and g.docno is not null and ph.docno is not null " +
								   "group by p.product, ph.upduser,ph.docno, ph.trndate, g.docno, p.trntype, p.trnno, g.trndate ";                

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetPOHeaderFromA21
		[WebMethod]
		public DataSet GetPOHeaderFromA21(short companyId, string poNO)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select ph.upduser as purchaser, ph.docno, ph.trntype, ph.trnno, ph.trndate, ph.delmode, ph.billamt as billamt, ph.gst as gst, ph.Code, m.CName, m.Add1, m.Add2, m.Add3, m.contact, ph.Currency, p.description as SupPayTerm " +
								   "from a21poorderheader ph,MSTCustomerSupplier m, SUTermOfPayment p " +
								   "where m.Code = ph.Code and p.Code = m.SupPayTerm " +
								   "and ph.docno = '" + poNO + "'";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion
		
		#region GetProductFromA21
		[WebMethod]
		public DataSet GetProductFromA21(short companyId)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select a.Product , a.PName, a.Class1 , a.Volume , a.purchaseunit , a.costwt, " +
								   "a.OnOrderQuantity , a.OnPOQuantity " +
								   "from MSTProduct a ";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetBankAccountCOA10BalanceFromA21HQ
		[WebMethod]
		public DataSet GetBankAccountCOA10BalanceFromA21HQ(short companyId, string bankCOA, string strCurrency)
		{
			string serverName = "";
			string dbName = "";
			string hqserverName = "";
			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select HQServerName, DBName, HQServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
					
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, (SUM(DebitAmount) - SUM(CreditAmount))as Amount, (SUM(DefaultDebitAmount) - SUM(DefaultCreditAmount))as DefaultAmount from ( " +
								   "select i.AccountCode as AccountCode, SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, SUM(DebitAmount*g.exchangerate) as DefaultDebitAmount, SUM(CreditAmount*g.exchangerate) as DefaultCreditAmount " +
								   "from A21GLTransactionDetail i " +
								   "inner join A21GLTransactionHeader g on g.company = i.company and i.TrnType = g.TrnType and i.TrnNo = g.TrnNo " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO') AND i.AccountCode = '" + bankCOA + "' " +
								   "group by i.AccountCode " +                                  
								   ") as x " +
								   "group by AccountCode";



				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetBankAccountCOAUtilisedBalanceFromA21HQ
		[WebMethod]
		public DataSet GetBankAccountCOAUtilisedBalanceFromA21HQ(short companyId, string bankCOA, string strCurrency)
		{
			string serverName = "";
			string dbName = "";
			string hqserverName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select HQServerName, DBName, HQServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
					
					
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();              


				string cmdString = "select SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, (SUM(CreditAmount) - SUM(DebitAmount))as Amount,(SUM(DefaultCreditAmount) - SUM(DefaultDebitAmount))as DefaultAmount from ( " +
								   "select i.AccountCode as AccountCode, SUM(DebitAmount) as DebitAmount, SUM(CreditAmount) as CreditAmount, SUM(DebitAmount*g.exchangerate) as DefaultDebitAmount, SUM(CreditAmount*g.exchangerate) as DefaultCreditAmount " +
								   "from A21GLTransactionDetail i " +
								   "inner join A21GLTransactionHeader g on g.company = i.company and i.TrnType = g.TrnType and i.TrnNo = g.TrnNo " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO') AND i.AccountCode = '" + bankCOA + "' " +
								   "group by i.AccountCode " +                                   
								   ") as x " +
								   "group by AccountCode";     
				
				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetBankAccountFromA21HQ
		[WebMethod]
		public DataSet GetBankAccountFromA21HQ(short companyId)
		{
			string serverName = "";
			string hqserverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select HQServerName, DBName, HQServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				   

				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select distinct(i.accountcode) as bankcoa, left(i.AccountCode,2) as code, right(left(i.accountcode,6),2) as bankdenoted, right(left(i.accountcode,4),2) as BankNumericCode " +
								   "from A21GLTransactionDetail i " +
								   "where (i.trntype = 'PV' or i.trntype = 'RR' or i.trntype = 'GJ' or i.trntype = 'GO' ) AND " +
								   "left(i.AccountCode,2) IN (10,11,30,31,33,32,34,41,35,42)";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;


		}
		#endregion

		#region GetPOFromA21ForWMS
		[WebMethod]
		public DataSet GetPOFromA21ForWMS(short companyId, string poNO)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select distinct b.DocNo as PONo, b.TrnDate as PODate, a.TrnNo as TrnNo, '' as TrnType, '' as SrNo, a.Product as ProductCode, c.pname as ProductDescription, c.StockUnit as UOM, a.IssueQuantity as OrderQuantity, a.ReceiptQuantity as ReceivedQty, '' as DelQuantity, '' as ShortQuantity, d.CName as AccountName, d.Code as AccountCode " +
									"from A21IMTransactionDetail  a " +
									"inner join A21GLTransactionHeader b on b.company = a.company and b.TrnType = a.TrnType and b.TrnNo = a.TrnNo " +
									"inner join MSTProduct c on a.product = c.product " +
									"inner join mstcustomersupplier d on d.code = b.code " +
									"where a.TrnType = 'IM' and a.Product NOT LIKE 'Z%' and b.DocNo = '" + poNO + "'union all " +

									"select distinct b.DocNo as PONo, b.TrnDate as PODate, a.TrnNo as TrnNo, a.TrnType as TrnType, '' as SrNo, a.Product as ProductCode, a.ProdDesc as ProductDescription, a.UOM as UOM, a.Quantity as OrderQuantity, '' as ReceivedQty, '' as DelQuantity, '' as ShortQuantity, c.CName as AccountName, c.Code as AccountCode " +
									"from A21OPInvoiceDetail a " +
									"inner join A21GLTransactionHeader b on b.company = a.company and b.TrnType = a.TrnType and b.TrnNo = a.TrnNo " +
									"inner join mstcustomersupplier c on c.code = b.code " +
									"where a.Product NOT LIKE 'Z%' and a.TrnType in ('CC') and b.DocNo = '" + poNO + "' union all " +

									"select max(a.DocNo) as PONo, max(a.TrnDate) as PODate, max(a.TrnNo) as TrnNo, max(a.TrnType) as TrnType, " +
									"min(b.SrNo) as SrNo, max(b.Product) as ProductCode, b.ProdDesc as ProductDescription, max(b.UOM) as UOM , sum(b.OrderQuantity) as OrderQuantity, '' as ReceivedQty, sum(b.DelQuantity) as DelQuantity, sum(b.ShortQuantity) as ShortQuantity, '' as AccountName, '' as AccountCode " +
									"from A21POOrderHeader a " +
									"inner join A21POOrderDetail b on a.TrnNo = b.TrnNo " +
									"where b.Product NOT LIKE 'Z%' and a.DocNo = '" + poNO + "' " +
									"group by b.Product, b.ProdDesc " +
									"order by b.SrNo";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		#region GetDOFromA21ForWMS
		[WebMethod]
		public DataSet GetDOFromA21ForWMS(short companyId, string doNO)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString = "select b.DocNo as DONo, b.TrnDate as DODate, a.Product as ProductCode, a.ProdDesc as ProductDescription, a.UOM as UOM, a.TrnNo, a.Quantity as IssueQty, '' as ReceiveQty, c.CName as AccountName, c.Code as AccountCode " +
									"from A21OPInvoiceDetail a " +
									"inner join A21GLTransactionHeader b on b.company = a.company and b.TrnType = a.TrnType and b.TrnNo = a.TrnNo " +
									"inner join mstcustomersupplier c on c.code = b.code " +
									"where a.Product NOT LIKE 'Z%' and a.TrnType in ('CY', 'CU') and b.DocNo = '" + doNO + "' union all " +

									"select b.DocNo as DONo, b.TrnDate as DODate, a.Product as ProductCode, c.pname as ProductDescription, c.StockUnit as UOM, a.TrnNo, a.IssueQuantity as IssueQty, a.ReceiptQuantity as ReceiveQty, d.CName as AccountName, d.Code as AccountCode " +
									"from A21IMTransactionDetail  a " +
									"inner join A21GLTransactionHeader b on b.company = a.company and b.TrnType = a.TrnType and b.TrnNo = a.TrnNo " +
									"inner join MSTProduct c on a.product = c.product " +
									"inner join mstcustomersupplier d on d.code = b.code " +
									"where a.TrnType = 'IM' and a.Product NOT LIKE 'Z%' and b.DocNo = '" + doNO + "'";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		#region GetSOFromA21ForWMS
		[WebMethod]
		public DataSet GetSOFromA21ForWMS(short companyId, string soNO)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();

				string cmdString =  "select b.TrnNo as DONo, b.TrnDate as DODate, a.Product as ProductCode, a.ProdDesc as ProductDescription, a.UOM as UOM, a.TrnNo, a.OrderQuantity as IssueQty, '' as ReceiveQty, c.CName as AccountName, c.Code as AccountCode " +
									"from A21OPOrderDetail a " +
									"INNER JOIN A21OPOrderHeader b on b.Company = a.Company AND b.TrnType = a.TrnType AND b.TrnNo = a.TrnNo " +
									"INNER JOIN MSTCustomerSupplier c on c.Code = b.Code " +
									"where a.Product NOT LIKE 'Z%' AND a.Trntype = 'CO' AND b.TrnNo = '" + soNO + "'";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		}
		#endregion

		#region GetAllProduct
		[WebMethod]
		public DataSet GetAllProduct(short companyId)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select ISNULL(a.Product,'') as ProductCode, ISNULL(a.PName,'') as ProductName, " +
								   " ISNULL(a.Class1,'') as ProductGroupCode, a.Volume as Volume, ISNULL(a.purchaseunit,'') as UOM, a.costwt as WeightedCost, " +
								   " isnull(b.HQ,0) as WarehouseHQ, isnull(b.[77],0) as [Warehouse77], isnull(b.Others,0) as WarehouseOthers, " +
								   " a.OnOrderQuantity as OnSOQty, a.OnPOQuantity as OnPOQty " +
								   " from MSTProduct a left join " +
								   " (select b.company, b.product, sum(case when b.warehouse = '22' then quantity else 0 end) as 'HQ', sum(case when b.warehouse = '77' then quantity else 0 end) as '77', " +
								   " sum(case when b.warehouse <> '22' and b.warehouse <> '77' then quantity else 0 end) as 'Others'" +
								   " from a21imwarehousestock b " +
								   " group by b.company, b.product) b " +
								   " on a.company = b.company and a.product = b.product ";


				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;
		   
		}
		#endregion

		#region GetAllAccount
		[WebMethod]
		public DataSet GetAllAccount(short companyId)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = "select Code as AccountCode, Type as AccountType, CName as AccountName, SalesPerson, Currency as DefaultCurrency, " +
								   " CusClass1 as IndustrySector, Remarks as RemarksShort, GSTType, CusCreditPeriod as CreditTerm, CusCreditLimit as CreditLimit, " +
								   " CusCreditUsed as CreditUsed, Contact as ContactPerson, Add1 as Address1, Add2 as Address2, Add3 as Address3, Add4 as Address4, " +
								   " Pin as PostalCode,  Telephone1 as OfficePhone, Mobile as MobilePhone, Fax, Email " +
								   " from MSTCustomerSupplier ";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

		}
		#endregion

		#region GetProductExtendedDescriptionByProductCode
		[WebMethod]
		public DataSet GetProductExtendedDescriptionByProductCode(short companyId, string productCode)
		{
			string serverName = "";
			string dbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					A21Pwd = (string)rdr[2];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}

			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
			   
				string cmdString = "select Product, DescNo, ProdDetail from MSTProductDetail " +
								   "where ProdDetail is not NULL AND Product = '" + productCode + "'" +
								   "order by DescNo ";



				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

		}
		#endregion

		#region GetTrialBalance
		[WebMethod]
		public DataSet GetTrialBalance(short companyId, short year, short month, short FYE,  bool Is2016COA)
		{
			string serverName = "";
			string dbName = "";
			string olddbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, case when (YEAR(Is2016COA)*100)+MONTH(Is2016COA)<=("+ year + "*100)+"+ month + " then dbname else OldDBName end as OldDBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					olddbName = (string)rdr[2];
					A21Pwd = (string)rdr[3];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}
			if (Is2016COA)
			{
				olddbName = dbName;
			}


			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + olddbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = @"
				DECLARE @FinancialYear smallint, @FinancialMonth smallint, @FYE smallint
				SET @FinancialYear = " + year.ToString() + "SET @FinancialMonth = " + month.ToString() + "SET @FYE = " + FYE.ToString() +

				@"
				SELECT '" + dbName + @"' AS DBName, AccountCode, -1 AS Project, -1 AS Department, -1 AS Section, 
				SUM(PrevBalance) AS PrevBalance, SUM(DebitAmount) AS Debit, SUM(CreditAmount) AS Credit
				FROM
				(
				SELECT 
				A21GLTransactionDetail.AccountCode, 
				CAST( (([A21GLTransactionDetail].[DebitAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[DebitBase])  +    
					  (([A21GLTransactionDetail].[CreditAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[CreditBase]) * -1 
				AS DECIMAL(20,2)) 
				AS PrevBalance, 
				0 AS DebitAmount, 0 AS CreditAmount
				FROM ((A21GLTransactionDetail 
					INNER JOIN A21GLTransactionHeader 
						ON (A21GLTransactionDetail.TrnNo = A21GLTransactionHeader.TrnNo) 
						AND (A21GLTransactionDetail.TrnType = A21GLTransactionHeader.TrnType) 
						AND (A21GLTransactionDetail.Company = A21GLTransactionHeader.Company)) 
					INNER JOIN MSTCompany 
						ON A21GLTransactionHeader.Company = MSTCompany.Code) 
					INNER JOIN MSTChartAccount 
						ON A21GLTransactionDetail.AccountCode = MSTChartAccount.Account
				WHERE 	(A21GLTransactionHeader.Year >= 1990 
						AND NOT (A21GLTransactionHeader.Year >= @FinancialYear AND A21GLTransactionHeader.Month >= @FinancialMonth) 
						AND NOT (A21GLTransactionHeader.Year > @FinancialYear))
				AND MSTChartAccount.Type in ('BA','BL')

				UNION ALL

				SELECT 
				A21GLTransactionDetail.AccountCode, 0 AS PrevBalance, 
				CAST( ([A21GLTransactionDetail].[DebitAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[DebitBase] AS DECIMAL(20,2)) AS DebitAmount, 
				CAST( ([A21GLTransactionDetail].[CreditAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[CreditBase] AS DECIMAL(20,2)) AS CreditAmount
				FROM ((A21GLTransactionDetail 
					INNER JOIN A21GLTransactionHeader 
						ON (A21GLTransactionDetail.TrnNo = A21GLTransactionHeader.TrnNo) 
						AND (A21GLTransactionDetail.TrnType = A21GLTransactionHeader.TrnType) 
						AND (A21GLTransactionDetail.Company = A21GLTransactionHeader.Company)) 
					INNER JOIN MSTCompany 
						ON A21GLTransactionHeader.Company = MSTCompany.Code) 
					INNER JOIN MSTChartAccount 
						ON A21GLTransactionDetail.AccountCode = MSTChartAccount.Account
				WHERE 	(A21GLTransactionHeader.Year = @FinancialYear AND A21GLTransactionHeader.Month = @FinancialMonth)
				AND MSTChartAccount.Type in ('BA','BL','PI','PE')

				UNION ALL

				SELECT 
				A21GLTransactionDetail.AccountCode, 
				CAST( (([A21GLTransactionDetail].[DebitAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[DebitBase])  +    
					  (([A21GLTransactionDetail].[CreditAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[CreditBase]) * -1 
				AS DECIMAL(20,2)) 
				AS PrevBalance, 
				0 AS DebitAmount, 0 AS CreditAmount
				FROM ((A21GLTransactionDetail 
					INNER JOIN A21GLTransactionHeader 
						ON (A21GLTransactionDetail.TrnNo = A21GLTransactionHeader.TrnNo) 
						AND (A21GLTransactionDetail.TrnType = A21GLTransactionHeader.TrnType) 
						AND (A21GLTransactionDetail.Company = A21GLTransactionHeader.Company)) 
					INNER JOIN MSTCompany 
						ON A21GLTransactionHeader.Company = MSTCompany.Code) 
					INNER JOIN MSTChartAccount 
						ON A21GLTransactionDetail.AccountCode = MSTChartAccount.Account
				WHERE ((@FYE!=12 AND A21GLTransactionHeader.Year = @FinancialYear AND A21GLTransactionHeader.Month < @FinancialMonth)
						OR
					  (@FYE=12 AND (((@FinancialYear<2016 OR (@FinancialYear=2016 AND @FinancialMonth<4)) 
									AND ((A21GLTransactionHeader.Year = @FinancialYear AND A21GLTransactionHeader.Month < @FinancialMonth)
										OR (A21GLTransactionHeader.Year = @FinancialYear-1 AND @FYE=12 AND @FinancialYear=2016 AND @FinancialMonth<4)))
								OR ((@FinancialYear>2016 OR (@FinancialYear=2016 AND @FinancialMonth>=4)) 
									AND ((@FinancialMonth BETWEEN 4 AND 12 AND (A21GLTransactionHeader.Year=@FinancialYear AND A21GLTransactionHeader.Month>= 4 AND A21GLTransactionHeader.Month < @FinancialMonth))
										OR (@FinancialMonth BETWEEN 1 AND 3 AND ((A21GLTransactionHeader.Year=@FinancialYear-1) OR (A21GLTransactionHeader.Year=@FinancialYear AND A21GLTransactionHeader.Month < @FinancialMonth))))))))
				AND MSTChartAccount.Type in ('PI','PE')

				) AS B
				GROUP BY AccountCode
				ORDER BY AccountCode 
				"; 

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				cmd.CommandTimeout = 3000;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

		}
		#endregion

		#region GetTrialBalanceForPDS
		[WebMethod]
		public DataSet GetTrialBalanceForPDS(short companyId, short year, short month, short FYE, bool Is2016COA)
		{
			string serverName = "";
			string dbName = "";
			string olddbName = "";

			string connString = ConfigurationSettings.AppSettings["DBConnection"];
			SqlConnection serverConn = new SqlConnection(connString);
			SqlDataReader rdr = null;

			try
			{
				serverConn.Open();
				SqlCommand cmd = new SqlCommand("select ServerName, DBName, case when (YEAR(Is2016COA)*100)+MONTH(Is2016COA)<=(" + year + "*100)+" + month + " then dbname else OldDBName end as OldDBName, ServerPwd from tbCompany where coyId=" + companyId, serverConn);
				rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					serverName = (string)rdr[0];
					dbName = (string)rdr[1];
					olddbName = (string)rdr[2];
					A21Pwd = (string)rdr[3];
				}
			}
			finally
			{
				if (rdr != null)
				{
					rdr.Close();
				}

				if (serverConn != null)
				{
					serverConn.Close();
				}
			}
			if (Is2016COA)
			{
				olddbName = dbName;
			}
			SqlConnection a21Conn = new SqlConnection(
			"Data Source=" + serverName + ";Initial Catalog=" + olddbName + ";user id=sa; PWD=" + A21Pwd + ";Connect Timeout=100;");

			DataSet ds = new DataSet();
			try
			{
				a21Conn.Open();
				string cmdString = @"
				DECLARE @FinancialYear smallint, @FinancialMonth smallint, @FYE smallint
				SET @FinancialYear = " + year.ToString() + "SET @FinancialMonth = " + month.ToString() + "SET @FYE = " + FYE.ToString() +

				@"
				SELECT '" + dbName + @"' AS DBName, AccountCode, Project, Department, Section, 
				SUM(PrevBalance) AS PrevBalance, SUM(DebitAmount) AS Debit, SUM(CreditAmount) AS Credit
				FROM
				(
				SELECT 
				A21GLTransactionDetail.AccountCode, 
				A21GLTransactionDetail.Project, A21GLTransactionDetail.Department, A21GLTransactionDetail.Section, 
				CAST( (([A21GLTransactionDetail].[DebitAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[DebitBase])  +    
					  (([A21GLTransactionDetail].[CreditAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[CreditBase]) * -1 
				AS DECIMAL(20,2)) 
				AS PrevBalance, 
				0 AS DebitAmount, 0 AS CreditAmount
				FROM ((A21GLTransactionDetail 
					INNER JOIN A21GLTransactionHeader 
						ON (A21GLTransactionDetail.TrnNo = A21GLTransactionHeader.TrnNo) 
						AND (A21GLTransactionDetail.TrnType = A21GLTransactionHeader.TrnType) 
						AND (A21GLTransactionDetail.Company = A21GLTransactionHeader.Company)) 
					INNER JOIN MSTCompany 
						ON A21GLTransactionHeader.Company = MSTCompany.Code) 
					INNER JOIN MSTChartAccount 
						ON A21GLTransactionDetail.AccountCode = MSTChartAccount.Account
				WHERE 	(A21GLTransactionHeader.Year >= 1990 AND NOT(A21GLTransactionHeader.Year >= @FinancialYear AND A21GLTransactionHeader.Month >= @FinancialMonth) AND NOT (A21GLTransactionHeader.Year > @FinancialYear))
				AND MSTChartAccount.Type in ('BA','BL')

				UNION ALL

				SELECT 
				A21GLTransactionDetail.AccountCode, 
				A21GLTransactionDetail.Project, A21GLTransactionDetail.Department, A21GLTransactionDetail.Section, 
				0 AS PrevBalance, 
				CAST( ([A21GLTransactionDetail].[DebitAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[DebitBase] AS DECIMAL(20,2)) AS DebitAmount, 
				CAST( ([A21GLTransactionDetail].[CreditAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[CreditBase] AS DECIMAL(20,2)) AS CreditAmount
				FROM ((A21GLTransactionDetail 
					INNER JOIN A21GLTransactionHeader 
						ON (A21GLTransactionDetail.TrnNo = A21GLTransactionHeader.TrnNo) 
						AND (A21GLTransactionDetail.TrnType = A21GLTransactionHeader.TrnType) 
						AND (A21GLTransactionDetail.Company = A21GLTransactionHeader.Company)) 
					INNER JOIN MSTCompany 
						ON A21GLTransactionHeader.Company = MSTCompany.Code) 
					INNER JOIN MSTChartAccount 
						ON A21GLTransactionDetail.AccountCode = MSTChartAccount.Account
				WHERE 	(A21GLTransactionHeader.Year = @FinancialYear AND A21GLTransactionHeader.Month = @FinancialMonth)
				AND MSTChartAccount.Type in ('BA','BL','PI','PE')

				UNION ALL

				SELECT 
				A21GLTransactionDetail.AccountCode, 
				A21GLTransactionDetail.Project, A21GLTransactionDetail.Department, A21GLTransactionDetail.Section, 
				CAST( (([A21GLTransactionDetail].[DebitAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[DebitBase])  +    
					  (([A21GLTransactionDetail].[CreditAmount]*[A21GLTransactionHeader].[ExchangeRate])+[A21GLTransactionDetail].[CreditBase]) * -1 
				AS DECIMAL(20,2)) 
				AS PrevBalance, 
				0 AS DebitAmount, 0 AS CreditAmount
				FROM ((A21GLTransactionDetail 
					INNER JOIN A21GLTransactionHeader 
						ON (A21GLTransactionDetail.TrnNo = A21GLTransactionHeader.TrnNo) 
						AND (A21GLTransactionDetail.TrnType = A21GLTransactionHeader.TrnType) 
						AND (A21GLTransactionDetail.Company = A21GLTransactionHeader.Company)) 
					INNER JOIN MSTCompany 
						ON A21GLTransactionHeader.Company = MSTCompany.Code) 
					INNER JOIN MSTChartAccount 
						ON A21GLTransactionDetail.AccountCode = MSTChartAccount.Account
				WHERE ((@FYE!=12 AND A21GLTransactionHeader.Year = @FinancialYear AND A21GLTransactionHeader.Month < @FinancialMonth)
						OR
					  (@FYE=12 AND (((@FinancialYear<2016 OR (@FinancialYear=2016 AND @FinancialMonth<4)) 
									AND ((A21GLTransactionHeader.Year = @FinancialYear AND A21GLTransactionHeader.Month < @FinancialMonth)
										OR (A21GLTransactionHeader.Year = @FinancialYear-1 AND @FYE=12 AND @FinancialYear=2016 AND @FinancialMonth<4)))
								OR ((@FinancialYear>2016 OR (@FinancialYear=2016 AND @FinancialMonth>=4)) 
									AND ((@FinancialMonth BETWEEN 4 AND 12 AND (A21GLTransactionHeader.Year=@FinancialYear AND A21GLTransactionHeader.Month>= 4 AND A21GLTransactionHeader.Month < @FinancialMonth))
										OR (@FinancialMonth BETWEEN 1 AND 3 AND ((A21GLTransactionHeader.Year=@FinancialYear-1) OR (A21GLTransactionHeader.Year=@FinancialYear AND A21GLTransactionHeader.Month < @FinancialMonth))))))))
				AND MSTChartAccount.Type in ('PI','PE')

				) AS B
				GROUP BY AccountCode, Project, Department, Section
				ORDER BY AccountCode, Project, Department, Section
				";

				SqlCommand cmd = new SqlCommand(cmdString, a21Conn);
				cmd.CommandType = CommandType.Text;
				cmd.CommandTimeout = 3000;
				SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
				dataAdapter.Fill(ds);
			}
			finally
			{
				if (a21Conn != null)
				{
					a21Conn.Close();
				}
			}
			return ds;

		}
		#endregion
				
	}
}
