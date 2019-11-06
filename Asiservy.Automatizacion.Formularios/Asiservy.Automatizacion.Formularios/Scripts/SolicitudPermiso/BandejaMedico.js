
function ValidarMedico(idSolicitud) {
    console.log(idSolicitud); 
    
    $.ajax({
        url: '../SolicitudPermiso/ValidarMedicoSolicitud',
        type: 'POST',
        dataType: "json",
        data: {
            diIdSolicitud: idSolicitud
        },
        success: function (resultado) {
            MensajeCorrecto(resultado + "\n Solicitud Finalizada", true);
        }
        ,
        error: function () {
            MensajeError("No se ha podido obtener la información", false);
        }
    });
}


function Mostrar(valor) {
    //console.log(valor);
    var sPath = window.location.pathname;
    var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
    MostrarModalCargando();
    $.ajax({
        url: '../SolicitudPermiso/SolicitudPermisoEdit',
        type: 'GET',
        data: {
            dsSolicitud: valor,
            frm: sPage
        },
        success: function (resultado) {
            //document.getElementById("modal_body").innerHTML = resultado;
            $("#modal_body").html(resultado);

            document.getElementById("frmName").value = sPage;
            $('#ModalAprobacion').modal('show');
            //$('#ModalAprobacion').modal('toggle');
            $("#CodDiagnostico").select2();
            CerrarModalCargando();
        }
        ,
        error: function () {
            MensajeError("No se ha podido obtener la información", false);
            CerrarModalCargando();

        }
    });
}








$(document).ready(function () {
    $('#TableBandejaMedico').DataTable({
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
