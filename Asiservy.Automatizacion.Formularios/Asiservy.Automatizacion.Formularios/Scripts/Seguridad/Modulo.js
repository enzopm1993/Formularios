
$(document).ready(function () {
    CargarOpciones();
    Limpiar();
});


function CargarModulo(id, nombre, estado,orden,icono) {
    $("#txtId").val(id);
    $("#txtNombre").val(nombre);
    $("#txtOrden").val(orden);
    $('#output').prop('hidden', false);
    $('#output').attr("src", icono);
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
    $('#lblfoto').text('Seleccione archivo');
    $('#LabelEstado').text('Activo');
    $("#CheckEstadoRegistro").prop('checked', true);
    $('#output').prop('hidden', true);
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

    var base64 = getBase64Image(document.getElementById("output"));
    $.ajax({
        url: "../Seguridad/Modulo",
        type: "POST",
        data: {
            IdModulo: $("#txtId").val(),
            Nombre: Nombre,
            EstadoRegistro: Estado,
            Orden: Orden,
            icono: base64
        },
        success: function (resultado) {
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parámetros");
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
var loadFile = function (event) {
    $('#output').prop('hidden', false);
    var image = document.getElementById('output');
    image.src = URL.createObjectURL(event.target.files[0]);
 
    $('#lblfoto').text(event.target.files[0].name);
    
    //console.log(base64);
};
function getBase64Image(img) {
    var canvas = document.createElement("canvas");
    canvas.width = img.width;
    canvas.height = img.height;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0);
    var dataURL = canvas.toDataURL();
    return dataURL;
}