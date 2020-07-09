var itemCabecera = [];
var estadoReporte = [];
var itemDetalle = [];
var siActualizar = [];
var itemAccionCorrectiva = [];
var itemAccionCorrectivaG = [];
var rotation = 0;
var actualizarSi = false;
$(document).ready(function () {
    CargarCabecera();
    var x = document.getElementById("selectVerificacion");
    var option = document.createElement("option");
    option.text = 'Todos';
    option.value = 'galo';
    x.add(option);
    $('#selectTurno').select2({
        width: '100%'
    });
    $('#selectTurnoIngresar').select2({
        width: '100%'
    });
    $('#selectVerificacion').select2({
        width: '100%'
    });
});

function ConsultarEstadoRegistro() {
    $.ajax({
        url: "../MaterialQuebradizo/ConsultarEstadoReporte",
        data: {
            fechaControl: $("#txtFecha").val(),
            turno: document.getElementById('selectTurno').value
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            estadoReporte = resultado.EstadoReporte;
            CambiarMensajeEstado(resultado.EstadoReporte);
        },
        error: function () {
            MensajeError(Mensajes.Error, false);
        }
    });
}

function CargarCabecera() {
    if (document.getElementById('selectTurno')==null) {
        $('#divBotonCrear').prop('hidden', true);
        $('#divBotonCrearDetalle').prop('hidden', true);
        return;
    }
    $('#cargac').show();
    if ($('#txtFecha').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../MaterialQuebradizo/ConsultarEstadoReporte",
        data: {
            fechaControl: $("#txtFecha").val(),
            turno: document.getElementById('selectTurno').value
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divMostrarCabecera').prop('hidden', true);
                $("#divMostarTablaCabecera").html("No existen registros");
                $('#divBotonCrear').prop('hidden', false);
                $('#divBotonCrearDetalle').prop('hidden', true);
                $('#divMostarTablaDetallesVer').prop('hidden', true);
                $('#divMostarTablaDetallesVer').html(resultado);
                itemCabecera = [];
                LimpiarModalIngresoCabecera();
                CambiarMensajeEstado('nada');
            } else {
                itemCabecera = resultado;
                CambiarMensajeEstado(resultado.EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaCabeceraVer").val(moment(resultado.Fecha).format('YYYY-MM-DD'));
                $("#txtObservacionVer").val(resultado.ObservacionCtrl);

                CargarDetalle(1);
            }
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function GuardarCabecera(siAprobar) {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {

            $.ajax({
                url: "../MaterialQuebradizo/GuardarModificarMaterialQuebradizo",
                type: "POST",
                data: {
                    IdMaterial: itemCabecera.IdMaterial,
                    Fecha: $("#txtIngresoFecha").val(),
                    ObservacionCtrl: $("#txtIngresoObservacion").val(),
                    Turno: document.getElementById('selectTurnoIngresar').value,
                    siAprobar: siAprobar
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    } else if (resultado == 3) {
                        MensajeAdvertencia('Error al ingresar la FECHA  : <span class="badge badge-danger">' + moment($("#txtIngresoFecha").val()).format('DD-MM-YYYY') + '</span>');
                        return;
                    } else if (resultado == 4) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        return;
                    } else if (resultado == 5) {
                        var e = document.getElementById("selectTurnoIngresar");
                        var turnoExiste = e.options[e.selectedIndex].text;
                        MensajeAdvertencia('Error el TURNO ya esta ingresado  : <span class="badge badge-danger">' + turnoExiste + '</span>');
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {                       
                        MensajeAdvertencia(Mensajes.MensajePeriodo);                 
                    }
                    $('#ModalIngresoCabecera').modal('hide');
                    $('#divBotonesCRUD').prop('hidden', false);
                    $('#divMostarTablaDetalle').prop('hidden', false);
                    $('#divBotonCrear').prop('hidden', true);
                    itemCabecera = [];
                    $('#cargac').hide();
                    CargarCabecera();
                },
                error: function () {
                    $('#cargac').hide();
                    MensajeError(Mensajes.Error, false);
                }
            });
        }
    }, 200);
}

