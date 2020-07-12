var itemEditar = [];
$(document).ready(function () {
    
});
function CargarCabecera() {
    $("#divMantenimientoPediluvio").html('');
    $("#hMensaje").html('');
    if ($("#selectArea").val() == "") {
        return;
    }
    MostrarModalCargando();
   
    $.ajax({
        url: "../MantenimientoPediluvio/MantenimientoPediluvioPartial",
        type: "GET",
        data: { Area: $("#selectArea").val() }
        ,
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
            Nuevo();
            CerrarModalCargando();
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(Mensajes.Error+resultado.responseText, false);
        }
    });
}

function Limpiar() {
    $("#txtDescripcion").val('');
    itemEditar = 0;
    $("#txtIdMantenimientoPediluvio").val('0');
}
function GuardarCabecera() {
    if ($("#txtDescripcion").val() == '') {
        MensajeAdvertencia("Ingrese una descripción al Pediluvio que desea ingresar");
        return;
    }
    if ($("#selectArea").val() == '') {
        MensajeAdvertencia("Debe seleccionar una area.");
        return;
    }
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoPediluvio/GuardarModificarMantenimientoPediluvio",
        type: "POST",
        data: {
            IdMantenimientoPediluvio: itemEditar.IdMantenimientoPediluvio,
            Descripcion: $("#txtDescripcion").val(),
            Area: $("#selectArea").val()
           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera();
            $("#txtDescripcion").val('');
            Nuevo();
            CerrarModalCargando();
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(Mensajes.Error +resultado.responseText, false);
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
            IdMantenimientoPediluvio: itemEditar.IdMantenimientoPediluvio,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdMantenimientoPediluvio");
                $("#modalEliminarControl").modal("hide");
                CerrarModalCargando();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                Nuevo();
                CerrarModalCargando();

            }
            itemEditar = 0;
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(Mensajes.Error +resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera(jdata) {
    $("#txtDescripcion").prop("disabled", false);
    $("#txtDescripcion").val(jdata.Descripcion);
    $("#selectArea").val(jdata.Area);
    $("#txtIdMantenimientoPediluvio").val(jdata.IdMantenimientoPediluvio);    
    itemEditar = jdata;
}

function Nuevo() {
    $("#txtDescripcion").val('');
    $("#txtIdMantenimientoPediluvio").val('0'); 
    $("#selectArea").prop("selecteIndex", 0);
}