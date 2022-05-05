<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GainLoss.aspx.cs"  Inherits="IRecordweb.Reports.GainLoss" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap.min.v4.css" rel="stylesheet" />
    <link href="Script/bootstrap-multiselect.css" rel="stylesheet" />
    <link href="../Content/app.min.css" rel="stylesheet" />
    <link href="../Content/theme-color.css" rel="stylesheet" />
    <link href="../Content/custom-body.css" rel="stylesheet" />


    <script src="../AngScripts/jquery-3.6.0.min.js"></script>

    <%--<link rel="stylesheet" type="text/css" href="Script/bootstrap.min.css" />--%>

    <script src="Script/bootstrap.min.js"></script>

    <script src="Script/bootstrap-multiselect.js"></script>
    <%--<link rel="stylesheet" type="text/css" href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link rel="stylesheet" type="text/css" href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" />
    <script type="text/javascript" src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"></script>--%>
    <style>
        /*button.multiselect-option.dropdown-item {
            width: 100% !important;
            text-align: left;
        }

        button.dropdown-item.multiselect-all {
            width: 100% !important;
            text-align: left;
        }

        .multiselect-container .multiselect-filter > input.multiselect-search {
            border: none;
            border-bottom: 1px solid lightgrey;
            padding-left: 2rem;
            margin-left: 0px !important;
            border-bottom-right-radius: 0;
            border-bottom-left-radius: 0;
        }

        .multiselect-container {
            width: 100% !important;
        }*/
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">

            <div class="card">
                <div class="card-body">
                    <div class="row form_2 inner_font_size">
                        <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <label for="txt_fromDate">From Date</label>
                            <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txt_fromDate"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <label for="txt_toDate">To Date</label>
                            <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txt_toDate"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <label for="txt_fromDate">Entry Type</label>
                            <asp:ListBox ID="lstentryType" CssClass="select2" runat="server" AutoPostBack="false"
                                
                                SelectionMode="Multiple"></asp:ListBox>
                           <%-- <asp:DropDownList ID="ddlEnteryType" CssClass="select2" runat="server">
                                <asp:ListItem Value="Equity" Text="Equity"></asp:ListItem>
                                <asp:ListItem Value="F & O" Text="F & O"></asp:ListItem>
                                <asp:ListItem Value="MCX" Text="MCX"></asp:ListItem>
                                <asp:ListItem Value="Currency" Text="Currency"></asp:ListItem>
                                   <asp:ListItem Value="NCDEX" Text="NCDEX"></asp:ListItem>
                            </asp:DropDownList>--%>
                        </div>
                     <%--   <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <label for="txt_toDate">To Date</label>
                            <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txt_toDate"></asp:TextBox>
                        </div>--%>

                        <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <label for="ListSCRIPT">Select Script</label>
                            <asp:ListBox ID="ListSCRIPT" CssClass="select2" runat="server" AutoPostBack="false"
                                
                                SelectionMode="Multiple"></asp:ListBox>
                        </div>
                      <%--  <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <label for="ListAccount">Select Account</label>
                            <asp:ListBox ID="ListAccount" CssClass="select2" runat="server" SelectionMode="Multiple"></asp:ListBox>
                        </div>--%>
                       <%-- <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <div class="w-100 d-flex flex-wrap radio_check_spc">
                                <div class="form-check">
                                    <asp:CheckBox ID="CheckBox3" Text="With Group Total" runat="server" />
                                </div>
                                <div class="form-check">
                                    <asp:CheckBox ID="CheckBox1" Text="With Narration" runat="server" />
                                </div>
                                <div class="form-check">
                                    <asp:CheckBox ID="CheckBox2" Text=" With A/c Confirmation" runat="server" />
                                </div>
                            </div>
                        </div>--%>
                        <div class="col-sm-12 col-md-6 mb-3 d-flex">
                            <label for="ListSCRIPT">Select Holding Type</label>
                            <asp:ListBox ID="LstHoldingType" CssClass="select2" runat="server" AutoPostBack="false"
                                
                                SelectionMode="Multiple"></asp:ListBox>
                        </div>
                        <div class="col-sm-12 text-center mt-3">
                            <asp:Button ID="btn_Search" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Search" OnClick="btn_Search_Click" />
                            <asp:HiddenField ClientIDMode="Static" ID="D1YEAR" runat="server" />
                            <asp:HiddenField ClientIDMode="Static" ID="D1Months" runat="server" />
                            <asp:HiddenField ClientIDMode="Static" ID="D1Day" runat="server" />
                            <asp:HiddenField ClientIDMode="Static" ID="D2YEAR" runat="server" />
                            <asp:HiddenField ClientIDMode="Static" ID="D2Months" runat="server" />
                            <asp:HiddenField ClientIDMode="Static" ID="D2Day" runat="server" />

                        </div>
                    </div>
                    <div class="row">
                        <label style="display: none; font-size: 40px;" id="Loder">Loading..</label>
                        <rsweb:ReportViewer ID="reportViewer1" Width="100%" Height="100%" AsyncRendering="false" SizeToReportContent="True" runat="server">
                        </rsweb:ReportViewer>
                    </div>
                </div>
            </div>
        </div>


        <script type="text/javascript">

            $(function () {
                $('#btn_Search').click(function () {
                    setTimeout(function () {
                        $("#Loder").show();
                    }, 200);
                });

            });
        </script>


        <script type="text/javascript">


            $.noConflict();
            jQuery(document).ready(function ($) {



                $(function () {
                    $('#ListSCRIPT').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        filterPlaceholder: 'Search',
                        enableCaseInsensitiveFiltering: true,
                        dropRight: true, buttonWidth: '100%'
                    });
                    $('#lstentryType').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        filterPlaceholder: 'Search',
                        enableCaseInsensitiveFiltering: true,
                        dropRight: true, buttonWidth: '100%'
                    });

                    $('#LstHoldingType').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        filterPlaceholder: 'Search',
                        enableCaseInsensitiveFiltering: true,
                        dropRight: true, buttonWidth: '100%'
                    });
                    
                });
                //$(function () {
                //    $('#ListAccount').multiselect({
                //        includeSelectAllOption: true,
                //        enableFiltering: true,
                //        filterPlaceholder: 'Search',
                //        enableCaseInsensitiveFiltering: true,
                //        dropRight: true, buttonWidth: '100%'
                //    });
                //});
            });
