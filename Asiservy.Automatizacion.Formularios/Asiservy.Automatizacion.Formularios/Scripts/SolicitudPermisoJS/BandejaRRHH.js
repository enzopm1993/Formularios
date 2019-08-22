
function Observacion() {
    console.log("ok");
    $('#ModalObservacion').modal("show");

}
function Guardar() {
    Mensaje("Registro Guardado..");
}

function Aprobar() {
    Mensaje("Solicitud aprobada..");
}

function Anular() {
    Mensaje("Solicitud anulada..");
}

$(document).ready(function () {
    $('#TableBandeja').DataTable({
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
        "pagingType": "full_numbers"
    });
});


function checkTodos() {
    var i = 1;
    var bool = document.getElementById("checkTodos").checked;
    console.log('prueba');

    $('#TableBandeja tr').each(function () {
        var desSol = "solicitud"
        var x = $(this).find("td").eq(1).html();
        console.log(x);
        if (x != null) {
            desSol += i;
            document.getElementById(desSol).checked = bool;
            i++;
        }
    });
}
function Mostrar() {
    $('#ModalAprobacion').modal('toggle')
}

