﻿function ConsultarCambioPersonal() {
    if ($('#Linea').prop('selectedIndex') == 0) {
        $('#msgerrorlinea').show();
        $('#Linea').focus();
        return false;
    } else {
        $('#msgerrorlinea').hide();
    }
    if ($('#fechadesde').val() == "") {
        //MensajeError("Debe ingresar la hora de inicio", false);

        $('#msgerrorfechaInicio').show();
        $('#fechadesde').focus();
        return false;
    } else {
        $('#msgerrorfechaInicio').hide();
    }
    if ($('#fechahasta').val() == "") {
        //MensajeError("Debe ingresar la hora de Fin", false);
        $('#msgerrorFechaFin').show();
        $('#fechahasta').focus();
        return false;
    } else {
        $('#msgerrorFechaFin').hide();
    }
    if ($('#fechahasta').val() < $('#fechadesde').val()) {
        $('#msgerrorFechas').show();
        return false;
    } else {
        $('#msgerrorFechas').hide();
    }
    $('#resultadoreporte').html('');
    $("#spinnerCargando").prop("hidden", false);
    $("#Consultar").prop("disabled", true);
    $.ajax({
        url: "../Asistencia/ReporteCambioPersonalPartial",
        type: "GET",
        data:
        {
            CodLinea: $('#Linea').val(),
            FechaInicio: $('#fechadesde').val(),
            FechaFin: $('#fechahasta').val()
        },
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            $("#Consultar").prop("disabled", false);
                $('#resultadoreporte').html(resultado);
            
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            $("#Consultar").prop("disabled", false);
            //console.log(resultado);
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}


