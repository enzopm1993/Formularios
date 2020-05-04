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
                ConsultarFirma();
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
        GuardarFirma();
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
                $("#ModalApruebaPendiente").modal("hide");
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else { MensajeError('El registro no debe guardase- solo actualizarce- Controller: GuardarModificarControlCuchilloPreparacion'); return; }      
            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarFirma() {   
    if (!signaturePad.isEmpty()) {   
        var canvas = document.getElementById("firmacanvas");
        var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', '');
        var formData = new FormData();
        formData.append('image', image);
        formData.append('idDesechosLiquidos', listaDatos.IdDesechosLiquidos);
        $.ajax({
            type: 'POST',
            url: '/DesechosLiquidosPeligrosos/BandejaGuardarImagenFirma',
            data: formData,
            processData: false,
            contentType: false,
            success: function (resultado) {
                ClearPAd();
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado != 0) {
                    $('#div_ImagenFirma').prop('hidden', false);
                    document.getElementById('ImgFirma').src = resultado;
                    $('#signature-pad').prop('hidden', true);
                    MensajeCorrecto("Firma ingresada Correctamente");
                } else {
                    MensajeAdvertencia('¡Error al guardar la Firma: !' + listaDatos.IdDesechosLiquidos);
                }
            }
        });
    }
}

function VolverAFirmar() {
    $('#div_ImagenFirma').prop('hidden', true);
    $('#signature-pad').prop('hidden', false);
}

function ConsultarFirma() {
    ClearPAd();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/BandejaConsultarImagenFirma",
        type: "GET",
        data: {
            idDesechosLiquidos: listaDatos.IdDesechosLiquidos
        },
        success: function (resultado) {
            $("#btnGuardarFirma").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '0') {
                document.getElementById('ImgFirma').src = resultado;
                $('#div_ImagenFirma').prop('hidden', false);
                $("#btnActualizarFirma").prop("hidden", false);
                $('#signature-pad').prop('hidden', true);
            } else {
                $('#signature-pad').prop('hidden', false);
                $('#div_ImagenFirma').prop('hidden', true);
            }
            if (listaDatos.EstadoReporte == true) {
                $("#btnActualizarFirma").prop("hidden", true);
                $("#signature-pad").prop("hidden", true);               
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function ClearPAd() {
    signaturePad.clear();
}

//BEGIN SIGNATURE API
var clearButton = wrapper.querySelector("[data-action=clear]");
//var changeColorButton = wrapper.querySelector("[data-action=change-color]");
//var undoButton = wrapper.querySelector("[data-action=undo]");
//var savePNGButton = wrapper.querySelector("[data-action=save-png]");
//var saveJPGButton = wrapper.querySelector("[data-action=save-jpg]");
//var saveSVGButton = wrapper.querySelector("[data-action=save-svg]");

var canvas = document.querySelector("canvas");

var signaturePad = new SignaturePad(canvas);
signaturePad.on();

function download(dataURL, filename) {
    if (navigator.userAgent.indexOf("Safari") > -1 && navigator.userAgent.indexOf("Chrome") === -1) {
        window.open(dataURL);
    } else {
        var blob = dataURLToBlob(dataURL);
        var url = window.URL.createObjectURL(blob);

        var a = document.createElement("a");
        a.style = "display: none";
        a.href = url;
        a.download = filename;

        document.body.appendChild(a);
        a.click();

        window.URL.revokeObjectURL(url);
    }
}

// One could simply use Canvas#toBlob method instead, but it's just to show
// that it can be done using result of SignaturePad#toDataURL.
function dataURLToBlob(dataURL) {
    // Code taken from https://github.com/ebidel/filer.js
    var parts = dataURL.split(';base64,');
    var contentType = parts[0].split(":")[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;
    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}

clearButton.addEventListener("click", function (event) {
    signaturePad.clear();
});



//END SIGNATURE API

