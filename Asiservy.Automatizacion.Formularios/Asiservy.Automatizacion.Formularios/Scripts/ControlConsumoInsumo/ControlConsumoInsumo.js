
var ListadoControl = [];

$(document).ready(function () {
    // CargarControlConsumo(); 
});

function CargarControlConsumo() {
    $("#divCabecera2").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", true);

    var txtFecha = $('#txtFecha').val();
    var selectTurno = $('#selectTurno').val();
    // console.log(selectLinea);
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
          //      config.opcionesDT.order = [[0, "asc"]];
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
    $("#txtOrdenFabricacion").prop("readonly", false);

    $("#txtPesoNeto").val('0');
    $("#txtLomo").val('0');
    $("#txtMiga").val('0');
    $("#txtAceite").val('0');
    $("#txtAgua").val('0');

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
        //console.log(ListadoControl);
        $("#txtIdControlConsumo").val(ListadoControl.IdControlConsumoInsumos);
        $("#txtOrdenFabricacion").val(ListadoControl.OrdenFabricacion);
        $("#txtOrdenFabricacion").prop("readonly",true);        
        $("#txtPesoNeto").val(ListadoControl.PesoNeto);
        $("#txtLomo").val(ListadoControl.Lomo);
        $("#txtMiga").val(ListadoControl.Miga);
        $("#txtAceite").val(ListadoControl.Aceite);
        $("#txtAgua").val(ListadoControl.Agua);
       // $("#txtCaldoVegetal").val(ListadoControl.IdControlConsumoInsumos);
        $("#txtOrdenVenta").val(ListadoControl.OrdenVenta);
        $("#txtHoraInicio").val(ListadoControl.HoraInicio);
        $("#txtHoraFin").val(ListadoControl.HoraFin);
        $("#txtDesperdicioSolido").val(ListadoControl.DesperdicioSolido);
        $("#txtDesperdicioLiquido").val(ListadoControl.DesperdicioLiquido);
        $("#txtDesperdicioAceite").val(ListadoControl.DesperdicioAceite);
        $("#txtEmpleados").val(ListadoControl.Empleados);
        $("#txtCajas").val(ListadoControl.Cajas);
        $("#txtObservacion").val(ListadoControl.Observacion);



    } else {
        var txtFecha = $('#txtFecha').val();
        var selectTurno = $('#selectTurno').val();
        // console.log(selectLinea);
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


function GenerarControlConsumo() {  
    var txtFecha = $('#txtFecha').val();
    var selectTurno = $('#selectTurno').val();
    // console.log(selectLinea);
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
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumo",
        type: "POST",
        data: {
            IdControlConsumoInsumos: $("#txtIdControlConsumo").val(),
            Fecha: txtFecha,
            LineaNegocio: $("#txtLineaNegocio").val(),
            Turno: selectTurno,
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            PesoNeto: $("#txtPesoNeto").val(),
            PesoEscurido: $("#txtPesoEscrundido").val(),
            Lomo: $("#txtLomo").val(),
            Miga: $("#txtMiga").val(),
            Aceite: $("#txtAceite").val(),
            Agua: $("#txtAgua").val(),
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
            } else if ($("#txtIdControlConsumo").val() == '0') {
                $("#spinnerCargando").prop("hidden", true);
                /// $("#chartCabecera2").html('')
                CargarControlConsumo();
                MensajeCorrecto("Registro Generado con Éxito");

            } else {
                MensajeCorrecto("Registro Actualzado con Éxito");
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


/////////////DETALLE///////////////////////////////////////////////////////////////////////


function SeleccionarControlDetalleConsumo(model) {
    ListadoControl = model;
    //  $("#divCabecera1").prop("hidden", true);
    $("#btnAtras").prop("hidden", false);
    $("#btnModalGenerar").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", false);
    $("#txtFecha").prop("disabled", true);
    $("#selectTurno").prop("disabled", true);
    
    $("#divCabecera2").prop("hidden", true);
    $("#divDetalleProceso").prop("hidden", false);

    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumoDetalleEnlatadoPartial",
        type: "GET",
        data: {
            IdControl: model.IdControlConsumoInsumos
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
                config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
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
    $("#btnModalEditar").prop("hidden", true);
    $("#divCabecera2").prop("hidden", false);
    $("#divDetalleProceso").prop("hidden", true);
    ListadoControl = [];
    NuevoControlConsumoInsumos();
}

function ModalGenerarControlDetalle() {
    
    $("#ModalGenerarControlDetalle").modal("show");
}




