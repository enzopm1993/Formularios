﻿$(document).ready(function () {
    CargarUsuarioRol();
    //NuevoUsuarioRol();
    $('#IdUsuario2').select2();    
    $('#IdRol2').select2();   
});


$('#NuevoUsuarioRol').click(function () {  
  
    $('#IdUsuarioRol').val(0); 
    $('#IdUsuario').val('');
    $('#IdRol').val('');
    
    $('#CheckEstadoRegistro').prop('checked', true);
    $('#LabelEstado').text('Activo');

    $('#IdUsuario2').attr('disabled', false);
    $('#IdRol2').attr('disabled', false);

   
    $("#IdRol2").prop('selectedIndex', 0).change();
    $("#IdUsuario2").prop('selectedIndex', 0).change();

});

function SelectUsuario() {   
        $('#IdUsuario').val($('#IdUsuario2').val()).trigger('change');    
}

function SelectRol() {
    $('#IdRol').val($('#IdRol2').val());
}



function SeleccionUsuarioRol(id,usuario,rol,estado) {

    $('#IdUsuarioRol').val(id);
    $('#IdUsuario').val(usuario);
    $('#IdRol').val(rol);
    $('#IdUsuario2').val(usuario).trigger('change');
    $('#IdRol2').val(rol).trigger('change');
    $('#IdUsuario2').prop('disabled', true);
    $('#IdRol2').prop('disabled', true);
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


function CambioEstado(valor) {
  //  console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}

