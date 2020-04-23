var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();   
});

//CARGAR BANDEJA
function CargarBandeja() {
    $('#cargac').show();
    var op = 3;
    if ($('#selectEstadoReporte').val() == 'true') {
        op = 2;
    }
    $.ajax({
        url: "../LavadoCisterna/BandejaLavadoCisternaPartial",
        type: "GET",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            idLavadoCisterna: 0,
            op: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostrarCabeceras").html("No existen registros");
            } else {
                $("#divMostrarCabeceras").html('');
                //$("#ModalApruebaPendiente").modal("show");
                $("#divMostrarCabeceras").html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    $('#cargac').show();
    listaDatos = model;
    if (model.EstadoReporteCab == true) {
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#btnPendiente').prop('hidden', true);
        $('#btnAprobado').prop('hidden', false);
    }
    var op = 4;
    $.ajax({
        url: "../LavadoCisterna/BandejaLavadoCisternaAprobarPartial",
        type: "GET",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            idLavadoCisterna: listaDatos.IdLavadoCisterna,
            op: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTblAprobarPendiente").html("No existen registros");
            } else {
                $("#tblAprobarPendientePartial").html('');
                $("#ModalApruebaPendiente").modal("show");
                $("#divAprobarPendientePartial").html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function AprobarPendiente(estadoReporte) {
    var siAprobar = 1;//en la condicion del la clase clsD se envia a actualizar solo la columna EstadoReporte 
    $.ajax({
        url: "../LavadoCisterna/GuardarBandejaModificarLavadoCisterna",
        type: "POST",
        data: {
            IdLavadoCisterna: listaDatos.IdLavadoCisterna,
            EstadoReporte: estadoReporte,
            siAprobar: siAprobar
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 1) {
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else {
                MensajeError('El registro no debe guardarse- solo actualizarce- Controller: GuardarModificarControlCuchilloPreparacion');
                return;
            }

            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

//DATE RANGE PICKER
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