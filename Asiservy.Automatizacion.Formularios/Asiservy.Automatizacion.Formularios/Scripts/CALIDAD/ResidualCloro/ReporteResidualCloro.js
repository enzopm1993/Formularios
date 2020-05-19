

var model = [];

function FiltrarAprobadosFecha() {
    $('#divPartialControl').html('');
    $("#spinnerCargando").prop("hidden", false);
    $("#MensajeRegistros").html('');

    $.ajax({
        url: "../ResidualCloro/ReporteResidualCloroControlPartial",
        type: "GET",
        data: {
            FechaDesde: $("#fechaDesde").val(),
            FechaHasta: $("#fechaHasta").val()
        },
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
                $("#MensajeRegistros").html(Mensajes.SinRegistros);
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

function SeleccionarBandeja(Control) {
    //console.log(Control);
    model = Control;

    if (!model.EstadoReporte) {
        MensajeAdvertencia("Reporte se encuentra pendiente.");
        return;
    }
    $("#btnImprimir").prop("hidden", false);
    $("#btnAtras").prop("hidden", false);
    $("#btnConsultar").prop("hidden", true);
    $("#divMensaje").html('');
    $("#divCabeceras").prop("hidden", true);
    $("#divDetalle").prop("hidden", true);
    $("#lblLomos").html('');
    $("#divDetalle").prop("hidden", false);
    $("#divDetalle2").prop("hidden", false);

    // console.log(model);

    $("#divMensaje").html('');
    if (model.Lomos) {
        $("#lblLomos").html("<i class='fas fa-check-circle' style='color:#1cc88a'></i>");
    }
    if (model.Latas) {
        $("#lblLatas").html("<i class='fas fa-check-circle' style='color:#1cc88a'></i>");
    }
   

    $("#txtUsuarioCreacion").html(model.UsuarioIngresoLog);
    $("#txtFechaCreacion").html(moment(model.FechaIngresoLog).format("DD-MM-YYYY HH:mm"));
    $("#txtUsuarioAprobacion").html(model.AprobadoPor);
    $("#txtFechaAprobacion").html(moment(model.FechaAprobacion).format("DD-MM-YYYY HH:mm"));
    $("#txtCodDetectorMetal").val(model.DetectorMetal);


    ConsultarReporte();
}

function Atras() {
    $("#btnAtras").prop("hidden", true);
    $("#btnImprimir").prop("hidden", false);
    $("#divCabeceras").prop("hidden", false);
    $("#divDetalle").prop("hidden", true);
    $("#btnConsultar").prop("hidden", false);

}


function ConsultarReporte() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '' || $("#selectArea").val() == '' ) {
        return;
    }
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../ResidualCloro/ReporteResidualCloroPartial",
        type: "GET",
        data: {
            Fecha: model.Fecha,
            Area: model.Area
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
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