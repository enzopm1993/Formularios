
$(document).ready(function () {
    ConsultarReporteControlMaterial();
});



function Validar() {
    var valida = true;
    if ($("#txtFechaDesde").val() == "") {
        $("#txtFechaDesde").css("border-color", "#DC143C");//#d1d3e2
        valida = false;
    } else {
        $("#txtFechaDesde").css("border-color", "#d1d3e2");
    }
    if ($("#txtFechaHasta").val() == "") {
        $("#txtFechaHasta").css("border-color", "#DC143C");//#d1d3e2
        valida = false;
    } else {
        $("#txtFechaHasta").css("border-color", "#d1d3e2");
    }

    if ($("#selectLinea").val() == "") {
        $("#selectLinea").css("border-color", "#DC143C");//#d1d3e2
        valida = false;
    } else {
        $("#selectLinea").css("border-color", "#d1d3e2");
    }

    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css("border-color", "#DC143C");//#d1d3e2
        valida = false;
    } else {
        $("#selectTurno").css("border-color", "#d1d3e2");
    }
    return valida;
}



function ConsultarReporteControlMaterial() {

    if (!Validar()) {
        return;
    }

    $("#spinnerCargando").prop("hidden", false);
    $("#divTablaControlDetalle").html('');
    $("#DivMensaje").html("");
    $.ajax({
        url: "../ControlMaterialQuebradizo/ReporteControlMaterialQuebradizoPartial",
        type: "GET",
        data:
        {
            FechaDesde: $('#txtFechaDesde').val(),
            FechaHasta: $('#txtFechaHasta').val(),
            Linea: $('#selectLinea').val(),
            Turno:$("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
                $("#btnEliminar").prop("hidden", false);
                $("#btnObservacion").prop("hidden", false);

            } else {
                $("#divTablaControlDetalle").html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
                $("#btnEliminar").prop("hidden", false);
                $("#btnObservacion").prop("hidden", false);
            }
            $("#spinnerCargando").prop("hidden", true);
            $('#btnGenerar').prop('hidden', true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}