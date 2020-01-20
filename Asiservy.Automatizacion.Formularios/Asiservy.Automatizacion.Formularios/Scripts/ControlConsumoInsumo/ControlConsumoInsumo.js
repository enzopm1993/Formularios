
var ListadoControl = [];

$(document).ready(function () {
    // CargarControlConsumo(); 
});

function CargarDatosOrdenFabricacion() {

    if ($("#txtOrdenFabricacion").val() == "") {
        NuevoControlConsumoInsumos();
        return;
    }

    $.ajax({
        url: "../ControlConsumoInsumo/ConsultarDatosOrdenFabricacion",
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
           // console.log(resultado);
            $("#txtPesoNeto").val(parseInt(resultado.PESO_NETO));
            $("#txtPesoEscrundido").val(parseInt(resultado.PESO_ESCURRIDO));
            $("#txtLomo").val(parseInt(resultado.LOMOS));
            $("#txtMiga").val(parseInt(resultado.MIGAS));
            $("#txtCajas").val(parseInt(resultado.UNIDADES_X_CAJA));
            $("#txtOrdenVenta").val(parseInt(resultado.PEDIDO_VENTA));

            //$("#").val('');
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
        url: "../ControlConsumoInsumo/ConsultarOrdenesFabricacion",
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


function CargarControlConsumo() {
    $("#divCabecera2").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", true);
    var txtFecha = $('#txtFecha').val();
    var selectTurno = $('#selectTurno').val();    
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    } 
    if ($("#selectTurno").val() == "" || $("#selectTurno").val() == "0") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }        
 
    $("#spinnerCargando").prop("hidden", false);
    $("#chartCabecera2").html('');
    CargarOrdenFabricacion();
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumoPartial",
        type: "GET",
        data: {
            Fecha: txtFecha,
            LineaNegocio: $("#txtLineaNegocio").val(),
            Turno: selectTurno
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

function NuevoControlConsumoInsumos() {
    $("#txtIdControlConsumo").val('0');
    $("#txtOrdenFabricacion").val('');
    $("#txtOrdenFabricacion").prop("disabled", false);

    $("#txtPesoNeto").val('0');
    $("#txtPesoEscrundido").val('0');
    $("#txtLomo").val('0');
    $("#txtMiga").val('0');
    $("#txtAceite").val('0');
    $("#txtAgua").val('0');
    $("#txtCaldoVegetal").val('0');

    $("#txtOrdenVenta").val('');
    $("#txtHoraInicio").val('');
    $("#txtHoraFin").val('');
    $("#txtDesperdicioSolido").val('0');
    $("#txtDesperdicioLiquido").val('0');
    $("#txtDesperdicioAceite").val('0');
    $("#txtEmpleados").val('0');
    $("#txtCajas").val('0');
    $("#txtObservacion").val('');

}

function ModalGenerarControl(edit) {

    if (edit) {      

    } else {
        var txtFecha = $('#txtFecha').val();
        var selectTurno = $('#selectTurno').val();      
        if (txtFecha == "") {
            MensajeAdvertencia("Igrese una Fecha");
            return;
        }
        if (selectTurno == "" || selectTurno == "0") {
            MensajeAdvertencia("Seleccione un Turno");
            return;
        }
    }  
    $("#ModalGenerarControl").modal("show");
}

function ValidarGenerarControlConsumo(){
    var valida = true;
    if ($("#txtHoraInicio").val() == "") {
        $("#txtHoraInicio").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraInicio").css('borderColor', '#ced4da');
    }

    if ($("#txtPesoNeto").val() == "") {
        $("#txtPesoNeto").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPesoNeto").css('borderColor', '#ced4da');
    }

    if ($("#txtPesoEscrundido").val() == "") {
        $("#txtPesoEscrundido").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPesoEscrundido").css('borderColor', '#ced4da');
    }

    if ($("#txtLomo").val() == "") {
        $("#txtLomo").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtLomo").css('borderColor', '#ced4da');
    }

    if ($("#txtMiga").val() == "") {
        $("#txtMiga").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtMiga").css('borderColor', '#ced4da');
    }

    if ($("#txtAceite").val() == "") {
        $("#txtAceite").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAceite").css('borderColor', '#ced4da');
    }

    if ($("#txtAgua").val() == "") {
        $("#txtAgua").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAgua").css('borderColor', '#ced4da');
    }
    
    return valida;
}

function GenerarControlConsumo() {  
    var txtFecha = $('#txtFecha').val();
    var selectTurno = $('#selectTurno').val();   
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#selectTurno").val() == "" || $("#selectTurno").val() == "0") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }
    if ($("#txtIdControlConsumo").val() == '0') {
        $("#spinnerCargando").prop("hidden", false);
        $("#chartCabecera2").html('');
    }   

    if (!ValidarGenerarControlConsumo()) {
        return;
    }
    
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumo",
        type: "POST",
        data: {
            IdControlConsumoInsumos: $("#txtIdControlConsumo").val(),
            Fecha: txtFecha,
            LineaNegocio: $("#txtLineaNegocio").val(),
            Turno: selectTurno,
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            OrdenVenta: $("#txtOrdenVenta").val(),
            PesoNeto: $("#txtPesoNeto").val(),
            PesoEscrundido: $("#txtPesoEscrundido").val(),
            Lomo: $("#txtLomo").val(),
            Miga: $("#txtMiga").val(),
            Aceite: $("#txtAceite").val(),
            Agua: $("#txtAgua").val(),
            CaldoVegetal: $("#txtCaldoVegetal").val(),
            HoraInicio: $("#txtHoraInicio").val(),
            HoraFin: $("#txtHoraFin").val(),
            DesperdicioSolido: $("#txtDesperdicioSolido").val(),
            DesperdicioLiquido: $("#txtDesperdicioLiquido").val(),
            DesperdicioAceite: $("#txtDesperdicioAceite").val(),
            Empleados: $("#txtEmpleados").val(),
            Cajas: $("#txtCajas").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else if (resultado == "1") {
                MensajeAdvertencia("No se encontró información de la OF")
                return;

            } else if ($("#txtIdControlConsumo").val() == '0') {
                $("#spinnerCargando").prop("hidden", true);               
                CargarControlConsumo();
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

function InactivarControlConsumo() {
    $.ajax({
        url: "../ControlConsumoInsumo/EliminarControlConsumoInsumo",
        type: "POST",
        data: {
            IdControlConsumoInsumos: $("#txtIdControlConsumo").val()           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            MensajeCorrecto("Registro Eliminado con Éxito");            
            $("#modalEliminarControl").modal("hide");
            AtrasControlPrincipal();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

$("#btnModalEliminar").on("click", function () {
    $("#modalEliminarControl").modal('show');
});

$("#modal-btn-si").on("click", function () {
    InactivarControlConsumo();
    $("#modalEliminarControl").modal('hide');
});

$("#modal-btn-no").on("click", function () {
    $("#modalEliminarControl").modal('hide');
});



/////////////DETALLE DEL PROCESO ENLATADO///////////////////////////////////////////////////////////////////////


function SeleccionarControlDetalleConsumo(model) {
    //console.log(model);
    ListadoControl = model;
    $("#txtIdControlConsumo").val(ListadoControl.IdControlConsumoInsumos);
    $("#txtOrdenFabricacion").val(ListadoControl.OrdenFabricacion);
    $("#txtOrdenFabricacion").prop("disabled", true);
    $("#txtPesoNeto").val(ListadoControl.PesoNeto);
    $("#txtPesoEscrundido").val(ListadoControl.PesoEscrundido);
    $("#txtLomo").val(ListadoControl.Lomo);
    $("#txtMiga").val(ListadoControl.Miga);
    $("#txtAceite").val(ListadoControl.Aceite);
    $("#txtAgua").val(ListadoControl.Agua);
    $("#txtCaldoVegetal").val(ListadoControl.CaldoVegetal);
    $("#txtOrdenVenta").val(ListadoControl.OrdenVenta);
    $("#txtHoraInicio").val(ListadoControl.HoraInicio);
    $("#txtHoraFin").val(ListadoControl.HoraFin);
    $("#txtDesperdicioSolido").val(ListadoControl.DesperdicioSolido);
    $("#txtDesperdicioLiquido").val(ListadoControl.DesperdicioLiquido);
    $("#txtDesperdicioAceite").val(ListadoControl.DesperdicioAceite);
    $("#txtEmpleados").val(ListadoControl.Empleados);
    $("#txtCajas").val(ListadoControl.Cajas);
    $("#txtObservacion").val(ListadoControl.Observacion);
    //  $("#divCabecera1").prop("hidden", true);
    $("#btnAtras").prop("hidden", false);
    $("#btnModalEliminar").prop("hidden", false);
    $("#btnModalGenerar").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", false);
    $("#txtFecha").prop("disabled", true);
    $("#selectTurno").prop("disabled", true);    
    $("#divCabecera2").prop("hidden", true);
    $("#divDetalleProceso").prop("hidden", false);
    if ($("#txtLineaNegocio").val() == "ENLATADO") {
        CargarProcesoDetalleEnlatado();
    } else {
        CargarProcesoDetallePouch();
    }
    CargarProcesoDetalleDaniado();
    CargarProcesoDetalleTiemposMuertos();
    CargarProcesoDetalleAditivos();
}

function CargarProcesoDetalleEnlatado() {
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $("#divTableDetalle").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumoDetalleEnlatadoPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle").html("No existen registros");
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#divTableDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
            //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function AtrasControlPrincipal() {
    $("#btnModalGenerar").prop("hidden", false);
    $("#txtFecha").prop("disabled", false);
    $("#selectTurno").prop("disabled", false);
    $("#btnAtras").prop("hidden", true);
    $("#btnModalEliminar").prop("hidden", true);

    $("#btnModalEditar").prop("hidden", true);
    $("#divCabecera2").prop("hidden", false);
    $("#divDetalleProceso").prop("hidden", true);
    ListadoControl = [];
    NuevoControlConsumoInsumos();
    CargarControlConsumo();
}

function ModalGenerarControlDetalle() {    
    $("#txtIdControlDetalleProceso").val('0');
    $("#txtPallet").val('');
    $("#txtLote").val('');
    $("#txtBulto").val('');
    $("#txtFechaFabricacion").val('');
    $("#ModalGenerarControlDetalle").modal("show");
}


function GuardarConsumoDetalle() {

    if ($("#txtLineaNegocio").val() == "ENLATADO") {
        GuardarConsumoEnlatado();
    } else {

    }
}


function validarConsumoEnlatado() {
    var valida = true;
    if ($("#txtPallet").val() == "") {
        $("#txtPallet").css('borderColor', '#FA8072');
        valida=false;
    } else {
        $("#txtPallet").css('borderColor', '#ced4da');
    }
    if ($("#txtLote").val() == "") {
        $("#txtLote").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtLote").css('borderColor', '#ced4da');
    }
    if ($("#txtBulto").val() == "") {
        $("#txtBulto").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtBulto").css('borderColor', '#ced4da');
    }
    if ($("#txtFechaFabricacion").val() == "") {
        $("#txtFechaFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFechaFabricacion").css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarConsumoEnlatado() {

    if (!validarConsumoEnlatado()) {
        return;
    }

    $.ajax({
        url: "../ControlConsumoInsumo/GuardarConsumoInsumoDetalleEnlatado",
        type: "POST",
        data: {
            IdProcesoDetalleLata: $("#txtIdControlDetalleProceso").val(),
            IdControlConsumoInsumos: ListadoControl.IdControlConsumoInsumos,
            Pallet: $("#txtPallet").val(),
            Lotes: $("#txtLote").val(),
            Bultos: $("#txtBulto").val(),
            FechaFabricacion: $("#txtFechaFabricacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divDetalleProceso").html("No existen registros");              
            }
            CargarProcesoDetalleEnlatado();
            $("#ModalGenerarControlDetalle").modal("hide");
           // MensajeCorrecto("Registro Guardado Correctamente");           
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);     
            $("#ModalGenerarControlDetalle").modal("hide");

        }
    });
}

function NuevoProcesoDetalle() {
    $("#txtIdControlDetalleProceso").val('0');
    $("#txtPallet").val('0');
    $("#txtLote").val('0');
    $("#txtBulto").val('0');
    $("#txtFechaFabricacion").val(model.FechaFabricacion);
}

function EditarProcesoDetalle(model) {
   // console.log(model);
    $("#txtIdControlDetalleProceso").val(model.IdProcesoDetalleLata);
    $("#txtPallet").val(model.Pallet);
    $("#txtLote").val(model.Lotes);
    $("#txtBulto").val(model.Bultos);
    $("#txtFechaFabricacion").val(moment(model.FechaFabricacion).format("YYYY-MM-DD"));

    $("#ModalGenerarControlDetalle").modal("show");
    //ModalGenerarControlDetalle();
}



function InactivarDetalleEnlatado() {
    $.ajax({
        url: "../ControlConsumoInsumo/EliminarConsumoDetalleLata",
        type: "POST",
        data: {
            IdProcesoDetalleLata: $("#txtEliminarProcesoDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarProcesoDetalleEnlatado();
          //   MensajeCorrecto("Registro Eliminado con Éxito");
            $("#modalEliminarProcesoDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarProcesoDetalleLata(model) {
    $("#txtEliminarProcesoDetalle").val(model.IdProcesoDetalleLata);
    $("#pModalDetalle").html("Pallet: " + model.Pallet);
    $("#modalEliminarProcesoDetalle").modal('show');
}


$("#modal-Detalle-btn-si").on("click", function () {
    if ($("#txtLineaNegocio").val() == "ENLATADO") {
        InactivarDetalleEnlatado();
    } else {

    }    
    $("#txtEliminarProcesoDetalle").val('0');
    $("#modalEliminarProcesoDetalle").modal('hide');
});

$("#modal-Detalle-btn-no").on("click", function () {
    $("#txtEliminarProcesoDetalle").val('0');
    $("#modalEliminarProcesoDetalle").modal('hide');
});
/////////////POUCH/////////////////////////////////

function CargarProcesoDetallePouch() {
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $("#divTableDetalle").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumoDetallePocuhPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle").html("No existen registros");
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#divTableDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}


function validarConsumoPouch() {
    var valida = true;
    if ($("#txtCaja").val() == "") {
        $("#txtCaja").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCaja").css('borderColor', '#ced4da');
    }
    if ($("#txtLote").val() == "") {
        $("#txtLote").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtLote").css('borderColor', '#ced4da');
    }
   
    if ($("#txtFechaFabricacion").val() == "") {
        $("#txtFechaFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFechaFabricacion").css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarConsumoPouch() {

    if (!validarConsumoPouch()) {
        return;
    }

    $.ajax({
        url: "../ControlConsumoInsumo/GuardarConsumoInsumoDetallePouch",
        type: "POST",
        data: {
            IdProcesoDetallePouch: $("#txtIdControlDetalleProceso").val(),
            IdControlConsumoInsumos: ListadoControl.IdControlConsumoInsumos,
            Cajas: $("#txtCaja").val(),
            Lotes: $("#txtLote").val(),
           // Bultos: $("#txtBulto").val(),
            FechaFabricacion: $("#txtFechaFabricacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divDetalleProceso").html("No existen registros");
            }
            CargarProcesoDetallePouch();
            $("#ModalGenerarControlDetalle").modal("hide");
           // MensajeCorrecto("Registro Guardado Correctamente");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#ModalGenerarControlDetalle").modal("hide");

        }
    });
}






////////CONSUMO DE DANIADOS //////////////////////////////
function CargarProcesoDetalleDaniado() {
    $("#spinnerCargandoDaniados").prop("hidden", false);
    $("#divTableDaniados").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoDaniadoPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos,
            Tipo: $("#txtLineaNegocio").val()
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
    $("#selectDaniado").prop("selectedIndex",0);
    $("#txtLatas").val(0);            
    $("#txtTapas").val(0);
    $("#txtFundas").val(0);
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
    return valida;
}

function GuardarConsumoDaniado() {
    if (!validarConsumoDaniado()) {
        return;
    }
    $.ajax({
        url: "../ControlConsumoInsumo/GuardarConsumoDaniado",
        type: "POST",
        data: {
            IdConsumoDetalleDaniado: $("#txtIdControlDetalleProceso").val(),
            IdControlConsumoInsumos: ListadoControl.IdControlConsumoInsumos,
            Codigo: $("#selectDaniado").val(),
            Latas: $("#txtLatas").val(),
            // Bultos: $("#txtBulto").val(),
            Tapas: $("#txtTapas").val(),
            Fundas: $("#txtFundas").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
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
        url: "../ControlConsumoInsumo/EliminarConsumoDaniado",
        type: "POST",
        data: {
            IdConsumoDetalleDaniado: $("#txtEliminarProcesoDaniado").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
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
    $("#txtEliminarProcesoDaniado").val(model.IdConsumoDetalleDaniado);
    $("#pModalDaniado").html("Merma: " + model.TipoConsumo);
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
    $("#txtEliminarProcesoDaniado").val(model.IdProcesoDetalleLata);
    $("#selectDaniado").val(model.Codigo);
    $("#txtLatas").val(model.Latas);
    $("#txtTapas").val(model.Tapas);
    $("#txtFundas").val(model.Fundas);   

    $("#ModalConsumoDaniado").modal("show");
    //ModalGenerarControlDetalle();
}



/////////////////////TIEMPOS MUERTOS////////////////////////////

function CargarProcesoDetalleTiemposMuertos() {
    $("#spinnerCargandoTiemposMuertos").prop("hidden", false);
    $("#divTableTiemposMuertos").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoTiemposMuertos",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos
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
    $("#selectTipoTiemposMuertos").prop("selectedIndex", 0);
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
        $("#selectDaniado").css('borderColor', '#ced4da');
    }
    if ($("#selectTipoTiemposMuertos").val() == "") {
        $("#selectTipoTiemposMuertos").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectTipoTiemposMuertos").css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarConsumoTiemposMuertos() {
    if (!validarConsumoTiemposMuertos()) {
        return;
    }
    $.ajax({
        url: "../ControlConsumoInsumo/GuardarTiemposMuertos",
        type: "POST",
        data: {
            IdTiempoMuertos: $("#txtIdConsumoTiemposMuertos").val(),
            IdControlConsumoInsumos: ListadoControl.IdControlConsumoInsumos,
            Tipo: $("#selectTipoTiemposMuertos").val(),
            HoraPara: $("#txtHoraPara").val(),
            HoraReinicio: $("#txtHoraReinicio").val(),
            Observacion: $("#txtObservacionTiemposMuertos").val()
         
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
        url: "../ControlConsumoInsumo/EliminarTiemposMuertos",
        type: "POST",
        data: {
            IdTiempoMuertos: $("#txtEliminarTiemposMuertos").val()
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
    $("#txtEliminarTiemposMuertos").val(model.IdTiempoMuertos);
    $("#pModalTiemposMuertos").html("Hora de para: " + moment(model.HoraPara).format("HH:MM"));
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
    $("#txtIdConsumoTiemposMuertos").val(model.IdTiempoMuertos);
    $("#selectTipoTiemposMuertos").val(model.CodTipo);
    $("#txtHoraPara").val(model.HoraPara);
    $("#txtHoraReinicio").val(model.HoraReinicio);
    $("#txtObservacionTiemposMuertos").val(model.Observacion);
    

    $("#ModalConsumoTiemposMuertos").modal("show");
    //ModalGenerarControlDetalle();
}



///////////////// ADITIVOS  /////////////////////////////////////////////////////////////////////

function CargarProcesoDetalleAditivos() {
    $("#spinnerCargandoAditivos").prop("hidden", false);
    $("#divTableAditivos").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoAditivos",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableAditivos").html("No existen registros");
                $("#spinnerCargandoAditivos").prop("hidden", true);
            } else {
                $("#spinnerCargandoAditivos").prop("hidden", true);
                $("#divTableAditivos").html(resultado);
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

function ModalGenerarAditivos() {
    $("#txtIdConsumoAditivos").val(0);
    $("#selectTipoAditivos").prop("selectedIndex", 0);
    //$("#txtHoraPara").val("");
    //$("#txtHoraReinicio").val("");
    //$("#txtObservacionAditivos").val("");
    $("#ModalConsumoAditivos").modal("show");
}

function validarConsumoAditivos() {
    var valida = true;
    if ($("#txtHoraPara").val() == "") {
        $("#txtHoraPara").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectDaniado").css('borderColor', '#ced4da');
    }
    if ($("#selectTipoAditivos").val() == "") {
        $("#selectTipoAditivos").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectTipoAditivos").css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarConsumoAditivos() {
    if (!validarConsumoAditivos()) {
        return;
    }
    $.ajax({
        url: "../ControlConsumoInsumo/GuardarAditivos",
        type: "POST",
        data: {
            IdTiempoMuertos: $("#txtIdConsumoAditivos").val(),
            IdControlConsumoInsumos: ListadoControl.IdControlConsumoInsumos,
            Tipo: $("#selectTipoAditivos").val(),
            HoraPara: $("#txtHoraPara").val(),
            HoraReinicio: $("#txtHoraReinicio").val(),
            Observacion: $("#txtObservacionAditivos").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            CargarProcesoDetalleAditivos();
            $("#ModalConsumoAditivos").modal("hide");
            //   MensajeCorrecto("Registro Guardado Correctamente");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#ModalConsumoAditivos").modal("hide");
        }
    });
}


function InactivarAditivos() {
    $.ajax({
        url: "../ControlConsumoInsumo/EliminarAditivos",
        type: "POST",
        data: {
            IdTiempoMuertos: $("#txtEliminarAditivos").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarProcesoDetalleAditivos();
            //  MensajeCorrecto("Registro Eliminado con Éxito");
            $("#modalEliminarAditivos").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConsumoAditivos(model) {
    $("#txtEliminarAditivos").val(model.IdTiempoMuertos);
    $("#pModalAditivos").html("Hora de para: " + moment(model.HoraPara).format("HH:MM"));
    $("#modalEliminarAditivos").modal('show');
}


$("#modal-Aditivos-btn-si").on("click", function () {
    InactivarAditivos();
    $("#txtEliminarAditivos").val('0');
    $("#modalEliminarAditivos").modal('hide');
});

$("#modal-Aditivos-btn-no").on("click", function () {
    $("#txtEliminarAditivos").val('0');
    $("#modalEliminarAditivos").modal('hide');
});


function EditarConsumoAditivos(model) {
    // console.log(model);
    $("#txtIdConsumoAditivos").val(model.IdTiempoMuertos);
    $("#selectTipoAditivos").val(model.CodTipo);
    $("#txtHoraPara").val(model.HoraPara);
    $("#txtHoraReinicio").val(model.HoraReinicio);
    $("#txtObservacionAditivos").val(model.Observacion);


    $("#ModalConsumoAditivos").modal("show");
    //ModalGenerarControlDetalle();
}

