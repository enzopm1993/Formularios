

$(document).ready(function () {
    CargarOpciones();
});

function CargarOpcion(id, nombre, formulario, clase, padre, estado) {
    console.log(id, nombre, formulario, clase, padre, estado);
    $('#IdOpcion').val(id);
    $('#Nombre').val(nombre);
    $('#Formulario').val(formulario);
    $('#Clase').val(clase);
    $('#Padre').val(padre);
    if (estado == 'A') {
        $('#defaultUnchecked').checked=true;
        $('#EstadoRegistro').val('Activo');
    }
    else {
        $('#defaultUnchecked').checked=false;
        $('#EstadoRegistro').val('Inactivo');
    }
}

function CargarOpciones() {
    $.ajax({
        url: "../Seguridad/OpcionPartial",
        type: "GET",
        success: function (resultado) {
            var bitacora = $('#DivTableOpciones');            
            bitacora.html(resultado);
            
        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}
