

$(document).ready(function () {
    CargarControlEnvaseEnlatado();
});
function Atras() {
    $('DivDetalleReporte').empty();
    $('#DivFiltros').show();
    $('#btnAtras').prop('hidden', true);

    $("#DivControlEnvasesEnlatado").show();
    $('#Detpartial').empty();
}
function CargarControlEnvaseEnlatado() {
    var txtFecha = $('#txtFecha').val();
    var selectTurno = $('#selectTurno').val();
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#selectTurno").val() == "" || $("#selectTurno").val() == "0") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }

    $("#spinnerCargando").prop("hidden", false);

    // CargarOrdenFabricacion();
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumoPartial",
        type: "GET",
        data: {
            Fecha: txtFecha,
            LineaNegocio: 'ENLATADO',
            Turno: selectTurno
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#DivControlEnvasesEnlatado').empty();
                $("#mensajeReg").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#mensajeReg").html("");
                $("#spinnerCargando").prop("hidden", true);
                $("#DivControlEnvasesEnlatado").html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

            $("#spinnerCargando").prop("hidden", true);
        }
    });
}
function SeleccionarControlDetalleConsumo(CabControl) {
    //console.log(CabControl);
    $('#cargac').show();
    
    //$('#principal').addClass("carga");

    $.ajax({
        url: "../ControlConsumoInsumo/ReporteEnvaseEnlatadoPartial",
        type: "GET",
        data: {
            IdCabeceraControlEnvEnlatado:CabControl.IdControlConsumoInsumos
        },
        success: function (resultado) {
            $('#btnAtras').prop('hidden',false)
            $('#cargac').hide();
            $('#DivFiltros').hide();
            $("#DivControlEnvasesEnlatado").hide();
            $('#DivDetalleReporte').html(resultado);
            //if (resultado == "101") {
            //    window.location.reload();
            //}
            //if (resultado == "0") {
            //    $("#mensajeReg").html("No existen registros");
            //    $("#spinnerCargando").prop("hidden", true);
            //} else {
            //    $("#spinnerCargando").prop("hidden", true);
            //    $("#DivControlEnvasesEnlatado").html(resultado);
            //    config.opcionesDT.pageLength = 10;
            //    $('#tblDataTable').DataTable(config.opcionesDT);
            //}

        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);

            //$("#spinnerCargando").prop("hidden", true);
        }
    });
}