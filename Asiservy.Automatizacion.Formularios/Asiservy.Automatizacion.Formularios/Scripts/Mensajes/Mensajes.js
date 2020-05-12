var sPath = window.location.pathname;
var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
var IdDivAprobar;
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

function MensajeConfirmacion(DivContenedor,IdModal,FuncionSi,mensaje) {
    $('#' + DivContenedor).html('');
    $('#' + DivContenedor).prop('hidden', false);
    $('<div class="alert alert-warning absolute1" role="alert" id="confirmaciondivalert">' +
        '<p id = "pMensaje">' + mensaje + '</p> <input type="button" id="BtnSi" class="btn btn-secondary" value="Si" onclick="' + FuncionSi +'"> &nbsp;  <input type="button" id="BtnNo" class="btn btn-secondary disabled" value="No" onclick="CerrarConfirmacionAprobar()">' +
        '<button class="btn btn-primary" type="button" disabled hidden id="btnCargando">' +
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>' +
        'Espere...' +
        '</button>'+
        '</div>').hide().appendTo('#' + DivContenedor).fadeIn(1000);
    $('#' + IdModal).animate({ scrollTop: 0 }, 500);
    IdDivAprobar = '#' + DivContenedor;
}
function CerrarConfirmacionAprobar() {
    $(IdDivAprobar).prop('hidden', true);
}