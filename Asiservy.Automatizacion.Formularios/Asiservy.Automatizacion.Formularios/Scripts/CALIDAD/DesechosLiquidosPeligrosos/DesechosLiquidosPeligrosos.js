var itemEditar = [];
var estadoReporte = [];
$(document).ready(function () {
    ComboAnio();     
    CargarCabecera(1);
    ConsultarEstadoReporte(2);
});

function ComboAnio() {
    var date = new Date();
    document.getElementById("selectMonth").selectedIndex = moment(date).format('MM');
    var n = date.getFullYear();
    var select = document.getElementById("selectAnio");
    for (var i = n; i >= 2015; i--) {
        select.options.add(new Option(i, i));
    }
}

function ConsultarEstadoReporte(op) {   
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/ConsultarEstadoReporte",
        data: {
            anioBusqueda: $('#selectAnio').val(),
            mesBusqueda: $('#selectMonth').val(),
            idDesechosLiquidos: 0,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            estadoReporte = [];
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado.length == 0) {
                $("#lblAprobadoPendiente").prop("hidden", true);
               
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#lblAprobadoPendiente").prop("hidden", false);
                if (resultado[0].EstadoReporte == true) {
                    $("#lblAprobadoPendiente").text("APROBADO");
                    $("#lblAprobadoPendiente").removeClass('badge-danger');
                    $("#lblAprobadoPendiente").addClass('badge badge-success');
                } else {
                    $("#lblAprobadoPendiente").text("PENDIENTE");
                    $("#lblAprobadoPendiente").removeClass('badge-success');
                    $("#lblAprobadoPendiente").addClass('badge badge-danger');
                }
                estadoReporte = resultado[0].EstadoReporte;
            }            
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarCabecera(op) {
    ConsultarEstadoReporte(2);
    $('#cargac').show();
    var date = new Date();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/DesechosLiquidosPeligrososPartial",
        data: {
            anioBusqueda: $('#selectAnio').val(),
            mesBusqueda: $('#selectMonth').val(),
            idDesechosLiquidos: 0,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            itemEditar = 0;
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/GuardarModificarDesechosLiquidos",
        type: "POST",
        data: {
            IdDesechosLiquidos: itemEditar.IdDesechosLiquidos,
            IdDesechosLiquidosDetalle: itemEditar.IdDesechosLiquidosDetalle,
            FechaMES: $("#txtFechaCabecera").val(),
            FechaDIA: $("#txtFechaCabecera").val(),
            Laboratorio: $("#txtLaboratorio").val(),
            Observaciones: $("#txtObservacion").val(),
            Otros: $("#txtOtros").val(),
            siAprobar: 0,
            anioBusqueda: $("#selectAnio").val(),
            mesBusqueda: moment($("#txtFechaCabecera").val()).format('MM'),
            op: 1
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            }
            if (resultado == '3') {
                MensajeAdvertencia('¡Ya existe un registro con ese DIA: <span class="badge badge-danger">' + moment($("#txtFechaCabecera").val()).format('DD') + '</span>!');
                $('#cargac').hide();
                return;
            }
            $('#ModalIngresoCabecera').modal('hide');
            setTimeout(function () {
                LimpiarCabecera();
                itemEditar = 0;
                $('#cargac').hide();
                CargarCabecera(1);
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ActualizarCabecera(jdata) {
    ConsultarEstadoReporte(2);
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            estadoReporte = [];
            $("#txtFechaCabecera").prop('disabled', true);
            $("#txtFechaCabecera").val(moment(jdata.FechaDIA).format("YYYY-MM-DD"));
            //$("#txtFechaCabecera").val(date[0].defaultValue);
            $("#txtLaboratorio").val(jdata.Laboratorio);
            $("#txtOtros").val(jdata.Otros);
            $("#txtObservacion").val(jdata.Observaciones);
            $('#ModalIngresoCabecera').modal('show');
            itemEditar = jdata;
        }
    },200);   
}

function ModalIngresoCabecera() {
    ConsultarEstadoReporte(2);
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            estadoReporte = [];
            LimpiarCabecera();
            $("#txtFechaCabecera").prop('disabled', false);
            var fecha = moment($("#txtFecha").val()).format("YYYY-MM-DD");
            var fechaSplit = fecha.split('-');
            var nuevaFecha = fechaSplit[0] + '-' + $('#selectMonth').val() + '-' + fechaSplit[2];
            $('#ModalIngresoCabecera').modal('show');
            $("#txtFechaCabecera").val(moment(nuevaFecha).format("YYYY-MM-DD"));
        }
    }, 200);    
}

function LimpiarCabecera() {
    $('#txtFechaCabecera').val('');
    $('#txtOtros').val('');
    $('#txtObservacion').val('');
    $('#txtLaboratorio').val('');
    $("#txtFechaCabecera").css('border', '');
    $("#txtOtros").css('border', '');
    $("#txtLaboratorio").css('border', '');
    $("#txtObservacion").css('border', '');
}

function ValidarDatosVacios() {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera();
}

function OnChangeTextBox() {  
    var con = 0;
    if ($('#txtFechaCabecera').val() == '') {
        $("#txtFechaCabecera").css('border', '1px dashed red');
        con = 1;
    } else $("#txtFechaCabecera").css('border', '');
    if ($('#txtLaboratorio').val() == '') {
        $("#txtLaboratorio").css('border', '1px dashed red');
        con = 1;
    } else $("#txtLaboratorio").css('border', '');
    return con;
}

function EliminarConfirmar(jdata) {
    ConsultarEstadoReporte(2);
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            estadoReporte = [];
            $("#modalEliminarControl").modal("show");
            itemEditar = jdata;
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    }, 200);    
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/EliminarDesechosLiquidosDetalle",
        type: "POST",
        data: {
            IdDesechosLiquidosDetalle: itemEditar.IdDesechosLiquidosDetalle,
            IdDesechosLiquidos: itemEditar.IdDesechosLiquidos,
            fechaDesde: $('#txtFecha').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdDesechosLiquidosDetalle");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera(1);
                MensajeCorrecto("Registro eliminado con Éxito");
                setTimeout(function () {
                    $('#cargac').hide();
                }, 200);
            } else if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            }
            itemEditar = 0;
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}