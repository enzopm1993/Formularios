var rotation = 0;
var ListaLotes;
var Error = 0;
var IdCabecera = 0;
var IdDetalle = 0;
var TotalMaximo;
var IdFotoEvaluacioProductoEnfundado = 0;
var ParametrosLomo =
{
    Limpieza1: {
        Venas: 3,
        Espinas: 8,
        Moretones: 5,
        Escamas: 2,
        Piel: 3,
        Total: 21
    },
    Limpieza2: {
        Venas: 2,
        Espinas: 5,
        Moretones: 3,
        Escamas: 1,
        Piel: 1,
        Total: 12
    },
    Limpieza3: {
        Venas: 0,
        Espinas: 3,
        Moretones: 1,
        Escamas: 0,
        Piel: 0,
        Total: 4
    }
}
var ParametrosMiga =
{
    Limpieza1: {
        Venas: 7,
        Espinas: 10,
        Moretones: 7,
        Escamas: 10,
        Piel: 6,
        Total: 40
    },
    Limpieza2: {
        Venas: 4,
        Espinas: 10,
        Moretones: 3,
        Escamas: 5,
        Piel: 3,
        Total: 25
    },
    Limpieza3: {
        Venas: 0,
        Espinas: 2,
        Moretones: 2,
        Escamas: 0,
        Piel: 0,
        Total: 4
    }
}
$(document).ready(function () {
    $('#cmbEmpacador').select2();
    //$('#txtMonto').mask('000,000,000.00', { reverse: true });
    $('#txtVenas').mask("9?9");
    $('#txtEspinas').mask("9?9");
    $('#txtSangre').mask("9?9");
    $('#txtEscamas').mask("9?9");
    $('#txtPiel').mask("9?9");
    $('#txtOtros').mask("9?9");

    $('#txtMoretones').mask("9?9");
    $('#txtHematomas').mask("9?9");
    $('#txtTrozos').mask("9?.99");
    $('#txtMiga').mask("9?.99");
    LLenarComboOrdenes();
});
function ValidarParametro() {
    $('#lblparametro').text("");
    //console.log($('#Lomo').val());
    //console.log($('#Miga').val());
    if ($('#Lomo').is(':checked') || $('#Miga').is(':checked') || $('#Trozo').is(':checked') && ($('#cmbNivelLimpieza').prop('selectedIndex') != 0)) {

        if ($('#Lomo').is(':checked') || $('#Trozo').is(':checked')) {
            console.log($('#cmbNivelLimpieza').val());
            if ($('#cmbNivelLimpieza').val() == '1') {
           
                $('#lblparametro').text(ParametrosLomo.Limpieza1.Total);
                TotalMaximo = ParametrosLomo.Limpieza1.Total;
            }
            if ($('#cmbNivelLimpieza').val() == '2') {
                $('#lblparametro').text(ParametrosLomo.Limpieza2.Total);
                TotalMaximo = ParametrosLomo.Limpieza2.Total;
            }
            if ($('#cmbNivelLimpieza').val() == '3') {
                $('#lblparametro').text(ParametrosLomo.Limpieza3.Total);
                TotalMaximo = ParametrosLomo.Limpieza3.Total;
            }

        } else {
            if ($('#cmbNivelLimpieza').val() == '1') {
                $('#lblparametro').text(ParametrosMiga.Limpieza1.Total);
                TotalMaximo = ParametrosMiga.Limpieza1.Total;
            }
            if ($('#cmbNivelLimpieza').val() == '2') {
                $('#lblparametro').text(ParametrosMiga.Limpieza2.Total);
                TotalMaximo = ParametrosMiga.Limpieza2.Total;
            }
            if ($('#cmbNivelLimpieza').val() == '3') {
                $('#lblparametro').text(ParametrosMiga.Limpieza3.Total);
                TotalMaximo = ParametrosMiga.Limpieza3.Total;
            }

        }
    }

}
$("#cmbNivelLimpieza").on("change", function () {
    ValidarParametro();
});
$("#Lomo").on("change", function () {
    ValidarParametro();
});
$("#Miga").on("change", function () {
    ValidarParametro();
});
$("#Trozo").on("change", function () {
    ValidarParametro();
});
$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});
$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").prop('selectedIndex') == 0) {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#cmbOrdeneFabricacion").val($("#SelectOrdenFabricacion").val());
    //CargarLotes($("#SelectOrdenFabricacion").val());
    DatosOrdenFabricacion();

    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);

    ConsultarCabControl();

});
$("#btncancelarguardarfoto").on("click", function () {
    IdFotoEvaluacioProductoEnfundado = 0;
});
function ModalGenerarControlDetalle2() {
    IdFotoEvaluacioProductoEnfundado = 0;
    NuevoControlDetalle2();
    $("#ModalGenerarControlDetalle2").modal("show");

}
function EliminarFoto() {
    //console.log('Eliminar');
    $('#modal-detalle2-si').prop('disabled', true);
    $('#modal-detalle2-no').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdFoto', IdFotoEvaluacioProductoEnfundado);


    fetch("../EvaluacionProductoEnfundado/EliminarFotoDetalle", {
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
            $('#modalEliminarControlDetalle2').modal('hide');

            if (resultado[0] == '002') {
                MensajeCorrecto(resultado[1]);
                CargarControlDetalle2();
            } else {
                MensajeAdvertencia(resultado[1]);
                CargarControlDetalle2();
            }


        }
        $('#modal-detalle2-si').prop('disabled', false);
        $('#modal-detalle2-no').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            $('#modal-detalle2-si').prop('disabled', false);
            $('#modal-detalle2-no').prop('disabled', false);
            MensajeError(resultado.responseText, false);

        })
}
async function CargarControlDetalle2Ajax() {
    $("#divTableDetalle2").html('');
    let params = {
        IdDetalle: IdDetalle
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionProductoEnfundado/DetalleFotoPartial?' + query;
    var promesa = fetch(url);
    return promesa;
    //$.ajax({
    //    url: "../OperatividadMetal/OperatividadMetalDetectorPartial",
    //    type: "GET",
    //    data: {
    //        IdDetalle: Model.IdOperatividadMetal

    //    },
    //    success: function (resultado) {
    //        if (resultado == "101") {
    //            window.location.reload();
    //        }
    //        if (resultado == "0") {
    //            $("#divTableDetalle2").html(Mensajes.SinRegistros);
    //            $("#spinnerCargandoDetalle2").prop("hidden", true);
    //        } else {
    //            $("#spinnerCargandoDetalle2").prop("hidden", true);
    //            $("#divTableDetalle2").html(resultado);

    //        }
    //    },
    //    error: function (resultado) {
    //        MensajeError(Mensajes.Error + resultado.responseText, false);
    //        $("#spinnerCargandoDetalle2").prop("hidden", true);
    //    }
    //});
}
async function CargarControlDetalle2() {
    try {
        $('cargac').show();
        var promesaDetalleFoto = await CargarControlDetalle2Ajax();
        if (!promesaDetalleFoto.ok) {
            throw "Error";
        }
        var ResultadoDetalleFoto = await promesaDetalleFoto.text();
        if (ResultadoDetalleFoto == '"101"') {
            window.location.reload();
        }
        if (ResultadoDetalleFoto != '"0"') {
            $("#divTableDetalle2").html(ResultadoDetalleFoto);
        }
        $('cargac').hide();
        //MensajeCorrecto(ResultadoGuardarFoto);
    } catch (ex) {
        $('cargac').hide();
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }
}
async function GenerarControlDetalle2() {
    try {
        $('#btnguardarfoto').prop('hidden', true);
        $('#btncancelarguardarfoto').prop('hidden', true);
        $('#btncargandoimg').prop('hidden', false);

        var promesaGuardarFoto = await GenerarControlDetalle2Ajax();
        if (!promesaGuardarFoto.ok) {
            throw "Error";
        }
        var ResultadoGuardarFoto = await promesaGuardarFoto.json();
        console.log(ResultadoGuardarFoto[0]);
        if (ResultadoGuardarFoto[0] == '003') {
            MensajeAdvertencia(ResultadoGuardarFoto[1]);
        } else {
            MensajeCorrecto(ResultadoGuardarFoto[1]);
        }

        $('#btnguardarfoto').prop('hidden', false);
        $('#btncancelarguardarfoto').prop('hidden', false);
        $('#btncargandoimg').prop('hidden', true);
        $("#ModalGenerarControlDetalle2").modal("hide");
        await CargarControlDetalle2();
    } catch (ex) {
        $('#btnguardarfoto').prop('hidden', false);
        $('#btncancelarguardarfoto').prop('hidden', false);
        $('#btncargandoimg').prop('hidden', true);
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }
}
async function GenerarControlDetalle2Ajax() {
    var imagen = $('#file-upload')[0].files[0];
    var data = new FormData();
    data.append("dataImg", imagen);
    data.append("IdDetalleEvaluacionProductoEnfundado", IdDetalle);
    data.append("IdFotoEvaluacioProductoEnfundado", IdFotoEvaluacioProductoEnfundado);
    data.append("Observacion", $("#txtNovedad").val());
    data.append("Rotacion", rotation);

    var promesa = fetch("../EvaluacionProductoEnfundado/GuardarFotoDetalle", {
        method: 'POST',
        body: data
    })
    return promesa;
    //$.ajax({
    //    url: "../EvaluacionDeLomoyMigaEnBandeja/GuardarFotoDetalle",
    //    type: "POST",
    //    cache: false,
    //    data: data,
    //    contentType: false,
    //    processData: false,
    //    async: false,
    //    data: data,
    //    success: function (resultado) {
    //        //  $("#spinnerCargandoDetalle").prop("hidden", true);
    //        if (resultado == "101") {
    //            window.location.reload();
    //        }
    //        CargarControlDetalle2();
    //        MensajeCorrecto(resultado);
    //        $("#ModalGenerarControlDetalle2").modal("hide");
    //    },
    //    error: function (resultado) {
    //        MensajeError(Mensajes.Error + resultado.responseText, false);
    //        $("#spinnerCargandoDetalle2").prop("hidden", true);
    //    }
    //});
}
async function LlenarComboOrdenesAjax() {
    let params = {
        Fecha: $("#txtFechaOrden").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultaSoloOFNivel3?' + query;
    var promesa = fetch(url);
    return promesa;
}
async function LLenarComboOrdenes(/*orden*/) {
    try {
        $('#txtCliente').val('');
        //if ($('#txtFechaProduccion').val() == '') {
        //    $('#msjErrorFechaProduccion').prop('hidden', false);
        //    return;
        //} else {
        //    $('#msjErrorFechaProduccion').prop('hidden', true);
        //}
        $('#SelectOrdenFabricacion').empty();
        $('#SelectOrdenFabricacion').append('<option> Seleccione</option>');
        if (!$('#txtFechaOrden').val() == '') {
            var PromesaConsultar = await LlenarComboOrdenesAjax();
            if (!PromesaConsultar.ok) {
                throw "Error";
            }
            var ResultadoConsultar = await PromesaConsultar.json();
            //console.log(ResultadoConsultar);
            if (ResultadoConsultar == "101") {
                window.location.reload();
            }
            $.each(ResultadoConsultar, function (key, value) {
                $('#SelectOrdenFabricacion').append('<option value=' + value.Orden + '>' + value.Orden + '</option>');
            });

        }
    } catch (ex) {
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }
//Error = 0;
//$('#txtCliente').val('');
//    $('#SelectOrdenFabricacion').empty();
//    $('#SelectOrdenFabricacion').append('<option>Seleccione</option>');
//let params = {
//    Fecha: $("#txtFechaOrden").val()
//}
//let query = Object.keys(params)
//    .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
//    .join('&');

//let url = '../General/ConsultaSoloOFNivel3?' + query;
//    if (!$('#txtFechaOrden').val() == '') {
//    fetch(url)
//        //,body: data
//        .then(function (respuesta) {
//            if (!respuesta.ok) {
//                //MensajeError(respuesta.statusText);
//                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
//                Error = 1;
//            }
//            return respuesta.json();
//        })
//        .then(function (resultado) {
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (Error == 0) {
//                $.each(resultado, function (key, value) {
//                    $('#SelectOrdenFabricacion').append('<option value=' + value.Orden + '>' + value.Orden + '</option>');
//                });
//                if (orden != null) {
//                    $('#SelectOrdenFabricacion').val(orden);
//                }
//            }
//        })
//        .catch(function (resultado) {
//            //console.log(resultado);
//            MensajeError(resultado.responseText, false);

//        })
//}

}
function DatosOrdenFabricacion() {
    Error = 0;
    $('#txtCliente').val('');
    //console.log(ListaOrdenesFabricacion);
    //var Datos = Enumerable.From(ListaOrdenesFabricacion)
    //    .Where(function (x) { return x.ORDEN_FABRICACION == $('#cmbOrdeneFabricacion').val() })
    //    .Select(function (x) { return x })
    //    .FirstOrDefault();
    let params = {
        Orden: $("#cmbOrdeneFabricacion").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultarDatosOrdenFabricacion?' + query;
    if ($('#cmbOrdeneFabricacion').prop('selectedIndex') == 0) {

    } else {
        fetch(url)
            //,body: data
            .then(function (respuesta) {
                if (!respuesta.ok) {
                    //MensajeError(respuesta.statusText);
                    MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                    Error = 1;
                }
                return respuesta.json();
            })
            .then(function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (Error == 0) {
                    $('#txtCliente').val(resultado.CLIENTE);
                    $('#txtDestino').val(resultado.DESTINO);
                    $('#txtMarca').val(resultado.MARCA);
                    //console.log(resultado);
                    //$.each(resultado, function (key, value) {
                    //    $('#cmbLote').append('<option value=' + value.Codigo + '>' + value.Descripcion + '</option>');
                    //});
                    //console.log(resultado);

                }
            })
            .catch(function (resultado) {
                //console.log(resultado);
                MensajeError(resultado.responseText, false);

            })
}

//$('#txtCliente').val(Datos.NombreCliente);
LlenarComboLotes($("#cmbOrdeneFabricacion").val());
}
async function ConsultarCabControlAjax() {
    const data = new FormData();
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    data.append('OrdenFabricacion', $("#cmbOrdeneFabricacion").val());
    data.append('Lomo', $("#Lomo").is(':checked'));
    data.append('Miga', $("#Miga").is(':checked'));
    data.append('Trozo', $("#Trozo").is(':checked'));
    var promesa = fetch("../EvaluacionProductoEnfundado/ConsultarCabeceraControl", {
        method: 'POST',
        body: data
    })
    return promesa;
}
async function ConsultarCabControl(/*bandera*/) {

    try {
        if ($('#txtFechaProduccion').val() == '') {
            $('#msjErrorFechaProduccion').prop('hidden', false);
            return false;
        } else {
            $('#msjErrorFechaProduccion').prop('hidden', true);
        }
        if ($('#cmbOrdeneFabricacion').val() == '') {
            $('#msjerrorordenfb').prop('hidden', false);
            return;
        } else {
            $('#msjerrorordenfb').prop('hidden', true);
        }
        //if (bandera != 'of')//bandera para que solo se ejecute si se llama desde onchange de fecha, y no por onchange de orden de fabricacion
        //{
        //    await LLenarComboOrdenes();
        //}
        $('#btnCargando').prop('hidden', false);
        $('#btnConsultar').prop('hidden', true);
        $('#btnLimpiar').prop('hidden', true);
        $('#btnEliminarCabeceraControl').prop('hidden', true);
        $('#btnGuardar').prop('hidden', true);
        $('#cargac').show();
        var PromesaConsultar = await ConsultarCabControlAjax();
        if (!PromesaConsultar.ok) {
            throw "Error";
        }
        var ResultadoConsulta = await PromesaConsultar.json();
        if (ResultadoConsulta == "101") {
            window.location.reload();
        }
        if (ResultadoConsulta != "0") {
         
            $('#mensajeRegistros').prop('hidden', true);
            IdCabecera = ResultadoConsulta.IdEvaluacionProductoEnfundado;
            $('#cmbOrdeneFabricacion').val(ResultadoConsulta.OrdenFabricacion);
            $('#txtCliente').val(ResultadoConsulta.Cliente);
            if (ResultadoConsulta.Lomo) {
                $('#Lomo').prop('checked', true);
            }
            if (ResultadoConsulta.Miga) {
                $('#Miga').prop('checked', true);
            }
            if (ResultadoConsulta.Trozo) {
                $('#Trozo').prop('checked', true);
            }
            
            if (ResultadoConsulta.EstadoControl == true) {
                $("#estadocontrol").removeClass("badge-danger").addClass("badge-success");
                $('#estadocontrol').text(Mensajes.Aprobado);
            }
            else {
                $("#estadocontrol").removeClass("badge-success").addClass("badge-danger");
                $('#estadocontrol').text(Mensajes.Pendiente);
            }
            $('#txtDestino').val(ResultadoConsulta.Destino);
            $('#txtMarca').val(ResultadoConsulta.Marca);
            $('#txtProveedor').val(ResultadoConsulta.Proveedor);
            $('#txtBatch').val(ResultadoConsulta.Batch);
            $('#txtLoteProveedor').val(ResultadoConsulta.Lote);
            $('#cmbNivelLimpieza').val(ResultadoConsulta.NivelLimpieza);
            ValidarParametro();
            $('#Observacion').val(ResultadoConsulta.Observacion);
            $('#CardDetalle').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            LlenarComboLotes(ResultadoConsulta.OrdenFabricacion);
            ConsultarDetalleControl();
            $('#brespacio').remove();
            $('#cargac').hide();
            //SlideCabecera();
        } else {
            //LLenarComboOrdenes();
            $('#brespacio').remove();
            $('#DivCabecera').after('<br id="brespacio">');
            $('#mensajeRegistros').prop('hidden', false);
            $('#mensajeRegistros').text(Mensajes.SinRegistros);
            LimpiarControles();
            $('#cargac').hide();
        }
        //DatosOrdenFabricacion();
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
        $('#btnLimpiar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('hidden', false);
        $('#btnGuardar').prop('hidden', false);
    } catch (ex) {
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
        $('#btnLimpiar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('hidden', false);
        $('#btnGuardar').prop('hidden', false);
    }
//const data = new FormData();
//data.append('FechaProduccion', $("#txtFechaProduccion").val());
//fetch("../EvaluacionProductoEnfundado/ConsultarCabeceraControl", {
//    method: 'POST',
//    body: data
//}).then(function (respuesta) {
//    if (!respuesta.ok) {
        
//        MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
//        Error = 1;
//    }
//    return respuesta.json();
//})
//    .then(function (resultado) {
    
//        if (resultado == "101") {
//            window.location.reload();
//        }
//        if (Error == 0) {
   
//            $('#txtFechaProduccion').prop('disabled', true);

//            LLenarComboOrdenes(resultado.OrdenFabricacion);
        
//            if (resultado != "0") {
//                $('#mensajeRegistros').prop('hidden', true);
//                IdCabecera = resultado.IdEvaluacionProductoEnfundado;
//                $('#cmbOrdeneFabricacion').val(resultado.OrdenFabricacion);
//                $('#txtCliente').val(resultado.Cliente);
//                if (resultado.Lomo) {
//                    $('#Lomo').prop('checked', true);
//                }
//                if (resultado.Miga) {
//                    $('#Miga').prop('checked', true);
//                }
//                if (resultado.Trozo) {
//                    $('#Trozo').prop('checked', true);
//                }

//                if (resultado.EstadoControl == true) {
//                    $('#EtiquetaEstadoControl').html('<text class="text-success">(Aprobado)</text>');
//                } else {
//                    $('#EtiquetaEstadoControl').html('<text class="text-warning">(Pendiente)</text>');
//                }
//                $('#txtDestino').val(resultado.Destino);
//                $('#txtMarca').val(resultado.Marca);
//                $('#txtProveedor').val(resultado.Proveedor);
//                $('#txtBatch').val(resultado.Batch);
//                $('#txtLoteProveedor').val(resultado.Lote);
//                $('#cmbNivelLimpieza').val(resultado.NivelLimpieza);
//                $('#Observacion').val(resultado.Observacion);
//                $('#CardDetalle').prop('hidden', false);
//                $('#btnEliminarCabeceraControl').prop('disabled', false);
//                LlenarComboLotes(resultado.OrdenFabricacion);
//                ConsultarDetalleControl();
       
//            } else {
//                LLenarComboOrdenes();
//                $('#brespacio').remove();
//                $('#DivCabecera').after('<br id="brespacio">');
//                $('#mensajeRegistros').prop('hidden', false);
//                $('#mensajeRegistros').text(Mensajes.SinRegistros);
//            }
//        }
//        $('#btnCargando').prop('hidden', true);
//        $('#btnConsultar').prop('hidden', false);
//        $('#btnLimpiar').prop('hidden', false);
//        $('#btnEliminarCabeceraControl').prop('hidden', false);
//        $('#btnGuardar').prop('hidden', false);
//    })
//    .catch(function (resultado) {
//        console.log(resultado);
//        MensajeError(resultado.responseText, false);
//    });

}

function GuardarCabceraControl() {
    var Lomo = false;
    var Miga = false;
    var Trozo = false;
    if ($('#cmbNivelLimpieza').prop('selectedIndex') == 0) {
        $('#msgerrorniveldelimpieza').prop('hidden', false);
        return false;
    } else {
        $('#msgerrorniveldelimpieza').prop('hidden', true);
    }
    if ($('#txtProveedor').val() == '') {
        $('#msgerrorproveedor').prop('hidden', false);
        return false;
    } else {
        $('#msgerrorproveedor').prop('hidden', true);
    }
    if ($('#txtBatch').val() == '') {
        $('#msgerrorbatch').prop('hidden', false);
        return false;
    } else {
        $('#msgerrorbatch').prop('hidden', true);
    }
    if ($('#txtLoteProveedor').val() == '') {
        $('#msgerrorloteproveedor').prop('hidden', false);
        return false;
    } else {
        $('#msgerrorloteproveedor').prop('hidden', true);
    }
    if (($('#Lomo').prop("checked") == false) && ($('#Miga').prop("checked") == false) && ($('#Trozo').prop("checked") == false)) {
        $('#msgerrortipoproducto').prop('hidden', false);
        return false;
    }
    else {
        $('#msgerrortipoproducto').prop('hidden', true);
    }
    if ($('#txtFechaProduccion').val() == '') {
        $('#msjErrorFechaProduccion').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorFechaProduccion').prop('hidden', true);
    }
    if ($('#cmbOrdeneFabricacion').prop('selectedIndex') == 0) {
        $('#msjErrorOrdenFabricacion').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorOrdenFabricacion').prop('hidden', true);
    }

    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    //var imagen1 = $('#file-uploadcod')[0].files[0];
    //var imagen2 = $('#file-upload1')[0].files[0];
    //var imagen3 = $('#file-upload2')[0].files[0];
    //var imagen4 = $('#file-upload3')[0].files[0];
    const data = new FormData();
    //data.append("RotacionImagenCod", rotation1);
    //data.append("RotacionImagenProd1", rotation1);
    //data.append("RotacionImagenProd2", rotation2);
    //data.append("RotacionImagenProd3", rotation3);
    //data.append('dataImg', imagen1);
    //data.append('dataImg1', imagen2);
    //data.append('dataImg2', imagen3);
    //data.append('dataImg3', imagen4);
    data.append('IdEvaluacionProductoEnfundado', IdCabecera);
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    data.append('Cliente', $("#txtCliente").val());
    data.append('OrdenFabricacion', $("#cmbOrdeneFabricacion").val());
    data.append('NivelLimpieza', $("#cmbNivelLimpieza").val());
    data.append('Marca', $('#txtMarca').val());
    data.append('Destino', $('#txtDestino').val());
    data.append('Proveedor', $('#txtProveedor').val());
    data.append('Lote', $('#txtLoteProveedor').val());
    data.append('Batch', $('#txtBatch').val());
    data.append('Observacion', $('#Observacion').val());
    if ($('#Lomo').is(":checked")) {
        Lomo = true;
    }
    if ($('#Miga').is(":checked")) {
        Miga = true;
    }
    if ($('#Trozo').is(":checked")) {
        Trozo = true;
    }

    data.append('Lomo', Lomo);
    data.append('Miga', Miga);
    data.append('Trozo', Trozo);
    fetch("../EvaluacionProductoEnfundado/GuardarCabeceraControl", {
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
            IdCabecera = resultado[2].IdEvaluacionProductoEnfundado;
            if (resultado[0] != "000" && resultado[0] != "001" ) {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                $('#brespacio').remove();
                $('#mensajeRegistros').text('');
                $('#CardDetalle').prop('hidden', false);
                SlideCabecera();
            }

        }
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
        $('#btnLimpiar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('hidden', false);
        $('#btnGuardar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function LimpiarControles() {
    IdCabecera = 0;
    Error = 0;
    IdDetalle = 0;
    //$('#divtabfotos label').text('Seleccione archivo');
    //$('#file-preview-zone1').html('');
    //$('#file-preview-zone2').html('');
    //$('#file-preview-zone3').html('');
    //$('#file-preview-zone4').html('');
    $('#estadocontrol').text('');
    $('#EtiquetaEstadoControl').html('');
    //$('#txtFechaProduccion').val('');
    $('#cmbOrdeneFabricacion').empty();
    //$('#cmbOrdeneFabricacion').append('<option>Seleccione...</option>');
    //$('#txtCliente').val('');
    $('#txtFechaProduccion').prop('disabled', false);
    //$('#mensajeRegistros').prop('hidden', true);
    //$('#Lomo').prop('checked', true);
    //$('#Miga').prop('checked', false);
    //$('#Trozo').prop('checked', false);
    //$('#txtDestino').val('');
    $('#txtProveedor').val('');
    $('#txtBatch').val('');
    $('#txtLoteProveedor').val('');
    //$('#txtMarca').val('');
    $('#cmbNivelLimpieza').prop('selectedIndex', 0);
    $('#Observacion').val('');
    $('#CardDetalle').prop('hidden', true);
    LimpiarDetalleControles();
    $('#DivDetalles').empty();
    $('#btnEliminarCabeceraControl').prop('disabled', true);
}
function ConfirmarEliminarCab() {
$('#ModalEliminarCabecera').modal('show');
}
function EliminarCabecera() {
$('#btnsicab').prop('disabled', true);
$('#btnnocab').prop('disabled', true);
Error = 0;
const data = new FormData();
data.append('IdCabecera', IdCabecera);


fetch("../EvaluacionProductoEnfundado/EliminarCabeceraControl", {
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
        $('#ModalEliminarCabecera').modal('hide');
        //if (resultado[0] == "002") {
        //    MensajeAdvertencia(resultado[1]);
        //} else {
        if (resultado[0] != '002') {
            MensajeAdvertencia(resultado[1]);
        } else {
            MensajeCorrecto(resultado[1]);

            LimpiarControles();
        }
        
        //}

    }
    $('#btnsicab').prop('disabled', false);
    $('#btnnocab').prop('disabled', false);

})
    .catch(function (resultado) {
        //console.log('error');
        //console.log(resultado);
        MensajeError(resultado.responseText, false);

    })
}
function LlenarComboLotes(orden) {
Error = 0;
$('#cmbLote').empty();
$('#cmbLote').append('<option>Seleccione...</option>');

let params = {
    Orden: orden /*$("#cmbOrdeneFabricacion").val()*/
}
let query = Object.keys(params)
    .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
    .join('&');

let url = '../General/ConsultarLotesPorOf?' + query;
if ($('#cmbOrdeneFabricacion').prop('selectedIndex') == 0) {

} else {
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            if (!respuesta.ok) {
                //MensajeError(respuesta.statusText);
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (Error == 0) {
                //console.log('combolotes');
                //console.log(resultado);
                ListaLotes = resultado;
                $.each(resultado, function (key, value) {
                    $('#cmbLote').append('<option value=' + value.descripcion + '>' + value.descripcion + '</option>');
                });

            }
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
}
function GuardarDetalleControl() {
    Error = 0;
    if ($('#txtHora').val() == '') {
        $('#msjHora').prop('hidden', false);
        ActivaInfo();
        return false;
    } else {
        $('#msjHora').prop('hidden', true);
    }
    if ($('#cmbEmpacador').prop('selectedIndex') == 0) {
        $('#msjempacador').prop('hidden', false);
        ActivaInfo();
        return false;
    } else {
        $('#msjempacador').prop('hidden', true);
    }
    if ($('#cmbLote').prop('selectedIndex') == 0) {
        $('#msjLote').prop('hidden', false);
        ActivaInfo();
        return false;
    } else {
        $('#msjLote').prop('hidden', true);
    }

    if ($('#cmbProteina').prop('selectedIndex') == 0) {
        $('#msjProteina').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjProteina').prop('hidden', true);
    }

    if ($('#cmbSabor').prop('selectedIndex') == 0) {
        $('#msjSabor').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjSabor').prop('hidden', true);
    }
    if ($('#cmbTextura').prop('selectedIndex') == 0) {
        $('#msjTextura').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjTextura').prop('hidden', true);
    }
    if ($('#cmbColor').prop('selectedIndex') == 0) {
        $('#msjColor').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjColor').prop('hidden', true);
    }
    if ($('#cmbOlor').prop('selectedIndex') == 0) {
        $('#msjOlor').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjOlor').prop('hidden', true);
    }
    if ($('#cmbMoreton').prop('selectedIndex') == 0) {
        $('#msjMoreton').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjMoreton').prop('hidden', true);
    }


    $('#btnEliminarDetalleControl').prop('hidden', true);
    $('#btnGuardarDetalle').prop('hidden', true);
    $('#btnLimpiarDetalle').prop('hidden', true);
    $('#btnCargandoDetalle').prop('hidden', false);
    const data = new FormData();
    data.append('IdDetalleEvaluacionProductoEnfundado', IdDetalle);
    data.append('Hora', $("#txtHora").val());
    data.append('Linea', $("#cmbLinea").val());
    data.append('Lote', $("#cmbLote").val());
    data.append('buque', $("#txtBuque").val());
    data.append('Sabor', $("#cmbSabor").val());
    data.append('Textura', $("#cmbTextura").val());
    data.append('Color', $("#cmbColor").val());
    data.append('Olor', $("#cmbOlor").val());
    data.append('Moretones', $("#cmbMoreton").val());
    data.append('Proteina', $("#cmbProteina").val());
    data.append('Trozo', $("#txtTrozos").val());
    data.append('Venas', $("#txtVenas").val());
    data.append('Espinas', $("#txtEspinas").val());
    data.append('Sangre', $("#txtSangre").val());
    data.append('Escamas', $("#txtEscamas").val());
    data.append('Piel', $("#txtPiel").val());
    data.append('Trozo', $("#txtTrozos").val());
    data.append('Miga', $("#txtMiga").val());
    data.append('Empacador', $("#cmbEmpacador").val());
    data.append('Otro', $("#txtOtros").val());
    data.append('IdCabeceraEvaluacionProductoEnfundado', IdCabecera);


fetch("../EvaluacionProductoEnfundado/GuardarDetalleControl", {
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

        if (resultado[0] != "000" && resultado[0] != "001") {
            MensajeAdvertencia(resultado[1]);
        } else {
            MensajeCorrecto(resultado[1]);
            //$('#CardDetalle').prop('hidden', false);
            //ConsultarDetalleControl();
            ConsultarDetalleControl(1);
        }
        LimpiarDetalleControles();
    }
    $('#btnGuardarDetalle').prop('hidden', false);
    $('#btnLimpiarDetalle').prop('hidden', false);
    $('#btnCargandoDetalle').prop('hidden', true);
    $('#btnEliminarDetalleControl').prop('hidden', false);
})
    .catch(function (resultado) {
        //console.log('error');
        //console.log(resultado);
        MensajeError(resultado, false);
        $('#btnGuardarDetalle').prop('hidden', false);
        $('#btnLimpiarDetalle').prop('hidden', false);
        $('#btnCargandoDetalle').prop('hidden', true);
        $('#btnEliminarDetalleControl').prop('hidden', false);

    })
}
function ConsultarDetalleControl(bandera) {
Error = 0;
let params = {
    IdCabeceraControl: IdCabecera
}
let query = Object.keys(params)
    .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
    .join('&');

let url = '../EvaluacionProductoEnfundado/PartialDetalleControl?' + query;
fetch(url)
    //,body: data
    .then(function (respuesta) {
        return respuesta.text();
    })
    .then(function (resultado) {
        if (resultado == '"101"') {
            window.location.reload();
        }
        if (bandera != 1) {
            SlideCabecera();
        }
        if (resultado != '"0"') {
            $('#DivDetalles').empty();
            $('#DivDetalles').html(resultado);
            config.opcionesDT.pageLength = 10;
            $('#TableDetalle').DataTable(config.opcionesDT);
            LimpiarDetalleControles();
            $('#brespacio').remove();
            //ConsultarFirma();

        } else {
            $('#DivDetalles').empty();
        }
        //console.log(resultado);
    })
    .catch(function (resultado) {
        console.log(resultado);
        MensajeError(resultado.responseText, false);
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
    })
}
function LimpiarDetalleControles() {
    $('#divDetalle2').prop('hidden', true);
    $('#txtHora').prop('disabled', false);
    $('#btnEliminarDetalleControl').prop('disabled', true);
    $('#txtHora').val(moment().format("HH:mm"));
    $('#txtBuque').val('');
    $('#cmbMoreton').prop('selectedIndex', 0);
    //$('#txtMoretones').val('');
    //$('#txtHematomas').val('');
    $('#txtTrozos').val('');
    $('#txtVenas').val('');
    $('#txtEspinas').val('');
    $('#txtSangre').val('');
    $('#txtEscamas').val('');
    $('#txtPiel').val('');
    $('#cmbProteina').prop('selectedIndex', 0);
    $('#cmbLinea').prop('selectedIndex', 0);
    $('#cmbLote').prop('selectedIndex', 0);
    $('#cmbSabor').prop('selectedIndex', 0);
    $('#cmbTextura').prop('selectedIndex', 0);
    $('#cmbColor').prop('selectedIndex', 0);
    $('#cmbOlor').prop('selectedIndex', 0);
    $('#cmbEmpacador').prop('selectedIndex', 0).change();
    $('#txtMiga').val('');
    $('#txtOtros').val('');

    IdDetalle = 0;
    ActivaInfo();

}
function ConfirmarEliminarDetalle() {
$('#ModalEliminarDetalle').modal('show');
}
function EliminarDetalle() {
$('#btnsicdet').prop('disabled', true);
$('#btnnodet').prop('disabled', true);
Error = 0;
const data = new FormData();
data.append('IdDetalle', IdDetalle);


fetch("../EvaluacionProductoEnfundado/EliminarDetalleControl", {
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
        $('#ModalEliminarDetalle').modal('hide');
        //if (resultado[0] == "002") {
        //    MensajeAdvertencia(resultado[1]);
        //} else {
        if (resultado[0] == '002') {
            MensajeCorrecto(resultado[1]);
        } else {
            MensajeAdvertencia(resultado[1]);
        }
        

        LimpiarDetalleControles();
        ConsultarDetalleControl(1);
        //}

    }
    $('#btnsidet').prop('disabled', false);
    $('#btnnodet').prop('disabled', false);

})
    .catch(function (resultado) {
        //console.log('error');
        //console.log(resultado);
        MensajeError(resultado.responseText, false);

    })
}
function ActivaInfo() {
$('#info').addClass("active");
$('#evalu').removeClass("active");
$('#verif').removeClass("active");
$("#informacion").removeClass("active fade");
$("#evaluacion").removeClass("active fade");
$("#verificacion").removeClass("active fade");
$('#informacion').addClass("active");
}
function ActivaEvaluacion() {
$('#evalu').addClass("active");
$('#info').removeClass("active");
$('#verif').removeClass("active");
$("#informacion").removeClass("active fade");
$("#evaluacion").removeClass("active fade");
$("#verificacion").removeClass("active fade");
$('#evaluacion').addClass("active");
}
function ActivaVerificacion() {
$('#verif').addClass("active");
$('#evalu').removeClass("active");
$('#info').removeClass("active");
$("#informacion").removeClass("active fade");
$("#evaluacion").removeClass("active fade");
$("#verificacion").removeClass("active fade");
$('#verificacion').addClass("active");
}
function DatosLote() {
var Datos = Enumerable.From(ListaLotes)
    .Where(function (x) { return x.descripcion == $('#cmbLote').val() })
    .Select(function (x) { return x })
    .FirstOrDefault();
$('#txtBuque').val(Datos.Barco);
}
async function ModificarDetalle(data) {
    IdFotoEvaluacioProductoEnfundado = 0;
    $('#divDetalle2').prop('hidden', false);
    IdDetalle = data.IdDetalle;

    await CargarControlDetalle2();
  
    $('#txtHora').val(data.Hora);
    $('#txtBuque').val(data.Buque);
    $('#cmbMoreton').val(data.CodMoretones);

    $('#txtTrozos').val(data.Trozos);
    $('#txtVenas').val(data.Venas);
    $('#txtEspinas').val(data.Espinas);
    $('#txtSangre').val(data.Sangre);
    $('#txtEscamas').val(data.Escamas);
    $('#txtPiel').val(data.Piel);
    $('#cmbLinea').val(data.Linea);
    $('#cmbLote').val(data.Lote);
    $('#cmbSabor').val(data.CodSabor);
    $('#cmbTextura').val(data.CodTextura);
    $('#cmbColor').val(data.CodColor);
    $('#cmbOlor').val(data.CodOlor);
    $('#cmbProteina').val(data.CodProteinas);
    $('#txtHora').prop('disabled', true);
    $('#btnEliminarDetalleControl').prop('disabled', false);
    $('#txtOtros').val(data.Otro);
    $('#txtMiga').val(data.Miga);
    $('#cmbEmpacador').val(data.empacador).change();
}
function SlideCabecera() {
$("#DivCabecera").slideToggle("fast");
}
function NuevoControlDetalle2() {
    $("#txtIdControlDetalle2").val("");
    $("#txtNovedad").val("");
    $("#file-upload").val('');
    $("#file-preview-zone").html('');
    $('#lblfoto').text('Seleccione archivo');
    rotation = 0;

}
function ModificarFoto(data) {
    //console.log(data);

    IdFotoEvaluacioProductoEnfundado = data.IdFotoEvaluacioProductoEnfundado;
    NuevoControlDetalle2();

    $("#txtNovedad").val(data.Observacion);
    if (data.Imagen != null && data.Imagen != '') {
        var re = 'EvaluacionProductoEnfundado/EvaluacionProductoEnfundado';
        var resultado = data.Imagen.replace(re, '');
        $('#lblfoto').text(resultado);

        var filePreview = document.createElement('img');
        filePreview.id = 'file-preview';
        filePreview.src = "/Content/Img/" + data.Imagen;
        var previewZone = document.getElementById('file-preview-zone');
        previewZone.appendChild(filePreview);

        $("#file-preview").addClass("img");
        $('#file-preview').rotate(parseInt(data.Rotacion));
        document.getElementById("file-preview").style.height = "0px";
        document.getElementById("file-preview").style.width = "0px";

        var img = new Image();
        img.onload = function () {
            //  alert(this.width + 'x' + this.height);
            var ancho = this.width;
            var alto = this.height;
            if (ancho < alto) {
                document.getElementById("file-preview").style.height = "350px";
                document.getElementById("file-preview").style.width = "250px";
            } else {
                document.getElementById("file-preview").style.height = "250px";
                document.getElementById("file-preview").style.width = "350px";
            }
            $("#ModalGenerarControlDetalle2").modal("show");

        }
        img.src = "/Content/Img/" + data.Imagen;

    } else {
        $("#ModalGenerarControlDetalle2").modal("show");
    }
}
function InactivarFoto(data) {
    $('#labelMensaje2').text('Novedad: ' + data.Observacion);
    $('#modalEliminarControlDetalle2').modal('show');
    IdFotoEvaluacioProductoEnfundado = data.IdFotoEvaluacioProductoEnfundado;
}
function readFile(input) {


    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            // console.log(this.width.toFixed(0));

            // alert('Imagen correcta :)')
            $("#file-preview-zone").html('');
            var filePreview = document.createElement('img');
            filePreview.id = 'file-preview';
            // filePreview.setAttribute("type", "hidden");

            //e.target.result contents the base64 data from the image uploaded
            filePreview.src = e.target.result;
            //console.log(e.target.result);
            var previewZone = document.getElementById('file-preview-zone');
            previewZone.appendChild(filePreview);
            $("#file-preview").addClass("img");
            document.getElementById("file-preview").style.height = "0px";
            document.getElementById("file-preview").style.width = "0px";
            //console.log(e.target.result);
            var image = new Image();
            image.src = e.target.result;
            image.onload = function () {
                if (this.width < this.height) {
                    document.getElementById("file-preview").style.height = "350px";
                    document.getElementById("file-preview").style.width = "250px";
                }
                else {
                    document.getElementById("file-preview").style.height = "250px";
                    document.getElementById("file-preview").style.width = "350px";
                }

            };


        }
        reader.readAsDataURL(input.files[0]);
    }
}

var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    readFile(e.srcElement);

}


$('#file-preview-zone').on("click", function (e) {
    rotation += 90;
    $('#file-preview').rotate(rotation);
    if (rotation == 360) {
        rotation = 0;
    }
});
//function AbrirModalImagenes() {
//$('#ModalAgregarImagenes').modal('show');
//}
//function readFile(input, n_prev) {
//if (input.files && input.files[0]) {
//    var reader = new FileReader();

//    reader.onload = function (e) {
//        // console.log(this.width.toFixed(0));

//        // alert('Imagen correcta :)')
//        $("#file-preview-zone" + n_prev).html('');
//        var filePreview = document.createElement('img');
//        filePreview.id = 'file-preview' + n_prev;
//        // filePreview.setAttribute("type", "hidden");

//        //e.target.result contents the base64 data from the image uploaded
//        filePreview.src = e.target.result;
//        //console.log(e.target.result);
//        var previewZone = document.getElementById('file-preview-zone' + n_prev);
//        previewZone.appendChild(filePreview);
//        $("#file-preview" + n_prev).addClass("img");
//        document.getElementById("file-preview" + n_prev).style.height = "0px";
//        document.getElementById("file-preview" + n_prev).style.width = "0px";
//        //console.log(e.target.result);
//        var image = new Image();
//        image.src = e.target.result;
//        image.onload = function () {
//            if (this.width < this.height) {
//                document.getElementById("file-preview" + n_prev).style.height = "350px";
//                document.getElementById("file-preview" + n_prev).style.width = "250px";
//            }
//            else {
//                document.getElementById("file-preview" + n_prev).style.height = "250px";
//                document.getElementById("file-preview" + n_prev).style.width = "350px";
//            }

//        };


//    }
//    reader.readAsDataURL(input.files[0]);
//}
//}

//var fileUploadcod = document.getElementById('file-uploadcod');
//var fileUpload1 = document.getElementById('file-upload1');
//var fileUpload2 = document.getElementById('file-upload2');
//var fileUpload3 = document.getElementById('file-upload3');
//fileUploadcod.onchange = function (e) {
//    var fileName = $(this).val().split("\\").pop();
//    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//    readFile(e.srcElement, 1);
//}
//fileUpload1.onchange = function (e) {
//    var fileName = $(this).val().split("\\").pop();
//    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//    readFile(e.srcElement, 2);
//}
//fileUpload2.onchange = function (e) {
//    var fileName = $(this).val().split("\\").pop();
//    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//    readFile(e.srcElement, 3);
//}
//fileUpload3.onchange = function (e) {
//    var fileName = $(this).val().split("\\").pop();
//    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//    readFile(e.srcElement, 4);
//}

//$('#file-preview-zone1').on("click", function (e) {
//    rotation1 += 90;
//    $('#file-preview1').rotate(rotation1);
//    if (rotation1 == 360) {
//        rotation1 = 0;
//    }
//});
//$('#file-preview-zone2').on("click", function (e) {
//    rotation2 += 90;
//    $('#file-preview2').rotate(rotation2);
//    if (rotation2 == 360) {
//        rotation2 = 0;
//    }
//});
//$('#file-preview-zone3').on("click", function (e) {
//    rotation3 += 90;
//    $('#file-preview3').rotate(rotation3);
//    if (rotation3 == 360) {
//        rotation3 = 0;
//    }
//});
//$('#file-preview-zone4').on("click", function (e) {
//    rotation4 += 90;
//    $('#file-preview4').rotate(rotation4);
//    if (rotation4 == 360) {
//        rotation4 = 0;
//    }
//});
    //function GuardarFirma() {
    //    var canvas = document.getElementById("firmacanvas");
    //    var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', '');
    //    var formData = new FormData();
    //    formData.append('imagen', image);
    //    formData.append('IdCabecera', IdCabecera);
    //    formData.append('Tipo', 'Control');
    //    $.ajax({
    //        type: 'POST',
    //        url: '/EvaluacionProductoEnfundado/GuardarImagenFirma',
    //        data: formData,
    //        processData: false,
    //        contentType: false,
    //        success: function (result) {
    //            if (result == "101") {
    //                window.location.reload();
    //            }
    //            //console.log(result);
    //            //$('#div_ImagenFirma').empty();
    //            $('#div_ImagenFirma').prop('hidden', false);
    //            //var img = $('<img />', { id: 'Myid', src: result, alt: 'MyAlt', width: '400px', height: '200px' }).appendTo($('#div_ImagenFirma'));
    //            document.getElementById('ImgFirma').src = result;
    //            $('#signature-pad').prop('hidden', true);
    //            MensajeCorrecto("Firma ingresada Correctamente");
    //        }
    //    });
    //}
    //function ConsultarFirma() {
    //    $.ajax({
    //        url: "../EvaluacionProductoEnfundado/ConsultarFirma",
    //        type: "GET",
    //        data: {
    //            IdCabecera: IdCabecera
    //        },
    //        success: function (resultado) {
    //            if (resultado == "101") {
    //                window.location.reload();
    //            }
    //            if (resultado != '0') {
    //                document.getElementById('ImgFirma').src = resultado;
    //                $('#div_ImagenFirma').prop('hidden', false);
    //                $('#signature-pad').prop('hidden', true);
    //            } else {
    //                $('#signature-pad').prop('hidden', false);
    //            }
    //        },
    //        error: function (resultado) {
    //            MensajeError("Error: Comuníquese con sistemas", false);

    //        }
    //    });
    //}
    //function VolverAFirmar() {
    //    $('#div_ImagenFirma').prop('hidden', true);
    //    $('#signature-pad').prop('hidden', false);
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
