

function CargarReporteDistribucion() {
    $("#validaFecha").prop("hidden", true);
    var linea = $('#selectLinea').val();   
    var fecha = $("#txtFecha").val();
    var selectTurno = $("#selectTurno").val();
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
                Fecha: fecha,
                Turno: selectTurno
               
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                var bitacora = $('#DivTableReporteDistribucion');
                $("#spinnerCargando").prop("hidden", true);
                if (resultado == "1") {
                    MensajeAdvertencia("Faltan parametros");
                    return;
                }

                bitacora.html(resultado);
                config.opcionesDT.pageLength = 15;
                $('#tblDataTable').DataTable(config.opcionesDT);

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