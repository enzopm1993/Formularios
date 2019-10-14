

function CargarControlCoche() {
    if ($("#txtFecha").val() == '') {
        $('#txtValidaFecha').prop('hidden', false);
        return;
    }
    else {
        $('#txtValidaFecha').prop('hidden', true);

    }
    $.ajax({
        url: "../ControlCocheLinea/ReporteControlCocheLineaPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            var DivControl = $('#DivTableReporteControlCoche');
            DivControl.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });

}