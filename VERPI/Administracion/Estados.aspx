<%@ Page Title="Estados del proceso" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Estados.aspx.cs" Inherits="VERPI.Administracion.Estados" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="panel panel-primary">
        <div class="panel-heading"><%: Title %></div>
        <br />
        <div class="panel-body form-vertical">
            <div class="btn">
                <asp:LinkButton runat="server" ID="lkBtn_nuevo" CssClass="btn btn-primary"><i aria-hidden="true" class="glyphicon glyphicon-pencil"></i> Nuevo </asp:LinkButton>
                <asp:LinkButton runat="server" ID="lkBtn_viewPanel"></asp:LinkButton>

                <cc1:modalpopupextender id="lkBtn_nuevo_ModalPopupExtender" runat="server" backgroundcssclass="modalBackground"
                    behaviorid="lkBtn_nuevo_ModalPopupExtender" popupcontrolid="pnl_nuevo" targetcontrolid="lkBtn_nuevo" cancelcontrolid="btnHide">
                </cc1:modalpopupextender>


                <cc1:modalpopupextender id="lkBtn_viewPanel_ModalPopupExtender" runat="server" backgroundcssclass="modalBackground"
                    behaviorid="lkBtn_viewPanel_ModalPopupExtender" popupcontrolid="pnl_nuevo" targetcontrolid="lkBtn_viewPanel">
                </cc1:modalpopupextender>

            </div>
            <br />
            <div>
                <asp:GridView runat="server" ID="gvEstados"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."
                    AutoGenerateColumns="false"
                    AllowPaging="true"
                    OnPageIndexChanging="gvEstados_PageIndexChanging"
                    OnRowCommand="gvEstados_RowCommand">

                    <PagerSettings Mode="Numeric"
                        Position="Bottom"
                        PageButtonCount="10" />

                    <PagerStyle BackColor="LightBlue"
                        Height="30px"
                        VerticalAlign="Bottom"
                        HorizontalAlign="Center" />

                    <Columns>
                        <asp:BoundField DataField="id_estado" SortExpression="id_estado" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="tipo_tramite" HeaderText="Tipo de Tramite" />
                        <asp:BoundField DataField="descripcion" HeaderText="Nombre" />
                        <asp:BoundField DataField="codigo_estado" HeaderText="Codigo" />
                        <asp:BoundField DataField="dias_max" HeaderText="Dias Maximos" />
                        <asp:BoundField DataField="dias_min" HeaderText="Dias Minimos" />

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

    <div>
        <asp:Panel runat="server" ID="pnl_nuevo" CssClass="panel panel-primary" BorderColor="Black" BackColor="White"
            BorderStyle="Inset" BorderWidth="1px" Style="overflow: auto; max-height: 545px; width: 65%;">
            <div class="panel-heading">Mantenimiento de <%:Title%>.</div>
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorMessage" />
            </p>
            <div class="panel-body form-horizontal">

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="cbo_tipoSolicitud" CssClass="control-label col-xs-2" Text="Tipo de Tramite"></asp:Label>
                    <div class="col-xs-10">
                        <asp:DropDownList ID="cbo_tipoSolicitud" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1" >Marcas</asp:ListItem>
                            <asp:ListItem Value="2" >Pantentes</asp:ListItem>
                            <asp:ListItem Value="3" >Derechos de Autor</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtNombre" CssClass="control-label col-xs-2" runat="server" Text="Nombre: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtNombre" type="text" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtCodigo" CssClass="control-label col-xs-2" runat="server" Text="Codigo: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtCodigo" type="text" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCodigo"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtDiasMax" CssClass="control-label col-xs-2" runat="server" Text="Dias Maximos: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtDiasMax" type="text" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDiasMax"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtDiasMin" CssClass="control-label col-xs-2" runat="server" Text="Dias Minimos: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtDiasMin" type="text" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDiasMin"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="panel-footer">
                    <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Guardar" CommandName="Guardar" OnClick="btnGuardar_Click" />
                    <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
                </div>
            </div>
        </asp:Panel>
    </div>

</asp:Content>
