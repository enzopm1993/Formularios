
var ListadoControl = [];
$(document).ready(function () {
    CargarProductoTerminado();
    $("#selectMaterial").select2({
        with:'100%'
    }); 

    $('#txtPersonal').inputmask({
        'alias': 'integer',
        'groupSeparator': '',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '100',
        'min': '0'
    });

    $('#txtRecibido').inputmask({
        'alias': 'integer',
        'groupSeparator': '',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '100000',
        'min': '0'
    });

    $('#txtDesechado').inputmask({
        'alias': 'integer',
        'groupSeparator': '',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '100000',
        'min': '0'

    });

    $('#txtUsado').inputmask({
        'alias': 'integer',
        'groupSeparator': '',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '100000',
        'min': '0'

    });
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

            $("#txtEtiqueta").val(parseInt(resultado.NOMBRE_ETIQUETA));
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


function ConsultaMateriales() {
    $.ajax({
        url: "../EntregaProductoTerminado/ConsultarMateriales",
        type: "GET",
        data: {
            OF: ListadoControl.OrdenFabricacion
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para esta OF.");
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros.");
            } else {
                //console.log(resultado);
                $("#selectMaterial").empty();
                $("#selectMaterial").append('<option value=""> Seleccione</option>');
                $.each(resultado, function (key, r) {
                    $("#selectMaterial").append('<option value=' + r.Codigo + '>' + r.Descripcion + '</option>');
                });
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
                MensajeAdvertencia("No existe ordenes de fabricación para esta fecha");
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
    $("#btnBodegas").prop("hidden", true);
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
            Etiqueta: $("#txtEtiqueta").val(),            
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                return;
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
                return;
            }else if (resultado == "1") {
                MensajeAdvertencia("No se encontró información de la OF")
                return;

            } else if ($("#txtIdEntregaProductoTerminado").val() == '0') {
                $("#chartCabecera2").html('');
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
    $("#txtCodigoMaterial").val('');   
    $("#txtProducto").val('');   
    $("#txtFechaVencimiento").val('');   
}


function SeleccionarControlEntregaProductoTerminado(model) {
 //  console.log(model);
    ListadoControl = model;
    $("#txtIdEntregaProductoTerminado").val(ListadoControl.IdProductoTerminado);
    $("#txtOrdenFabricacion").empty();
    $("#txtOrdenFabricacion").append("<option value='" + ListadoControl.OrdenFabricacion + "'>" + ListadoControl.OrdenFabricacion + "</option>")
 //   $("#txtOrdenFabricacion").val(ListadoControl.OrdenFabricacion);
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
    $("#btnBodegas").prop("hidden", false);

    $("#btnModalEliminar").prop("hidden", false);
    $("#btnModalGenerar").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", false);
    $("#txtFecha").prop("disabled", true);
    $("#txtFechaPaletizado").prop("disabled", true);
   // $("#selectTurno").prop("disabled", true);
    $("#divCabecera2").prop("hidden", true);
    $("#divDetalleProceso").prop("hidden", false);
    //console.log(model);
    $("#txtFecha").val(moment(model.FechaProduccion).format("YYYY-MM-DD"));

    ConsultaMateriales();
    CargarProcesoDetalleMaterial();
    CargarEntregaProductoTerminadoDetalle();
    CargarProcesoDetalleTiemposMuertos();
    CargarProcesoDetalleDaniado();
}



function AtrasControlPrincipal() {
    $("#btnModalGenerar").prop("hidden", false);
    $("#txtFecha").prop("disabled", false);
    $("#txtFechaPaletizado").prop("disabled", false);

  //  $("#selectTurno").prop("disabled", false);
    $("#btnAtras").prop("hidden", true);
    $("#btnBodegas").prop("hidden", true);
    
    $("#btnModalEliminar").prop("hidden", true);

    $("#btnModalEditar").prop("hidden", true);
    $("#divCabecera2").prop("hidden", false);
    $("#divDetalleProceso").prop("hidden", true);
    ListadoControl = [];
    NuevaEntrega();
    CargarProductoTerminado();
}


function InactivarControl() {
    $.ajax({
        url: "../EntregaProductoTerminado/EliminarEntregaProductoTerminado",
        type: "POST",
        data: {
            IdProductoTerminado: $("#txtEliminarControl").val(),
            FechaPaletizado: ListadoControl.FechaPaletizado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                return;
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
                return;

            }else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            AtrasControlPrincipal();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}


function EliminarControl() {
    $("#txtEliminarControl").val(ListadoControl.IdProductoTerminado);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControl").modal('show');
}

$("#modal-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControl").modal('hide');
});

$("#modal-no").on("click", function () {
    $("#modalEliminarControl").modal('hide');
});

///////////////////////////////////////////////////////////BODEGAS////////////////////////////////////////////

function ModalBodegas() {
    MostrarModalCargando();
    $.ajax({
        url: "../EntregaProductoTerminado/ConsultarBodegas",
        type: "GET",
        data: {
            OF: ListadoControl.OrdenFabricacion
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para esta OF.");
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros.");
            } else {
                //console.log(resultado);
                $("#txtControlCalidad").val(resultado.UnidadesControlCalidad);
                $("#txtRechazadas").val(resultado.UnidadesRechazadas);
                $("#txtLatasSueltas").val(resultado.LataSueltas);
                $("#txtDefectos").val(resultado.UnidadesConDefecto);
                $("#txtEntregadas").val(resultado.CajasEntregadas);
                $("#ModalBodegas").modal("show");
            } 
            CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            CerrarModalCargando();
        }
    });

   

}


//////////////////////////////////////////////////////////CONSUMO DE MATERIALES //////////////////////////////
function CargarProcesoDetalleMaterial() {
    $("#spinnerCargandoMaterial").prop("hidden", false);
    $("#divTableMaterial").html('');
    $.ajax({
        url: "../EntregaProductoTerminado/ControlConsumoMaterialPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdProductoTerminado,
            OrdenFabricacion: ListadoControl.OrdenFabricacion
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableMaterial").html("No existen registros");
                $("#spinnerCargandoMaterial").prop("hidden", true);
            } else {
                $("#spinnerCargandoMaterial").prop("hidden", true);
                $("#divTableMaterial").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoMaterial").prop("hidden", true);
        }
    });
}

function ModalGenerarMaterial() {
    $("#txtIdConsumoMaterial").val(0);
    $("#selectMaterial").prop("selectedIndex", 0).change();   
    $("#txtUsado").val('');
    $("#txtDesechado").val('');
    $("#txtRecibido").val('');
    $("#ModalConsumoMaterial").modal("show");
}

function validarConsumoMaterial() {
    var valida = true;
    if ($("#selectMaterial").val() == "") {
        $("#selectMaterial").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });

        //$("#selectMaterial").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectMaterial").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });
       // $("#selectMaterial").css('borderColor', '#ced4da');
    }
    if ($("#txtRecibido").val() == "") {
        $("#txtRecibido").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtRecibido").css('borderColor', '#ced4da');
    }
    if ($("#txtDesechado").val() == "") {
        $("#txtDesechado").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtDesechado").css('borderColor', '#ced4da');
    }
    if ($("#txtUsado").val() == "") {
        $("#txtUsado").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtUsado").css('borderColor', '#ced4da');
    }

    return valida;
}

function GuardarConsumoMaterial() {
    if (!validarConsumoMaterial()) {
        return;
    }
    $.ajax({
        url: "../EntregaProductoTerminado/GuardarConsumoMaterial",
        type: "POST",
        data: {
            IdMateriales: $("#txtIdConsumoMaterial").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado, 
            CodigoMaterial: $("#selectMaterial").val(),
            Recibido: $("#txtRecibido").val(),
            Desechado: $("#txtDesechado").val(),
            Usado: $("#txtUsado").val(),
            FechaPaletizado: ListadoControl.FechaPaletizado

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }
            CargarProcesoDetalleMaterial();
            $("#ModalConsumoMaterial").modal("hide");
            //   MensajeCorrecto("Registro Guardado Correctamente");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#ModalConsumoMaterial").modal("hide");
        }
    });
}


