$(document).ready(function () {

   
    CargarPallets();
});
function EditarPallet(IdPallet, IdProveedor, Proveedor, Numero_Pallet, Envase, Lamina, Unidades) {
    $('#IdPallet').val(IdPallet);
    $('#cmbProveedor').val(IdProveedor);
    $('#txtPallet').val(Numero_Pallet);
    $('#txtenvase').val(Envase);
    $('#txtlamina').val(Lamina);
    $('#txtunidades').val(Unidades);
    $('#txtenvase').prop('disabled', true);
    $('#cmbProveedor').prop('disabled', true);
}
function Nuevo() {
    $('#txtPallet').val('');
    $('#txtenvase').val('');
    $('#txtlamina').val('');
    $('#txtunidades').val('');
    $("#cmbProveedor").prop('selectedIndex', 0);
    $('#IdPallet').val(0);
    $('#txtenvase').prop('disabled', false);
    $('#cmbProveedor').prop('disabled', false);
    $('#msjProveedor').hide();
    $('#msjPallet').hide();
    $('#msjenvase').hide();
    $('#msjlamina').hide();
    $('#msjunidades').hide();
}

function GuardarPallet() {
    if ($('#cmbProveedor').prop('selectedIndex') == 0) {
        $('#msjProveedor').show();
        return false;
    } else {
        $('#msjProveedor').hide();
    }
    if ($('#txtPallet').val() == '') {
        $('#msjPallet').show();
        return false;
    } else {
        $('#msjPallet').hide();
    }
    if ($('#txtenvase').val() == '') {
        $('#msjenvase').show();
        return false;
    } else {
        $('#msjenvase').hide();
    }
    if ($('#txtlamina').val() == '') {
        $('#msjlamina').show();
        return false;
    } else {
        $('#msjlamina').hide();
    }
    if ($('#txtunidades').val() == '') {
        $('#msjunidades').show();
        return false;
    } else {
        $('#msjunidades').hide();
    }
    $.ajax({
        url: "../ControlConsumoInsumo/GuardarPallet",
        type: "POST",
        data: {
            IdPallet: $('#IdPallet').val(),
            Proveedor: $('#cmbProveedor').val(),
            Numero_Pallet: $('#txtPallet').val(),
            Envase: $('#txtenvase').val() ,
            Lamina: $('#txtlamina').val(),
            Unidades: $('#txtunidades').val(),
            EstadoRegistro: 'A'
            //  Linea: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado, false);
            CargarPallets();
            Nuevo();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
function CargarPallets() {
    $('#spinnerCargando').prop("hidden", false);
    $.ajax({
        url: "../ControlConsumoInsumo/PartialMantenimientoPallet",
        type: "POST",
        data: {
            
        },
        success: function (resultado) {
            $('#spinnerCargando').prop("hidden", true);
            $('#DivPartialPallet').empty();
            $('#DivPartialPallet').html(resultado);

        },
        error: function (resultado) {
            $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);
        }
    });
}