<%@ Page Title="Campos del Formulario" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CamposFormularios.aspx.cs" Inherits="VERPI.Administracion.CamposFormularios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><%: Title %></div>
        <br />
        <div class="panel-body form-vertical">
            <div class="btn">

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
                <asp:GridView runat="server" ID="gvCamposFormulario"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."
                    AutoGenerateColumns="false" OnRowCommand="gvCamposFormulario_RowCommand" OnRowDataBound="gvCamposFormulario_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="correlativo_campo" SortExpression="no_formulario" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="seccion" HeaderText="Seccion" />         
                        <asp:BoundField DataField="no_orden" HeaderText="Orden" />
                        <asp:BoundField DataField="Etiqueta" HeaderText="Etiqueta" />
                        <asp:BoundField DataField="nombre_control" HeaderText="Control" />
                        <asp:BoundField DataField="tipo_control" HeaderText="Tipo de Control" />

                        <asp:ButtonField ButtonType="Button" Text="Modificar" HeaderText="Modificar" CommandName="modificar" ControlStyle-CssClass="btn btn-success" />
                        <asp:ButtonField ButtonType="Button" Text="Combo" HeaderText="Combo" CommandName="combo" ControlStyle-CssClass="btn btn-info" />
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
            BorderStyle="Inset" BorderWidth="1px" Style="overflow: auto; max-height: 545px; width: 75%;">
            <div class="panel-heading">Mantenimiento de <%:Title%>.</div>
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorMessage" />
            </p>
            <div class="panel-body form-horizontal">

                <div class="form-group">
                    <asp:Label AssociatedControlID="cbo_formulario" CssClass="control-label col-xs-2" runat="server" Text="Formulario: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:DropDownList runat="server" ID="cbo_formulario" CssClass="form-control">
                        </asp:DropDownList>
                    </div>

                    <asp:Label AssociatedControlID="cbo_seccion" CssClass="control-label col-xs-2" runat="server" Text="Sección del Formulario: "></asp:Label>
                    <div class="col-xs-4">

                        <asp:DropDownList runat="server" ID="cbo_seccion" CssClass="form-control">
                            <asp:ListItem Value="1">Seccion Uno (Encabezado)</asp:ListItem>
                            <asp:ListItem Value="2">Seccion Dos (Cuerpo)</asp:ListItem>
                            <asp:ListItem Value="3">Seccion Tres (Adjuntos)</asp:ListItem>
                            <asp:ListItem Value="4">Seccion Cuatro</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtOrden" CssClass="control-label col-xs-2" runat="server" Text="Orden: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtOrden" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrden"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>

                    <asp:Label AssociatedControlID="txtEtiqueta" CssClass="control-label col-xs-2" runat="server" Text="Etiqueta: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtEtiqueta" TextMode="SingleLine" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEtiqueta"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>

                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtNombreControl" CssClass="control-label col-xs-2" runat="server" Text="Nombre Control: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtNombreControl" type="text" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombreControl"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>

                    <asp:Label AssociatedControlID="txtDescripcion" CssClass="control-label col-xs-2" runat="server" Text="Descripcion:"></asp:Label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtDescripcion" type="text" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="cbo_tipoControl" CssClass="control-label col-xs-2" runat="server" Text="Tipo Control: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:DropDownList runat="server" ID="cbo_tipoControl" CssClass="form-control">
                            <asp:ListItem Value="1">Texto</asp:ListItem>
                            <asp:ListItem Value="2">Combo</asp:ListItem>
                            <asp:ListItem Value="5">Combo de Paises</asp:ListItem>
                            <asp:ListItem Value="3">Adjunto</asp:ListItem>
                            <asp:ListItem Value="4">Check</asp:ListItem>
                            <asp:ListItem Value="6">Etiqueta de Bloque</asp:ListItem>                            
                        </asp:DropDownList>
                    </div>

                    <asp:Label AssociatedControlID="cbo_modo_texto" CssClass="control-label col-xs-2" runat="server" Text="Modo de Texto: "></asp:Label>
                    <div class="col-xs-4">

                        <asp:DropDownList runat="server" ID="cbo_modo_texto" CssClass="form-control">
                            <asp:ListItem Value="Number">Numerico</asp:ListItem>
                            <asp:ListItem Value="SingleLine">Una Linea</asp:ListItem>
                            <asp:ListItem Value="Multiline">Descripcion</asp:ListItem>
                            <asp:ListItem Value="Email">Correo</asp:ListItem>
                        </asp:DropDownList>

                    </div>

                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txt_nombre_campo_db" CssClass="control-label col-xs-2" runat="server" Text="Nombre Campos en Base de Datos RPI: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txt_nombre_campo_db" TextMode="SingleLine" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_nombre_campo_db"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>

                    <asp:Label AssociatedControlID="txt_ExpresionRegular" CssClass="control-label col-xs-2" runat="server" Text="Expresion Regular: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txt_ExpresionRegular" TextMode="SingleLine" CssClass="form-control" runat="server"></asp:TextBox>
<%--                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_ExpresionRegular"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />--%>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="check_obligatorio" CssClass="control-label col-xs-2" runat="server" Text="Obligatorio: "></asp:Label>
                    <div class="col-xs-4">
                        <asp:CheckBox ID="check_obligatorio" runat="server" />
                    </div>

                </div>


            </div>
            <div class="panel-footer">
                <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Guardar" CommandName="Guardar" OnClick="btnGuardar_Click" />
                <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
