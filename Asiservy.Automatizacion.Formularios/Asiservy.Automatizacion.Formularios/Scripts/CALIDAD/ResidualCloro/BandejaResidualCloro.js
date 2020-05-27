var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
});
    
//CARGAR BANDEJA
function CargarBandeja() {
    $("#spinnerCargando").prop("hidden", false);
    $('#divPartialControlCloro').html('');
    $.ajax({
        url: "../ResidualCloro/BandejaResidualCloroPartial",
        type: "GET",
        success: function (resultado) {
            $('#divPartialControlCloro').html('');
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
                $('#divPartialControlCloro').html(resultado);
            }
            $("#btnPendiente").prop("hidden", true);
            $("#btnAprobado").prop("hidden", false);
            $("#spinnerCargando").prop("hidden", true);
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(Mensajes.Error, false);
        }
    });
}

function SeleccionarBandejaControl(model) {
    if ($("#selectEstadoRegistro").val() =="false") {
        $("#txtFechaAprobacion").prop("hidden", false);
    } else {
        $("#txtFechaAprobacion").prop("hidden", true);
    }
    $("#divTableReporte").html('');
    listaDatos = model;
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
           // console.log(resultado);
            if (resultado == 0) {
                MensajeAdvertencia("¡El REGISTRO no tiene detalle, por favor ingrese los datos en el CONTROL!");
                return;
            } else {
                $("#divTableReporte").html(resultado);
                $("#ModalApruebaCntrolCloro").modal("show");
            }
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error, false);
            $("#spinnerCargandoMaterial").prop("hidden", true);
        }
    });
}

function AprobarControlCloroDetalle() {
    if (moment($("#txtFechaAprobacion").val()) < moment(listaDatos.Fecha)) {
        MensajeAdvertencia("Fecha de Aprobación no puede ser menor a la Fecha de Creación.");
        return;
    }
    if (moment($("#txtFechaAprobacion").val()) > moment()) {
        MensajeAdvertencia("Fecha de Aprobación no puede ser mayor a la fecha actual.");
        return;
    }
 $.ajax({
        url: "../ResidualCloro/AprobarBandejaControlCloro",
        type: "POST",
        data: {
            IdResidualCloroControl: listaDatos.IdResidualCloroControl,
            Fecha: listaDatos.Fecha,
            Area: listaDatos.Area,  
            FechaAprobacion: $("#txtFechaAprobacion").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            FiltrarAprobadosFecha();
            $("#ModalApruebaCntrolCloro").modal("hide");
            listaDatos = [];
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error, false);
        }
    });
}


function ReversarBandejaControlCloro() {
    //var estadoReporte = data;
    $.ajax({
        url: "../ResidualCloro/ReversarBandejaControlCloro",
        type: "POST",
        data: {
            IdResidualCloroControl: listaDatos.IdResidualCloroControl,
            Fecha: listaDatos.Fecha,
            Area: listaDatos.Area

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            FiltrarAprobadosFecha();
            $("#ModalApruebaCntrolCloro").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error, false);
        }
    });
}


function FiltrarAprobadosFecha() {
    if ($("#selectEstadoRegistro").val() == 'false') {
        $("#divDateRangePicker").prop('hidden', true);
        CargarBandeja();

    } else {
        $('#divPartialControlCloro').html('');
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../ResidualCloro/BandejaResidualCloroPartial",
            type: "GET",
            data: {
                FechaDesde: $("#fechaDesde").val(),
                FechaHasta: $("#fechaHasta").val(),
                Estado: $("#selectEstadoRegistro").val()
            },
            success: function (resultado) {
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                    $('#divPartialControlCloro').html('');

                } else {
                    $('#MensajeRegistros').hide();
                    $('#divPartialControlCloro').html(resultado);
                }
                $("#spinnerCargando").prop("hidden", true);
                $("#btnPendiente").prop("hidden", false);
                $("#btnAprobado").prop("hidden", true);
                $("#divDateRangePicker").prop('hidden', false);
            },
            error: function (resultado) {
                $("#spinnerCargando").prop("hidden", true);
                MensajeError(Mensajes.Error, false);
            }
        });
    }
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
});