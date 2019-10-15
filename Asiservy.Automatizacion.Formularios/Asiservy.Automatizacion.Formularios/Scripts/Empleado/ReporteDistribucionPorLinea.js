﻿

function CargarReporteDistribucion() {
    var linea = $('#selectLinea').val();
    var proceso = $('#txtProceso').val();
    var bool = false;
    if (proceso == 1)
        bool = true;
    console.log(proceso);
    if (linea > 0) {
        $('#btnGuardarCargando').prop("hidden", false);
        $('#btnGuardar').prop("hidden", true);
        $.ajax({
            url: "../Empleado/ReporteDistribucionPorLineaPartial",
            type: "GET",
            data: {
                Linea: linea,
                Proceso: bool
            },
            success: function (resultado) {
                var bitacora = $('#DivTableReporteDistribucion');
                bitacora.html(resultado);
                $('#btnGuardarCargando').prop("hidden", true);
                $('#btnGuardar').prop("hidden", false);
            },
            error: function (resultado) {
             
                MensajeError(resultado.responseText, false);
                var bitacora = $('#DivTableReporteDistribucion');
                bitacora.html('');
                $('#btnGuardarCargando').prop("hidden", true);
                $('#btnGuardar').prop("hidden", false);
            }
        });
    } else {

        var bitacora = $('#DivTableReporteDistribucion');
        bitacora.html('');
    }

}