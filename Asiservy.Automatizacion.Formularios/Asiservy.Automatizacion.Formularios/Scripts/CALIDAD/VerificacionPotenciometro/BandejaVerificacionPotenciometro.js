﻿var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
});

//CARGAR BANDEJA
function CargarBandeja() {
    $("#spinnerCargando").prop("hidden", false);
    $("#txtFechaAprobacion").prop("hidden", true);
    $('#divPartialControl').html('');
    $.ajax({
        url: "../VerificacionPotenciometro/BandejaVerificacionPotenciometroPartial",
        type: "GET",
        success: function (resultado) {
            $('#divPartialControl').html('');
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
                $('#divPartialControl').html(resultado);
            }
            $("#btnPendiente").prop("hidden", true);
            $("#btnAprobado").prop("hidden", false);
            $("#btnReversar").prop("hidden", true);
            $("#spinnerCargando").prop("hidden", true);
            config.opcionesDT.pageLength = 10;
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    // console.log(model);
    //  CargarControlDetalle(model.IdVerificacionPotenciometro);
    if ($("#selectEstadoRegistro").val() == "false") {
        $("#txtFechaAprobacion").prop("hidden", false);
    } else {
        $("#txtFechaAprobacion").prop("hidden", true);
    }
    $("#ModalApruebaCntrol").modal("show");
    listaDatos = model;

    $("#txtModelo").val(model.Modelo);
    $("#txtSerie").val(model.Serie);
    $("#txtNaci1").val(model.NaCI1);
    $("#txtNaci2").val(model.NaCI2);
    $("#txtNaci3").val(model.NaCI3);

}

function AprobarControl() {
    //var estadoReporte = data;
    //console.log(listaDatos);
    //console.log(moment($("#txtFechaAprobacion").val()) < moment(listaDatos.FechaIngresoLog));
    if (moment($("#txtFechaAprobacion").val()) < moment(listaDatos.Fecha)) {
        MensajeAdvertencia("Fecha de Aprobación no puede ser menor a la Fecha de Creación.");
        return;
    }
    if (moment($("#txtFechaAprobacion").val()).format('YYYY-MM-DD') > moment().format('YYYY-MM-DD')) {
        MensajeAdvertencia("Fecha de Aprobación no puede ser mayor a la fecha actual.");
        return;
    }

    $.ajax({
        url: "../VerificacionPotenciometro/AprobarBandejaControlCloro",
        type: "POST",
        data: {
            IdVerificacionPotenciometroControl: listaDatos.IdVerificacionPotenciometroControl,
            Fecha: listaDatos.Fecha,
            FechaAprobacion: $("#txtFechaAprobacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }

            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                return;
            } 
            MensajeCorrecto(resultado);
            CargarBandeja();
            $("#ModalApruebaCntrol").modal("hide");
        },
        error: function (resultado) {
            MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
        }
    });
}


function ReversarControl() {
    //var estadoReporte = data;
    $.ajax({
        url: "../VerificacionPotenciometro/ReversarBandejaControl",
        type: "POST",
        data: {
            IdVerificacionPotenciometroControl: listaDatos.IdVerificacionPotenciometroControl,
            Fecha: listaDatos.Fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                return;
            } 
            MensajeCorrecto(resultado);
            FiltrarAprobadosFecha();
            $("#ModalApruebaCntrol").modal("hide");
        },
        error: function (resultado) {
            MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
        }
    });
}

function FiltrarAprobadosFecha() {
    if ($("#selectEstadoRegistro").val() == 'false') {
        $("#divDateRangePicker").prop('hidden', true);
        CargarBandeja();

    } else {
        $('#divPartialControl').html('');
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../VerificacionPotenciometro/BandejaVerificacionPotenciometroPartial",
            type: "GET",
            data: {
                FechaDesde: $("#fechaDesde").val(),
                FechaHasta: $("#fechaHasta").val(),
                Estado: $("#selectEstadoRegistro").val()
            },
            success: function (resultado) {
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
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
                MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
            }
        });
    }
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
        FiltrarAprobadosFecha();
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