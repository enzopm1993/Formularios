$(document).ready(function () {
    CargarReporte();
});

var model = [];

function CargarReporte() {
    
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }



    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableReporteProyeccion').html('');
    $.ajax({
        url: "../ProyeccionProgramacion/ProyeccionProgramacionEstadoLotePartial",
        type: "GET",
        data: { Fecha: $('#txtFecha').val(), Turno: $('#selectTurno').val() },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#DivTableReporteProyeccion').html(resultado);
            $("#spinnerCargando").prop("hidden", true);
            config.opcionesDT.order = [1, "asc"];
            config.opcionesDT.pageLength = 50;
            $('#tblDataTable').DataTable(config.opcionesDT);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function CerrarLote(m) {
    console.log(m);
    model = m;
    $("#txtOrdenFabricacion").val(model.OrdenFabricacion);
    $("#txtLote").val(model.Lote);
    $("#modalCambiarLote").modal("show");
}


function GuardarCerrarLote() {
    $.ajax({
        url: "../ProyeccionProgramacion/CerrarLote",
        type: "POST",
        data: {
            IdProyeccionProgramacionDetalle: model.IdProyeccionProgramacionDetalle
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#modalCambiarLote").modal("hide");
            MensajeCorrecto(resultado);
            CargarReporte();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
        }
    });
}