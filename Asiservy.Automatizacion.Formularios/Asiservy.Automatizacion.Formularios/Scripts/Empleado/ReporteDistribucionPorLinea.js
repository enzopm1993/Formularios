

function CargarReporteDistribucion() {
    var linea = $('#selectLinea').val();   
    
    if (linea > 0) {
        $("#spinnerCargando").prop("hidden", false);
        var bitacora = $('#DivTableReporteDistribucion');
        bitacora.html('');
        $.ajax({
            url: "../Empleado/ReporteDistribucionPorLineaPartial",
            type: "GET",
            data: {
                Linea: linea
               
            },
            success: function (resultado) {
                var bitacora = $('#DivTableReporteDistribucion');
                $("#spinnerCargando").prop("hidden", true);
                bitacora.html(resultado);
          

            },
            error: function (resultado) {
             
                MensajeError(resultado.responseText, false);
                var bitacora = $('#DivTableReporteDistribucion');
                $("#spinnerCargando").prop("hidden", true);

                bitacora.html('');

            }
        });
    } else {

        var bitacora = $('#DivTableReporteDistribucion');
        bitacora.html('');
    }

}