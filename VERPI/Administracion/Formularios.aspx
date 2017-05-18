<%@ Page Title="Listado de Formularios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Formularios.aspx.cs" Inherits="VERPI.Administracion.Formularios" %>

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
                <asp:GridView runat="server" ID="gvFormularios"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."
                    AutoGenerateColumns="false" OnRowCommand="gvFormularios_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="no_formulario" SortExpression="no_formulario" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="tipo_tramite"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>                        
                        <asp:BoundField DataField="nombre_tipo" HeaderText="Tipo de Tramite" />
                        <asp:BoundField DataField="nombre" HeaderText="Nombre Formulario" />                        
                        <asp:BoundField DataField="descripcion_formulario" HeaderText="Descripcion" />                        

                        <asp:ButtonField ButtonType="Button" Text="Modificar" HeaderText="Modificar" CommandName="modificar" ControlStyle-CssClass="btn btn-success" />
                        <asp:TemplateField HeaderText="Eliminar">
                            <ItemTemplate>
                                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" CausesValidation="false" CommandName="eliminar" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-danger" OnClientClick="return confirm(&quot;¿Esta seguro de borrar opcion seleccionada?&quot;)" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:ButtonField  ButtonType="Button" Text="Campos" HeaderText="Campos" CommandName="campos" ControlStyle-CssClass="btn btn-primary" />
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
                    <asp:Label AssociatedControlID="cbo_tipo_tramite" CssClass="control-label col-xs-2" runat="server" Text="Tipo de Tramite: "></asp:Label>
                    <div class="col-xs-10">
                        
                        <asp:DropDownList runat="server" ID="cbo_tipo_tramite" CssClass="form-control">
                            <asp:ListItem Value="1" >Marcas</asp:ListItem>
                            <asp:ListItem Value="2" >Pantentes</asp:ListItem>
                            <asp:ListItem Value="3" >Derechos de Autor</asp:ListItem>
                        </asp:DropDownList>
                        
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtNombre" CssClass="control-label col-xs-2" runat="server" Text="Nombre formulario: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtNombre" type="text" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtDescripcion" CssClass="control-label col-xs-2" runat="server" Text="Descripcion:"></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtDescripcion" type="text" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
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
