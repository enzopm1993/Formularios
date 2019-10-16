

function CargarReporte() {

    if ($('#txtFecha').val() == "") {
        MensajeAdvertencia("Ingrese una fecha");
        return;
    }
    MostrarModalCargando();

    $.ajax({
        url: "../ProyeccionProgramacion/ReporteProyeccionProgramacionPartial",
        type: "GET",
        data: { Fecha: $('#txtFecha').val() },
        success: function (resultado) {     
            $('#DivTableReporteProyeccion').html(resultado);
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            CerrarModalCargando(); 
        }
    });
}

