<%@ Page Title="Opciones de SubMenu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuOpcion.aspx.cs" Inherits="VERPI.Administracion.MenuOpcion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><%: Title %></div>
        <br />
        <div class="panel-body form-vertical">
            <div class="btn" >
                <asp:LinkButton runat="server" ID="lkBtn_nuevo" CssClass="btn btn-primary"><i aria-hidden="true" class="glyphicon glyphicon-pencil"></i> Nuevo </asp:LinkButton>
                <asp:LinkButton runat="server" ID="lkBtn_viewPanel"></asp:LinkButton>

                <cc1:ModalPopupExtender ID="lkBtn_nuevo_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    BehaviorID="lkBtn_nuevo_ModalPopupExtender" PopupControlID="pnl_nuevo" TargetControlID="lkBtn_nuevo" CancelControlID="btnHide">
                </cc1:ModalPopupExtender>

                <cc1:ModalPopupExtender ID="lkBtn_viewPanel_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    BehaviorID="lkBtn_viewPanel_ModalPopupExtender" PopupControlID="pnl_nuevo" TargetControlID="lkBtn_viewPanel">
                </cc1:ModalPopupExtender>

            </div>
            <br />
            <div>
                <asp:GridView runat="server" ID="gvMenu"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."
                    AutoGenerateColumns="false" OnRowCommand="gvMenu_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="id_opcion" SortExpression="id_opcion" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="descripcion" HeaderText="Descripcion" />
                        <asp:BoundField DataField="url" HeaderText="URL" />
                        <asp:BoundField DataField="comando" HeaderText="Comando" />
                        <asp:BoundField DataField="obligatorio" HeaderText="Obligatorio" />
                        <asp:BoundField DataField="visible" HeaderText="Visible" />
                        <asp:BoundField DataField="login" HeaderText="Login" />

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
                    <asp:Label AssociatedControlID="txtNombreOpcion" CssClass="control-label col-xs-2" runat="server" Text="Nombre: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtNombreOpcion" type="text" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombreOpcion"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtDescripcionOpcion" CssClass="control-label col-xs-2" runat="server" Text="Descripcion:"></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtDescripcionOpcion" type="text" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtURL" CssClass="control-label col-xs-2" runat="server" Text="URL:"></asp:Label>
                    <div class="col-xs-8">
                        <asp:TextBox ID="txtURL" type="text" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtURL"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label Text="Comando:" AssociatedControlID="txtComando" runat="server" CssClass="control-label col-xs-2" />
                    <div class="col-xs-10">
                        <asp:TextBox runat="server" ID="txtComando" CssClass="form-control" />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtOrden" CssClass="control-label col-xs-2" runat="server" Text="Orden:"></asp:Label>
                    <div class="col-xs-2">
                        <asp:TextBox ID="txtOrden" type="text" CssClass="form-control" runat="server"></asp:TextBox>

                    </div>

                    <asp:Label ID="Label3" CssClass="control-label col-xs-2" runat="server" Text="Obligatorio:"></asp:Label>
                    <div class="col-xs-1">
                        <asp:CheckBox ID="cb_obligatorio" runat="server" />
                    </div>

                    <asp:Label ID="Label1" CssClass="control-label col-xs-2" runat="server" Text="Visible:"></asp:Label>
                    <div class="col-xs-1">
                        <asp:CheckBox ID="cb_visible" runat="server" />
                    </div>

                    <asp:Label ID="LblLogin" CssClass="control-label col-xs-1" runat="server" Text="Login:"></asp:Label>
                    <div class="col-xs-1">
                        <asp:CheckBox ID="cb_login" runat="server" />
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-xs-offset-2 col-xs-10">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrden"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>


                <div class="panel-footer">
                    <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardar_Click" CommandName="Guardar"/>
                    <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
