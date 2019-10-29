
$(document).ready(function () {
    //var fecha = new Date(); //Fecha actual
    //var mes = fecha.getMonth() + 1; //obteniendo mes
    //var dia = fecha.getDate()+1; //obteniendo dia
    //var ano = fecha.getFullYear(); //obteniendo año
    //if (dia < 10)
    //    dia = '0' + dia; //agrega cero si el menor de 10
    //if (mes < 10)
    //    mes = '0' + mes //agrega cero si el menor de 10
    //document.getElementById('FechaProduccion').value = ano + "-" + mes + "-" + dia;
});
function ConsultaProyProgramacion(){
    $.ajax({
        url: "../ProyeccionProgramacion/ProyeccionProgramacionPartial",
        type: "POST",
        data:
        {
            FechaProduccion: $('#FechaProduccion').val()
        },
        success: function (resultado) {
          
            $('#DivProyeccion').empty();
            $('#DivProyeccion').html(resultado);
           
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}
function Limpiar() {
    $('#Lote').val("");
    //$('#FechaProduccion').val("");
    $('#Toneladas').val("");
    $('#Destino').prop('selectedIndex', 0);
    $('#TipoLimpieza').prop('selectedIndex', 0);
    $('#Especie').prop('selectedIndex', 0);
    $('#Talla').prop('selectedIndex', 0);
    $('#Observacion').val("");
    $('#IdProyeccion').val("");
    //var d = new Date();

    //var dia = d.getDate()+1;

    //var mes = (d.getMonth() + 1) < 10 ? ("0" + (d.getMonth() + 1)) : d.getMonth() + 1;
    //var anio = d.getFullYear();

    //var fechatotal = anio + "-" + mes + "-" + dia
    //$('#FechaProduccion').val(fechatotal);

    //ConsultaProyProgramacion();
}
function LimpiarBoton() {
    Limpiar();
    var d = new Date();

    var dia = d.getDate()+1;

    var mes = (d.getMonth() + 1) < 10 ? ("0" + (d.getMonth() + 1)) : d.getMonth() + 1;
    var anio = d.getFullYear();

    var fechatotal = anio + "-" + mes + "-" + dia
    $('#FechaProduccion').val(fechatotal);

    ConsultaProyProgramacion();

}
function EditarProyeccion(IdProyeccion, Lote, Fecha, Toneladas, Destino, TipoLimpieza, Observacion, Especie, Talla) {
   // alert(Fecha);
    var FechaD = new Date(Fecha);
    var mes = FechaD.getMonth()+1; //obteniendo mes
    var dia = FechaD.getDate(); //obteniendo dia
    var ano = FechaD.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes; //agrega cero si el menor de 10
    $('#IdProyeccion').val(IdProyeccion);
    $('#Lote').val(Lote);
    $('#FechaProduccion').val(ano + "-" + mes + "-" + dia);
    $('#Toneladas').val(Toneladas);
    $('#Destino').val(Destino);
    $('#TipoLimpieza').val(TipoLimpieza);
    $('#Observacion').val(Observacion);
    $('#Talla').val(Talla);
    $('#Especie').val(Especie);
    console.log(Especie);
    console.log(Talla);

}
function IngresarProyeccionProgramacion() {
    var idPro;
    if ($('#IdProyeccion').val() == "") {
        idPro = 0;
    } else {
        idPro = $('#IdProyeccion').val();
    }
    if ($('#Lote').val() == "" || $('#FechaProduccion').val() == "" || $('#Toneladas').val() == "" || $('#Destino').prop('selectedIndex') == 0 || $('#TipoLimpieza').prop('selectedIndex') == 0
        || $('#Especie').prop('selectedIndex') == 0 || $('#Talla').prop('selectedIndex') == 0) {
        MensajeError("Debe ingresar los campos requeridos", false);
    } else {
        $.ajax({
            url: "../ProyeccionProgramacion/ProyeccionProgramacionPartial",
            type: "POST",
            data:
            {
                IdProyeccionProgramacion: idPro,
                Lote: $('#Lote').val(),
                FechaProduccion: $('#FechaProduccion').val(),
                Toneladas: $('#Toneladas').val(),
                Destino: $('#Destino').val(),
                TipoLimpieza: $('#TipoLimpieza').val(),
                Observacion: $('#Observacion').val(),
                Especie: $('#Especie').val(),
                Talla: $('#Talla').val()
            },
            success: function (resultado) {
                Limpiar();
                $('#DivProyeccion').empty();
                $('#DivProyeccion').html(resultado);
                MensajeCorrecto("Registro ingresado con éxito", false);
            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);

            }
        });
    }
    
}

$('#tablaProyeccion').DataTable({
    "language": {
        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    },
    "pageLength": 5,
    "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
    "pagingType": "full_numbers"

});