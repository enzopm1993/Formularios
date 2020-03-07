function GuardarCabEsterilizacion() {
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    $.ajax({
        url: "../EsterilizacionConserva/GuardarModificarCabeceraEsterilizacion",
        type: "POST",
        data: {
            IdCabControlEsterilizado: $('#CabeceraControl').val(),
            Fecha: $("#Fecha").val(),
            Turno: $("#Turno").val(),
            TipoLinea: $("#Linea").val(),
            Observacion: $("#Observacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false);
            $('#CabeceraControl').val(resultado.IdCabControlEsterilizado);
            ConsultarCoches();
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false)
            //$('#btnConsultar').prop("disabled", false);
            //$("#spinnerCargando").prop("hidden", true);
        }
    });
}
function ConsultarCabControl() {
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    $.ajax({
        url: "../EsterilizacionConserva/ConsultarCabeceraEsterilizacion",
        type: "POST",
        data: {
            Fecha: $("#Fecha").val(),
            Turno: $("#Turno").val(),
            TipoLinea: $("#Linea").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false);
            $('#CabeceraControl').val(resultado.IdCabControlEsterilizado);
            $('#Observacion').val(resultado.Observacion);
            ConsultarCoches();
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false)
            //$('#btnConsultar').prop("disabled", false);
            //$("#spinnerCargando").prop("hidden", true);
        }
    });
}
function ConsultarCoches() {
    $.ajax({
        url: "../EsterilizacionConserva/PartialCocheAutoclave",
        type: "POST",
        data: {
            Fecha: $("#Fecha").val(),
            Turno: $("#Turno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#DivCoches').html(resultado);

            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
    
        }
    });
}