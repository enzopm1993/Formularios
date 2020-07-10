var itemEditar = [];
$(document).ready(function () {
    CargarCabecera();
    mask();
    $('#txtCodFormClasif').select2({
        width: '100%'
    });
});

function mask() {   
    $('#txtValorMin').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '999', 'min': '0' });
    $('#txtValorMax').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '999', 'min': '0' });
}

function CargarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../MantParametrosLaboratorio/MantParametrosLaboratorioPartial",
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
        error: function (result) {
            //console.log(result.responseText);
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function GuardarCabecera() {
    $('#cargac').show();
    if (document.getElementById('txtValorMin').value > document.getElementById('txtValorMax').value) {
        MensajeAdvertencia('El valor mínimo no puede ser mayor al valor máximo');
        $('#cargac').hide();
        return;
    }
    if ($('#txtNombre').val().length > 50 || $('#txtDescripcion').val().length > 100 || document.getElementById('txtCodFormClasif').value.length==0) {
        $('#cargac').hide();
        MensajeAdvertencia('Paso e límite de caracteres en la Capacidad');
        return;
    }
    $.ajax({
        url: "../MantParametrosLaboratorio/GuardarModificarMantenimiento",
        type: "POST",
        data: {
            IdParametro: itemEditar.IdParametro,
            NombreParametro: $("#txtNombre").val(),
            CodFormClasif: document.getElementById('txtCodFormClasif').value,
            ValorMin: document.getElementById('txtValorMin').value,
            ValorMax: document.getElementById('txtValorMax').value,
            DescripcionParametro: document.getElementById('txtDescripcion').value
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
                var sel = document.getElementById("txtCodFormClasif");
                var text = sel.options[sel.selectedIndex].text;
                MensajeAdvertencia('!El nombre: <span class="badge badge-danger">' + $('#txtNombre').val().toUpperCase() + '</span> ya existe en:  <span class="badge badge-danger">' + text+'</span>!');
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
        url: "../MantParametrosLaboratorio/EliminarMantenimiento",
        type: "POST",
        data: {
            IdParametro: itemEditar.IdParametro,
            NombreParametro: itemEditar.NombreParametro,
            CodFormClasif: document.getElementById('txtCodFormClasif').value,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdCisterna");
                $("#modalEliminarControl").modal("hide");
                CerrarModalCargando();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                $('#cargac').hide();
            } else if (resultado == "2") {
                MensajeAdvertencia('!El nombre: <span class="badge badge-danger">' + itemEditar.NombreParametro.toUpperCase() + '</span> ya existe en:  <span class="badge badge-danger">' + itemEditar.Descripcion + '</span>!');
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
        $("#txtNombre").val(jdata.NombreParametro);
        $("#txtDescripcion").val(jdata.DescripcionParametro);
        $("#txtValorMax").val(jdata.ValorMax);
        document.getElementById('txtValorMin').value = jdata.ValorMin;
        document.getElementById('txtCodFormClasif').value = jdata.CodFormClasif;
        $('#txtCodFormClasif').val(jdata.CodFormClasif).trigger('change');
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
    } else {
        MensajeAdvertencia('¡Por favor <span class="badge badge-danger">ACTIVE</span> el PARÁMETRO y vuelva a intentar!');
    }

}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function LimpiarCabecera() {
    mask();
    $('#txtNombre').val('');
    $('#txtDescripcion').val('');
    $("#txtNombre").css('border', '');
    $("#txtDescripcion").css('border', '');
    $('#txtValorMin').css('border','');
    $("#txtValorMax").css('border', '');
    document.getElementById('txtValorMax').value = 0;
    document.getElementById('txtValorMin').value = 0;
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
    if ($('#txtValorMin').val() == '') {
        $("#txtValorMin").css('border', '1px dashed red');
        con = 1;
    } else $("#txtValorMin").css('border', '');
    if ($('#txtValorMax').val() == '') {
        $("#txtValorMax").css('border', '1px dashed red');
        con = 1;
    } else $("#txtValorMax").css('border', '');
    if ($('#txtCodFormClasif').val() == '') {
        $("#txtCodFormClasif").css('border', '1px dashed red');
        con = 1;
    } else $("#txtCodFormClasif").css('border', '');

    return con;
}