// 

        </script>

        <script src="../js/jquery-3.5.1.min.js"></script>
        <script src="../js/popper.min.js"></script>
        <script src="../js/bootstrap.min.js"></script>
        <script src="../js/select2.min.js"></script>
        <link href="../Content/themes/base/jquery-ui.min.css" rel="stylesheet" />
        <script src="../Scripts/jquery-2.1.1.min.js"></script>
        <script src="../Scripts/jquery-ui-1.12.1.min.js"></script>
        <script>
            $.noConflict();
            jQuery(document).ready(function ($) {
                $('#txt_fromDate').attr('readonly', true);
                $("#txt_fromDate").datepicker({
                    dateFormat: "dd-M-yy",
                    minDate: new Date($("#D1YEAR").val(), $("#D1Months").val(), $("#D1Day").val()),
                    maxDate: new Date($("#D2YEAR").val(), $("#D2Months").val(), $("#D2Day").val()),
                    changeMonth: true,
                    changeYear: true

                });
                $('#txt_toDate').attr('readonly', true);
                $("#txt_toDate").datepicker({
                    dateFormat: "dd-M-yy",
                    minDate: new Date($("#D1YEAR").val(), $("#D1Months").val(), $("#D1Day").val()),
                    maxDate: new Date($("#D2YEAR").val(), $("#D2Months").val(), $("#D2Day").val()),
                    changeMonth: true,
                    changeYear: true

                });

            });
        </script>
    </form>
</body>
</html>
