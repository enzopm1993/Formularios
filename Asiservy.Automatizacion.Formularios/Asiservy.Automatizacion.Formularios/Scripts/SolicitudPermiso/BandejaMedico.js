

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
            document.getElementById("modal_body").innerHTML = resultado;
            document.getElementById("frmName").value = sPage;
            $('#ModalAprobacion').modal('toggle');
            CerrarModalCargando();
        }
        ,
        error: function () {
            MensajeError("No se ha podido obtener la información", false);
            CerrarModalCargando();

        }
    });
}






