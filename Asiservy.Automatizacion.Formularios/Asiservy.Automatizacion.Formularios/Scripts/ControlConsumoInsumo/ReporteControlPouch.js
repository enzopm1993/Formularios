//var idControl = 0;
var ListadoControl = [];

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
            LineaNegocio: 'POUCH',
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
    // idControl = model.IdControlConsumoInsumos;
    ListadoControl = model;

    $("#txtOrdenFabricacion").html(ListadoControl.OrdenFabricacion);
    $("#txtPesoNeto").html(ListadoControl.PesoNeto);
    $("#txtPesoEscurrido").html(ListadoControl.PesoEscrundido);
    $("#txtLomo").html(ListadoControl.Lomo);
    $("#txtMiga").html(ListadoControl.Miga);
    $("#txtAceite").html(ListadoControl.Aceite);
    $("#txtAgua").html(ListadoControl.Agua);
    $("#txtCaldoVegetal").html(ListadoControl.CaldoVegetal);
    $("#txtOrdenVenta").html(ListadoControl.OrdenVenta);
    $("#txtCliente").html(ListadoControl.Cliente);
    $("#txtMarca").html(ListadoControl.Marca);
    $("#txtDestino").html(ListadoControl.Destino);
    $("#txtProducto").html(ListadoControl.Producto);
    $("#txtEnvase").html(ListadoControl.Envase);
    $("#txtTapa").html(ListadoControl.Tapa);

    if ($("#selectTurno").val() == 1) {
        $("#txtTurno").html("A");
    } else {
        $("#txtTurno").html("B");
    }
    $("#txtFecha2").html($("#txtFecha").val());
      $("#txtDesperdicioSolido").html(ListadoControl.DesperdicioSolido);
    $("#txtDesperdicioLiquido").html(ListadoControl.DesperdicioLiquido);
    $("#txtDesperdicioAceite").html(ListadoControl.DesperdicioAceite);
    $("#txtRecibidos").html(ListadoControl.UnidadesRecibidas);
     $("#txtProducido").html(ListadoControl.UnidadesProducidas);
    $("#txtCodigoProducto").html(ListadoControl.CodigoProducto);
    $("#txtObservaciones").html(ListadoControl.Observacion);
    $("#btnAtras").prop("hidden", false);
    $("#btnImprimir").prop("hidden", false);
    $("#txtFecha").prop("disabled", true);
    $("#selectTurno").prop("disabled", true);
    $("#divCabecera1").prop("hidden", true);
    $("#divCabecera2").prop("hidden", true);
    $("#divImpresion").prop("hidden", false);
    $("#divAcciones").prop("hidden", false);

    //if ($("#txtLineaNegocio").val() == "ENLATADO") {
    ReporteInsumoDetallePouchPartial();
    //CargarProcesoDetalleTapa();
    //} else {
    //    CargarProcesoDetallePouch();
    //}
    ReporteConsumoAditivos();
    ReporteDaniadoPartial();
    ReporteTiemposMuertos();
    DatosDelProcesoEnlatado();
    ReporteProcedencia();
    ReporteConsumoInsumoDetalle();

    //ConsultarConsultarAditivos();
}



function AtrasControlPrincipal() {
    $("#txtFecha").prop("disabled", false);
    $("#selectTurno").prop("disabled", false);
    $("#btnAtras").prop("hidden", true);
    $("#btnImprimir").prop("hidden", true);

    $("#divCabecera1").prop("hidden", false);
    $("#divCabecera2").prop("hidden", false);
    $("#divImpresion").prop("hidden", true);
    $("#divAcciones").prop("hidden", true);
    ListadoControl = [];
    CargarControlConsumo();
}


function ReporteInsumoDetallePouchPartial() {
    //$("#spinnerCargandoDetalleL").prop("hidden", false);
    $("#divTableDetalleFundas").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteInsumoDetallPouchPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalleFundas").html("No existen registros");
            } else {
                $("#divTableDetalleFundas").html(resultado);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            //      $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}


function ReporteConsumoInsumoDetalle() {
    //$("#spinnerCargandoDetalleL").prop("hidden", false);
    //$("#divTableDetalleFundas").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ConsultaDetalleConsumoInsumo",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#txtHoras").html("No existen registros");
                //$("#divTableDetalleFundas").html("No existen registros");
            } else {
                console.log(resultado);
                //$("#divTableDetalleFundas").html(resultado);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            //      $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function ReporteConsumoAditivos() {
    $("#divTableDetalleConsumoAditivo").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteConsumoAditivos",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalleConsumoAditivo").html("No existen registros");
            } else {
                $("#divTableDetalleConsumoAditivo").html(resultado);
            }
            //      $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            //        $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}



function ReporteDaniadoPartial() {
    $("#divTableDetalleDaniado").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteDaniadoPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalleDaniado").html("No existen registros");
            } else {
                $("#divTableDetalleDaniado").html(resultado);
            }
            //     $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            //    $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}


function ReporteTiemposMuertos() {
    $("#divTableDetalleTiemposMuertos").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteTiemposMuertos",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalleTiemposMuertos").html("No existen registros");
            } else {
                $("#divTableDetalleTiemposMuertos").html(resultado);
            }
            //   $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            //     $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function ReporteProcedencia() {
    $("#divTableDetalleProcedencia").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteProcedencia",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalleProcedencia").html("No existen registros");
            } else {
                $("#divTableDetalleProcedencia").html(resultado);
            }
            // $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            //   $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function DatosDelProcesoEnlatado() {
    $.ajax({
        url: "../ControlConsumoInsumo/DatosDelProcesoEnlatado",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#txtTiempoMuertoProceso").html(resultado.MinutoProceso);
            $("#txtTiempoMuertoMantenimiento").html(resultado.MinutoMantenimiento);
            $("#txtCajasHora").html(resultado.CajaHora);
            $("#txtCajasMinutos").html(resultado.CajaMinuto);
            $("#txtTotalCajas").html(resultado.TotalCajas);
            $("#txtGrsXLata").html(resultado.Grs);
            $("#txtPersonal").html(resultado.Personal);
            $("#txtSaldo").html(resultado.Saldo);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

// Code goes here
function printDiv() {
    var contenido = document.getElementById('divImpresion').innerHTML;
    var contenidoOriginal = document.body.innerHTML;
    document.body.innerHTML = contenido;
    window.print();
    document.body.innerHTML = contenidoOriginal;
}