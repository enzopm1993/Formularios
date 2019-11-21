﻿
$(document).ready(function () {
    CargarReporte();
    
});

function CargarReporte() {

    if ($('#txtFecha').val() == "") {
        MensajeAdvertencia("Ingrese una fecha");
        return;
    }

    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableReporteProyeccion').html('');
    $.ajax({
        url: "../ProyeccionProgramacion/ReporteProyeccionProgramacionPartial",
        type: "GET",
        data: { Fecha: $('#txtFecha').val() },
        success: function (resultado) {     
            $('#DivTableReporteProyeccion').html(resultado);
            $("#spinnerCargando").prop("hidden", true);
            $('#tblDataTable').DataTable(config.opcionesDT);
            config.opcionesDT.pageLength = 15;
        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

