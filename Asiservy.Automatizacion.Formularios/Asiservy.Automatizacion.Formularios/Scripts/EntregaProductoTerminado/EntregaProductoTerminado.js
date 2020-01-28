
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
        url: "../EntregaProductoTerminado/ConsultarOrdenesFabricacion",
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
    if (!ValidarGenerarEntregaProducto()) {
        return;
    }
    if ($("#txtIdEntregaProductoTerminado").val() == '0') {
        $("#chartCabecera2").html('');
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
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            CodigoProducto: $("#txtCodigoProducto").val(),
            LineaNegocio: $("#txtLineaNegocio").val(),
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


function SeleccionarControlEntregaProductoTerminado(model) {
 //  console.log(model);
    ListadoControl = model;
    $("#txtIdEntregaProductoTerminado").val(ListadoControl.IdProductoTerminado);
    $("#txtOrdenFabricacion").val(ListadoControl.OrdenFabricacion);
    $("#txtOrdenFabricacion").prop("disabled", true);    
    $("#txtOrdenVenta").val(ListadoControl.OrdenVenta);
    $("#txtCliente").val(ListadoControl.Cliente);   
    $("#txtCodigoMaterial").val(ListadoControl.CodigoSap);   
    $("#txtEtiqueta").val(ListadoControl.Etiqueta);   
    $("#txtProducto").val(ListadoControl.Producto);
    $("#txtFechaVencimiento").val(moment(ListadoControl.FechaVencimiento).format("YYYY-MM-DD"));   
    
    $("#txtCodigoProducto").val(ListadoControl.CodigoProducto);
    $("#txtObservacion").val(ListadoControl.Observacion);
  
    //  $("#divCabecera1").prop("hidden", true);
    $("#btnAtras").prop("hidden", false);
    $("#btnModalEliminar").prop("hidden", false);
    $("#btnModalGenerar").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", false);
    $("#txtFecha").prop("disabled", true);
   // $("#selectTurno").prop("disabled", true);
    $("#divCabecera2").prop("hidden", true);
    $("#divDetalleProceso").prop("hidden", false);
    
    CargarEntregaProductoTerminadoDetalle();
    CargarProcesoDetalleTiemposMuertos();
}



function AtrasControlPrincipal() {
    $("#btnModalGenerar").prop("hidden", false);
    $("#txtFecha").prop("disabled", false);
  //  $("#selectTurno").prop("disabled", false);
    $("#btnAtras").prop("hidden", true);
    $("#btnModalEliminar").prop("hidden", true);

    $("#btnModalEditar").prop("hidden", true);
    $("#divCabecera2").prop("hidden", false);
    $("#divDetalleProceso").prop("hidden", true);
    ListadoControl = [];
    NuevaEntrega();
    CargarProductoTerminado();
}

//////////////////////////////// DETALLE HORAS ///////////////////////////////////////

function CargarEntregaProductoTerminadoDetalle() {
    $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", false);
    $("#divTableEntregaProductoDetalle").html('');
    $.ajax({
        url: "../EntregaProductoTerminado/EntregaProductoTerminadoDetallePartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdProductoTerminado
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableEntregaProductoDetalle").html("No existen registros");
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
                $("#divTableEntregaProductoDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
        }
    });
}

function ModalGenerarConsumoInsumoDetalle() {
    $("#txtIdEntregaProductoTerminadoDetalle").val(0);
    $("#txtHoraInicioDetalle").val("");
    $("#txtHoraFinDetalle").val("");
    $("#ModalGenerarEntregaProductoTerminadoDetalle").modal("show");

}

function ValidarGenerarControlConsumoDetalle() {
    var valida = true;
    if ($("#txtHoraInicioDetalle").val() == "") {
        $("#txtHoraInicioDetalle").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraInicioDetalle").css('borderColor', '#ced4da');
    }
    if ($("#txtHoraFinDetalle").val() == "") {
        $("#txtHoraFinDetalle").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraFinDetalle").css('borderColor', '#ced4da');
    }

    if ($("#txtPersonal").val() == "") {
        $("#txtPersonal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPersonal").css('borderColor', '#ced4da');
    }

    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }

    return valida;
}
function GenerarControlConsumoDetalle() {
    if (!ValidarGenerarControlConsumoDetalle()) {
        return;
    }
    $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", false);
    $.ajax({
        url: "../EntregaProductoTerminado/EntregaProductoTerminadoDetallePartial",
        type: "POST",
        data: {
            IdProductoTerminadoDetalle: $("#txtIdEntregaProductoTerminadoDetalle").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado,
            HoraInicio: $("#txtHoraInicioDetalle").val(),
            HoraFin: $("#txtHoraFinDetalle").val(),
            Personal: $("#txtPersonal").val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            }
            CargarEntregaProductoTerminadoDetalle();
            $("#ModalGenerarEntregaProductoTerminadoDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
        }
    });
}


