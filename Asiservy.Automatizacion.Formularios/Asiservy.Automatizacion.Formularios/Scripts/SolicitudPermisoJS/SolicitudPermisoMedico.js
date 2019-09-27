


$(document).ready(function () {
    $("#DivHora").hide();
  
});


function CambioLinea(valor) {
    $("#selectArea").empty();
    $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#selectCargo").empty();
    $("#selectCargo").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoAreas",
        type: "Get",
        data:
        {
            CodLinea: valor
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}


function CambioArea(valor) {

    $("#selectCargo").empty();
    $("#selectCargo").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoCargos",
        type: "Get",
        data:
        {
            CodArea: valor
        },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $.each(data, function (create, row) {
                    $("#selectCargo").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}




function CambioHoraFecha() {
    var HoraDesde = document.getElementById("timeHoraSalida");
    var HoraHasta = document.getElementById("timeHoraRegreso");
    var FechaSalidaRegreso = document.getElementById("dateSalidaRegreso");
    var FechaDesde = document.getElementById("dateSalida");
    var FechaHasta = document.getElementById("dateRegreso");
    var check = document.getElementById("switchHoraFecha").checked


    if (check) {
        $("#LabelFecha").text("Hora");
        $("#DivHora").show();
        $("#DivFecha").hide();
        FechaDesde.value = null;
        FechaHasta.value = null;
    } else {
        $('#LabelFecha').text("Fecha");
        $("#DivHora").hide();
        $("#DivFecha").show();
        FechaSalidaRegreso.value = null;
        HoraDesde.value = null;
        HoraHasta.value = null;
    }
}




$("#selectSubGrupoEnfermedad").change(function () {
    $.ajax({
        url: "../SolicitudPermiso/ObtenerEnfermedades",
        type: "Get",
        data:
        {
            SubGrupoEnfermedad: $("#selectSubGrupoEnfermedad").val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $("#selectDiagnostico").empty();
                $("#selectDiagnostico").append("<option value='' >Seleccione Diagnóstico</option>");
                $.each(resultado, function (create, row) {
                $("#selectDiagnostico").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
            });
        } else {
            $("#selectDiagnostico").empty();
            $("#selectDiagnostico").append("<option value='' >Seleccione Diagnóstico</option>");
            //alert("El vendedor seleccionado no tiene zonas asignadas");
        }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $('#' + formulario).remove("disabled");
        }
    });
   
});
//fin combo anidados enfermedades


//consultar grupo enfermedades modal
function ConsultarGrupoEnfermedad(Codigo, Descripcion) {

    $('#CodigoDiagnostico').val(Codigo);
    $('#DescripcionDiagnostico').val(Descripcion);
   

    //$.ajax({
    //    url: "../SolicitudPermiso/ObtenerSubGrupoEnfermedades",
    //    type: "Get",
    //    data:
    //    {
    //        GrupoEnfermedad: Codigo
    //    },
    //    success: function (resultado) {
    //        if (!$.isEmptyObject(resultado)) {
    //            $("#selectSubGrupoEnfermedad").empty();
    //            $("#selectSubGrupoEnfermedad").append("<option value='' >Seleccione Sub Grupo</option>");
    //            $.each(resultado, function (create, row) {
    //            $("#selectSubGrupoEnfermedad").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
    //        });
    //        } else {
    //        $("#selectSubGrupoEnfermedad").empty();
    //        $("#selectSubGrupoEnfermedad").append("<option value='' >Seleccione Sub Grupo</option>");
    //        //alert("El vendedor seleccionado no tiene zonas asignadas");
    //        }
    //    },
    //    error: function (resultado) {
    //        MensajeError(JSON.stringify(resultado), false);
    //        $('#' + formulario).remove("disabled");
    //    }
    //});

    //$("#selectDiagnostico").empty();
    //$("#selectDiagnostico").append("<option value='' >Seleccione Diagnóstico</option>");
    cerrarmodalgrupoenfermedad();
    
}
function cerrarmodalgrupoenfermedad() {
    $('#ModalGrupoEnfermedad').modal('hide');

    //if (sPage = "SolicitudPermiso") {
    //    $('#CargarEmpleadoPG').attr("disabled", false);
    //}

    if (sPage = "SolicitudPermisoDispensario") {
        $('#CargarGrupoEnfermedadesp').attr("disabled", false);
    }
}

// boton consulta enfermedades grupo
function CargarGrupoEnfermedades(formulario) {

    
        $('#' + formulario).attr("disabled", true);
        $.ajax({
            url: "../SolicitudPermiso/ConsultarGrupoEnfermedades",
            type: "Get",
           
            success: function (resultado) {
                $('#DivCargarGrupoEnfermedades').html(resultado);
                $("#ModalGrupoEnfermedad").modal("show");

            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);
                $('#' + formulario).remove("disabled");
            }
        });
   

}
//fin boton grupo enfermedades