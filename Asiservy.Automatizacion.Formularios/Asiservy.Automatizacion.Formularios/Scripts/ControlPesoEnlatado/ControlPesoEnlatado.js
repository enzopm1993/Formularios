
$(document).ready(function () {
    //ValidaProyeccion();
    CargarControlPesoEnlatado();
});


function LimpiarDetalle() {
    $("#txtOrdenVenta").val('');
    $("#txtCodigoProducto").val('');
    $("#txtProducto").val('');
    $("#txtLineaNegocio").val('');
 


}

function CargarOrdenFabricacion() {
    valor = $("#txtFechaOrden").val();
    $('#txtOrdenFabricacion').val("");

    if (valor == '' || valor == null)
        return;
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../ControlPesoEnlatado/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            LimpiarDetalle();
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.ORDEN_FABRICACION + "'>" + row.ORDEN_FABRICACION + "</option>")
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


function CargarControlPesoEnlatado() {
    $("#DivTableControl").html('');
    $("#DivMensaje").html('');
    if ($("#txtFechaProduccion").val() == "") {
        $("#txtFechaProduccion").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFechaProduccion").css('borderColor', '#ced4da');
    }
    if ($("#txtTurno").val() == "") {
        $("#txtTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtTurno").css('borderColor', '#ced4da');
    }
    $("#txtFechaOrden").val($("#txtFechaProduccion").val());   
    

    var fecha = moment($("#txtFechaProduccion").val()).format("YYYY-MM-DDTHH:mm");
    $("#txtHoraDetalle").val(fecha);
    //$("#txtFechaFinDetalle").val(fecha);
  
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ControlPesoEnlatado/ControlPesoEnlatadoPartial",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaProduccion').val(),
            Turno: $("#txtTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarOrdenFabricacion();
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTableControl").html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                //config.opcionesDT.scrollX = "800px";
                //config.opcionesDT.scrollY = "800px";
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $("#spinnerCargando").prop("hidden", true);
            $("#DivCard").prop("hidden", false);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});

$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").val() == '') {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#txtOrdenFabricacion").val($("#SelectOrdenFabricacion").val());
    CargarOrdenFabricacionDetalle($("#SelectOrdenFabricacion").val());
    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);
});

$("#modal-orden-no").on("click", function () {
    $("#ModalOrdenes").modal('hide');
});


function CargarOrdenFabricacionDetalle(orden) {
    LimpiarDetalle();
    valor = $("#txtFechaProduccion").val();
    if (valor == '' || valor == null)
        return;
    if ($("#txtOrdenFabricacion").val() == '')
        return;

    $.ajax({
        url: "../General/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor,
            Orden: orden
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }

            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    // $("#SelectOrdenFabricacion").append("<option value='" + row.ORDEN_FABRICACION + "'>" + row.ORDEN_FABRICACION + "</option>")

                    if (row.OrdenVenta == '0') {
                        $("#txtOrdenVenta").val('0');
                        $("#txtCodigoCliente").val('0');
                        $("#txtCliente").val('Libre Utilización');
                    } else {
                        $("#txtOrdenVenta").val(row.OrdenVenta);
                        $("#txtCodigoCliente").val(row.CodigoCliente);
                        $("#txtCliente").val(row.NombreCliente);
                    }

                    $("#txtCodigoProducto").val(row.ItemCode);
                    $("#txtProducto").val(row.ItemName);
                    $("#txtPesoNeto").val(parseInt(row.PesoNeto));
                    $("#txtLineaNegocio").val(row.LineaNegocio);

                });
                $('#validaFecha').prop("hidden", true);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}



function Validar() {
    var bool = true;


    if ($("#txtFechaProduccion").val() == "") {
        $("#txtFechaProduccion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtFechaProduccion").css('borderColor', '#ced4da');
    }
    if ($("#txtOrdenVenta").val() == "") {
        $("#txtOrdenVenta").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtOrdenVenta").css('borderColor', '#ced4da');
    }
    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }
   
    if ($("#txtProducto").val() == "") {
        $("#txtProducto").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtProducto").css('borderColor', '#ced4da');
    }  
    if ($("#selectPeso").val() == "") {
        $("#selectPeso").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectPeso").css('borderColor', '#ced4da');
    }  
    if ($("#selectLinea").val() == "") {
        $("#selectLinea").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectLinea").css('borderColor', '#ced4da');
    }  
    return bool;
}


function NuevoControlPesoEnlatado() {
    $("#txtIdControlPesoEnlatado").val('');
    $("#SelectOrdenFabricacion").prop("selectedIndex", 0);
    $("#selectPeso").prop("selectedIndex", 0);
    $("#selectLinea").prop("selectedIndex", 0);
    LimpiarDetalle();
}

