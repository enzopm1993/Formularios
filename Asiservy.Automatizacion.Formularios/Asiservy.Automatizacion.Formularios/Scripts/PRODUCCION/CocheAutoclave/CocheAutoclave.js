var DatosOF = [];

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
    $("#txtAutoclave").val('');
    $("#txtParada").val('');
    $("#txtCodProducto").val('');
    $("#txtLote").val('');
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

    if ($("#txtTurno").val() == "") {
        $("#txtTurno").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTurno").css('borderColor', '#ced4da');
    }

    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }

    if ($("#txtAutoclave").val() == "") {
        $("#txtAutoclave").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAutoclave").css('borderColor', '#ced4da');
    }

    if ($("#txtParada").val() == "") {
        $("#txtParada").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtParada").css('borderColor', '#ced4da');
    }

    if ($("#txtLote").val() == "") {
        $("#txtLote").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtLote").css('borderColor', '#ced4da');
    }

    if ($("#txtCodProducto").val() == "") {
        $("#txtCodProducto").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodProducto").css('borderColor', '#ced4da');
    }

    return valida;

}

function GuardarCocheAutoclave() {
    if (!Validar()) {
        return;
    }

    $.ajax({
        url: "../CocheAutoclave/CocheAutoclave",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#txtTurno").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            Autoclave: $("#txtAutoclave").val(),
            Parada: $("#txtParada").val(),
            Producto: DatosOF.NOMBRE_PRODUCTO,
            CodigoProducto: $("#txtCodProducto").val(),
            Envase: DatosOF.ENVASE,
            Lote: $("#txtLote").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            CargarCocheAutoclave();
            NuevoControl();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
           
        }
    });
}
function CargarDatosOrdenFabricacion() {
    if ($("#txtOrdenFabricacion").val() == "") {      
        return;
    }
    DatosOF = [];
    $.ajax({
        url: "../General/ConsultarDatosOrdenFabricacion",
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
           // console.log(resultado);
            $("#txtProducto").val(resultado.NOMBRE_PRODUCTO);
            DatosOF = resultado;
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
            MensajeError("Error: Comuníquese con sistemas", false);
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
    //CargarLotes();
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
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}



//function CargarLotes() {
//    $("#selectLote").empty();
//    $("#selectLote").append("<option value='' >-- Seleccionar Opción--</option>");
//    valor = $("#txtOrdenFabricacion").val();
//    if (valor == '' || valor == null)
//        return;

//    $.ajax({
//        url: "../General/ConsultarLotesPorOf",
//        type: "GET",
//        data: {
//            Orden: valor
//        },
//        success: function (resultado) {
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (resultado == "0") {
//                MensajeAdvertencia("No existen ordenes para esa fecha");
//                return;
//            }
//            // LimpiarDetalle();
//            if (!$.isEmptyObject(resultado)) {
//                $.each(resultado, function (create, row) {
//                    $("#SelectOrdenFabricacion").append("<option value='" + row.descripcion + "'>" + row.descripcion + "</option>")
//                });
//            }
//        },
//        error: function (resultado) {
//            console.log(resultado);
//            MensajeError("Error: Comuníquese con sistemas", false);
//        }
//    });
//}





function SeleccionarControl(model) {
    console.log(model);
    $("#divCabecera").prop("hidden", true);
    $("#divCabecera2").prop("hidden", true);
    $("#btnGenerar").prop("hidden", true);
    $("#btnAtras").prop("hidden", false);

}

function AtrasControlPrincipal() {
    $("#divCabecera").prop("hidden", false);
    $("#divCabecera2").prop("hidden", false);
    $("#btnGenerar").prop("hidden", false);
    $("#btnAtras").prop("hidden", true);
    CargarCocheAutoclave();
}