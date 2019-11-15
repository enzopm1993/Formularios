//mover empleados
function MoverEmpleados() {
    var result = new Array();
    i = 0; 
    //**
    if ($('#optcambiaremp').val() == 'prestar') {
        if ($('#selectLinea').val() != "" && $('#SelectArea').val() != "" && $('#SelectRecurso').val() != "" && $('#SelectCargo').val() != ""
            && $('#txtFechaInicio').val() != "" && $('#txtHoraInicio').val() != "") {
            $("input[type=checkbox]:checked").each(function (resultado) {
                id = $(this).attr("id");
                this.id = id.replace('Empleado-', '');
                result.push(this.id);
                i++;
            });
            //console.log(result);
            Mover(result);
        } else {
            MensajeAdvertencia("Centro de Costos, Recurso, Línea,Cargo,fecha y hora son obligatorios", false);
        }
    } else {
        $("input[type=checkbox]:checked").each(function (resultado) {
            id = $(this).attr("id");
            this.id = id.replace('Empleado-', '');
            result.push(this.id);
            i++;
        });
        //console.log(result);
        Mover(result);
    }
    //**
    
}

function Mover(result) {
    //console.log(result);
    if (result.length  == 0) {
        MensajeAdvertencia("Error, no se ha seleccionado ningún empleado");
        return false;
    }
    var pslinea = "";
    var psarea = "";
    var psrecurso = "";
    var pscargo = "";
    var resultado = JSON.stringify(result)
    var resultado2 = JSON.parse(resultado)
    
    if ($('#optcambiaremp').val() == 'prestar') {
        pslinea = $('#SelectLinea').val();
        psarea = $('#SelectArea').val();
        psrecurso = $('#SelectRecurso').val();
        pscargo = $('#SelectCargo').val();
        psfecha = $('#txtFechaInicio').val();
        pshora = $('#txtHoraInicio').val();
    } else {
        if ($('#txtFechaFin').val() == "" && $('#txtHoraFin').val() == "") {
            MensajeAdvertencia("Error, Debe ingresar Fecha y hora");
            return false;
        }
            
        pslinea = $('#SelectLineaRegresar').val();
        psarea = $('#SelectAreaRegresar').val();
        psfecha = $('#txtFechaFin').val();
        pshora = $('#txtHoraFin').val();
    }
    console.log(resultado2);
    $.ajax({
        url: '../Asistencia/MoverEmpleados',
        type: 'POST',
        dataType: "json",
        data: {
            dCedulas: resultado2,
            dlinea: pslinea,
            darea: psarea,
            drecurso:psrecurso,
            dcargo: pscargo,
            dfecha: psfecha,
            dhora: pshora,
            tipo: $('#optcambiaremp').val()
        },
        success: function (resultado) {
            MensajeCorrectoTiempo(resultado, true,10000);

        }
        ,
        error: function (resultado) {
            //MensajeError("No se pudieron mover", false);
            MensajeError(resultado, false);
        }
    });
}
// fin mover empleados
//consultar_empleados
function ConsultarEmpleados() {
    //ConsultarEmpleado = "ConsultarEmpleado";
  
    if ($('#SelectAreaOrigen').val() != "") {
        MostrarModalCargando();
        $.ajax({
            url: "../Asistencia/EmpleadosCambioPersonalPartial",
            type: "GET",
            data:
            {
                //pslinea: $('#SelectLineaOrigen').val(),
                //psarea: $('#SelectAreaOrigen').val(),
                //pscargo: $('#SelectCargoOrigen').val(),
                //tipo: $('#optcambiaremp').val()
                psCentroCosto: $('#SelectAreaOrigen').val(),
                psRecurso: $('#SelectRecursoOrigen').val(),
                psLinea: $('#SelectLineaOrigen').val(),
                psCargo: $('#SelectCargoOrigen').val(),
                tipo: $('#optcambiaremp').val()
            },
            success: function (data) {
                $('#DivEmpleados').html(data);
                $('#btnGuardarCambioEmp').show();
                $('#Guardar').show();
                $('#Guardar').val('Mover Empleados');
                CerrarModalCargando();

            }
        });
        $('#contempleados').show();
    } else {
        MensajeAdvertencia("Debe seleccionar al menos el centro de costos a consultar", false);

    }
    

}
function ConsultarEmpleadosRegresar() {
    
    //ConsultarEmpleado = "ConsultarEmpleado";
    if ($('#SelectAreaRegresar').val() != "") {
        $.ajax({
            type: "GET",
            data:
            {//string psCentroCosto, string psRecurso, string psLinea,string psCargo, string tipo
                pslinea: $('#SelectLineaRegresar').val(),
                psCentroCosto: $('#SelectAreaRegresar').val(),
                psRecurso: $('#SelectRecursoRegresar').val(),
                psCargo: $('#SelectCargoRegresar').val(),
                tipo: $('#optcambiaremp').val()
            },
            url: '../Asistencia/EmpleadosCambioPersonalPartial',
            success: function (data) {
                $('#DivEmpleados').html(data);
                $('#btnGuardarCambioEmp').show();
                $('#Guardar').val('Regresar Empleados');
                $('#Guardar').show();
            }
        });
        $('#contempleados').show();
    } else {
        MensajeAdvertencia("Debe seleccionar al menos el centro de costos a consultar", false);
    }
   
}
//fin consultar empleados
//$('#DivEmpleados').hide();
$('#EmpleadosRegresar').hide();
function RegresarEmpleados() {
    $('#EmpleadosRegresar').show();
}
$('#btnGuardarCambioEmp').hide();
//$('#ConsultarEmpleados').on("click", function () {
//    $('#btnGuardarCambioEmp').show();
//    $('#Guardar').val('Mover Empleados');
//});
////$('#ConsultarEmpleadosRegresar').on("click", function () {
//    $('#btnGuardarCambioEmp').show();
//    $('#Guardar').val('Regresar Empleados');
//});

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
        $('#Guardar').hide();
        $('#DivEmpleados').empty();
        $('#divprestar').show();
        $('#divregresar').hide();
        $('#EmpleadosRegresar').hide();
    }else
        if ($(this).val() == 'regresar') {
            $('#contempleados').hide();
            $('#Guardar').hide();
            $('#DivEmpleados').empty();
        $('#divprestar').hide();
        $('#divregresar').show();
    }else
        {
            $('#DivEmpleados').empty();
            $('#contempleados').hide();
        $('#EmpleadosRegresar').hide();
        $('#divprestar').hide();
        $('#divregresar').hide();
    }
});
$('#ConsultarEmpleadosRegresar').click(function () {
    $('#contempleados').show();
});

