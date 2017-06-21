<%@ Page Title="Subir Documentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="VERPI.PreIngresos.Marcas.Documentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary" runat="server" id="pnlMarcas">
        <div class="panel-heading"><%:Title %></div>
        <div class="panel-body form-horizontal" runat="server" id="pnlBody">

            <asp:UpdatePanel runat="server" ID="upnlDoctos">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnl_seccion_adjuntos"></asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGuardar" />
                </Triggers>
            </asp:UpdatePanel>

            <asp:UpdatePanel runat="server" >
                <ContentTemplate>
                    <asp:GridView runat="server" ID="gvAnexos"
                        CssClass="table table-hover table-striped"
                        GridLines="None"
                        EmptyDataText="No se han agregado documentos."
                        AutoGenerateColumns="false" OnRowCommand="gvAnexos_RowCommand">

                        <Columns>
                            <asp:BoundField DataField="correlativo_adjunto" SortExpression="correlativo_adjunto" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            <asp:BoundField DataField="path" HeaderText="Direccion" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            <asp:BoundField DataField="nombre_documento" HeaderText="Documento" />
                            <asp:BoundField DataField="etiqueta" HeaderText="Tipo Adjunto" />
                            <asp:ButtonField ButtonType="Button" Text="Ver Documento" HeaderText="Ver Documento" CommandName="mostrar" ControlStyle-CssClass="btn btn-success" />
                            <asp:ButtonField ButtonType="Button" Text="Eliminar" HeaderText="Eliminar" CommandName="Eliminar" ControlStyle-CssClass="btn btn-primary" />
                        </Columns>

                    </asp:GridView>
                </ContentTemplate>
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

            <asp:Button runat="server" ID="btnRegresar" CssClass="btn btn-info" Text="<< Regresar" CommandName="Regresar" OnClick="btnRegresar_Click" />
            <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Subir Documento" CommandName="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-success" Text="3) Enviar Solicitud" CommandName="Enviar" OnClick="btnEnviar_Click" />
            <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
        </div>
    </div>

</asp:Content>
