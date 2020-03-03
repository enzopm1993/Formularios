$(document).ready(function () {
    CargarCocheAutoclave();
    CargarOrdenFabricacion();
    //$('#SelectTextura').select2();
    //$('#selectEspecie').select2();

});


function CargarCocheAutoclave() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '' || $("#txtTurno").val() =='' ) {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);

    //  CargarOrdenFabricacion();
    $.ajax({
        url: "../CocheAutoclave/CocheAutoclavePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#txtTurno").val()
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



function GuardarCocheAutoclave() {
  
    $.ajax({
        url: "../CocheAutoclave/CocheAutoclavePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#txtTurno").val()
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










function CargarDatosOrdenFabricacion() {
    if ($("#txtOrdenFabricacion").val() == "") {
      //  NuevoControlConsumoInsumos();
        return;
    }
    $.ajax({
        url: "../General/ConsultarDatosOrdenFabricacion",
        type: "GET",
        data: {
            Orden: $("#txtOrdenFabricacion").val()
            //  Linea: $("#txtLineaNegocio").val()
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
            $("#txtProducto").val(resultado.NOMBRE_PRODUCTO);
            //$("#txtPesoEscrundido").val(resultado.PESO_ESCURRIDO);
            //$("#txtLomo").val(resultado.LOMOS);
            //$("#txtMiga").val(resultado.MIGAS);
            //$("#txtCajas").val(parseInt(resultado.UNIDADES_X_CAJA));
            //$("#txtOrdenVenta").val(parseInt(resultado.PEDIDO_VENTA));
            //$("#txtMarca").val(resultado.MARCA);
            //if (resultado.CLIENTE_CORTO != '') {
            //    $("#txtCliente").val(resultado.CLIENTE_CORTO);
            //} else {
            //    $("#txtCliente").val(resultado.CLIENTE);
            //}

            // $("#txtCodigoProducto").val(resultado.CODIGO_PRODUCTO);


            //$("#").val('');
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
          //  NuevoControlConsumoInsumos();
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
    CargarDatosOrdenFabricacion();    
  //  CargarLotes($("#txtOrdenFabricacion").val());
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
        url: "../General/ConsultaOFNivel2",
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