function EliminarConfirmar() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    }, 200);
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {

            $.ajax({
                url: "../MaterialQuebradizo/EliminarMaterialQuebradizo",
                type: "POST",
                data: {
                    IdMaterial: itemCabecera.IdMaterial
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == "0") {
                        MensajeAdvertencia("Falta Parametro IdLavadoCisterna");
                        $("#modalEliminarControl").modal("hide");
                        $('#cargac').hide();
                        return;
                    } else if (resultado == "1") {
                        $('#firmaDigital').prop('hidden', true);
                        $("#modalEliminarControl").modal("hide");
                        CargarCabecera();
                        MensajeCorrecto("Registro eliminado con Éxito");
                        $('#cargac').hide();
                    } else if (resultado == '2') {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
                        $('#cargac').hide();
                        return;
                    }
                    itemCabecera = [];
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(Mensajes.Error, false);
                }
            });
        }
    }, 200);
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            LimpiarModalIngresoCabecera();
            $("#txtIngresoFecha").val(moment(itemCabecera.Fecha).format("YYYY-MM-DD"));
            $("#txtIngresoObservacion").val(itemCabecera.ObservacionCtrl);
            $('#selectTurnoIngresar').val(document.getElementById('selectTurno').value).trigger('change');     
            $('#ModalIngresoCabecera').modal('show');
        }
    }, 200);
}

function ModalIngresoCabecera() {    
    LimpiarModalIngresoCabecera();
    $('#selectTurnoIngresar').val(document.getElementById('selectTurno').value ).trigger('change');
    $('#ModalIngresoCabecera').modal('show');
    itemCabecera = [];
}

function LimpiarModalIngresoCabecera() {
    $('#txtIngresoFecha').val(moment($('#txtFecha').val()).format('YYYY-MM-DD'));
    $('#txtIngresoObservacion').val('');
}

function ValidarDatosVacios(siAprobar) {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera(siAprobar);
}

function OnChangeTextBox() {
    var con = 0;
    if ($('#txtIngresoFecha').val() == '') {
        $("#txtIngresoFecha").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtIngresoFecha").css('border', ''); }
    return con;
}

//DETALLE
function CargarDetalle() {
    $('#cargac').show();
    var op = 0;
    $.ajax({
        url: "../MaterialQuebradizo/ConsultarDetallePartial",
        data: {
            IdMaterial: itemCabecera.IdMaterial,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaDetallesVer").html("No existen registros");
                $('#divBotonCrearDetalle').prop('hidden', false);
                $('#selectVerificacion').prop('disabled', false);
            } else {
                $('#divMostarTablaDetallesVer').prop('hidden', false);
                //$('#divBotonCrearDetalle').prop('hidden',true);
                $('#divMostarTablaDetallesVer').html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            //console.log(resultado.responseText, false)
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function ModalIngresoDetalle() {
    siActualizar = false;
    LimpiarDetalle();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {     
            ArmarTablaIngreso();
            $('#ModalIngresoDetalle').modal('show');
            $('#cargac').hide();
        }
    }, 200);
}

function GuardarDetalle(jdataAreas, jdataMateriales) {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#cargac').show();
            listaDetalle = new Array;
            jdataAreas.forEach(function (rowArea) {
                jdataMateriales.forEach(function (rowMateriales) {
                    var element = document.getElementById('checkMateriales_' + rowArea.IdMantenimiento + '_' + rowMateriales.IdMantMaterial);
                    if (typeof (element) != 'undefined' && element != null) {
                        var check = $('#checkMateriales_' + rowArea.IdMantenimiento + '_' + rowMateriales.IdMantMaterial).prop('checked');
                        var obs = document.getElementById('txtObservacion_' + rowArea.IdMantenimiento).value;
                        if (check == true) {
                            detalle = {};
                            itemDetalle.forEach(function (rowDetalle) {
                                if (rowDetalle.IdMantenimiento == rowArea.IdMantenimiento && rowDetalle.IdMantMaterial == rowMateriales.IdMantMaterial) {
                                    detalle.IdMaterialDetalle = rowDetalle.IdMaterialDetalle;
                                }
                            });                                                        
                            

                            detalle.IdMantenimiento = rowArea.IdMantenimiento;
                            detalle.IdMaterial = itemCabecera.IdMaterial;
                            detalle.IdMantMaterial = rowMateriales.IdMantMaterial;
                            detalle.EstadoVerificacion = check;
                            detalle.Observaciones = obs;
                            listaDetalle.push(detalle);
                        }
                    }
                });
            });

            $.ajax({
                url: "../MaterialQuebradizo/GuardarModificarDetalle",
                type: "POST",
                data: {
                    listaDetalle: listaDetalle
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    } else if (resultado == 2) {
                        MensajeAdvertencia('¡Error! No se a guardado  : <span class="badge badge-danger">' + 'SIN DATOS' + '</span>');
                        $('#cargac').hide();
                        $('#ModalIngresoDetalle').modal('hide');
                        return;
                    } else if (resultado == 3) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
                    }
                    $('#selectTurnoFiltro').val($('#selectTurno').val());
                    $('#selectAreaAuditarFiltro').val($('#selectAreaAuditar').val()).trigger('change');
                    LimpiarDetalle();
                    CargarDetalle();
                    $('#ModalIngresoDetalle').modal('hide');
                    $('#cargac').hide();
                },
                error: function () {
                    $('#cargac').hide();
                    MensajeError(Mensajes.Error, false);
                }
            });
        }
    }, 200);
}

