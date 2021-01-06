app.factory('ClaimService', function ($http,$timeout) {
    return {
        getApproveRejectAccess: function (id) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetActionAccessRight',
                data: {
                    'UserID': id
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getCompanyProjectList: function (id) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetCompanyProjectListByCoyID',
                data: {
                    'CompanyID': id
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getCompanyDepartmentList: function (CompanyID,ProjectID) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetCompanyDepartmentListByCoyIDAndProjectID',
                data: {
                    'CompanyID': CompanyID,
                    'ProjectID':ProjectID
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getCompanySectionList: function (CompanyID, DepartmentID,date) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetCompanySectionList',
                data: {
                    'CompanyID': CompanyID,
                    'DepartmentID': DepartmentID,
                    'Date': date
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getCompanyUnitList: function (CompanyID, SectionID) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetCompanyUnitList',
                data: {
                    'CompanyID': CompanyID,
                    'SectionID': SectionID
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getAttachment: function (id) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetClaimAttachment',
                data: {
                    'ClaimDetailID': id
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        saveAttachment: function (CompanyID,attachmentID, claimDetailId, data) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/SaveClaimAttachment',
                data: {
                    'CompanyID': CompanyID,
                    'ClaimAttachmentID': attachmentID,
                    'ClaimDetailID': claimDetailId,
                    'data':data
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        DeleteAttachment: function (id) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/DeleteAttachment',
                data: { 'ClaimAttachmentID': id },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getClaimInfo: function (ClaimID, timeoutCanceler) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetClaim',
                data: {
                    'ClaimID': ClaimID
                },
                timeout: timeoutCanceler.promise,
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        updateClaimInfo: function (claimInfo) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/UpdateClaim',
                data: claimInfo,
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getClaimDetailModal: function () {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/DetailModel',
                data: { },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        addClaimDetail: function (id, CompanyID, detailString, timeoutCanceler) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/CreateClaimDetail',
                data: { claimID : id , details: detailString, companyID : CompanyID},
                timeout: timeoutCanceler.promise,
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        DeleteClaimDetail: function (id) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/DeleteClaimDetail',
                data: { ClaimDetailID: id },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        SubmitClaim: function (id,userid,type,rejectremark,apppayvou) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/SubmitClaim',
                data: {
                    ClaimID: id,
                    Type: type,
                    UserID: userid,
                    RejectRemark: rejectremark,
                    ApprovePaymentVoucher : apppayvou
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getCustomerList: function (Name, CompanyID, timeoutCanceler) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetSelectionCustomerList',
                data: {
                    'Name':Name,
                    'CompanyID': CompanyID
                },
                timeout: timeoutCanceler.promise,
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getDetail: function (ClaimID, CompanyID, timeoutCanceler) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetClaimDetails',
                data: {
                    'ClaimID': ClaimID,
                    'CompanyID': CompanyID
                },
                timeout: timeoutCanceler.promise,
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getSalesPersonID: function (id, claimantID) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetClaimSalesPersonID',
                data: {
                    'CompanyID': id,
                    'ClaimantID': claimantID
                },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        getEntertainmentType: function () {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/GetEntertainmentType',
                data: { },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        deleteClaim: function (id, coyid) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/DeleteClaim',
                data: { ClaimID: id , CoyID : coyid},
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        isLatestTransaction: function (coyid, id, modifydate, module) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/IsLatestTransaction',
                data: { CoyID: coyid, ClaimID: id, ModifyDate: modifydate, Module: module  },
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        },
        createOnBehalf: function (coyid, userid) {
            var action = $http({
                url: Path + '/Claim/Detail.aspx/CreateOnBehalf',
                data: { CoyID: coyid, UserID: userid},
                dataType: 'JSON',
                contentType: 'application/json; characterset=utf-8',
                method: 'POST'
            });

            return action;
        }
    };
});