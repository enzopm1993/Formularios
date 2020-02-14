$(document).ready(function () {
    CargarMapeoProductoTunel();
    CargarOrdenFabricacion();
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


function CargarLotes(valor) {


    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>");
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
                });
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarMapeoProductoTunel() { 
    $("#spinnerCargando").prop("hidden", false);
    $("#chartCabecera2").html('');
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
    $("#SelectTipoLimpieza").prop("SelectedIndex",0);
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



function SeleccionarControl(model) {
    //console.log(model);
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

    NuevoControl();
}