
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
    $("#txtFechaOrden").val($("#txtFechaProduccion").val());


    var fecha = moment($("#txtFechaProduccion").val()).format("YYYY-MM-DDTHH:mm");
    $("#txtFechaInicioDetalle").val(fecha);
    $("#txtFechaFinDetalle").val(fecha);

  
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ControlPesoEnlatado/ControlPesoEnlatadoPartial",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaProduccion').val()          
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
        url: "../ControlHoraMaquina/ConsultarOrdenesFabricacion",
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
    $("#txtIdControlHoraMaquina").val('');
    $("#SelectOrdenFabricacion").prop("selectedIndex", 0);
    $("#selectPeso").prop("selectedIndex", 0);
    $("#selectLinea").prop("selectedIndex", 0);
    LimpiarDetalle();
}

function GuardarControlHoraMaquina() {
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
            IdControlPesoEnlatado: $("#txtIdControlHoraMaquina").val(),
            OrdenVenta: $("#txtOrdenVenta").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),           
            Peso: $("#selectPeso").val(),
            LineaEnlatado: $("#selectLinea").val(),
            CodigoProducto: $("#txtCodigoProducto").val(),
            Producto: $("#txtProducto").val(),           
            Fecha: $("#txtFechaProduccion").val()
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

function SeleccionarControlPesoEnlatado(model) {
    LimpiarDetalle();
    $("#divDetalleControl").fadeIn("swing");
    $("#divCabeceraControl").fadeOut("swing");
    $("#btnInactivar").prop("hidden", false);

    $("#btnAtras").prop("hidden", false);
    $("#btnNuevo").prop("hidden", false);
    $("#btnGenerar").prop("hidden", true);
    $("#btnGuardar").prop("hidden", false);
    $("#txtIdControlPesoEnlatado").val(model.IdControlPesoEnlatado);
    $("#txtOrdenFabricacion").val(model.OrdenFabricacion);
    $("#txtOrdenVenta").val(model.OrdenVenta);
    $("#selectLinea").val(model.LineaEnlatado);
    $("#txtProducto").val(model.Producto);
  
    $("#DivMensaje").prop("hidden", false);
    $("#DivMensaje").html('');
    $("#DivMensaje").html("OF:" + model.OrdenFabricacion + ", " + "OV:" + model.OrdenVenta + ", " + model.Producto + ", " + model.LineaEnlatado + ", " + model.Peso);
    CargarControlPesoEnlatadoDetalle();   
}


function CargarControlPesoEnlatadoDetalle() {
    $("#DivTableControl").html('');
    $("#DivCard").prop("hidden", true);
    $("#DivCardDetalle").prop("hidden", false);
    
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ControlHoraMaquina/ControlHoraMaquinaDetallePartial",
        type: "GET",
        data:
        {
            IdControl: $('#txtIdControlHoraMaquina').val(),
            Fecha: $("#txtFechaProduccion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarOrdenFabricacion();
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivCardDetalle").html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}
