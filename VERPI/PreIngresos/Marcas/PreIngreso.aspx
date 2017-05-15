<%@ Page Title="Pre Ingreso de Marcas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PreIngreso.aspx.cs" Inherits="VERPI.PreIngresos.Marcas.PreIngreso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
   
    <div class="panel panel-primary" runat="server" id="pnlMarcas">
        <div class="panel-heading"><%:Title %></div>
        <div class="panel-body form-horizontal" runat="server" id="pnlBody">

            <div class="thumbnail">
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

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
