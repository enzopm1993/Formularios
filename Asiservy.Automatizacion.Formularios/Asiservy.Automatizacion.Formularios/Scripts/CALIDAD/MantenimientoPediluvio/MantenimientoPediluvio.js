var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
});
function CargarCabecera() {
    MostrarModalCargando();
    $("#divMantenimientoPediluvio").html('');
    $("#hMensaje").html('');
    $.ajax({
        url: "../MantenimientoPediluvio/MantenimientoPediluvioPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            console.log(resultado);
            if (resultado == "0") {
                $("#hMensaje").html(Mensajes.SinRegistros);
            } else {
                $("#divMantenimientoPediluvio").html(resultado);
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
    if ($("#txtDescripcion").val() == '') {
        MensajeAdvertencia("Ingrese una descripción al Pediluvio que desea ingresar");
        return;
    }
    if ($("#checkPreparacion").prop("checked") == false && $("#checkProceso").prop("checked") == false) {
        MensajeAdvertencia("Debe seleccionar una area al menos");
        return;
    }
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoPediluvio/GuardarModificarMantenimientoPediluvio",
        type: "POST",
        data: {
            IdMantenimientoPediluvio: itemEditar.IdPediluvio,
            Descripcion: $("#txtDescripcion").val(),
            Proceso: $("#checkProceso").prop("checked"),
            Preparacion: $("#checkPreparacion").prop("checked")
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera();
            $("#txtDescripcion").val('');
            setTimeout(function () {
                CerrarModalCargando();
            }, 500);
        },
        error: function (resultado) {
            CerrarModalCargando();
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
        url: "../MantenimientoPediluvio/EliminarMantenimientoPediluvio",
        type: "POST",
        data: {
            IdPediluvio: itemEditar.IdPediluvio,
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
    itemEditar = jdata;
}