<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesplegarFormulario.aspx.cs" Inherits="VERPI.Reportes.Formularios.DesplegarFormulario" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button Text="Imprimir" runat="server" id="btnVerFormulario" OnClick="btnVerFormulario_Click" CssClass="btn btn-primary"/>
    <br />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" Height="50px" ReportSourceID="CrystalReportSource1"  ToolPanelWidth="200px" Width="350px" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" EnableToolTips="False" HasDrilldownTabs="False" HasToggleGroupTreeButton="False" PrintMode="ActiveX" ToolPanelView="None"  HasPrintButton="False" HasExportButton="False"/>
    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
<%--        <Report FileName="CrystalReport1.rpt">
        </Report>--%>
    </CR:CrystalReportSource>
</asp:Content>
