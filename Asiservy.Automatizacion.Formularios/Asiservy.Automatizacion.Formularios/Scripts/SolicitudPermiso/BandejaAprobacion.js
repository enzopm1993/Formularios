
function AprobarSolitudes() {
    var result = new Array();    
    i = 0;
    $("input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        this.id = id.replace('solicitud-', '');
        result.push(this.id);
        i++;
    });
    //console.log(result);
    Aprobar(result);    
}
function AprobarSolicitud(valor) {
   // console.log(valor);
    var solicitud=[];
    solicitud[0] = valor;
    Aprobar(solicitud);  
}

function Aprobar(result) {
    //console.log(result);
    $("#tdAcciones").prop("hidden", true);
    $("#btnAprobar").prop("hidden", true);
    $("#btnArpobarEspera").prop("hidden", false);
    var resultado = JSON.stringify(result)
    var resultado2 = JSON.parse(resultado)
    console.log(resultado2);
    $.ajax({
        url: '../SolicitudPermiso/AprobarSolicitud',
        type: 'POST',
        dataType: "json",
        data: {
            diIdSolicitud: resultado2           
        },
        success: function (resultado) {
            MensajeCorrecto(resultado, true);
            $("#btnAprobar").prop("hidden", false);
            $("#tdAcciones").prop("hidden", false);

            $("#btnArpobarEspera").prop("hidden", true);
        }
        ,
        error: function () {
            MensajeError("No se ha podido obtener la información", false);
            $("#btnAprobar").prop("hidden", false);
    $("#tdAcciones").prop("hidden", false);
            $("#btnArpobarEspera").prop("hidden", true);
        }
    });
}

function Anular() {
    valor = document.getElementById("txtIdSolicitud").value;
    Observacion = document.getElementById("txtObservaccionAnulacion").value;
    //console.log(Observacion);
    if (!Observacion || Observacion == undefined || Observacion == "" || Observacion.length == 0)
    {
        MensajeCorrecto("Debe ingresar un motivo");
    } else {
        $.ajax({
            url: '../SolicitudPermiso/AnularSolicitud',
            type: 'GET',
            data: {
                diIdSolicitud: valor,
                dsObservacion: " -Anulación: "+Observacion
            },
            success: function (resultado) {
                MensajeCorrecto(resultado + "\n Solicitud Anulada",true);
            }
            ,
            error: function () {
                MensajeError("No se ha podido obtener la información",false);
            }
        });
    }
}

function Observacion(valor) {
   // console.log(valor);
    document.getElementById("txtObservaccionAnulacion").value = "";
    document.getElementById("txtIdSolicitud").value = valor;
    $('#ModalObservacion').modal("show");
}

function Mostrar(valor) {
    //console.log(valor);
    MostrarModalCargando();
    var sPath = window.location.pathname;
    var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
    $.ajax({
        url: '../SolicitudPermiso/SolicitudPermisoEdit',
        type: 'GET',
        data: {
            dsSolicitud: valor,
            frm: sPage
        },
        success: function (resultado) {
            CerrarModalCargando();
            document.getElementById("modal_body").innerHTML = resultado;
            $('#ModalAprobacion').modal('toggle');
        }
        ,
        error: function () {
            CerrarModalCargando();

            MensajeError("No se ha podido obtener la información",false);
        }
    });
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


//function checkTodos() {
//    var i = 1;
//    var bool = document.getElementById("checkTodos").checked;
//        console.log('prueba');

//    $('#TableBandeja tr').each(function () {       
//        var desSol="solicitud"
//        var x = $(this).find("td").eq(1).html();
//        console.log(x);
//        if (x != null) {
//            desSol += i;
//            document.getElementById(desSol).checked = bool;
//            i++;
//        }
//    });
//}



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
