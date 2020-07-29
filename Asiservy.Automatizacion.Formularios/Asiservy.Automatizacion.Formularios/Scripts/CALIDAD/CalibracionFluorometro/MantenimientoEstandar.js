var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
    $('#txtOrden').inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': 99, 'min': 1 });
});

function CargarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../CalibracionFluorometro/MantenimientoEstandarPartial",
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
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function GuardarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../CalibracionFluorometro/GuardarModificarEstandar",
        type: "POST",
        data: {
            IdEstandar: itemEditar.IdEstandar,
            NombEstandar: $('#txtNombre').val(),
            DescEstandar: $("#txtDescripcion").val(),
            Orden: document.getElementById('txtOrden').value,
            DatoNumerico: $("#chkTipoCampo").prop('checked')
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
                MensajeAdvertencia('!El registro no se pudo Actualizar ¡Por favor ACTIVE y vuelva a intentar!');
            } else if (resultado == 3) {
                MensajeAdvertencia('Error al guardar el registro: No se permite espacios en blanco ni vacío');
                $("#txtNombre").css('border', '1px dashed red');
                $('#cargac').hide();
                return;
                
            } else {
                MensajeAdvertencia('!Ya existe un registro con ese NOMBRE!: <span class="badge badge-danger">' + $('#txtNombre').val()+'</span>');
                return;
            }
            $("#txtDescripcion").val('');
            setTimeout(function () {
                LimpiarCabecera();
                $('#ModalIngresoCabecera').modal('hide');
                $('#cargac').hide();
            }, 200);
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
    CambioEstado(true);
    itemEditar = [];
}

function ActualizarCabecera(jdata) {
    LimpiarCabecera();
    if (jdata.EstadoRegistro == 'A') {
        $("#txtNombre").val(jdata.NombEstandar);
        $("#txtDescripcion").val(jdata.DescEstandar);
        document.getElementById('txtOrden').value = jdata.Orden;
        CambioEstado(jdata.DatoNumerico);
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
    } else {
        MensajeAdvertencia('¡Por favor ACTIVE el registro y vuelva a intentar!');
    }
}

function LimpiarCabecera() {
    $("#txtNombre").val('');
    document.getElementById('txtOrden').value = 1;
    $("#txtDescripcion").val('');
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
        url: "../CalibracionFluorometro/EliminarEstandar",
        type: "POST",
        data: {
            IdEstandar: itemEditar.IdEstandar,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdObjeto");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
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

function CambioEstado(valor) {
    if (valor) {
        $('#LabelEstado').text('Numérico');  
        $("#chkTipoCampo").prop('checked',true)
    }
    else {
        $('#LabelEstado').text('AlfaNumérico');       
        $("#chkTipoCampo").prop('checked', false)
    }
}