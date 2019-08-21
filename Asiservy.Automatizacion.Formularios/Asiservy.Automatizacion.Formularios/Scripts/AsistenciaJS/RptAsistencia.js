﻿$('#tbldia').hide();
$('#tblmes').hide();
$('#tblsemana').hide();
$('#tblsemanarpt').DataTable({
    language: {
        "decimal": "",
        "emptyTable": "No hay información",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
        "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
        "infoFiltered": "(Filtrado de _MAX_ total entradas)",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": "Mostrar _MENU_ Entradas",
        "loadingRecords": "Cargando...",
        "processing": "Procesando...",
        "search": "Buscar:",
        "zeroRecords": "Sin resultados encontrados",
        "paginate": {
            "first": "Primero",
            "last": "Ultimo",
            "next": "Siguiente",
            "previous": "Anterior"
        }
    },
    ordering: false,
    pageLength: 5,
    lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']]

});
$('#tblrptmes').DataTable({
    language: {
        "decimal": "",
        "emptyTable": "No hay información",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
        "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
        "infoFiltered": "(Filtrado de _MAX_ total entradas)",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": "Mostrar _MENU_ Entradas",
        "loadingRecords": "Cargando...",
        "processing": "Procesando...",
        "search": "Buscar:",
        "zeroRecords": "Sin resultados encontrados",
        "paginate": {
            "first": "Primero",
            "last": "Ultimo",
            "next": "Siguiente",
            "previous": "Anterior"
        }
    },
    ordering: false,
    pageLength: 5,
    lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']]

});
$('#tblrptdia').DataTable({
    language: {
        "decimal": "",
        "emptyTable": "No hay información",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
        "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
        "infoFiltered": "(Filtrado de _MAX_ total entradas)",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": "Mostrar _MENU_ Entradas",
        "loadingRecords": "Cargando...",
        "processing": "Procesando...",
        "search": "Buscar:",
        "zeroRecords": "Sin resultados encontrados",
        "paginate": {
            "first": "Primero",
            "last": "Ultimo",
            "next": "Siguiente",
            "previous": "Anterior"
        }
    },
    ordering: false,
    pageLength: 5,
    lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']]

});
$('#trlineas').hide();
$('#comboarea').change(function () {
    if ($(this).val() == 'Procesos') {
        $('#trlineas').show();
    } else {
        $('#trlineas').hide();
    }
});
function ConsultarEmpleados() {
    if (($('#combodia').val() != "0") && ($('#combomes').val() != "0") && ($('#combosemana').val() != "0")) {
        $('#tbldia').show();
        $('#tblmes').hide();
        $('#tblsemana').hide();
        $('#showorhide').hide(1000);
        $("#img-arrow").attr("src", "../Content/images/arrow-hidden.jpg");
    }
    if (($('#combodia').val() == "0") && ($('#combomes').val() != "0") && ($('#combosemana').val() != "0")) {
        $('#tblsemana').show();
        $('#tbldia').hide();
        $('#tblmes').hide();
        $('#showorhide').hide(1000);
        $("#img-arrow").attr("src", "../Content/images/arrow-hidden.jpg");
    }
    if (($('#combodia').val() == "0") && ($('#combomes').val() != "0") && ($('#combosemana').val() == "0")) {
        $('#tblmes').show();
        $('#tblsemana').hide();
        $('#tbldia').hide();
        $('#showorhide').hide(1000);
        $("#img-arrow").attr("src", "../Content/images/arrow-hidden.jpg");
    }
    if (($('#combodia').val() == "0") && ($('#combomes').val() == "0") && ($('#combosemana').val() == "0")) {
        $('#tbldia').hide();
        $('#tblmes').hide();
        $('#tblsemana').hide();
    }

}

$('.botonconsultaemp').click(function () {
    
    ConsultarEmpleados();
    
})

function showOrHide() {
    var div = document.getElementById("showorhide");

    if ($('#showorhide').is(':visible')) {
        document.getElementById("img-arrow").src = "../Content/images/arrow-hidden.jpg";
        $('#showorhide').hide(1000);

    }
    else {
        $('#showorhide').show(1000);

        $("#img-arrow").attr("src", "../Content/images/arrow-hidden1.jpg");

    }
}