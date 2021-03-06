﻿
$(document).ready(function () {
    CargarControlEnfundado();
});


function Clear(id) {
    if ($("#" + id).val() == 0) {
        $("#" + id).val('')
    }

}

function checkControlEnfundado(id, detalle, IdControlEnfundadoDetalle) {
    id = "#" + id;
    label = "#labelCheck-" + detalle;
    var txtFundas = '#Fundas-' + detalle;
    var fundas = $(txtFundas).val();
    if ($(id).prop('checked')) {
        if (fundas > 0) {
            $(label).css("background", "#28B463");
            $(txtFundas).prop("readonly", true);
          
            GuardarControlEnfundado(detalle, fundas, id, IdControlEnfundadoDetalle);
        } else {
            $(id).prop('checked', false);
            $(label).css("background", "#7b8a8b");
            MensajeAdvertencia("Ingrese una cantidad de Fundas");
        }
    } else {
        $(label).css("background", "#7b8a8b");
        $(txtFundas).prop("readonly", false);
  
        GuardarControlEnfundado(detalle, 0, id, IdControlEnfundadoDetalle);

    }
}

function GuardarControlEnfundado(detalle, fundas, id, IdDetalle) {
    label = "#labelCheck-" + detalle;
    var txtFundas = '#Fundas-' + detalle;
    $.ajax({
        url: "../ControlEnfundado/GuardarControlEnfundado",
        type: "POST",
        data: {
            IdControlEnfundadoDetalle: IdDetalle,
            Fundas: fundas,
        
        },
        success: function (resultado) {

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $(txtFundas).prop("readonly", false);          
            $(id).prop('checked', false);
            $(label).css("background", "#7b8a8b");
        }
    });

}

function Atras() {
   // $('#divFiltros').fadeIn("slow");
    $('#btnGenerar').prop("hidden", false);
    $('#btnNuevo').prop("hidden", false);
    $('#btnAtras').prop("hidden", true);
    $('#btnInactivar').prop("hidden", true);
    CargarControlEnfundado();
    Limpiar();
}

function Limpiar() {
    
    $('#txtIdControl').val(0);  
    $('#txtHora').val('00:00');  
    $('#txtFundasTeoricas').val(25);
    $('#txtPeso').val('7.5');
    $('#selectEspecificacionFunda').prop('selectedIndex',0);
    $('#selectLotes').prop('selectedIndex', 0);

    $("#txtFecha").prop("readonly", false);
    $("#txtHora").prop("readonly", false);
    $("#txtFundasTeoricas").prop("readonly", false);
    $("#txtPeso").prop("disabled", false);
    $("#selectLotes").prop("disabled", false);
    $("#selectEspecificacionFunda").prop("disabled", false);

    $('#ValidaFecha').prop('hidden', true);
    $("#txtFecha").css('borderColor', '#ced4da');
    $('#ValidaHora').prop('hidden', true);
    $("#txtHora").css('borderColor', '#ced4da');
    $('#ValidaLote').prop('hidden', true);
    $("#selectLotes").css('borderColor', '#ced4da');
    if ($('#txtFecha').val() == '') {
        $("#ValidaFecha").prop("hidden", false);
        return;
    }
    //
}

function CambioFecha() {
    if ($('#txtFecha').val() == '') {
        $("#ValidaFecha").prop("hidden", false);
        return;
    }
    $("#ValidaFecha").prop("hidden", true);
    CargarLotes();
    CargarControlEnfundado();

}

function CargarLotes() {
    $("#selectLotes").empty();
    $("#selectLotes").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../ControlEnfundado/ConsultarLotes",
        type: "GET",
        data: {
            Fecha: $('#txtFecha').val(),
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectLotes").append("<option value='" + row.descripcion + "'>" + row.descripcion + "</option>")
                });
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);       

        }
    });
}

