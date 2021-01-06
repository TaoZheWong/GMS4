<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old AddEditSession.aspx.cs"
    Inherits="GMSWeb.SysHR.Training.AddEditSession" %>

<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>

    <title>Add/Edit Course Session</title>

    <script type="text/javascript">
		function dataSplit(ctr)
		{
            var addy_parts = ctr.value.split(":");

            if (addy_parts[0].length < 1 && addy_parts[1].length < 1){

                alert("Time format should be hh:mm");
                ctr.value = '';
                return;
            }
            else
            {
                IntegerBoxControl_Validate(addy_parts[0],addy_parts[1], ctr);
            }
        }
        
        function IntegerBoxControl_Validate(data, data1, ctr)
        {
        // parse the input as an integer
        // var intValue = parseInt(document.all('txtRefilledTime').value, 10);
        var intValue = parseInt(data, 10);
        var intValue1 = parseInt(data1, 10);

        // if this is not an integer
        if (isNaN(intValue))
        {
            // clear text box
            ctr.value = '';
            alert("Time format should be hh:mm");
            return;
        }
        // if this is an integer
        else
        {
   
            switch (true)
            {  
                case (intValue == 0) :

                // clear text box
                ctr.value = ctr.value;
                break;
                case (intValue >= 0) :
                    // put the parsed integer value in the text box
                    ctr.value = ctr.value;
                    break;
                case (intValue < 0) :
                {// put the positive parsed integer value in the text box
                    alert("Time format should be hh:mm");
                    ctr.value = '';
		        }
                break;
            }
      
        }

        // if this is not an integer
        if (isNaN(intValue1))
        {
            // clear text box
            ctr.value = '';
            alert("Time format should be hh:mm");
            return;
        }
        // if this is an integer
        else
        {
            switch (true)
            {
                case (intValue1 == 0) :
                // clear text box
                ctr.value =  ctr.value;
                break;
                case (intValue1 >= 0) :
                // put the parsed integer value in the text box
                ctr.value = ctr.value;
                break;
                case (intValue1 < 0) :
                {
                    // put the positive parsed integer value in the text box
                    alert("Time format should be hh:mm");
		            ctr.value = '';
		        }
                break;
             }
      
        }
   }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Add/Edit Course Session</h3>
            Add or edit a training course session.
            <br />
            <br />
            <asp:ScriptManager ID="sriptmgr1" runat="server">
                <Services>
                    <asp:ServiceReference Path="AutoCompleteCourseTtitle.asmx" />
                </Services>
            </asp:ScriptManager>
            <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                border="1" width="90%">
                <tr>
                    <td class="tbLabel">
                        Course Title</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCourseTitle" runat="server" Columns="40" MaxLength="80" CssClass="textbox"
                            onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="true"
                            OnTextChanged="SetCourseInfo" /><asp:RequiredFieldValidator ID="rfvCourseTitle" runat="server"
                                ControlToValidate="txtCourseTitle" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                            TargetControlID="txtCourseTitle" ServicePath="AutoCompleteCourseTtitle.asmx"
                            ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=";">
                        </ajaxToolkit:AutoCompleteExtender>
                        <input type="hidden" id="hidCourseSessionID" runat="server" />
                        <input type="hidden" id="hidCourseTitle" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Date From</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtDateFrom" MaxLength="10" Columns="10" CssClass="textbox"></asp:TextBox>
                        <img id="imgCalendarNewFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0" />
                        <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                        <asp:CompareValidator ID="cvDateFrom" runat="server" ErrorMessage="Invalid Date"
                            ControlToValidate="txtDateFrom" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow"
                            Operator="DataTypeCheck"></asp:CompareValidator>
                        <asp:TextBox runat="server" ID="txtDateFromTime" MaxLength="5" Columns="10" CssClass="textbox"
                            ToolTip="Time format should be 00:00" onchange="dataSplit(this)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvateFromTime" runat="server" ControlToValidate="txtDateFromTime"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Date To</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtDateTo" MaxLength="10" Columns="10" CssClass="textbox"></asp:TextBox>
                        <img id="imgCalendarNewTo" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0" />
                        <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                        <asp:CompareValidator ID="cvDateTo" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtDateTo"
                            Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
                        <asp:TextBox runat="server" ID="txtDateToTime" MaxLength="5" Columns="10" CssClass="textbox"
                            ToolTip="Time format should be 00:00" onchange="dataSplit(this)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDateToTime" runat="server" ControlToValidate="txtDateToTime"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Course Language</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlCourseLanguage" runat="server" DataTextField="LanguageName"
                            DataValueField="LanguageID" CssClass="dropdownlist">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Venue</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtVenue" runat="server" Columns="20" MaxLength="20" CssClass="textbox"
                            onfocus="select();" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Facilitator</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtFacilitator" runat="server" Columns="20" MaxLength="20" CssClass="textbox"
                            onfocus="select();" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Course Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalCourseFee" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvLocalCourseFee" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalCourseFee"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Registration Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalRegistrationFee" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvLocalRegistrationFee" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalRegistrationFee"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Examination Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalExaminationFee" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvLocalExaminationFee" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalExaminationFee"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local Membership Fee</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalMembershipFee" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvLocalMembershipFee" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalMembershipFee"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Local GST</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtLocalGST" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvLocalGST" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtLocalGST"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Flight Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasFlightCost" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvOverseasFlightCost" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasFlightCost"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Hotel Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasHotelCost" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvOverseasHotelCost" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasHotelCost"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Transport Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasTransportCost" runat="server" Columns="7" MaxLength="7"
                            CssClass="textbox" /><asp:CompareValidator ID="cvOverseasTransportCost" runat="server"
                                ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasTransportCost"
                                Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Meal Cost</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasMealCost" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvLOverseasMealCost" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasMealCost"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas Others</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasOthers" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvOverseasOthers" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasOthers"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Overseas SDF</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOverseasSDF" runat="server" Columns="7" MaxLength="7" CssClass="textbox" /><asp:CompareValidator
                            ID="cvOverseasSDF" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtOverseasSDF"
                            Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" /></td>
                    <td style="width: 10%">
                        <asp:Button ID="btnSubmit" Text="Submit" EnableViewState="False" runat="server" CssClass="button"
                            ValidationGroup="valGrpNewRow" OnClick="btnSubmit_Click"></asp:Button>
                            <asp:Button ID="btnDuplicate" Text="Duplicate" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnDuplicate_Click" Visible="false"></asp:Button>
                    </td>
                </tr>
            </table>
            <br />
            <asp:HyperLink ID="lnkCourseRegistration" runat="server"></asp:HyperLink>
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li></li>
                        </ul>
                        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
