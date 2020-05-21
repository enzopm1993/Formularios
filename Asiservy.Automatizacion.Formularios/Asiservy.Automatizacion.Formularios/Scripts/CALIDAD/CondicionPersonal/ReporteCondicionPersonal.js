$(document).ready(function () {
   // FiltrarAprobadosFecha();
});


function FiltrarAprobadosFecha() {
    $('#divPartialControl').html('');
    $("#spinnerCargando").prop("hidden", false);
    $("#MensajeRegistros").html('');

    $.ajax({
        url: "../CondicionPersonal/ReporteControlCondicionPersonalPartial",
        type: "GET",
        data: {
            FechaDesde: $("#fechaDesde").val(),
            FechaHasta: $("#fechaHasta").val()
        },
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
                $("#MensajeRegistros").html(Mensajes.SinRegistrosRangoFecha);
                $('#divPartialControl').html('');

            } else {
                $('#MensajeRegistros').hide();
                $('#divPartialControl').html(resultado);
            }
            $("#spinnerCargando").prop("hidden", true);
            $("#btnPendiente").prop("hidden", false);
            $("#btnReversar").prop("hidden", false);
            $("#btnAprobado").prop("hidden", true);
            $("#divDateRangePicker").prop('hidden', false);
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(Mensajes.Error + resultado.responseText, false);
        }
    });

}




function ConsultarReporte(model) {
    //if (!model.EstadoReporte) {
    //    MensajeAdvertencia(Mensajes.ReportePendiente);
    //    //return;
    //}

    $("#txtMensaje").html('');
    $("#btnAtras").prop("hidden", false);
    $("#btnConsultar").prop("hidden", true);
    $("#tblTitulo2").prop("hidden", false);
    $("#divCabeceras").prop("hidden", true); 
   
    $("#spinnerCargando").prop("hidden", false);
 
    $.ajax({
        url: "../CondicionPersonal/ReporteCondicionPersonalPartial",
        type: "GET",
        data: {
            Fecha: model.Fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divDetalle").prop("hidden", false);
            if (resultado == "0") {
                $("#txtMensaje").html(Mensajes.SinRegistros);
                $("#spinnerCargando").prop("hidden", true);
                $("#divDetallePartial").html('');   

            } else {
                $("#btnImprimir").prop("hidden", false);
                $("#spinnerCargando").prop("hidden", true);
                $("#divDetallePartial").html(resultado);   
            }

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}


function Atras() {
    $("#divCabeceras").prop("hidden", false);
    $("#btnAtras").prop("hidden", true);
    $("#btnImprimir").prop("hidden", true);
    $("#btnConsultar").prop("hidden", false);
    $("#divDetalle").prop("hidden", true);
    $("#divDetallePartial").html('');   
    FiltrarAprobadosFecha();
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
    window.print();
}