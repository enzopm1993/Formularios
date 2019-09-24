
$(document).ready(function () {
    CargarControlHueso();
});

function SeleccionarTipoControl(valor) {
    console.log(valor);
    if (valor == "3") {
        $('#divPiezas').prop("hidden", false);
    } else
        $('#divPiezas').prop("hidden", true);

}
function NuevoControlHueso() {
    $('#btnNuevo').prop("disabled", true);
    $('#txtIdControlHueso').val(0);
    $('#txtHoraInicio').val(null);
    $('#txtHoraFin').val(null);
    $('#SelectTipoControl').val(1);
    $('#txtObservacion').val('');
    $('#SelectLote').prop("selectedIndes", 0);

    $('#SelectLote').prop("disabled", false);
    $('#SelectTipoControl').prop("disabled", false);
    $('#txtObservacion').prop("disabled", false);
    $('#txtHoraInicio').prop("disabled", false);
    $('#txtHoraFin').prop("disabled", false);
    $('#divPiezas').prop("hidden", true);
    $('#txtPiezas').val(0);
    CargarControlHueso();
}

function SeleccionControlHueso(id, lote, orden, tipo, horainicio, horafin, observacion,piezas) {
    $('#txtIdControlHueso').val(id);
    $('#txtHoraInicio').val(horainicio);
    $('#txtHoraFin').val(horafin);
    $('#SelectTipoControl').val(tipo);
    $('#txtObservacion').val(observacion);
    $('#SelectLote').val(lote);
    $('#SelectLote').prop("disabled", true);
    $('#SelectTipoControl').prop("disabled", true);
    $('#txtObservacion').prop("disabled", true);
    $('#txtHoraInicio').prop("disabled", true);
    $('#txtHoraFin').prop("disabled", true);

    if (tipo == 3) {
        $('#divPiezas').prop("hidden", false);
        $('#txtPiezas').val(piezas);
    } else {
        $('#divPiezas').prop("hidden", true);
        $('#txtPiezas').val(0);
    }

    if (tipo==1|| tipo ==4)
    CargarControlHuesoDetalle(id);
}

function CargarControlHuesoDetalle(id) {
  //  console.log(id);
   
    $.ajax({
        url: "../Empleado/ControlHuesoPartial",
        type: "GET",
        data: {
            id: id         
        },
        success: function (resultado) {
            var bitacora = $('#DivTableControlHueso');
            bitacora.html('');
            var bitacora = $('#DivTableControlHuesoDetalle');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
        }
    });

}
function CargarControlHueso() {
    $.ajax({
        url: "../Empleado/ControlHuesoPartialCabecera",
        type: "GET",       
        success: function (resultado) {
            var bitacora = $('#DivTableControlHueso');
            bitacora.html(resultado);
            var bitacora = $('#DivTableControlHuesoDetalle');
            bitacora.html('');
            $('#btnNuevo').prop("disabled", false);

        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
            $('#btnNuevo').prop("disabled", false);

        }
    });

}

function GenerarControlHueso() {  

    var horaInicio = $('#txtHoraInicio').val();
    var horaFin = $('#txtHoraFin').val();
    var tipoControl = $('#SelectTipoControl').val();
    var observacion = $('#txtObservacion').val();
    var lote = $('#SelectLote').val();
  
    if (lote=='0') {
        MensajeAdvertencia("Seleccione un lote");
        return;
    }
    if ($('#txtIdControlHueso').val() > 0) {
        MensajeAdvertencia("Ya se ha generado el control de hueso");
        return;
    }

    if (tipoControl == "3" && $('#txtPiezas').val() == 0) {
        MensajeAdvertencia("Total de piezas es requerido");
        return;
    }
    if (tipoControl == "2" && $('#txtObservacion').val() =='') {
        MensajeAdvertencia("La observacion es requerido");
        return;
    }

    if (horaInicio == '' || horaFin == '') {
        MensajeAdvertencia("Ingrese rango de horas");
        return;

    }   
    $('#btnGenerar').prop("disabled", true);     
    $.ajax({
        url: "../Empleado/GenerarControlHueso",
        type: "GET",
        data: {
            Linea: $('#txtLinea').val(),
            Lote: lote,
            TipoControlHueso: tipoControl,
            HoraInicio: horaInicio,
            HoraFin: horaFin,
            Observacion: observacion+"",
            TotalPieza: $('#txtPiezas').val()
        },
        success: function (resultado) {
            if (resultado == 0) {
                MensajeAdvertencia("Ya se ha generado un control con esos parametros");
                return;
            }
            if (TipoControlHueso == 1 || TipoControlHueso == 4)
                CargarControlHuesoDetalle(resultado);
            else
                NuevoControlHueso();
            $('#btnGenerar').prop("disabled", false);


        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
            $('#btnGenerar').prop("disabled", false);

        }
    });
       
    

   

}

//function ValidaControlHueso(hora) {
//    if (hora != 0) {
//        if (hora == "1") {
//            var fecha = new Date();
//            hora = fecha.toLocaleTimeString();
//         //   console.log(hora);
//        }
//        $.ajax({
//            url: "../Empleado/ValidaControlHueso",
//            type: "GET",
//            data: {
//                dsLinea: $('#txtLinea').val(),
//                Hora: hora
//            },
//            success: function (resultado) {
//                if (resultado == 0) {
//                    var bitacora = $('#DivTableControlHuesoDetalle');
//                    bitacora.html('');
//                    $('#btnGenerar').prop("hidden", false);
//                    $('#SelectLote').prop("disabled", false);

//                } else {
//                    CargarControlHuesoDetalle(resultado)
//                    $('#SelectLote').prop("selectedIndex", 0);
//                    $('#SelectLote').prop("disabled", true);
//                    $('#btnGenerar').prop("hidden", true);

//                }
//            },
//            error: function (resultado) {
//                MensajeError(resultado.responseJSON, false);
//            }
//        });

//    }
//}

//function seleccionLote(valor) {
//    if (valor != 0) {
//        SelectHora = $('#SelectHora').val();
//        if (SelectHora == 0) {
//            MensajeAdvertencia("Seleccione una Hora");
//            $('#SelectLote').prop("selectedIndex", 0);
//        } else {
//            ValidaControlHueso(SelectHora);
//        }

//    }

//}


function checkControlHueso(id, detalle) {
    id = "#" + id;
    label = "#labelCheck-" + detalle;
    var txtHueso = '#Huesos-' + detalle;
    var huesos = $(txtHueso).val();
    if ($(id).prop('checked')) {
        if (huesos > 0) {
            $(label).css("background", "#28B463");
            $(txtHueso).prop("readonly", true);
            GuardarControlHueso(detalle, huesos);
        } else {
            $(id).prop('checked', false);
            $(label).css("background", "#7b8a8b");
            MensajeAdvertencia("Ingrese una cantidad de huesos");
        }
    } else {
        $(label).css("background", "#7b8a8b");
        $(txtHueso).prop("readonly", false);
        GuardarControlHueso(detalle, 0);

    }
}


function GuardarControlHueso(detalle, hueso) {
        $.ajax({
            url: "../Empleado/GuardarControlHueso",
            type: "GET",
            data: {
                IdControlHuesoDetalle: detalle,
                CantidadHueso: hueso
            },
            success: function (resultado) {                

            },
            error: function (resultado) {
                MensajeError(resultado.responseJSON, false);
            }
        });   

}
