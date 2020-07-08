//$(document).ready(function () {
//    CargarProductoTerminado();

//});
var ListadoControl  = [];

function CargarProductoTerminado() {   
    $("#divTable").html('');

    if ($("#txtLinea").val() == "") {
        $("#txtLinea").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtLinea").css('borderColor', '#ced4da');
    }

    if ($("#txtFechaPaletizado").val() == "") {
        $("#txtFechaPaletizado").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFechaPaletizado").css('borderColor', '#ced4da');
    }
    
   
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../EntregaProductoTerminado/ReporteEntregaProductoTerminadoPartial",
        type: "GET",
        data: {
            FechaDesde: $("#fechaDesde").val(),
            FechaHasta: $("#fechaHasta").val(),
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
    $('#btnConsultar').prop("hidden", true);

    $("#txtLinea").prop("disabled", true);
    $("#txtFechaPaletizado").prop("disabled", true);
    // $("#selectTurno").prop("disabled", true);
    $("#divCabecera2").prop("hidden", true);
    $("#divDetalleProceso").prop("hidden", false);

    CargarDatosBodega();
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
    $('#btnConsultar').prop("hidden", false);
  //  $("#btnModalEliminar").prop("hidden", true);

  //  $("#btnModalEditar").prop("hidden", true);
    $("#divCabecera2").prop("hidden", false);
    $("#divDetalleProceso").prop("hidden", true);

    CargarProductoTerminado();
}


//////////// CONSULTA BODEGA ////////////////////////////
function CargarDatosBodega() {
    $.ajax({
        url: "../EntregaProductoTerminado/ConsultarBodegas",
        type: "GET",
        data: {
            OF: ListadoControl.OrdenFabricacion
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle").html("No existen registros");
            } else {
                //$("#divTableDetalle").html(resultado);
                $("#txtControlCalidad").val(resultado.UnidadesControlCalidad);
                $("#txtRechazadas").val(resultado.UnidadesRechazadas);
                //$("#txtReproceso").val(resultado.UnidadesReproceso);
                $("#txtDefectos").val(resultado.UnidadesConDefecto);
                $("#txtEntregadas").val(resultado.CajasEntregadas);
                 $("#txtLatasSueltas").val(resultado.LataSueltas);
               
            }
        },
        error: function (resultado) {
            MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
        }
    });
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

//FECHA DataRangePicker
$(function () {
    var start = moment();
    var end = moment();
    var mesesLetras = {
        '01': "Enero",
        '02': "Febrero",
        '03': "Marzo",
        '04': "Abril",
        '05': "Mayo",
        '06': "Junio",
        '07': "Julio",
        '08': "Agosto",
        '09': "Septiembre",
        '10': "Octubre",
        '11': "Noviembre",
        '12': "Diciembre"
    }

    function cb(start, end) {
        var fechaMuestraDesde = mesesLetras[start.format('MM')] + ' ' + start.format('D') + ', ' + start.format('YYYY');
        var fechaMuestraHasta = mesesLetras[end.format('MM')] + ' ' + end.format('D') + ', ' + end.format('YYYY');
        $("#fechaDesde").val(start.format('YYYY-MM-DD'));
        $("#fechaHasta").val(end.format('YYYY-MM-DD'));

        $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
        CargarProductoTerminado();
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 61
        },
        minDate: moment("01/10/2019", "DD/MM/YYYY"),
        maxDate: moment(),
        ranges: {
            'Hoy': [moment(), moment()],
            'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Últimos 7 días': [moment().subtract(6, 'days'), moment()],
            'Últimos 30 días': [moment().subtract(29, 'days'), moment()],
            'Mes actual (hasta hoy)': [moment().startOf('month'), moment()],
            'Último mes': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "fromLabel": "De",
            "toLabel": "a",
            "customRangeLabel": "Personalizada",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        }
    }, cb);
    cb(start, end);
    FiltrarAprobadosFecha();
});



function printDiv() {
    //var contenido = document.getElementById('divDetalleProceso').innerHTML;
    //var contenidoOriginal = document.body.innerHTML;
    //document.body.innerHTML = contenido;
    window.print();
    //document.body.innerHTML = contenidoOriginal;
}