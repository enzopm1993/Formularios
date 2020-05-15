var Error = 0;
var IdControlAp;
$(document).ready(function () {
    CargarBandeja();

});
function CargarBandeja() {
    $('#cargac').show();
    if ($("#cmbEstadoControl").val() == 'false') {
        $("#divDateRangePicker").prop('hidden', true);
        $.ajax({
            url: "../EvaluacionProductoEnfundado/BandejaAprobadosEvaluacionProductoEnfundadoPartial",
            type: "GET",
            data: {
                EstadoControl: false
            },
            success: function (resultado) {
                $('#DivEvaluacionLomosMigas').empty();
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                } else {
                    $('#MensajeRegistros').hide();

                    $('#DivEvaluacionLomosMigas').html(resultado);
                    config.opcionesDT.pageLength = 30;
                    $('#tblBandejaEvaluacion').DataTable(config.opcionesDT);
                }

                $('#cargac').hide();

                //$("#btnPendiente").prop("hidden", false);
                //$("#btnAprobado").prop("hidden", true);


            },
            error: function (resultado) {
                $('#cargac').hide();
                MensajeError(resultado.responseText, false);
            }
        });
    } else {
        $.ajax({
            url: "../EvaluacionProductoEnfundado/BandejaAprobadosEvaluacionProductoEnfundadoPartial",
            type: "GET",
            data: {
                FechaInicio: $("#fechaDesde").val(),
                FechaFin: $("#fechaHasta").val(),
                EstadoControl: $("#cmbEstadoRegistro").val() == 'false' ? false : true
            },
            success: function (resultado) {
                $('#DivEvaluacionLomosMigas').empty();
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                } else {
                    $('#MensajeRegistros').hide();

                    $('#DivEvaluacionLomosMigas').html(resultado);
                    config.opcionesDT.pageLength = 30;
                    $('#tblBandejaEvaluacion').DataTable(config.opcionesDT);
                }
                $("#divDateRangePicker").prop('hidden', false);
                $('#cargac').hide();

                //$("#btnPendiente").prop("hidden", false);
                //$("#btnAprobado").prop("hidden", true);

                //$("#divDateRangePicker").prop('hidden', false);
            },
            error: function (resultado) {
                $('#cargac').hide();
                MensajeError(resultado.responseText, false);
            }
        });
    }
}

function AbrirModalDetalle(IdCabecera) {
    CerrarConfirmacionAprobar();
    $('#file-preview-zone1').html('');
    $('#file-preview-zone2').html('');
    $('#file-preview-zone3').html('');
    $('#file-preview-zone4').html('');
    $('#cargac').show();
    Error = 0;
    let params = {
        IdCabeceraControl: IdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionProductoEnfundado/PartialDetalleBandeja?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#DivDetalle').empty();
                $('#DivDetalle').html(resultado);
                IdControlAp = IdCabecera;
                $('#txtfechaaprob').prop('readonly', true);


                $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
                    icons: {
                        time: 'far fa-clock',
                        date: 'far fa-calendar-alt',
                        up: 'fas fa-caret-up',
                        down: 'fas fa-caret-down',
                        previous: 'fas fa-backward',
                        next: 'fas fa-forward',
                        today: 'fas fa-calendar-day',
                        clear: 'fas fa-trash-alt',
                        close: 'fas fa-window-close'
                    }
                });
                $('#datetimepicker1').datetimepicker(
                    {
                        date: moment().format("YYYY-MM-DD HH:mm"),
                        format: "YYYY-MM-DD HH:mm",
                        minDate: moment($('#fechacontrol').val(), "YYYY-MM-DD HH:mm"),
                        ignoreReadonly: true
                    });
                config.opcionesDT.pageLength = 10;
                $('#tblDetalleBandeja').DataTable(config.opcionesDT);
                if ($('#txtAprobado').val() == 'True') {
                    $('#btnAprobar').prop('hidden', true);
                    $('#btnReversar').prop('hidden', false);
                    $('#divfechaap').prop('hidden', true);
                } else {
                    $('#btnAprobar').prop('hidden', false);
                    $('#btnReversar').prop('hidden', true);
                    $('#divfechaap').prop('hidden', false);
                }
                ConsultarFotosControl(IdCabecera);
                //ConsultarFirma(IdCabecera);
                $('#ModalDetalle').modal('show');
            } else {
                $('#DivDetalles').empty();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}

