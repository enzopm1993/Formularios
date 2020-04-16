﻿var ListaDatos = [];

$(document).ready(function () {
    CargarCabecera(0);    
});

function CargarCabecera(opcion) {
    $('#cargac').show(); 
    var op = opcion;
    if ($("#txtFecha").val() == '') {
        return;
    } else {
        $.ajax({
            url: "../LavadoDesinfeccionManos/ConsultarControlLavadoDesinfeccionManos",
            type: "GET",
            data: {
                fechaDesde: $("#txtFecha").val(),
                fechaHasta: $("#txtFecha").val(),
                opcion: op
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#txtObservacion").val('');
                if (resultado.length == 0) {        
                    $("#divObservacion").hide();
                    $("#txtObservacion").prop("hidden", true);
                    //ListaDatos = resultado;
                    ListaDatos.IdDesinfeccionManos = 0;
                } else {  
                    $("#divObservacion").show();
                    $("#txtObservacion").prop("hidden", false);
                    $("#txtObservacion").val(resultado[0].Observacion);
                    ListaDatos = resultado;  
                }
                CargarDetalle(0);
                setTimeout(function () {
                    $('#cargac').hide();
                }, 200);                
            },
            error: function (resultado) {
                $('#cargac').hide();
                MensajeError(resultado.responseText, false);
            }
        });
    }
}

//Retorna PartialView
function CargarDetalle(opcion) {      
    var op = opcion;
    //if (ListaDatos.IdDesinfeccionManos =='undefined')  
    //$("#divTableEntregaProductoDetalle").html('');
    $.ajax({
        url: "../LavadoDesinfeccionManos/ReporteDesinfeccionManosDetallePartial",
        type: "GET",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            opcion: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {                
                $("#divTableEntregaProductoDetalle").html("No existen registros");
            } else {
                $("#divTableEntregaProductoDetalle").prop("hidden", false);
                $("#divTableEntregaProductoDetalle").html(resultado);
                $("#divDetalleControlCloro").prop("hidden", false);                
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}


