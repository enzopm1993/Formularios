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



function CargarReporteAvance() {
    //var txtFecha = $('#txtFecha').val();
    var txtFechaDesde = $('#txtFechaDesde').val();
    var txtFechaHasta = $('#txtFechaHasta').val();
    var selectLinea = $('#selectLinea').val();
    var bitacora = $('#DivTableReporteControlAvance');
    bitacora.html('');

    //console.log(txtFechaDesde);
    //console.log(txtFechaHasta);

   
    if (txtFechaDesde == "" || txtFechaHasta == "") {
        MensajeAdvertencia("Igrese un rango de fechas");
        return;
    }
    if (txtFechaDesde > txtFechaHasta) {
        MensajeAdvertencia("Fecha hasta no puede ser menor a fecha desde");
        return;
    }
    if (selectLinea == "") {
        MensajeAdvertencia("Seleccione una Linea");
        return;
    }
    if ($("#selectTurno").val() == "") {
        MensajeAdvertencia("Seleccione un turno");
        return;
    }
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Avance/ReporteControlAvanceDiarioPartial",
        type: "GET",
        data: {
            ddFechaDesde: txtFechaDesde,
            ddFechaHasta: txtFechaHasta,
            Turno: $("#selectTurno").val(),
            dsLinea: selectLinea
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#btnConsultar").prop("disabled", false);
            if (resultado == "1") { 

                MensajeAdvertencia("No existen registros para esa linea");
                $("#spinnerCargando").prop("hidden", true);

            } else {
                var bitacora = $('#DivTableReporteControlAvance');
                $("#spinnerCargando").prop("hidden", true);
                bitacora.html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
           
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);


        }
    });

}