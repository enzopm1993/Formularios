var DatosCabecera = [];

$(document).ready(function () {
    ConsultarMonitoreoDescongelado();

});

function ConsultarMonitoreoDescongelado() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '') {
        $("#divCabecera2").prop("hidden", true);
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }

    $("#divCabecera2").prop("hidden", false);
    $("#spinnerCargando").prop("hidden", false);
  //  ConsultarPeliduvios();
    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongeladoPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()     
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = [[2, "asc"]];;

                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function nuevoControl(){
    $("#txtHora").css('borderColor', '#ced4da');
    $("#txtTemperaturaAgua").css('borderColor', '#ced4da');
    $("#txtMuestra1").css('borderColor', '#ced4da');
    $("#txtMuestra2").css('borderColor', '#ced4da');
    $("#txtMuestra3").css('borderColor', '#ced4da');

}

function SeleccionarControl(model) {
    nuevoControl();
    DatosCabecera = model;
    //console.log(DatosCabecera);
    $("#txtTanque").val(DatosCabecera.U_SYP_TANQUE);
    $("#txtLote").val(DatosCabecera.U_SYP_LOTE);    
    $("#txtHora").val(moment().format("YYYY-MM-DDTHH:mm"));    
    $("#txtTipo").val($("#selectTipo option:selected").text());
    if ($("#selectTipo").val() == "C") {
        $("#divTemperaturaAgua").prop("hidden", true);
    } else {
        $("#divTemperaturaAgua").prop("hidden", false);
    }
    ConsultarMonitoreoDetalle();
    $("#ModalMonitoreo").modal("show");
}

function ConsultarMonitoreoDetalle() {
    
    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongeladoDetallePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Tanque: DatosCabecera.U_SYP_TANQUE,
            Lote: DatosCabecera.U_SYP_LOTE,
            Tipo: $("#selectTipo").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != "0") {
                $("#txtHora").val(moment(resultado.Hora).format("YYYY-MM-DDTHH:mm"));
                $("#txtTemperaturaAgua").val(resultado.TemperaturaAgua);
                $("#txtMuestra1").val(resultado.Muestra1);
                $("#txtMuestra2").val(resultado.Muestra2);
                $("#txtMuestra3").val(resultado.Muestra3);
                $("#txtIdControl").val(resultado.IdMonitoreoDescongelado);
                
            } else {
                $("#txtIdControl").val(0);
                $("#txtHora").val(moment().format("YYYY-MM-DDTHH:mm"));
                $("#txtTemperaturaAgua").val('');
                $("#txtMuestra1").val('');
                $("#txtMuestra2").val('');
                $("#txtMuestra3").val('');

            }
            $("#btnGuardarMonitoreo").prop("disabled", false);
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);         
        }
    });
}


function Validar() {
    var valida = true;
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }
    if ($("#selectTipo").val() != 'C') {
        if ($("#txtTemperaturaAgua").val() == "") {
            $("#txtTemperaturaAgua").css('borderColor', '#FA8072');
            valida = false;
        } else {
            $("#txtTemperaturaAgua").css('borderColor', '#ced4da');
        }
    }
    if ($("#txtMuestra1").val() == "") {
        $("#txtMuestra1").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtMuestra1").css('borderColor', '#ced4da');
    }

    if ($("#txtMuestra2").val() == "") {
        $("#txtMuestra2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtMuestra2").css('borderColor', '#ced4da');
    }

    if ($("#txtMuestra3").val() == "") {
        $("#txtMuestra3").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtMuestra3").css('borderColor', '#ced4da');
    }   
    return valida;
}

function GuardarMonitoreoDescongelado() {
    if (!Validar()) {
        return;
    }

    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongelado",
        type: "POST",
        data: {
            IdMonitoreoDescongelado: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            Tanque: DatosCabecera.U_SYP_TANQUE,
            Lote: DatosCabecera.U_SYP_LOTE,
            Especie: DatosCabecera.U_SYP_ESPECIE,
            Talla: DatosCabecera.U_SYP_TALLA,
            Hora: $("#txtHora").val(),
            Tipo: $("#selectTipo").val(),
            TemperaturaAgua: $("#txtTemperaturaAgua").val(),
            Muestra1: $("#txtMuestra1").val(),
            Muestra2: $("#txtMuestra2").val(),
            Muestra3: $("#txtMuestra3").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
           
            MensajeCorrecto(resultado);         
            $("#ModalMonitoreo").modal("hide");
            ConsultarMonitoreoDescongelado();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}
