

function GenerarBarcos() {

    $("#btnGenerar").prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);

    $.ajax({
        url: "../GenerarDatosServicio/GenerarBarco",
        type: "GET",
        success: function (resultado) {
            $("#divMensaje").html("<h3>" + resultado + "</h3>");
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $("#btnGenerar").prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}