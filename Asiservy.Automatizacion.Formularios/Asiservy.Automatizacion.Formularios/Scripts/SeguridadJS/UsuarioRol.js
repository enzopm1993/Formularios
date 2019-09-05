$(document).ready(function () {
    CargarUsuarioRol();
    //Nuevo();
});


function CargarUsuarioRol() {
    $.ajax({
        url: "../Seguridad/UsuarioRolPartial",
        type: "GET",
        success: function (resultado) {
            var bitacora = $('#DivTableUsuarioRol');
            bitacora.html(resultado);

        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}