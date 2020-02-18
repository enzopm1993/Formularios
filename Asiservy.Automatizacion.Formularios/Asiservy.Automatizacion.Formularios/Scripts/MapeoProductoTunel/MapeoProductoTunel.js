$(document).ready(function () {
    CargarMapeoProductoTunel();
    CargarOrdenFabricacion();
    $('#SelectTextura').select2();
    $('#selectEspecie').select2();
    
});

$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});

$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").val() == '') {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#txtOrdenFabricacion").val($("#SelectOrdenFabricacion").val());
   CargarLotes($("#txtOrdenFabricacion").val());
    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);
});

$("#modal-orden-no").on("click", function () {
    $("#ModalOrdenes").modal('hide');
});


function CargarOrdenFabricacion() {
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    valor = $("#txtFechaOrden").val();
    if (valor == '' || valor == null)
        return;

    $.ajax({
        url: "../General/ConsultaSoloOFNivel3",
        type: "GET",
        data: {
            Fecha: valor            
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("No existen ordenes para esa fecha");
                return;
            }
            // LimpiarDetalle();
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.Orden + "'>" + row.Orden + "</option>")
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


function CargarLotes2(valor,lote) {
     $("#SelectLote2").empty();
    $("#SelectLote2").append("<option value='0' >-- Seleccionar Opción--</option>");
    if (valor == '') {
        return;
    }
    $.ajax({
        url: "../General/ConsultarLotesPorOf",
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
                    $("#SelectLote2").append("<option value='" + row.descripcion + "'>" + row.descripcion + "</option>")
                });
            }

            $("#SelectLote2").val(lote);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarLotes(valor) {
    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>");
    $("#SelectLote2").empty();
    $("#SelectLote2").append("<option value='0' >-- Seleccionar Opción--</option>");
    if (valor == '') {
        return;
    }
    $.ajax({
        url: "../General/ConsultarLotesPorOf",
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
                    $("#SelectLote2").append("<option value='" + row.descripcion + "'>" + row.descripcion + "</option>")
                });
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarMapeoProductoTunel() { 
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
  
  //  CargarOrdenFabricacion();
    $.ajax({
        url: "../MapeoProductoTunel/MapeoProductoTunelPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()      
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
          //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function NuevoControl() {
    $("#txtOrdenFabricacion").val('');
    $("#txtObservacion").val('');    
    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>");
    $("#SelectTipoLimpieza").prop("SelectedIndex", 0);
    $("#txtIdControl").val("0");
   
}

function Validar() {
    var valida = true;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }
    if ($("#SelectLote").val() == "") {
        $("#SelectLote").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#SelectLote").css('borderColor', '#ced4da');
    }
    if ($("#SelectTipoLimpieza").val() == "") {
        $("#SelectTipoLimpieza").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#SelectTipoLimpieza").css('borderColor', '#ced4da');
    }
    return valida;
}

function GenerarControl() {
    if (!Validar()) {
        return;
    }
    $.ajax({
        url: "../MapeoProductoTunel/MapeoProductoTunel",
        type: "POST",
        data: {
            IdMapeoProductoTunel: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            Lote: $("#SelectLote").val(),
            TipoLimpieza: $("#SelectTipoLimpieza").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("Problemas con los datos del Lote");
                return;
            }            
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                NuevoControl();
                CargarMapeoProductoTunel();
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });

    //alert("generado");
}



function ValidarEdicion() {
    var valida = true;
  
    if ($("#SelectLote2").val() == "") {
        $("#SelectLote2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#SelectLote2").css('borderColor', '#ced4da');
    }
    if ($("#SelectTipoLimpieza2").val() == "") {
        $("#SelectTipoLimpieza2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#SelectTipoLimpieza2").css('borderColor', '#ced4da');
    }
    return valida;
}

function EditarControl() {
    if (!ValidarEdicion()) {
        return;
    }
    $("#ModalControl").modal("hide");
    $.ajax({
        url: "../MapeoProductoTunel/MapeoProductoTunel",
        type: "POST",
        data: {
            IdMapeoProductoTunel: $("#txtIdControl").val(),            
            Lote: $("#SelectLote2").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            TipoLimpieza: $("#SelectTipoLimpieza2").val(),
            Observacion: $("#txtObservacionModal").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("Problemas con los datos del Lote");
                return;
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                MensajeCorrecto(resultado)
                $("#SelectLote").val($("#SelectLote2").val());
                $("#SelectTipoLimpieza").val($("#SelectTipoLimpieza2").val());
                $("#SelectLote").empty();
                $("#SelectLote").append("<option value='" + $("#SelectLote2").val() + "'>" + $("#SelectLote2").val() + "</option>")
                
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });

    //alert("generado");
}



function SeleccionarControl(model) {
    //console.log(model);
    CargarLotes2(model.OrdenFabricacion, model.Lote);
    $("#divCabecera2").prop("hidden", true);
    $("#divDetalle").prop("hidden", false);
    $("#btnEliminar").prop("hidden", false);
    $("#btnEditar").prop("hidden", false);
    $("#btnAtras").prop("hidden", false);
    $("#btnGenerar").prop("hidden", true);

    $("#txtFecha").prop("disabled", true);
    $("#txtOrdenFabricacion").prop("disabled", true);
    $("#btnOrden").prop("disabled", true);   
    $("#SelectLote").prop("disabled", true);   
    $("#SelectTipoLimpieza").prop("disabled", true);   
    $("#txtObservacion").prop("disabled", true);       

    $("#txtOrdenFabricacion").val(model.OrdenFabricacion);
    $("#txtObservacion").val(model.Observacion);
    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='" + model.Lote + "'>" + model.Lote + "</option>")
    $("#SelectTipoLimpieza").val(model.CodTipoLimpieza);
    $("#txtIdControl").val(model.IdMapeoProductoTunel);   
    $("#SelectTipoLimpieza2").val(model.CodTipoLimpieza); 
    $("#txtObservacionModal").val(model.Observacion);     
    CargarMapeoProductoTunelDetalle();
}

function AtrasControlPrincipal() {
    $("#divCabecera2").prop("hidden", false);
    $("#divDetalle").prop("hidden", true);
    $("#btnEliminar").prop("hidden", true);
    $("#btnEditar").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#btnGenerar").prop("hidden", false);


    $("#txtFecha").prop("disabled", false);
    $("#txtOrdenFabricacion").prop("disabled", false);
    $("#btnOrden").prop("disabled", false);
    $("#SelectLote").prop("disabled", false);
    $("#SelectTipoLimpieza").prop("disabled", false);
    $("#txtObservacion").prop("disabled", false);
    CargarMapeoProductoTunel();
    NuevoControl();
}

function ModalGenerar() {
    $("#ModalControl").modal("show");
}

function EliminarControl() {
    $("#txtIdControl").val($("#txtIdControl").val());
    $("#pModalControl").html("Lote: " + $("#SelectLote").val());
    $("#modalEliminarControl").modal('show');
}


$("#modal-btn-si").on("click", function () {
    InactivarControl();
    $("#txtIdControl").val('0');
    $("#modalEliminarControl").modal('hide');
});

$("#modal-btn-no").on("click", function () {
    $("#txtIdControl").val('0');
    $("#modalEliminarControl").modal('hide');
});

function InactivarControl() {
    $.ajax({
        url: "../MapeoProductoTunel/EliminarMapeoProductoTunel",
        type: "POST",
        data: {
            IdMapeoProductoTunel: $("#txtIdControl").val(),
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                MensajeCorrecto(resultado);
                AtrasControlPrincipal();
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

/////////////////////////////////////////// DETALLE /////////////////////////////////////////////////////////////

function ModalGenerarDetalle() {
    NuevoDetalle();
    $("#ModalDetalle").modal("show");
}


function CargarMapeoProductoTunelDetalle() {
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $("#chartDetalle").html('');
    $.ajax({
        url: "../MapeoProductoTunel/MapeoProductoTunelDetallePartial",
        type: "GET",
        data: {
            IdControl: $("#txtIdControl").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargandoDetalle").prop("hidden", true);
            if (resultado == "0") {
                $("#chartDetalle").html("No existen registros");
            } else {
                $("#chartDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //$('#tblDataTable2').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}



function ValidarDetalle() {
    var valida = true;
    if ($("#SelectTextura").val() == "") {
        $("#SelectTextura").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#SelectTextura").css('borderColor', '#ced4da');
    }
    if ($("#txtTunel").val() == "") {
        $("#txtTunel").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTunel").css('borderColor', '#ced4da');
    }
    if ($("#txtCoche").val() == "") {
        $("#txtCoche").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCoche").css('borderColor', '#ced4da');
    }
    if ($("#txtProducto").val() == "") {
        $("#txtProducto").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtProducto").css('borderColor', '#ced4da');
    }
    if ($("#txtFundas").val() == "") {
        $("#txtFundas").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFundas").css('borderColor', '#ced4da');
    }
    if ($("#txtHoraInicio").val() == "") {
        $("#txtHoraInicio").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraInicio").css('borderColor', '#ced4da');
    }
    if ($("#txtHoraFin").val() == "") {
        $("#txtHoraFin").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraFin").css('borderColor', '#ced4da');
    }
    if ($("#selectEspecie").val() == "") {
        $("#selectEspecie").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectEspecie").css('borderColor', '#ced4da');
    }
    return valida;
}

function GenerarDetalle() {
    if (!ValidarDetalle()) {
        return;
    }
    $.ajax({
        url: "../MapeoProductoTunel/MapeoProductoTunelDetalle",
        type: "POST",
        data: {
           IdMapeoProductoTunel:$("#txtIdControl").val(),
            IdMapeoProductoTunelDetalle: $("#txtIdDetalle").val(),
            Tunel: $("#txtTunel").val(),
            Coche: $("#txtCoche").val(),
            Producto: $("#txtProducto").val(),
            Especie: $("#selectEspecie").val(),
            Fundas: $("#txtFundas").val(),
            HoraInicio: $("#txtHoraInicio").val(),
            HoraFin: $("#txtHoraFin").val(),
            HoraFinLote: $("#txtHoraFinLote").val(),
            TotalFunda: $("#txtTotalFundas").val(),
            Textura: $("#SelectTextura").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                MensajeCorrecto(resultado);
                CargarMapeoProductoTunelDetalle();
                $("#ModalDetalle").modal("hide");
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);          
        }
    });
}

function NuevoDetalle() {
    $("#txtIdDetalle").val("0");
    $("#txtTunel").val("");
    $("#txtCoche").val("");
    $("#txtProducto").val("");
    $("#selectEspecie").prop("selectedIndex", 0).change();
    $("#txtFundas").val("");
    $("#txtHoraInicio").val("");
    $("#txtHoraFin").val("");
    $("#txtHoraFinLote").val("");
    $("#txtTotalFundas").val("");
    $("#SelectTextura").prop('selectedIndex', 0).change();
}

function EditarDetalle(model) {
  //  console.log(model);   
    $("#txtIdDetalle").val(model.IdMapeoProductoTunelDetalle);
    $("#txtTunel").val(model.Tunel);
    $("#txtCoche").val(model.Coche);
    $("#txtProducto").val(model.Producto);
    $("#selectEspecie").val(model.Especie).trigger('change');
    $("#txtFundas").val(model.Fundas);
    $("#txtHoraInicio").val(model.HoraInicio);
    $("#txtHoraFin").val(model.HoraFin);
    $("#txtHoraFinLote").val(model.HoraFinLote);
    $("#txtTotalFundas").val(model.TotalFunda);
    $("#ModalDetalle").modal("show");
    $('#SelectTextura').val(model.Textura).trigger('change');    
}

function EliminarDetalle(model) {
    $("#txtIdDetalle").val(model.IdMapeoProductoTunelDetalle);
    $("#pModalDaniado").html("Producto: " + model.Producto);
    $("#modalEliminarDetalle").modal('show');
}


$("#modal-Detalle-btn-si").on("click", function () {
    InactivarDetalle();
    $("#txtIdDetalle").val('0');
    $("#modalEliminarDetalle").modal('hide');
});

$("#modal-Detalle-btn-no").on("click", function () {
    $("#txtIdDetalle").val('0');
    $("#modalEliminarDetalle").modal('hide');
});

function InactivarDetalle() {
    $.ajax({
        url: "../MapeoProductoTunel/EliminarMapeoProductoTunelDetalle",
        type: "POST",
        data: {          
            IdMapeoProductoTunelDetalle: $("#txtIdDetalle").val(),          
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                MensajeCorrecto(resultado);
                CargarMapeoProductoTunelDetalle();              
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);         
        }
    });
}