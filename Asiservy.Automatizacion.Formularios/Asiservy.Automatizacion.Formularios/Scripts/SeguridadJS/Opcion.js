$(document).ready(function () {
   
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
function CambioModulo(id) {
    CargarOpciones(id);
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
    $('#Orden').val('');
    $('#Formulario').val('');
    $('#Clase').prop('selectedIndex', 0);
    $('#Padre').prop('selectedIndex', 0);
    $('#Url').val('');
    $('#CheckEstadoRegistro').prop('checked', true);   
    $('#LabelEstado').text('Activo');
    $('#divPadre').show();
    $('#divUrl').show();

}

function CargarOpcion(id, nombre, formulario, clase, padre,url,orden, estado,modulo) {
    //console.log(id, nombre, formulario, clase, padre, estado);
    $('#IdOpcion').val(id);
    $('#txtIdModulo').val(modulo);
    $('#txtNombre').val(nombre);
    $('#txtOrden').val(orden);
    $('#txtFormulario').val(formulario);
    
    if (clase == 'P') {
        $('#selectClase').prop('selectedIndex', 2);
        $('#selectPadre').prop('selectedIndex', 0);
        $('#divPadre').hide();
        $('#divUrl').hide();
    }
    else {
        $('#selectClase').prop('selectedIndex', 1);
        $('#divPadre').show();
        $('#txtUrl').show();
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

function GuargarOpcion() {
    //var Nombre = $("#txtNombre").val();
    //if (Nombre == "") {
    //    $("#ValidaNombre").prop("hidden", false);
    //    return;
    //}
    //else {
    //    $("#ValidaNombre").prop("hidden", true);

    //}
    var Estado = $("#CheckEstadoRegistro").val();
    if (Estado == "true")
        Estado = "A";
    else
        Estado = "I";


    $.ajax({
        url: "../Seguridad/Opcion",
        type: "POST",
        data: {
            IdModulo: $("#txtIdModulo").val(),
            IdOpcion: $("#txtIdOpcion").val(),
            Nombre: $("#txtNombre").val(), 
            Formulario : $("#txtFormulario").val(),
            Clase: $("#selectClase").val(),
            Padre: $("#selectPadre").val(),
            Url: $("#txtUrl").val(),
            Orden: $("#txtOrden").val(),
            EstadoRegistro: Estado
        },
        success: function (resultado) {
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            MensajeCorrecto(resultado);
            CargarOpciones($("#txtIdModulo").val());

        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}
function CargarOpciones(id) {
    if (id > 0) {
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../Seguridad/OpcionPartial",
            type: "GET",
            data: { idModulo: id },
            success: function (resultado) {
                var bitacora = $('#DivTableOpciones');
                bitacora.html(resultado);

            },
            error: function (resultado) {
                MensajeError(resultado, false);
                var bitacora = $('#DivTableOpciones');
                bitacora.html('');
            }
        });
    } else {
        var bitacora = $('#DivTableOpciones');
        bitacora.html('');
    }

}