async function ActualizarDetalle(jModel) {
    document.getElementById('selectVerificacion').value = jModel[0].TipoVerificacion;
    ModalIngresoDetalle();    
    $('#cargac').show();
    setTimeout(function () {
        //$('#selectVerificacion').prop('disabled', true);
        jModel.forEach(function (rowModel) {
            $('#checkMateriales_' + rowModel.IdMantenimiento + '_' + rowModel.IdMantMaterial).prop('checked', rowModel.EstadoVerificacion);
            document.getElementById('txtObservacion_' + rowModel.IdMantenimiento).value = rowModel.Observaciones;
        });        
    },500);
    
    itemDetalle = jModel;
    siActualizar = true;
    $('#cargac').hide();

}

function CambiarMensajeEstado(estadoReporteParametro) {
    if (estadoReporteParametro == true) {
        $("#lblAprobadoPendiente").text("APROBADO");
        $("#lblAprobadoPendiente").removeClass('badge-danger');
        $("#lblAprobadoPendiente").addClass('badge badge-success');
    } else if (estadoReporteParametro == false) {
        $("#lblAprobadoPendiente").text("PENDIENTE");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").addClass('badge badge-danger');
    } else if (estadoReporteParametro == 'nada') {
        $("#lblAprobadoPendiente").text("");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").removeClass('badge badge-danger');
    }
}

function OnChangeTextBoxDetalle() {
    var con = 0;
    if ($('#txtIngresoFechaDetalle').val() == '') {
        $("#txtIngresoFechaDetalle").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtIngresoFechaDetalle").css('border', ''); }
    return con;
}

function EliminarConfirmarDetalle(jdata) {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControlDetalle").modal("show");
            $("#myModalLabelDetalle").text("¿Desea Eliminar el detalle?");
            itemDetalle = jdata;
        }
    }, 200);
}

