function ConsultarReporte() {
    $('#btnConsultarReporte').prop('hidden', true);
    $('#btnConsultarCargando').prop('hidden', false);
    $.ajax({
        url: "../ControlPesoyCodificacion/PartialReporteControlPesoCodificacion",
        type: "GET",
        data: {
            Fecha: $('#txtFecha').val(),
            Turno: $('#cmbTurno').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {
                $('#btnConsultarReporte').prop('hidden', false);
                $('#btnConsultarCargando').prop('hidden', true);
                //MensajeCorrecto(resultado[1],false);
                $('#DivPartialReporte').empty();
                $('#DivPartialReporte').html(resultado);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultarReporte').prop('hidden', false);
            $('#btnConsultarCargando').prop('hidden', true);

        }
    });
}