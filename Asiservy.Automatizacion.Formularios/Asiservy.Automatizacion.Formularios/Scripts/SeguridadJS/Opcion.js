$(document).ready(function () {
    CargarOpciones();
    Nuevo();
});

function CambioClase(valor) {
    if (valor == "1") {
        $('#Padre').prop('selectedIndex', 0);
        $('#Url').val('');
        $('#divPadre').hide();
        $('#divUrl').hide();

    } else {
        $('#Url').val('');
        $('#Padre').prop('selectedIndex', 0);
        $('#divUrl').show();
        $('#divPadre').show();

    }

}

function CambioEstado(valor) {
   // console.log(valor);
    if(valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}

function Nuevo() {
    $('#IdOpcion').val('0');
    $('#Nombre').val('');
    $('#Formulario').val('');
    $('#Clase').prop('selectedIndex', 0);
    $('#Padre').prop('selectedIndex', 0);
    $('#Url').val('');
    $('#CheckEstadoRegistro').prop('checked', true);   
    $('#LabelEstado').text('Activo');
    $('#divPadre').show();
    $('#divUrl').show();

}

function CargarOpcion(id, nombre, formulario, clase, padre,url, estado) {
    //console.log(id, nombre, formulario, clase, padre, estado);
    $('#IdOpcion').val(id);
    $('#Nombre').val(nombre);
    $('#Formulario').val(formulario);
    
    if (clase == 'P') {
        $('#Clase').prop('selectedIndex', 2);
        $('#Padre').prop('selectedIndex', 0);
        $('#divPadre').hide();
        $('#divUrl').hide();
    }
    else {
        $('#Clase').prop('selectedIndex', 1);
        $('#divPadre').show();
        $('#divUrl').show();
        $('#Url').val(url);
    }

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
