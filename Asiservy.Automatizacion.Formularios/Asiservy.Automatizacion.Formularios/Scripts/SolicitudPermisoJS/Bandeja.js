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
    $('#TableBandejaRRHH tr').each(function () {       
        var desSol="solicitud"
        var x = $(this).find("td").eq(1).html();
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


//$(document).ready(function () {
//    $("#search").keyup(function () {
//        _this = this;
//        // Show only matching TR, hide rest of them
//        $.each($("#WebGrid tbody tr"), function () {
//            if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1)
//                $(this).hide();
//            else
//                $(this).show();
//        });
//    });
//});



//    $("body").on("click", ".Grid tfoot a", function () {
//        $('#WebGridForm').attr('action', $(this).attr('href')).submit();
//    return false;
//});

//$('table tbody tr  td').on('click', function () {
//    $("#myModal").modal("show");
//    $("#txtSolicitud").val($(this).closest('tr').children()[0].textContent);
//    $("#txtFecha").val($(this).closest('tr').children()[1].textContent);
//    $("#txtMotivo").val($(this).closest('tr').children()[2].textContent);
//    $("#txtArea").val($(this).closest('tr').children()[3].textContent);
//    $("#txtEmpleado").val($(this).closest('tr').children()[4].textContent);

//    $("#txtSolicitud").prop('disabled', true);
//    $("#txtFecha").prop('disabled', true);
//    $("#txtMotivo").prop('disabled', true);
//    $("#txtArea").prop('disabled', true);
//    $("#txtEmpleado").prop('disabled', true);
//});
//comboFind


//function LimpiarTexto() {
//    $.each($("#TableBandejaRRHH tbody tr"), function () {
//        $(this).show();
//    });
//    document.getElementById("search").innerText = "";
//    $("#search").val("");
//}
