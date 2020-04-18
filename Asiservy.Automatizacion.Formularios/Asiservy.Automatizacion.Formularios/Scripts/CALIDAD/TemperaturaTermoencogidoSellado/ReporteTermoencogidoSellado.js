var ListaDatos = [];
var ListaDatosDetalle = [];
var horaActualizar = '';//0=No, 1=Yes

$(document).ready(function () {
    CargarCabeceraDetalle(3);  
    
});

function CargarCabeceraDetalle(op) {
    $('#cargac').show();
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/ReporteTermoencogidoSelladoPartial",
        type: "GET",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            id: 0,
            opcion: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTblAprobarPendiente").html("No existen registros");
            } else {
                $("#divReporte").html('');
                $("#divReporte").html(resultado);
                //var table = $("#tblDataTable");
                //table.DataTable().clear();
                //table.DataTable().destroy();
                //config.opcionesDT.buttons = [];
                //table.DataTable(config.opcionesDT);
                //table.DataTable().draw();
                //ocultarBotones();
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

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
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 60
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
});

//function CargarCabecera(opcion) {
//    MostrarModalCargando();
//    var op = opcion;
//    if ($("#txtFecha").val() == '') {
//        CerrarModalCargando();
//        return;
//    } else {
//        $.ajax({
//            url: "../TemperaturaTermoencogidoSellado/ConsultarBandejaTermoencogidoSellado",
//            type: "GET",
//            data: {
//                fecha: $("#txtFecha").val(),
//                opcion: op
//            },
//            success: function (resultado) {
//                if (resultado == "101") {
//                    window.location.reload();
//                }
//                $("#txtObservacion").val('');
//                if (resultado == "0") {
//                    $("#txtObservacion").prop("disabled", false);
//                    $("#divDetalleControlCloro").prop("hidden", true);
//                    $("#btnModalEditar").prop("hidden", true);
//                    $("#btnModalEliminar").prop("hidden", true);
//                    $("#btnModalGenerar").prop("hidden", false);
//                    ListaDatos = [];
//                } else {
//                    $("#divDetalleControlCloro").prop("hidden", false);
//                    $("#btnModalGenerar").prop("hidden", false);
//                    $("#btnModalEditar").prop("hidden", false);
//                    $("#btnModalEliminar").prop("hidden", false);
//                    $("#txtObservacion").prop("disabled", true);
//                    $("#txtObservacion").val(resultado.Observacion);   
//                    ListaDatos = resultado;
//                }
//                CargarDetalle(0);
//                setTimeout(function () {
//                    CerrarModalCargando();
//                }, 500);
//            },
//            error: function (resultado) {
//                MensajeError(resultado.responseText, false);
//                CerrarModalCargando();
//            }
//        });
//    }
//}
////Retorna PartialView
//function CargarDetalle(opcion) {
//    var op = opcion;
//    if (ListaDatos.length == 0) {
//        ListaDatos.Id = 0;
//    }
//    if (ListaDatosDetalle.length == 0) {
//        ListaDatosDetalle.Id = 0;
//    }
//    $("#divTableEntregaProductoDetalle").html('');
//    $.ajax({
//        url: "../TemperaturaTermoencogidoSellado/ControlTermoencogidoSelladoPartial",
//        type: "GET",
//        data: {
//            id: ListaDatosDetalle.Id,
//            idCabecera: ListaDatos.Id,
//            op: op
//        },
//        success: function (resultado) {
//            ListaDatosDetalle = [];
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (resultado == "0") {
//                $("#divTableEntregaProductoDetalle").html("No existen registros");
//            } else {
//                $("#divTableEntregaProductoDetalle").prop("hidden", false);
//                $("#divTableEntregaProductoDetalle").html(resultado);
//                $("#divDetalleControlCloro").prop("hidden", false);
//                //Oculto la columna Acciones
//                var oTable = $('#tblDataTable').DataTable();
//                var info = oTable.page.info();
//                var count = info.recordsTotal;
//                for (var i = 0; i < count; i++) {
//                    document.getElementById("tdAccionesBody_" + i).style.display = "none";
//                }                
//                document.getElementById("tdAccionesHead").style.display = "none";                
//            }
//        },
//        error: function (resultado) {
//            MensajeError(resultado.responseText, false);
//            CerrarModalCargando();
//        }
//    });
//}