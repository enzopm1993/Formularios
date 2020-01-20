$(document).ready(function () {
    CargarNivelUsuario();
    Nuevo();
    
    //$('#IdUsuario').select2({
    //    width: 'resolve'
    //});
});


function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}

function Nuevo() {
    $('#IdNivelUsuario').val('');
    $('#IdUsuario').val('');
    $('#Nivel').val('');

}

function SeleccionNivelUsuario(id, usuario, nivel, estado, UsuarioAprueba) {
    console.log(id, usuario, nivel, estado, UsuarioAprueba);
    $('#IdNivelUsuario').val(id);
    $('#IdUsuario').val(usuario);
    $('#Nivel').val(nivel);
    if (nivel == '4') {
        $("#divUsuarioAprueba").prop("hidden", false);
        $("#UsuarioAprueba").val(UsuarioAprueba);
    } else {
        $("#divUsuarioAprueba").prop("hidden", true);
    }

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

function CargarNivelUsuario() {
    $.ajax({
        url: "../Seguridad/NivelUsuarioPartial",
        type: "GET",
        success: function (resultado) {
           
            var bitacora = $('#DivTableNivelUsuario');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}

function CambioNivel() {
    if ($("#Nivel").val() == '4') {
        $("#divUsuarioAprueba").prop("hidden", false);
    } else {
        $("#divUsuarioAprueba").prop("hidden", true);
    }

}