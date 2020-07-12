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
                $('#firmaDigital').prop('hidden', true);
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $('#firmaDigital').prop('hidden', false);
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
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarCabecera(op) {
    ConsultarEstadoReporte(2);
    $('#cargac').show();
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
                $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {    
    var date = new Date();
    var m = new Date().getMonth()+1;
    var mingreso = moment($("#txtFechaCabecera").val()).format('DD-MM-YYYY');
    var mactual = moment(date).format('DD-MM-YYYY');
    if (m == moment($("#txtFechaCabecera").val()).format('MM')) {
        if (mingreso > mactual) {
            MensajeAdvertencia('La fecha no puede ser mayor a la fecha actual: <span class="badge badge-danger">' + moment(date).format('DD-MM-YYYY') + '</span>');
            return;
        }
    }
   
    
    if (moment($("#txtFechaCabecera").val()).format('MM') > m) {
        MensajeAdvertencia('El Mes no puede ser mayor al mes actual <span class="badge badge-danger">' + moment(date).format('MM') + '</span>');
        return;
    }
    if (moment($("#txtFechaCabecera").val()).format('MM') < $('#selectMonth').val() || moment($("#txtFechaCabecera").val()).format('MM') > $('#selectMonth').val()) {
        MensajeAdvertencia('El Mes no puede ser menor/mayor al mes seleccionado <span class="badge badge-danger">' + $('#selectMonth').val() + '</span>');
        return;
    }
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
                LimpiarCabecera();
                itemEditar = 0;
                $('#cargac').hide();
                CargarCabecera(1);
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
            if ($('#selectMonth').val() == 2 && fechaSplit[2]>28) {//CONTROL PARA CUANDO ES FEBRERO SI SE LECCCIONA LA FECHA now() UN 30 DE ABRIL SE LE BAJA DOS DIAS
                fechaSplit[2] = fechaSplit[2] - 2;
            }
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
    ConsultarEstadoReporte(2);
    setTimeout(function () {
        if (estadoReporte == true) {
            $("#modalEliminarControl").modal("hide");
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
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
    }, 200);   
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}