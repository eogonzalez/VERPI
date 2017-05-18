<%@ Page Title="Pre Ingreso de Marcas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PreIngreso.aspx.cs" Inherits="VERPI.PreIngresos.Marcas.PreIngreso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="panel panel-primary" runat="server" id="pnlMarcas">
        <div class="panel-heading"><%:Title %></div>
        <div class="panel-body form-horizontal" runat="server" id="pnlBody">

            <div class="form-group">
                <asp:Label AssociatedControlID="cbo_tramite" CssClass="control-label col-xs-2" runat="server" Text="Seleccione Tramite: "></asp:Label>
                <div class="col-xs-10">
                    <asp:DropDownList runat="server" ID="cbo_tramite" AutoPostBack="True" OnSelectedIndexChanged="CargaFormulario" CssClass="form-control">
                        <asp:ListItem Value="0">Seleccione Opcion...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="thumbnail" id="pnlFormulario" runat="server">
                <ul class="nav nav-tabs nav-justified">
                    <li role="presentation" class="active"><a href="#tbEncabezado" aria-controls="tbEncabezado" data-toggle="tab">Encabezado</a></li>
                    <li role="presentation"><a href="#tbDatos" aria-controls="tbDatos" data-toggle="tab">Datos del Pre Ingreso</a></li>
                    <li role="presentation"><a href="#tbAnexos" aria-controls="tbAnexos" data-toggle="tab">Documentacion Anexa</a></li>
                </ul>

                <%--Contenido de los paneles--%>
                <div class="tab-content">
                    <%--Tab Encabezado--%>
                    <div role="tabpanel" class="tab-pane active" id="tbEncabezado">
                        <div class="panel-body form-horizontal" runat="server" id="pnl_seccion_1">
                        </div>
                    </div>

                    <%--Tab Datos del Pre Ingreso--%>
                    <div role="tabpanel" class="tab-pane" id="tbDatos">
                        <div class="panel-body form-horizontal" runat="server" id="pnl_seccion_2">
                        </div>
                    </div>

                    <%-- Tab Documentos Adjuntos --%>
                    <div role="tabpanel" class="tab-pane" id="tbAnexos">

                        <div class="panel-body form-horizontal" runat="server" id="pnl_seccion_3">
                            <br>
                            <div class="form-group ">
                                <div class="col-xs-12">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>

                                            <asp:GridView runat="server" ID="gvAnexos"
                                                CssClass="table table-hover table-striped"
                                                GridLines="None"                                                
                                                AutoGenerateColumns="false" OnRowCommand="gvAnexos_RowCommand">

                                                <Columns>
                                                    <asp:BoundField DataField="correlativo_campo" SortExpression="ID_TipoPago" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                    <asp:BoundField DataField="Etiqueta" HeaderText="Documento" />


                                                    <%--<asp:ButtonField ButtonType="Button" Text="Adjuntar" HeaderText="Adjuntar" CommandName="adjuntar" ControlStyle-CssClass="btn btn-success" />--%>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <%--<asp:Button Text="Eliminar" runat="server" ID="btnEliminar" CausesValidation="false" CommandName="eliminar" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-danger" OnClientClick="return confirm(&quot;¿Esta seguro de borrar opcion seleccionada?&quot;)" />--%>
                                                            <asp:FileUpload runat="server"></asp:FileUpload>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>

                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>

                    </div>
                </div>

            </div>


        </div>
        <div class="panel-footer">
            <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Guardar" CommandName="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-success" Text="Enviar" CommandName="Enviar" OnClick="" />
            <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
        </div>
    </div>

</asp:Content>
