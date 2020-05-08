﻿$(document).ready(function () {
    CargarCabecera(1);

});
function CargarCabecera(op) {
    if ($("#fechaDesde").val() == '') {
        var date = new Date();
        $("#fechaDesde").val(moment(date).format('YYYY-MM-DD'))
        $("#fechaHasta").val(moment(date).format('YYYY-MM-DD'))
    }
    $("#firmaDigital").prop("hidden", true);
    if ($("#fechaDesde").val() == $("#fechaHasta").val()) {
        ConsultarID();
    }
    $('#cargac').show();
    $.ajax({
        url: "../HigieneComedorCocina/ReporteHigieneComedorCocinaPartail",
        data: {
            fechaDesde: $("#fechaDesde").val(),
            fechaHasta: $("#fechaHasta").val(),
            idControlHigiene: 0,
            op: op
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
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
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

function PrintReport(op) {
    var url = $("#RedirectTo").val() + '?' + 'fechaDesde=' + $("#fechaDesde").val() + '&fechaHasta=' + $("#fechaHasta").val() + '&idControlHigiene=' + 0 + '&op=' + op;
    var win = window.open(url, '_blank');
}

function ConsultarID() {
    $.ajax({
        url: "../HigieneComedorCocina/ConsultarHigieneControl",
        type: "GET",
        data: {
            fecha: $("#fechaDesde").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado.length != 0) {
                ConsultarFirma(resultado[0].IdControlHigiene, resultado[0].AprobadoPor);
            }          
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function ConsultarFirma(idControlHigiene, aprobadoPor) {
    $.ajax({
        url: "../HigieneComedorCocina/BandejaConsultarImagenFirma",
        type: "GET",
        data: {
            idControlHigiene: idControlHigiene
        },
        success: function (resultado) {
            $("#btnGuardarFirma").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '0') {
                $("#firmaDigital").prop("hidden", false);
                document.getElementById('ImgFirma').src = resultado;
                $('#div_ImagenFirma').prop('hidden', false);  
                $("#lblMostrarFirma").text(aprobadoPor);
                //lblMostrarFirma
            }          
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}