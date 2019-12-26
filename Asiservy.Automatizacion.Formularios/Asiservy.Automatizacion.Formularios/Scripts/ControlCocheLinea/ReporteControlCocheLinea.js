

function CargarControlCoche() {
    if ($("#txtFecha").val() == '') {
        $('#txtValidaFecha').prop('hidden', false);
        return;
    }
    else {
        $('#txtValidaFecha').prop('hidden', true);

    }
    $('#spinnerCargando').prop("hidden", false);
    $('#DivTableReporteControlCoche').html('');
    $.ajax({
        url: "../ControlCocheLinea/ReporteControlCocheLineaPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            var DivControl = $('#DivTableReporteControlCoche');
            DivControl.html(resultado);
            $('#spinnerCargando').prop("hidden", true);
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#spinnerCargando').prop("hidden", true);
           

        }
    });

}