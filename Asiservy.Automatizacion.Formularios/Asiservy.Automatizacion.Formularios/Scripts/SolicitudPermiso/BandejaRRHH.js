function mostrartabla() {
    $('#codsenfermedad').show();
    $('#buscarenfermedad').prop('disabled', false);
    $('#buscarenfermedad').val("");
    $('#CodigoDiagnostico').val("");
    $('#buscarenfermedad').focus();
}
function ocultartabla(codigo, descripcion) {
    $('#codsenfermedad').hide();
    $('#CodigoDiagnostico').val(codigo);
    $('#buscarenfermedad').val(descripcion);
    $('#buscarenfermedad').prop('disabled', true);
}
function buscare() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("buscarenfermedad");
    filter = input.value.toUpperCase();
    table = document.getElementById("tbldiagnostico");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}
function FinalizarSolitudes() {
    var result = new Array();
    i = 0;
    $("input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        this.id = id.replace('solicitud-', '');
        result.push(this.id);
        i++;
    });
    //console.log(result);
    Finalizar(result);
}
function FinalizarSolicitud(valor) {
    // console.log(valor);
    var solicitud = [];
    solicitud[0] = valor;
    Finalizar(solicitud);
}

function Finalizar(result) {
   
    if (result.length > 0) {
        var resultado = JSON.stringify(result)
        var resultado2 = JSON.parse(resultado)
        //  console.log(resultado2);
        $("#btnFinalizarEspera").prop("hidden", false);
        $("#btnFinalizar").prop("hidden", true);
        $.ajax({
            url: '../SolicitudPermiso/FinalizarSolicitud',
            type: 'POST',
            dataType: "json",
            data: {
                diIdSolicitud: resultado2
            },
            success: function (resultado) {
                $("#btnFinalizarEspera").prop("hidden", true);
                $("#btnFinalizar").prop("hidden", false);
                //console.log(resultado[0]);
                //MensajeError(resultado[0].Mensaje);
                var divMensaje = $("#divMensaje");
                divMensaje.html('');
                for (var i = 0, l = resultado.length; i < l; i++) {
                    if (resultado[i].Respuesta) {
                        divMensaje.append('<label>' + resultado[i].Mensaje + '</label>')
                    } else {
                        divMensaje.append('<label class="text-danger">' + resultado[i].Mensaje + '</label>')

                    }
                }
                $("#ModalMensajeBandeja").modal("show");

               // MensajeCorrecto(resultado + "\n Solicitud Finalizada", true);
            }
            ,
            error: function (resultado) {
                $("#btnFinalizarEspera").prop("hidden", true);
                $("#btnFinalizar").prop("hidden", false);
                MensajeError(resultado.responseText, false);
            }
        });
    } else {
        MensajeAdvertencia("Seleccione al menos una solicitud");
    }
}
function Anular() {
    valor = document.getElementById("txtIdSolicitud").value;
    Observacion = document.getElementById("txtObservaccionAnulacion").value;
    console.log(Observacion);
    if (!Observacion || Observacion == undefined || Observacion == "" || Observacion.length == 0) {
        MensajeCorrecto("Debe ingresar un motivo");
    } else {
        $.ajax({
            url: '../SolicitudPermiso/AnularSolicitud',
            type: 'GET',
            data: {
                diIdSolicitud: valor,
                dsObservacion: " -Anulación: " +Observacion
            },
            success: function (resultado) {
                MensajeCorrecto(resultado + "\n Solicitud Anulada", true);
            }
            ,
            error: function () {
                MensajeError("No se ha podido obtener la información", false);
            }
        });
    }
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
            frm:sPage
        },
        success: function (resultado) {
            CerrarModalCargando();
            document.getElementById("modal_body").innerHTML = resultado;
           
            document.getElementById("frmName").value = sPage;
            //console.log(sPage);
            $('#ModalEditarSolicitud').modal('toggle');
         
        }
        ,
        error: function () {
            CerrarModalCargando();

            MensajeError("No se ha podido obtener la información",false);
        }
    });
}

function Observacion(valor) {
    console.log(valor);
    document.getElementById("txtObservaccionAnulacion").value = "";
    document.getElementById("txtIdSolicitud").value = valor;
    $('#ModalObservacion').modal("show");
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
