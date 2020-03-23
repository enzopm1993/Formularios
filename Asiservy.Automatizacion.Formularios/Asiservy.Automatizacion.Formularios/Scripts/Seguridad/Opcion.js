$(document).ready(function () {
    $('#selectPadre').select2();
    Nuevo();
});

function CambioClase(valor) {
    CargarOpciones();
    if (valor == "1") {
        $('#Padre').prop('selectedIndex', 0);
        $('#Url').val('');
        $('#divPadre1').hide();
        $('#divPadre2').hide();
        $('#divUrl').hide();

    } else {
        $('#Url').val('');
        $('#Padre').prop('selectedIndex', 0);
        $('#divUrl').show();
        $('#divPadre1').show();
        $('#divPadre2').show();

    }

}
function CambioModulo(id) {
    if (id > 0) {        
        CargarPadres(id);
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

    if ($('#selectClase').val() == "1") {
        $('#txtIdOpcion').val('0');
        $('#txtNombre').val('');
        $('#txtOrden').val('');
        $('#txtFormulario').val('');
        $('#selectPadre').prop('selectedIndex', 0).change();
        // $('#selectClase').prop('selectedIndex', 0);
        $('#txtUrl').val('');
        $('#CheckEstadoRegistroOp').prop('checked', true);
        $('#LabelEstado').text('Activo');
    } else {
        $('#txtIdOpcion').val('0');
        $('#txtNombre').val('');
        $('#txtOrden').val('');
        $('#txtFormulario').val('');
        $('#selectPadre').prop('selectedIndex', 0).change();
        // $('#selectClase').prop('selectedIndex', 0);
        $('#txtUrl').val('');
        $('#CheckEstadoRegistroOp').prop('checked', true);
        $('#LabelEstado').text('Activo');
        $('#divPadre1').show();
        $('#divPadre2').show();
        $('#divUrl').show();
    }
   

}

function CargarOpcion(id, nombre, formulario, clase, padre,url,orden, estado,modulo) {
    //console.log(id, nombre, formulario, clase, padre, estado);
    $('#txtIdOpcion').val(id);
   // $('#txtIdModulo').val(modulo);
    $('#txtNombre').val(nombre);
    $('#txtOrden').val(orden);
    $('#txtFormulario').val(formulario);
    
    if (clase == 'P') {
        $('#selectClase').prop('selectedIndex', 2);
        $('#selectPadre').prop('selectedIndex', 0).change();
        $('#divPadre1').hide();
        $('#divPadre2').hide();
        $('#divUrl').hide();
    }
    else {
        $('#selectClase').prop('selectedIndex', 1);
        $('#divPadre1').show();
        $('#divPadre2').show();
        $('#divUrl').show();
        $('#txtUrl').val(url);
    }

    if(padre!='')
        $('#selectPadre').val(padre).change();

    if (estado == 'A') {
        $('#CheckEstadoRegistroOp').prop('checked', true);
       // console.log($('#LabelEstado').val());
        $('#LabelEstado').text('Activo');
        
    }
    else {
        $('#CheckEstadoRegistroOp').prop('checked', false);
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
    var Estado = "I";
    if ($("#CheckEstadoRegistroOp").prop("checked"))
        Estado = "A";   
    
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
            Nuevo();
            CargarOpciones($("#txtIdModulo").val());
            CargarPadres($("#txtIdModulo").val());

        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}
function CargarOpciones() {
    var id = $("#txtIdModulo").val();
    if ($("#selectClase").val() != '') {
        if ($("#selectClase").val() == '0') {
            var padre = 'H';
        } else {
            var padre = 'P';
        }
    }
    if (id > 0 && $("#selectPadre").val()!= '') {
        $('#DivTableOpciones').html('')
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../Seguridad/OpcionPartial",
            type: "GET",
            data: { idModulo: id, Clase: padre },
            success: function (resultado) {
                var bitacora = $('#DivTableOpciones');
                bitacora.html(resultado);
                $("#spinnerCargando").prop("hidden", true);
            },
            error: function (resultado) {
                MensajeError(resultado, false);
                $("#spinnerCargando").prop("hidden", true);

              //  var bitacora = $('#DivTableOpciones');
               /// bitacora.html('');
            }
        });
    } else {
        $("#spinnerCargando").prop("hidden", true);

      //  var bitacora = $('#DivTableOpciones');
       // bitacora.html('');
    }

}


function CargarPadres(id) {
    $("#selectPadre").empty().change();
    $("#selectPadre").append("<option value='0' >Seleccione</option>").change();
    $.ajax({
        url: "../Seguridad/ConsultarPadres",
        type: "Get",       
        data:
        {
            idModulo: 0
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectPadre").append("<option value='" + row.codigo + "'>" + row.descripcion + "</option>").change();
                });
            } else {
                MensajeAdvertencia("El modulo no tiene padres asignados", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado.responseText), false);
        }
    });
}