﻿var sPath = window.location.pathname;
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
//**
function MensajeCorrectoTiempo(mensaje, r, tiempo) {
    if (sPage = 'SolicitudPermiso') {
        $('#GuardarSolicitudGeneral').prop('readonly', true);
    }
    if (sPage = 'SolicitudPermisoDispensario') {
        $('#GuardarSolicitudDispensario').prop('readonly', true);
    }


    $('body,html').animate({ scrollTop: 0 }, 500);

    $('#response').prop('hidden', false);
    $('#response').html('');
    $('<div class="alert alert-success">' +
        '<button type="button" class="close" onclick="CerrarMensaje()" style="height: min-content;padding: 5px;" data-dismiss="alert">' +
        '&times;</button><label id="pMensaje"></label></div>').hide().appendTo('#response').fadeIn(1000);
    $('#pMensaje').html(mensaje);


    $(".alert").delay(tiempo).fadeOut(
        "normal",
        function () {
            $(this).remove();
            $('#response').prop('hidden', true);
        });


    if (r)
        setTimeout('location.reload()', tiempo+500);


}
//**
function MensajeCorrecto(mensaje, r) {
    if (sPage = 'SolicitudPermiso') {
        $('#GuardarSolicitudGeneral').prop('readonly', true);
    }
    if (sPage = 'SolicitudPermisoDispensario') {
        $('#GuardarSolicitudDispensario').prop('readonly', true);
    }
   
  
    $('body,html').animate({ scrollTop: 0 }, 500);  

    $('#response').prop('hidden',false);
    $('#response').html('');
    $('<div class="alert alert-success">' +
        '<button type="button" class="close" onclick="CerrarMensaje()" style="height: min-content;padding: 5px;" data-dismiss="alert">' +
        '&times;</button><label id="pMensaje"></label></div>').hide().appendTo('#response').fadeIn(1000);
    $('#pMensaje').html(mensaje);
   
    
    $(".alert").delay(2000).fadeOut(
        "normal",
        function () {
            $(this).remove();
            $('#response').prop('hidden', true);
        });
   

    if (r) 
        setTimeout('location.reload()', 2500);
   

}

function CerrarMensaje(){
    $('#response').prop('hidden', true);
}

function MensajeError(mensaje, r) {
  
    $('body,html').animate({ scrollTop: 0 }, 500);
    $('#response').prop('hidden', false);
    $('#response').html('');
    $('<div class="alert alert-danger">' +
        '<button type="button" class="close" onclick="CerrarMensaje()" data-dismiss="alert">' +
        '&times;</button><p id="pMensaje"></p></div>').hide().appendTo('#response').fadeIn(1000);
    $('#pMensaje').html(mensaje);
    //$(".alert").delay(3000).fadeOut(
    //    "normal",
    //    function () {
    //        $(this).remove();
    //    });
    //if (r)
    //    location.reload();
    //console.log("Prueba de Mensaje");
    //console.log(mensaje);
    //$.ajax({
    //    url: "../Mensaje/Error",
    //    type: "Get",
    //    data: { reload: r },
    //    success: function (resultado) {
    //        $('#ModalMensaje').html(resultado);
    //        //var m = document.getElementById("ModalMensaje");
    //        //m.innerHTML = resultado;
    //        $('#mensajeError').html(mensaje);
    //        $("#ModalError").modal("show");
    //        //document.getElementById('mensajeError').innerHTML = mensaje;
    //           //console.log(mensaje);
    //    },
    //    error: function (resultaod) {
    //        console.log(resultaod);
    //    }
        
    //});
}



function MensajeAdvertencia(mensaje, r) {

    $('body,html').animate({ scrollTop: 0 }, 500);
    $('#response').prop('hidden', false);
    $('#response').html('');
    $('<div class="alert alert-warning">' +
        '<button type="button" class="close" onclick="CerrarMensaje()" data-dismiss="alert">' +
        '&times;</button><p id="pMensaje"></p></div>').hide().appendTo('#response').fadeIn(1000);
    $('#pMensaje').html(mensaje);
    $(".alert").delay(2500).fadeOut(
        "normal",
        function () {
            $(this).remove();
            $('#response').prop('hidden', true);
        });
    //if (r)
    //    location.reload();
    //$.ajax({
    //    url: "../Mensaje/Advertencia",
    //    type: "Get",
    //    data: { reload: r },
    //    success: function (resultado) {
    //        var m = document.getElementById("ModalMensaje");
    //        m.innerHTML = resultado;
    //        //var modal = document.getElementById("ModalError");
    //        $("#ModalAdvertencia").modal("show");
    //        document.getElementById('mensajeAdvertencia').innerHTML = mensaje;
    //        //console.log(mensaje);
    //    }
    //});
}


