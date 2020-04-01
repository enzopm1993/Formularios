$(document).ready(function () {
    config.opcionesDT.pageLength = -1;
    config.opcionesDT.order = false;
    config.opcionesDT.ordering = false;
    $('#tblDataTable2').DataTable(config.opcionesDT);

});

function ConsultarReporte() {
    $("#txtMensaje").html('');
    var cantidad = $("#txtCantidadCondiciones").val();
    //alert(cantidad);
    while (cantidad > 0) {
        $("#txt_" + cantidad).html('0');
        cantidad--;
    }

    if ($("#txtFecha").val() == '' || $("#selectArea").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    var table = $("#tblDataTableCargar");
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();  
    $.ajax({
        url: "../CondicionPersonal/ReporteCondicionPersonalPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#txtMensaje").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                table.DataTable().clear();
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#tblDataTableCargar tbody").empty();
                //config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'Cedula' },
                    { data: 'Nombre' },
                    { data: 'Condicion' },
                    { data: 'Observacion' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                table.DataTable().clear();
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();

                var query = Enumerable.From(resultado)
                    // GroupBy (keySelector, elementSelector, resultSelector, compareSelector)
                    .GroupBy(
                        null, // (identity)
                        null, // (identity)
                        "{ CodCondicion: $.CodCondicion, Total: $$.Count() }",
                        "'' + $.CodCondicion"
                    )
                    .ToArray();

                $.each(query, function (index, value) {
                    //alert(index + ": " + value);
                    //console.log(value.CodCondicion);
                    //console.log(value.Total);
                    $("#txt_" + value.CodCondicion).html(value.Total);
                    //console.log($("#txt_" + value.CodCondicion).val());

                });

               // console.log(query);

                //var queryResult = Enumerable.From(resultado)
                //    .Select(function (x) { return x.CodCondicion })
                //    .GroupBy(function (x) { return x.CodCondicion })
                //    .ToArray();
                //console.log(queryResult);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

