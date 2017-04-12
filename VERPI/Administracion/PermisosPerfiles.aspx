<%@ Page Title="Permisos Perfiles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PermisosPerfiles.aspx.cs" Inherits="VERPI.Administracion.PermisosPerfiles" %>
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

                <cc1:modalpopupextender id="lkBtn_viewPanel_ModalPopupExtender" runat="server" backgroundcssclass="modalBackground"
                    behaviorid="lkBtn_viewPanel_ModalPopupExtender" popupcontrolid="pnl_nuevo" targetcontrolid="lkBtn_viewPanel">
                </cc1:modalpopupextender>
            </div>
            <br />
            <div>
                <asp:GridView runat="server" ID="gvPermisosPerfiles"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."                   
                    AutoGenerateColumns="false" OnRowCommand="gvPermisosPerfiles_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="corrPermisoTipoUsuario" SortExpression="corrPermisoTipoUsuario" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                        <asp:BoundField DataField="nombrePerfil" HeaderText="Perfil" />
                        <asp:BoundField DataField="nombreMenu" HeaderText="Menu Opcion" />
                        <asp:BoundField DataField="acceder" HeaderText="Acceder" />
                        <asp:BoundField DataField="insertar" HeaderText="Insertar" />
                        <asp:BoundField DataField="editar" HeaderText="Editar" />
                        <asp:BoundField DataField="borrar" HeaderText="Borrar" />
                        <asp:BoundField DataField="aprobar" HeaderText="Aprobar" />
                        <asp:BoundField DataField="rechazar" HeaderText="Rechazar" />

                        <asp:ButtonField  ButtonType="Button" Text="Modificar" HeaderText="Modificar" CommandName="modificar" ControlStyle-CssClass="btn btn-success" />
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
                    <asp:Label runat="server" AssociatedControlID="cboPerfil" CssClass="control-label col-xs-2" Text="Perfil: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:DropDownList runat="server" ID="cboPerfil" CssClass="form-control">                            
                        </asp:DropDownList>                                                
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="cboMenu" CssClass="control-label col-xs-2" runat="server" Text="Menu Opcion:"></asp:Label>
                    <div class="col-xs-10">
                        <asp:DropDownList runat="server" ID="cboMenu" CssClass="form-control">                            
                        </asp:DropDownList>
                    </div>
                </div>

                <h4><span class="label label-primary">Permisos</span></h4>

                <div class="form-group">
                    <%--<asp:Label Text="Insertar" runat="server"  CssClass="control-label col-xs-2"/>--%>

                    <div class="col-xs-2">
                        <asp:CheckBox Text="Acceder" runat="server"  ID="cb_acceder"/>
                    </div>

                    <div class="col-xs-2">
                        <asp:CheckBox Text="Insertar" runat="server"  ID="cb_insertar"/>
                    </div>

                    <div class="col-xs-2">
                        <asp:CheckBox Text="Editar" runat="server" id="cb_editar"/>
                    </div>

                    <div class="col-xs-2">
                        <asp:CheckBox Text="Borrar" runat="server" id="cb_borrar"/>
                    </div>

                    <div class="col-xs-2">
                        <asp:CheckBox Text="Aprobar" runat="server" id="cb_aprobar"/>
                    </div>

                    <div class="col-xs-2">
                        <asp:CheckBox Text="Rechazar" runat="server" id="cb_rechazar"/>
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
