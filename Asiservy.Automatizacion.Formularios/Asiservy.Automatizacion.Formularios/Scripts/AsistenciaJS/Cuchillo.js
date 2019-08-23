//Tabla con paginacion, busqueda, ordenar
$(document).ready(function () {
    $('#TableReporteCuchillos').DataTable({
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
});


//Limpia el campo de busqueda creado sin DataTable.net
function LimpiarTexto() {
        $.each($("#TableCuchillos tbody tr"), function () {
                $(this).show();
        });
    document.getElementById("search").innerText = "";
    $("#search").val("");
 }

//Busqueda Manual para tablas
$(document).ready(function () {
    $("#search").keyup(function () {
        _this = this;
        // Show only matching TR, hide rest of them
        $.each($("#TableCuchillos tbody tr"), function () {
            if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1)
                $(this).hide();
            else
                $(this).show();
        });
    });
});


//Bloquear todos los cuchillos negros que no se usan por el momento
var i = 0;
$("tr").each(function () {
    var desCheck = "CheckCuchilloNegro";
    if(i>1)
        desCheck += i;
    var x = document.getElementById(desCheck);
    if (x != null)
        x.disabled = true;
    i++;
});


//pintan celda de check de cuchillo
function Cuchillo(color, fila) {

    var desLabel = "labelCuchillo";
    var desCheck = "CheckCuchillo";

    if (color == 1) { desLabel += "Rojo"; desCheck += "Rojo"; }
    if (color == 2) { desLabel += "Negro"; desCheck += "Negro"; }
    if (color == 3) { desLabel += "Blanco"; desCheck += "Blanco"; }

    if (fila != 1) {
        desLabel += fila;
        desCheck += fila;
    }
    //console.log(desCheck);
    var label = document.getElementById(desLabel);
    var check = document.getElementById(desCheck).checked;
    //console.log(check);

    if (check) {
        label.style.background = "#27D5C3";
        document.getElementById(desCheck).checked = true;
    } else {
        label.style.background = "#ccc";
        document.getElementById(desCheck).checked = false;
    }

}

function Guardar() {
    var Estado = document.getElementById("SelectEstado");
   // console.log(Estado);
    //console.log(Estado.selectedIndex);
    if (Estado.selectedIndex == 0) {
        Mensaje("Seleccione un estado..");
        //   alert("Seleccione un Estado..");
    } else {
        Mensaje("Registro Guardado Exitosamente");
    }

}

function prueba() {

    var checkRojo = document.getElementById("CheckCuchilloRojo").checked;
    var checkBlanco = document.getElementById("CheckCuchilloBlanco").checked;
    var checkNegro = document.getElementById("CheckCuchilloNegro").checked;

    console.log(checkRojo, checkNegro, checkBlanco);

}
