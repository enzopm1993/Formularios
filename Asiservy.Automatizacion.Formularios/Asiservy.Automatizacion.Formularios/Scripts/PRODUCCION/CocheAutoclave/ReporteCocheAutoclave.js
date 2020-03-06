$(document).ready(function () {
   // CargarCocheAutoclave();
    CargarOrdenFabricacion();
});


function CargarReporteCocheAutoclave() {
    $("#chartCabecera2").html('');
  
    if ($("#SelectOrdenFabricacion").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../CocheAutoclave/ReporteCocheAutoclaveDetallePartial",
        type: "GET",
        data: {
            OrdenFabricacion: $("#SelectOrdenFabricacion").val()            
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}



function CargarOrdenFabricacion() {

    $("#chartCabecera2").html('');
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    valor = $("#txtFecha").val();
    if (valor == '' || valor == null)
        return;

    $.ajax({
        url: "../CocheAutoclave/ConsultaOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("No existen ordenes para esa fecha");
                return;
            }
            // LimpiarDetalle();
            if (!$.isEmptyObject(resultado)) {
               // console.log(resultado);
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row + "'>" + row + "</option>")
                });
                $('#validaFecha').prop("hidden", true);
            } else {
                $('#validaFecha').prop("hidden", false);
            }
            //CargarControlHueso();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}