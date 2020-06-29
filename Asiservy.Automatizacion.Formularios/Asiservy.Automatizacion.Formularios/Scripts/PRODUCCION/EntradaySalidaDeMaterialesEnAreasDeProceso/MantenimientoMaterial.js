$(document).ready(function () {
    CargarMateriales();   
});

function Limpiar() {
    $("#txtIdMaterial").val('');
    //$("#SelectMaterial").prop("selectedIndex",0);
    $("#txtNombre").val('');
    $("#txtDescripcion").val('');
    $("#txtIdMaterial").val("0");

}

function CargarMaterial(model) {
   // console.log(model);
    $("#txtIdMaterial").val(model.IdMaterial);
    //$("#SelectMaterial").val(model.Codigo);
    $("#txtDescripcion").val(model.Descripcion);
    $("#txtNombre").val(model.Nombre);
    if (model.EstadoRegistro == "A") {
        CambioEstado(true);
        $("#CheckEstadoRegistro").prop('checked', true);
    } else {
        $("#CheckEstadoRegistro").prop('checked', false);
        CambioEstado(false);
    }
}

function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');
}

function CargarMateriales() {
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableMateriales').html('');
    $('#Mensaje').html('');

    $.ajax({
        url: "../EntradaySalidaDeMaterialesEnAreasDeProceso/MantenimientoMaterialPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                $('#Mensaje').html('<h3 class="text-warning">No existen registros.</h3>');
                $("#spinnerCargando").prop("hidden", true);
                return;
            }
            var div = $('#DivTableMateriales');
             $("#spinnerCargando").prop("hidden", true);
            div.html(resultado);           
            config.opcionesDT.pageLength = 10;
            config.opcionesDT.order = [[1, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}



function GuargarMaterial() {
    //if ($("SelectMaterial").val() == "") {
    //    $("SelectMaterial").css("border-color", "#DC143C");
    //    return;
    //}
    //$("SelectMaterial").css("border-color", "#d1d3e2");
    var Estado = $("#CheckEstadoRegistro").prop('checked');
    if (Estado == true)
        Estado = "A";
    else
        Estado = "I";

    $.ajax({
        url: "../EntradaySalidaDeMaterialesEnAreasDeProceso/MantenimientoMaterial",
        type: "POST",
        data: {
            IdMaterial: $("#txtIdMaterial").val(),
            Nombre: $("#txtNombre").val(),
            Descripcion: $("#txtDescripcion").val(),
            EstadoRegistro: Estado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            Limpiar();
            CargarMateriales();
        },
        error: function (resultado) {
            MensajeError(resultado, false);
           
        }
    });





}