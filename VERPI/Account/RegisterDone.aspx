<%@ Page Title="Registro Completo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterDone.aspx.cs" Inherits="VERPI.Account.RegisterDone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Su usuario se ha sido registrado correctamente.</h3>
    <p>Se ha enviado un correo para confirmar dicho registro.</p>
    <p>
        Inicie session 
            <asp:HyperLink runat="server" ID="LoginHyperLink" ViewStateMode="Disabled">aqui</asp:HyperLink>
    </p>
</asp:Content>
