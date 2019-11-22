$(document).ready(function () {

    
    $(".cargaDatos").click(function () {
        
        
        if ($(this).attr("tipoSolicitud") == 'datos') {
            limpiarCampos();
            var id_solicitud = $(this).attr("idTipo");       
            $("#cargaDatos_datos_" + id_solicitud).hide();
            $("#procesando_datos_" + id_solicitud).show();
            $(".cargaDatos").attr('href', "javascript:void(0)");
            $.ajax({
                dataType: "json",
                url: "../App/InfoSolicitudDatos/" + id_solicitud,
                type: "GET",
                success: function (resultado) {
                    
                    $("#txtCodigoSolicitud").val(resultado.id);
                    $("#txtDireccion").val(resultado.direccion);
                    $("#txtBarrio").val(resultado.barrio);
                    $("#txtTelefono").val(resultado.telefono);
                    $("#txtCelular").val(resultado.celular);
                    $("#txtCorreo").val(resultado.correo);

                    if (resultado.cambia_direccion) {
                        $("#txtDireccion").addClass('datoCambia');
                    } else {
                        $("#txtDireccion").attr('disabled', 'disabled');
                    }
                    if (resultado.cambia_barrio) {
                        $("#txtBarrio").addClass('datoCambia');
                    } else {
                        $("#txtBarrio").attr('disabled', 'disabled');
                    }
                    if (resultado.cambia_telefono) {
                        $("#txtTelefono").addClass('datoCambia');
                    } else {
                        $("#txtTelefono").attr('disabled', 'disabled');
                    }
                    if (resultado.cambia_celular) {
                        $("#txtCelular").addClass('datoCambia');
                    } else {
                        $("#txtCelular").attr('disabled', 'disabled');
                    }
                    if (resultado.cambia_correo) {
                        $("#txtCorreo").addClass('datoCambia');
                    } else {
                        $("#txtCorreo").attr('disabled', 'disabled');
                    }
                    $("#procesando_datos_" + id_solicitud).hide();
                    $("#cargaDatos_datos_" + id_solicitud).show();
                    $(".cargaDatos").attr('href', "#");
                    $("#DatosEmpleado").modal({ backdrop: 'static', keyboard: false, show: true });
                },
                error: function (resultado) {
                    MensajeError(resultado.statusText, false);
                }
            });
        } else {

        }
       
     
        return false;
    });

    $("#rechazarSolicitud").click(function () {
        var observacion = null;
        var r = confirm("¿Está seguro de rechazar esta solicitud?");
        if (r == true) {
            do {
                observacion = prompt("Por favor, especifica el motivo de rechazo de la solicitud");
            }while(observacion == '')
           
        }
        
        if (observacion != null) {
            cambiarEstadoSolicitud("R", observacion); 
        }
      
        return false;
    });

    $("#sincronizarDL").click(function () {
        cambiarEstadoSolicitud("A","");
        return false;
    });

    function cambiarEstadoSolicitud(desde,observacion) {
               
        $.ajax({
            dataType: "json",
            url: "../App/ActualizaEstadoSolicitud",
            type: "POST",
            data: {
                id: $("#txtCodigoSolicitud").val(),
                estado: desde,
                observacion: observacion,
                username: $("#usernameLogin").val()
            },
            success: function (resultado) {
                console.log(resultado);
                if (resultado.Codigo == "1") {
                    alert("Registro actualizado con éxito");
                    window.location.reload(false);
                } else {
                    alert(resultado.Descripcion);
                }
            },
            error: function (resultado) {
                MensajeError(resultado.statusText, false);
            }
        });
    }
    
    function limpiarCampos() {        
        $(".campoSolicitud").val('');
        $(".campoSolicitud").removeClass('datoCambia');
        $(".campoSolicitud").removeAttr('disabled');
        $(".procesa_loader").hide();
    } 

});
