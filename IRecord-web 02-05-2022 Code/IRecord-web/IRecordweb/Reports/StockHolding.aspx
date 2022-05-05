<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockHolding.aspx.cs" Inherits="IRecordweb.Reports.StockHolding" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../AngScripts/jquery-3.6.0.min.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="Script/bootstrap.min.css" />

    <script src="Script/bootstrap.min.js"></script>
    <link rel="stylesheet" type="text/css" href="Script/bootstrap-multiselect.css" />
    <script src="Script/bootstrap-multiselect.js"></script>
    <%--<link rel="stylesheet" type="text/css" href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link rel="stylesheet" type="text/css" href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" />
    <script type="text/javascript" src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"></script>--%>
    <style>
        button.multiselect-option.dropdown-item {
            width: 100% !important;
            text-align: left;
            /* margin-left: 0px; */
            /* padding-left: 1px; */
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
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <div class="row">

                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txt_fromDate">From Date</label>
                        <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txt_fromDate"></asp:TextBox>

                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="txt_toDate">To Date</label>
                        <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txt_toDate"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-3">
                </div>
                <div class="col-sm-3">
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                    <%--    <label for="ListGroup">Select Group</label>--%>
                       <%-- <asp:ListBox ID="ListGroup" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ListGroup_SelectedIndexChanged"
                            SelectionMode="Multiple"></asp:ListBox>--%>
                    </div>



                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                    <%--    <label for="ListAccount">Select Account</label>--%>
                       <%-- <asp:ListBox ID="ListAccount" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>
                    </div>


                </div>
                <%--<div class="col-sm-4">
                    <div class="form-group">
                        <asp:CheckBox ID="CheckBox3" Text="With Group Total" runat="server" />
                    </div>
                    <div class="form-group">
                        <asp:CheckBox ID="CheckBox1" Text="With Narration" runat="server" />
                    </div>
                    <div class="form-group">
                        <asp:CheckBox ID="CheckBox2" Text=" With A/c Confirmation" runat="server" />
                    </div>

                </div>--%>
                <div class="col-sm-4">
                    <asp:Button ID="btn_Search" Style="width: 70%;"
                        CssClass=" btn-success" ClientIDMode="Static" runat="server" Text="Search" OnClick="btn_Search_Click" />
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
               <%-- <rsweb:ReportViewer ID="reportViewer1" Width="100%" Height="100%" AsyncRendering="False" SizeToReportContent="True" runat="server">
                </rsweb:ReportViewer>--%>
                <rsweb:ReportViewer ID="reportViewer1" runat="server"></rsweb:ReportViewer>
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
                    $('#ListGroup').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        filterPlaceholder: 'Search',
                        enableCaseInsensitiveFiltering: true,
                        dropRight: true, buttonWidth: '100%'
                    });
                });
                $(function () {
                    $('#ListAccount').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        filterPlaceholder: 'Search',
                        enableCaseInsensitiveFiltering: true,
                        dropRight: true, buttonWidth: '100%'
                    });
                });
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
