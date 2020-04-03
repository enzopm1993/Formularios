$(document).ready(function () {
    ConsultarControl();
});

var model = [];

function ConsultarControl() {
    $("#divMensaje").html('');
    $("#divDetalle").prop("hidden", true);
    $("#divDetalle2").prop("hidden", true);
    $("#lblLomos").html('');
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {

                $("#divMensaje").html('NO SE HA GENERADO EL CONTROL');
                model = [];
            } else {
                $("#divDetalle").prop("hidden", false);
                $("#divDetalle2").prop("hidden", false);
                model = resultado;

                //console.log(model);

                $("#divMensaje").html('');
                if (model.Lomos) {
                    $("#lblLomos").html("<i class='fas fa-check-circle' style='color:greenyellow'></i>");
                }
                if (model.Latas) {
                    $("#lblLatas").html("<i class='fas fa-check-circle' style='color:greenyellow'></i>");
                }
                $("#lblFerroro").html(model.Ferroso);
                $("#lblNoFerroso").html(model.NoFerroso);
                $("#lblAceroInoxidable").html(model.AceroInoxidable);
                $("#pObservacion").html(model.Observacion);
                CargarControlDetalle();
                //CargarControlDetalle2();
           
            }

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas," + resultado, false);
        }
    });
}


function CargarControlDetalle() {
    $("#divTableDetalle").html('');
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../OperatividadMetal/ReporteOperatividadMetalDetallePartial",
        type: "GET",
        data: {
            IdControl: model.IdOperatividadMetal
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle").html("No existen registros");
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#divTableDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}