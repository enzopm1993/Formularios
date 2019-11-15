function ChangeCentroCosto(valor) {
    $("#SelectRecurso").empty();
    $("#SelectRecurso").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectLinea").empty();
    $("#SelectLinea").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectCargo").empty();
    $("#SelectCargo").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoRecursos",
        type: "Get",
        data:
        {
            CodCentroCostos: valor
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectRecurso").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("El centro de costos no tiene recurso", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function ChangeRecurso(valor) {

    $("#SelectLinea").empty();
    $("#SelectLinea").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectCargo").empty();
    $("#SelectCargo").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoLineas",
        type: "Get",
        data:
        {
            CodRecurso: valor,
            CodCentroCostos: $('#SelectArea').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectLinea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("no se encontro una línea", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function ChangeLinea(valor) {
    $("#SelectCargo").empty();
    $("#SelectCargo").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoCargosxRecursoyLinea",
        type: "Get",
        data:
        {
            CodLinea: valor,
            CodRecurso: $('#SelectRecurso').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectCargo").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("no se encontro un cargo", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
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
//----
function ChangeCentroCostoRegresar(valor) {
    $("#SelectRecursoRegresar").empty();
    $("#SelectRecursoRegresar").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectLineaRegresar").empty();
    $("#SelectLineaRegresar").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectCargoRegresar").empty();
    $("#SelectCargoRegresar").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoRecursos",
        type: "Get",
        data:
        {
            CodCentroCostos: valor
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectRecursoRegresar").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("El centro de costos no tiene recurso", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function ChangeRecursoRegresar(valor) {

    $("#SelectLineaRegresar").empty();
    $("#SelectLineaRegresar").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectCargoRegresar").empty();
    $("#SelectCargoRegresar").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoLineas",
        type: "Get",
        data:
        {
            CodRecurso: valor,
            CodCentroCostos: $('#SelectAreaRegresar').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectLineaRegresar").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("no se encontro una línea", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function ChangeLineaRegresar(valor) {
    $("#SelectCargoRegresar").empty();
    $("#SelectCargoRegresar").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoCargosxRecursoyLinea",
        type: "Get",
        data:
        {
            CodLinea: valor,
            CodRecurso: $('#SelectRecursoRegresar').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectCargoRegresar").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("no se encontro un cargo", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
//--
function CambioLineaRegresar(valor) {
    $("#SelectAreaRegresar").empty();
    $("#SelectAreaRegresar").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectAreaRegresar").empty();
    $("#SelectAreaRegresar").append("<option value='' >-- Seleccionar Opción--</option>");

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
                    $("#SelectAreaRegresar").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
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

//origen
function ChangeLineaOrigen(valor) {
    $("#SelectCargoOrigen").empty();
    $("#SelectCargoOrigen").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoCargosxRecursoyLinea",
        type: "Get",
        data:
        {
            CodLinea: valor,
            CodRecurso: $('#SelectRecursoOrigen').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectCargoOrigen").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("no se encontro un cargo", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function ChangeRecursoOrigen(valor) {
    
    $("#SelectLineaOrigen").empty();
    $("#SelectLineaOrigen").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectCargoOrigen").empty();
    $("#SelectCargoOrigen").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoLineas",
        type: "Get",
        data:
        {
            CodRecurso: valor,
            CodCentroCostos: $('#SelectAreaOrigen').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectLineaOrigen").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("no se encontro una línea", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function ChangeCentroCostoOrigen(valor) {
    $("#SelectRecursoOrigen").empty();
    $("#SelectRecursoOrigen").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectLineaOrigen").empty();
    $("#SelectLineaOrigen").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectCargoOrigen").empty();
    $("#SelectCargoOrigen").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../Asistencia/ConsultaListadoRecursos",
        type: "Get",
        data:
        {
            CodCentroCostos: valor
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectRecursoOrigen").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("El centro de costos no tiene recurso", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function CambioLineaOrigen(valor) {
    $("#SelectAreaOrigen").empty();
    $("#SelectAreaOrigen").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#SelectCargoOrigen").empty();
    $("#SelectCargoOrigen").append("<option value='' >-- Seleccionar Opción--</option>");

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
                    $("#SelectAreaOrigen").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
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


function CambioAreaOrigen(valor) {

    $("#SelectCargoOrigen").empty();
    $("#SelectCargoOrigen").append("<option value='' >-- Seleccionar Opción--</option>");
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
                    $("#SelectCargoOrigen").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
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