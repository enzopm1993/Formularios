$(document).ready(function () {
    CargarProductoTerminado();

});

function CargarProductoTerminado() {
    $("#chartCabecera2").html('');
    if ($("#txtFechaPaletizado").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
  
   // CargarOrdenFabricacion();
    $.ajax({
        url: "../EntregaProductoTerminado/BandejaControlCalidadPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFechaPaletizado").val()           

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}