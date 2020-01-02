function ConsultarAsistencia() {
    if ($('#FechaDesde').val() == '') {
        $('#msjerrorFecha1').show();
        return false;
    } else {
        $('#msjerrorFecha1').hide();
    }
    if ($('#Linea').val() == '') {
        $('#msjerrorLinea').show();
        return false;
    } else {
        $('#msjerrorLinea').hide();
    }
    if ($('#Turno').val() == '') {
        $('#msjerrorTurno').show();
        return false;
    } else {
        $('#msjerrorTurno').hide();
    }
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Asistencia/RptAsistenciaPartial",
        type: "GET",
        data:
        {
            FechaInicio: $('#FechaDesde').val(),
            FechaFin: $('#FechaHasta').val(),
            Linea: $('#Linea').val(),
            Turno: $('#Turno').val()
        },
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            $('#divRptAsistencia').html(resultado);
            //MensajeCorrecto("Registro ingresado con éxito", false);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}