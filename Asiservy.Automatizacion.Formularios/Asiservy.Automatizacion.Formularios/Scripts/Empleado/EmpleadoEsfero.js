

$(document).ready(function () {
    CargarTablaEmpleadoEsfero();
    $('#SelectEmpleado').val(0);  

});



function SeleccionEmpleadoEsfero(cedula, numero, estado) {

    $('#SelectEmpleado').val(cedula);
    $('#NumeroEsfero').val(numero);
   
    if (estado == 'A') {
        $('#CheckEstadoRegistro').prop('checked', true);
        // console.log($('#LabelEstado').val());
        $('#LabelEstado').text('Activo');

    }
    else {
        $('#CheckEstadoRegistro').prop('checked', false);
        $('#LabelEstado').text('Inactivo');
    }

}



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