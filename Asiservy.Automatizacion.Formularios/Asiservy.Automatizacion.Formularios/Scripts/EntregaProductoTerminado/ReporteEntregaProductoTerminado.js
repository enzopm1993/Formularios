//$(document).ready(function () {
//    CargarProductoTerminado();

//});
var ListadoControl  = [];

function CargarProductoTerminado() {   
    $("#divTable").html('');
    if ($("#txtLinea").val() == "") {
        return;
    }
    if ($("#txtFechaPaletizado").val() == "") {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../EntregaProductoTerminado/ReporteEntregaProductoTerminadoPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFechaPaletizado").val(),
            Linea: $("#txtLinea").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } 
            if (resultado == "0") {               
                $("#divTable").html('No Existen Registros');
            } else {
                $("#divTable").html(resultado);
                config.opcionesDT.pageLength = 15;
                $('#tblDataTable').DataTable(config.opcionesDT);
                $('#btnConsultar').prop("disabled", true);
            }
            $("#spinnerCargando").prop("hidden", true);
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}



function SeleccionarControlEntregaProductoTerminado(model) {
    //  console.log(model);
    ListadoControl  = model;
      $("#txtOrdenFabricacion").val(model.OrdenFabricacion);
    $("#txtFechaProduccion").val(moment(model.FechaProduccion).format("YYYY/MM/DD"));
    $("#txtFechaPaletizado2").val(moment(model.FechaPaletizado).format("YYYY/MM/DD"));
    $("#txtFechaVencimiento").val(moment(model.FechaVencimiento).format("YYYY/MM/DD"));
    $("#txtCliente").val(model.Cliente);
    $("#txtCodigoSap").val(model.CodigoSap);
    $("#txtEtiqueta").val(model.Etiqueta);
    $("#txtProducto").val(model.Producto);
    $("#txtCodigoProducto").val(model.CodigoProducto);
    $("#txtObservacion").val(model.Observacion);

    if (model.EstadoReporte == null || model.EstadoReporte == false) {
        $("#txtEstado").html("(PENDIENTE)");
    } else {
        $("#txtEstado").html("");
    }
 
    $("#btnAtras").prop("hidden", false);
    $("#btnImprimir").prop("hidden", false);

    $("#txtLinea").prop("disabled", true);
    $("#txtFechaPaletizado").prop("disabled", true);
    // $("#selectTurno").prop("disabled", true);
    $("#divCabecera2").prop("hidden", true);
    $("#divDetalleProceso").prop("hidden", false);

    CargarProcesoDetalleMaterial();
    CargarEntregaProductoTerminadoDetalle();
    CargarProcesoDetalleTiemposMuertos();
    //CargarProcesoDetalleDaniado();
}

function AtrasControlPrincipal() {
//    $("#btnModalGenerar").prop("hidden", false);
    $("#txtLinea").prop("disabled", false);
    $("#txtFechaPaletizado").prop("disabled", false);

    //  $("#selectTurno").prop("disabled", false);
    $("#btnAtras").prop("hidden", true);
    $("#btnImprimir").prop("hidden", true);
  //  $("#btnModalEliminar").prop("hidden", true);

  //  $("#btnModalEditar").prop("hidden", true);
    $("#divCabecera2").prop("hidden", false);
    $("#divDetalleProceso").prop("hidden", true);

    CargarProductoTerminado();
}



////////CONSUMO DE MATERIALES //////////////////////////////
function CargarProcesoDetalleMaterial() {
    $("#spinnerCargandoMaterial").prop("hidden", false);
    $("#divTableMaterial").html('');
    $.ajax({
        url: "../EntregaProductoTerminado/ReporteControlConsumoMaterialPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdProductoTerminado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableMaterial").html("No existen registros");
                $("#spinnerCargandoMaterial").prop("hidden", true);
            } else {
                $("#spinnerCargandoMaterial").prop("hidden", true);
                $("#divTableMaterial").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoMaterial").prop("hidden", true);
        }
    });
}


//////////////////////////////// DETALLE HORAS ///////////////////////////////////////

function CargarEntregaProductoTerminadoDetalle() {
    $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", false);
    $("#divTableEntregaProductoDetalle").html('');
    $.ajax({
        url: "../EntregaProductoTerminado/ReporteEntregaProductoTerminadoDetallePartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdProductoTerminado
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableEntregaProductoDetalle").html("No existen registros");
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
                $("#divTableEntregaProductoDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
        }
    });
}



/////////////////////TIEMPOS MUERTOS////////////////////////////

function CargarProcesoDetalleTiemposMuertos() {
    $("#spinnerCargandoTiemposMuertos").prop("hidden", false);
    $("#divTableTiemposMuertos").html('');
    $.ajax({
        url: "../EntregaProductoTerminado/ReporteControlConsumoTiemposMuertos",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdProductoTerminado
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableTiemposMuertos").html("No existen registros");
                $("#spinnerCargandoTiemposMuertos").prop("hidden", true);
            } else {
                $("#spinnerCargandoTiemposMuertos").prop("hidden", true);
                $("#divTableTiemposMuertos").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoDaniados").prop("hidden", true);
        }
    });
}


function printDiv() {
    var contenido = document.getElementById('divDetalleProceso').innerHTML;
    var contenidoOriginal = document.body.innerHTML;
    document.body.innerHTML = contenido;
    window.print();
    document.body.innerHTML = contenidoOriginal;
}