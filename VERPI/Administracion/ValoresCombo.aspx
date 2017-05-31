<%@ Page Title="Valores del Combo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ValoresCombo.aspx.cs" Inherits="VERPI.Administracion.ValoresCombo" %>

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
                <asp:GridView runat="server" ID="gvValoresCombo"
                    CssClass="table table-hover table-striped"
                    GridLines="None"
                    EmptyDataText="No existen registros."
                    AutoGenerateColumns="false" OnRowCommand="gvValoresCombo_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="id_valor_combo" SortExpression="id_valor_combo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="texto" HeaderText="Texto" />                        
                        <asp:BoundField DataField="valor" HeaderText="Valor" />                        

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
                    <asp:Label AssociatedControlID="txtTexto" CssClass="control-label col-xs-2" runat="server" Text="Texto: "></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtTexto" CssClass="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTexto"
                            CssClass="text-danger" ErrorMessage="El campo no puede quedar vacio." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="txtValor" CssClass="control-label col-xs-2" runat="server" Text="Valor:"></asp:Label>
                    <div class="col-xs-10">
                        <asp:TextBox ID="txtValor"  CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
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
