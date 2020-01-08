function SetearHoraInicio() {
    if ($('#horaswitch').prop('checked')) {
        $('#txtHoraInicio').show();
        $('#labelhora').text("");
    } else {
        $('#txtHoraInicio').hide();
        $('#labelhora').text("Inicio Jornada");
        $('#txtHoraInicio').val("");
    }
    
}
//mover empleados
function MoverEmpleados() {
    var result = new Array();
    i = 0; 
    //**
    if ($('#optcambiaremp').val() == 'prestar') {
        if ($('#selectLinea').val() != "" && $('#SelectArea').val() != "" && $('#SelectRecurso').val() != "" && $('#SelectCargo').val() != ""
            && $('#txtFechaInicio').val() != "" && (($('#horaswitch').prop('checked')==false) || ($('#txtHoraInicio').val()!=""))) {
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
//**************funcion para inactivar un empleado movido
function InactivarCambioPersonal() {
    var result = new Array();
    i = 0; 
    $("input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        this.id = id.replace('Empleado-', '');
        result.push(this.id);
        i++;
    });
    InactivarRegistroMovidos(result);
}
function InactivarRegistroMovidos(result) {
    if (result.length == 0) {
        MensajeAdvertencia("Error, no se ha seleccionado ningún empleado");
        return false;
    }
    var resultado = JSON.stringify(result)
    var resultado2 = JSON.parse(resultado)
    //spinner
    $("#btnInactivar").prop("hidden", true);
    $("#btnInactivarEspera").prop("hidden", false);
    //**
    $.ajax({
        url: '../Asistencia/InactivarEmpleadoCambioPersonal',
        type: 'POST',
        dataType: "json",
        data: {
            dCedulas: resultado2
        },
        success: function (resultado) {
            //spinner
            $("#btnInactivar").prop("hidden", false);
            $("#btnInactivarEspera").prop("hidden", true);
            //**
            //MensajeCorrectoTiempo(resultado, true, 10000);
            $('#BodyMensajeCp').html(resultado);
            $('#ModalMensajeCP').modal('show');
        }
        ,
        error: function (resultado) {
            //spinner
            $("#btnInactivar").prop("hidden", false);
            $("#btnInactivarEspera").prop("hidden", true);
            //**
            //MensajeError("No se pudieron mover", false);
            MensajeError(resultado, false);
        }
    });
}
//********************
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
    var resultado = JSON.stringify(result);
    var resultado2 = JSON.parse(resultado);

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
    //spinner
        $("#Guardar").prop("hidden", true);
        $("#btnGuardarEspera").prop("hidden", false);
    //**
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
            //spinner
            $("#Guardar").prop("hidden", false);
            $("#btnGuardarEspera").prop("hidden", true);
            //**
            //MensajeCorrectoTiempo(resultado, true,10000);
            $('#BodyMensajeCp').html(resultado);
            $('#ModalMensajeCP').modal('show');
        }
        ,
        error: function (resultado) {
            //spinner
            $("#Guardar").prop("hidden", false);
            $("#btnGuardarEspera").prop("hidden", true);
            //**
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
                $('#Guardar').val('Prestar Empleados');
                CerrarModalCargando();

            }
        });
        $('#contempleados').show();
    } else {
        MensajeAdvertencia("Debe seleccionar al menos el centro de costos a consultar", false);

    }
    

}
function ConsultarEmpleadosRegresar() {
    $("#ConsultarEmpleadosRegresar").prop("hidden", true);
    $("#btnConsultarEspera").prop("hidden", false);
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
                $("#ConsultarEmpleadosRegresar").prop("hidden", false);
                $("#btnConsultarEspera").prop("hidden", true);
                $('#DivEmpleados').html(data);
               
                $('#btnGuardarCambioEmp').show();
                $('#Guardar').val('Regresar Empleados');
                //**modificacion cambio personal boton inactivar 
                $('#btnInactivar').show();
                //**
                $('#Guardar').show();
                $('#inputsregresar').show();
            },
            error: function (data) {
                $("#ConsultarEmpleadosRegresar").prop("hidden", false);
                $("#btnConsultarEspera").prop("hidden", true);
                MensajeError(data,true);
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
        $('#inputsregresar').hide();
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
            $('#inputsregresar').hide();
            $('#DivEmpleados').empty();
            $('#contempleados').hide();
        $('#EmpleadosRegresar').hide();
        $('#divprestar').hide();
        $('#divregresar').hide();
        }
    $('#btnInactivar').hide();
});
$('#ConsultarEmpleadosRegresar').click(function () {
    $('#contempleados').show();
});

