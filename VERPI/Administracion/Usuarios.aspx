<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="VERPI.Administracion.Usuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><%: Title %></div>
        <br />
        <p class="text-danger">
            <asp:Literal ID="ErrorMessage" runat="server" />
        </p>

        <div class="panel-body form-vertical">
            <div class="btn">
                <asp:LinkButton runat="server" ID="lkBtn_nuevo" CssClass="btn btn-primary"><i aria-hidden="true" class="glyphicon glyphicon-pencil"></i> Nuevo </asp:LinkButton>
                <asp:LinkButton runat="server" ID="lkBtn_viewPanel"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="lkBtn_ModificarContraseña" />

                <cc1:ModalPopupExtender ID="lkBtn_nuevo_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackgroupd"
                    BehaviorID="lkBtn_nuevo_ModalPopupExtender" PopupControlID="pnl_nuevo" TargetControlID="lkBtn_nuevo" CancelControlID="btnHide">
                </cc1:ModalPopupExtender>

                <cc1:ModalPopupExtender ID="lkBtn_viewPanel_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    BehaviorID="lkBtn_viewPanel_ModalPopupExtender" PopupControlID="pnl_nuevo" TargetControlID="lkBtn_viewPanel">
                </cc1:ModalPopupExtender>

                <cc1:ModalPopupExtender ID="lkBtn_ModificarContraseña_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    BehaviorID="lkBtn_ModificarContraseña_ModalPopupExtender" PopupControlID="pnl_modificarContraseña" TargetControlID="lkBtn_ModificarContraseña">
                </cc1:ModalPopupExtender>
            </div>
            <br />
            <div>
                <asp:GridView runat="server" ID="gvUsuarios"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen usuarios."
                    OnRowCommand="gvUsuarios_RowCommand"
                    AutoGenerateColumns="false">

                    <Columns>
                        <asp:BoundField DataField="id_usuario" SortExpression="id_usuario" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="nombres" HeaderText="Nombre" />
                        <asp:BoundField DataField="apellidos" HeaderText="Apellido" />
                        <asp:BoundField DataField="cui" HeaderText="CUI" />
                        <asp:BoundField DataField="correo" HeaderText="Correo" />
                        <asp:BoundField DataField="fecha_registro" HeaderText="Fecha Registro" />
                        <asp:BoundField DataField="permiso" HeaderText="Tipo Permiso" />

                        <asp:ButtonField ButtonType="Button" Text="Modificar" HeaderText="Modificar" CommandName="modificar" ControlStyle-CssClass="btn btn-success" />
                        <asp:ButtonField ButtonType="Button" Text="Cambiar Contraseña" HeaderText="Cambiar Contraseña" CommandName="generarpassword" ControlStyle-CssClass="btn btn-info" />
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
            BorderStyle="Inset" BorderWidth="1px" heigth="600" Width="35%">
            <div class="panel-heading">Mantenimiento de <%:Title%>.</div>
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorMessagePanel" />
            </p>
            <div class="panel-body form-horizontal">
                <div class="form-group input-sm">
                    <asp:Label runat="server" AssociatedControlID="NombreUsuario" CssClass="col-md-2 control-label">Nombres</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="NombreUsuario" CssClass="form-control input-sm"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="NombreUsuario"
                            CssClass="text-danger" ErrorMessage="El campo de Nombre es obligatorio." />
                    </div>
                </div>

                <div class="form-group input-sm">
                    <asp:Label runat="server" AssociatedControlID="ApellidoUsuario" CssClass="col-md-2 control-label">Apellidos</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="ApellidoUsuario" CssClass="form-control input-sm"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ApellidoUsuario"
                            CssClass="text-danger" ErrorMessage="El campo Apellido es obligatorio." />
                    </div>
                </div>

                <div class="form-group input-sm">
                    <asp:Label runat="server" AssociatedControlID="CuiUsuario" CssClass="col-md-2 control-label">CUI</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="CuiUsuario" CssClass="form-control input-sm" TextMode="Number" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CuiUsuario"
                            CssClass="text-danger" ErrorMessage="El campo de CUI de usuario es obligatorio." />
                    </div>
                </div>

                <div class="form-group input-sm">
                    <asp:Label runat="server" AssociatedControlID="txtNumero" CssClass="col-md-2 control-label">Telefono</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control input-sm" TextMode="Number" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNumero"
                            CssClass="text-danger" ErrorMessage="El campo Telefono es obligatorio y debe de ser numerico." />
                    </div>
                </div>

                <div class="form-group input-sm">
                    <asp:Label runat="server" AssociatedControlID="txtDireccion" CssClass="col-md-2 control-label">Direccion</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtDireccion" CssClass="form-control input-sm" />
                        <asp:RequiredFieldValidator ErrorMessage="El campo Direccion es obligatorio." ControlToValidate="txtDireccion" runat="server" CssClass="text-danger" />
                    </div>
                </div>

                <div class="form-group input-sm">
                    <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Correo</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control input-sm" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                            CssClass="text-danger" ErrorMessage="El campo de correo electrónico es obligatorio." />
                    </div>

                </div>

                <div class="form-group input-sm">
                    <asp:Label runat="server" AssociatedControlID="ddlTipoPermiso" CssClass="control-label col-md-2">Permiso</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlTipoPermiso" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlTipoPermiso"
                            CssClass="text-danger" ErrorMessage="El campo de permiso es obligatorio." />
                    </div>
                </div>

                <div class="form-group input-sm" runat="server" id="divPassword">
                    <asp:Label runat="server" ID="lblPassword" AssociatedControlID="Password" CssClass="control-label col-md-2">Contraseña</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control input-sm" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                            CssClass="text-danger" ErrorMessage="El campo de contraseña es obligatorio." />
                    </div>
                </div>

                <div class="form-group input-sm" runat="server" id="divConfirmPassword">
                    <asp:Label runat="server" ID="lblConfirmPassword" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirmar contraseña</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control input-sm" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="El campo de confirmación de contraseña es obligatorio." />
                        <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="La contraseña y la contraseña de confirmación no coinciden." />
                    </div>
                </div>

                <div class="form-group input-sm">
                    <%--<asp:Label runat="server" AssociatedControlID="cb_generarContrasenia" CssClass="control-label col-md-2">Contraseña</asp:Label>--%>
                    <div class="col-md-12">
                        <asp:CheckBox runat="server" ID="cb_generarContrasenia" Text="Establecer Contraseña" AutoPostBack="True" OnCheckedChanged="cb_generarContrasenia_CheckedChanged" />
                    </div>
                </div>

                <p class="text-danger">
                    <asp:Literal runat="server" ID="ErrorMessageForm" />
                </p>
            </div>
            <div class="panel-footer">
                <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-success" CommandName="Guardar" Text="Guardar" OnClick="btnGuardar_Click" />
                <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-danger" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="pnl_modificarContraseña" CssClass="panel panel-primary" BorderColor="Black" BackColor="White"
            BorderStyle="Inset" BorderWidth="1px" Style="display: none; overflow: auto; max-height: 445px; width: 35%;">
            <div class="panel-heading">Modificar Contraseña</div>
            <div class="panel-body form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtContraseña" CssClass="col-md-2 control-label">Contraseña</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtContraseña" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContraseña"
                            CssClass="text-danger" ErrorMessage="El campo de contraseña es obligatorio." ValidationGroup="genera" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtConfirmarContraseña" CssClass="col-md-2 control-label">Confirmar contraseña</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtConfirmarContraseña" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmarContraseña"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="El campo de confirmación de contraseña es obligatorio." ValidationGroup="genera"/>
                        <asp:CompareValidator runat="server" ControlToCompare="txtContraseña" ControlToValidate="txtConfirmarContraseña"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="La contraseña y la contraseña de confirmación no coinciden." ValidationGroup="genera" />
                    </div>
                </div>
            </div>
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorPanelContraseña" />
            </p>
            <div class="panel-footer">
                <asp:Button runat="server" ID="btnModificarContraseña" CssClass="btn btn-success" CommandName="Modificar" Text="Modificar" ValidationGroup="genera" OnClick="btnModificarContraseña_Click" />
                <asp:Button runat="server" ID="btnSalirContraseña" CssClass="btn btn-danger" Text="Salir" CausesValidation="false" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
