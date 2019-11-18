$(document).ready(function () {
    CargarOpciones();
});
function CargarOpciones() {
    $.ajax({
        url: "../Nomina/ListaEmpleadosPartial",
        type: "GET",
        success: function (resultado) {

            var contenidoTabla = $('#DivTableEmpleadosClientes');
            contenidoTabla.html('');
            contenidoTabla.html(resultado);
            config.opcionesDT.pageLength = 100;
            config.opcionesDT.order = [[1, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}
