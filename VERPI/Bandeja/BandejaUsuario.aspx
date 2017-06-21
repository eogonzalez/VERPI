﻿<%@ Page Title="Mi Bandeja" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BandejaUsuario.aspx.cs" Inherits="VERPI.Bandeja.BandejaUsuario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><%: Title %><asp:Label runat="server" ID="lblCantidadBandeja" CssClass="label label-info" /></div>
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
                <asp:GridView runat="server" ID="gvBorradores"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."
                    AutoGenerateColumns="false"
                    AllowPaging="true"
                    OnRowCommand="gvBorradores_RowCommand"
                    OnPageIndexChanging="gvBorradores_PageIndexChanging">

                    <PagerSettings Mode="Numeric"
                        Position="Bottom"
                        PageButtonCount="10" />

                    <PagerStyle BackColor="LightBlue"
                        Height="30px"
                        VerticalAlign="Bottom"
                        HorizontalAlign="Center" />

                    <Columns>
                        <asp:BoundField DataField="no_preingreso" HeaderText="No Solicitud" SortExpression="no_preingreso" />
                        <asp:BoundField DataField="cmd" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="no_formulario" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="nombre" HeaderText="Formulario" />
                        <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha Creacion" />
                        <asp:BoundField DataField="estado_txt" HeaderText="Estado" />

                        <asp:ButtonField ButtonType="Button" Text="Modificar" HeaderText="Modificar" CommandName="modificar" ControlStyle-CssClass="btn btn-success" />

                        <asp:TemplateField HeaderText="Eliminar">
                            <ItemTemplate>
                                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" CausesValidation="false" CommandName="eliminar" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-danger" OnClientClick="return confirm(&quot;¿Esta seguro de borrar opcion seleccionada?&quot;)" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
