$(document).ready(function () {
    CargarOpciones();
});

function Nuevo() {
    $('#IdOpcion').val('0');
    $('#Nombre').val('');
    $('#Formulario').val('');
    $('#Clase').prop('selectedIndex', 0);
    $('#Padre').prop('selectedIndex', 0);
    $('#CheckEstadoRegistro').prop('checked', true);   
    $('#LabelEstado').text('Activo');
}

function CargarOpcion(id, nombre, formulario, clase, padre, estado) {
    //console.log(id, nombre, formulario, clase, padre, estado);
    $('#IdOpcion').val(id);
    $('#Nombre').val(nombre);
    $('#Formulario').val(formulario);

    if(clase=='P')
        $('#Clase').prop('selectedIndex', 1);
    else
        $('#Clase').prop('selectedIndex', 0);

    if(padre!='')
        $('#Padre').val(padre);

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
