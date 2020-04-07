var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();    
});
function CargarCabecera() {
    MostrarModalCargando();
       
        $.ajax({
            url: "../MantenimientoColor/MantenimientoColorPartial",
            type: "GET",
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }                
                if (resultado == "0") {
                    $("#divMantenimientoColor").html("No existen registros");
                } else {
                    $("#divMantenimientoColor").html(resultado);
                }                
                itemEditar = 0;
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

function GuardarCabecera() {
    if ($("#txtDescripcion").val()=='') {
        MensajeAdvertencia("<span class='badge badge-danger'>!Ingrese una descripción al Color que desea ingresar¡</span>");
        return;
    }
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoColor/GuardarModificarMantenimientoColor",
        type: "POST",
        data: {
            IdColor:itemEditar.IdColor,
            Descripcion: $("#txtDescripcion").val()
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
        url: "../MantenimientoColor/EliminarMantenimientoColor",
        type: "POST",
        data: {
            IdColor: itemEditar.IdColor,
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