function AprobarControl() {
    Error = 0;
    $('#BtnSi').prop('hidden', true);
    $('#BtnNo').prop('hidden', true);
    $('#btnCargando').prop('hidden', false);

    $('#btnAprobar').prop('disabled', true);
    $('#btnclose').prop('disabled', true);
    $('#btncerrar').prop('disabled', true);
    ////var canvas = document.getElementById("firmacanvas");
    ////var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', '');
    var fechaingresada = $('#datetimepicker1').datetimepicker('viewDate');
    const data = new FormData();
    data.append('IdCabecera', IdControlAp);
    data.append('Fecha', moment(fechaingresada._d).format('YYYY-MM-DD HH:mm'));
    fetch("../EvaluacionProductoEnfundado/AprobarControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            MensajeCorrecto(resultado);
            $('#ModalDetalle').modal('hide');
            CargarBandeja();
        }


    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function ConfirmarAprobar() {
    if ($('#txtFechaAprob').val() == '') {
        $('#msjerrorfechaaprobacion').prop('hidden', false);
        return false;
    } else {
        $('#msjerrorfechaaprobacion').prop('hidden', true);
        MensajeConfirmacion('divconfirm', 'ModalDetalle', 'AprobarControl()', '¿Está seguro que desea aprobar el control?');
    }
}
function ConfirmarReversar() {
    MensajeConfirmacion('divconfirm', 'ModalDetalle', 'ReversarControl()', '¿Está seguro que desea reversar el control?');
}
function ReversarControl() {
    Error = 0;
    $('#BtnSi').prop('hidden', true);
    $('#BtnNo').prop('hidden', true);
    $('#btnCargando').prop('hidden', false);

    $('#btnAprobar').prop('disabled', true);
    $('#btnclose').prop('disabled', true);
    $('#btncerrar').prop('disabled', true);



    const data = new FormData();
    data.append('IdCabecera', IdControlAp);

    fetch("../EvaluacionProductoEnfundado/ReversarControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {

            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {

        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            MensajeCorrecto(resultado);
            $('#ModalDetalle').modal('hide');
            CargarBandeja();
        }
        $('#BtnSi').prop('hidden', false);
        $('#BtnNo').prop('hidden', false);
        $('#btnCargando').prop('hidden', true);
        $('#btnAprobar').prop('disabled', false);
        $('#btnclose').prop('disabled', false);
        $('#btncerrar').prop('disabled', false);
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado, false);
            $('#btnAprobar').prop('disabled', false);
            $('#btnclose').prop('disabled', false);
            $('#btncerrar').prop('disabled', false);

            $('#BtnSi').prop('hidden', false);
            $('#BtnNo').prop('hidden', false);
            $('#btnCargando').prop('hidden', true);
        })
}
//function ConsultarFirma(IdCabecera) {
//    $.ajax({
//        url: "../EvaluacionProductoEnfundado/ConsultarFirmaAprobacion",
//        type: "GET",
//        data: {
//            IdCabecera: IdCabecera
//        },
//        success: function (resultado) {
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (resultado != '0') {
//                document.getElementById('ImgFirma').src = '';
//                document.getElementById('ImgFirma').src = resultado;
//                $('#div_ImagenFirma').prop('hidden', false);
//                $('#signature-pad').prop('hidden', true);
//            } else if (resultado == '000') {
//                document.getElementById('ImgFirma').src = '';
//                $('#div_ImagenFirma').prop('hidden', false);
//                $('#signature-pad').prop('hidden', true);
//            }
//            else {
//                $('#signature-pad').prop('hidden', false);
//            }
//        },
//        error: function (resultado) {
//            MensajeError("Error: Comuníquese con sistemas", false);

//        }
//    });
//}
function CargarImagenes(imagen, Rotacion, Npreview) {
    //$('#file-preview1').remove();
    //$('#file-preview2').remove();
    //$('#file-preview3').remove();
    //$('#file-preview4').remove();
    
    if (imagen != null && imagen != '') {
        var filePreview = document.createElement('img');
        filePreview.id = 'file-preview' + Npreview;
        filePreview.src = "/Content/Img/" + imagen;
        var previewZone = document.getElementById('file-preview-zone' + Npreview);
        previewZone.appendChild(filePreview);

        $("#file-preview" + Npreview).addClass("img");
        $('#file-preview' + Npreview).rotate(parseInt(Rotacion));
        document.getElementById("file-preview" + Npreview).style.height = "0px";
        document.getElementById("file-preview" + Npreview).style.width = "0px";

        var img = new Image();
        img.onload = function () {
            //  alert(this.width + 'x' + this.height);
            var ancho = this.width;
            var alto = this.height;
            if (ancho < alto) {
                document.getElementById("file-preview" + Npreview).style.height = "350px";
                document.getElementById("file-preview" + Npreview).style.width = "250px";
            } else {
                document.getElementById("file-preview" + Npreview).style.height = "250px";
                document.getElementById("file-preview" + Npreview).style.width = "350px";
            }


        }
        img.src = "/Content/Img/" + imagen;

    }
}
function ConsultarFotosControl(IdCabecera) {
    Error = 0;
    let params = {
        IdControl: IdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionProductoEnfundado/ConsultarFotosControlxId?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarImagenes(resultado.ImagenCodigo, resultado.RotacionImagenCod, 1);
            CargarImagenes(resultado.ImagenProducto1, resultado.RotacionImagenProd1, 2);
            CargarImagenes(resultado.ImagenProducto2, resultado.RotacionImagenProd2, 3);
            CargarImagenes(resultado.ImagenProducto3, resultado.RotacionImagenProd3, 4);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado, false);
         
        })
}
//FECHA DataRangePicker
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
            "days": 61
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


//prueba

//function GuardarImagen() {

//    var canvas = document.getElementById("pizarra");
//    var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', ''); //image/png.....

  

//    var formData = new FormData();


//    formData.append('imagen', image);

//    $.ajax({
//        type: 'POST',
//        url: '/EvaluacionDeLomoyMigaEnBandeja/GuardarImagenFirma',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function (result) {
//            console.log(result);
//            var img = $('<img />', { id: 'Myid', src: result, alt: 'MyAlt' }).appendTo($('#result'));
//        }
//    });

//}

//prueba api firma


//var clearButton = wrapper.querySelector("[data-action=clear]");


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


// fin prueba api firma
