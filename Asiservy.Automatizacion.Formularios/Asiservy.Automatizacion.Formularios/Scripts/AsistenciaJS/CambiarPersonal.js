//mover empleados
function MoverEmpleados() {
    var result = new Array();
    i = 0;
    $("input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        this.id = id.replace('Empleado-', '');
        result.push(this.id);
        i++;
    });
    console.log(result);
    Mover(result);
}

function Mover(result) {
    console.log(result);

    var resultado = JSON.stringify(result)
    var resultado2 = JSON.parse(resultado)
    console.log(resultado2);
    $.ajax({
        url: '../Asistencia/MoverEmpleados',
        type: 'POST',
        dataType: "json",
        data: {
            dCedulas: resultado2,
            dlinea: $('#selectLinea').val(),
            darea: $('#selectArea').val(),
            tipo: $('#optcambiaremp').val()
        },
        success: function (resultado) {
            MensajeCorrecto(resultado, true);

        }
        ,
        error: function () {
            MensajeError("No se pudieron mover", false);
        }
    });
}
// fin mover empleados
//consultar_empleados
function ConsultarEmpleados() {
    //ConsultarEmpleado = "ConsultarEmpleado";
    $.ajax({
        type: "GET",
        data:
        {
            pslinea: $('#SelectLineaOrigen').val(),
            psarea: $('#SelectAreaOrigen').val(),
            pscargo: $('#SelectCargoOrigen').val(),
            tipo: $('#optcambiaremp').val()
        },
        url: '../Asistencia/EmpleadosCambioPersonalPartial',
        success: function (data) {
            $('#DivEmpleados').html(data);
        }
    });
    $('#contempleados').show();
}
function ConsultarEmpleadosRegresar() {
    //ConsultarEmpleado = "ConsultarEmpleado";
    $.ajax({
        type: "GET",
        data:
        {
            pslinea: $('#SelectLineaRegresar').val(),
            psarea: $('#SelectAreaRegresar').val(),
            tipo: $('#optcambiaremp').val()
        },
        url: '../Asistencia/EmpleadosCambioPersonalPartial',
        success: function (data) {
            $('#DivEmpleados').html(data);
        }
    });
    $('#contempleados').show();
}
//fin consultar empleados
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
            
            //$('#DivEmpleados').empty();
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
$('#ConsultarEmpleadosRegresar').click(function () {
    $('#contempleados').show();
});

