var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
    $('#txtValorMax').inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '100', 'min': 0 });
    $('#txtValorMin').inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '100', 'min': 0 });
    $('#selectFormulario').select2({ width: '100%' });
});

function CargarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../MantenimientoTemperatura/MantenimientoTemperaturaPartial",
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
            //itemEditar = 0;
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
        url: "../MantenimientoTemperatura/GuardarModificarRegistro",
        type: "POST",
        data: {
            IdTemperaturaMant: itemEditar.IdTemperaturaMant,
            CodFormulario: document.getElementById('selectFormulario').value,
            ValorMax: document.getElementById('txtValorMax').value,
            ValorMin: document.getElementById('txtValorMin').value,
            Descripcion: document.getElementById('txtDescripcion').value
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }            
            if (resultado == 0) {
                MensajeCorrecto('Registro guardado correctamente');
            } else if (resultado == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (resultado == 2) {
                MensajeAdvertencia('El registro no se pudo Actualizar ¡Por favor ACTIVE y vuelva a intentar!');
            } else if (resultado == 3) {


                var t = document.getElementById("selectFormulario");
                var selectedText = t.options[t.selectedIndex].text;
                MensajeAdvertencia('!El formulario ya existe:! <span class="badge badge-danger">' + selectedText + '</span>');
                $('#cargac').hide();
                return;
            } else {
                MensajeAdvertencia('Error al guardar el registro: No se permite espacios en blanco ni vacío');
                $("#txtNombre").css('border', '1px dashed red');
                $('#cargac').hide();
                return;
            }
            CargarCabecera();
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
        $('#selectFormulario').val(jdata.CodFormulario).trigger('change');    
        document.getElementById("txtValorMax").value = jdata.ValorMax;
        document.getElementById("txtValorMin").value = jdata.ValorMin;
        document.getElementById("txtDescripcion").value = jdata.Descripcion;
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
    } else {
        MensajeAdvertencia('¡Por favor ACTIVE el registro y vuelva a intentar!');
    }
}

function LimpiarCabecera() {
    document.getElementById("txtValorMax").value = '';
    document.getElementById("txtValorMin").value = '';
    document.getElementById("txtDescripcion").value = '';
}

function InactivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}

function ActivarConfirmar(jdata, NombreFormulario) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.NombreFormulario = NombreFormulario;
    itemEditar.EstadoRegistro = 'A';
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    $.ajax({
        url: "../MantenimientoTemperatura/EliminarRegistro",
        type: "POST",
        data: {
            IdTemperaturaMant: itemEditar.IdTemperaturaMant,
            CodFormulario: itemEditar.CodFormulario,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdTemperaturaMant");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                $('#cargac').hide();
            } else if (resultado == "2") {
                MensajeAdvertencia('Ya existe una registro activo con el FORMULARIO: <span class="badge badge-danger">' + itemEditar.NombreFormulario + '</span>');
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
    //if (document.getElementById('selectFormulario').value == '') {
    //    $("#txtNumero").css('border', '1px dashed red');
    //    con = 1;
    //} else $("#txtNumero").css('border', '');
    return con;
}