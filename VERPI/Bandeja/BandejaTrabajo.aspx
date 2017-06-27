<%@ Page Title="Bandeja de Entrada" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BandejaTrabajo.aspx.cs" Inherits="VERPI.Bandeja.BandejaTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><%: Title %> <asp:Label runat="server" ID="lblCantidadBandeja" CssClass="label label-info" /></div>
        <br />
        <div class="panel-body form-vertical">
            <div class="btn-group" role="group">
            </div>
            <br />
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
            <div>
                <asp:GridView runat="server" ID="gvExpedientes"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."
                    AllowPaging="true"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvExpedientes_RowCommand"
                    OnPageIndexChanging="gvExpedientes_PageIndexChanging">

                    <PagerSettings Mode="Numeric"
                        Position="Bottom"
                        PageButtonCount="10" />

                    <PagerStyle BackColor="LightBlue"
                        Height="30px"
                        VerticalAlign="Bottom"
                        HorizontalAlign="Center" />

                    <Columns>
                        <asp:BoundField DataField="no_expediente" HeaderText="No Expediente" SortExpression="no_expediente" />
                        <asp:BoundField DataField="cmd" HeaderText="cmd" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="no_preingreso" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="nombres" HeaderText="Usuario" />
                        <asp:BoundField DataField="nombre_formulario" HeaderText="Formulario" />
                        <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha Creacion" />
                        <asp:BoundField DataField="estado_txt" HeaderText="Estado" />

                        <asp:ButtonField ButtonType="Button" Text="Auto Asignar" HeaderText="Asignar" CommandName="Asignar" ControlStyle-CssClass="btn btn-success" />
                        <asp:ButtonField ButtonType="Button" Text="Asignar Funcionario" HeaderText="AsignarFuncionario" CommandName="AsignarFuncionario" ControlStyle-CssClass="btn btn-info" />

                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
