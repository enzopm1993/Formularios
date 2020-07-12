$(document).ready(function () {
 
    $("#Linea").select2({
        width: '100%' // need to override the changed default
    });

});
function Limpiar() {
    $('#Linea').prop('selectedIndex', 0).change();
    $('#Turno').prop('selectedIndex', 0);
    $('#divRptAsistencia').empty();
    $('#FechaDesde').val(moment().format('YYYY-MM-DD'));
    $('#FechaHasta').val(moment().format('YYYY-MM-DD'));
    
}
function ConsultarAsistencia() {
    $('#divRptAsistencia').empty();
    if ($('#FechaDesde').val() == '') {
        $('#msjerrorFecha1').show();
        return false;
    } else {
        $('#msjerrorFecha1').hide();
    }
    if ($('#Linea').val() == '') {
        $('#msjerrorLinea').show();
        return false;
    } else {
        $('#msjerrorLinea').hide();
    }
    if ($('#Turno').val() == '') {
        $('#msjerrorTurno').show();
        return false;
    } else {
        $('#msjerrorTurno').hide();
    }
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Asistencia/RptAsistenciaPartial",
        type: "GET",
        data:
        {
            FechaInicio: $('#FechaDesde').val(),
            FechaFin: $('#FechaHasta').val(),
            Linea: $('#Linea').val(),
            Turno: $('#Turno').val()
        },
        success: function (resultado) {
            $('#mensajeregistros').html('');
            $("#spinnerCargando").prop("hidden", true);
            $('#divRptAsistencia').html(resultado);
            if ($('#contempleados').val() == '0') {
                $('#divRptAsistencia').empty();
                $('#mensajeregistros').html('No existen Registros a Mostrar');
            }
            config.opcionesDT.pageLength = 15;
            config.opcionesDT.order = false;
            config.opcionesDT.ordering = false;
            $('#tblDataTable').DataTable(config.opcionesDT);
            //MensajeCorrecto("Registro ingresado con éxito", false);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
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
        $("#FechaDesde").val(start.format('YYYY-MM-DD'));
        $("#FechaHasta").val(end.format('YYYY-MM-DD'));

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