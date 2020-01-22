
$(document).ready(function () {
    CargarControlConsumo();
});



function CargarControlConsumo() {
    $("#divCabecera2").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#btnImprimir").prop("hidden", true); 
    
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
   // CargarOrdenFabricacion();
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumoPartial",
        type: "GET",
        data: {
            Fecha: txtFecha,
            LineaNegocio: 'ENLATADO',
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



function SeleccionarControlDetalleConsumo(model) {
    
    $("#btnAtras").prop("hidden", false);
    $("#btnImprimir").prop("hidden", false);
    //$("#btnModalEliminar").prop("hidden", false);
    //$("#btnModalGenerar").prop("hidden", true);
    //$("#btnModalEditar").prop("hidden", false);
    $("#txtFecha").prop("disabled", true);
    $("#selectTurno").prop("disabled", true);    
    $("#divCabecera1").prop("hidden", true);
    $("#divCabecera2").prop("hidden", true);
    $("#divImpresion").prop("hidden", false);
    //if ($("#txtLineaNegocio").val() == "ENLATADO") {
    //    CargarProcesoDetalleEnlatado();
    //} else {
    //    CargarProcesoDetallePouch();
    //}
    //CargarProcesoDetalleDaniado();
    //CargarProcesoDetalleTiemposMuertos();
    //CargarProcesoDetalleAditivos();
    //ConsultarConsultarAditivos();
}



function AtrasControlPrincipal() {
    $("#txtFecha").prop("disabled", false);
    $("#selectTurno").prop("disabled", false);
    $("#btnAtras").prop("hidden", true); 
    $("#btnImprimir").prop("hidden", true); 

    $("#divCabecera2").prop("hidden", false);
    $("#divImpresion").prop("hidden", true);
    CargarControlConsumo();
}

// Code goes here
function printDiv() {
    var contenido = document.getElementById('prueba').innerHTML;
    var contenidoOriginal = document.body.innerHTML;
    document.body.innerHTML = contenido;
    window.print();
    document.body.innerHTML = contenidoOriginal;
}