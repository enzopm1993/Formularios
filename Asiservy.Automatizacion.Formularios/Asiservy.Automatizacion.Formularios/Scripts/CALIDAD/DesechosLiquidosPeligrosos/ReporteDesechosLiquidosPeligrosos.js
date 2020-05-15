var mes = [ "","ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"];
$(document).ready(function () {
    ComboAnio();
    CargarCabecera(3);    
});

function ComboAnio() {
    var date = new Date();
    var n = date.getFullYear();
    var select = document.getElementById("selectAnio");
    for (var i = n; i >= 2015; i--) {
        select.options.add(new Option(i, i));
    }
}

function CargarCabecera() {
    Atras();
    $('#lblMostrarFecha').text('');
    $('#lblMostrarHora').text('');
    $('#lblMostrarObservacion').text('');
    $('#cargac').show();   

    $.ajax({
        url: "../DesechosLiquidosPeligrosos/ReporteDesechosLiquidosPeligrososCabeceraPartial",
        data: {
            anioBusqueda: $("#selectAnio").val(),           
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarCabecera(jdata) {
    $('#cargac').show();
    var op = 0;
    var mesLetras = mes[parseInt(moment(jdata.FechaMES).format('MM'))];
    var anio = moment(jdata.FechaMES).format('YYYY');
    $('#lblMostrarFecha').text(mesLetras+' - '+anio);
    $('#txtUsuarioCreacion').text('\u00a0' + jdata.UsuarioIngresoLog.toUpperCase());
    $('#txtFechaCreacion').text('\u00a0' + moment(jdata.FechaIngresoLog).format('DD-MM-YYYY'));
    if (jdata.AprobadoPor == null) {
        jdata.AprobadoPor = '';
    }

    if (jdata.FechaAprobacion != null) {
        jdata.FechaAprobacion = moment(jdata.FechaAprobacion).format('DD-MM-YYYY');
    } else if (jdata.FechaAprobacion == null) {
        jdata.FechaAprobacion = '';
    }
    $('#txtUsuarioAprobacion').text('\u00a0' + jdata.AprobadoPor);
    $('#txtFechaAprobacion').text('\u00a0' + jdata.FechaAprobacion);
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/ReporteDesechosLiquidosPeligrososPartial",//MUESTRO EL DETALLE DE LA FILA SELECCIONADA
        data: {
            anioBusqueda: $("#selectAnio").val(),
            mesBusqueda: 0,
            idDesechosLiquidos: jdata.IdDesechosLiquidos,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divBotones').prop('hidden', true);
                $("#divMostarTablaCabecera").prop('hidden', false);
                $("#divCardMostrarDetalle").prop('hidden', true);
                MensajeAdvertencia('No existen registro de DETALLE');
            } else {
                $("#divMostarTablaCabecera").prop('hidden', true);
                $("#divCardMostrarDetalle").prop('hidden', false);
                $('#divBotones').prop('hidden', false);
                $("#divMostarTablaDetalle").html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function printDiv() {
    window.print();
}

function Atras() {
    $('#cargac').show();
    $('#divBotones').prop('hidden', true);
    $("#divMostarTablaCabecera").prop('hidden', false);
    $("#divCardMostrarDetalle").prop('hidden', true);
    $("#divMostarTablaDetalle").html('');

    $('#cargac').hide();

}

