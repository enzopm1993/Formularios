var sPath = window.location.pathname;
var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
function MensajeClose() {
    //location.reload();
   //$(this).modal('hide');
    if (sPage = 'SolicitudPermiso') {
        $('#GuardarSolicitudGeneral').prop('readonly', false);
    }
    if (sPage = 'SolicitudPermisoDispensario') {
        $('#GuardarSolicitudDispensario').prop('readonly', false);
    }
    //var sPath = window.location.pathname;
    //var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
    //alert(sPage);
    //if (sPage = "SolicitudPermiso - My ASP.NET Application") {
    //    $('#' + formulario).attr("disabled", false);
    //}
}

function MensajeCloseReload() {
    if (sPage = 'SolicitudPermiso') {
        $('#GuardarSolicitudGeneral').prop('readonly', false);
    }
    if (sPage = 'SolicitudPermisoDispensario') {
        $('#GuardarSolicitudDispensario').prop('readonly', false);
    }
    
    location.reload();
}

function MensajeCorrecto(mensaje, r) {
    if (sPage = 'SolicitudPermiso') {
        $('#GuardarSolicitudGeneral').prop('readonly', true);
    }
    if (sPage = 'SolicitudPermisoDispensario') {
        $('#GuardarSolicitudDispensario').prop('readonly', true);
    }
    $.ajax({
        url: "../Mensaje/Correcto",
        type: "Get",
        data: { reload: r },
        success: function (resultado) {
            var m = document.getElementById("ModalMensaje");
            m.innerHTML = resultado;
            //var modal = document.getElementById("ModalError");
            $("#ModalCorrecto").modal("show");
            document.getElementById('mensajeCorrecto').innerHTML = mensaje;
               //console.log(r);
        }
    });
}


function MensajeError(mensaje, r) {
    //console.log("Prueba de Mensaje");
    //console.log(mensaje);
    $.ajax({
        url: "../Mensaje/Error",
        type: "Get",
        data: { reload: r },
        success: function (resultado) {
            $('#ModalMensaje').html(resultado);
            //var m = document.getElementById("ModalMensaje");
            //m.innerHTML = resultado;
            $('#mensajeError').html(mensaje);
            $("#ModalError").modal("show");
            //document.getElementById('mensajeError').innerHTML = mensaje;
               //console.log(mensaje);
        },
        error: function (resultaod) {
            console.log(resultaod);
        }
        
    });
}



function MensajeAdvertencia(mensaje, r) {
    $.ajax({
        url: "../Mensaje/Advertencia",
        type: "Get",
        data: { reload: r },
        success: function (resultado) {
            var m = document.getElementById("ModalMensaje");
            m.innerHTML = resultado;
            //var modal = document.getElementById("ModalError");
            $("#ModalAdvertencia").modal("show");
            document.getElementById('mensajeAdvertencia').innerHTML = mensaje;
            //console.log(mensaje);
        }
    });
}


