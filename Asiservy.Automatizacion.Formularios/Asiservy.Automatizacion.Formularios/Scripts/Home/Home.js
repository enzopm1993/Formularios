var Com = [];
$(document).ready(function () {
    ConsultaComunicados();
});

function ConsultaComunicados() {
    $.ajax({
        url: "../Home/ConsultaComunicados",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divComunicados").html('')
                return;
            }
            $("#divComunicados").html(resultado);
            $("#divComunicados").css("height","100px");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarComunicado(Contenido) {
    $("#modalBodyComunicados").html('');
    $("#modalBodyComunicados").html(Contenido);
    $("#ModalComunicado").modal("show");
}