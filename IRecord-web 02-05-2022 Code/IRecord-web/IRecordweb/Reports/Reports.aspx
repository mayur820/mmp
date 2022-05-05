<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="IRecordweb.Reports.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
             <rsweb:ReportViewer ID="ReportViewer1"  Width="100%" Height="100%" AsyncRendering="False" SizeToReportContent="True" runat="server"></rsweb:ReportViewer>
        </div>
       
    </form>
</body>
</html>
