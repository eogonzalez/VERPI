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
                    <li id="liEncabezado"  runat="server" role="presentation" class="active"><a href="#tbEncabezado" aria-controls="tbEncabezado" data-toggle="tab">Encabezado</a></li>
                    <li id="liDatos" runat="server" role="presentation"><a href="#tbDatos" aria-controls="tbDatos" data-toggle="tab">Datos del Pre Ingreso</a></li>
                    <li id="liAnexos" runat="server" role="presentation"><a href="#tbAnexos" aria-controls="tbAnexos" data-toggle="tab">Otros Datos</a></li>
                </ul>

                <%--Contenido de los paneles--%>
                <div class="tab-content">
                    <%--Tab Encabezado--%>
                    <div role="tabpanel" class="tab-pane fade in active" id="tbEncabezado">
                        <div class="panel-body form-horizontal">
                            <asp:Panel ID="pnl_seccion_1" runat="server"></asp:Panel>
                            <div id="divAlertClase" runat="server">
                                <p class="alert alert-success">
                                    <asp:Literal runat="server" ID="MensajeClase" />
                                </p>
                            </div>
                        </div>
                    </div>

                    <%--Tab Datos del Pre Ingreso--%>
                    <div role="tabpanel" class="tab-pane fade" id="tbDatos">
                        <div class="panel-body form-horizontal">
                            <asp:Panel ID="pnl_seccion_2" runat="server"></asp:Panel>
                        </div>
                    </div>

                    <%-- Tab Documentos Adjuntos --%>
                    <div role="tabpanel" class="tab-pane fade" id="tbAnexos">
                        <div class="panel-body form-horizontal">

                            <%--Panel de Controles adicionales--%>
                            <asp:Panel ID="pnl_seccion_3" runat="server">
                            </asp:Panel>

                        </div>
                    </div>
                </div>


            </div>

        </div>

        <div class="panel-footer">

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

            <div id="divContrasenia" runat="server">
                <div class="row">
                    <asp:Label Text="Ingrese Contraseña para Validar Envio" runat="server" CssClass="col-xs-4" />
                    <div class="col-xs-8">
                        <asp:TextBox runat="server" ID="txt_contraseña" CssClass="form-control" TextMode="Password" />
                    </div>
                </div>
            </div>
            <br />

            <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="1) Guardar Solicitud" CommandName="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button runat="server" ID="btnAdjuntar" CssClass="btn btn-info" Text="2) Adjuntar Documentos" CommandName="Adjuntar" OnClick="btnAdjuntar_Click" />
            <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-success" Text="3) Enviar Solicitud" CommandName="Enviar" OnClick="btnEnviar_Click" />
            <asp:Button runat="server" ID="btnIrBandeja" CssClass="btn btn-info" Text="Mi Bandeja" CommandName="Bandeja" OnClick="btnIrBandeja_Click" />
            <asp:Button runat="server" ID="btnSalir" CssClass="btn btn-default" Text="Salir" CausesValidation="false" OnClick="btnSalir_Click" />
        </div>
    </div>


</asp:Content>