function EditarConsumoInsumoDetalle(model) {
    console.log(model);
    $("#txtIdEntregaProductoTerminadoDetalle").val(model.IdProductoTerminadoDetalle);
    $("#txtHoraInicioDetalle").val(model.HoraInicio);
    $("#txtHoraFinDetalle").val(model.HoraFin);
    $("#ModalGenerarEntregaProductoTerminadoDetalle").modal("show");
    //ModalGenerarControlDetalle();
}

function InactivarControlConsumoDetalle() {
    $.ajax({
        url: "../EntregaProductoTerminado/EliminarEntregaProductoTerminadoDetalle",
        type: "POST",
        data: {
            IdProductoTerminadoDetalle: $("#txtEliminarDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarEntregaProductoTerminadoDetalle();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarProcesoConsumoInsumoDetalle(model) {
    $("#txtEliminarDetalle").val(model.IdProductoTerminadoDetalle);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarControlConsumoDetalle();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});







/////////////////////TIEMPOS MUERTOS////////////////////////////

function CargarProcesoDetalleTiemposMuertos() {
    $("#spinnerCargandoTiemposMuertos").prop("hidden", false);
    $("#divTableTiemposMuertos").html('');
    $.ajax({
        url: "../EntregaProductoTerminado/ControlConsumoTiemposMuertos",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdProductoTerminado
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableTiemposMuertos").html("No existen registros");
                $("#spinnerCargandoTiemposMuertos").prop("hidden", true);
            } else {
                $("#spinnerCargandoTiemposMuertos").prop("hidden", true);
                $("#divTableTiemposMuertos").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoDaniados").prop("hidden", true);
        }
    });
}

function ModalGenerarTiemposMuertos() {
    $("#txtIdConsumoTiemposMuertos").val(0);
   // $("#selectTipoTiemposMuertos").prop("selectedIndex", 0);
    $("#txtHoraPara").val("");
    $("#txtHoraReinicio").val("");
    $("#txtObservacionTiemposMuertos").val("");
    $("#ModalConsumoTiemposMuertos").modal("show");
}

function validarConsumoTiemposMuertos() {
    var valida = true;
    if ($("#txtHoraPara").val() == "") {
        $("#txtHoraPara").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraPara").css('borderColor', '#ced4da');
    }
    if ($("#txtHoraReinicio").val() == "") {
        $("#txtHoraReinicio").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraReinicio").css('borderColor', '#ced4da');
    }
   
    return valida;
}

function GuardarConsumoTiemposMuertos() {
    if (!validarConsumoTiemposMuertos()) {
        return;
    }
    $.ajax({
        url: "../EntregaProductoTerminado/GuardarTiemposMuertos",
        type: "POST",
        data: {
            IdTiempoParada: $("#txtIdConsumoTiemposMuertos").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado,
       //     Tipo: $("#selectTipoTiemposMuertos").val(),
            HoraInicio: $("#txtHoraPara").val(),
            HoraFin: $("#txtHoraReinicio").val(),
            Causa: $("#txtObservacionTiemposMuertos").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            CargarProcesoDetalleTiemposMuertos();
            $("#ModalConsumoTiemposMuertos").modal("hide");
            //   MensajeCorrecto("Registro Guardado Correctamente");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#ModalConsumoTiemposMuertos").modal("hide");
        }
    });
}


function InactivarTiemposMuertos() {
    $.ajax({
        url: "../EntregaProductoTerminado/EliminarTiemposMuertos",
        type: "POST",
        data: {
            IdTiempoParada: $("#txtEliminarTiemposMuertos").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarProcesoDetalleTiemposMuertos();
            //  MensajeCorrecto("Registro Eliminado con Éxito");
            $("#modalEliminarTiemposMuertos").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConsumoTiemposMuertos(model) {
    $("#txtEliminarTiemposMuertos").val(model.IdTiempoParada);
    $("#pModalTiemposMuertos").html("Hora de para: " + moment(model.HoraInicio).format("HH:mm"));
    $("#modalEliminarTiemposMuertos").modal('show');
}


$("#modal-TiemposMuertos-btn-si").on("click", function () {
    InactivarTiemposMuertos();
    $("#txtEliminarTiemposMuertos").val('0');
    $("#modalEliminarTiemposMuertos").modal('hide');
});

$("#modal-TiemposMuertos-btn-no").on("click", function () {
    $("#txtEliminarTiemposMuertos").val('0');
    $("#modalEliminarTiemposMuertos").modal('hide');
});


function EditarConsumoTiemposMuertos(model) {
    // console.log(model);
    $("#txtIdConsumoTiemposMuertos").val(model.IdTiempoParada);
    //   $("#selectTipoTiemposMuertos").val(model.CodTipo);
    $("#txtHoraPara").val(model.HoraInicio);
    $("#txtHoraReinicio").val(model.HoraFin);
    $("#txtObservacionTiemposMuertos").val(model.Causa);


    $("#ModalConsumoTiemposMuertos").modal("show");
    //ModalGenerarControlDetalle();
}