


$(document).ready(function () {
    CargarTablaPeriodo();
    $('#IdParametro').val(0);
});


function Valida() {
    var bol = true;
    if ($('#txtCodigo').val() == '') {
        $("#pValidaCodigo").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaCodigo").prop("hidden", true);
    }
    if ($('#txtDescripcion').val() == '') {
        $("#pValidaDescripcion").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaDescripcion").prop("hidden", true);
    }
    if ($('#txtValor').val() < 0) {
        $("#pValidaValor").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaValor").prop("hidden", true);
    }

    return bol;

}

function GuardarModificarParametro() {

    if (!Valida())
        return;
    var Estado = $("#CheckEstadoRegistro").prop('checked');

    if (Estado == true)
        Estado = "A";
    else
        Estado = "I";
    $.ajax({
        url: "../Seguridad/Parametro",
        type: "POST",
        data: {
            IdParametro: $('#IdParametro').val(),
            Codigo: $('#txtCodigo').val(),
            Descripcion: $('#txtDescripcion').val(),
            Valor: $('#txtValor').val(),
            Observacion: $('#txtObservacion').val(),
            EstadoRegistro: Estado

        },
        success: function (resultado) {
            if (resultado.Codigo == 0)
                MensajeAdvertencia(resultado.Mensaje);
            else {
                MensajeCorrecto(resultado.Mensaje);
                CargarTablaClasificadores();
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#SpinnerCargando").prop("hidden", true);

        }
    });
}


function CargarTablaPeriodo() {
    $("#SpinnerCargando").prop("hidden", false);
    $('#divTableParametros').html('');
    $.ajax({
        url: "../Periodo/Periodo",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#divTablePeriodos');
            bitacora.html(resultado);
            $("#SpinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#SpinnerCargando").prop("hidden", true);

        }
    });
}

function Nuevo() {
    $('#IdParametro').val('0');
    $('#txtCodigo').val('');
    $('#txtDescripcion').val('');
    $('#txtObservacion').val('');
    $('#txtValor').val(0);
    $('#CheckEstadoRegistro').prop('checked', true);
    $('#LabelEstado').text('Activo');
}

function SeleccionParametro(id, codigo, descripcion, valor, observacion, estado) {

    $('#IdParametro').val(id);
    $('#txtCodigo').val(codigo);
    $('#txtDescripcion').val(descripcion);
    $('#txtObservacion').val(observacion);
    $('#txtValor').val(valor);
    if (estado == "A") {
        $('#CheckEstadoRegistro').prop('checked', true);
        $('#LabelEstado').text('Activo');
    }
    else {
        $('#CheckEstadoRegistro').prop('checked', false);
        $('#LabelEstado').text('Inactivo');

    }
}