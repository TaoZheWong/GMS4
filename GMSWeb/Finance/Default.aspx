<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GMSWeb.Finance.Default" Title="Finance Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>
    <h1 class="page-header">Finance <br /><small>Main page for Finance.</small></h1>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-md-12 col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="panel-heading-btn">
                            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                        </div>
                        <h4 class="panel-title">
                           <i class="ti-stats-up"></i>
                           MONTHLY SALES ('000s)
                        </h4>
                    </div>
                    <div class="panel-body">
                        <canvas id="monthlySale" width="100%" height="40%"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-12 col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="panel-heading-btn">
                            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                        </div>
                        <h4 class="panel-title">
                           <i class="ti-stats-up"></i>
                           MONTHLY OPERATING INCOME ('000s)
                        </h4>
                    </div>
                    <div class="panel-body">
                        <canvas id="monthOperatingIncome" width="100%" height="40%"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-6 col-md-12 col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="panel-heading-btn">
                            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                        </div>
                        <h4 class="panel-title">
                           <i class="ti-stats-up"></i>
                           YEAR-TO-DATE SALES ('000s)
                        </h4>
                    </div>
                    <div class="panel-body">
                        <canvas id="ytdSale" width="100%" height="40%"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-12 col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="panel-heading-btn">
                            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                        </div>
                        <h4 class="panel-title">
                           <i class="ti-stats-up"></i>
                           YEAR-TO-DATE OPERATING INCOME ('000s)
                        </h4>
                    </div>
                    <div class="panel-body">
                        <canvas id="ytdOperatingIncome" width="100%" height="40%"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        'use strict';

        window.chartColors = {
            red: 'rgb(255, 99, 132)',
            orange: 'rgb(242, 124, 34)',
            yellow: 'rgb(255, 205, 86)',
            green: 'rgb(76, 175, 79)',
            blue: 'rgb(0, 145, 234)',
            purple: 'rgb(153, 102, 255)',
            grey: 'rgb(201, 203, 207)'
        };

        var ctx = document.getElementById("monthlySale");
        var ctx2 = document.getElementById("monthOperatingIncome");
        var ctx3 = document.getElementById("ytdSale");
        var ctx4 = document.getElementById("ytdOperatingIncome");
        
        function generateChart(context, labels, dataSets) {

            Chart.defaults.global.defaultFontSize = 10;
            //Chart.defaults.global.plugins.datalabels.padding.top = 0;

            var ChartOption = {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                layout: {
                    padding: {
                        left: 0,
                        right: 0,
                        top: 30,
                        bottom: 0
                    }
                },
                legend: {
                    position: 'bottom',
                    fullWidth: true,
                    labels: {
                        boxWidth: 12
                    }
                },
                plugins: {
                    datalabels: {
                        align: 'end',
                        anchor: 'end'
                    }
                }
            }

            return new Chart(context, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: dataSets,
                },
                options: ChartOption
            });
        }

        function getMonthlySaleReport(elem) {
            var context = elem.getContext('2d');
            
            var t = elem.closest(".panel");
            if (!$(t).hasClass("panel-loading")) {
                var a = $(t).find(".panel-body");
                $(t).addClass("panel-loading"), $(a).prepend('<div class="panel-loading"><div class="spinner"></div></div>');
            }
         
            return $.ajax({
                url: "Default.aspx/GetMonthlySale",
                dataType: "json",
                data: JSON.stringify({ 'coyId': coyId }),
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (resp) {
                   
                    
                    if (resp.Status != 0) {
                        console.log(resp.Message);
                        return 0;
                    }

                    var data = resp.Params.data;
                    // Success and generate graph dataset
                    var dataSet = [
                        {
                            label: 'Current Month',
                            data: data[0].current_month,
                            backgroundColor: window.chartColors.blue,
                        },
                        {
                            label: 'Budget',
                            data: data[0].budget,
                            backgroundColor:window.chartColors.orange,
                        },
                        {
                            label: 'Last Year',
                            data: data[0].last_year,
                            backgroundColor: window.chartColors.green,
                        },
                    ];

                    generateChart(context, data[0].label, dataSet);
                    setTimeout(function () {
                        $(t).removeClass("panel-loading"), $(t).find(".panel-loading").remove()
                    }, 600);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(textStatus);
                    console.log(textStatus);
                }
            });


            
        }
        function getMonthlyOperatingIncome(elem) {

            var context = elem.getContext('2d');

            var t = elem.closest(".panel");
            if (!$(t).hasClass("panel-loading")) {
                var a = $(t).find(".panel-body");
                $(t).addClass("panel-loading"), $(a).prepend('<div class="panel-loading"><div class="spinner"></div></div>');
            }

            return $.ajax({
                url: "Default.aspx/GetMonthlyOperatingIncome",
                dataType: "json",
                data: JSON.stringify({ 'coyId': coyId }),
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (resp){
                    
                    if (resp.Status != 0) {
                        console.log(resp.Message);
                        return 0;
                    }

                    var data = resp.Params.data;
                    // Success and generate graph dataset
                    var dataSet = [
                        {
                            label: 'Current Month',
                            data: data[0].current_month,
                            backgroundColor: window.chartColors.blue,
                        },
                        {
                            label: 'Budget',
                            data: data[0].budget,
                            backgroundColor: window.chartColors.orange,
                        },
                        {
                            label: 'Last Year',
                            data: data[0].last_year,
                            backgroundColor:window.chartColors.green,
                        },
                    ];

                    generateChart(context, data[0].label, dataSet);
                    setTimeout(function () {
                        $(t).removeClass("panel-loading"), $(t).find(".panel-loading").remove()
                    }, 600);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(textStatus);
                }
            });
        }
        function getYtdSaleReport(elem) {

            var context = elem.getContext('2d');

            var t = elem.closest(".panel");
            if (!$(t).hasClass("panel-loading")) {
                var a = $(t).find(".panel-body");
                $(t).addClass("panel-loading"), $(a).prepend('<div class="panel-loading"><div class="spinner"></div></div>');
            }

            return $.ajax({
                url: "Default.aspx/GetYtdSale",
                dataType: "json",
                data: JSON.stringify({ 'coyId': coyId }),
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (resp){
                    
                    if (resp.Status != 0) {
                        console.log(resp.Message);
                        return 0;
                    }

                    var data = resp.Params.data;
                    // Success and generate graph dataset
                    var dataSet = [
                        {
                            label: 'Current Month',
                            data: data[0].current_month,
                            backgroundColor: window.chartColors.blue,
                        },
                        {
                            label: 'Budget',
                            data: data[0].budget,
                            backgroundColor: window.chartColors.orange,
                        },
                        {
                            label: 'Last Year',
                            data: data[0].last_year,
                            backgroundColor: window.chartColors.green,
                        },
                    ];

                    generateChart(context, data[0].label, dataSet);
                    setTimeout(function () {
                        $(t).removeClass("panel-loading"), $(t).find(".panel-loading").remove()
                    }, 600);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(textStatus);
                }
            });
        }
        function getYtdOperatingIncome(elem) {

            var context = elem.getContext('2d');

            var t = elem.closest(".panel");
            if (!$(t).hasClass("panel-loading")) {
                var a = $(t).find(".panel-body");
                $(t).addClass("panel-loading"), $(a).prepend('<div class="panel-loading"><div class="spinner"></div></div>');
            }

            return $.ajax({
                url: "Default.aspx/GetYtdOperatingIncome",
                dataType: "json",
                data: JSON.stringify({ 'coyId': coyId }),
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (resp){

                    
                    if (resp.Status != 0) {
                        console.log(resp.Message);
                        return 0;
                    }

                    var data = resp.Params.data;
                    // Success and generate graph dataset
                    var dataSet = [
                        {
                            label: 'Current Month',
                            data: data[0].current_month,
                            backgroundColor: window.chartColors.blue,
                        },
                        {
                            label: 'Budget',
                            data: data[0].budget,
                            backgroundColor: window.chartColors.orange,
                        },
                        {
                            label: 'Last Year',
                            data: data[0].last_year,
                            backgroundColor: window.chartColors.green,
                        },
                    ];
                    generateChart(context, data[0].label, dataSet);
                    setTimeout(function () {
                        $(t).removeClass("panel-loading"), $(t).find(".panel-loading").remove()
                    }, 600);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(textStatus);
                    console.log(textStatus);
                }
            });
        }

        var ajaxMonthlySaleReport = getMonthlySaleReport(ctx);
        var ajaxMonthlyOperatingIncome = getMonthlyOperatingIncome(ctx2);
        var ajaxYtdSaleReport = getYtdSaleReport(ctx3);
        var ajaxYtdOperatingIncome = getYtdOperatingIncome(ctx4);

        $(window).bind('beforeunload', function () {
            ajaxMonthlySaleReport.abort();
            ajaxMonthlyOperatingIncome.abort();
            ajaxYtdSaleReport.abort();
            ajaxYtdOperatingIncome.abort();
            return
        });


    </script>
</asp:Content>
