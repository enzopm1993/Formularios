var start = moment().subtract(1, 'days');
var end = moment().subtract(1, 'days');
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
    $("#txtFechaDesde").val(start.format('YYYY-MM-DD'));
    $("#txtFechaHasta").val(end.format('YYYY-MM-DD'));



    $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
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

$(document).ready(function () {
    ConsultarReporte();
});


function ConsultarReporte() {
    $("#chartDetalle").html('');
    if ($("#txtFechaDesde").val() == '' || $("#txtFechaHasta").val() == '' || $("#selectTurno").val()=='') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    //  CargarOrdenFabricacion();
    $.ajax({
        url: "../MapeoProductoTunel/ReporteMapeoProductoTunelPartial",
        type: "GET",
        data: {
            FechaDesde: $("#txtFechaDesde").val(),
            FechaHasta: $("#txtFechaHasta").val(),
            Turno: $("#selectTurno").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "0") {
                $("#chartDetalle").html("No existen registros");
            } else {
                $("#chartDetalle").html(resultado);
                config.opcionesDT.pageLength = -1;
                //config.opcionesDT.order = false;
                //config.opcionesDT.ordering = false;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}