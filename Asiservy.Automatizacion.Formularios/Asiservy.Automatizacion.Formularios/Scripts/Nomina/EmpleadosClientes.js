$(document).ready(function () {
    CargarOpciones();
    
    $("#DivTableEmpleadosClientes").on("click", ".enviarSap", function () {
        
        return false;
    });
    $("#DivTableEmpleadosClientes").on("click", "#enviarMarcadosSap", function () {

        return false;
    });


    

  

    var nuevoBtn = {
        text: '<i class="far fa-check-square fa-2x" style="vertical-align: middle;"></i>',
        titleAttr: 'Seleccionar filtrados',
        className: '',
        action: seleccionarFiltrados
    };
    config.opcionesDT.buttons.push(nuevoBtn);
    nuevoBtn = {
        text: '<i class="far fa-minus-square fa-2x" style="vertical-align: middle;"></i>',
        titleAttr: 'Quitar selección',
        className: '',
        action: quitarSeleccionFiltrados
    };
    config.opcionesDT.buttons.push(nuevoBtn);
    nuevoBtn = {
        text: '<i class="far fa-paper-plane fa-2x" style="vertical-align: middle;"></i>',
        titleAttr: 'Enviar marcados a SAP',
        className: '',
        action: enviarSAP
    };
    config.opcionesDT.buttons.push(nuevoBtn);
});
var table;
function CargarOpciones() {
    $.ajax({
        url: "../Nomina/ListaEmpleadosPartial",
        type: "GET",
        success: function (resultado) {

            var contenidoTabla = $('#DivTableEmpleadosClientes');
            contenidoTabla.html('');
            contenidoTabla.html(resultado);
            config.opcionesDT.pageLength = -1;
            config.opcionesDT.order = [[1, "asc"]];
            config.opcionesDT.orderCellsTop = true;
            config.opcionesDT.fixedHeader = true;


            $('#tblDataTable thead tr').clone(true).appendTo('#tblDataTable thead');
            $('#tblDataTable thead tr:eq(1) th').each(function (i) {
                if (i > 0 && i < 6) {
                    var title = $(this).text();
                    $(this).html('<input type="text" placeholder="Buscar ' + $.trim(title) + '" />');

                    $('input', this).on('keyup change', function () {
                        if (table.column(i).search() !== this.value) {
                            table
                                .column(i)
                                .search(this.value)
                                .draw();
                        }
                    });
                }
               
            });

            table = $('#tblDataTable').DataTable(config.opcionesDT);

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
            $.ajax({
                url: "../Nomina/ProcesarEnvioEmpleados",
                type: "POST",
                success: function (resultado) {

                },
                error: function (resultado) {
                    MensajeError(resultado, false);
                }
            });
        }
    } else {
        alert("Seleccione uno o más registros para continuar");
    }
    return false;
}

function seleccionarFiltrados() {

    $("#tblDataTable  > tbody  > tr").each(function (index, tr) {
        $(tr).find("td.td_cedula").each(function (t,td) {
            $(td).find("input.checkEmpleado").prop("checked", true);
        });
        
    });
}
function quitarSeleccionFiltrados() {

    $("#tblDataTable  > tbody  > tr").each(function (index, tr) {
        $(tr).find("td.td_cedula").each(function (t, td) {
            $(td).find("input.checkEmpleado").prop("checked", false);
        });

    });
}
