
$(document).ready(function () {
    CargarControlHueso();
 });


function Clear(id) {
    if ($("#" + id).val() == 0) {
        $("#" + id).val('')
    }
    
}

function CargarOrdenFabricacion(valor) {
    if (valor == '' || valor == null)
        return;
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../Hueso/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.Orden + "'>" + row.Orden + "</option>")
                });
            }
            CargarControlHueso();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}


function CargarLotes(valor) {
   

    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>");
    if (valor == '') {
        return;
    }
    $.ajax({
        url: "../Hueso/ConsultarLotesPorLinea",
        type: "GET",
        data: {
            Orden: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
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
    // console.log(valor);
    $('#divPiezas').prop("hidden", true);
    $('#divLimpiadoras').prop("hidden", true);

    if (valor == "3") {
        $('#divPiezas').prop("hidden", false);
        $('#divLimpiadoras').prop("hidden", false);
    }
    if (valor=="2")
    {
        $('#divLimpiadoras').prop("hidden", false);
    }


}
function NuevoControlHueso() {
    $('#btnNuevo').prop("disabled", true);
    $("#btnNuevo").prop("hidden", true);

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
    
    $('#selectLimpieza').val(0);

    $('#divLimpiadoras').prop("hidden", true);
    $('#divPiezas').prop("hidden", true);
    $('#txtPiezas').val(0);
    $('#txtPiezas').prop("disabled", false);
    $('#txtLimpiadoras').val(0);
    $('#txtLimpiadoras').prop("disabled", false);
    
    $('#SelectOrdenFabricacion').prop("disabled", false);
    $('#txtFechaProduccion').prop("disabled", false);

    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>"); 
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='0' >-- Seleccionar Opción--</option>"); 
    CargarOrdenFabricacion($('#txtFechaProduccion').val());
    CargarControlHueso();
    $("#btnGenerar").prop("hidden", false);
    $("#btnInactivar").prop("hidden", true);
    $("#btnEditar").prop("hidden", true);
    

    //$("#divCabecera").prop("hidden", false);
    $('#divCabecera').slideUp(300).fadeIn(1000);


}

function SeleccionControlHueso(model) {
  //  console.log(model);
    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='" + model.Lote + "' >" + model.Lote+"</option>");

    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='" + model.OrdenFabricacion + "' >" + model.OrdenFabricacion + "</option>");


    //$('#txtFechaProduccion').val(model.Fecha);
    $('#txtIdControlHueso').val(model.IdControlHueso);
    $('#txtHoraInicio').val(model.HoraInicio);
    $('#txtHoraFin').val(model.HoraFin);
    $('#SelectTipoControl').val(model.CodTipoControl);
    $('#txtObservacion').val(model.Observacion);
    $('#txtPiezas').val(model.TotalPieza);
    $('#txtLimpiadoras').val(model.TotalLimpiadoras);
    $('#selectLimpieza').val(model.Limpieza);
  
    $('#SelectLote').prop("disabled", true);
    $('#SelectTipoControl').prop("disabled", true);
    $('#txtObservacion').prop("disabled", true);
    $('#txtHoraInicio').prop("disabled", true);
    $('#txtHoraFin').prop("disabled", true);
    $('#txtPiezas').prop("disabled", true);
    $('#txtLimpiadoras').prop("disabled", true);
    $('#SelectOrdenFabricacion').prop("disabled", true);
    $('#txtFechaProduccion').prop("disabled", true);
    
    $('#divPiezas').prop("hidden", true);
    $('#divLimpiadoras').prop("hidden", true);

    if (model.CodTipoControl == "3") {
        $('#divPiezas').prop("hidden", false);
        $('#divLimpiadoras').prop("hidden", false);
    }
    if (model.CodTipoControl == "2") {
        $('#divLimpiadoras').prop("hidden", false);
    }

    $("#btnNuevo").prop("hidden", false);
    $("#btnGenerar").prop("hidden", true);
    $("#btnInactivar").prop("hidden", false);
    $("#btnEditar").prop("hidden", false);

  //  console.log(id);

    if (model.CodTipoControl == 1 || model.CodTipoControl ==4)
        CargarControlHuesoDetalle(model.IdControlHueso);

}

function CargarControlHuesoDetalle(id) {
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
            if (resultado == "101") {
                window.location.reload();
            }
            $('#divCabecera').slideUp(300).fadeOut(1000);  
            var bitacora = $('#DivTableControlHueso');
            bitacora.html('');
            var bitacora = $('#DivTableControlHuesoDetalle');
            $("#spinnerCargando").prop("hidden", true);
            bitacora.html(resultado);
            $("#btnNuevo").prop("hidden", false);
            $("#btnGenerar").prop("hidden", true);
            $("#btnInactivar").prop("hidden", false);
            $("#btnEditar").prop("hidden", false);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });

}
function CargarControlHueso() {
    //console.log($("#txtFechaProduccion").val());
    if ($("#txtFechaProduccion").val() == '' || $("#txtFechaProduccion").val() == null)
        return;
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableControlHueso').html('');
    $('#DivTableControlHuesoDetalle').html('');
    $.ajax({
        url: "../Hueso/ControlHuesoPartialCabecera",
        type: "GET",
        data: {
            Fecha: $("#txtFechaProduccion").val()
        },
    
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
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
    if (tipoControl == "3" && $('#txtLimpiadoras').val() == 0) {
        MensajeAdvertencia("Total de limpiadoras es requerido");
        return;
    }
    if (tipoControl == "2" && $('#txtObservacion').val() =='') {
        MensajeAdvertencia("La observacion es requerido");
        return;
    }
    if (tipoControl == "2" && $('#txtLimpiadoras').val() == 0) {
        MensajeAdvertencia("Total de limpiadoras es requerido");
        return;
    }
    if (horaInicio == '' || horaFin == '') {
        MensajeAdvertencia("Ingrese rango de horas");
        return;
    }
    if ($('#selectLimpieza').val() == '0') {
        MensajeAdvertencia("Seleccione un tipo de limpieza");
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
            TotalLimpiadoras: $('#txtLimpiadoras').val(),
            OrdenFabricacion: $('#SelectOrdenFabricacion').val(),
            Fecha: $('#txtFechaProduccion').val(),
            Limpieza: $('#selectLimpieza').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                MensajeAdvertencia("Ya se ha generado un control con esos parametros");
                $('#spinnerCargando').prop("hidden", true);
                $('#btnGenerar').prop("disabled", false);
                CargarControlHueso();
                return;
            }
            
            if (tipoControl == 1 || tipoControl == 4) {
                CargarControlHuesoDetalle(resultado);
                $("#txtIdControlHueso").val(resultado);
            }
            else { NuevoControlHueso(); }

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
function checkControlHueso(id, detalle) {
    id = "#" + id;
    label = "#labelCheck-" + detalle;
    var txtHueso = '#Huesos-' + detalle;
    var txtMiga = '#Miga-' + detalle;
    var huesos = $(txtHueso).val();
    var miga = $(txtMiga).val();
    if ($(id).prop('checked')) {
        if (huesos > 0) {
            if (miga == '') {                
               // $(txtMiga).val('0.00');  
                //miga = '0.00';
                MensajeAdvertencia2("El formato de la miga es incorrecto", detalle)
                $(id).focus();
                $(id).prop('checked', false);
                return
            }
            if (miga > 9.9) {
                MensajeAdvertencia2("Miga no puede ser mayor de 9.9kg", detalle)
                $(txtMiga).val('0.00');
                $(id).focus();
                $(id).prop('checked', false);
                return;
            }
            $(label).css("background", "#28B463");
            $(txtHueso).prop("readonly", true);
            $(txtMiga).prop("readonly", true);
            GuardarControlHueso(detalle, huesos, miga, id);
        } else {
            $(id).prop('checked', false);
           // $(label).css("background", "#7b8a8b");
            MensajeAdvertencia2("Ingrese una cantidad de huesos", detalle);
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
                if (resultado == "101") {
                    window.location.reload();
                }
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



function InactivarRegistro() {
    if ($("#txtIdControlHueso").val() < 1) {
        NuevoControlHueso();
        return;
    }
 
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableControlHueso').html('');
    $('#DivTableControlHuesoDetalle').html('');
    $.ajax({
        url: "../Hueso/InactivarControlHueso",
        type: "GET",
        data: {
            id: $("#txtIdControlHueso").val()
        },

        success: function (resultado) {            
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargando").prop("hidden", true);
            MensajeCorrecto(resultado);
            var bitacora = $('#DivTableControlHuesoDetalle');
            bitacora.html('');
            $('#btnNuevo').prop("disabled", false);
            $('#btnInactivar').prop("disabled", false);
            $("#btnEditar").prop("hidden", false);

            NuevoControlHueso();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);
            $('#btnNuevo').prop("disabled", false);
            $('#btnInactivar').prop("disabled", false);
            $("#btnEditar").prop("hidden", false);


          

        }
    });

}

var modalModificar = function (callback) {

    $("#btnEditar").on("click", function () {
        $('#txtIdControlModal').val($('#txtIdControlHueso').val());
        $('#txtHoraDesdeModal').val($('#txtHoraInicio').val());
        $('#txtHoraHastaModal').val($('#txtHoraFin').val());
        $('#txtLoteModal').val($('#SelectLote').val());
        $('#selectLimpiezaModal').val($('#selectLimpieza').val());
        $('#txtObservacionModal').val($('#txtObservacion').val());
        

        $("#ModalEditControl").modal('show');
    });

    $("#btnModificarModal").on("click", function () {
        callback(true);
        $("#ModalEditControl").modal('hide');
    });
    
};
modalModificar(function (confirm) {
    if (confirm) {
        //Acciones si el usuario confirma
        ModificarControlHueso();

    }
});


function ModificarControlHueso() {
    if ($("#txtIdControlModal").val() < 1) {
        MensajeAdvertencia("No se pudo modificar el control, faltan parametros.");
        return;
    }
    $.ajax({
        url: "../Hueso/ModificarControl",
        type: "GET",
        data: {
            IdControlHueso: $("#txtIdControlModal").val(),
            HoraInicio: $("#txtHoraDesdeModal").val(),
            HoraFin: $("#txtHoraHastaModal").val(),
            Limpieza: $("#selectLimpiezaModal").val(),
            Observacion: $("#txtObservacionModal").val()
        },

        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            if (resultado.Respuesta) {
                MensajeCorrecto(resultado.Mensaje);
            } else {
                MensajeAdvertencia(resultado.Mensaje);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}





var modalConfirm = function (callback) {

    $("#btnInactivar").on("click", function () {
        $("#mi-modal").modal('show');
    });

    $("#modal-btn-si").on("click", function () {
        callback(true);
        $("#mi-modal").modal('hide');
    });

    $("#modal-btn-no").on("click", function () {
        callback(false);
        $("#mi-modal").modal('hide');
    });
};

modalConfirm(function (confirm) {
    if (confirm) {
        //Acciones si el usuario confirma
        InactivarRegistro();

    }
});

function MensajeAdvertencia2(mensaje, id) {
    $('#trMensaje-' + id).prop('hidden',false);
    $('#trMensaje-' + id).show();
    $('#trMensaje-' + id).prop('display ', 'block');
    $('#divMensaje-' + id).html('');
    $('<div class="alert alert-warning">' +
        '<button type="button" class="close" data-dismiss="alert">' +
        '&times;</button><p id="pMensaje-' + id + '"></p></div>').hide().appendTo('#divMensaje-' + id).fadeIn(1000);
    $('#pMensaje-' + id).text(mensaje);
    $('#trMensaje-' + id).delay(2500).fadeOut(
        "normal",
        function () {
            //$(this).prop('hidden',true);
        });

}

function filterFloat(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}
function filter(__val__) {
    var preg = /^([0-9]+\.?[0-9]{0,2})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }
}

function SoloNumeros(evt,input) {
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    } 
}

