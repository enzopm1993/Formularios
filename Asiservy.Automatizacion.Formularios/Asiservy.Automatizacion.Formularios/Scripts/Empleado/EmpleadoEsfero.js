

$(document).ready(function () {
    CargarTablaEmpleadoEsfero();
    $('#SelectEmpleado').val(0);  

});



function CargarTablaEmpleadoEsfero() {
    $.ajax({
        url: "../Empleado/EmpleadoEsferoPartial",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#DivTableEmpleadoEsferoPartial');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}

function CambioEstadoRegistro(valor) {
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}