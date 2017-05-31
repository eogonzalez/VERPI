<%@ Page Title="Subir Documentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="VERPI.PreIngresos.Marcas.Documentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary" runat="server" id="pnlMarcas">
        <div class="panel-heading"><%:Title %></div>
        <div class="panel-body form-horizontal" runat="server" id="pnlBody">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnl_seccion_adjuntos"></asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGuardar" />
                </Triggers>
            </asp:UpdatePanel>


        </div>

        <div class="panel-footer">
            <div id="divAlertCorrecto" runat="server">
                <p class="alert alert-success">
                    <asp:Literal runat="server" ID="MensajeCorrectoPrincipal" />
                </p>
            </div>
            <div id="divAlertError" runat="server">
                <p class="alert alert-danger" id="pAlertError" runat="server">
                    <asp:Literal runat="server" ID="ErrorMessagePrincipal" />
                </p>
            </div>

            <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Guardar" CommandName="Guardar" OnClick="btnGuardar_Click" />
            <%--<asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-success" Text="Enviar" CommandName="Enviar" OnClick="btnEnviar_Click" />
            <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />--%>
        </div>
    </div>

</asp:Content>
