var listaDatos = [];
$(document).ready(function () {
    ComboAnio();
    CargarBandeja();
    $('#tblDataTableDetalle tbody').on('click', 'tr', function () {
        var table = $('#tblDataTableDetalle').DataTable();
        var dataCabecera = table.row(this).data();
        SeleccionarBandeja(dataCabecera);
    });
});

function ComboAnio() {
    var n = (new Date()).getFullYear()
    var select = document.getElementById("selectAnio");
    for (var i = n; i >= 2015; i--) {
        select.options.add(new Option(i, i));
    }
}

//CARGAR BANDEJA
function CargarBandeja() {
    $('#cargac').show();
    var table = $("#tblDataTableDetalle");
    table.DataTable().clear();
    table.DataTable().destroy();
    table.DataTable().draw();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/BandejaConsultarDesechosLiquidosPeligrosos",
        data: {
            anioBusqueda: $('#selectAnio').val(),
            estadoReporte: $('#selectEstadoReporte').val()            
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para este model.");
            }
            if (resultado == "0") {
                $("#divTablaAplrobados").html("No existen registros: " + resultado);
            } else {
                $("#btnPendiente").prop("hidden", true);
                $("#btnAprobado").prop("hidden", false);
                $("#divTablaAplrobados").show();
                $("#tblDataTableDetalle tbody").empty();
                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'FechaMES' },
                    { data: 'FechaIngresoLog' },
                    { data: 'UsuarioIngresoLog' },
                    { data: 'EstadoReporteControl' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                $('#cargac').hide();
                var conRow = 0;
                resultado.forEach(function (row) {
                    var estado = 'PENDIENTE';
                    var css = 'badge-danger';
                    row.FechaMES = moment(row.FechaMES).format('MM');
                    row.FechaIngresoLog = moment(row.FechaIngresoLog).format('DD-MM-YYYY');
                    if (row.EstadoReporte == true) {
                        estado = 'APROBADO';
                        css = 'badge-success';
                    }
                    resultado[conRow].EstadoReporteControl = "<center><span class='badge " + css + "' >" + estado + "</span></center>";//Aplico estrilos al estadoReporte
                    conRow++;
                });
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
            }
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

function SeleccionarBandeja(model) {
    $('#cargac').show();
    listaDatos = model;
    if (model.EstadoReporte == true) {
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#btnPendiente').prop('hidden', true);
        $('#btnAprobado').prop('hidden', false);
    }
    var op = 0;
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/BandejaDesechosLiquidosPeligrososPartial",
        type: "GET",
        data: {
            anioBusqueda: $('#selectAnio').val(),
            mesBusqueda: model.FechaMES,
            idDesechosLiquidos: model.IdDesechosLiquidos,
            op: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia('¡Este registro no contiene detalle¡');
            } else {
                $("#divTblAprobarPendiente").html('');
                $("#ModalApruebaPendiente").modal("show");
                $("#divTblAprobarPendiente").html(resultado);
                //ocultarBotones();
            }
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

function AprobarPendiente(estadoReporte) {
    var siAprobar = 1;//en la condicion del la clase clsD se envia a actualizar solo la columna EstadoReporte 
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/BandejaGuardarModificarDesechosLiquidos",
        type: "POST",
        data: {
            idDesechosLiquidos: listaDatos.IdDesechosLiquidos,            
            EstadoReporte: estadoReporte,
            siAprobar: siAprobar
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 1) {
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else { MensajeError('El registro no debe guardase- solo actualizarce- Controller: GuardarModificarControlCuchilloPreparacion'); }

            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

