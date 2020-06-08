var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
});
function CargarCabecera() {
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoClorinacionCisterna/MantenimientoClorinacionCisternaPartial",
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
    $('#cargac').show();
    console.log($('#txtCapacidad').val().length);
    if ($('#txtCapacidad').val().length > 100) {
        $('#cargac').hide();
        MensajeAdvertencia('Paso e límite de caracteres en la Capacidad');
        return;
    }
    $.ajax({
        url: "../MantenimientoClorinacionCisterna/GuardarModificarMantenimientoCisterna",
        type: "POST",
        data: {
            IdCisterna: itemEditar.IdCisterna,
            NDescripcion: $("#txtNDescripcion").val(),
            Ubicacion: $("#txtUbicacion").val(),
            Asignacion: $("#txtAsignacion").val(),
            Tipo: $("#txtTipo").val(),
            Capacidad: $("#txtCapacidad").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera();
            $("#txtDescripcion").val('');
            LimpiarCabecera();
            $('#ModalIngresoCabecera').modal('hide');
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
    $('#cargac').show();
    $.ajax({
        url: "../MantenimientoClorinacionCisterna/EliminarMantenimientoCisterna",
        type: "POST",
        data: {
            IdCisterna: itemEditar.IdCisterna,
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
                $('#cargac').hide();
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

function ActualizarCabecera(jdata) {
    $("#txtNDescripcion").val(jdata.NDescripcion);
    $("#txtUbicacion").val(jdata.Ubicacion);
    $("#txtAsignacion").val(jdata.Asignacion);
    $("#txtTipo").val(jdata.Tipo);
    $("#txtCapacidad").val(jdata.Capacidad);
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = jdata;
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function LimpiarCabecera() {
    $('#txtNDescripcion').val('');
    $('#txtUbicacion').val('');
    $('#txtAsignacion').val('');
    $('#txtTipo').val('');
    $('#txtCapacidad').val('');
    $("#txtNDescripcion").css('border', '');
    $("#txtUbicacion").css('border', '');
    $("#txtAsignacion").css('border', '');
    $("#txtTipo").css('border', '');
    $("#txtCapacidad").css('border', '');
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