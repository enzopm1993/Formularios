var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
    $('#txtNumero').inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '100', 'min':0});
});

function CargarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../MantenimientoPCC/MantenimientoPCCPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();h
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            itemEditar = 0;
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function GuardarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../MantenimientoPCC/GuardarModificarRegistro",
        type: "POST",
        data: {
            IdPcc: itemEditar.IdPcc,
            Numero: document.getElementById('txtNumero').value,         
            DescripcionMant: document.getElementById('txtDescripcion').value
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera();
            if (resultado == 0) {
                MensajeCorrecto('Registro guardado correctamente');
            } else if (resultado == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (resultado == 2) {
                MensajeAdvertencia('El registro no se pudo Actualizar ¡Por favor ACTIVE y vuelva a intentar!');
            } else if (resultado == 3) {
                MensajeAdvertencia('!El nombre ya existe:! <span class="badge badge-danger">' + document.getElementById('txtNumero').value + '</span>');
                return;
            }else {
                MensajeAdvertencia('Error al guardar el registro: No se permite espacios en blanco ni vacío');
                $("#txtNombre").css('border', '1px dashed red');
                $('#cargac').hide();
                return;
            }
            document.getElementById('txtDescripcion').value = '';
            LimpiarCabecera();
            $('#ModalIngresoCabecera').modal('hide');
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function ActualizarCabecera(jdata) {
    if (jdata.EstadoRegistro == 'A') {
        document.getElementById("txtNumero").value=jdata.Numero;
        document.getElementById("txtDescripcion").value=jdata.DescripcionMant;
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
    } else {
        MensajeAdvertencia('¡Por favor ACTIVE el registro y vuelva a intentar!');
    }
}

function LimpiarCabecera() {
    document.getElementById("txtNumero").value='';
    document.getElementById("txtDescripcion").value='';
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
        url: "../MantenimientoPCC/EliminarRegistro",
        type: "POST",
        data: {
            IdPcc: itemEditar.IdPcc,
            Numero: itemEditar.Numero,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdPcc");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                $('#cargac').hide();
            } else if (resultado == "2") {
                MensajeAdvertencia('Ya existe una registro activo con el NÚMERO: <span class="badge badge-danger">' + itemEditar.Numero + '</span>');
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
            }
            itemEditar = 0;
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
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
    if (document.getElementById('txtNumero').value == '') {
        $("#txtNumero").css('border', '1px dashed red');
        con = 1;
    } else $("#txtNumero").css('border', '');   
    return con;
}