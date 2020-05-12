var itemEditar = [];
//PARAMETROS SP op=0 FILTRO POR idControlHigiene
var imagenFirma = [];
var estadoReporte = [];
$(document).ready(function () {
    CargarCabecera();
});

function CargarCabecera() {
    $('#cargac').show();
    if ($('#txtFecha').val()=='') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../HigieneComedorCocina/ConsultarHigieneControl",
        data: {
            fecha: $("#txtFecha").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#firmaDigital').prop('hidden', true);
                $('#divMostrarCabecera').prop('hidden', true);
                $("#divMostarTablaCabecera").html("No existen registros");
                $('#divBotonCrear').prop('hidden', false); divBotonCrearDetalle
                $('#divBotonCrearDetalle').prop('hidden', true); 
                $('#divMostarTablaDetallesVer').prop('hidden', true);
                $('#divMostarTablaDetallesVer').html(resultado);
                LimpiarCabecera();
            } else {                
                CambiarMensajeEstado(resultado[0].EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false); 
                CargarDetalle(resultado[0].IdControlHigiene, 0);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaCabeceraVer").val(moment(resultado[0].Fecha).format('YYYY-MM-DDTHH:mm'));
                $("#txtObservacionVer").val(resultado[0].Observacion);
                itemEditar = resultado;
                imagenFirma = resultado;                
            }            
            setTimeout(function () {
                $('#cargac').hide();
            }, 100);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera(siAprobar) {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            var idControlHigiene = 0;
            if (itemEditar.length != 0) {
                idControlHigiene = itemEditar[0].IdControlHigiene;
            }
            $.ajax({
                url: "../HigieneComedorCocina/GuardarModificarHigieneControl",
                type: "POST",
                data: {
                    IdControlHigiene: idControlHigiene,
                    Fecha: $("#txtIngresoFechaCabecera").val(),
                    Hora: $("#txtIngresoFechaCabecera").val(),
                    Observacion: $("#txtObservacion").val(),
                    siAprobar: siAprobar
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    }
                    $('#ModalIngresoCabecera').modal('hide');
                    $('#divBotonesCRUD').prop('hidden', false);
                    $('#divMostarTablaDetalle').prop('hidden', false);
                    $('#divBotonCrear').prop('hidden', true);
                    LimpiarCabecera();
                    itemEditar = 0;
                    $('#cargac').hide();
                    CargarCabecera(0);
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function EliminarConfirmar() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte== true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    },200);
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            var idControlHigiene = 0;
            if (itemEditar.length != 0) {
                idControlHigiene = itemEditar[0].IdControlHigiene;
            }
            $.ajax({
                url: "../HigieneComedorCocina/EliminarHigieneControl",
                type: "POST",
                data: {
                    IdControlHigiene: idControlHigiene
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == "0") {
                        MensajeAdvertencia("Falta Parametro IdLavadoCisterna");
                        $("#modalEliminarControl").modal("hide");
                        $('#cargac').hide();
                        return;
                    } else if (resultado == "1") {
                        $('#firmaDigital').prop('hidden', true);
                        $("#modalEliminarControl").modal("hide");
                        CargarCabecera();
                        MensajeCorrecto("Registro eliminado con Éxito");
                        $('#cargac').hide();
                    }else if (resultado == '2') {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                        $('#cargac').hide();
                        return;
                    }
                    itemEditar = 0;
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            LimpiarModalIngresoCabecera();
            $("#txtIngresoFechaCabecera").val(moment(itemEditar[0].Fecha).format("YYYY-MM-DDTHH:hh"));
            $("#txtObservacion").val(itemEditar[0].Observacion);
            $('#ModalIngresoCabecera').modal('show');
        }
    }, 200);
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $("#txtFechaCabecera").prop('disabled', false);
    $('#ModalIngresoCabecera').modal('show');
    $("#txtIngresoFechaCabecera").val(moment($("#txtFecha").val()).format("YYYY-MM-DDTHH:mm"));
    itemEditar = [];
}

function LimpiarCabecera() {
    $('#txtFechaCabeceraVer').val('');
    $('#txtObservacionVer').val('');    
}

function LimpiarModalIngresoCabecera() {
    $('#txtIngresoFechaCabecera').val(moment($('#txtFecha').val()).format('YYYY-MM-DDTHH:mm'));
    $('#txtObservacion').val(''); 
}

function ValidarDatosVacios(siAprobar) {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera(siAprobar);
}

