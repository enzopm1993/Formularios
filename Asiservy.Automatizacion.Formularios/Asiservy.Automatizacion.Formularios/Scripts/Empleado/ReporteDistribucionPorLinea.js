

function CargarReporteDistribucion() {
    $("#validaFecha").prop("hidden", true);
    var linea = $('#selectLinea').val();   
    var fecha = $("#txtFecha").val();
    if (fecha == '') {
        $("#validaFecha").prop("hidden", false);
        return;
    }
    if (linea > 0) {
        $("#spinnerCargando").prop("hidden", false);
        var bitacora = $('#DivTableReporteDistribucion');
        bitacora.html('');
        $.ajax({
            url: "../Empleado/ReporteDistribucionPorLineaPartial",
            type: "GET",
            data: {
                Linea: linea,
                Fecha: fecha
               
            },
            success: function (resultado) {

                var bitacora = $('#DivTableReporteDistribucion');
                $("#spinnerCargando").prop("hidden", true);
                if (resultado == "1") {
                    MensajeAdvertencia("Faltan parametros");
                    return;
                }

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