
function ConsultarEmpleados() {
    ConsultarEmpleado = "ConsultarEmpleado";
    $.ajax({
        type: "GET",
        url: '../Asistencia/Empleados',
        success: function (data) {
            $('#DivEmpleados').html(data);
        }
    });
    $('#contempleados').show();
}
//$('#DivEmpleados').hide();
$('#EmpleadosRegresar').hide();
function RegresarEmpleados() {
    $('#EmpleadosRegresar').show();
}
$('#btnGuardarCambioEmp').hide();
$('#ConsultarEmpleados').on("click", function () {
    $('#btnGuardarCambioEmp').show();
});
$("#checkall").on("click", function () {
    $(".checks").prop("checked", this.checked);
});
$('#EmpleadosRegresar').hide();
$('#TablaEmpleadosRegresar').DataTable({
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
    pageLength: 5,
    lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']]
})
$('#trlinea').hide();
$('#trlineao').hide();
$('#trlineaop').hide();
$('#divprestar').hide();
$('#divregresar').hide();
$('#contempleados').hide();
$('#comboareaop').change(function () {
    if ($(this).val() == 'procesos') {
        $('#trlineaop').show();
    } else {
        $('#trlineaop').hide();
    }
});
$('#comboareao').change(function () {
    if ($(this).val() == 'procesos') {
        $('#trlineao').show();
    } else {
        $('#trlineao').hide();
    }
});
$('#comboarea').change(function () {
    if ($(this).val() == 'procesos') {
        $('#trlinea').show();
    } else {
        $('#trlinea').hide();
    }
}); 
$('#optcambiaremp').change(function () {
    if ($(this).val() == 'prestar') {
        
        $('#divprestar').show();
        $('#divregresar').hide();
        $('#EmpleadosRegresar').hide();
    }else
        if ($(this).val() == 'regresar') {
            $('#contempleados').hide();
        $('#divprestar').hide();
        $('#divregresar').show();
    }else
        {
            $('#contempleados').hide();
        $('#EmpleadosRegresar').hide();
        $('#divprestar').hide();
        $('#divregresar').hide();
    }
});

