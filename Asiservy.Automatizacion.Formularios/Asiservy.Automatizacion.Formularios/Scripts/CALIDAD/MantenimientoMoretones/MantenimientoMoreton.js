var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
});

function CargarCabecera() {
    $.ajax({
        url: "../MantenimientoMoretones/MantenimientoMoretonPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMantenimientoMoretones").html("No existen registros");
            } else {
                $("#divMantenimientoMoretones").html(resultado);
            }
            itemEditar = 0;
            CerrarModalCargando();
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {
    if ($("#txtDescripcion").val() == '' || $('#txtAbreviatura').val() == '') {
        MensajeAdvertencia("<span class='badge badge-danger'>!Ingrese una Descripción/Abreviatura al Color que desea ingresar¡</span>");
        return;
    }
    if ($('#txtAbreviatura').val().length > 5) {
        MensajeAdvertencia("<span class='badge badge-danger'>!Exidio el maximo de caracteres: 5¡</span>");
        return;
    }
    $('#cargac').show();
    $.ajax({
        url: "../MantenimientoMoretones/GuardarModificarMantenimientoMoreton",
        type: "POST",
        data: {
            IdMoreton: itemEditar.IdMoreton,
            Abreviatura: $('#txtAbreviatura').val().toUpperCase(),
            Descripcion: $("#txtDescripcion").val().toUpperCase()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera();
            $("#txtDescripcion").val('');
            $('#txtAbreviatura').val('');
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function InactivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}

function ActivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.EstadoRegistro = 'A';
}

function EliminarCabeceraSi() {
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoMoretones/EliminarMantenimientoMoreton",
        type: "POST",
        data: {
            IdMoreton: itemEditar.IdMoreton,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdDesinfeccionManos");
                $("#modalEliminarControl").modal("hide");
                CerrarModalCargando();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                setTimeout(function () {
                    CerrarModalCargando();
                }, 500);
            }
            itemEditar = 0;
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera(jdata) {
    $("#txtDescripcion").prop("disabled", false);
    $("#txtDescripcion").val(jdata.Descripcion);
    $('#txtAbreviatura').val(jdata.Abreviatura)
    itemEditar = jdata;
}

function NuevoControl() {
    $("#txtDescripcion").val('');
    $("#txtAbreviatura").val('');
    itemEditar = [];
}