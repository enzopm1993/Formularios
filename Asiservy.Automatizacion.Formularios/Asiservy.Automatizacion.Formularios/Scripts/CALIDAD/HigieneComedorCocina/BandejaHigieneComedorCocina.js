var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
});

//CARGAR BANDEJA
function CargarBandeja() {
    $('#cargac').show();
    var estadoReporte = $('#selectEstadoReporte').val();
    $.ajax({
        url: "../HigieneComedorCocina/BandejaHigieneComedorCodinaPartial",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
           estadoReporte: estadoReporte
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para este model.");
            }
            if (resultado == "0") {
                //MensajeAdvertencia("No existen registros.");
                $("#divTablaAprobados").html("No existen registros: " + resultado);
            } else {
                $("#btnPendiente").prop("hidden", true);
                $("#btnAprobado").prop("hidden", false);
                $("#divTablaAprobados").show();
                $("#divTablaAprobados").html(resultado);
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
    var date = new Date();
    $('#txtFechaAprobado').val(moment(date).format('YYYY-MM-DDTHH:mm'));
    $('#cargac').show();
    listaDatos = model;
    if (model.EstadoReporte == true) {
        $('#txtFechaAprobado').prop('hidden', true);
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#txtFechaAprobado').prop('hidden', false);        
        $('#btnPendiente').prop('hidden', true);
        $('#btnAprobado').prop('hidden', false);
    }
    var op = 0;
    var ocultar = 1;
    $.ajax({
        url: "../HigieneComedorCocina/HigieneComedorCocinaDetallePartial",
        type: "GET",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            idControlHigiene: listaDatos.IdControlHigiene,
            op: op,
            ocultar:ocultar
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia('¡No existe detalle para este registro!');
                $("#divTblAprobarPendiente").html("No existen registros");
            } else {                
                $("#tblAprobarPendientePartial").html('');
                $("#ModalApruebaPendiente").modal("show");
                $("#divAprobarPendientePartial").html(resultado);
                //ConsultarFirma();               
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function AprobarPendiente(estadoReporte) {
    var date = new Date();
    if ($("#selectEstadoRegistro").val() == 'false') {
        if (moment($('#txtFechaAprobado').val()).format('') < moment(listaDatos.FechaIngresoLog).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser menor a la fecha de creacion del reporte: <span class="badge badge-danger">' + moment(listaDatos.FechaIngresoLog).format('DD-MM-YYYY') + '</span>');
            return;
        }
        if ($('#txtFechaAprobado').val() > moment(date).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser mayor a la fecha actual: <span class="badge badge-danger">' + moment(date).format('DD-MM-YYYY') + '</span>');
            return;
        }
    } else { $('#txtFechaAprobado').val(''); }
    var siAprobar = 1;
    $.ajax({
        url: "../HigieneComedorCocina/GuardarModificarHigieneControl",
        type: "POST",
        data: {
            IdControlHigiene: listaDatos.IdControlHigiene,
            EstadoReporte: estadoReporte,
            FechaAprobado: $('#txtFechaAprobado').val(),
            siAprobar: siAprobar
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado==2) {
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else {
                MensajeError('El registro no debe guardarse- solo actualizarce- Controller: GuardarModificarControlCuchilloPreparacion');
                return;
            }            
            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function validar() {
    if ($('#txtFechaAprobado').val() == '') {
        $("#txtFechaAprobado").css('border', '1px dashed red');
        MensajeAdvertencia('Fecha invalida');
        return;
    } else {
        $("#txtFechaAprobado").css('border', '');
    }
}
//function GuardarFirma() {
//    if (!signaturePad.isEmpty()) {
//        var canvas = document.getElementById("firmacanvas");
//        var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', '');
//        var formData = new FormData();
//        formData.append('image', image);
//        formData.append('idControlHigiene', listaDatos.IdControlHigiene);
//        $.ajax({
//            type: 'POST',
//            url: '/HigieneComedorCocina/BandejaGuardarImagenFirma',
//            data: formData,
//            processData: false,
//            contentType: false,
//            success: function (resultado) {
//                ClearPAd();
//                if (resultado == "101") {
//                    window.location.reload();
//                }
//                if (resultado != 0) {
//                    $('#div_ImagenFirma').prop('hidden', false);
//                    document.getElementById('ImgFirma').src = resultado;
//                    $('#signature-pad').prop('hidden', true);
//                    MensajeCorrecto("Firma ingresada Correctamente");
//                } else {
//                    MensajeAdvertencia('¡Error al guardar la Firma: !' + listaDatos.IdControlHigiene);
//                }
//            }
//        });
//    }
//}

//function VolverAFirmar() {
//    $('#div_ImagenFirma').prop('hidden', true);
//    $('#signature-pad').prop('hidden', false);
//}

//function ConsultarFirma() {
//    ClearPAd();
//    $.ajax({
//        url: "../HigieneComedorCocina/BandejaConsultarImagenFirma",
//        type: "GET",
//        data: {
//            idControlHigiene: listaDatos.IdControlHigiene
//        },
//        success: function (resultado) {
//            $("#btnGuardarFirma").prop("hidden", true);
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (resultado != '0') {
//                document.getElementById('ImgFirma').src = resultado;
//                $('#div_ImagenFirma').prop('hidden', false);
//                $("#btnActualizarFirma").prop("hidden", false);
//                $('#signature-pad').prop('hidden', true);
//            } else {
//                $('#signature-pad').prop('hidden', false);
//                $('#div_ImagenFirma').prop('hidden', true);
//                //$('#firmaDigital').prop('hidden', true);
//            }
//            if (listaDatos.EstadoReporte == true) {
//                $("#btnActualizarFirma").prop("hidden", true);
//                $("#signature-pad").prop("hidden", true);
//            }
//        },
//        error: function (resultado) {
//            MensajeError("Error: Comuníquese con sistemas", false);
//        }
//    });
//}

//function ClearPAd() {
//    signaturePad.clear();
//}

//BEGIN SIGNATURE API
//var clearButton = wrapper.querySelector("[data-action=clear]");
//var changeColorButton = wrapper.querySelector("[data-action=change-color]");
//var undoButton = wrapper.querySelector("[data-action=undo]");
//var savePNGButton = wrapper.querySelector("[data-action=save-png]");
//var saveJPGButton = wrapper.querySelector("[data-action=save-jpg]");
//var saveSVGButton = wrapper.querySelector("[data-action=save-svg]");

//var canvas = document.querySelector("canvas");

//var signaturePad = new SignaturePad(canvas);
//signaturePad.on();

//function download(dataURL, filename) {
//    if (navigator.userAgent.indexOf("Safari") > -1 && navigator.userAgent.indexOf("Chrome") === -1) {
//        window.open(dataURL);
//    } else {
//        var blob = dataURLToBlob(dataURL);
//        var url = window.URL.createObjectURL(blob);

//        var a = document.createElement("a");
//        a.style = "display: none";
//        a.href = url;
//        a.download = filename;

//        document.body.appendChild(a);
//        a.click();

//        window.URL.revokeObjectURL(url);
//    }
//}

// One could simply use Canvas#toBlob method instead, but it's just to show
// that it can be done using result of SignaturePad#toDataURL.
//function dataURLToBlob(dataURL) {
//    // Code taken from https://github.com/ebidel/filer.js
//    var parts = dataURL.split(';base64,');
//    var contentType = parts[0].split(":")[1];
//    var raw = window.atob(parts[1]);
//    var rawLength = raw.length;
//    var uInt8Array = new Uint8Array(rawLength);

//    for (var i = 0; i < rawLength; ++i) {
//        uInt8Array[i] = raw.charCodeAt(i);
//    }

//    return new Blob([uInt8Array], { type: contentType });
//}

//clearButton.addEventListener("click", function (event) {
//    signaturePad.clear();
//});
//END SIGNATURE API


//DATE RANGE PICKER
$(function () {
    var start = moment();
    var end = moment();
    var mesesLetras = {
        '01': "Enero",
        '02': "Febrero",
        '03': "Marzo",
        '04': "Abril",
        '05': "Mayo",
        '06': "Junio",
        '07': "Julio",
        '08': "Agosto",
        '09': "Septiembre",
        '10': "Octubre",
        '11': "Noviembre",
        '12': "Diciembre"
    }

    function cb(start, end) {
        var fechaMuestraDesde = mesesLetras[start.format('MM')] + ' ' + start.format('D') + ', ' + start.format('YYYY');
        var fechaMuestraHasta = mesesLetras[end.format('MM')] + ' ' + end.format('D') + ', ' + end.format('YYYY');
        $("#fechaDesde").val(start.format('YYYY-MM-DD'));
        $("#fechaHasta").val(end.format('YYYY-MM-DD'));

        $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 60
        },
        minDate: moment("01/10/2019", "DD/MM/YYYY"),
        maxDate: moment(),
        ranges: {
            'Hoy': [moment(), moment()],
            'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Últimos 7 días': [moment().subtract(6, 'days'), moment()],
            'Últimos 30 días': [moment().subtract(29, 'days'), moment()],
            'Mes actual (hasta hoy)': [moment().startOf('month'), moment()],
            'Último mes': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "fromLabel": "De",
            "toLabel": "a",
            "customRangeLabel": "Personalizada",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        }
    }, cb);
    cb(start, end);
});
