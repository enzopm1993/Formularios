$(document).ready(function () {
    cargarpartial();
});

function ValidateNumber(event) {

    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;

    if (!regex.test(key)) {
        theEvent.preventDefault ? theEvent.preventDefault() : (theEvent.returnValue = false);
    }
}
function EditarCabeceraToalla() {
    $('#ModalEditCabToalla').modal('show');
}
function ConfirmarInactivar() {
    $('#ModalMsjInactivar').modal('show');
}
function InactivarControlToalla() {
    $.ajax({
        url: "../ControlToalla/InactivarControlToalla",
        type: "POST",
        data: {
            IdCabToalla: $('#CodCabecera').val(),
            
        },
        success: function (resultado) {
            $('#headercabToalla').empty();
            $('#DivDetToalla').empty();
            $('#DivDetToalla').hide();
            cargarpartial();
            $('#DivControl').show();
            $('#DivCabToalla').show("slow");
            $('#btnGuardar').prop("hidden", false);
            $('#btnInactivar').prop("hidden", true);
            $('#btnEditar').prop("hidden", true);
            $('#btnAtras').prop("hidden", true);
            $('#txtObservacion').val('');

        },
        error: function (resultado) {
            //$('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);
          
        }
    });
}
function checkControlToalla(IdCheck,IdDetalleToalla) {
    if ($('#' + IdCheck).prop('checked')) {
        $('#NumToalla' + IdDetalleToalla).prop('disabled', true); 
        $('#labelCheck-' + IdDetalleToalla).css("background", "#28B463");
        $.ajax({
            url: "../ControlToalla/GuardarDetalleToalla",
            type: "POST",
            data: {
                IdDetalleToalla: IdDetalleToalla,
                NumToalla: $('#NumToalla' + IdDetalleToalla).val()
            },
            success: function (resultado) {
                //$('#spinnerCargando').prop("hidden", true);
                //$('#DivDetToalla').html(resultado);
                //$('#DivDetToalla').show();
            },
            error: function (resultado) {
                //$('#spinnerCargando').prop("hidden", true);
                MensajeError(resultado.responseText, false);
                $('#Check'+IdDetalleToalla).prop('checked',false)
                $('#labelCheck-' + IdDetalleToalla).css("background", "#7b8a8b");
                $('#NumToalla' + IdDetalleToalla).prop('disabled', false); 
            }
        });
    } else {
        $('#labelCheck-' + IdDetalleToalla).css("background", "#7b8a8b");
        $('#NumToalla' + IdDetalleToalla).prop('disabled', false); 
        
        $.ajax({
            url: "../ControlToalla/GuardarDetalleToalla",
            type: "POST",
            data: {
                IdDetalleToalla: IdDetalleToalla,
                NumToalla: ''
            },
            success: function (resultado) {
                //$('#spinnerCargando').prop("hidden", true);
                //$('#DivDetToalla').html(resultado);
                //$('#DivDetToalla').show();
                $('#NumToalla' + IdDetalleToalla).val('');
            },
            error: function (resultado) {
                //$('#spinnerCargando').prop("hidden", true);
                MensajeError(resultado.responseText, false);
                $('#NumToalla' + IdDetalleToalla).prop('disabled', true);
                $('#Check' + IdDetalleToalla).prop('checked', true)
                $('#labelCheck-' + IdDetalleToalla).css("background", "#28B463");

            }
        });
    }
}
function Atras() {
    $('#headercabToalla').empty();
    $('#DivCabToalla').show();
    $('#DivDetToalla').hide();

    $('#btnGuardar').prop("hidden", false);
    $('#btnInactivar').prop("hidden", true);
    $('#btnEditar').prop("hidden", true);
    $('#btnAtras').prop("hidden", true);
    cargarpartial();
    $('#DivControl').show();
}
function obtenerfechaFormato(date) {
    var fecha = new Date(date);
    let day = fecha.getDate();
    let month = fecha.getMonth() + 1;
    let year = fecha.getFullYear();

    if (month < 10) {
        month = '0' + month;
    } 
    if (day < 10) {
        day = '0' + day;
    }
    return year + '-' + month + '-' + day;
}
function ConsultarDetallexIDToalla(idCabeceraToalla,fecha,turno,hora,observacion) {
    
   
    $('#spinnerCargando').prop("hidden", false);
    $.ajax({
        url: "../ControlToalla/PartialDetalleToalla",
        type: "POST",
        data: {
            IdCabToalla: idCabeceraToalla
        },
        success: function (resultado) {
            $('#headercabToalla').html(obtenerfechaFormato(fecha) +'  '+ hora.slice(0, -3));//para agregar la fecha y la hora de la cabecera seleccionada
            $('#CodCabecera').val(idCabeceraToalla);
            var fechaformato = obtenerfechaFormato(fecha)
            $('#txtFechaEdit').val(fechaformato);
            $('#TurnoGenEdit').val(turno);
            console.log(fechaformato);
            $('#txtHoraEdit').val(hora.slice(0,-3));
            $('#txtobservacionEdit').val(observacion);

            $('#DivControl').hide();
            $('#DivCabToalla').hide("slow");
            $('#btnGuardar').prop("hidden", true);
            $('#btnInactivar').prop("hidden", false);
            $('#btnEditar').prop("hidden", false);
            $('#btnAtras').prop("hidden", false);

            $('#spinnerCargando').prop("hidden", true);
            $('#DivDetToalla').html(resultado);
            $('#DivDetToalla').show();
        },
        error: function (resultado) {
            $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);
         

        }
    });
}
function cargarpartial() {
    $('#spinnerCargando').prop("hidden", false);
    $.ajax({
        url: "../ControlToalla/PartialControlToalla",
        type: "POST",
        data: {
            Turno: $('#TurnoGen').val(),
            Fecha: $("#txtFecha").val(),
            //Hora: $('#txtHora').val(),
            Linea: $("#txtLinea").val(),
            //Observacion: $("#txtObservacion").val(),
        },
        success: function (resultado) {
            //CargarControlCoche();
            //MensajeCorrecto(resultado);
            $('#spinnerCargando').prop("hidden", true);
            $('#DivControl').html(resultado);
            //Nuevo();
            //$("#btnGuardar").prop("disabled", false);

        },
        error: function (resultado) {

            //CargarControlCoche();
            //$("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);

            MensajeError(resultado.responseText, false);
            //Nuevo();

        }
    });
}
function GuardarControl() {
    if ($('#txtFecha').val() == '') {
        MensajeAdvertencia("Debe ingresar la fecha", false);
        return false;
    }
    $("#btnGuardar").prop("disabled", true);
    $('#spinnerCargando').prop("hidden", false);
    //var DivControl = $('#DivTableControlCoche');
    $('#DivControl').empty();
    $.ajax({
        url: "../ControlToalla/GuardarControlToalla",
        type: "POST",
        data: {
            Turno: $('#TurnoGen').val(),
            Fecha: $("#txtFecha").val(),
            Hora: $('#txtHora').val(),
            Linea: $("#txtLinea").val(),
            Observacion: $("#txtObservacion").val(),
            estadoreg:'A'
        },
        success: function (resultado) {
            //CargarControlCoche();
            //MensajeCorrecto(resultado);
            //Nuevo();
            $("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);
            if (resultado == '999') {
                MensajeAdvertencia("Ya existe un registro en la hora indicada", false);
            } 
                cargarpartial();
            

        },
        error: function (resultado) {

            //CargarControlCoche();
            $("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);

            MensajeError(resultado.responseText, false);
            //Nuevo();

        }
    });
}
function ActualizarControlToallaCab() {
    $.ajax({
        url: "../ControlToalla/GuardarControlToalla",
        type: "POST",
        data: {
            Turno: $('#TurnoGenEdit').val(),
            Fecha: $("#txtFechaEdit").val(),
            Hora: $('#txtHoraEdit').val(),
            Observacion: $("#txtobservacionEdit").val(), 
            id: $("#CodCabecera").val(),
            estadoRegistro:'A'
        },
        success: function (resultado) {
            //CargarControlCoche();
            //MensajeCorrecto(resultado);
            //Nuevo();
            
            
            //$("#btnGuardar").prop("disabled", false);
            //$('#spinnerCargando').prop("hidden", true);
            if (resultado == '555') {
                MensajeAdvertencia("Ya existe un registro en esa fecha, hora y turno", false);
            } else {
                $('#ModalEditCabToalla').modal('hide');
                MensajeCorrecto(resultado,false);
            }
            cargarpartial();


        },
        error: function (resultado) {

            //CargarControlCoche();
            //$("#btnGuardar").prop("disabled", false);
            //$('#spinnerCargando').prop("hidden", true);

            MensajeError(resultado.responseText, false);
            //Nuevo();

        }
    });
}