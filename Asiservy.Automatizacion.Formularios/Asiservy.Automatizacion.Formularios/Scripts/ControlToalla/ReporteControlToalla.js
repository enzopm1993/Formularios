function ConsultarReporteToalla() {
    $('#spinnerCargando').prop("hidden", false);
    $.ajax({
        url: "../ControlToalla/PartialReporteToalla",
        type: "POST",
        data: {
            Turno: $('#TurnoGen').val(),
            Fecha: $("#Fecha").val(),
            //Hora: $('#txtHora').val(),
            CodLinea: '52',
            //Observacion: $("#txtObservacion").val(),
        },
        success: function (resultado) {
            //CargarControlCoche();
            //MensajeCorrecto(resultado);
            $('#spinnerCargando').prop("hidden", true);
            $('#DivReporteToalla').html(resultado);
            //Nuevo();
            //$("#btnGuardar").prop("disabled", false);

        },
        error: function (resultado) {

            //CargarControlCoche();
            //$("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);

            MensajeError(resultado.responseText, false);
            //Nuevo();

        }
    });
}