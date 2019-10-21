

$(document).ready(function () {
    console.log("prueba");

    CargarEmpleadoCuchillo();
    NuevoCuchilloEmpleado();
});

function NuevoCuchilloEmpleado() {
    $('#IdEmpleadoCuchillo').val("0");
    $("#SelectEmpleado").prop('selectedIndex', 0);
    $('#SelectCuchilloBlanco').prop('selectedIndex', 0);
    $('#SelectCuchilloRojo').prop('selectedIndex', 0);
    $('#SelectCuchilloNegro').prop('selectedIndex', 0);
    $('#NombreEmpleado').val("");
    $('#Identificacion').val("");
    
}

function SeleccionEmpleadoCuchillo(id, cedula, blanco,rojo, negro, estado, nombre) {
    
    $('#IdEmpleadoCuchillo').val(id);
    $('#SelectEmpleado').val(cedula);
    $('#SelectCuchilloBlanco').val(blanco);
    $('#SelectCuchilloRojo').val(rojo);
    $('#SelectCuchilloNegro').val(negro);
    $('#NombreEmpleado').val(nombre);
    $('#Identificacion').val(cedula);
   
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


function CargarEmpleadoCuchillo() {
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Asistencia/CuchilloEmpleadoPartial",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#DivTableEmpleadoCuchillo');
            $("#spinnerCargando").prop("hidden", true);

            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}
