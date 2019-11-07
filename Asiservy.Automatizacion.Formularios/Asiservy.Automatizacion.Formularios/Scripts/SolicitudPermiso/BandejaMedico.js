//$('#buscarenfermedad').keyup(function () {
//    console.log("entro ");
//    var input, filter, table, tr, td, i, txtValue;
//    input = document.getElementById("buscarenfermedad");
//    filter = input.value.toUpperCase();
//    table = document.getElementById("tbldiagnostico");
//    tr = table.getElementsByTagName("tr");

//    // Loop through all table rows, and hide those who don't match the search query
//    for (i = 0; i < tr.length; i++) {
//        td = tr[i].getElementsByTagName("td")[0];
//        if (td) {
//            txtValue = td.textContent || td.innerText;
//            if (txtValue.toUpperCase().indexOf(filter) > -1) {
//                tr[i].style.display = "";
//            } else {
//                tr[i].style.display = "none";
//            }
//        }
//    }
//});
function mostrartabla() {
    $('#codsenfermedad').show();
    $('#buscarenfermedad').prop('disabled', false);
    $('#buscarenfermedad').val("");
    $('#CodigoDiagnostico').val("");
    $('#buscarenfermedad').focus();
}
function ocultartabla(codigo,descripcion) {
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
            CerrarModalCargando();
            $("#modal_body").html(resultado);
            document.getElementById("frmName").value = sPage;
            $('#ModalAprobacion').modal('show');
            //$('#ModalAprobacion').modal('toggle');
            //$("#CodDiagnostico").select2();
          
        }
        ,
        error: function () {
            MensajeError("No se ha podido obtener la información", false);
            CerrarModalCargando();

        }
    });
}

function sololetras(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8 || tecla==32 || tecla==13) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y letras
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}




