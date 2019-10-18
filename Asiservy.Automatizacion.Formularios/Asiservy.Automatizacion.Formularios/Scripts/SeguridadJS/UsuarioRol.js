$(document).ready(function () {
    CargarUsuarioRol();
    NuevoUsuarioRol();
});


function NuevoUsuarioRol() {
    $('#IdUsuarioRol').val('0');   
    $('#IdUsuario2').prop('selectedIndex', 0);
    $('#IdRol2').prop('selectedIndex', 0);   
    $('#IdUsuario').val('');
    $('#IdRol').val('');
    $('#IdUsuario2').prop('disabled',false);
    $('#IdRol2').prop('disabled', false);
    $('#CheckEstadoRegistro').prop('checked', true);
    $('#LabelEstado').text('Activo');

}

function SelectUsuario() {
    if ($('#IdUsuario2').val() == "") {
        MensajeAdvertencia("Usuario no tiene registrado una cedula", false);
        $('#IdUsuario2').val('');
        $('#IdUsuario2').prop('selectedIndex', 0);

    } else {
        $('#IdUsuario').val($('#IdUsuario2').val());

    }
}

function SelectRol() {
    $('#IdRol').val($('#IdRol2').val());
}



function SeleccionUsuarioRol(id,usuario,rol,estado) {

    $('#IdUsuarioRol').val(id);
    $('#IdUsuario').val(usuario);
    $('#IdRol').val(rol);
    $('#IdUsuario2').val(usuario);
    $('#IdRol2').val(rol);
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