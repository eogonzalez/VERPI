<%@ Page Title="Perfiles de Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TipoUsuarios.aspx.cs" Inherits="VERPI.Administracion.TipoUsuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><%: Title %></div>

        <br />
        <div class="panel-body form-vertical">
            <div class="btn">
                <asp:LinkButton runat="server" ID="lkBtn_nuevo" CssClass="btn btn-primary"><i aria-hidden="true" class="glyphicon glyphicon-pencil"></i> Nuevo </asp:LinkButton>
                <asp:LinkButton runat="server" ID="lkBtn_test"></asp:LinkButton>

                <cc1:ModalPopupExtender ID="lkBtn_nuevo_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    BehaviorID="lkBtn_nuevo_ModalPopupExtender" PopupControlID="pnl_nuevo" TargetControlID="lkBtn_nuevo" CancelControlID="btnHide">
                </cc1:ModalPopupExtender>

                <cc1:ModalPopupExtender ID="lkBtn_testModalPopupExtender" runat="server" DropShadow="false"
                    PopupControlID="pnl_nuevo" TargetControlID="lkBtn_test"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>

            </div>
            <br />
            <div>

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>


                        <asp:GridView runat="server" ID="gvTipoUsuario"
                            CssClass="table table-hover table-striped"
                            GridLines="None"
                            EmptyDataText="No existen registros."
                            OnRowCommand="gvTipoUsuario_RowCommand"
                            AutoGenerateColumns="false">

                            <Columns>
                                <asp:BoundField DataField="id_tipousuario" SortExpression="id_tipousuario" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <%--<asp:BoundField DataField="tipo_permiso" HeaderText="Tipo Permiso" />--%>
                                <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha Registro" />

                                <asp:ButtonField ButtonType="Button" Text="Modificar" HeaderText="Modificar" CommandName="modificar" ControlStyle-CssClass="btn btn-success" />
                                
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEliminar" CausesValidation="false" CommandName="eliminar" CommandArgument="<%# Container.DataItemIndex %>" Text="Eliminar" CssClass="btn btn-danger" OnClientClick="return confirm(&quot;¿Esta seguro de borrar el perfil seleccionado?&quot;)"/>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:ButtonField ButtonType="Button" Text="Permisos" HeaderText="Permisos" CommandName="permisos" ControlStyle-CssClass="btn btn-primary" />

                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>

    <div>
        <asp:Panel runat="server" ID="pnl_nuevo" CssClass="panel panel-primary" BorderColor="Black" BackColor="White"
            BorderStyle="Inset" BorderWidth="1px" heigth="600" Width="35%">

            <div class="panel-heading">Mantenimiento de <%:Title%>.</div>
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorMessage" />
            </p>
            <div class="panel-body form-horizontal">

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtNombre" CssClass="control-label col-xs-2" Text="Nombre: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtDescripcion" CssClass="control-label col-xs-2" runat="server" Text="Descripción:"></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtDescripcion" type="text" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

<%--                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlTipoPermiso" CssClass="control-label col-xs-2" Text="Categoria de Perfil: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:DropDownList runat="server" ID="ddlTipoPermiso" CssClass="form-control">
                            <asp:ListItem Value="A">Administrador</asp:ListItem>
                            <asp:ListItem Value="E">Usuario Externo</asp:ListItem>
                            <asp:ListItem Value="F">Funcionario</asp:ListItem>                                                        
                        </asp:DropDownList>
                    </div>
                </div>--%>


                <div class="panel-footer">
                    <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardar_Click" CommandName="Guardar" />
                    <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
