$(document).ready(function () {
    ConsultarReporte();
});


function ConsultarReporte() {
    $("#chartDetalle").html('');
    if ($("#txtFecha").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    //  CargarOrdenFabricacion();
    $.ajax({
        url: "../MapeoProductoTunel/ReporteMapeoProductoTunelPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "0") {
                $("#chartDetalle").html("No existen registros");
            } else {
                $("#chartDetalle").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = false;
                config.opcionesDT.ordering = false;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}