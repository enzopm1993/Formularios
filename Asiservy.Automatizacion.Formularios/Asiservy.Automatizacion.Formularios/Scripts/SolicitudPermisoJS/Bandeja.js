//import { parse } from "path";

function Grabar() {
    Mensaje("Registro Guardado..");
}

function AprobarSolicitud(valor) {
   // console.log(valor);
    $.ajax({
            url: '../SolicitudPermiso/AprobarSolicitud',
            type: 'GET',
            data: {
                diIdSolicitud:valor
            },
            success: function (resultado) {
                MensajeCorrecto(resultado + "\n Solicitud Aprobada");
            }
            ,
            error: function () {
                MensajeError("No se ha podido obtener la información");
            }
        });
}

function Anular(valor) {
    $.ajax({
        url: '../SolicitudPermiso/AnularSolicitud',
        type: 'GET',
        data: {
            diIdSolicitud: valor,
            dsObservacion: $('txtObservacion').val()
        },
        success: function (resultado) {
            MensajeCorrecto(resultado+"\n Solicitud Anulada");
        }
        ,
        error: function () {
            MensajeError("No se ha podido obtener la información");
        }
    });
}

function Observacion(valor) {
    console.log(valor);
    $('#ModalObservacion').modal("show");
    var prueba = document.getElementById("txtObservacion");
    prueba = "Hola";
    console.log(prueba);
   
//    $('#txtObservacion').value() =valor ;

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
        var desSol="solicitud"
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
