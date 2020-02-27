﻿$(document).ready(function () {
 
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