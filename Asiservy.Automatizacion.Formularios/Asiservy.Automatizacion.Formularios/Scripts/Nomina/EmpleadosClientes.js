$(document).ready(function () {
    CargarOpciones();
    
    $("#DivTableEmpleadosClientes").on("click", ".enviarSap", function () {
        
        return false;
    });
    $("#DivTableEmpleadosClientes").on("click", "#enviarMarcadosSap", function () {

        return false;
    });

    var nuevoBtn = {
        text: '<i class="fas fa-paper-plane fa-2x" style="vertical-align: middle;"></i>',
        titleAttr: 'Enviar marcados a SAP',
        className: '',
        action: enviarSAP
    };
    config.opcionesDT.buttons.push(nuevoBtn);
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
function enviarSAP() {
    var totalSeleccionados = $('.checkEmpleado:checkbox:checked').length;
    if (totalSeleccionados > 0) {
        var resp = confirm("Se ha seleccionado " + totalSeleccionados + " empleado(s), ¿Está seguro de enviar a crear como Clientes en SAP?");
        if (resp) {

        }
    } else {
        alert("Seleccione uno o más registros para continuar");
    }
    return false;
}

