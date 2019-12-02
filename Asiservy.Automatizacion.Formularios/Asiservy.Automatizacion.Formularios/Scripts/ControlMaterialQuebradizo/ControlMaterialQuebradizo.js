

$(document).ready(function () {
    ValidaMaterialQuebradizo();   
});

function ValidaMaterialQuebradizo() {
    if ($('#txtFechaControl').val() == "" || $('#selectLinea').val()=="") {
        return;
    }
    $("#DivMensaje").html("");
    $("#btnEliminar").prop("hidden", true);
    $("#btnObservacion").prop("hidden", true);  
    $("#divTablaControlDetalle").html('');
    $.ajax({
        url: "../ControlMaterialQuebradizo/ValidarControl",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaControl').val(),
            Linea: $('#selectLinea').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado.Codigo == 0) {
                $('#btnGenerar').prop('hidden', false);
                $("#DivMensaje").html("<h3 class'text-center'> No se ha generado el control </h3>");
                return;
            }
            $('#txtIdControl').val(resultado.Codigo);
            $("#txtObservacionMaterial").val(resultado.Mensaje);
            CargarMaterialQuebradizo();
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}



function CargarMaterialQuebradizo() {

    $("#spinnerCargando").prop("hidden", false);
    $("#divTablaControlDetalle").html('');
    $.ajax({
        url: "../ControlMaterialQuebradizo/ControlMaterialQuebradizoPartial",
        type: "GET",
        data:
        {
            idControl: $('#txtIdControl').val()
        },
        success: function (resultado) {
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
                $("#btnEliminar").prop("hidden", false);
                $("#btnObservacion").prop("hidden", false);  

            } else {
                $("#divTablaControlDetalle").html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
                $("#btnEliminar").prop("hidden", false);
                $("#btnObservacion").prop("hidden", false);  
            }
            $("#spinnerCargando").prop("hidden", true);
            $('#btnGenerar').prop('hidden', true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function Validar() {
    var valida = true;
    if ($("#txtFechaControl").val() == "") {
        $("#txtFechaControl").css("border-color", "#DC143C");//#d1d3e2
        valida = false;
    } else {
        $("#txtFechaControl").css("border-color", "#d1d3e2");
    }
    if ($("#selectLinea").val() == "") {
        $("#selectLinea").css("border-color", "#DC143C");//#d1d3e2
        valida = false;
    } else {
        $("#selectLinea").css("border-color", "#d1d3e2");
    }

    return valida;
}

function GenerarControlMaterialQuebradizo() {
    if (!Validar()) {
        return;
    }
    $.ajax({
        url: "../ControlMaterialQuebradizo/GeneraControl",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaControl').val(),
            Linea: $('#selectLinea').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado.Codigo > 0) {
                $("#txtIdControl").val(resultado.Codigo);
                CargarMaterialQuebradizo();
                $("#DivMensaje").html("");
                $("#btnGenerar").prop("hidden", true);
                $("#btnEliminar").prop("hidden", false);
                $("#btnObservacion").prop("hidden", false);  
                $("#txtObservacionMaterial").val(resultado.Mensaje);

            }

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}




function EliminarControlMaterialQuebradizo() {   
    $.ajax({
        url: "../ControlMaterialQuebradizo/EliminarControl",
        type: "GET",
        data:
        {
            idControl: $('#txtIdControl').val()
            
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#txtIdControl").val('');
            ValidaMaterialQuebradizo();
            $("#DivMensaje").html("");
            $("#btnGenerar").prop("hidden", false);
            $("#btnEliminar").prop("hidden", true);  
            $("#btnObservacion").prop("hidden", true);  
            
            $("#divTablaControlDetalle").html('');

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}

function ValidarDetalle(id) {
    var total = "#txtTotal-" + id;
    var buenEstado = "#txtBuenEstado-" + id;   
    var dadoBaja = "#txtDadoBaja-" + id;

    var valida = true;

    if ($(total).val() > 0) {
        $(total).css("border-color", "#d1d3e2");
    } else {
        $(total).css("border-color", "#DC143C");//#d1d3e2
        valida = false;
    }

    //if ($(buenEstado).val() > 0) {
    //    $(buenEstado).css("border-color", "#d1d3e2");a
    //} else {
    //    $(buenEstado).css("border-color", "#DC143C");//#d1d3e2
    //    valida = false;
    //}

    var valortotal =  $(total).val();
    var valorestado = $(buenEstado).val();
    var valorDadoBaja = $(dadoBaja).val();
    if (parseInt(valorestado) > parseInt(valortotal)) {
        MensajeAdvertencia("Material en buen estado no puede ser mayor que el total");
        $(buenEstado).css("border-color", "#DC143C");
        valida = false;
    } else {
        $(buenEstado).css("border-color", "#d1d3e2");
    }


    if (parseInt(valorDadoBaja) > parseInt(valortotal)) {
        MensajeAdvertencia("Material en dado de baja no puede ser mayor que el total");

        $(valorDadoBaja).css("border-color", "#DC143C");
        valida = false;
    } else {
        $(valorDadoBaja).css("border-color", "#d1d3e2");
    }


    return valida;
}

function check(id) {
    if (!ValidarDetalle(id)) {
        $("#material" + id).prop("checked", false);
        return;
    }

    var total = "#txtTotal-" + id;
    var buenEstado = "#txtBuenEstado-" + id;
    var observacion = "#txtObservacion-" + id;
    var dadoBaja = "#txtDadoBaja-" + id;

    if ($("#material" + id).prop("checked")) {
        var estado = "A";
    } else {
        var estado = "I";
    }

    $.ajax({
        url: "../ControlMaterialQuebradizo/GuardarControlDetalle",
        type: "GET",
        data:
        {
            IdControlMaterialDetalle: id,
            TotalMaterial: $(total).val(),
            BuenEstado: $(buenEstado).val(),
            Observacion: $(observacion).val(),
            DadoBaja: $(dadoBaja).val(),
            EstadoRegistro: estado

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (estado == "A") {
                $(total).prop("disabled", true);
                $(buenEstado).prop("disabled", true);
                $(observacion).prop("disabled", true);
                $(dadoBaja).prop("disabled", true);
            } else {
                 $(total).prop("disabled", false);
                $(buenEstado).prop("disabled", false);
                $(observacion).prop("disabled", false);
                $(dadoBaja).prop("disabled", false);
            }
            
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });

}

function clearMaterial(id) {
  
    if ($('#' + id).val()==0)
        $('#' + id).val('');
}

function ModalObservacion() {
    $("#ModalObservacion").modal("show");
}


$("#modal-obs-si").on("click", function () {

    if ($("#txtObservacionMaterial").val() == '') {
        $("#txtObservacionMaterial").css("border-color", "#DC143C");
        return;
    } 
    $("#txtObservacionMaterial").css("border-color", "#d1d3e2");
    GuardarObservacionControl();
    $("#ModalObservacion").modal('hide');
});

$("#modal-obs-no").on("click", function () {   
    $("#ModalObservacion").modal('hide');
});


function GuardarObservacionControl() {
    $.ajax({
        url: "../ControlMaterialQuebradizo/GuardarObservacionControl",
        type: "GET",
        data:
        {
            idControl: $('#txtIdControl').val(),
            Observacion: $("#txtObservacionMaterial").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}




$("#btnEliminar").on("click", function () {  
    $("#mi-modal").modal('show');
});

$("#modal-btn-si").on("click", function () {
    EliminarControlMaterialQuebradizo();
    $("#mi-modal").modal('hide');
});

$("#modal-btn-no").on("click", function () {   
    $("#mi-modal").modal('hide');
});
