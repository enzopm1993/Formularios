$(document).ready(function () {
    ReporteControlCuchilloPreparacionPartial(0);
});
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

function ReporteControlCuchilloPreparacionPartial(opcion) {
    $('#cargac').show();
    var op = opcion;
    var table = $('#tblDataTable');    
    table.DataTable().destroy();
    table.DataTable().clear(); 
    $.ajax({
        url: "../ControlCuchillosPreparacion/ReporteControlCuchilloPreparacionPartial",
        type: "GET",
        data: {
            filtroFechaDesde: $("#fechaDesde").val(),
            filtroFechaHasta: $("#fechaHasta").val(),
            opcion:op
        },
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
            }
            $("#tblDataTable tbody").empty(); 
            config.opcionesDT.order = [1, 'desc'];
            config.opcionesDT.buttons = [];
            //config.opcionesDT.buttons = [{
            //    extend: 'print',
            //    text: '<img style="width:100%" src="../../Content/icons/print24.png" />',
            //    titleAttr: 'Imprimir'
            //}];
            table.DataTable().destroy();
            table.DataTable(config.opcionesDT);
            table.DataTable().clear();
            $('#DivReporteCuchillos').html(resultado);   
            setTimeout(function () {
                $('#cargac').hide();
            },200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function PrintReport(op) {
    localStorage.setItem("fechaDesde", $("#fechaDesde").val());
    var url = $("#RedirectTo").val() + '?' + 'filtroFechaDesde=' + $("#fechaDesde").val() + '&filtroFechaHasta=' + $("#fechaHasta").val() + '&op=' + op;
    var win = window.open(url, '_blank');    
}

