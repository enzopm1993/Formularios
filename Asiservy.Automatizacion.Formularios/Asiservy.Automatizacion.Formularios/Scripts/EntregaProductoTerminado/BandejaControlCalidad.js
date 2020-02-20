﻿$(document).ready(function () {
    CargarProductoTerminado();
    $("#txtIdControl").val('0');

});

function CargarProductoTerminado() {
    $("#chartCabecera2").html('');
    if ($("#txtFechaPaletizado").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
  
   // CargarOrdenFabricacion();
    $.ajax({
        url: "../EntregaProductoTerminado/BandejaControlCalidadPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFechaPaletizado").val()           

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

function SeleccionarBandeja(model) {
   // console.log(model);

    $.ajax({
        url: "../EntregaProductoTerminado/ConsultarBodegas",
        type: "GET",
        data: {
            OF: model.OrdenFabricacion
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para esta OF.");
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros.");
            } else {
                // console.log(resultado.UnidadesControlCalidad);
                $("#txtControlCalidad").val(resultado.UnidadesControlCalidad);
                $("#txtRechazadas").val(resultado.UnidadesRechazadas);
                $("#txtReproceso").val(resultado.UnidadesReproceso);
                $("#txtDefectos").val(resultado.UnidadesConDefecto);
                $("#txtEntregadas").val(resultado.CajasEntregadas);
                $("#txtOrdenFrabricacion").val(model.OrdenFabricacion);
                $("#txtProducto").val(model.Producto);
                $("#txtIdControl").val(model.IdProductoTerminado);
                
                $("#ModalApruebaProductoTerminado").modal("show");
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoMaterial").prop("hidden", true);
        }
    });
  

}


function AprobarEntregaProductoTermiando() {
    $.ajax({
        url: "../EntregaProductoTerminado/ApruebaEntregaProductoTerminado",
        type: "GET",
        data: {
            IdControl: $("#txtIdControl").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);  
            CargarProductoTerminado();
            $("#txtIdControl").val('0');

            $("#ModalApruebaProductoTerminado").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);           
        }
    });
}