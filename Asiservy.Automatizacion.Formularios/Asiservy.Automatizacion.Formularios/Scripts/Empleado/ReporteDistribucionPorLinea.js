

function CargarReporteDistribucion(linea) {
    if (linea > 0) {
        $.ajax({
            url: "../Empleado/ReporteDistribucionPorLineaPartial",
            type: "GET",
            data: { Linea: linea },
            success: function (resultado) {
                var bitacora = $('#DivTableReporteDistribucion');
                bitacora.html(resultado);
            },
            error: function (resultado) {
                MensajeError(resultado.responseJSON, false);
                var bitacora = $('#DivTableReporteDistribucion');
                bitacora.html('');
            }
        });
    } else {

        var bitacora = $('#DivTableReporteDistribucion');
        bitacora.html('');
    }

}