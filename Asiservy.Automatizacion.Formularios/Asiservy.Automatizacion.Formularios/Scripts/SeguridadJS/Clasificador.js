
$(document).ready(function () {
    CargarTablaClasificadores();
   // Nuevo();
});



function CambioEstado(valor) {
    if (valor) {
        $('#Grupo').attr("disabled", true);
        //$('#Grupo').val("0");
        $('#Grupo option').eq(0).prop('selected', true);
    } else {

        $('#Grupo').attr("disabled", false);
    } 

}



function CargarTablaClasificadores() {
    $.ajax({
        url: "../Seguridad/ClasificadorPartial",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#DivTableClasificador');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}