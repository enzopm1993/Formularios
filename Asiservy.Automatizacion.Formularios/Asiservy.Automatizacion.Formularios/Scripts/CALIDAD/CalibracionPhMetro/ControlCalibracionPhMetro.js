﻿IdControl = 0;
$(document).ready(function () {
    //$('#txtPh4').mask("9?.99");
    //$('#txtPh7').mask("9?.99");
    //$('#txtPh10').mask("99?.99");
    $('#txtPh4').inputmask({
        alias: "integer",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 9.99
    });;
    $('#txtPh7').inputmask({
        alias: "integer",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 9.99
    });
    $('#txtPh10').inputmask({
        alias: "integer",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 99.99
    });
    ConsultarControl();
});
function Validar() {
    var validar = true;
    if ($('#txtFecha').val() == '') {
        $('#msjfechaerror').prop('hidden', false);
        validar = false;
    }
    if ($('#txtHora').val()=='') {
        validar = false;
        $('#msjhoraerror').prop('hidden', false);
    }
    if ($('#txtCodigoPhMetro').val() == '') {
        validar = false;
        $('#msjcodigoerror').prop('hidden', false);
    }
    if ($('#txtPh4').val() == '') {
        $('#msjph40rror').prop('hidden', false);
        validar = false;
    }
    if ($('#txtPh7').val() == '') {
        $('#msjph70error').prop('hidden', false);
        validar = false;
    }
    if ($('#txtPh10').val() == '') {
        $('#msjph10error').prop('hidden', false);
        validar = false;
    }
    return validar;
}
async function GuardarControlAjax() {
    var promesa= fetch("../CalibracionPhMetro/GuardarControl", {
        method: 'POST',
        body: JSON.stringify({  IDPhMetro:IdControl,
                                Fecha: $('#txtFecha').val(),
                                Hora: $('#txtHora').val(),
                                CodigoPhMetro: $('#txtCodigoPhMetro').val(),
                                Ph40: $('#txtPh4').val(),
                                ph70: $('#txtPh7').val(),
                                ph10: $('#txtPh10').val(),
                                observacion: $('#txtObservacion').val(),
        }),
        headers: {'Content-type':'application/json'}
    })
    return promesa;
}
async function GuardarControl() {
    try {
        if (!Validar()) {
            return;
        }
        $('#cargac').show();
        var PromesaGuardar = await GuardarControlAjax();
        $('#cargac').hide();
        if (!PromesaGuardar.ok) {
            throw Error;
        }
        var RespuestaGuardar = await PromesaGuardar.json();
        //console.log(RespuestaGuardar);
        if (RespuestaGuardar == "101") {
            window.location.reload();
        }
        IdControl = RespuestaGuardar[2].IDPhMetro;
        if (RespuestaGuardar[0] == "002" || RespuestaGuardar[0] == "444") {
            MensajeAdvertencia(RespuestaGuardar[1]);
            
        } else {
            MensajeCorrecto(RespuestaGuardar[1]);
            $('#msjregistros').html('');
        }
    } catch (error) {
        MensajeError('Error comuníquese con el departamento de Sistemas', false);
    }
}
async function ConsultarControlAjax() {
    let params = {
        Fecha: $('#txtFecha').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../CalibracionPhMetro/ConsultarControl?' + query;
    var promesa = fetch(url)
    return promesa;
}
async function ConsultarControl() {
    try {

        LimpiarControles();
        if ($('#txtFecha').val() == '') {
            $('#msjfechaerror').prop('hidden', false);
            return;
        } else {
            $('#msjfechaerror').prop('hidden', true);
        }
        $('#cargac').show();
        var PromesaConsultar = await ConsultarControlAjax();
        $('#cargac').hide();
        if (!PromesaConsultar.ok) {
            throw Error;
        }
        var RespuestaConsultar = await PromesaConsultar.json();
        //console.log(RespuestaConsultar);
        if (RespuestaConsultar == "101") {
            window.location.reload();
        }
        if (RespuestaConsultar == "0") {
            $('#msjregistros').html(Mensajes.SinRegistros);
            IdControl = 0;
            $('#btnEliminarControl').prop('disabled', true);
            LimpiarControles();
            return;
        } else {
            IdControl = RespuestaConsultar.IDPhMetro;
            $('#txtHora').val(moment(RespuestaConsultar.Hora).format('hh:mm'));
            $('#txtCodigoPhMetro').val(RespuestaConsultar.CodigoPhMetro);
            $('#txtPh4').val(RespuestaConsultar.Ph40);
            $('#txtPh7').val(RespuestaConsultar.ph70);
            $('#txtPh10').val(RespuestaConsultar.ph10);
            $('#txtObservacion').val(RespuestaConsultar.observacion);
            $('#msjregistros').html('');
        }
        if (RespuestaConsultar.EstadoControl == true) {
            $("#estadocontrol").removeClass("badge-danger").addClass("badge-success");
            $('#estadocontrol').text('APROBADO');
            $("#DivContenido :input").prop("disabled", true);
            $('#txtFecha').prop('disabled', false);
            //$('#btnLimpiar').prop('disabled', false);
        } else if (RespuestaConsultar != 0) {
            $("#DivContenido :input").prop("disabled", false);
            $('#estadocontrol').text('PENDIENTE');
            $("#estadocontrol").removeClass("badge-success").addClass("badge-danger");

        
        }
        //if (RespuestaConsultar == 0) {
        //    IdControl = 0;
        //    $('#btnEliminarControl').prop('disabled', true);
        //    LimpiarControles();
           
        //} else {
            
        //    IdControl = RespuestaConsultar.IDPhMetro;
        //    $('#txtHora').val(moment(RespuestaConsultar.Hora).format('hh:mm'));
        //    $('#txtCodigoPhMetro').val(RespuestaConsultar.CodigoPhMetro);
        //    $('#txtPh4').val(RespuestaConsultar.Ph40);
        //    $('#txtPh7').val(RespuestaConsultar.ph70);
        //    $('#txtPh10').val(RespuestaConsultar.ph10);
        //    $('#txtObservacion').val(RespuestaConsultar.observacion);
        //}
        if (RespuestaConsultar.EstadoControl != true) {

            $('#btnEliminarControl').prop('disabled', false);
        } 
        
        
    } catch (error) {
        MensajeError('Error comuníquese con el departamento de Sistemas'+error, false);
    }
}
function LimpiarControles() {
    $("#DivContenido :input").prop("disabled", false);
    $('#btnEliminarControl').prop('disabled', true);
    $('#estadocontrol').text('');
    $('#txtHora').val(moment().format("hh:mm"));
    $('#txtCodigoPhMetro').val('PH.01.01');
    $('#txtPh4').val('');
    $('#txtPh7').val('');
    $('#txtPh10').val('');
    $('#txtObservacion').val('');
}
function ConfirmarEliminarControl() {
    $('#ModalEliminarControl').modal('show');
}
async function EliminarControlAjax() {
    let params = {
        IdControl: IdControl,
        poFecha: $('#txtHora').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../CalibracionPhMetro/EliminarControl?' + query;
    var promesa = fetch(url)
    return promesa;
}
async function EliminarControl() {
    try {
        $('#cargac').show();
        var PromesaEliminar = await EliminarControlAjax();
        $('#cargac').hide();
        if (!PromesaEliminar.ok) {
            throw Error;
        }
        var RespuestaEliminar = await PromesaEliminar.json();
        //console.log(RespuestaEliminar);
        if (RespuestaEliminar == 101) {
            window.location.reload();
        }
        if (RespuestaEliminar[0] == '002') {
            MensajeCorrecto(RespuestaEliminar[1]);
            $('#ModalEliminarControl').modal('hide');
            LimpiarControles();
            IdControl = 0;
        } else {
            MensajeAdvertencia(RespuestaEliminar[1]);
            $('#ModalEliminarControl').modal('hide');
        }
    } catch (error) {
        MensajeError('Error comuníquese con el departamento de Sistemas' + error, false);
    }
}