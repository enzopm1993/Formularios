var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();    
});

function CargarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../LaboratorioAnalisisQuimico/MantenimientoTurnoPartial",
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
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function GuardarCabecera() {
    $('#cargac').show();
    if ($('#txtNombre').val().length > 50 || $('#txtDescripcion').val().length > 200) {
        $('#cargac').hide();
        MensajeAdvertencia('Paso e límite de caracteres en la Capacidad');
        return;
    }
    $.ajax({
        url: "../LaboratorioAnalisisQuimico/GuardarModificarMantenimiento",
        type: "POST",
        data: {
            IdTurno: itemEditar.IdTurno,
            Nombre: $("#txtNombre").val(),
            DescripcionMant: $("#txtDescripcion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == 0) {
                MensajeCorrecto('Datos guardados correctamente');
            } else if (resultado == 1) {
                MensajeCorrecto('Datos actualizados correctamente');
            } else if (resultado == 2) {
                MensajeAdvertencia('El registro no se pudo Actualizar ¡Por favor ACTIVE y vuelva a intentar!');
            } else if (resultado == 3) {
                MensajeAdvertencia('!El nombre ya existe:! <span class="badge badge-danger">' + $('#txtNombre').val().toUpperCase() + '</span>');
                $('#cargac').hide();
                return;
            } else if (resultado == 4) {
                MensajeAdvertencia('Error al guardar el registro: No se permite espacios en blanco ni vacío');
                $('#ModalIngresoCabecera').modal('show');
                $("#txtNombre").css('border', '1px dashed red');
                $('#cargac').hide();
                return;
            }
            CargarCabecera();
            LimpiarCabecera();
            $('#ModalIngresoCabecera').modal('hide');
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
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
        url: "../LaboratorioAnalisisQuimico/EliminarMantenimiento",
        type: "POST",
        data: {
            IdTurno: itemEditar.IdTurno,
            Nombre: itemEditar.Nombre,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdTurno");
                $("#modalEliminarControl").modal("hide");
                CerrarModalCargando();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                $('#cargac').hide();
            } else if (resultado == "2") {
                MensajeAdvertencia('Ya existe un TURNO activo con el Nombre: <span class="badge badge-danger">' + itemEditar.Nombre.toUpperCase() + '</span>');
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
            }
            itemEditar = 0;
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera(jdata) {
    if (jdata.EstadoRegistro != 'I') {
        $("#txtNombre").val(jdata.Nombre);
        $("#txtDescripcion").val(jdata.DescripcionMant);
        $("#txtOrden").val(jdata.Orden);
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
    } else {
        MensajeAdvertencia('¡Por favor <span class="badge badge-danger">ACTIVE</span> el AREA y vuelva a intentar!');
    }
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function LimpiarCabecera() {
    $('#txtNombre').val('');
    $('#txtDescripcion').val('');
    $("#txtNombre").css('border', '');
    $("#txtDescripcion").css('border', '');   
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
    if ($('#txtNombre').val() == '') {
        $("#txtNombre").css('border', '1px dashed red');
        con = 1;
    } else $("#txtNombre").css('border', '');
    return con;
}