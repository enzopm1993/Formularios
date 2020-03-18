$(document).ready(function () {
    ListarCuchillocPreparacion(0);
    $('#tblDataTableCargar tbody').on('click', 'tr', function () {
        var table = $('#tblDataTableCargar').DataTable();
        var data = table.row(this).data();
        SelecionarFila(data);
    });
});


function ListarCuchillocPreparacion(opcion) {
    $('#CheckEstadoRegistroOp').prop('checked', false);
    $('#LabelEstado').text('Estado Registro');
    $("#txtDescripcion").val('');
    $("#ModalCheckBox").hide();
    var op = opcion;
    if (op!=1) {
        var table = $("#tblDataTableCargar");
        table.DataTable().destroy();
        table.DataTable().clear();
        table.DataTable().draw();        
    }
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarCuchilloPreparacion",
        type: "GET",        
        data: {
            CodigoCuchillo: $("#txtCodigoCuchillo").val().toUpperCase(),
            opcion: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("¡No hay datos en la consulta!", false);
            } else {
                if (op == 1) {                   
                        for (var item in resultado) {
                            $("#txtDescripcion").val(resultado[item].DescripcionCuchillo);
                            $("#ModalCheckBox").show();                            
                            if (resultado[item].EstadoRegistro == 'A') {
                                $('#CheckEstadoRegistroOp').prop('checked', true);
                                $('#LabelEstado').text('Activo');//.addClass('badge badge-success');
                            } else {
                                $('#CheckEstadoRegistroOp').prop('checked', false);
                                $('#LabelEstado').text('Inactivo');//.addClass('badge badge-danger');
                            } 
                        }                    
                    return;
                } else {
                    $("#tblDataTableCargar tbody").empty();
                    $('#MensajeRegistros').prop("hidden", true);
                    config.opcionesDT.order = [];
                    config.opcionesDT.columns = [
                        { data: 'IdCuchilloPreparacion' },
                        { data: 'CodigoCuchillo' },
                        { data: 'DescripcionCuchillo' },
                        { data: 'EstadoRegistro' }
                    ];
                    config.opcionesDT.aoColumnDefs = [{
                        "aTargets": [3], // Columna a la que se quiere aplicar el css
                        "mRender": function (data, type, full) {
                            var inactivo = 'Inactivo';
                            var clscolor = "badge-danger";
                            if (data =='A') {
                                clscolor = "badge-success";
                                inactivo = 'Activo';
                            }
                            return '<span class="badge ' + clscolor + '">' + inactivo + '</span>';
                        }
                    }];
                    table.DataTable().destroy();
                    table.DataTable(config.opcionesDT);
                    table.DataTable().clear();
                    table.DataTable().rows.add(resultado);
                    table.DataTable().draw();
                }
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarModificarCuchilloPreparacion() {
    var estadoRegistro = 'I';
    if ($("#txtCodigoCuchillo").val().toUpperCase()=='') {
        MensajeError("¡El código del cuchillo no puede estar vacío!", false);
        return;
    }
    if ($("#CheckEstadoRegistroOp").prop('checked')) {
        estadoRegistro = 'A';
    }
    $.ajax({
        url: "../ControlCuchillosPreparacion/GuardarModificarCuchilloPreparacion",
        type: "POST",
        data: {
            CodigoCuchillo: $("#txtCodigoCuchillo").val().toUpperCase(),
            DescripcionCuchillo: $("#txtDescripcion").val(),
            EstadoRegistro: estadoRegistro
        },
        success: function (resultado) {
            listaDatos = resultado;
            limpiar();
            ListarCuchillocPreparacion(0);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function limpiar() {
    $("#txtCodigoCuchillo").val('');
    $("#txtDescripcion").val('');
    $("#txtCodigoCuchillo").val('');
}

function CambioEstado(valor) {
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}

function SelecionarFila(data) {
    $("#txtDescripcion").val(data.DescripcionCuchillo);
    $("#txtCodigoCuchillo").val(data.CodigoCuchillo);
    $("#ModalCheckBox").show();
    if (data.EstadoRegistro == 'A') {
        $('#CheckEstadoRegistroOp').prop('checked', true);
        $('#LabelEstado').text('Activo');//.addClass('badge badge-success');
    } else {
        $('#CheckEstadoRegistroOp').prop('checked', false);
        $('#LabelEstado').text('Inactivo');//.addClass('badge badge-danger');
    } 
}