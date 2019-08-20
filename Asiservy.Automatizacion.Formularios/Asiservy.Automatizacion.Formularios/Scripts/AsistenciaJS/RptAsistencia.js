$('#tbldia').hide();
$('#tblmes').hide();
$('#tblsemana').hide();
$('#tblsemanarpt').DataTable({
    language: {
        "decimal": "",
        "emptyTable": "No hay información",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
        "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
        "infoFiltered": "(Filtrado de _MAX_ total entradas)",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": "Mostrar _MENU_ Entradas",
        "loadingRecords": "Cargando...",
        "processing": "Procesando...",
        "search": "Buscar:",
        "zeroRecords": "Sin resultados encontrados",
        "paginate": {
            "first": "Primero",
            "last": "Ultimo",
            "next": "Siguiente",
            "previous": "Anterior"
        }
    },
    ordering: false,
    pageLength: 5,
    lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']]

});
$('#tblrptmes').DataTable({
    language: {
        "decimal": "",
        "emptyTable": "No hay información",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
        "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
        "infoFiltered": "(Filtrado de _MAX_ total entradas)",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": "Mostrar _MENU_ Entradas",
        "loadingRecords": "Cargando...",
        "processing": "Procesando...",
        "search": "Buscar:",
        "zeroRecords": "Sin resultados encontrados",
        "paginate": {
            "first": "Primero",
            "last": "Ultimo",
            "next": "Siguiente",
            "previous": "Anterior"
        }
    },
    ordering: false,
    pageLength: 5,
    lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']]

});
$('#tblrptdia').DataTable({
    language: {
        "decimal": "",
        "emptyTable": "No hay información",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
        "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
        "infoFiltered": "(Filtrado de _MAX_ total entradas)",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": "Mostrar _MENU_ Entradas",
        "loadingRecords": "Cargando...",
        "processing": "Procesando...",
        "search": "Buscar:",
        "zeroRecords": "Sin resultados encontrados",
        "paginate": {
            "first": "Primero",
            "last": "Ultimo",
            "next": "Siguiente",
            "previous": "Anterior"
        }
    },
    ordering: false,
    pageLength: 5,
    lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']]

});
$('#trlineas').hide();
$('#comboarea').change(function () {
    if ($(this).val() == 'Procesos') {
        $('#trlineas').show();
    } else {
        $('#trlineas').hide();
    }
});
function ConsultarEmpleados() {
    if (($('#combodia').val() != "0") && ($('#combomes').val() != "0") && ($('#combosemana').val() != "0")) {
        $('#tbldia').show();
    }
    if (($('#combodia').val() == "0") && ($('#combomes').val() != "0") && ($('#combosemana').val() != "0")) {
        $('#tblsemana').show();
    }
    if (($('#combodia').val() == "0") && ($('#combomes').val() != "0") && ($('#combosemana').val() == "0")) {
        $('#tblmes').show();
    }
    if (($('#combodia').val() == "0") && ($('#combomes').val() == "0") && ($('#combosemana').val() == "0")) {
        $('#tbldia').hide();
        $('#tblmes').hide();
        $('#tblsemana').hide();
    }

}