function InactivarConsumoMaterial() {
    $.ajax({
        url: "../EntregaProductoTerminado/EliminarConsumoMaterial",
        type: "POST",
        data: {
            IdMateriales: $("#txtEliminarProcesoMaterial").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado, 
            FechaPaletizado: ListadoControl.FechaPaletizado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarProcesoDetalleMaterial();
            //  MensajeCorrecto("Registro Eliminado con Éxito");
            $("#modalEliminarProcesoDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConsumoMaterial(model) {
    $("#txtEliminarProcesoMaterial").val(model.IdMateriales);
    $("#pModalMaterial").html("Material: " + model.Material);
    $("#modalEliminarConsumoMaterial").modal('show');
}


$("#modal-Material-btn-si").on("click", function () {
    InactivarConsumoMaterial();
    $("#txtEliminarProcesoMaterial").val('0');
    $("#modalEliminarConsumoMaterial").modal('hide');
});

$("#modal-Material-btn-no").on("click", function () {
    $("#txtEliminarProcesoMaterial").val('0');
    $("#modalEliminarConsumoMaterial").modal('hide');
});


function EditarConsumoMaterial(model) {
    // console.log(model);
    // $("#txtEliminarProcesoMaterial").val(model.IdProductosMaterial);    
    $("#txtIdConsumoMaterial").val(model.IdMateriales);
    $("#selectMaterial").prop("selectedIndex", model.CodigoMaterial).change();;
    $("#txtUsado").val(model.Usado);
    $("#txtDesechado").val(model.Desechado);
    $("#txtRecibido").val(model.Recibido);
    $("#ModalConsumoMaterial").modal("show");
    //ModalGenerarControlDetalle();
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
    $("#txtPersonal").val('');
    $("#selectTurno").prop("selectedIndex",0);
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

    if (moment($("#txtHoraInicioDetalle").val()) > moment($("#txtHoraFinDetalle").val())) {
        MensajeAdvertencia("Fecha de inicio no puede ser mayor a la de reinicio.")
        return;
    }

    if (moment($("#txtHoraInicioDetalle").val()).format("YYYY-MM-DD") < moment(ListadoControl.FechaPaletizado).format("YYYY-MM-DD")) {
        MensajeAdvertencia("Fecha de inicio no puede ser menor a la de paletizado.")
        return;
    }

    if (moment($("#txtHoraFinDetalle").val()).format("YYYY-MM-DD") > moment(ListadoControl.FechaPaletizado).add(1, 'days').format("YYYY-MM-DD")) {
        MensajeAdvertencia("Fecha de fin no puede ser mayor a la de paletizado.")
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
            Turno: $("#selectTurno").val(),
            FechaPaletizado: ListadoControl.FechaPaletizado
        },
        success: function (resultado) {
            $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
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
    //console.log(model);
    $("#txtIdEntregaProductoTerminadoDetalle").val(model.IdProductoTerminadoDetalle);
    $("#txtHoraInicioDetalle").val(moment(model.HoraInicio).format("YYYY-MM-DDTHH:mm"));
    $("#txtHoraFinDetalle").val(model.HoraFin);
    $("#txtPersonal").val(model.Personal);
    $("#selectTurno").val(model.Turno);
    $("#ModalGenerarEntregaProductoTerminadoDetalle").modal("show");
    //ModalGenerarControlDetalle();
}

function InactivarControlConsumoDetalle() {
    $.ajax({
        url: "../EntregaProductoTerminado/EliminarEntregaProductoTerminadoDetalle",
        type: "POST",
        data: {
            IdProductoTerminadoDetalle: $("#txtEliminarDetalle").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado, 
            FechaPaletizado: ListadoControl.FechaPaletizado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            } else if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
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





////////CONSUMO DE DANIADOS //////////////////////////////
function CargarProcesoDetalleDaniado() {
    $("#spinnerCargandoDaniados").prop("hidden", false);
    $("#divTableDaniados").html('');
    $.ajax({
        url: "../EntregaProductoTerminado/ControlConsumoDaniadoPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdProductoTerminado           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDaniados").html("No existen registros");
                $("#spinnerCargandoDaniados").prop("hidden", true);
            } else {
                $("#spinnerCargandoDaniados").prop("hidden", true);
                $("#divTableDaniados").html(resultado);
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

function ModalGenerarDanadio() {
    $("#txtIdControlDetalleProceso").val(0);
    $("#selectDaniado").prop("selectedIndex", 0);
    $("#txtCantidad").val(0);  
    $("#ModalConsumoDaniado").modal("show");
}

function validarConsumoDaniado() {
    var valida = true;
    if ($("#selectDaniado").val() == "") {
        $("#selectDaniado").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectDaniado").css('borderColor', '#ced4da');
    }
    if ($("#txtCantidad").val() == "") {
        $("#txtCantidad").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCantidad").css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarConsumoDaniado() {
    if (!validarConsumoDaniado()) {
        return;
    }
    $.ajax({
        url: "../EntregaProductoTerminado/GuardarConsumoDaniado",
        type: "POST",
        data: {
            IdProductosDaniados: $("#txtIdControlDetalleProceso").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado,
            Codigo: $("#selectDaniado").val(),
            Cantidad: $("#txtCantidad").val(),
            FechaPaletizado: ListadoControl.FechaPaletizado
            
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }
            CargarProcesoDetalleDaniado();
            $("#ModalConsumoDaniado").modal("hide");
            //   MensajeCorrecto("Registro Guardado Correctamente");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#ModalConsumoDaniado").modal("hide");
        }
    });
}


function InactivarConsumoDaniado() {
    $.ajax({
        url: "../EntregaProductoTerminado/EliminarConsumoDaniado",
        type: "POST",
        data: {
            IdProductosDaniados: $("#txtEliminarProcesoDaniado").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado, 
            FechaPaletizado: ListadoControl.FechaPaletizado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarProcesoDetalleDaniado();
            //  MensajeCorrecto("Registro Eliminado con Éxito");
            $("#modalEliminarProcesoDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConsumoDaniado(model) {
    $("#txtEliminarProcesoDaniado").val(model.IdProductosDaniados);
    $("#pModalDaniado").html("Merma: " + model.Merma);
    $("#modalEliminarConsumoDaniado").modal('show');
}


$("#modal-Daniado-btn-si").on("click", function () {
    InactivarConsumoDaniado();
    $("#txtEliminarProcesoDaniado").val('0');
    $("#modalEliminarConsumoDaniado").modal('hide');
});

$("#modal-Daniado-btn-no").on("click", function () {
    $("#txtEliminarProcesoDaniado").val('0');
    $("#modalEliminarConsumoDaniado").modal('hide');
});


function EditarConsumoDaniado(model) {
    // console.log(model);
   // $("#txtEliminarProcesoDaniado").val(model.IdProductosDaniados);    
    $("#txtIdConsumoDaniado").val(model.IdProductosDaniados);
    $("#selectDaniado").val(model.Codigo);
    $("#txtCantidad").val(model.Cantidad);  
    $("#ModalConsumoDaniado").modal("show");
    //ModalGenerarControlDetalle();
}





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

    if (moment($("#txtHoraPara").val()) > moment($("#txtHoraReinicio").val())) {
        MensajeAdvertencia("Fecha de para no puede ser mayor a la de reinicio.")
        return;
    }

    if (moment($("#txtHoraPara").val()).format("YYYY-MM-DD") < moment(ListadoControl.FechaPaletizado).format("YYYY-MM-DD")) {
        MensajeAdvertencia("Fecha de para no puede ser menor a la de paletizado.")
        return;
    }

    if (moment($("#txtHoraReinicio").val()).format("YYYY-MM-DD") > moment(ListadoControl.FechaPaletizado).add(1,'days').format("YYYY-MM-DD")) {
        MensajeAdvertencia("Fecha de reinicio no puede ser mayor a la de paletizado.")
        return;
    }


    $.ajax({
        url: "../EntregaProductoTerminado/GuardarTiemposMuertos",
        type: "POST",
        data: {
            IdTiempoParada: $("#txtIdConsumoTiemposMuertos").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado,
            FechaPaletizado: ListadoControl.FechaPaletizado,
       //     Tipo: $("#selectTipoTiemposMuertos").val(),
            HoraInicio: $("#txtHoraPara").val(),
            HoraFin: $("#txtHoraReinicio").val(),
            Causa: $("#txtObservacionTiemposMuertos").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }else if (resultado == "0") {
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
            IdTiempoParada: $("#txtEliminarTiemposMuertos").val(),
            IdProductoTerminado: ListadoControl.IdProductoTerminado,
            FechaPaletizado: ListadoControl.FechaPaletizado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 2) {
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }else if (resultado == "0") {
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