function GuardarControlPesoEnlatado() {
    if (!Validar())
        return;
    $("#btnGenerar").prop("disabled", true);
    $('#spinnerCargando').prop("hidden", false);
    var DivControl = $('#DivTableControl');
    DivControl.html('');
    $.ajax({
        url: "../ControlPesoEnlatado/ControlPesoEnlatado",
        type: "POST",
        data: {
            IdControlPesoEnlatado: $("#txtIdControlPesoEnlatado").val(),
            OrdenVenta: $("#txtOrdenVenta").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),           
            Peso: $("#selectPeso").val(),
            LineaEnlatado: $("#selectLinea").val(),
            CodigoProducto: $("#txtCodigoProducto").val(),
            Producto: $("#txtProducto").val(),           
            Fecha: $("#txtFechaProduccion").val(),
            Turno: $("#txtTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == '0') {
                MensajeAdvertencia("Faltan Parametros");
            } else if (resultado == '1') {
                MensajeAdvertencia("Control ya ha sido ingresado con esos parametros");
                CargarControlPesoEnlatado();
            } else {
                CargarControlPesoEnlatado();
                MensajeCorrecto(resultado);
                NuevoControlPesoEnlatado();
            }
            $("#btnGenerar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);
        },
        error: function (resultado) {
            CargarControlPesoEnlatado();
            $("#btnGenerar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);
            NuevoControlPesoEnlatado();

        }
    });

}


function EliminarControlPesoEnlatado() {
    $.ajax({
        url: "../ControlPesoEnlatado/EliminarControlPesoEnlatado",
        type: "POST",
        data: {
            IdControlPesoEnlatado: $("#txtIdControlPesoEnlatado").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            Atras();
            MensajeCorrecto(resultado);
         //   $('#spinnerCargando').prop("hidden", true);
        },
        error: function (resultado) {
           // CargarControlPesoEnlatado();
       //     $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);

        }
    });
}


function ModalEliminar() {  
    $("#mi-modal").modal('show');
}

$("#modal-btn-si").on("click", function () {
    EliminarControlPesoEnlatado();
    $("#mi-modal").modal('hide');
});

$("#modal-btn-no").on("click", function () {
    $("#txtEliminar").val('');
    $("#mi-modal").modal('hide');
});


//----------------------------------------------PESO ENLATADO DETALLE--------------------------------------------------------

function SeleccionarControlPesoEnlatado(model) {
    LimpiarDetalle();
    $("#divDetalleControl").fadeIn("swing");
    $("#divCabeceraControl").fadeOut("swing");
    $("#btnEliminarControlPeso").prop("hidden", false);

    $("#btnAtras").prop("hidden", false);
    $("#btnNuevo").prop("hidden", false);
    $("#btnGenerar").prop("hidden", true);
    $("#btnGuardar").prop("hidden", false);
    $("#txtIdControlPesoEnlatado").val(model.IdControlPesoEnlatado);
    $("#txtOrdenFabricacion").val(model.OrdenFabricacion);
    $("#txtOrdenVenta").val(model.OrdenVenta);
    $("#selectLinea").val(model.LineaEnlatado);
    $("#txtProducto").val(model.Producto);
  
    $("#MensajeDetalleGeneral").prop("hidden", false);
    $("#MensajeDetalleGeneral").html('');
    $("#MensajeDetalleGeneral").html("OF:" + model.OrdenFabricacion + ", " + "OV:" + model.OrdenVenta + ", " + model.Producto + ", " + model.LineaEnlatado + ", " + model.Peso);
    CargarControlPesoEnlatadoDetalle();   
}


function CargarControlPesoEnlatadoDetalle() {
    $("#DivTableControl").html('');
    $("#DivTableControlDetalle").html('');
    $("#DivCardCabecera").prop("hidden", true);
    $("#DivCard").prop("hidden", true);    
    $("#DivCardDetalle").prop("hidden", false);
    $("#DivMensajeDetalle").html('');
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../ControlPesoEnlatado/ControlPesoEnlatadoDetallePartial",
        type: "GET",
        data:
        {
            IdControl: $('#txtIdControlPesoEnlatado').val(),
            Fecha: $("#txtFechaProduccion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
            if (resultado == 0) {
                $("#DivMensajeDetalle").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTableControlDetalle").html(resultado);
            }
            $("#spinnerCargandoDetalle").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargandoDetalle").prop("hidden", true);

        }
    });
}

