var listaDatos = [];
$(document).ready(function () {
    //CargarReporteControlCloro(0);
    CargarCabecera();
});

var turno = ['', "A", "B","C","D","E","F","G"];

function CargarCabecera() {
    Atras();
    $('#lblMostrarFecha').text('');
    $('#lblMostrarHora').text('');
    $('#lblMostrarObservacion').text('');
    $('#cargac').show();
   
    if ($("#fechaDesde").val() == '' || $("#fechaHasta").val() == '') {
        var date = new Date();
        var shortDate = moment(date).format('YYYY-MM-DD');
        shortDate += ' 23:59';
        $("#fechaDesde").val(moment(date).format('YYYY-MM-DD'));
        $("#fechaHasta").val(moment(shortDate).format('YYYY-MM-DDTHH:mm'));
    } else {
        var d = '';
        d = $("#fechaHasta").val();
        $("#fechaHasta").val(moment(d).format('YYYY-MM-DDTHH:mm'));
    }

    $.ajax({
        url: "../CloroCisternaDescongelado/ReporteCloroCisternaDescongeladoCabeceraPartial",
        data: {
            fechaDesde: $("#fechaDesde").val(),
            fechaHasta: $("#fechaHasta").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
                $('#cargac').hide();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarCabecera(jdata) {
    $('#cargac').show();
    var op = 4;
    if (jdata.Turno != null || jdata.Turno != '') {
        $('#lblMostrarHora').text(turno[jdata.Turno]);
        $('#lblMostrarHoraM').text(turno[jdata.Turno]);
    }
    $('#lblMostrarFecha').text(moment(jdata.Fecha).format('DD-MM-YYYY'));
    $('#lblMostrarFechaM').text(moment(jdata.Fecha).format('DD-MM-YYYY'));
    //$('#lblMostrarHora').text(moment(jdata.Fecha).format('HH:mm'));
    if (jdata.Observaciones!=null) {
        $('#lblMostrarObservacion').text('\u00a0' + jdata.Observaciones.toUpperCase());
    }    
    $('#txtUsuarioCreacion').text('\u00a0' + jdata.UsuarioIngresoLog.toUpperCase());
    $('#txtFechaCreacion').text('\u00a0' + moment(jdata.FechaIngresoLog).format('DD-MM-YYYY'));
    if (jdata.AprobadoPor == null) {
        jdata.AprobadoPor = '';
    }

    if (jdata.FechaAprobacion != null) {
        jdata.FechaAprobacion = moment(jdata.FechaAprobacion).format('DD-MM-YYYY HH:mm');
    } else if (jdata.FechaAprobacion == null) {
        jdata.FechaAprobacion = '';
    }
    $('#txtUsuarioAprobacion').text('\u00a0' + jdata.AprobadoPor);
    $('#txtFechaAprobacion').text('\u00a0' + jdata.FechaAprobacion);
    $.ajax({
        url: "../CloroCisternaDescongelado/ReporteCloroCisternaDescongeladoPartial",//MUESTRO EL DETALLE DE LA FILA SELECCIONADA
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            idCloroCisterna: jdata.IdCloroCisterna,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divBotones').prop('hidden', true);
                $("#divMostarTablaCabecera").prop('hidden', false);
                $("#divCardMostrarDetalle").prop('hidden', true);
                MensajeAdvertencia('No existen registro de DETALLE');
            } else {
                $("#divMostarTablaCabecera").prop('hidden', true);
                $("#divCardMostrarDetalle").prop('hidden', false);
                $('#divBotones').prop('hidden', false);
                $("#divMostarTablaDetalle").html(resultado);
            }
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function printDiv() {
    window.print();
}

function Atras() {
    $('#cargac').show();
    $('#divBotones').prop('hidden', true);
    $("#divMostarTablaCabecera").prop('hidden', false);
    $("#divCardMostrarDetalle").prop('hidden', true);
    $("#divMostarTablaDetalle").html('');
    
        $('#cargac').hide();
    
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
        maxDate: moment().add(1,'days'),
        ranges: {
            'Mañana': [moment().add(1,'days'), moment().add(1,'days')],
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