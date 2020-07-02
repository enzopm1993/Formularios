
$(document).ready(function () {
    CargarTablaClasificadores();
    $('#IdClasificador').val(0);
  //  console.log($('#CheckGrupo').val());
    if ($('#CheckGrupo').prop("checked")) {
        $('#Grupo').attr("disabled", true);
    }

    $('#Grupo').select2();

   
});


function SeleccionClasificador(id, grupo, codigo, descripcion, estado) {
    //console.log(id);
    //console.log(grupo);
  
    //console.log(codigo);
    //console.log(descripcion);
    //console.log(estado);
    $('#IdClasificador').val(id);
    $('#Grupo').val(grupo).change();
    $('#Descripcion').val(descripcion);
    $('#Codigo').val(codigo);
    if (estado == "A")
        $('#CheckEstadoRegistro').prop('checked', true);
    else
        $('#CheckEstadoRegistro').prop('checked', false);
}

function Nuevo() {
    $('#IdClasificador').val(0);
    $('#Grupo').attr("disabled", false);
    $('#Grupo').prop('selectedIndex', 0).change();
    $('#Descripcion').val("");
    $('#Codigo').val("");
    $('#LabelEstado').text('Activo');

    $('#CheckEstadoRegistro').prop('checked', true);
    $('#CheckGrupo').prop('checked', false);


}


function CambioEstado(valor) {
    if (valor) {
       // Nuevo();
        $('#Grupo').attr("disabled", true);
        $('#Grupo').prop('selectedIndex', 0).change();
    } else {
        $('#Grupo').attr("disabled", false);
    } 

}


function CambioEstadoRegistro(valor) {
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}



function CargarTablaClasificadores() {
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Seguridad/ClasificadorPartial",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#DivTableClasificador');
            bitacora.html(resultado);
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}