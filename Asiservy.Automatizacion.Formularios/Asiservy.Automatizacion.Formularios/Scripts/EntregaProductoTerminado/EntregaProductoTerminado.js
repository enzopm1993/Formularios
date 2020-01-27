
var ListadoControl = [];
$(document).ready(function () {
     CargarProductoTerminado();
    
});

function CargarDatosOrdenFabricacion() {

    if ($("#txtOrdenFabricacion").val() == "") {
        NuevaEntrega();
        return;
    }
    $.ajax({
        url: "../EntregaProductoTerminado/ConsultarDatosOrdenFabricacion",
        type: "GET",
        data: {
            Orden: $("#txtOrdenFabricacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            if (resultado == "1") {
                MensajeAdvertencia("No se pudo obtener información");
                return;
            }
            //console.log(resultado);
            
            $("#txtEtiqueta").val(parseInt(resultado.CODIGO_MATERIAL));
            $("#txtCodigoMaterial").val(parseInt(resultado.CODIGO_MATERIAL));
            $("#txtOrdenVenta").val(parseInt(resultado.PEDIDO_VENTA));
            if (resultado.CLIENTE_CORTO != '') {
                $("#txtCliente").val(resultado.CLIENTE_CORTO);
            } else {
                $("#txtCliente").val(resultado.CLIENTE);
            }
            if (resultado.NOMBRE_ADICIONAL != '') {
                $("#txtProducto").val(resultado.NOMBRE_ADICIONAL);
            } else {
                $("#txtProducto").val(resultado.NOMBRE_PRODUCTO);
            }
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });

}



function CargarOrdenFabricacion() {
    valor = $("#txtFecha").val();
    if (valor == '' || valor == null)
        return;
    $("#txtOrdenFabricacion").empty();
    $("#txtOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../EntregaProductoTerminado/ConsultaOrdenFabricacionPorFechaProductoTerminado",
        type: "GET",
        data: {
            Fecha: valor,
            Linea: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            // LimpiarDetalle();
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#txtOrdenFabricacion").append("<option value='" + row.ORDEN_FABRICACION + "'>" + row.ORDEN_FABRICACION + "</option>")
                });
                $('#validaFecha').prop("hidden", true);
            } else {
                $('#validaFecha').prop("hidden", false);
            }
            //CargarControlHueso();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}



function CargarProductoTerminado() {
    $("#divCabecera2").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", true);
    var txtFecha = $('#txtFecha').val();   
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }  
    if ($("#txtFechaPaletizado").val() == "") {
        $("#txtFechaPaletizado").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFechaPaletizado").css('borderColor', '#ced4da');
    }  
    $("#spinnerCargando").prop("hidden", false);
    $("#chartCabecera2").html('');
    CargarOrdenFabricacion();
    $.ajax({
        url: "../EntregaProductoTerminado/EntregaProductoTerminadoPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFechaPaletizado").val(),
            LineaNegocio: $("#txtLineaNegocio").val()
          
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function ModalGenerarControl(edit) {
    if (!edit) {
        var txtFecha = $('#txtFecha').val();      
        var txtFecha2 = $('#txtFechaPaletizado').val();      
        if (txtFecha == "") {
            MensajeAdvertencia("Igrese Fecha de Producción");
            return;
        }   
        if (txtFecha2 == "") {
            MensajeAdvertencia("Igrese Fecha de Paletizado");
            return;
        }   
    }
    $("#ModalGenerarControl").modal("show");
}



function ValidarGenerarEntregaProducto() {
    var valida = true;   

    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }
   
    if ($("#txtCodigoProducto").val() == "") {
        $("#txtCodigoProducto").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodigoProducto").css('borderColor', '#ced4da');
    }

    if ($("#txtFechaVencimiento").val() == "") {
        $("#txtFechaVencimiento").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFechaVencimiento").css('borderColor', '#ced4da');
    }

    return valida;
}

function GenerarControlConsumo() {
    var txtFecha = $('#txtFecha').val();
  
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#txtFechaPaletizado").val() == "") {
        $("#txtFechaPaletizado").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFechaPaletizado").css('borderColor', '#ced4da');
    }  

    if ($("#txtIdEntregaProductoTerminado").val() == '0') {
        $("#chartCabecera2").html('');
    }

    if (!ValidarGenerarEntregaProducto()) {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../EntregaProductoTerminado/EntregaProductoTerminado",
        type: "POST",
        data: {
            IdProductoTerminado: $("#txtIdEntregaProductoTerminado").val(),
            FechaProduccion: txtFecha,
            FechaPaletizado: $("#txtFechaPaletizado").val(),
            FechaVencimiento: $("#txtFechaVencimiento").val(),
           // CodigoSap: $("#txtOrdenFabricacion").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            //OrdenVenta: $("#txtPesoNeto").val(),
            //Producto: $("#txtPesoEscrundido").val(),
            CodigoProducto: $("#txtLomo").val(),
            LineaNegocio: $("#txtLineaNegocio"),
            //Cliente: $("#txtMiga").val(),
            //Etiqueta: $("#txtAceite").val(),            
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else if (resultado == "1") {
                MensajeAdvertencia("No se encontró información de la OF")
                return;

            } else if ($("#txtIdEntregaProductoTerminado").val() == '0') {
                CargarProductoTerminado();
                MensajeCorrecto("Registro Generado con Éxito");

            } else {

                MensajeCorrecto("Registro Actualizado con Éxito");
            }
            $("#ModalGenerarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}


function NuevaEntrega() {
    $("#txtIdEntregaProductoTerminado").val('0');
    $("#txtOrdenFabricacion").val('');
    $("#txtOrdenFabricacion").prop("disabled", false);
    $("#txtOrdenVenta").val('');
    $("#txtCliente").val('');
    $("#txtEtiqueta").val('');
    $("#txtCodigoProducto").val('');
    $("#txtObservacion").val('');   
}
