var ListaLotes;
var Error = 0;
var IdCabecera = 0;
var IdDetalle = 0;
var rotation = 0;
var IdFotoEvaluacioLomosyMigas = 0;
var TotalMaximo;
var ParametrosLomo =
{
    Limpieza1: {
        Venas: 8,
        Espinas: 10,
        Moretones: 9,
        Escamas: 3,
        Piel: 5,
        Total:35
    },
    Limpieza2: {
        Venas: 6,
        Espinas: 8,
        Moretones: 5,
        Escamas: 2,
        Piel: 4,
        Total:25
    },
    Limpieza3: {
        Venas: 1,
        Espinas: 3,
        Moretones: 3,
        Escamas: 0,
        Piel: 0,
        Total:7
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
    
    $('#txtVenas').mask("9?9");
    $('#txtEspinas').mask("9?9");
    $('#txtSangre').mask("9?9");
    $('#txtEscamas').mask("9?9");
    $('#txtPiel').mask("9?9");
    $('#txtTrozos').mask("9?.99");
    
    LLenarComboOrdenes();
    //.then(function () {
    //    ConsultarCabControl();
    //})
   
});
$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});
$("#Lomo").on("change", function () {
    ValidarParametro();
});
$("#Miga").on("change", function () {
    ValidarParametro();
});
$("#cmbNivelLimpieza").on("change", function () {
    ValidarParametro();
});
$("#btncancelarguardarfoto").on("click", function () {
    IdFotoEvaluacioLomosyMigas = 0;
});

