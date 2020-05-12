var ListaDatos = [];
var ListaDatosDetalle = [];
var horaActualizar = '';//0=No, 1=Yes

$(document).ready(function () {
    CargarCabecera(0);
});

function CargarCabecera(opcion) {
    $('#cargac').show();
    $("#divTableEntregaProductoDetalle").html('');
    var op = opcion;
    if ($("#txtFecha").val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    } else {
        $.ajax({
            url: "../LavadoDesinfeccionManos/ConsultarControlLavadoDesinfeccionManos",
            type: "GET",
            data: {
                fechaDesde: $("#txtFecha").val(),
                fechaHasta: $("#txtFecha").val(),
                opcion:op
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#txtObservacion").val('');
                if (resultado.length == 0) {
                    $("#txtObservacion").prop("disabled", false);
                    $("#divDetalleControlCloro").prop("hidden", true);
                    $("#btnModalEditar").prop("hidden", true);
                    $("#btnModalEliminar").prop("hidden", true);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalGenerarRegistro").prop("hidden", false);
                    ListaDatos = [];
                    $("#lblAprobadoPendiente").text("");
                } else {
                    $("#divDetalleControlCloro").prop("hidden", false);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalEditar").prop("hidden", false);
                    $("#btnModalEliminar").prop("hidden", false);                    
                    $("#txtObservacion").prop("disabled", true);
                    $("#btnModalGenerarRegistro").prop("hidden", true);
                    $("#txtObservacion").val(resultado[0].Observacion);
                    ListaDatos = resultado;
                    if (resultado[0].EstadoReporte == true) {
                        $("#lblAprobadoPendiente").text("APROBADO");
                        $("#lblAprobadoPendiente").removeClass('badge-danger');
                        $("#lblAprobadoPendiente").addClass('badge badge-success');
                    } else {
                        $("#lblAprobadoPendiente").text("PENDIENTE");
                        $("#lblAprobadoPendiente").removeClass('badge-success');
                        $("#lblAprobadoPendiente").addClass('badge badge-danger');}
                    CargarDetalle(0);                    
                }
                setTimeout(function () {
                    $('#cargac').hide();
                },200);               
            },
            error: function (resultado) {
                $('#cargac').hide();
                MensajeError(resultado.responseText, false);                
            }
        });
    }
}

