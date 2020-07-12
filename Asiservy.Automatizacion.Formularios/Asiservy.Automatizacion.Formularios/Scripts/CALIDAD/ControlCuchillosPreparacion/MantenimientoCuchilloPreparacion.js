var itemEditar=[];
$(document).ready(function () {
    ListarCuchillocPreparacion(0);   
});

function ListarCuchillocPreparacion(op) {   
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarCuchilloPreparacionPartial",
        type: "GET",
        data: {
            codigoCuchillo: "",
            op:op
        },
        success: function (resultado) {            
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#DivCargarCuchillos').html('No hay datos en la consulta: '+resultado);
            } else {              
                $('#DivCargarCuchillos').html(resultado);
            }
            $('#cargac').hide(); 
            itemEditar = [];
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ConsultarCuchillo(op) {
    $("#txtDescripcion").val('');
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarCuchilloPreparacion",
        type: "GET",
        data: {
            codigoCuchillo: $("#txtCodigoCuchillo").val().toUpperCase(),
            op: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado.length!=0) {
                $("#txtDescripcion").val(resultado[0].DescripcionCuchillo);
                itemEditar = resultado;
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarModificarCuchilloPreparacion() {   
    if ($("#txtCodigoCuchillo").val().toUpperCase()=='') {
        MensajeAdvertencia("¡El código del cuchillo no puede estar vacío!");
        return;
    }   
    $.ajax({
        url: "../ControlCuchillosPreparacion/GuardarModificarCuchilloPreparacion",
        type: "POST",
        data: {
            IdCuchilloPreparacion: itemEditar.IdCuchilloPreparacion,
            CodigoCuchillo: $("#txtCodigoCuchillo").val().toUpperCase(),
            DescripcionCuchillo: $("#txtDescripcion").val(),
            EstadoRegistro: 'A'
        },
        success: function (resultado) {
            $("#ModalIngresoRegistro").modal('hide');
            listaDatos = resultado;
            LimpiarCabecera();
            ListarCuchillocPreparacion(0);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraSi() {   
    $.ajax({
        url: "../ControlCuchillosPreparacion/GuardarModificarCuchilloPreparacion",
        type: "POST",
        data: {
            CodigoCuchillo: itemEditar.CodigoCuchillo.toUpperCase(),
            DescripcionCuchillo: itemEditar.DescripcionCuchillo.toUpperCase(),
            EstadoRegistro: itemEditar.EstadoRegistro 
        },
        success: function (resultado) {
            $("#ModalIngresoRegistro").modal('hide');
            listaDatos = resultado;
            LimpiarCabecera();
            ListarCuchillocPreparacion(0);
            EliminarCabeceraNo();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function LimpiarCabecera() {
    $("#txtCodigoCuchillo").val('');
    $("#txtDescripcion").val('');   
}

function CambioEstado(valor) {
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');
}

function SelecionarFila(jdata) {    
    ModalNuevoRegistro();
    $("#txtDescripcion").val(jdata.DescripcionCuchillo);
    $("#txtCodigoCuchillo").val(jdata.CodigoCuchillo);
    $("#ModalCheckBox").show();
    //if (EstadoRegistro == 'A') {
    //    $('#CheckEstadoRegistroOp').prop('checked', true);
    //    $('#LabelEstado').text('Activo');//.addClass('badge badge-success');
    //} else {
    //    $('#CheckEstadoRegistroOp').prop('checked', false);
    //    $('#LabelEstado').text('Inactivo');//.addClass('badge badge-danger');
    //} 
}

function ModalNuevoRegistro() {
    $("#ModalIngresoRegistro").modal('show');
    LimpiarCabecera();    
}

function InactivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}

function ActivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.EstadoRegistro = 'A';
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}