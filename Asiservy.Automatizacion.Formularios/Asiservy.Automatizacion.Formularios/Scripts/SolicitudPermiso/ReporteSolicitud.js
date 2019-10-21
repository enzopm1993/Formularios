
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


function MarcarSalida(IdSolicitudPermiso,fecha) {
    //console.log(IdSolicitudPermiso);
    //console.log(fecha);
    $.ajax({
        type: "POST",
        url: '../SolicitudPermiso/MarcarSalidaSolicitudPermiso',
        data: {
            IdSolicitudPermiso: IdSolicitudPermiso,
            FechaBiometrico: fecha
        },
        success: function (Resultado) {
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

        }
    });

}