function EliminarDetalleSi() {
    var model = [];
    itemDetalle.forEach(function (det) {
        var detalle = {};
        detalle.IdMaterial = det.IdMaterial;
        detalle.IdMaterialDetalle = det.IdMaterialDetalle;
        detalle.IdMantMaterial = det.IdMantMaterial;
        detalle.IdMantenimiento = det.IdMantenimiento;
        detalle.EstadoVerificacion = det.EstadoVerificacion;
        detalle.Observaciones = det.Observaciones;
        model.push(detalle);
    });   
    $.ajax({
        url: "../MaterialQuebradizo/EliminarDetalle",
        type: "POST",
        data: {
            listaDetalle: model
        },
        success: function (resultado) {
            itemDetalle = [];
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdMaterial");
                $("#modalEliminarControlDetalle").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControlDetalle").modal("hide");
                CargarDetalle();
                MensajeCorrecto("Registro eliminado con Éxito");                
                $('#cargac').hide();
            } else if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            } else if (resultado == '3') {
                MensajeAdvertencia('¡No se encontro ningun registro Cabecera en esta fecha!');
                $('#cargac').hide();
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function LimpiarDetalle() {    
    var date = new Date();
    $('#txtIngresoFechaDetalle').val(moment(date).format('HH:mm'));    
    itemDetalle = [];
}

function EliminarDetalleNo() {
    $("#modalEliminarControlDetalle").modal("hide");
}

function ArmarTablaIngreso() {
    var verificacion = $('#selectVerificacion').val();
    if ($('#selectVerificacion').val()=='galo') {
        verificacion = null;
    }
    $.ajax({
        url: "../MaterialQuebradizo/ControlMaterialIngresoPartial",
        type: 'Post',
        data: {
            verificacion: verificacion
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
            $('#divMostrarTablaIngresoDetalle').html(resultado);
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function AccionCorrectiva(jdata) {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            LimpiarAccionCorrectiva();
            itemAccionCorrectiva = jdata;
            itemAccionCorrectivaG = jdata;
            $("#modalAccionCorrectiva").modal("show");
            CargarAccionCorrectiva();
        }
    }, 200);
}

function GuardarAccionCorrectiva() {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {            
            if (OnChangeTextBoxAccion() == 1) {
                MensajeAdvertencia('Ingrese todos los datos requeridos');
                $('#cargac').hide();
                return;
            }
            $('#cargac').show();
            var imagen = $('#file-upload')[0].files[0];
            var data = new FormData();
            data.append("dataImg", imagen);
            data.append("IdMaterial", itemAccionCorrectiva.IdMaterial);
            data.append("IdMantenimiento", itemAccionCorrectiva.IdMantenimiento);
            data.append("IdAccion", itemAccionCorrectiva.IdAccion);
            data.append("DescripcionAccion", $("#txtAccionCorrectiva").val());
            data.append("Rotation", rotation);
            $.ajax({
                url: "../MaterialQuebradizo/GuardarModificarAccionCorrectiva",
                type: "POST",
                cache: false,
                data: data,
                contentType: false,
                processData: false,
                async: false,
                data:data ,
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Acción Correctiva guardada correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    } else if (resultado == 3) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 4) {
                        MensajeAdvertencia('¡Solo se permiten imagenes .JPG y .PNG!', 5);
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
                    }else {
                        var mb = parseFloat(resultado / (1024 * 1024)).toFixed(2);
                        MensajeAdvertencia('¡Exedio el limite de capacidad permitido!:  <span class="badge badge-success">5Mb</span>: Su imagen:<span class="badge badge-danger">' + mb + 'Mb</span>');
                        $('#cargac').hide();
                        return;
                    }
                    CargarAccionCorrectiva();
                    LimpiarAccionCorrectiva();
                    NuevaFoto();                    
                    $('#cargac').hide();
                },
                error: function (resultado) {
                    //console.log(resultado.innerText);
                    $('#cargac').hide();
                    MensajeError(Mensajes.Error, false);
                }
            });
        }
    }, 200);
}