$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").prop('selectedIndex')==0) {
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
function ValidarParametro() {
    $('#lblparametro').text("");
    //console.log($('#Lomo').val());
    //console.log($('#Miga').val());
    if ($('#Lomo').is(':checked') || $('#Miga').is(':checked') && ($('#cmbNivelLimpieza').prop('selectedIndex') != 0)) {
        if ($('#Lomo').is(':checked')) {
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
async function CargarControlDetalle2Ajax() {
    $("#divTableDetalle2").html('');
    let params = {
        IdDetalle: IdDetalle
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionDeLomoyMigaEnBandeja/DetalleFotoPartial?' + query;
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
        MensajeCorrecto(ResultadoGuardarFoto);
    } catch (ex) {

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
        if (MensajeCorrecto(ResultadoGuardarFoto[0] == '003')){
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
    data.append("IdDetalleEvaluacionLomosyMigas", IdDetalle);
    data.append("IdFotoEvaluacioLomosyMigas", IdFotoEvaluacioLomosyMigas);
    data.append("Observacion", $("#txtNovedad").val());
    data.append("Rotacion", rotation);

    var promesa = fetch("../EvaluacionDeLomoyMigaEnBandeja/GuardarFotoDetalle", {
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
function NuevoControlDetalle2() {
    $("#txtIdControlDetalle2").val("");
    $("#txtNovedad").val("");
    $("#file-upload").val('');
    $("#file-preview-zone").html('');
    $('#lblfoto').text('Seleccione archivo');
    rotation = 0;
    
}
function ModalGenerarControlDetalle2() {
    IdFotoEvaluacioLomosyMigas = 0;
    NuevoControlDetalle2();
    $("#ModalGenerarControlDetalle2").modal("show");
    
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
            console.log(ResultadoConsultar);
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
    

}
function DatosOrdenFabricacion() {
    Error = 0;
    $('#cargac').show();
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
                    //console.log(resultado);
                    //$.each(resultado, function (key, value) {
                    //    $('#cmbLote').append('<option value=' + value.Codigo + '>' + value.Descripcion + '</option>');
                    //});
                    //console.log(resultado);

                }
                $('#cargac').hide();
            })
            .catch(function (resultado) {
                //console.log(resultado);
                MensajeError(resultado.responseText, false);
                $('#cargac').hide();
            })
   

    //$('#txtCliente').val(Datos.NombreCliente);
    //LlenarComboLotes($("#cmbOrdeneFabricacion").val());
}
async function ConsultarCabControlAjax() {
    const data = new FormData();
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    data.append('OrdenFabricacion', $("#cmbOrdeneFabricacion").val());
    data.append('Turno', $("#cmbTurno").val());
    var promesa=fetch("../EvaluacionDeLomoyMigaEnBandeja/ConsultarCabeceraControl", {
        method: 'POST',
        body: data
    })
    return promesa;
}
async function ConsultarCabControl(bandera) {
    
    try {
       
       
        if ($('#txtFechaProduccion').val() == '') {
            $('#msjErrorFechaProduccion').prop('hidden', false);
            return;
        }
        else {
            
            $('#msjErrorFechaProduccion').prop('hidden', true);
        }
        if ($('#cmbOrdeneFabricacion').val() == '') {
            $('#msjerrorordenfb').prop('hidden', false);
            return;
        } else {
            $('#msjerrorordenfb').prop('hidden', true);
        }
        if ($('#cmbTurno').prop('selectedIndex') == 0) {
            console.log("hola");
            $('#msjTurno').prop('hidden', false);
            return;
        } else {
            $('#msjTurno').prop('hidden', true);
        }
        
        if (bandera != 'of')//bandera para que solo se ejecute si se llama desde onchange de fecha, y no por onchange de orden de fabricacion
        {
            await LLenarComboOrdenes();
        }
        $('#btnCargando').prop('hidden', false);
        $('#btnConsultar').prop('hidden', true);
        $('#btnLimpiar').prop('hidden', true);
        $('#btnEliminarCabeceraControl').prop('hidden', true);
        $('#btnGuardar').prop('hidden', true);
        var PromesaConsultar = await ConsultarCabControlAjax();
        if (!PromesaConsultar.ok) {
            throw "Error";
        }
        var ResultadoConsulta = await PromesaConsultar.json();
        if (ResultadoConsulta != "0") {

            $('#mensajeRegistros').prop('hidden', true);
            IdCabecera = ResultadoConsulta.IdEvaluacionDeLomosYMigasEnBandejas;
            $('#cmbOrdeneFabricacion').val(ResultadoConsulta.OrdenFabricacion);
            $('#txtCliente').val(ResultadoConsulta.Cliente);
            if (ResultadoConsulta.Lomo) {
                $('#Lomo').prop('checked', true);
            }
            if (ResultadoConsulta.Miga) {
                $('#Miga').prop('checked', true);
            }
            if (ResultadoConsulta.Empaque) {
                $('#Empaque').prop('checked', true);
            }
            if (ResultadoConsulta.Enlatado) {
                $('#Enlatado').prop('checked', true);
            }
            if (ResultadoConsulta.Pouch) {
                $('#Pouch').prop('checked', true);
            }
            if (ResultadoConsulta.EstadoControl == true) {
                $("#estadocontrol").removeClass("badge-danger").addClass("badge-success");
                $('#estadocontrol').text(Mensajes.Aprobado);
            }
            else {
                $("#estadocontrol").removeClass("badge-success").addClass("badge-danger");
                $('#estadocontrol').text(Mensajes.Pendiente);
            }
            $('#cmbNivelLimpieza').val(ResultadoConsulta.NivelLimpieza);
            $('#Observacion').val(ResultadoConsulta.Observacion);
            $('#CardDetalle').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            ValidarParametro();
            LlenarComboLotes(ResultadoConsulta.OrdenFabricacion);
            ConsultarDetalleControl();
            //ConsultarDetalleControl();

            SlideCabecera();
        } else {
            $('#brespacio').remove();
            $('#DivCabecera').after('<br id="brespacio">');
            $('#mensajeRegistros').prop('hidden', false);
            $('#mensajeRegistros').text(Mensajes.SinRegistros);
            $('#DivDetalles').empty();
            LimpiarControles();

            $('#CardDetalle').prop('hidden', true);

        }
        DatosOrdenFabricacion();
       
    } catch (ex) {
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }
    $('#btnCargando').prop('hidden', true);
    $('#btnConsultar').prop('hidden', false);
    $('#btnLimpiar').prop('hidden', false);
    $('#btnEliminarCabeceraControl').prop('hidden', false);
    $('#btnGuardar').prop('hidden', false);
   
}
function GuardarCabceraControl() {
    var Lomo = false;
    var Miga = false;
    var Empaque = false;
    var Enlatado = false;
    var Pouch = false;
    if ($('#cmbNivelLimpieza').prop('selectedIndex') == 0) {
        $('#msgerrorniveldelimpieza').prop('hidden', false);
        return;
    } else {
        $('#msgerrorniveldelimpieza').prop('hidden', true);
    }
    if (!$('#Lomo').is(':checked') && !$('#Miga').is(':checked')) {
        $('#msjerrorProducto').prop('hidden', false);
        return;
    } else {
        $('#msjerrorProducto').prop('hidden', true);
    }
    if (!$('#Empaque').is(':checked') && !$('#Enlatado').is(':checked') && !$('#Pouch').is(':checked')) {
        $('#msjerrorDestino').prop('hidden', false);
        return;
    } else {
        $('#msjerrorDestino').prop('hidden', true);
    }
    if ($('#txtFechaProduccion').val() == '') {
        $('#msjErrorFechaProduccion').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorFechaProduccion').prop('hidden', true);
    }
    if ($('#cmbOrdeneFabricacion').val()=='') {
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
    const data = new FormData();
    data.append('IdEvaluacionDeLomosYMigasEnBandejas', IdCabecera);
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    data.append('Turno', $("#cmbTurno").val());
    data.append('Cliente', $("#txtCliente").val());
    data.append('OrdenFabricacion', $("#cmbOrdeneFabricacion").val());
    data.append('NivelLimpieza', $("#cmbNivelLimpieza").val());
    if ($('#Lomo').is(":checked")) {
        Lomo = true;
    }
    if ($('#Miga').is(":checked")) {
        Miga = true;
    }
    if ($('#Empaque').is(":checked")) {
        Empaque = true;
    }
    if ($('#Enlatado').is(":checked")) {
        Enlatado = true;
    }
    if ($('#Pouch').is(":checked")) {
        Pouch = true;
    }
    data.append('Lomo', Lomo);
    data.append('Miga', Miga);
    data.append('Empaque', Empaque);
    data.append('Enlatado', Enlatado);
    data.append('Pouch', Pouch);
    data.append('Observacion', $('#Observacion').val());
    fetch("../EvaluacionDeLomoyMigaEnBandeja/GuardarCabeceraControl", {
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
            IdCabecera = resultado[2].IdEvaluacionDeLomosYMigasEnBandejas;
            if (resultado[0] == "002" || resultado[0] == "003") {
                MensajeAdvertencia(resultado[1]);
                (resultado[2].Lomo) ? $('#Lomo').prop("checked", true) : $('#Lomo').prop("checked", false);
                (resultado[2].Miga) ? $('#Miga').prop("checked", true) : $('#Miga').prop("checked", false);
                (resultado[2].Empaque) ? $('#Empaque').prop("checked", true) : $('#Empaque').prop("checked", false);
                (resultado[2].Enlatado) ? $('#Enlatado').prop("checked", true) : $('#Enlatado').prop("checked", false);
                (resultado[2].Pouch) ? $('#Pouch').prop("checked", true) : $('#Pouch').prop("checked", false);
                $('#cmbNivelLimpieza').val(resultado[2].NivelLimpieza);
                $('#Observacion').val(resultado[2].Observacion);
                ValidarParametro();
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
        LlenarComboLotes($('#cmbOrdeneFabricacion').val());
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function LimpiarControles() {
    //console.log($("#cmbOrdeneFabricacion").val());
    IdCabecera = 0;
    Error = 0;
    IdDetalle = 0;
    $('#estadocontrol').text('');
    $('#cmbNivelLimpieza').prop('selectedIndex',0);
    $('#EtiquetaEstadoControl').html('');
    //$('#txtFechaProduccion').val('');
    //$('#cmbOrdeneFabricacion').empty();
    //$('#cmbOrdeneFabricacion').append('<option>Seleccione...</option>');
    //$('#txtCliente').val('');
    $('#txtFechaProduccion').prop('disabled', false);
    //$('#mensajeRegistros').prop('hidden', true);
    $('#Lomo').prop('checked', false);
    $('#Miga').prop('checked', false);
    $('#Empaque').prop('checked', false);
    $('#Enlatado').prop('checked', false);
    $('#Pouch').prop('checked', false);
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


    fetch("../EvaluacionDeLomoyMigaEnBandeja/EliminarCabeceraControl", {
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
function EliminarFoto() {
    //console.log('Eliminar');
    $('#modal-detalle2-si').prop('disabled', true);
    $('#modal-detalle2-no').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdFoto', IdFotoEvaluacioLomosyMigas);


    fetch("../EvaluacionDeLomoyMigaEnBandeja/EliminarFotoDetalle", {
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

            if (resultado[0] == '003') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
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
function DatosLote(){
     var Datos = Enumerable.From(ListaLotes)
         .Where(function (x) { return x.descripcion == $('#cmbLote').val() })
        .Select(function (x) { return x })
        .FirstOrDefault();
    $('#txtBuque').val(Datos.Barco);
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
    if ($('#cmbLinea').prop('selectedIndex') == 0) {
        $('#msjLinea').prop('hidden', false);
        ActivaInfo();
        return false;
    } else {
        $('#msjLinea').prop('hidden', true);
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
        return;
    } else {
        $('#msjMoreton').prop('hidden', true);
    }
  

    $('#btnEliminarDetalleControl').prop('hidden', true);
    $('#btnGuardarDetalle').prop('hidden', true);
    $('#btnLimpiarDetalle').prop('hidden', true);
    $('#btnCargandoSubDetalle').prop('hidden', false)
    const data = new FormData();
    data.append('IdDetalleEvaluacionLomoyMigasEnBandeja', IdDetalle);
    data.append('Hora', $("#txtHora").val());
    data.append('Linea', $("#cmbLinea").val());
    data.append('Lote', $("#cmbLote").val());
    data.append('buque', $("#txtBuque").val());
    data.append('Sabor', $("#cmbSabor").val());
    data.append('Textura', $("#cmbTextura").val());
    data.append('Color', $("#cmbColor").val());
    data.append('Olor', $("#cmbOlor").val());
    data.append('Moretones', $("#cmbMoreton").val());
    //data.append('HematomasProfundos', $("#txtHematomas").val());
    data.append('Proteina', $("#cmbProteina").val());
    data.append('Trozo', $("#txtTrozos").val());
    data.append('Venas', $("#txtVenas").val());
    data.append('Espinas', $("#txtEspinas").val());
    data.append('Sangre', $("#txtSangre").val());
    data.append('Escamas', $("#txtEscamas").val());
    data.append('Piel', $("#txtPiel").val());
    data.append('IdCabeceraEvaluacionLomosYMigasEnBandeja', IdCabecera);

    fetch("../EvaluacionDeLomoyMigaEnBandeja/GuardarDetalleControl", {
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

            if (resultado[0] == "002" || resultado[0] == "003") {
                MensajeAdvertencia(resultado[1]);
            }
            if (resultado[0] == "000" || resultado[0] == "001") {
                MensajeCorrecto(resultado[1]);
                //$('#CardDetalle').prop('hidden', false);
                //ConsultarDetalleControl();
                ConsultarDetalleControl();
            }
         
            LimpiarDetalleControles();
        }
        $('#btnGuardarDetalle').prop('hidden', false);
        $('#btnLimpiarDetalle').prop('hidden', false);
        $('#btnCargandoSubDetalle').prop('hidden', true);
        $('#btnEliminarDetalleControl').prop('hidden', false);
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function ConsultarDetalleControl() {
    $('#cargac').show();
    Error = 0;
    let params = {
        IdCabeceraControl: IdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionDeLomoyMigaEnBandeja/PartialDetalleControl?' + query;
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
                $('#DivDetalles').empty();
                $('#DivDetalles').html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#TableDetalle').DataTable(config.opcionesDT);
                $('#brespacio').remove();
                LimpiarDetalleControles();
                //ConsultarFirma();
               
            } else {
                $('#DivDetalles').empty();
                LimpiarDetalleControles();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#cargac').hide();
        })
    
}
function LimpiarDetalleControles() {
    $('#divDetalle2').prop('hidden', true);
    $('#txtHora').prop('disabled', false);
    $('#btnEliminarDetalleControl').prop('disabled', true);
    $('#txtHora').val(moment().format("HH:mm"));
    $('#txtBuque').val('');
    $('#cmbMoreton').prop('selectedIndex',0);
    //$('#txtHematomas').val('');
    $('#txtTrozos').val('');
    $('#txtVenas').val('');
    $('#txtEspinas').val('');
    $('#txtSangre').val('');
    $('#txtEscamas').val('');
    $('#txtPiel').val('')
    $('#cmbProteina').prop('selectedIndex', 0);
    $('#cmbLinea').prop('selectedIndex', 0);
    $('#cmbLote').prop('selectedIndex', 0);
    $('#cmbSabor').prop('selectedIndex', 0);
    $('#cmbTextura').prop('selectedIndex', 0);
    $('#cmbColor').prop('selectedIndex', 0);
    $('#cmbOlor').prop('selectedIndex', 0);
    IdDetalle = 0;
    IdFotoEvaluacioLomosyMigas = 0;
    ActivaInfo();

}
async function ModificarDetalle(data) {
    IdFotoEvaluacioLomosyMigas = 0;
    $('#divDetalle2').prop('hidden', false);
    IdDetalle = data.IdDetalle;
    await CargarControlDetalle2();
    //console.log(data.Hora);
    $('#txtHora').val(data.Hora);

    $('#txtBuque').val(data.Buque);
    $('#cmbMoreton').val(data.CodMoretones);
    //$('#txtHematomas').val(data.HematomasProfundos);
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
function ConfirmarEliminarDetalle() {
    $('#ModalEliminarDetalle').modal('show');
}
function EliminarDetalle() {
    $('#btnsicdet').prop('disabled', true);
    $('#btnnodet').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdDetalle', IdDetalle);


    fetch("../EvaluacionDeLomoyMigaEnBandeja/EliminarDetalleControl", {
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
            MensajeCorrecto(resultado[1]);

            LimpiarDetalleControles();
            ConsultarDetalleControl();
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
function SlideCabecera() {
    $("#DivCabecera").slideToggle("fast");
}
function ModificarFoto(data) {
    //console.log(data);

    IdFotoEvaluacioLomosyMigas = data.IdFotoEvaluacioLomosyMigas;
    NuevoControlDetalle2();
    
    $("#txtNovedad").val(data.Observacion);
    if (data.Imagen != null && data.Imagen != '') {
        var re = 'EvaluacionDeLomosYMigasEnBandeja/EvaluacionDeLomosYMigasEnBandeja';
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
    $('#labelMensaje2').text('Novedad: '+ data.Observacion);
    $('#modalEliminarControlDetalle2').modal('show');
    IdFotoEvaluacioLomosyMigas = data.IdFotoEvaluacioLomosyMigas;
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

//function GuardarFirma() {
//    var canvas = document.getElementById("firmacanvas");
//    var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', '');
//    var formData = new FormData();
//    formData.append('imagen', image);
//    formData.append('IdCabecera', IdCabecera);
//    formData.append('Tipo', 'Control');
//    $.ajax({
//        type: 'POST',
//        url: '/EvaluacionDeLomoyMigaEnBandeja/GuardarImagenFirma',
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
//        url: "../EvaluacionDeLomoyMigaEnBandeja/ConsultarFirma",
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

