function ConsultarReporteToalla() {
    if ($('#Turno').prop('selectedIndex') == 0) {
        $('#msjerrorTurno').show();
        return false;
    } else {
        $('#msjerrorTurno').hide();
    }
    if ($('#Fecha').val() == '') {
        $('#msjerrorFecha').show();
        return false;
    } else {
        $('#msjerrorFecha').hide();
    }
    $('#spinnerCargando').prop("hidden", false);
    $.ajax({
        url: "../ControlToalla/PartialReporteToalla",
        type: "POST",
        data: {
            Turno: $('#Turno').val(),
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
            if ($('#contreg').val() == '0') {
                $('#DivReporteToalla').empty();
                $('#mensajeregistros').text('No existen Registros');
                //console.clear();
            } else{
                $('#mensajeregistros').text('');
            }
            //Nuevo();
            //$("#btnGuardar").prop("disabled", false);
            config.opcionesDT.pageLength = 15;
            config.opcionesDT.order = false;
            config.opcionesDT.ordering = false;
            $('#tblDataTable').DataTable(config.opcionesDT);
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