$(document).ready(function () {
    CargarReporte();

});
function CargarReporte() {
    if ($('#txtFecha').val() == "") {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableReporte').html('');
    $('#DivMensaje').html('');
    $.ajax({
        url: "../ControlPesoEnlatado/ReporteControlPesoEnlatadoPartial",
        type: "GET",
        data: {
            Fecha: $('#txtFecha').val(),
            Turno: $("#txtTurno").val()           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == '0') {               
                $('#DivMensaje').html("<h3>No existen registros</h3>");                
                return;
            }
            $('#DivTableReporte').html(resultado);
          //  console.log(resultado);
            config.opcionesDT.pageLength = 50;
            config.opcionesDT.order = false;
            config.opcionesDT.ordering = false;
            config.opcionesDT.paging = false;         

            var cantidad = $("#txtCantidadDetalle").val();          
            for (i = 1; i <= cantidad; i++) {
                 $('#tblDataTable-'+i).DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}