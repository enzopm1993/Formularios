
$(document).ready(function () {
    ConsultarSolicitudes();

});

function CambioLinea(valor) {
    $("#selectArea").empty();
    $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoAreas",
        type: "Get",
        data:
        {
            CodLinea: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}


function MarcarSalida(IdSolicitudPermiso,fecha,fechaSalida) {
    //console.log(IdSolicitudPermiso);
    // console.log(fecha);
    if (fecha == "" || fecha == null ) {

        MensajeAdvertencia("No ha marcado en el biometríco");
        return;
    }

    $.ajax({
        type: "POST",
        url: '../SolicitudPermiso/MarcarSalidaSolicitudPermiso',
        data: {
            IdSolicitudPermiso: IdSolicitudPermiso,
            FechaBiometrico: fecha,
            FechaSalida: fechaSalida
        },
        success: function (Resultado) {
            if (Resultado == "101") {
                window.location.reload();
            }
            if (Resultado == "1") {
                var horaSalida = moment(fechaSalida).format('HH:mm');
                MensajeAdvertencia("Su hora de salida es a las: " + horaSalida);
                return;
            }
            MensajeCorrecto(Resultado,false);
            ConsultarSolicitudes();
        },
        error: function (Resultado) {
            MensajeError(Resultado);
        }
    });
   

}

function ConsultarSolicitudes() {
    $("#spinnerCargando").prop("hidden", false);
    $('#RptSolicitudes').html('');
    $.ajax({
        type: "GET",
        url: '../SolicitudPermiso/ConsultaSolicitudes',
        data: {
            dsLinea: $('#selectLinea').val(),
            dsArea: $('#selectArea').val(),
            dsEstado: $('#selectEstado').val(),
            dsGarita: $('#Garita').val(),
            ddFechaDesde: $("#txtFechaDesde").val(),
            ddFechaHasta: $("#txtFechaHasta").val(),
        },
        success: function (data) {
            $('#RptSolicitudes').html(data);
            $("#spinnerCargando").prop("hidden", true);
            config.opcionesDT.pageLength = -1;
            config.opcionesDT.order = [[2, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (result)
        {
            MensajeError(result);
            $("#spinnerCargando").prop("hidden", true);
            
        }
    });

}

