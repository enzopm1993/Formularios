$(document).ready(function () {
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../MoverPersonal/PartialBandejaMovimientoPersonalRRHH",
        type: "POST",
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            //CerrarModalCargando();
            //console.log(resultado);
            $('#divPartialBandeja').empty();
            $('#divPartialBandeja').html(resultado);
            //console.log($('#Nregistros').val())
            if ($('#Nregistros').val() == '0') {
                $('#MensajeRegistros').show();
                $('#btnAprobar').hide();
            } else {
                $('#btnAprobar').show();
                $('#MensajeRegistros').hide();
            }
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado, false);
        }
    });
});
function AprobarSolicitud(IdSolicitud) {
    $("#Aprobar-" + IdSolicitud).prop("hidden", true);
    $("#AprobarCargando-" + IdSolicitud).prop("hidden", false);
    $.ajax({
        url: "../Nomina/ActualizaEmpleadosArea",
        type: "POST",
        data:
        {
            IdMovimientoPersonal: IdSolicitud,
        },
        success: function (resultado) {
            $("#Aprobar-" + IdSolicitud).prop("hidden", false);
            $("#AprobarCargando-" + IdSolicitud).prop("hidden", true);
            //CerrarModalCargando();
            //console.log(resultado);
            if (resultado.Codigo == '999') {
                MensajeError(resultado.Descripcion, false)
            } else {
                MensajeCorrecto(resultado.Descripcion, false);
            }
            
            $.ajax({
                url: "../MoverPersonal/PartialBandejaMovimientoPersonalRRHH",
                type: "POST",
                success: function (resultado) {
                    //$("#spinnerCargando").prop("hidden", true);
                    //CerrarModalCargando();
                    //console.log(resultado);
                    $('#divPartialBandeja').empty();
                    $('#divPartialBandeja').html(resultado);
                    if ($('#Nregistros').val() == '0') {
                        $('#MensajeRegistros').show();
                        $('#btnAprobar').hide();
                    } else {
                        $('#btnAprobar').show();
                        $('#MensajeRegistros').hide();
                    }
                },
                error: function (resultado) {
                    //$("#spinnerCargando").prop("hidden", true);
                    MensajeError(resultado, false);
                }
            });
        },
        error: function (resultado) {
            $("#Aprobar-" + IdSolicitud).prop("hidden", false);
            $("#AprobarCargando-" + IdSolicitud).prop("hidden", true);
            MensajeError(resultado, false);
        }
    });
}
function CerrarModalMensajeMP() {
    $("#ModalMensajeMP").modal("hide");
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../MoverPersonal/PartialBandejaMovimientoPersonalRRHH",
        type: "POST",
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            //CerrarModalCargando();
            //console.log(resultado);
            $('#divPartialBandeja').empty();
            $('#divPartialBandeja').html(resultado);
            //console.log($('#Nregistros').val())
            if ($('#Nregistros').val() == '0') {
                $('#MensajeRegistros').show();
                $('#btnAprobar').hide();
            } else {
                $('#MensajeRegistros').hide();
                $('#btnAprobar').show();
            }
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado, false);
        }
    });
}
function AprobarMas() {
    var result = new Array();
    i = 0;
    $("#bodydatos input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        this.id = id.replace('Empleado-', '');
        result.push(this.id);
        i++;
    });
    if (result.length == 0) {
        MensajeAdvertencia("Debe seleccionar al menos un empleado", false);
        return false;
    }
    $("#btnAprobar").prop("hidden", true);
    $("#btnAprobarEspera").prop("hidden", false);
    $.ajax({
        url: '../Nomina/ActualizaEmpleadosAreaMas',
        type: 'POST',
        dataType: "json",
        data: {
            IdMovimientoPersonal: result
        },
        success: function (resultado) {
            $("#btnAprobar").prop("hidden", false);
            $("#btnAprobarEspera").prop("hidden", true);
            //MensajeCorrectoTiempo(resultado, true, 10000);
            //$('#BodyMensajeCp').html(resultado);
            //$('#ModalMensajeCP').modal('show');
            console.log(resultado);
            $.each(resultado, function (index, value) {
                $('#tablamensaje').find('tbody').append("<tr><td>"+value.cedula+": </td><td>"+value.Descripcion+"</td></tr>");
            });
            $("#ModalMensajeMP").modal("show");

        }
        ,
        error: function (resultado) {
            $("#btnAprobar").prop("hidden", false);
            $("#btnAprobarEspera").prop("hidden", true);
            //MensajeError("No se pudieron mover", false);
            MensajeError(resultado, false);
        }
    });
}

function PreguntaEliminar(id) {
    $('#ideliminar').val(id);
    $("#Inactivar-" + id).prop("hidden", true);
    $("#InactivarCargando-" + id).prop("hidden", false);
    //console.log(id);
    $('#regELiminar').val(id);
    //console.log($('#regELiminar').val());
    $("#ModalMensajeMPAnular").modal("show");
}
function CerrarModalEliminaReg() {

    $("#Inactivar-" + $('#ideliminar').val()).prop("hidden", false);
    $("#InactivarCargando-" + $('#ideliminar').val()).prop("hidden", true);
}
function EliminarRegistroMP() {
    $.ajax({
        url: '../MoverPersonal/InactivarRegistro',
        type: 'POST',
        dataType: "json",
        data: {
            IdMovimientoPersonal: $('#regELiminar').val()
        },
        success: function (resultado) {
            $("#Inactivar-" + $('#ideliminar').val()).prop("hidden", false);
            $("#InactivarCargando-" + $('#ideliminar').val()).prop("hidden", true);
            //MensajeCorrectoTiempo(resultado, true, 10000);
            //$('#BodyMensajeCp').html(resultado);
            //$('#ModalMensajeCP').modal('show');
            //console.log(resultado);
            //$.each(resultado, function (index, value) {
            //    $('#tablamensaje').find('tbody').append("<tr><td>" + value.cedula + ": </td><td>" + value.Descripcion + "</td></tr>");
            //});
            //$("#ModalMensajeMP").modal("show");

            MensajeCorrecto(resultado,false);
            $.ajax({
                url: "../MoverPersonal/PartialBandejaMovimientoPersonalRRHH",
                type: "POST",
                success: function (resultado) {
                    $("#spinnerCargando").prop("hidden", true);
                    //CerrarModalCargando();
                    //console.log(resultado);
                    $('#divPartialBandeja').empty();
                    $('#divPartialBandeja').html(resultado);
                    //console.log($('#Nregistros').val())
                    if ($('#Nregistros').val() == '0') {
                        $('#MensajeRegistros').show();
                        $('#btnAprobar').hide();
                    } else {
                        $('#btnAprobar').show();
                        $('#MensajeRegistros').hide();
                    }
                },
                error: function (resultado) {
                    $("#spinnerCargando").prop("hidden", true);
                    MensajeError(resultado, false);
                }
            });

        }
        ,
        error: function (resultado) {
            $("#Inactivar-" + $('#ideliminar').val()).prop("hidden", false);
            $("#InactivarCargando-" + $('#ideliminar').val()).prop("hidden", true);
            //MensajeError("No se pudieron mover", false);
            MensajeError(resultado, false);
        }
    });
}
function SeleccionarTodos(){
   
    if ($('#select_all').prop('checked')) {
        $("input[type='checkbox']").attr("checked", true);
    } else {
        $("input[type='checkbox']").attr("checked", false);
    }
}