using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSCore.Exceptions;

using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class MRActivity : ActivityBase
    {

        public MRActivity()
        {
        }

        public ResultType CreateMR(ref MR mr, LogSession session)
        {
            if (mr == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!mr.IsValid())
                return ResultType.MainDataNotValid;

            mr.Save();

            return ResultType.Ok;            

        }

        #region RetrieveMRByMRNo
        public MR RetrieveMRByMRNo(short companyID, string mrNo)
        {
            
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MR.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MR.CoyID"),
                                helper.CleanValue(companyID));

            return MR.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region DeleteMRConfirmedSales
        public ResultType DeleteMRConfirmedSales(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            MRAttachment ma = RetrieveConfirmedSalesByID(session.CompanyId, ID);
            if (ma == null)
                return ResultType.Error;

            ma.Delete();
            return ResultType.Ok;
        }
        #endregion

        public ResultType DeleteMRConfirmedSales(short companyID, string ID)
        {
            if (ID == null)
                return ResultType.NullMainData;           

            MRAttachment ma = RetrieveConfirmedSalesByID(companyID, ID);
            if (ma == null)
                return ResultType.Error;

            ma.Delete();
            return ResultType.Ok;
        }


        #region DeleteMRAttachment
        public ResultType DeleteMRAttachment(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            MRAdditionalAttachment ma = RetrieveMRAttachementByID(session.CompanyId, ID);
            if (ma == null)
                return ResultType.Error;

            ma.Delete();
            return ResultType.Ok;
        }
        #endregion

        public ResultType DeleteMRAttachment(short companyID, string ID)
        {
            if (ID == null)
                return ResultType.NullMainData;


            MRAdditionalAttachment ma = RetrieveMRAttachementByID(companyID, ID);
            if (ma == null)
                return ResultType.Error;

            ma.Delete();
            return ResultType.Ok;
        }

        
        #region DeleteMRConfirmedSalesWOSession
        public ResultType DeleteMRConfirmedSalesWOSession(string ID, short CompanyId)
        {
            if (ID == null)
                return ResultType.NullMainData;

            

            MRAttachment ma = RetrieveConfirmedSalesByID(CompanyId, ID);
            if (ma == null)
                return ResultType.Error;

            ma.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region DeleteMRDelivery
        public ResultType DeleteMRDelivery(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            MRDelivery md = RetrieveDeliveryByID(session.CompanyId, ID);
            if (md == null)
                return ResultType.Error;

            md.Delete();
            return ResultType.Ok;
        }
        #endregion

        public ResultType DeleteMRDelivery(short companyID, string ID)
        {
            MRDelivery md = RetrieveDeliveryByID(companyID, ID);
            if (md == null)
                return ResultType.Error;

            md.Delete();
            return ResultType.Ok;
        }


        #region DeleteMRDeliveryWOSession
        public ResultType DeleteMRDeliveryWOSession(string ID, short CompanyId)
        {
            if (ID == null)
                return ResultType.NullMainData;            

            MRDelivery md = RetrieveDeliveryByID(CompanyId, ID);
            if (md == null)
                return ResultType.Error;

            md.Delete();
            return ResultType.Ok;
        }
        #endregion

        public MRDelivery RetrieveDeliveryByID(short companyID, string DeliveryID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRDelivery.DeliveryNo"),
                               helper.CleanValue(DeliveryID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDelivery.CoyID"),
                                helper.CleanValue(companyID));

            return MRDelivery.RetrieveFirst(stb.ToString());
        }

        #region DeleteMRVendor
        public ResultType DeleteMRVendor(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            MRVendor mv = RetrieveVendorByID(session.CompanyId, ID);
            if (mv == null)
                return ResultType.Error;

            mv.Delete();
            return ResultType.Ok;
        }
        #endregion

        public ResultType DeleteMRVendor(short companyID, string ID)
        {
            if (ID == null)
                return ResultType.NullMainData;            

            MRVendor mv = RetrieveVendorByID(companyID, ID);
            if (mv == null)
                return ResultType.Error;

            mv.Delete();
            return ResultType.Ok;
        }


        #region DeleteMRVendor
        public ResultType DeleteMRVendorWOSession(string ID, short CompanyId)
        {
            if (ID == null)
                return ResultType.NullMainData;            

            MRVendor mv = RetrieveVendorByID(CompanyId, ID);
            if (mv == null)
                return ResultType.Error;

            mv.Delete();
            return ResultType.Ok;
        }
        #endregion

       
        public MRVendor RetrieveVendorByID(short companyID, string VendorID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRVendor.VendorID"),
                               helper.CleanValue(VendorID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRVendor.CoyID"),
                                helper.CleanValue(companyID));

            return MRVendor.RetrieveFirst(stb.ToString());
        }

        public MRVendor RetrieveVendorByCoyIDMRNoVendorName(short companyID, string mrNo, string vendorName)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRVendor.CoyID"),
                               helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRVendor.MRNo"),
                                helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRVendor.VendorName"),
                               helper.CleanValue(vendorName));

            return MRVendor.RetrieveFirst(stb.ToString());
        }

        #region RetrieveConfirmedSalesByID
        public MRAttachment RetrieveConfirmedSalesByID(short companyID, string FileID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRAttachment.FileID"),
                               helper.CleanValue(FileID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRAttachment.CoyID"),
                                helper.CleanValue(companyID));

            return MRAttachment.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveMRAttachementByID
        public MRAdditionalAttachment RetrieveMRAttachementByID(short companyID, string FileID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRAdditionalAttachment.FileID"),
                               helper.CleanValue(FileID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRAdditionalAttachment.CoyID"),
                                helper.CleanValue(companyID));

            return MRAdditionalAttachment.RetrieveFirst(stb.ToString());
        }
        #endregion

        
        #region RetrieveConfirmedSalesByMRNo
        public IList<MRAttachment> RetrieveConfirmedSalesByMRNo(short companyID, string mrNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRAttachment.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRAttachment.CoyID"),
                                helper.CleanValue(companyID));
            
            return MRAttachment.RetrieveQuery(stb.ToString());
        }
        #endregion
       

        #region RetrieveVendorByMRNo
        public IList<MRVendor> RetrieveVendorByMRNo(short companyID, string mrNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRVendor.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRVendor.CoyID"),
                                helper.CleanValue(companyID));

            return MRVendor.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveMRByStatus
        public IList<MR> RetrieveMRByStatus(short companyID, string statusID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MR.CoyID"),
                               helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MR.StatusID"),
                                helper.CleanValue(statusID));

            return MR.RetrieveQuery(stb.ToString());
        }
        #endregion

    

        #region RetrieveProductByMRNo
        public IList<MRDetail> RetrieveMRProductByMRNo(short companyID, string mrNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRDetail.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDetail.CoyID"),
                                helper.CleanValue(companyID));

            return MRDetail.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveAutoInsertedVendorByMRNo
        public IList<MRVendor> RetrieveAutoInsertedVendorByMRNo(short companyID, string mrNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRVendor.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRVendor.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} IS NOT NULL ", helper.GetFieldName("MRVendor.MRSupplierID"));                           

            return MRVendor.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveMManuallInsertedVendorByMRNo
        public IList<MRVendor> RetrieveManualInsertedVendorByMRNo(short companyID, string mrNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRVendor.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRVendor.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} IS NULL ", helper.GetFieldName("MRVendor.MRSupplierID"));

            return MRVendor.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveMRDeliveryByMRNo
        public IList<MRDelivery> RetrieveMRDeliveryByMRNo(short companyID, string mrNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRDelivery.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDelivery.CoyID"),
                                helper.CleanValue(companyID));

            return MRDelivery.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region DeleteMRProduct
        public ResultType DeleteMRProduct(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            MRDetail mp = RetrieveMRProductByID(session.CompanyId, ID);
            if (mp == null)
                return ResultType.Error;

            mp.Delete();
            return ResultType.Ok;
        }

        public ResultType DeleteMRProduct(short companyID, string ID)
        {
            if (ID == null)
                return ResultType.NullMainData;

            MRDetail mp = RetrieveMRProductByID(companyID, ID);
            if (mp == null)
                return ResultType.Error;

            mp.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region DeleteMRProduct
        public ResultType DeleteMRProductWOSession(string ID, short CompanyId)
        {
            if (ID == null)
                return ResultType.NullMainData;

            MRDetail mp = RetrieveMRProductByID(CompanyId, ID);
            if (mp == null)
                return ResultType.Error;

            mp.Delete();
            return ResultType.Ok;
        }

        #endregion

        public MRDetail RetrieveMRProductByID(short companyID, string DetailNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRDetail.DetailNo"),
                               helper.CleanValue(DetailNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDetail.CoyID"),
                                helper.CleanValue(companyID));

            return MRDetail.RetrieveFirst(stb.ToString());
        }

        #region RetrieveMRPurchaserByCoyID
        public IList<MRPurchaser> RetrieveMRPurchaserByCoyID(short companyID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRPurchaser.CoyID"),
                               helper.CleanValue(companyID));           

            return MRPurchaser.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveMRDetailsByCoyIDMRNo
        public IList<MRDetail> RetrieveInvalidOrderQtyMRDetailsByCoyIDMRNo(short companyID, string mrNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRDetail.CoyID"),
                               helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDetail.MRNo"),
                                helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} <= {1} ", helper.GetFieldName("MRDetail.OrderQty"),
                                helper.CleanValue(0));

            return MRDetail.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveMRDeliveryPOByMRNo
        public MRDelivery RetrieveMRDeliveryPOByMRNo(short companyID, string mrNo, string PONo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRDelivery.MRNo"),
                               helper.CleanValue(mrNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDelivery.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRDelivery.PONo"),
                                helper.CleanValue(PONo));

            return MRDelivery.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveMRSupplierByCoyIDProductGroupCode
        public IList<MRSupplier> RetrieveMRSupplierByCoyIDProductGroupCode(short companyID, string productGroupCode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRSupplier.CoyID"),
                               helper.CleanValue(companyID));           
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRSupplier.ProductGroupCode"),
                                helper.CleanValue(productGroupCode));



            return MRSupplier.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveMRSupplierByCoyIDVendorDetails
        public IList<MRSupplier> RetrieveMRSupplierByCoyIDVendorDetails(short companyID, string productGroupCode, string accountName)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRSupplier.CoyID"),
                               helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRSupplier.ProductGroupCode"),
                                helper.CleanValue(productGroupCode));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRSupplier.AccountName"),
                                helper.CleanValue(accountName));
            

            return MRSupplier.RetrieveQuery(stb.ToString());
        }
        #endregion


        public MRSupplier RetrieveMRSupplierByID(short ID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRSupplier.Id"),
                               helper.CleanValue(ID));

            return MRSupplier.RetrieveFirst(stb.ToString());
        }


        public ResultType DeleteMRSupplier(short ID, LogSession session)
        {
           
            MRSupplier ms = RetrieveMRSupplierByID(ID);
            if (ms == null)
                return ResultType.Error;

            ms.Delete();
            return ResultType.Ok;
        }

        public ProductManagerProduct RetriveProductTeamDetailByCoyID(short companyId, string productGroupCode)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("ProductManagerProduct.CoyID"),
                               helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("ProductManagerProduct.ProductGroupCode"),
                                helper.CleanValue(productGroupCode));

            return ProductManagerProduct.RetrieveFirst(stb.ToString());
        }



        public MRGRNDetail RetriveMRGRNDetail(short companyId, string productCode, string grnNo, string poNo)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRGRNDetail.CoyID"),
                               helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRGRNDetail.PONo"),
                                helper.CleanValue(poNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRGRNDetail.GRNNo"),
                                helper.CleanValue(grnNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRGRNDetail.ProductCode"),
                                helper.CleanValue(productCode));

            return MRGRNDetail.RetrieveFirst(stb.ToString());

        }

        public IList<MRGRNDetail> RetriveMRGRNDetailByPO(short companyId, string poNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRGRNDetail.CoyID"),
                               helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRGRNDetail.PONo"),
                                helper.CleanValue(poNo));           

            return MRGRNDetail.RetrieveQuery(stb.ToString());
        }
       

        public IList<MRGRNDetail> RetriveMRGRNDetailByPOAndProduct(short companyId, string productCode, string poNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("MRGRNDetail.CoyID"),
                               helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRGRNDetail.PONo"),
                                helper.CleanValue(poNo));           
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("MRGRNDetail.ProductCode"),
                                helper.CleanValue(productCode));

            return MRGRNDetail.RetrieveQuery(stb.ToString());
        }


        
    }
}