function Atras() {
    $("#DivCardCabecera").prop("hidden", false);
    $("#DivCard").prop("hidden", false);
    $("#DivCardDetalle").prop("hidden", true);
    $("#MensajeDetalleGeneral").prop("hidden", true);
    $("#divDetalleControl").fadeOut("swing");
    $("#divCabeceraControl").fadeIn("swing");
    $("#btnAtras").prop("hidden", true);
    $("#btnGenerar").prop("hidden", false);
    $("#txtIdControlPesoEnlatado").val('0');
    $("#txtIdControlPesoEnlatadoDetalleCard").val('0');
    $("#DivTableControlSubDetalle").html('');
    $("#MensajeTituloSubDetalle").html('');
    $("#btnEliminarControlPeso").prop("hidden", true);

    CargarControlPesoEnlatado();
}

function ModalDetalleEditar(id,agua,aceite,hora) {
    $("#txtTemperaturaAgua").val(agua);
    $("#txtTemperaturaAceite").val(aceite);
    var fecha = moment(hora).format("YYYY-MM-DDTHH:mm");
    $("#txtHoraDetalle").val(fecha);
    $("#txtIdControlPesoEnlatadoDetalle").val(id);

    $("#txtHoraDetalle").prop("disabled", true);
    $("#txtHoraDetalle").css('borderColor', '#ced4da');
    $("#txtTemperaturaAceite").css('borderColor', '#ced4da');
    $("#txtTemperaturaAgua").css('borderColor', '#ced4da');

    $("#ModalNuevoPesoEnlatadoDetalle").modal("show");
}

function ModalDetalle() {
    $("#txtTemperaturaAgua").val('');
    $("#txtTemperaturaAceite").val('');
    $("#txtIdControlPesoEnlatadoDetalle").val('0');

    $("#ModalNuevoPesoEnlatadoDetalle").modal("show");

    $("#txtHoraDetalle").prop("disabled", false);
    $("#txtHoraDetalle").css('borderColor', '#ced4da');
    $("#txtTemperaturaAceite").css('borderColor', '#ced4da');
    $("#txtTemperaturaAgua").css('borderColor', '#ced4da');
}


function ValidarDetalle() {
    var bool = true;
    if ($("#txtHoraDetalle").val() == "") {
        $("#txtHoraDetalle").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtHoraDetalle").css('borderColor', '#ced4da');
    }
    return bool;
}

function GuardarControlPesoEnlatadoDetalle() {
    if (!ValidarDetalle())
        return;   
    $('#spinnerCargando').prop("hidden", false);    
    $.ajax({
        url: "../ControlPesoEnlatado/GuardarControlPesoEnlatadoDetalle",
        type: "POST",
        data: {
            IdControlPesoEnlatadoDetallado: $("#txtIdControlPesoEnlatadoDetalle").val(),
            IdControlPesoEnlatado: $("#txtIdControlPesoEnlatado").val(),
            Hora: $("#txtHoraDetalle").val(),
            TemperaturaAgua: $("#txtTemperaturaAgua").val(),
            TemperaturaAceite: $("#txtTemperaturaAceite").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
            $("#ModalNuevoPesoEnlatadoDetalle").modal("hide");
            CargarControlPesoEnlatadoDetalle();
            MensajeCorrecto(resultado);  
            $('#spinnerCargando').prop("hidden", true);
        },
        error: function (resultado) {
            CargarControlPesoEnlatadoDetalle();           
            $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);
           
        }
    });

}

function ModalEliminarDetalle(id) {
    $("#txtEliminar").val(id);
    $("#mi-modal-detalle").modal('show');
}

$("#modal-btn-si-detalle").on("click", function () {
    EliminarControlPesoDetalle();
    $("#mi-modal-detalle").modal('hide');
});

$("#modal-btn-no-detalle").on("click", function () {
    $("#txtEliminar").val('');
    $("#mi-modal-detalle").modal('hide');
});

function EliminarControlPesoDetalle() {
    $.ajax({
        url: "../ControlPesoEnlatado/EliminarControlPesoEnlatadoDetalle",
        type: "POST",
        data: {
            IdControlPesoEnlatadoDetallado: $("#txtEliminar").val()          
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
            CargarControlPesoEnlatadoDetalle();
            MensajeCorrecto(resultado);
            $('#spinnerCargando').prop("hidden", true);
        },
        error: function (resultado) {
            CargarControlPesoEnlatadoDetalle();
            $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);          

        }
    });
}


//----------------------------------------------PESO ENLATADO SUBDETALLE--------------------------------------------------------
function ModalSubDetalle() {
    if ($("#txtIdControlPesoEnlatadoDetalleCard").val() < 1) {
        MensajeAdvertencia("Seleccione un detalle");
        return;
    }
    var filas = $('#tablaControlPesoEnlatadoSubDetalle >tbody >tr').length+1;
    //alert(filas);

    $("#txtPeso").val('');
    $("#txtMuestra").val(filas);
    $("#txtPeso").css('borderColor', '#ced4da');
    $("#txtMuestra").css('borderColor', '#ced4da');
    $("#txtIdControlPesoEnlatadoSubDetalle").val('0');
    $("#ModalNuevoPesoEnlatadoSubDetalle").modal("show");
}

