
$(document).ready(function () {
    CargarOpciones();
    Limpiar();
});


function CargarModulo(id, nombre, estado,orden) {
    $("#txtId").val(id);
    $("#txtNombre").val(nombre);
    $("#txtOrden").val(orden);

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
    $("#txtOrden").val("");

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
    var Orden = $("#txtOrden").val();
    if (Nombre == "") {
        $("#ValidaNombre").prop("hidden", false);
        return;
    }
    else {
        $("#ValidaNombre").prop("hidden", true);

    }

    if (Orden == "") {
        $("#ValidaOrden").prop("hidden", false);
        return;
    }
    else {
        $("#ValidaOrden").prop("hidden", true);

    }
    var Estado = $("#CheckEstadoRegistro").prop('checked'); 
    if (Estado == true)
        Estado = "A";
    else
        Estado = "I";


    $.ajax({
        url: "../Seguridad/Modulo",
        type: "POST",
        data: {
            IdModulo: $("#txtId").val(),
            Nombre: Nombre,
            EstadoRegistro: Estado,
            Orden: Orden
        },
        success: function (resultado) {
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            CargarOpciones();
            MensajeCorrecto(resultado);      
           

        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}


function CargarOpciones() {
    var bitacora = $('#DivTableModulos');
    bitacora.html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Seguridad/ModuloPartial",
        type: "GET",
        success: function (resultado) {
            
            var bitacora = $('#DivTableModulos');
            bitacora.html('');
            $("#spinnerCargando").prop("hidden", true);
            bitacora.html(resultado);


        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}
