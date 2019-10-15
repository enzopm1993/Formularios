

function CargarControlCoche() {
    if ($("#txtFecha").val() == '') {
        $('#txtValidaFecha').prop('hidden', false);
        return;
    }
    else {
        $('#txtValidaFecha').prop('hidden', true);

    }
    $('#btnGuardarCargando').prop("hidden", false);
    $('#btnGuardar').prop("hidden", true);
    $.ajax({
        url: "../ControlCocheLinea/ReporteControlCocheLineaPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            var DivControl = $('#DivTableReporteControlCoche');
            DivControl.html(resultado);
            $('#btnGuardarCargando').prop("hidden", true);
            $('#btnGuardar').prop("hidden", false);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnGuardarCargando').prop("hidden", true);
            $('#btnGuardar').prop("hidden", false);

        }
    });

}