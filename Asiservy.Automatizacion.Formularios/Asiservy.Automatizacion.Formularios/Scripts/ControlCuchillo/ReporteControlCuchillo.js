﻿




function CargarReporteControlCuchillo() {
    var fecha = $('#txtFecha').val();
    var linea = $('#selectLinea').val();
    if (fecha == '' || linea == '') {

        MensajeAdvertencia("Ingrese filtros de busqueda");
        return;
    }
    var bitacora = $('#DivTableControlCuchillo');
    bitacora.html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ControlCuchillo/ReporteControlCuchilloPartial",
        type: "GET",
        data: {
            Fecha: fecha,
            Linea: linea,
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {

            var bitacora = $('#DivTableControlCuchillo');
            $("#spinnerCargando").prop("hidden", true);

            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
            var bitacora = $('#DivTableControlCuchillo');
            bitacora.html('');
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}