function OnChangeTextBoxAccion() {
    var con = 0;
    if ($('#txtAccionCorrectiva').val() == '') {
        $("#txtAccionCorrectiva").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtAccionCorrectiva").css('border', ''); }
    if (!actualizarSi) {
        if ($('#file-upload').val() == '') {
            $("#file-upload").css('border', '1px dashed red');
            con = 1;
        } else { $("#file-upload").css('border', ''); }
        if ($('#lblfoto').val() == '') {
            $("#lblfoto").css('border', '1px dashed red');

        } else { $("#lblfoto").css('border', ''); }
    }
    return con;
}

function LimpiarAccionCorrectiva() {
    $("#txtAccionCorrectiva").val("");
    $("#file-upload").val('');
    $("#file-preview-zone").html('');
    $('#lblfoto').text('Seleccione archivo');
    $("#lblfoto").css('border', '');
    $("#file-upload").css('border', '');
    $("#txtAccionCorrectiva").css('border', ''); 
    rotation = 0;    
    actualizarSi = false;
}

function CargarAccionCorrectiva() {
    $('#cargac').show();
    var op = 1;
    $.ajax({
        url: "../MaterialQuebradizo/VerCrearImagenPartial",
        data: {
            idMaterial: itemCabecera.IdMaterial,
            idArea: itemAccionCorrectiva.IdMantenimiento,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#divListarFotos').html(resultado);
            $('#cargac').hide();
        },
            error: function (resultado) {
            //console.log(resultado.responseText, false)
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function readFile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#file-preview-zone").html('');
            var filePreview = document.createElement('img');
            filePreview.id = 'file-preview';
            filePreview.src = e.target.result;
            var previewZone = document.getElementById('file-preview-zone');
            previewZone.appendChild(filePreview);
            $("#file-preview").addClass("img");
            var image = new Image();
            image.src = e.target.result;
            image.onload = function () {
                //if (this.width < this.height) {
                    document.getElementById("file-preview").style.height = "250px";
                    document.getElementById("file-preview").style.width = "250px";
                //}
                //else {
                //    document.getElementById("file-preview").style.height = "300px";
                //    document.getElementById("file-preview").style.width = "250px";
                //}
            };
        }
        reader.readAsDataURL(input.files[0]);
    }
}

var fileUpload = document.getElementById('file-upload');

fileUpload.onchange = function (e) {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    readFile(e.srcElement);
}

$('#file-preview-zone').on("click", function (e) {
    rotation += 90;
    $('#file-preview').rotate(rotation);
    if (rotation == 360) {
        rotation = 0;
    }
});

function validarImg(rotacion, id, imagen) {
    $('#' + id).rotate(parseInt(rotacion));
    var img = new Image();
    document.getElementById(id).style.borderRadius = "20px";
    document.getElementById(id).style.height = "250px";
    document.getElementById(id).style.width = "250px";
    img.src = $('#btnPath').val() + imagen;
}

function SalirAccicionCorrectiva() {
    itemAccionCorrectiva = [];
    itemDetalle = [];
    $('#modalAccionCorrectiva').modal('hide');
    LimpiarAccionCorrectiva();
}

function EditarAccionCorrectiva(jdata) {
    LimpiarAccionCorrectiva();
    itemAccionCorrectiva = [];
    actualizarSi = true;
    $("#txtAccionCorrectiva").val(jdata.DescripcionAccion);
    if (jdata.RutaFoto != null && jdata.RutaFoto != '') {
        var filePreview = document.createElement('img');
        filePreview.id = 'file-preview';
        filePreview.src = $('#btnPath').val() + jdata.RutaFoto;
        var previewZone = document.getElementById('file-preview-zone');
        previewZone.appendChild(filePreview);

        $("#file-preview").addClass("img");
        $('#file-preview').rotate(parseInt(jdata.Rotation));

        document.getElementById("file-preview").style.height = "250px";
        document.getElementById("file-preview").style.width = "250px";
        itemAccionCorrectiva = jdata;
    }
}

function NuevaFoto() {
    itemAccionCorrectiva = itemAccionCorrectivaG;
    LimpiarAccionCorrectiva();
}

function EliminarAccionCorrectiva() {    
    $.ajax({
        url: "../MaterialQuebradizo/EliminarAccionCorrectiva",
        type: "POST",
        data: {
            IdAccion: itemAccionCorrectiva.IdAccion,
            IdMaterial: itemAccionCorrectiva.IdMaterial
        },
        success: function (resultado) {            
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdAccion");
                $("#modalEliminarControlDetalle").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                CargarAccionCorrectiva();
                $("#modalEliminarAccionCorrectiva").modal("hide");                
                MensajeCorrecto("Registro eliminado con Éxito");                                
            } else if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            $("#modalAccionCorrectiva").modal("show");
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function EliminarAccionCorrectivaNo() {    
    $("#modalEliminarAccionCorrectiva").modal("hide");    
    $("#modalAccionCorrectiva").modal("show");
}

function EliminarConfirmarAccionCorrectiva(jdata) {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalAccionCorrectiva").modal("hide");
            $("#modalEliminarAccionCorrectiva").modal("show");
            $("#accionCorrectiva").text("¿Desea Eliminar la Acción Correctiva?");
            itemAccionCorrectiva = jdata;
        }
    }, 200);
}