function OnChangeTextBox() {
    var con = 0;
    if ($('#txtIngresoFechaCabecera').val() == '') {
        $("#txtIngresoFechaCabecera").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtIngresoFechaCabecera").css('border', '');}
    return con;
}

//DETALLE
function CargarDetalle(idControlHigiene, op) {
    $('#cargac').show();
    $.ajax({
        url: "../HigieneComedorCocina/HigieneComedorCocinaDetallePartial",
        data: {
            IdControlHigiene: idControlHigiene,
            fechaDesde: $('#txtFecha').val(),
            fechaHasta: $('#txtFecha').val(),
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {                
                $("#divMostarTablaDetallesVer").html("No existen registros");
                $('#firmaDigital').prop('hidden', true);
            } else {
                $('#firmaDigital').prop('hidden', false);
                $('#divBotonCrearDetalle').prop('hidden', true);
                $('#divMostarTablaDetallesVer').prop('hidden', false); 
                $('#divMostarTablaDetallesVer').html(resultado);
                ConsultarFirma();
                //ORDENAMOS LA TABLA POR LA COLUMNA CATEGORIA
                var table = $('#tblDataTableVer').DataTable();
                table
                    .order([2, 'asc'])
                    .draw();
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

function ModalIngresoDetalle() {
    LimpiarDetalle();
    $('#ModalIngresoDetalle').modal('show');
    var estadoRegistro = 'A';
    //INICIO AJAX
    $.ajax({
        url: "../HigieneComedorCocina/ConsultaHigieneMantActivosPartial",
        type: "GET",
        data: {
            estadoRegistro: estadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaDetalles").html("No existen registros");
            } else {
                
                $("#divMostarTablaDetalles").html(resultado);
                //ORDENAMOS LA TABLA POR LA COLUMNA CATEGORIA
                var table = $('#tblDataTableDetalle').DataTable();
                table
                    .order([2, 'asc'])
                    .draw();
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
    //FIN AJAX
}

function LimpiarDetalle() {

}

function GuardarDetalle(jdata) {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#cargac').show();
            var con = 0;
            var idControlHigiene = itemEditar[0].IdControlHigiene;
            jdata.forEach(function (rowMantenimiento) {
                var sel = document.getElementById('selectLimpiezaEstado-' + rowMantenimiento.IdMantenimiento).value;
                var obs = document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdMantenimiento).value;
                var acc = document.getElementById('txtACorrectivaDetalle-' + rowMantenimiento.IdMantenimiento).value;
                var idControlMantenimiento = document.getElementById('txtIdControlDetalle-' + rowMantenimiento.IdMantenimiento).value;
                jdata[con].IdControlHigiene = idControlHigiene;
                jdata[con].LimpiezaEstado = sel;
                jdata[con].Observacion = obs;
                jdata[con].AccionCorrectiva = acc;
                jdata[con].IdControlDetalle = idControlMantenimiento;
                con++;
            });

            $.ajax({
                url: "../HigieneComedorCocina/GuardarModificarHigieneControlDetalle",
                type: "POST",
                data: {
                    listaControlDetalle: jdata
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    }
                    $('#ModalIngresoDetalle').modal('hide');
                    LimpiarCabecera();
                    itemEditar = 0;
                    $('#cargac').hide();
                    CargarCabecera(0);
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function ActualizarDetalle(jdata) {//LLAMADA DESDE EL PARTIAL HigieneComedorCocinaDetallePartial
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            ModalIngresoDetalle();
            setTimeout(function () {
                jdata.forEach(function (rowMantenimiento) {
                    document.getElementById('selectLimpiezaEstado-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.LimpiezaEstado;
                    document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.Observacion;
                    document.getElementById('txtACorrectivaDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.AccionCorrectiva;
                    document.getElementById('txtIdControlDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.IdControlDetalle;                    
                });
            }, 200);
        }
    }, 200);
}

function ConsultarEstadoRegistro() {
    $.ajax({
        url: "../HigieneComedorCocina/ConsultarHigieneControl",
        data: {
            fecha: $("#txtFecha").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            estadoReporte = resultado[0].EstadoReporte;
            CambiarMensajeEstado(resultado[0].EstadoReporte);
            ConsultarFirma();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function CambiarMensajeEstado(estadoReporteParametro) {
    if (estadoReporteParametro == true) {
        $("#lblAprobadoPendiente").text("APROBADO");
        $("#lblAprobadoPendiente").removeClass('badge-danger');
        $("#lblAprobadoPendiente").addClass('badge badge-success');
    } else {
        $("#lblAprobadoPendiente").text("PENDIENTE");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").addClass('badge badge-danger');
    }
}

function GuardarFirma() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            signaturePad.clear();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            if (!signaturePad.isEmpty()) {
                document.getElementById('ImgFirma').src = '';
                var canvas = document.getElementById("firmacanvas");
                var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', '');
                var formData = new FormData();
                formData.append('image', image);
                formData.append('idControlHigiene', imagenFirma[0].IdControlHigiene);
                signaturePad.clear();

                //document.getElementById("ImgFirma").style.display = 'none';
                $.ajax({
                    type: 'POST',
                    url: '/HigieneComedorCocina/GuardarImagenFirma',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (resultado) {
                        signaturePad.clear();
                        if (resultado == "101") {
                            window.location.reload();
                        }
                        if (resultado != 0) {
                            $('#div_ImagenFirma').prop('hidden', false);
                            document.getElementById('ImgFirma').src = resultado;
                            $('#signature-pad').prop('hidden', true);
                            MensajeCorrecto("Firma ingresada Correctamente");
                        } else {
                            MensajeAdvertencia('¡Error al guardar la Firma: !' + imagenFirma[0].IdDesechosLiquidos);
                        }
                    }
                });
            } else {
                MensajeAdvertencia('¡No se ha firmado el documento!   FIRMA INVALIDA');
            }
        }
    }, 200);
}

function VolverAFirmar() {
    $('#div_ImagenFirma').prop('hidden', true);
    $('#signature-pad').prop('hidden', false);
}

function ConsultarFirma() {
    $.ajax({
        url: "../HigieneComedorCocina/ConsultarImagenFirma",
        type: "GET",
        data: {
            idControlHigiene: imagenFirma[0].IdControlHigiene
        },
        success: function (resultado) {
            $("#firmaDigital").prop("hidden", false);
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
            if (estadoReporte == true) {
                $("#btnActualizarFirma").prop("hidden", true);
                $("#signature-pad").prop("hidden", true);
                if (imagenFirma[0].FirmaControl == null) {
                    $("#firmaDigital").prop("hidden", true);
                }
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
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