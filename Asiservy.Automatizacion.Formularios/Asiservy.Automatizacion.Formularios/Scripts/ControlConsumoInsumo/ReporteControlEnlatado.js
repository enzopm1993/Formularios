﻿//var idControl = 0;
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

function CargarDatosOrdenFabricacion(orden) {
  
    $.ajax({
        url: "../ControlConsumoInsumo/ConsultarDatosOrdenFabricacion",
        type: "GET",
        data: {
            Orden: orden
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

            console.log(resultado);
            //console.log(resultado);
            $("#txtDestino").html(resultado.DESTINO);
            $("#txtOrdenVenta").html(parseInt(resultado.PEDIDO_VENTA));
            //$("#txtMarca").html(resultado.MARCA);
            if (resultado.CLIENTE_CORTO != '') {
                $("#txtCliente").html(resultado.CLIENTE_CORTO);
            } else {
                $("#txtCliente").html(resultado.CLIENTE);
            }

            if (resultado.NOMBRE_ADICIONAL != '') {
                $("#txtProducto").html(resultado.NOMBRE_ADICIONAL);
            } else {
                $("#txtProducto").html(resultado.NOMBRE_PRODUCTO);
            }

            $("#txtEnvase").html(resultado.ENVASE);
            $("#txtTapa").html(resultado.TAPA);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            NuevoControlConsumoInsumos();
        }
    });

}

function SeleccionarControlDetalleConsumo(model) {
   // idControl = model.IdControlConsumoInsumos;
    ListadoControl = model;
    CargarDatosOrdenFabricacion(model.OrdenFabricacion);
    $("#txtOrdenFabricacion").html(ListadoControl.OrdenFabricacion);
    $("#txtPesoNeto").html(ListadoControl.PesoNeto);
    $("#txtPesoEscurrido").html(ListadoControl.PesoEscrundido);
    $("#txtLomo").html(ListadoControl.Lomo);
    $("#txtMiga").html(ListadoControl.Miga);
    $("#txtAceite").html(ListadoControl.Aceite);
    $("#txtAgua").html(ListadoControl.Agua);
    $("#txtCaldoVegetal").html(ListadoControl.CaldoVegetal);
   // $("#txtOrdenVenta").html(ListadoControl.OrdenVenta);
   
  

    if ($("#selectTurno").val() == 1) {
        $("#txtTurno").html("A");
    } else {
        $("#txtTurno").html("B");
    }
    $("#txtFecha2").html($("#txtFecha").val());
    //console.log(ListadoControl.HoraInicio);
    //console.log(moment(ListadoControl.HoraInicio).format("HH:MM"));

    //var horaInicio = moment(ListadoControl.HoraInicio).format("HH:mm");
    //var horaFin = moment(ListadoControl.HoraFin).format("HH:mm");
    //$("#txtHoras").html(horaInicio + " - " + horaFin);
    


    $("#txtDesperdicioSolido").html(ListadoControl.DesperdicioSolido);
    $("#txtDesperdicioLiquido").html(ListadoControl.DesperdicioLiquido);
    $("#txtDesperdicioAceite").html(ListadoControl.DesperdicioAceite);
    //$("#txtEmpleados").val(ListadoControl.Empleados);
    //$("#txtCajas").val(ListadoControl.Cajas);
    $("#txtRecibidos").html(ListadoControl.UnidadesRecibidas);
    //$("#txtSobrantes").val(ListadoControl.UnidadesSobrantes);
    $("#txtProducido").html(ListadoControl.UnidadesProducidas);
    $("#txtCodigoProducto").html(ListadoControl.CodigoProducto);
    $("#txtObservaciones").html(ListadoControl.Observacion);

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
    $("#divAcciones").prop("hidden", false);

    //if ($("#txtLineaNegocio").val() == "ENLATADO") {
    CargarProcesoDetalleCuerpo();
    CargarProcesoDetalleTapa();
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


function CargarProcesoDetalleCuerpo() {
    //$("#spinnerCargandoDetalleL").prop("hidden", false);
    $("#divTableDetalleCuerpo").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteInsumoDetalleEnlatadoPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos,
            Tipo: 'C'
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalleCuerpo").html("No existen registros");            
            } else {            
                $("#divTableDetalleCuerpo").html(resultado);
            }
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
      //      $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}


function CargarProcesoDetalleTapa() {  
    $("#divTableDetalleTapa").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteInsumoDetalleEnlatadoPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos,
            Tipo: 'T'
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalleTapa").html("No existen registros");               
            } else {
                $("#divTableDetalleTapa").html(resultado);               
            }
     //       $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        //    $('#btnConsultar').prop("disabled", false);
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


function ReporteConsumoInsumoDetalle() {
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
            } else {
                var horas='';
                $.each(resultado, function (index, value) {
                    horas = horas + moment(value.HoraInicio).format("HH:mm") + '-' + moment(value.HoraFin).format("HH:mm")+'; ' 
                });               
                $("#txtHoras").html(horas);            
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);         
         //   $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function ReporteDaniadoPartial() {
    $("#divTableDetalleDaniado").html('');
    $.ajax({
        url: "../ControlConsumoInsumo/ReporteDaniadoPartial",
        type: "GET",
        data: {
            IdControl: ListadoControl.IdControlConsumoInsumos,
            Tipo:"ENLATADO"

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