function SeleccionarControlSubDetalle(id, fecha) {
    $("#txtIdControlPesoEnlatadoDetalleCard").val(id);
    $("#MensajeTituloSubDetalle").html(fecha);
    CargarControlPesoEnlatadoSubDetalle();
}


function CargarControlPesoEnlatadoSubDetalle() {  
    $("#DivTableControlSubDetalle").html('');
    $("#DivMensajeSubDetalle").html('');
    $("#spinnerCargandoSubDetalle").prop("hidden", false);
    $.ajax({
        url: "../ControlPesoEnlatado/ControlPesoEnlatadoDetalleSubPartial",
        type: "GET",
        data:
        {
            IdControlDetalle: $("#txtIdControlPesoEnlatadoDetalleCard").val()          
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
            if (resultado == 0) {
                $("#DivMensajeSubDetalle").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTableControlSubDetalle").html(resultado);                
            }
            $("#spinnerCargandoSubDetalle").prop("hidden", true);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargandoSubDetalle").prop("hidden", true);
        }
    });
}

function ValidarSubDetalle() {
    var bool = true;
    if ($("#txtPeso").val() == "" || $("#txtPeso").val()<1 ) {
        $("#txtPeso").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtPeso").css('borderColor', '#ced4da');
    }
    if ($("#txtMuestra").val() == "" || $("#txtMuestra").val() < 1) {
        $("#txtMuestra").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtMuestra").css('borderColor', '#ced4da');
    }
    
    return bool;
}

function GuardarControlPesoEnlatadoSubDetalle() {   
    if (!ValidarSubDetalle())
        return;
   // $('#spinnerCargandoSubDetalle').prop("hidden", false);
    $.ajax({
        url: "../ControlPesoEnlatado/GuardarControlPesoEnlatadoSubDetalle",
        type: "POST",
        data: {
            IdControlPesoEnlatadoDetallado: $("#txtIdControlPesoEnlatadoDetalleCard").val(),
            IdControlPesoEnlatadoSubDetalle: $("#txtIdControlPesoEnlatadoSubDetalle").val(),
            IdControlPesoEnlatado: $("#txtIdControlPesoEnlatado").val(),
            Muestra: $("#txtMuestra").val(),
            Peso: $("#txtPeso").val()           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 1) {
                MensajeAdvertencia("Muestra ya fue ingresada");
                return;
            }
            $("#ModalNuevoPesoEnlatadoSubDetalle").modal("hide");
            CargarControlPesoEnlatadoSubDetalle();
           // MensajeCorrecto(resultado);
          //  $('#spinnerCargandoSubDetalle').prop("hidden", true);
        },
        error: function (resultado) {
            CargarControlPesoEnlatadoSubDetalle();
            $('#spinnerCargandoSubDetalle').prop("hidden", true);
            MensajeError(resultado.responseText, false);       
        }
    });

}

function ModalEditarSubDetalle(id,muestra,peso) {
    $("#txtPeso").val(peso);
    $("#txtMuestra").val(muestra);
    $("#txtPeso").css('borderColor', '#ced4da');
    $("#txtMuestra").css('borderColor', '#ced4da');
    $("#txtIdControlPesoEnlatadoSubDetalle").val(id);
    $("#ModalNuevoPesoEnlatadoSubDetalle").modal("show");
}


function ModalEliminarSubDetalle(id) {
    $("#txtEliminarSub").val(id);
    $("#mi-modal-Subdetalle").modal('show');
}

$("#modal-btn-si-Subdetalle").on("click", function () {
    EliminarControlPesoSubDetalle();
    $("#mi-modal-Subdetalle").modal('hide');
});

$("#modal-btn-no-Subdetalle").on("click", function () {
    $("#txtEliminarSub").val('');
    $("#mi-modal-Subdetalle").modal('hide');
});

function EliminarControlPesoSubDetalle() {
    $.ajax({
        url: "../ControlPesoEnlatado/EliminarControlPesoEnlatadoSubDetalle",
        type: "POST",
        data: {
            IdControlPesoEnlatadoSubDetalle: $("#txtEliminarSub").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarControlPesoEnlatadoSubDetalle();
            MensajeCorrecto(resultado);
            $('#spinnerCargando').prop("hidden", true);
        },
        error: function (resultado) {
            CargarControlPesoEnlatadoSubDetalle();
            $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);

        }
    });
}
