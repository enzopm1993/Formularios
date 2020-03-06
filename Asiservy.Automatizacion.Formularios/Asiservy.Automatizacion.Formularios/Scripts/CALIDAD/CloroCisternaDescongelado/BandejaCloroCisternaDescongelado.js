$(document).ready(function () {
    CargarBandeja();
});
//CARGAR BANDEJA
function CargarBandeja() {
    $("#spinnerCargando").prop("hidden", false);

    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../CloroCisternaDescongelado/BandejaCloroCisternaDescongeladoPartial",
        type: "GET",
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            //CerrarModalCargando();
            //console.log(resultado);
            $('#divPartialBandeja').empty();
            $('#divPartialBandeja').html(resultado);
            //console.log($('#Nregistros').val())
            if ($('#Nregistros').val() == '0') {
                $('#MensajeRegistros').show();
                $('#btnAprobar').hide();
            } else {
                $('#btnAprobar').show();
                $('#MensajeRegistros').hide();
            }
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    var table = $("#tblDataTableAprobar");  
        table.DataTable().destroy();
        table.DataTable().clear();
        table.DataTable().draw();

    $.ajax({
        url: "../CloroCisternaDescongelado/BandejaAprobarCloroCisternaDescongelado",
        type: "GET",
        data: {
            Fecha:model.Fecha,
            IdCloroCisterna: model.IdCloroCisterna
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para esta OF.");
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros.");
            } else {
                $("#tblDataTableAprobar tbody").empty();
                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'Hora' },
                    { data: 'Ppm_Cloro' },
                    { data: 'Cisterna' },
                    { data: 'UsuarioIngresoLog'},
                    { data: 'Observaciones' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                table.DataTable().clear();
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();

                $("#ModalApruebaProductoTerminado").modal("show");
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoMaterial").prop("hidden", true);
        }
    });


}