

    $('#tblDataTable').DataTable({
        "language": {
        "sProcessing": "Procesando...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sSearch": "Buscar:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando...",
            "oPaginate": {
        "sFirst": "Primero",
    "sLast": "Último",
    "sNext": "Siguiente",
    "sPrevious": "Anterior"
},
            "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
    "sSortDescending": ": Activar para ordenar la columna de manera descendente"
}
},
"pageLength": 5,
"lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
"pagingType": "full_numbers",
"dom": 'Bfrtip',
"order": [[0, "desc"]],
"buttons": [
            {
        extend: 'copyHtml5',
                text: ' <img style="width:100%" src="../../Content/icons/copy24.png" />',
    titleAttr: 'Copy'
},
            {
        extend: 'excelHtml5',
                text: '<img style="width:100%" src="../../Content/icons/excel24.png" />',
    titleAttr: 'Excel'
},
            {
        extend: 'print',
                text: '<img style="width:100%" src="../../Content/icons/print24.png" />',
    titleAttr: 'Print'
}
]

});