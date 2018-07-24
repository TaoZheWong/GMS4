<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrjGeneralInfo.aspx.cs" Inherits="GMSWeb.Sales.Engineering.Project.PrjGeneralInfo" %>


    <div class="row"> 
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtCurrencyCode">Currency Code:</label>
                <select class="form-control" id="txtCurrencyCode" tabindex="9">
                    <option data-tokens="AUD" value="AUD">AUD</option>
                    <option data-tokens="CAD" value="CAD">CAD</option>
                    <option data-tokens="CNY" value="CNY">CNY</option>
                    <option data-tokens="EUR" value="EUR">EUR</option>
                    <option data-tokens="GBP" value="GBP">GBP</option>
                    <option data-tokens="HKD" value="HKD">HKD</option>
                    <option data-tokens="IDR" value="IDR">IDR</option>
                    <option data-tokens="JPY" value="JPY">JPY</option>
                    <option data-tokens="MYR" value="MYR">MYR</option>
                    <option data-tokens="NOK" value="NOK">NOK</option>
                    <option data-tokens="NTD" value="NTD">NTD</option>
                    <option data-tokens="PHP" value="PHP">PHP</option>
                    <option data-tokens="SEK" value="SEK">SEK</option>
                    <option data-tokens="SFR" value="SFR">SFR</option>
                    <option data-tokens="SGD" value="SGD">SGD</option>
                    <option data-tokens="THB" value="THB">THB</option>
                    <option data-tokens="USD" value="USD">USD</option>
                </select>
            </div>
        </div>
        <div class="col-sm-3">   
            <div class="form-group">
                <label for="TotalBillableAmt">Total Amount to be Billed:</label>
                <input type="text" class="form-control" id="txtTotalBillableAmt" placeholder="0.00" tabindex="10"/>
            </div>
        </div>
        <div class="col-sm-3">  
            <div class="form-group">
                <label for="txtTotalPrjCost">Total Project Cost:</label>
                <input type="text" class="form-control" id="txtTotalPrjCost" placeholder="0.00" readonly="readonly"/>
            </div>
        </div>
        <div class="col-sm-3">   
            <div class="form-group">
                <label for="txtTotalProfit">Total Profit:</label>
                <input type="text" class="form-control" id="txtTotalProfit" placeholder="0.00" readonly="readonly"/>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-sm-3">   
            <div class="form-group">
                <label for="txtClaimedToDate">Total Payment Amount Claimed:</label>
                <input type="text" class="form-control" id="txtClaimedToDate" placeholder="0.00" readonly="readonly"/>
            </div>
        </div>
        <div class="col-sm-3">   
            <div class="form-group">
                <label for="txtTotalCE">Cost Estimate Currency:</label>
                <input type="text" class="form-control" id="txtCECurrency"  placeholder="0.00" readonly="readonly"/>
            </div> 
        </div>  
        <div class="col-sm-3">   
            <div class="form-group">
                <label for="txtTotalCE">Total Cost Estimate (Original):</label>
                <input type="text" class="form-control" id="txtTotalCE"  placeholder="0.00" readonly="readonly"/>
            </div> 
        </div>
        <div class="col-sm-3">   
            <div class="form-group">
                <label for="txtTotalCE">Total Cost Estimate (Current):</label>
                <input type="text" class="form-control" id="txtTotalCurrentCE"  placeholder="0.00" readonly="readonly"/>
            </div> 
        </div>  
    </div>
    <div class="row">
        
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtContractNo">Contract No:</label>
                <input type="text" class="form-control" id="txtContractNo" tabindex="11"/>
            </div>
        </div>
        <div class="col-sm-3">    
            <div class="form-group">
                <label for="txtContractDateFrom">Contract Date From:</label>
                <div class='input-group date' id='datetimepicker1'>
                    <input type='text' class="form-control" id="txtContractDateFrom" tabindex="12"/>
                    <span class="input-group-addon">
                        <span class="ti-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
         <div class="col-sm-3">
            <div class="form-group">
                <label for="txtContractDateTo">Contract Date To:</label>    
                <div class='input-group date' id='datetimepicker2'>
                    <input type='text' class="form-control" id="txtContractDateTo" tabindex="13"/>
                    <span class="input-group-addon">
                        <span class="ti-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtClosingDate">Closing Date:</label>    
                <div class='input-group date' id='Div3'>
                    <input type='text' class="form-control" id="txtClosingDate" tabindex="13"/>
                    <span class="input-group-addon">
                        <span class="ti-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
    </div>
    
    <%--<div class="row">
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtCommencementDate">Commencement Date:</label>    
                <div class='input-group date' id='Div4'>
                    <input type='text' class="form-control" id="txtCommencementDate" tabindex="13"/>
                    <span class="input-group-addon">
                        <span class="ti-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtCompletionDate">Completion Date:</label>    
                <div class='input-group date' id='Div2'>
                    <input type='text' class="form-control" id="txtCompletionDate" tabindex="13"/>
                    <span class="input-group-addon">
                        <span class="ti-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        
    </div>--%>
    <div class="row">
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtCustomerPO">Customer P/O:</label>
                <input type="text" class="form-control" id="txtCustomerPO" tabindex="14"/>
                
            </div>
        </div>
        <div class="col-sm-3">    
            <div class="form-group">
                <label for="txtCustomerPIC">Customer PIC:</label>
                <input type="text" class="form-control" id="txtCustomerPIC" tabindex="15"/>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtTelNo">Tel No.:</label>
                <input type="text" class="form-control" id="txtOfficePhone" tabindex="16"/>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="form-group">
                <label for="txtFaxNo">Fax No.:</label>
                <input type="text" class="form-control" id="txtFax" tabindex="17"/>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6">    
            <div class="form-group">
                <label for="txtBillingAddress">Billing Address:</label>
                <div class='input-group' id='Div1'>
                    <textarea class="form-control" rows="5" id="txtBillingAddress" tabindex="18"></textarea>
                    <span  id="BillingAddressDropDown" class="input-group-addon">
                        <span class="glyphicon glyphicon-list-alt"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="form-group">
                <label for="txtOnsiteLocation">Onsite Location:</label>
                <textarea class="form-control" rows="5" id="txtOnsiteLocation" tabindex="19"></textarea>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6">    
            <div class="form-group">
                <label for="txtDescription">Description: </label>
                <textarea class="form-control" rows="5" id="txtDescription" tabindex="20"></textarea>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="form-group">
                <label for="txtRemarks">Remarks:</label>
                <textarea class="form-control" rows="5" id="txtRemarks" tabindex="21"></textarea>
            </div>
        </div>
    </div>
    <p></p>

    <div class="btn-group">
        <div class="btn-group" id="PrintReport">
        <button type="button" class="btn btn-default btn-sm active dropdown-toggle" data-toggle="dropdown" id="Print">Print <span class="caret"></span></button>
        <ul class="dropdown-menu" role="menu">
            <li id="li-PC"><a href="#" id="PrintPC">Project Costing</a></li>
            <li id="li-PC"><a href="#" id="PrintMR">Material Requisition List</a></li>
        </ul>
        </div>
    </div>
    <button type="button" class="btn btn-primary btn-sm active" id="btnSubmitInfo" title="Submit">Save</button>


<script src="PrjGeneralInfo.aspx.js" type="text/javascript"></script>