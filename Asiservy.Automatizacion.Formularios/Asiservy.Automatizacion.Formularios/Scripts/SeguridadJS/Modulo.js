
$(document).ready(function () {
    CargarOpciones();
    Limpiar();
});


function CargarModulo(id, nombre, estado) {
    $("#txtId").val(id);
    $("#txtNombre").val(nombre);

    if (estado == "A") {
        CambioEstado(true);
        $("#CheckEstadoRegistro").prop('checked', true);
    } else {
        $("#CheckEstadoRegistro").prop('checked', false);
        CambioEstado(false);

    }

}
function Limpiar() {
    $("#txtId").val("0");
    $("#txtNombre").val("");
    $('#LabelEstado').text('Activo');
    $("#CheckEstadoRegistro").prop('checked', true);

}

function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}
function GuargarModulo() {
    var Nombre = $("#txtNombre").val();
    if (Nombre == "") {
        $("#ValidaNombre").prop("hidden", false);
        return;
    }
    else {
        $("#ValidaNombre").prop("hidden", true);

    }
    var Estado = $("#CheckEstadoRegistro").val();
    if (Estado == "true")
        Estado = "A";
    else
        Estado = "I";


    $.ajax({
        url: "../Seguridad/Modulo",
        type: "POST",
        data: {
            IdModulo: $("#txtId").val(),
            Nombre: Nombre,
            EstadoRegistro: Estado
        },
        success: function (resultado) {
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            MensajeCorrecto(resultado);      
            CargarOpciones();

        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}


function CargarOpciones() {
    $.ajax({
        url: "../Seguridad/ModuloPartial",
        type: "GET",
        success: function (resultado) {
            
            var bitacora = $('#DivTableModulos');
            bitacora.html('');
            bitacora.html(resultado);

        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}
