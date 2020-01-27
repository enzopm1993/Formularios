
function CargarReportePersonalPresente() {
    $("#validaFecha").prop("hidden", true);
  
    var fecha = $("#txtFecha").val();
    if (fecha == '') {
        $("#validaFecha").prop("hidden", false);
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    var bitacora = $('#DivTableReportePersonalPresente');
    bitacora.html('');
    $.ajax({
        url: "../Empleado/ReportePersonalPresentePartial",
        type: "GET",
        data: {
            Fecha: fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            var bitacora = $('#DivTableReportePersonalPresente');
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "1") {
                MensajeAdvertencia("Faltan parametros");
                return;
            }

            bitacora.html(resultado);
            config.opcionesDT.order = [[0, "asc"], [1, "asc"]],
            config.opcionesDT.pageLength = -1;
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {

            MensajeError(resultado.responseText, false);
            var bitacora = $('#DivTableReportePersonalPresente');
            $("#spinnerCargando").prop("hidden", true);

            bitacora.html('');

        }
    });

}