function GuardarCabecera() {
    var idDesinfeccionManos = 0;
    if (ListaDatos.length != 0) {
        idDesinfeccionManos = ListaDatos[0].IdDesinfeccionManos;
    }
    $('#cargac').show();  
    $.ajax({
        url: "../LavadoDesinfeccionManos/GuardarModificarControlLavadoDesinfeccionManos",
        type: "POST",
        data: {
            IdDesinfeccionManos: idDesinfeccionManos,
            Fecha: moment($("#txtFecha").val()).format("YYYY-MM-DD"),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera(0);
            $("#txtObservacion").prop("disabled", true);
            $("#btnModalGenerarRegistro").prop("hidden", true);
            $("#btnModalGenerar").prop("hidden", true);
            $("#btnModalEditar").prop("hidden", false);
            $("#btnModalEliminar").prop("hidden", false);
            $("#divDetalleProceso").prop("hidden", false);
            $("#divDetalleControlCloro").prop("hidden", false);
            setTimeout(function () {
                $('#cargac').hide();
            },200);       
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConfirmar() {
    CargarCabecera(0);
    setTimeout(function () {
        if (ListaDatos[0].EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
        }
    },500);
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    $.ajax({
        url: "../LavadoDesinfeccionManos/EliminarControlLavadoDesinfeccionManos",
        type: "POST",
        data: {
            IdDesinfeccionManos: ListaDatos[0].IdDesinfeccionManos
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdDesinfeccionManos");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado=="1"){
                $("#divTableEntregaProductoDetalle").prop("hidden", true);
                CargarCabecera(0);
                MensajeCorrecto("Registro Eliminado con Éxito");
                $("#txtFecha").prop("disabled", false);
                $("#modalEliminarControl").modal("hide");
                $("#btnModalGenerar").prop("hidden", false);
                $("#btnModalEditar").prop("hidden", true);
                $("#btnModalEliminar").prop("hidden", true);
                $("#divDetalleProceso").prop("hidden", true);
                $("#txtObservacion").prop("disabled", false);
                $("#txtObservacion").val('');
                $("#divDetalleControlCloro").prop("hidden", true);
                $("#divCabecera2").prop("hidden", true);
                setTimeout(function () {
                    $('#cargac').hide();
                },500);                
            }
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

function ActualizarCabeceraActivarCotroles() {
    CargarCabecera(0);
    setTimeout(function () {
        if (ListaDatos[0].EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#txtObservacion").prop("disabled", false)
            $("#btnModalEliminar").prop("hidden", true);
            $("#btnModalGenerarRegistro").prop("hidden", false);
            $("#btnModalEditar").prop("hidden", true);
        }
    },500);
}

//DETALLE INGRESO DE LINEAS (SE LISTA EL CLASIFICADOR PARA PODER ARMAR LA CABECERA DE LA TABLA)
function ModalGenerarDetalle() {    
    CargarCabecera(0);    
    setTimeout(function () {
        if (ListaDatos[0].EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#ModalGenerarDetalle").modal("show");
            $.ajax({
                url: "../LavadoDesinfeccionManos/IngresoLavadoDesinfeccionManosDetallePartial",
                type: "GET",
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == "0") {
                        return 0;
                    } else {
                        $("#divModalPartialDetalle").prop("hidden", false);
                        $("#divModalPartialDetalle").html(resultado);
                        return 1;
                    }
                },
                error: function (resultado) {
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    },500);
}

function limpiarDetalle(data) {
    for (var i in data) {
        var nomElemento = 'selectLinea_' + data[i].CodigoLinea;
        document.getElementById(nomElemento).value = '';
    }
    horaActualizar = '';
}

function GuardarDetalle(jdata) {
    $('#cargac').show();
    $("#ModalGenerarDetalle").modal("hide");
    var data = jdata;
    $.ajax({
        url: "../LavadoDesinfeccionManos/GuardarModificarControlLavadoDesinfeccionManosDetalle",
        type: "POST",
        data: {model: data },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            
            if (resultado == 0) {
                MensajeCorrecto("Datos guardados correctamente");
            } else if (resultado == 1) { MensajeCorrecto("Actualización de datos correcta"); }
            else {
                MensajeError("!Error al Guardar/Actuaizar los datos¡");
                return;
            }
            //CargarCabecera(0);
            CargarDetalle(0);
            limpiarDetalle(data);
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
                MensajeError(resultado.responseText, false);                
            }
        });            
}

//Retorna PartialView
function CargarDetalle(opcion) {
    var op = opcion; 
    var idDesinfeccionManos = 0;
    if (ListaDatos.length!=0) {
        idDesinfeccionManos = ListaDatos[0].IdDesinfeccionManos;
    }
    $.ajax({
        url: "../LavadoDesinfeccionManos/LavadoDesinfeccionManosDetallePartial",
        type: "GET",
        data: {
            IdDesinfeccionManos: idDesinfeccionManos,
            opcion: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableEntregaProductoDetalle").html("No existen registros");
            } else {                
                $("#divTableEntregaProductoDetalle").prop("hidden", false);
                $("#divTableEntregaProductoDetalle").html('');
                $("#divTableEntregaProductoDetalle").html(resultado);
                $("#divDetalleControlCloro").prop("hidden", false);
            }
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);            
        }
    });
}

function ActulizarDetalle(jFilaSeleccionada, jordenColumnasTabla) {
    CargarCabecera(0);
    setTimeout(function () {
    if (ListaDatos[0].EstadoReporte == true) {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        return;
    } else {
        $('#cargac').show();
        ListaDatosDetalle = jFilaSeleccionada;//Con esto Guardo el json para usarlo en el metodo prepararAntesGuardar(jdata)
        $.ajax({
            url: "../LavadoDesinfeccionManos/IngresoLavadoDesinfeccionManosDetallePartial",
            type: "GET",
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado == "0") {
                    return 0;
                } else {
                    $("#divModalPartialDetalle").prop("hidden", false);
                    $("#divModalPartialDetalle").html(resultado);
                    return 1;
                }
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
        
        setTimeout(function () {
            for (var i in jordenColumnasTabla) {
                for (var j in jFilaSeleccionada) {
                    if (jordenColumnasTabla[i].Codigo == jFilaSeleccionada[j].CodigoLinea) {
                        var nomElemento = 'selectLinea_' + jFilaSeleccionada[j].CodigoLinea;
                        document.getElementById('txtHora').setAttribute("value", jFilaSeleccionada[j].Hora);
                        horaActualizar = jFilaSeleccionada[j].Hora;//Guardo la hora para validarla al momento de Actualizar el registro
                        document.getElementById(nomElemento).value = jFilaSeleccionada[j].EstadoCumplimiento;
                    }
                }
            }
            $("#ModalGenerarDetalle").modal("show");
            $('#cargac').hide();
        }, 2000);
    }
    },500);
}

function ValidarHoraRepetida() {
    var tblObj = $("#tblDataTable").DataTable();
    var form_data = tblObj.rows().data();
    var h = moment($("#txtFecha").val()+' '+$("#txtHora").val()).format('HH:mm:ss');    
    for (var i in form_data) {
        if (h == horaActualizar) {//horaActualizar la almaceno al momento de dar click en EDITAR, solo si uso el evento onChange la hora se valida caso contrario se
                                  //entiende que no ha cambiado la hora y se la actualiza con la misma hora
            return false;
        }
        if (form_data[i][1] == h) {
            MensajeAdvertencia("<span class='badge badge-danger'>!Ya existe una HORA:    " + form_data[i][1] + "    resgistrada¡</span>",10);
            return true;
        }
    }    
}

function controlDiaMayorMenor() {
    var fecha = moment($("#txtHora").val()).format('YYYY-MM-DD');
    var fechasum = moment($("#txtFecha").val()).add(0, 'day').format('YYYY-MM-DD');
    if (fecha > fechasum) {
        MensajeAdvertencia('Solo se permite una dia mayor a la fecha de CONTROL ' + moment($("#txtFecha").val()).format('DD-MM-YYYY'));        
        CargarHora();
    }
    if ($("#txtHora").val() < $("#txtFecha").val()) {
        MensajeAdvertencia('No se puede ingresar una fecha menor a la fecha del CONTROL ' + moment($("#txtFecha").val()).format('DD-MM-YYYY'));    
        CargarHora();
    }    
}

function CargarHora() {
    var fechaactual = new Date();
    var horaactual = moment(fechaactual).format('HH:mm');
    var fecha = $("#txtFecha").val();
    var fechahora = moment(fecha + " " + horaactual).format('YYYY-MM-DDTHH:mm');
    $("#txtHora").val(fechahora);
}

function prepararAntesGuardar(jdata) {//jdata tare el orden en la que se van a guardar las columnas.   
    if (ValidarHoraRepetida() == true) {
        return;
    } else {
        var Hora = moment($("#txtFecha").val() + ' ' + $("#txtHora").val()).format("YYYY-MM-DDTHH:mm")
        var listaData = [{}, {}, {}, {}, {}, {}, {}, {}];
        var estado;
        var selectText = $('table#tblDataTableIngreso td').map(function () {
            return $(this).find(':selected').map(function () {
                return $(this).html();
            }).get();
        }).get();

        for (var i = 0; i < selectText.length; i++) {
            estado = "false";
            if (selectText[i] == "Seleccione..") {
                $("#selectLinea_" + jdata[i]).css('border', '2px dashed red');
                MensajeAdvertencia("<span class='badge badge-danger'>¡Se deben ingresar todos los datos requeridos!</span>");
                return;
            } else {
                $("#selectLinea_" + jdata[i]).css('border', '');
            }
            if (selectText[i] == "C-CUMPLE") {
                estado = "true";
            }
            if (ListaDatosDetalle != '') {
                for (var j in ListaDatosDetalle) {
                    if (jdata[i] == ListaDatosDetalle[j].CodigoLinea) {
                        listaData[i].IdDesinfeccionManosDetalle = ListaDatosDetalle[j].IdDesinfeccionManosDetalle;//Para el ingreso(Guardar) no es necesario que tenga datos.
                    }
                }
            } else {
                listaData[i].IdDesinfeccionManosDetalle = ListaDatosDetalle.IdDesinfeccionManosDetalle;
            }
            listaData[i].IdDesinfeccionManos = ListaDatos[0].IdDesinfeccionManos;
            listaData[i].Hora = Hora;
            listaData[i].CodigoLinea = jdata[i];
            listaData[i].EstadoCumplimiento = estado;
        }
        ListaDatosDetalle = '';
        GuardarDetalle(listaData);
        horaActualizar = '';
    }
}

function quitarEstiloSelect(numSelect) {
    var nombreSelect = "#" + numSelect.id;
    $(nombreSelect).css('border', '');
}

function EliminarConfirmarDetalle(jdata) {
    CargarCabecera(0);
    setTimeout(function () {
        if (ListaDatos[0].EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            ListaDatosDetalle = jdata;
            $("#modalEliminarDetalle").modal("show");
        }
    },500);
}

function EliminarDetalleSi() {
    var jdato = ListaDatosDetalle;
    $("#modalEliminarDetalle").modal("hide");
    $('#cargac').show();
    $.ajax({
        url: "../LavadoDesinfeccionManos/EliminarLavadoDesinfeccionManosDetalle",
        type: "POST",
        data: {
            model: jdato
        },
        success: function (resultado) {
            $('#cargac').hide();
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros (IdDesinfeccionManosDetalle)");
                return;
            }            
            MensajeCorrecto("Registro Eliminado con Éxito");
            CargarDetalle(0);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);

        }
    });
}

function EliminarDetalleNo() {
    ListaDatosDetalle = [];
    $("#modalEliminarDetalle").modal("hide");
}


