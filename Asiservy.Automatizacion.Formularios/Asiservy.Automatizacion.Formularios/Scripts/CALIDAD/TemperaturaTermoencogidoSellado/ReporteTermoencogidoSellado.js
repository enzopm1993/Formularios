var ListaDatos = [];
var ListaDatosDetalle = [];
var horaActualizar = '';//0=No, 1=Yes

$(document).ready(function () {
    CargarCabecera(0);  
    
});


function CargarCabecera(opcion) {
    MostrarModalCargando();
    var op = opcion;
    if ($("#txtFecha").val() == '') {
        CerrarModalCargando();
        return;
    } else {
        $.ajax({
            url: "../TemperaturaTermoencogidoSellado/ConsultarTermoencogidoSellado",
            type: "GET",
            data: {
                fecha: $("#txtFecha").val(),
                opcion: op
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#txtObservacion").val('');
                if (resultado == "0") {
                    $("#txtObservacion").prop("disabled", false);
                    $("#divDetalleControlCloro").prop("hidden", true);
                    $("#btnModalEditar").prop("hidden", true);
                    $("#btnModalEliminar").prop("hidden", true);
                    $("#btnModalGenerar").prop("hidden", false);
                    ListaDatos = [];
                } else {
                    $("#divDetalleControlCloro").prop("hidden", false);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalEditar").prop("hidden", false);
                    $("#btnModalEliminar").prop("hidden", false);
                    $("#txtObservacion").prop("disabled", true);
                    $("#txtObservacion").val(resultado.Observacion);   
                    ListaDatos = resultado;
                }
                CargarDetalle(0);
                setTimeout(function () {
                    CerrarModalCargando();
                }, 500);
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
                CerrarModalCargando();
            }
        });
    }
}
//Retorna PartialView
function CargarDetalle(opcion) {
    var op = opcion;
    if (ListaDatos.length == 0) {
        ListaDatos.Id = 0;
    }
    if (ListaDatosDetalle.length == 0) {
        ListaDatosDetalle.Id = 0;
    }
    $("#divTableEntregaProductoDetalle").html('');
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/ControlTermoencogidoSelladoPartial",
        type: "GET",
        data: {
            id: ListaDatosDetalle.Id,
            idCabecera: ListaDatos.Id,
            op: op
        },
        success: function (resultado) {
            ListaDatosDetalle = [];
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableEntregaProductoDetalle").html("No existen registros");
            } else {
                $("#divTableEntregaProductoDetalle").prop("hidden", false);
                $("#divTableEntregaProductoDetalle").html(resultado);
                $("#divDetalleControlCloro").prop("hidden", false);
                //Oculto la columna Acciones
                var oTable = $('#tblDataTable').DataTable();
                var info = oTable.page.info();
                var count = info.recordsTotal;
                for (var i = 0; i < count; i++) {
                    document.getElementById("tdAccionesBody_" + i).style.display = "none";
                }                
                document.getElementById("tdAccionesHead").style.display = "none";                
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            CerrarModalCargando();
        }
    });
}