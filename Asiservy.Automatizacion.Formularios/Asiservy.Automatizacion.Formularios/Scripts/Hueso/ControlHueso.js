
$(document).ready(function () {
    CargarControlHueso();
});

function CargarOrdenFabricacion(valor) {
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../Hueso/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
           
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.Orden + "'>" + row.Orden + "</option>")
                });
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}


function CargarLotes(valor) {
    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../Hueso/ConsultarLotesPorLinea",
        type: "GET",
        data: {
            Orden: valor
        },
        success: function (resultado) {

            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectLote").append("<option value='" + row.descripcion + "'>" + row.descripcion + "</option>")
                });
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
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
    $('#txtPiezas').prop("disabled", false);
    $('#SelectOrdenFabricacion').prop("disabled", false);
    $('#txtFechaProduccion').prop("disabled", false);

    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>");
    var fecha = new Date();
   // console.log(fecha);
    var dia = fecha.getDate();
    var mes = fecha.getMonth()+1;

    if (dia < 10)
        dia = "0" + dia;
    if (mes < 10)
        mes = "0" + mes;

    var fechaProduccion = fecha.getFullYear() + "-" + mes + "-" + dia;    
    $('#txtFechaProduccion').val(fechaProduccion);
    CargarOrdenFabricacion(fechaProduccion);
    CargarControlHueso();
    $("#btnGenerar").prop("hidden", false);

}

function SeleccionControlHueso(id, lote, orden, tipo, horainicio, horafin, observacion,piezas,fecha) {
    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='" + lote + "' >" + lote+"</option>");

    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='" + orden + "' >" + orden + "</option>");

   
    $('#txtFechaProduccion').val(fecha);
    $('#txtIdControlHueso').val(id);
    $('#txtHoraInicio').val(horainicio);
    $('#txtHoraFin').val(horafin);
    $('#SelectTipoControl').val(tipo);
    $('#txtObservacion').val(observacion);
  
    $('#SelectLote').prop("disabled", true);
    $('#SelectTipoControl').prop("disabled", true);
    $('#txtObservacion').prop("disabled", true);
    $('#txtHoraInicio').prop("disabled", true);
    $('#txtHoraFin').prop("disabled", true);
    $('#txtPiezas').prop("disabled", true);
    $('#SelectOrdenFabricacion').prop("disabled", true);
    $('#txtFechaProduccion').prop("disabled", true);

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
    $("#spinnerCargando").prop("hidden", false);
    var bitacora = $('#DivTableControlHueso');  
    bitacora.html('');
    $.ajax({
        url: "../Hueso/ControlHuesoPartial",
        type: "GET",
        data: {
            id: id         
        },
        success: function (resultado) {
            $("#btnGenerar").prop("hidden", true);
            var bitacora = $('#DivTableControlHueso');
            bitacora.html('');
            var bitacora = $('#DivTableControlHuesoDetalle');
            $("#spinnerCargando").prop("hidden", true);
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });

}
function CargarControlHueso() {
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableControlHueso').html('');
    $('#DivTableControlHuesoDetalle').html('');
    $.ajax({
        url: "../Hueso/ControlHuesoPartialCabecera",
        type: "GET",       
        success: function (resultado) {
            var bitacora = $('#DivTableControlHueso');
            $("#spinnerCargando").prop("hidden", true);
            bitacora.html(resultado);
            var bitacora = $('#DivTableControlHuesoDetalle');
            bitacora.html('');
            $('#btnNuevo').prop("disabled", false);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);

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
    $('#DivTableControlHueso').html('');
    $('#DivTableControlHuesoDetalle').html('');

    $('#spinnerCargando').prop("hidden", false);     
    $('#btnGenerar').prop("disabled", true);     
    $.ajax({
        url: "../Hueso/GenerarControlHueso",
        type: "GET",
        data: {
            Linea: $('#txtLinea').val(),
            Lote: lote,
            TipoControlHueso: tipoControl,
            HoraInicio: horaInicio,
            HoraFin: horaFin,
            Observacion: observacion+"",
            TotalPieza: $('#txtPiezas').val(),
            OrdenFabricacion: $('#SelectOrdenFabricacion').val(),
            Fecha: $('#txtFechaProduccion').val()
        },
        success: function (resultado) {
            if (resultado == 0) {
                MensajeAdvertencia("Ya se ha generado un control con esos parametros");
                $('#spinnerCargando').prop("hidden", true);
                CargarControlHueso();
                return;
            }
            if (tipoControl == 1 || tipoControl == 4)
                CargarControlHuesoDetalle(resultado);
            else
                NuevoControlHueso();
            $('#btnGenerar').prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);     

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnGenerar').prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);     


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
//            url: "../Hueso/ValidaControlHueso",
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
//                MensajeError(resultado.responseText, false);
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
    var txtMiga = '#Miga-' + detalle;
    var huesos = $(txtHueso).val();
    var miga = $(txtMiga).val();
    if ($(id).prop('checked')) {
        if (huesos > 0) {
            $(label).css("background", "#28B463");
            $(txtHueso).prop("readonly", true);
            $(txtMiga).prop("readonly", true);
            GuardarControlHueso(detalle, huesos, miga, id);
        } else {
            $(id).prop('checked', false);
            $(label).css("background", "#7b8a8b");
            MensajeAdvertencia("Ingrese una cantidad de huesos");
        }
    } else {
        $(label).css("background", "#7b8a8b");
        $(txtHueso).prop("readonly", false);
        $(txtMiga).prop("readonly", false);
        GuardarControlHueso(detalle, 0, 0, id);

    }
}


function GuardarControlHueso(detalle, hueso, miga, id) {
    
    label = "#labelCheck-" + detalle; var txtHueso = '#Huesos-' + detalle;
    var txtMiga = '#Miga-' + detalle;

        $.ajax({
            url: "../Hueso/GuardarControlHueso",
            type: "POST",
            data: {
                IdControlHuesoDetalle: detalle,
                CantidadHueso: hueso,
                diMiga: miga
            },
            success: function (resultado) {                

            },
            error: function (resultado) {

                MensajeError(resultado.responseText, false);
                $(txtHueso).prop("readonly", false);
                $(txtMiga).prop("readonly", false);
                $(id).prop('checked', false);
                $(label).css("background", "#7b8a8b");
            }
        });   

}

