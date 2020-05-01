var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
});
function CargarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../HigieneComedorCocina/MantHigieneComedorCocinaPartial",
        type: "GET",       
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            itemEditar = 0;
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../HigieneComedorCocina/GuardarModificarAreaAuditoria",
        type: "POST",
        data: {
            IdAuditoria: itemEditar.IdAuditoria,
            NombreAuditoria: $('#txtNombre').val(),
            DescripcionAuditoria: $("#txtDescripcion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera();
            $("#txtDescripcion").val('');
            setTimeout(function () {
                LimpiarCabecera();
                $('#ModalIngresoCabecera').modal('hide');
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function ActualizarCabecera(jdata) {
    $("#txtNombre").val(jdata.NombreAuditoria);
    $("#txtDescripcion").val(jdata.DescripcionAuditoria);   
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = jdata;
}

function LimpiarCabecera() {
    $("#txtNombre").val('');
    $("#txtDescripcion").val('');
}

function InactivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}
//SIN USO








function ActivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.EstadoRegistro = 'A';
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    $.ajax({
        url: "../HigieneComedorCocina/EliminarAreaAuditoria",
        type: "POST",
        data: {
            IdAuditoria: itemEditar.IdAuditoria,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdDesinfeccionManos");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                setTimeout(function () {
                    $('#cargac').hide();
                }, 200);
            }
            itemEditar = 0;
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}






function ValidarDatosVacios() {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera();
}

function OnChangeTextBox() {
    var con = 0;
    if ($('#txtNDescripcion').val() == '') {
        $("#txtNDescripcion").css('border', '1px dashed red');
        con = 1;
    } else $("#txtNDescripcion").css('border', '');
    if ($('#txtUbicacion').val() == '') {
        $("#txtUbicacion").css('border', '1px dashed red');
        con = 1;
    } else $("#txtUbicacion").css('border', '');
    if ($('#txtAsignacion').val() == '') {
        $("#txtAsignacion").css('border', '1px dashed red');
        con = 1;
    } else $("#txtAsignacion").css('border', '');
    if ($('#txtTipo').val() == '') {
        $("#txtTipo").css('border', '1px dashed red');
        con = 1;
    } else $("#txtTipo").css('border', '');
    if ($('#txtCapacidad').val() == '') {
        $("#txtCapacidad").css('border', '1px dashed red');
        con = 1;
    } else $("#txtCapacidad").css('border', '');
    return con;
}