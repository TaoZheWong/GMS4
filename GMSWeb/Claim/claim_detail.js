/*
Decependency

Global Variable
===============
app = located at sitemaster , app for angular js module
globalCoyID = located at sitemaster , company id from session
globalUserID = located at sitemaster , user id from session
globalDefaultCurrency = located at sitemaster , defaultCurrency from session
*/

app.controller('ClaimDetailController', function (ClaimService, UtilityService, $location, $scope, $q, $window) {

    var claim = this;
    var timeoutCanceler = $q.defer();
    var initialize = true;
    var isLatest;

    //default value
    claim.allowApproveReject = false;
    claim.total = 0;
    claim.totalSGD = 0;
    claim.companyID = globalCoyID;
    claim.selectedDetail = {};
    claim.customerListSrc = Path + '/Claim/Detail.aspx/GetSelectionCustomerList',
    claim.currencyListSrc = Path + '/Claim/Detail.aspx/GetSelectionCurrencyList',
    claim.typeList = [];
    claim.dim1List = [];
    claim.dim2List = [];
    claim.dim3List = [];
    claim.dim4List = [];
    claim.detail = [];
    claim.claimInfo = {};
    claim.attachmentSrc = "";
    claim.attachmentList = [];
    claim.selectedAttachmentID = 0;
    claim.defaultCurrency = globalDefaultCurrency;
    claim.salesPersonID = [];
    claim.createdby = globalUserID;
    claim.coyid = globalCoyID;
    claim.access = true;
    claim.modifieddate = '';
    claim.allowCreateOnbehalf = false;
    claim.onbehalfuser = [];

    claim.getDim1List = function () {
        ClaimService.getCompanyProjectList(claim.companyID).then(function (resp) {
            claim.dim1List = resp.data.Params.data;
        });
    }

    claim.getDim2List = function (CoyID, dim1ID, init) {
        ClaimService.getCompanyDepartmentList(CoyID, dim1ID).then(function (resp) {
            claim.dim2List = resp.data.Params.data;
            
            if (resp.data.Params.data.length > 0) {
                if(!init)
                    claim.claimInfo.dim2 = resp.data.Params.data[0].DepartmentID;
            }
            else {
                claim.claimInfo.dim2 = -1;
                claim.claimInfo.dim3 = -1;
                claim.claimInfo.dim4 = -1;
                claim.dim2List = [];
                claim.dim3List = [];
                claim.dim4List = [];
            }
        });
    }

    claim.getDim3List = function (CoyID, dim2ID, init) {
        ClaimService.getCompanySectionList(CoyID, dim2ID).then(function (resp) {
            claim.dim3List = resp.data.Params.data;

            if (resp.data.Params.data.length > 0) {
                if (!init)
                    claim.claimInfo.dim3 = resp.data.Params.data[0].SectionID;
            }
            else {
                claim.claimInfo.dim3 = -1;
                claim.claimInfo.dim4 = -1;
                claim.dim3List = [];
                claim.dim4List = [];
            }
        });
    }

    claim.getDim4List = function (CoyID, dim3ID, init) {
        ClaimService.getCompanyUnitList(CoyID, dim3ID).then(function (resp) {
            claim.dim4List = resp.data.Params.data;
            if (resp.data.Params.data.length > 0) {
                if (!init)
                    claim.claimInfo.dim4 = resp.data.Params.data[0].UnitID;
            }
            else {
                claim.claimInfo.dim4 = -1;
                claim.dim4List = [];
            }
        });
    }

    claim.getClaimDetails = function (ClaimID) {
        claim.detail = [];
        ClaimService.getDetail(ClaimID, claim.companyID, timeoutCanceler).then(function (detailResp) {

            angular.forEach(detailResp.data.Params.data, function (data) {
                var newObj = angular.copy(claim.detailModel);
                newObj = data;
                newObj.amount = parseFloat(data.amount);
                newObj.amountSGD = parseFloat(data.amountSGD);
                claim.detail.push(newObj);
            })
        });
    }

    claim.getClaimInfo = function () {

        ClaimService.getClaimInfo($location.$$search.id, timeoutCanceler).then(function (resp) {
            claim.claimInfo = resp.data.Params.data[0];
            claim.modifieddate = resp.data.Params.data[0].ModifiedDate;
            claim.getClaimDetails(resp.data.Params.data[0].ClaimID);
            claim.getSalesPersonList(resp.data.Params.data[0].CreatedBy);

            //Only the user that created the claims can submit the claims for approval.
            if (resp.data.Params.data[0].CreatedBy != globalUserID) {
                claim.access = false;
                    //if ($location.$$search.CurrentLink != 'CompanyFinance') {
                    //    $window.location.href = Path + '/Unauthorized.aspx?ModuleID=Sales';
                    //} else {
                    //    //to prevent finance department able to access all other claim through changing id in URL
                    //    claim.diffaccess = 'F';
                    //}
            }

            ClaimService.getApproveRejectAccess(globalUserID).then(function (resp) {
                if (resp.data.Params.data.length > 0)
                    claim.allowApproveReject = resp.data.Params.data[0].Access;
                else
                    claim.allowApproveReject = false;
            });

            $(".panel").removeClass("panel-loading");
            $(".panel-loading").hide();
        }).catch(function (response) {
            alert('This claim has been deleted.');
            $window.location.href = Path + '/Claim/Default.aspx';
        });
    }

    claim.updateClaimInfo = function () {
        ClaimService.isLatestTransaction(globalCoyID, $location.$$search.id, claim.modifieddate, 'Claim').then(function (resp) {
            if (!resp.data) {
                alert('This transaction is outdated, this page will be refreshed.');
                $window.location.reload();
            } else {
                ClaimService.updateClaimInfo(claim.claimInfo).then(function (resp) {
                    if (resp.data.Status == 1) {
                        claim.claimInfo = resp.data.Params.data[0];
                    }
                    alert(resp.data.Message);
                    claim.getClaimInfo();
                });
            }
        });
    }

    claim.submitClaim = function (data, type) {
        ClaimService.isLatestTransaction(globalCoyID, $location.$$search.id, claim.modifieddate, 'Claim').then(function (resp) {
            if (!resp.data) {
                alert('This transaction is outdated, this page will be refreshed.');
                $window.location.reload();
            } else {
                if (!angular.equals(type, 'reject')) {
                    claim.saveDetails();
                    data.rejectremark = '';
                }

                ClaimService.SubmitClaim(data.ClaimID, globalUserID, type, data.rejectremark).then(function (resp) {
                    alert(resp.data.Message);
                    claim.getClaimInfo();
                });
            }
        });
    }

    claim.deleteClaim = function (data) {
        ClaimService.isLatestTransaction(globalCoyID, $location.$$search.id, claim.modifieddate, 'Claim').then(function (resp) {
            if (!resp.data) {
                alert('This transaction is outdated, this page will be refreshed.');
                $window.location.reload();
            } else {
                if (confirm("Confirm delete this claim? (This step can't be undo)")) {
                    ClaimService.deleteClaim($location.$$search.id, globalCoyID).then(function (resp) {
                        $window.location.href = Path + '/Claim/Default.aspx';
                        alert(resp.data.Message);
                    });
                }
            }
        });
    }

    ClaimService.getClaimDetailModal().then(function (resp) {
        claim.detailModel = JSON.parse(resp.data.Params.data);
    });


    claim.getSalesPersonList = function (claimantId) {
        ClaimService.getSalesPersonID(claim.companyID, claimantId).then(function (resp) {
            claim.salesPersonID = resp.data.Params.data;
        });
    }

    claim.getEntertainmentType = function () {
        ClaimService.getEntertainmentType().then(function (resp) {
            claim.typeList = resp.data.Params.data;
        });
    }
   
    claim.addDetail = function () {
        var newObj = angular.copy(claim.detailModel);
        newObj.date = '';
        newObj.currencyRate = 0
        claim.detail.push(newObj);
    }

    claim.deleteDetail = function (data, index) {
        if (data.id != null ) {
            if (confirm("Confirm delete saved claim detail ?")) {
                ClaimService.DeleteClaimDetail(data.id).then(function (resp) {
                    
                    if (resp.Status > 0)
                        alert(resp.Message);
                    else {
                        claim.detail = _.filter(claim.detail, function (x) { return x.id !== data.id; });
                    }
                });
                
            }
        }else{
            claim.detail.splice(index, 1);  
        }
        $scope.claim_detail_form.$setUntouched();
    }

    claim.resetCurrency = function (data) {
        data.currencyCode = '';
        data.currencyRate = '';
    }

    claim.saveDetails = function (data) {
        if ($scope.claim_detail_form.$invalid) {
            return
        }
        
        ClaimService.addClaimDetail(claim.claimInfo.ClaimID, claim.companyID, JSON.stringify(claim.detail), timeoutCanceler).then(function (resp) {
            if (resp.data.Status == 0) {
                claim.detail = [];
                claim.getClaimDetails(claim.claimInfo.ClaimID);
            }
            alert(resp.data.Message);
        });
    }

    claim.uploadReceipt = function (data) {
        claim.selectedDetail = data;
        $("#attachmentFile").val("")
        claim.resetSelectedAttachment();
        claim.attachmentList = [];
        
        ClaimService.getAttachment(data.id).then(function (resp) {
            if (resp.data.Status == 1) {
                alert(resp.data.Message);
            } else {
                if (resp.data.Params.data.length > 0) {
                    claim.attachmentList = resp.data.Params.data;
                    //claim.attachmentSrc = resp.data.Params.data[0].attachment;
                }
                $("#uploadModal").modal('show');
            }
        });
    }

    claim.rejectClaim = function () {
        $("#rejectModal").modal('show');
    }

    claim.resetSelectedAttachment = function () {
        claim.selectedAttachmentID = 0;
        claim.attachmentSrc = '';
        $("#attachmentFile").val('');
    }

    claim.previewAttachment = function (data) {
        claim.selectedAttachmentID = data.ID;
        claim.attachmentSrc = data.attachment;
        $("#attachmentFile").val('');
    }

    claim.deleteAttachment = function (data) {
        if (confirm("Confirm delete selected attachment ?")) {
            ClaimService.DeleteAttachment(data.ID).then(function (result) {
                if (result.data.Status == 1) {
                    alert(result.data.Message);
                } else {
                    //refresh the attachment list
                    ClaimService.getAttachment(data.ClaimDetailID).then(function (resp) {
                        if (resp.data.Status == 1) {
                            alert(resp.data.Message);
                        } else {
                            claim.attachmentList = resp.data.Params.data;
                            claim.resetSelectedAttachment();
                        }

                    });
                    // refresh claim details list
                    claim.detail = [];
                    claim.getClaimDetails(claim.claimInfo.ClaimID);
                }
            });
        }
    }

    claim.saveAttachment = function () {
        ClaimService.saveAttachment(claim.selectedAttachmentID, claim.selectedDetail.id, claim.attachmentSrc).then(function (result) {
            if (result.data.Status == 1) {
                alert(result.data.Message);
            } else {
                //refresh the attachment list
                ClaimService.getAttachment(claim.selectedDetail.id).then(function (resp) {
                    if (resp.data.Status == 1) {
                        alert(resp.data.Message);
                    } else {
                        claim.attachmentList = resp.data.Params.data;
                        claim.resetSelectedAttachment();
                    }

                });

                // refresh claim details list
                claim.detail = [];
                claim.getClaimDetails(claim.claimInfo.ClaimID);
            }
        });
    }

    claim.createOnBehalf = function () {
        ClaimService.createOnBehalf(globalCoyID, globalUserID).then(function (resp) {
            if (resp.data.Params.data.length > 0) {
                claim.allowCreateOnbehalf = true;
                claim.onbehalfuser = resp.data.Params.data;
            }
        });
    }

    //init
    claim.getClaimInfo();
    claim.getDim1List();
    claim.createOnBehalf();
    claim.getEntertainmentType();

    var calculateAmountInSGD = function (data) {
        data.amountSGD = data.amount * data.currencyRate;
    }


    var calculateTotal = function (data,targetField) {
        var total = 0;

        angular.forEach(data, function (detail, key) {
            if (detail[targetField] && detail[targetField] != "")
                total += detail[targetField];
            else
                total += 0;
        });
        return total;
    }

    $scope.$watch('claim.detail', function (newValue, oldValue) {
        if (newValue != oldValue) {
            angular.forEach(claim.detail, function (detail, key) {
                if (detail.amount != "" && detail.currencyRate != "")
                    calculateAmountInSGD(detail);
                else {
                    detail.amountSGD = 0;
                }
            });

            //calculate total
            claim.total = calculateTotal(claim.detail, 'amount');

            //calculate totalSGD
            claim.totalSGD = calculateTotal(claim.detail, 'amountSGD');
        }
    }, true);

    angular.element(document).ready(function () {
        $scope.$watch('claim.claimInfo', function (newValue, oldValue) {

            if (newValue === oldValue) {
                return;
            }

            if (oldValue.dim1 != newValue.dim1 && newValue.dim1) {
                claim.getDim2List(claim.companyID, newValue.dim1, initialize);
            }

            if (oldValue.dim2 != newValue.dim2 && newValue.dim2) {
                claim.getDim3List(claim.companyID, newValue.dim2, initialize);
            }

            if (oldValue.dim3 != newValue.dim3 && newValue.dim3) {
                claim.getDim4List(claim.companyID, newValue.dim3, initialize);
            }

            initialize = false;
        }, true);
    });
});

app.controller('ClaimDefaultController', function (ClaimService, UtilityService, $scope, $location) {
    var claim = this;
    claim.allowApproveReject = false;

    claim.getapproverights = function () {
        ClaimService.getApproveRejectAccess(globalUserID).then(function (resp) {
            if (resp.data.Params.data.length > 0)
                claim.allowApproveReject = resp.data.Params.data[0].Access;
            else
                claim.allowApproveReject = false;
        });
    }
    claim.getapproverights();
});