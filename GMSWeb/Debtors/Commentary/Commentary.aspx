<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Commentary.aspx.cs" Inherits="GMSWeb.Debtors.Commentary.Commentary" Title="Debtors - Commentary Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <style>

        .btn-scroll {
            position: fixed;
            display: block;
            position: fixed;
            bottom:50px;
            width: 36px;
            height: 36px;
            border-radius: 18px;
            z-index: 1000;
            background: #666;
            color: #fff;
            text-align: center;
            line-height: 36px;
            text-decoration: none;
        }

        .btn-scroll:hover {
            background: #007aff;
            color: #fff;
            text-decoration: none;
        }

        .btn-scroll:visited {
            background: #007aff;
            color: #fff;
            text-decoration: none;
        }

        .btn-scroll-left {
            right:150px;

        }

        .btn-scroll-right {
            right:100px;
        }
    </style>
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">Customer Info</a></li>
        <li class="active">Overdue Debts</li>
    </ul>
    <h1 class="page-header">Key Customers 
     <br />
        <small>Salesperson or Credit Control can enter commentary for customer's debts.</small>
    </h1>
    <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
        <p>System displays debtor's commentary for the past 2 months based on the As Of Date keyed in by the user.</p>
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a> 
            </div>
            <h4 class="panel-title">
                <i class="ti-search"></i>
                Search filter
            </h4>
        </div>
        <div class="panel-body ">
            <div class="m-t-20">
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">As Of Date</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="txtAsOfDate" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control"></asp:TextBox>
                            <input type="hidden" id="hidAsOfDate" runat="server" />
                            <input type="hidden" id="hidCoyID" runat="server" />
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
                        </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">Salesperson ID</label>
                        <%--<asp:DropDownList ID="ddlSalesperson" runat="Server" CssClass="form-control" />--%>
                        <asp:TextBox runat="server" ID="txtSalespersonID" MaxLength="50" Columns="50" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. S10188"></asp:TextBox>       
                        <input type="hidden" id="hidSalesperson" runat="server" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">Salesperson Name</label>
                        <%--<asp:DropDownList ID="ddlSalesperson" runat="Server" CssClass="form-control" />--%>
                        <asp:TextBox runat="server" ID="txtSalesPersonName" MaxLength="50" Columns="50" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. ADRIAN LIN"></asp:TextBox>       
                        <input type="hidden" id="Hidden1" runat="server" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">Overdue Period</label>
                        <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-control">
                            <asp:ListItem Value="90">> 90 Days</asp:ListItem>
                            <asp:ListItem Value="120">> 120 Days</asp:ListItem>
                            <asp:ListItem Value="180">> 180 Days</asp:ListItem>
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">Sales Person Type</label>
                        <asp:DropDownList ID="ddlSalesPersonType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Customer">Customer</asp:ListItem>
                            <asp:ListItem Value="Invoice">Invoice</asp:ListItem>
                        </asp:DropDownList>
                </div> 
            </div>    
              <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">Customer Code</label>
                            <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="50" Columns="50" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. DLK690"></asp:TextBox>                
                </div> 
               <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">Customer Name</label>
                            <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="50" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. Keppel"></asp:TextBox>                
                </div> 
            </div> 
         
        <div class="panel-footer clearfix">
            <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                OnClick="btnSearch_Click" OnClientClick="return btnSearch_OnClick();"></asp:Button>
        </div>
        <ContentTemplate>
                <uctrl:MsgPanel ID="MsgPanel2" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </div>


    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:moveLeft();" class="btn hidden scrollBtn" ><i class="glyphicon glyphicon-chevron-left"></i></a>
                <a href="javascript:moveRight();" class="btn hidden scrollBtn" ><i class="glyphicon glyphicon-chevron-right"></i></a>
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div id="data-table" class="table-responsive">
                <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
                    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Account Code" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAccountCode" runat="server">
                                    <%# Eval("AccountCode")%>
                                    <input type="hidden" id="hidAccountCode" runat="server" value='<%# Eval("AccountCode")%>' />
                                   
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Account Name" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAccountName" runat="server">
                                            <%# Eval("AccountName")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Currency" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSALES_Currency" runat="server">
                                            <%# Eval("SALES_Currency")%>
                                </asp:Label>
                                <input type="hidden" id="hidCurrency" runat="server" value='<%# Eval("SALES_Currency")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Credit Term" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblCreditTerm" runat="server">
                                            <%# Eval("CreditTerm")%>
                                    </asp:Label>
                                </div>

                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="1 - 30 Days" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblFr1To30" runat="server">
                                            <%# Eval("Fr1To30", "{0:C}")%>
                                    </asp:Label>
                                </div>
                                <%--<a href="#" title="Detail1" onclick='viewDetail(1, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>--%>
                                <asp:LinkButton ID="lnkView" runat="server" OnCommand="lnkViewDetail_Click" CommandArgument='<%#"1"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="31 - 60 Days" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblFr31To60" runat="server">
                                            <%# Eval("Fr31To60", "{0:C}")%>
                                    </asp:Label>
                                </div>
                                <%--<a href="#" title="Detail1" onclick='viewDetail(2, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>--%>
                                <asp:LinkButton ID="lnkView2" runat="server" OnCommand="lnkViewDetail_Click" CommandArgument='<%#"2"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="61 - 90 Days" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblFr61To90" runat="server">
                                            <%# Eval("Fr61To90", "{0:C}")%>
                                    </asp:Label>
                                </div>
                                <%--<a href="#" title="Detail1" onclick='viewDetail(3, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>--%>
                                <asp:LinkButton ID="lnkView3" runat="server" OnCommand="lnkViewDetail_Click" CommandArgument='<%#"3"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="91 - 120 Days" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblFr91To120" runat="server">
                                            <%# Eval("Fr91To120", "{0:C}")%>
                                    </asp:Label>
                                </div>
                                <%--<a href="#" title="Detail1" onclick='viewDetail(4, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>--%>
                                <asp:LinkButton ID="lnkView4" runat="server" OnCommand="lnkViewDetail_Click" CommandArgument='<%#"4"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="121 - 180 Days" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblFr121To180" runat="server">
                                            <%# Eval("Fr121To180", "{0:C}")%>
                                    </asp:Label>
                                </div>
                                <%--<a href="#" title="Detail1" onclick='viewDetail(5, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>--%>
                                <asp:LinkButton ID="lnkView5" runat="server" OnCommand="lnkViewDetail_Click" CommandArgument='<%#"5"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="181 - 365 Days" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblFr181To365" runat="server">
                                            <%# Eval("Fr181To365", "{0:C}")%>
                                    </asp:Label>
                                </div>
                                <%--<a href="#" title="Detail1" onclick='viewDetail(6, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>--%>
                                <asp:LinkButton ID="lnkView6" runat="server" OnCommand="lnkViewDetail_Click" CommandArgument='<%#"6"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="> 365 Days" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblFr365" runat="server">
                                            <%# Eval("Fr365", "{0:C}")%>
                                    </asp:Label>
                                </div>
                                <%--<a href="#" title="Detail1" onclick='viewDetail(7, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>Detail</a>--%>
                                <asp:LinkButton ID="lnkView7" runat="server" OnCommand="lnkViewDetail_Click" CommandArgument='<%#"7"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Total Debts" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblTotalAmount" runat="server">
                                            <%# Eval("TotalAmount", "{0:C}")%>
                                    </asp:Label>
                                </div>

                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Credit Limit" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblCreditLimit" runat="server">
                                            <%# Eval("CreditLimit", "{0:C}")%>
                                    </asp:Label>
                                </div>

                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Last Payment" HeaderStyle-Wrap="false">
                            <ItemTemplate>

                                <div><%# Eval("PaymentRefNo")%></div>
                                <div><%# Eval("PaymentDate","{0: dd-MMM-yyyy}")%></div>
                                <div><%# Eval("PaymentAmount", "{0:C}")%></div>
                                <%--<div><a href="#" title="Detail1" onclick='viewPaymentDetail(5, "<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>","<%# Eval("PaymentRefNo")%>");return false;'>Detail</a></div>--%>
                                <asp:LinkButton ID="lnkView8" runat="server" OnCommand="lnkViewPaymentDetail_Click" CommandArgument='<%#"5"+";"+Eval("AccountCode")+";"+Eval("SALES_Currency")+";"+Eval("PaymentRefNo")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Last Invoice #" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div><%# Eval("docno")%></div>
                                <%--<div><a href="#" title="Detail1" onclick='viewLastPaymentDetail("<%#  Eval("AccountCode")%>","<%# Eval("docno")%>");return false;'>Detail</a></div>--%>
                                <asp:LinkButton ID="lnkView9" runat="server" OnCommand="lnkViewLastPaymentDetail_Click" CommandArgument='<%#Eval("AccountCode")+";"+Eval("docno")+";"+Eval("SalesPersonID")%>'><span>Detail</span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Previous Month Comment" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblComment1" runat="server">
                                            <%# FixCrLf(Eval("Comment1").ToString())%>
                                    </asp:Label>
                                </div>
                                <asp:ImageButton ID="lnkEditComment1"
                                    runat="server" ImageUrl="../../images/Icons/ModifyItem.gif" OnClientClick="EditComment(this)" />
                                <input type="hidden" id="hidComment1Date" runat="server" value='<%# Eval("Comment1Date")%>' />
                                <input type="hidden" id="hidOwnComment1" runat="server" value='<%# Eval("OwnComment1")%>' />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkEditComment1"
                                    PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="This Month Comment" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblComment2" runat="server">
                                            <%# FixCrLf(Eval("Comment2").ToString())%>
                                    </asp:Label>
                                </div>
                                <asp:ImageButton ID="lnkEditComment2"
                                    runat="server" ImageUrl="../../images/Icons/ModifyItem.gif" OnClientClick="EditComment(this)" />
                                <input type="hidden" id="hidComment2Date" runat="server" value='<%# Eval("Comment2Date")%>' />
                                <input type="hidden" id="hidOwnComment2" runat="server" value='<%# Eval("OwnComment2")%>' />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkEditComment2"
                                    PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="View All Comments" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <a href="#" title="View All Comments" onclick='viewCommentHistory("<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>View</a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                    <AlternatingItemStyle CssClass="tAltRow" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:LinkButton ID="LINKBUTTON1" OnClientClick="print();return false;" runat="server" Text="Print"
                CssClass="btn btn-default pull-right m-l-10" ToolTip="Please click to print report." CausesValidation="False"><i class="ti-printer"></i></asp:LinkButton>

            <asp:DropDownList ID="ddlReport" runat="server" CssClass="form-control no-full-width pull-right">
                <asp:ListItem Value="158">Debtor's Commentary By Sales Executives</asp:ListItem>
            </asp:DropDownList>

        </div>
    </div>


    <asp:Panel ID="PNL" runat="server" Style="display: none;" CssClass="popup-layer">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Comment</h4>
                </div>
                <div class="modal-body form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-4 control-label text-left">
                            Characters typed 
                        </label>
                        <div class="col-sm-8">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtCounter" CssClass="form-control" ReadOnly="true" Columns="3"></asp:TextBox>
                                <span class="input-group-addon">
                                    (Limit:600)
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-4 control-label text-left">
                            Comment
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtComment" TextMode="MultiLine" Columns="30" Rows="5" onfocus="select();"
                                CssClass="form-control" onkeyup="update();" MaxLength="10"></asp:TextBox>
                        </div>
                    </div>
                    <input type="hidden" id="hidAccountCode" runat="server" value="" />
                    <input type="hidden" id="hidCurrency" runat="server" value="" />
                    <input type="hidden" id="hidCommentDate" runat="server" value="" />

                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="ButtonOk" runat="server" Text="Save" OnCommand="EditCommentCommand" CssClass="btn btn-primary" />
                <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
            </div>
        </div>
    </asp:Panel>

    <div class="TABCOMMAND">
        <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

   
    <a href="javascript:moveLeft();" class="scrollBtnBtm btn-scroll btn-scroll-left  fade "><i class="ti-arrow-left"></i></a>
    <a href="javascript:moveRight();" class="scrollBtnBtm btn-scroll btn-scroll-right  fade "><i class="ti-arrow-right"></i></a>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $.fn.overflown = function () { var e = this[0]; return e.scrollHeight > e.clientHeight || e.scrollWidth > e.clientWidth; };

        $(document).ready(function () {
            $(".customer-info-menu").addClass("active expand");
            $(".sub-commentary").addClass("active");
           
            scrollButtonVisibleControl();

            $(".content").scroll(function () {
                scrollButtonVisibleControl();
            });
        });

        $(window).resize(function () {
            scrollButtonVisibleControl();
        });

        function scrollButtonVisibleControl() {
                if ($(".table-responsive").overflown())
                    $(".scrollBtn").removeClass("hidden")
                else {
                    if (!$(".scrollBtn").hasClass("hidden"))
                        $(".scrollBtn").addClass("hidden")
                }
                //scroll left right appear control
                $(".table-responsive").overflown() && $("#data-table").offset().top < 0 ? $(".scrollBtnBtm").addClass("in") : $(".scrollBtnBtm").removeClass("in")
              
        }

        var moveRight = function () {
            $(".table-responsive").animate({
                scrollLeft: "+=800px"
            }, "800");
        }

        var moveLeft = function () {
            $(".table-responsive").animate({
                scrollLeft: "-=800px"
            }, "800");
        }
    </script>
</asp:Content>
