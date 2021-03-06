﻿<%@ Page Title="Bandeja de Entrada" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BandejaTrabajo.aspx.cs" Inherits="VERPI.Bandeja.BandejaTrabajo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <%: Title %>
            <asp:Label runat="server" ID="lblCantidadBandeja" CssClass="label label-info" />
        </div>
        <br />
        <div class="panel-body form-horizontal">

            <div class="form-group">
                <%--                <asp:Label AssociatedControlID="cbo_estado_Filtro" Text="Estado:" CssClass="control-label col-xs-2" runat="server" />
                <div class="col-xs-4">
                    <asp:DropDownList runat="server" ID="cbo_estado_Filtro" CssClass="form-control"
                        OnSelectedIndexChanged="cbo_estado_Filtro_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Seleccione Opcion..</asp:ListItem>
                        <asp:ListItem Value="1">Enviado</asp:ListItem>
                        <asp:ListItem Value="2">Borrador</asp:ListItem>
                    </asp:DropDownList>
                </div>--%>

                <asp:Label Text="Seleccione Tipo de Tramite:" runat="server" CssClass="control-label col-xs-2" />
                <div class="col-xs-3">
                    <asp:CheckBox runat="server" ID="chk_Marcas" onclick="UncheckOthers(this);"
                        Text="Marcas" OnCheckedChanged="chk_Marcas_CheckedChanged" AutoPostBack="true" />
                </div>

                <div class="col-xs-3">
                    <asp:CheckBox runat="server" ID="chk_Patentes" onclick="UncheckOthers(this);"
                        Text="Patentes" OnCheckedChanged="chk_Patentes_CheckedChanged" AutoPostBack="true" />
                </div>

                <div class="col-xs-4">
                    <asp:CheckBox runat="server" ID="chk_Derechos" onclick="UncheckOthers(this);"
                        Text="Derechos de Autor" OnCheckedChanged="chk_Derechos_CheckedChanged" AutoPostBack="true" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="lblPeriodo" AssociatedControlID="txtFechaInicial" CssClass="control-label col-xs-2"
                    runat="server" Text="Periodo: Fecha Inicial:"></asp:Label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtFechaInicial" CssClass="form-control" runat="server" OnTextChanged="txtFechaInicial_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" BehaviorID="txtFechaInicial_CalendarExtender"
                        TargetControlID="txtFechaInicial" Format="dd/MM/yyyy" />
                </div>
                <asp:Label ID="lblFechaFin" AssociatedControlID="txtFechaFinal" CssClass="control-label col-xs-2" runat="server" Text=" Fecha Final:"></asp:Label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtFechaFinal" CssClass="form-control" runat="server" OnTextChanged="txtFechaFinal_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" BehaviorID="txtFechaFinal_CalendarExtender"
                        TargetControlID="txtFechaFinal" Format="dd/MM/yyyy" />
                </div>
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

                        <%--<asp:ButtonField ButtonType="Button" Text="Auto Asignar" HeaderText="Asignar" CommandName="Asignar" ControlStyle-CssClass="btn btn-success" />
                        <asp:ButtonField ButtonType="Button" Text="Asignar Funcionario" HeaderText="AsignarFuncionario" CommandName="AsignarFuncionario" ControlStyle-CssClass="btn btn-info" />--%>

                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