function CargarControlEnfundado() {
    $("#spinnerCargando").prop("hidden", false);
    $("#DivTableControl").html('');
    $.ajax({
        url: "../ControlEnfundado/ControlEnfundadoPartial",
        type: "GET",
        data: {
            Fecha: $('#txtFecha').val(),          
        },
        success: function (resultado) {
            $("#DivTableControl").html('');
            $("#DivTableControl").html(resultado);
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#DivTableControl").html('');
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function CargarControlEnfundadoDetalle(IdControl,fecha, hora, fundasTeoricas, peso, lote) {
    $('#txtIdControl').val(IdControl);  

    $("#txtFecha").val(fecha);
    $("#txtHora").val(hora);
    $("#txtFundasTeoricas").val(fundasTeoricas);
    $("#txtPeso").val(peso);
    $("#selectLotes").val(lote);

    $("#txtFecha").prop("readonly",true);
    $("#txtHora").prop("readonly", true);
    $("#txtFundasTeoricas").prop("readonly", true);
    $("#txtPeso").prop("disabled", true);
    $("#selectLotes").prop("disabled", true);
   $("#selectEspecificacionFunda").prop("disabled", true);

    $("#spinnerCargando").prop("hidden", false);
    $("#DivTableControl").html('');
    $('#btnNuevo').prop("hidden", true);
    $('#btnAtras').prop("hidden", false);
    $('#btnInactivar').prop("hidden", false);
    $('#btnGenerar').prop("hidden", true);
    //$('#divFiltros').fadeOut("slow");
    $.ajax({
        url: "../ControlEnfundado/ControlEnfundadoDetallePartial",
        type: "GET",
        data: {
            Id: IdControl
        },
        success: function (resultado) {
            $("#DivTableControl").html('');
            $("#DivTableControl").html(resultado);
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#DivTableControl").html('');
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function GenerarControl() {

    var hora = $('#txtHora').val();
    var fecha = $('#txtFecha').val();
    var FundasTeoricas = $('#txtFundasTeoricas').val();
    var Peso = $('#txtPeso').val();    
    var especificacionFunda = $('#selectEspecificacionFunda').val();
    var lote = $('#selectLotes').val();
    if (Validar()) {      
       
       // MostrarModalCargando();
        $('#btnGuardarCargando').prop("hidden", false);
        $('#btnGenerar').prop("hidden", true);
              $.ajax({
                  url: "../ControlEnfundado/GenerarControlEnfundado",
            type: "POST",
            data: {
                Fecha: fecha,
                Hora: hora,
                Lote: lote,
                TeoricoFunda: FundasTeoricas,
                PesoProducto: Peso,
                EspecificacionFunda: especificacionFunda
            },
            success: function (resultado) {

                if (resultado == 0) {
                    MensajeAdvertencia("Ya se ha generado un control con esos parametros");               
                    $('#btnGuardarCargando').prop("hidden", true);
                    $('#btnGenerar').prop("hidden", false);
                    return;
                }               
                //CargarControlEnfundado(fecha);
                $('#btnGenerar').prop("disabled", false);
              //  $('#divFiltros').fadeOut("slow");

                $('#btnGuardarCargando').prop("hidden", true);
                CargarControlEnfundadoDetalle(resultado, fecha, hora, FundasTeoricas, Peso, lote);
           
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
                $('#btnGuardarCargando').prop("hidden", true);
            }
        });
    }
    
   
}


function Validar() {
    var hora = $('#txtHora').val();
    var fecha = $('#txtFecha').val();
    var FundasTeoricas = $('#txtFundasTeoricas').val();
    var Peso = $('#txtPeso').val();
    var EspecificacionFunda = $('#selectEspecificacionFunda').val();
    var Lote = $('#selectLotes').val();
    var bool = true;
    if (fecha == '') {
        $('#ValidaFecha').prop('hidden', false);
        $("#txtFecha").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $('#ValidaFecha').prop('hidden', true);
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if (hora == '' || hora == "00:00") {
        $('#ValidaHora').prop('hidden', false);
        $("#txtHora").css('borderColor', '#FA8072');
        bool = false;

    } else {
        $('#ValidaHora').prop('hidden', true);
        $("#txtHora").css('borderColor', '#ced4da');
    }
    if (FundasTeoricas == '' || FundasTeoricas < 1) {
        $('#ValidaFundasTeoricas').prop('hidden', false);
        $("#txtFundasTeoricas").css('borderColor', '#FA8072');
        bool = false;

    } else {
        $('#ValidaFundasTeoricas').prop('hidden', true);
        $("#txtFundasTeoricas").css('borderColor', '#ced4da');
    }

    if (Lote == '' || Lote == null) {
        $('#ValidaLote').prop('hidden', false);
        $("#selectLotes").css('borderColor', '#FA8072');
        bool = false;

    } else {
        $('#ValidaLote').prop('hidden', true);
        $("#selectLotes").css('borderColor', '#ced4da');
    }
    return bool;
}




function InactivarRegistro() {
    if ($("#txtIdControl").val() < 1) {
        return;
    }

    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableControl').html('');
    
    $.ajax({
        url: "../ControlEnfundado/InactivarControlEnfundado",
        type: "GET",
        data: {
            IdControl: $("#txtIdControl").val()
        },

        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            if (resultado.Codigo == 0) {
                MensajeAdvertencia(resultado.Mensaje);
            } else {

                MensajeCorrecto(resultado.Mensaje);           
            }
            $('#btnNuevo').prop("disabled", false);
            $('#btnInactivar').prop("disabled", false);
            Atras();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);
            $('#btnNuevo').prop("disabled", false);
            $('#btnInactivar').prop("disabled", false);



        }
    });

}

var modalConfirm = function (callback) {

    $("#btnInactivar").on("click", function () {
        $("#mi-modal").modal('show');
    });

    $("#modal-btn-si").on("click", function () {
        callback(true);
        $("#mi-modal").modal('hide');
    });

    $("#modal-btn-no").on("click", function () {
        callback(false);
        $("#mi-modal").modal('hide');
    });
};

modalConfirm(function (confirm) {
    if (confirm) {
        //Acciones si el usuario confirma
        InactivarRegistro();

    }
});
