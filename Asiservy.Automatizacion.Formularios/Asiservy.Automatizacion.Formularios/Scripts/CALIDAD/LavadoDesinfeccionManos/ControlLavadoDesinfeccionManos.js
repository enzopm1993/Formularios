var ListaDatos = [];
var ListaDatosDetalle = [];
var horaActualizar = '';//0=No, 1=Yes

$(document).ready(function () {
    //CargarCabecera(0);
    DatePicker();
});

function ConsultarEstadoRegistro() {
    var fechaingresada = moment($('#datetimepicker1').datetimepicker('viewDate')).format('MM-DD-YYYY');
    
    $.ajax({
        url: "../LavadoDesinfeccionManos/ConsultarControlLavadoDesinfeccionManos",
        data: {
            fechaControl: fechaingresada,
            turno: document.getElementById('selectTurno').value
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            ListaDatos = resultado;
            CambiarMensajeEstado(resultado.EstadoReporte);
        },
        error: function () {
            MensajeError(Mensajes.Error, false);
        }
    });
}

function CargarCabecera(opcion) {
    $("#divTableEntregaProductoDetalle").html('');
    var op = opcion;
    if ($("#txtFecha").val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    } else {
        var fechaingresada = moment($('#datetimepicker1').datetimepicker('viewDate')).format('MM-DD-YYYY');
        //console.log(fechaingresada);
        $.ajax({
            url: "../LavadoDesinfeccionManos/ConsultarControlLavadoDesinfeccionManos",
            type: "GET",
            data: {               
                fechaControl: fechaingresada,
                turno:document.getElementById('selectTurno').value
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#txtObservacion").val('');
                if (resultado == 0) {
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
                    $("#txtObservacion").val(resultado.Observacion);
                    ListaDatos = resultado;
                    CambiarMensajeEstado(resultado.EstadoReporte);
                    CargarDetalle(0);                    
                }         
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
        idDesinfeccionManos = ListaDatos.IdDesinfeccionManos;
    }
    $('#cargac').show();  
    $.ajax({
        url: "../LavadoDesinfeccionManos/GuardarModificarControlLavadoDesinfeccionManos",
        type: "POST",
        data: {
            IdDesinfeccionManos: idDesinfeccionManos,
            Fecha: moment($('#datetimepicker1').datetimepicker('viewDate')).format('MM-DD-YYYY'),
            Observacion: $("#txtObservacion").val(),
            Turno: document.getElementById('selectTurno').value
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                MensajeCorrecto('Registro guardado correctamente');
            }
            if (resultado == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            }
            if (resultado==3) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            CargarCabecera(0);
            $("#txtObservacion").prop("disabled", true);
            $("#btnModalGenerarRegistro").prop("hidden", true);
            $("#btnModalGenerar").prop("hidden", true);
            $("#btnModalEditar").prop("hidden", false);
            $("#btnModalEliminar").prop("hidden", false);
            $("#divDetalleProceso").prop("hidden", false);
            $("#divDetalleControlCloro").prop("hidden", false);
                $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConfirmar() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (ListaDatos.EstadoReporte == true) {
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
            IdDesinfeccionManos: ListaDatos.IdDesinfeccionManos,
            Fecha: moment(ListaDatos.Fecha).format('DD-MM-YYYY')
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
                
                $("#btnModalGenerar").prop("hidden", false);
                $("#btnModalEditar").prop("hidden", true);
                $("#btnModalEliminar").prop("hidden", true);
                $("#divDetalleProceso").prop("hidden", true);
                $("#txtObservacion").prop("disabled", false);
                $("#txtObservacion").val('');
                $("#divDetalleControlCloro").prop("hidden", true);
                $("#divCabecera2").prop("hidden", true);                
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            $("#modalEliminarControl").modal("hide");
            $('#cargac').hide();
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
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (ListaDatos.EstadoReporte == true) {
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
    ConsultarEstadoRegistro();    
    setTimeout(function () {
        if (ListaDatos.EstadoReporte == true) {
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
            else if (resultado == 2) {
                $("#divModalPartialDetalle").prop("hidden", true);
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                CargarCabecera(0);    
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
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
    //$('#cargac').show();
    var op = opcion; 
    var idDesinfeccionManos = 0;
    if (ListaDatos.length!=0) {
        idDesinfeccionManos = ListaDatos.IdDesinfeccionManos;
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
            //$('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);            
        }
    });
}

function ActulizarDetalle(jFilaSeleccionada, jordenColumnasTabla) {
    ConsultarEstadoRegistro();
    $('#cargac').show();
    setTimeout(function () {
    if (ListaDatos.EstadoReporte == true) {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        $('#cargac').hide();
        return;
    } else {        
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
                MensajeError(Mensajes.Error, false);
            }
        });
        
        setTimeout(function () {
            for (var i in jordenColumnasTabla) {
                for (var j in jFilaSeleccionada) {
                    if (jordenColumnasTabla[i].Codigo == jFilaSeleccionada[j].CodigoLinea) {
                        var nomElemento = 'selectLinea_' + jFilaSeleccionada[j].CodigoLinea;
                        document.getElementById('txtHora').setAttribute("value", jFilaSeleccionada[j].FechaHora);
                        horaActualizar = moment(jFilaSeleccionada[j].FechaHora).format('DD-MM-YYYY HH:mm');//Guardo la hora para validarla al momento de Actualizar el registro
                        document.getElementById(nomElemento).value = jFilaSeleccionada[j].EstadoCumplimiento;
                    }
                }
            }
            $("#ModalGenerarDetalle").modal("show");
            $('#cargac').hide();
        }, 200);
    }
    },500);
}

function ValidarHoraRepetida() {
    var tblObj = $("#tblDataTable").DataTable();
    var form_data = tblObj.rows().data();
    var h = moment($("#txtHora").val()).format('DD-MM-YYYY HH:mm');    
    for (var i in form_data) {
        if (h == horaActualizar) {//horaActualizar la almaceno al momento de dar click en EDITAR, solo si uso el evento onChange la hora se valida caso contrario se
                                  //entiende que no ha cambiado la hora y se la actualiza con la misma hora
            return false;
        }
        if (form_data[i][0] == h.toString()) {
            MensajeAdvertencia("<span class='badge badge-danger'>!Ya existe una FECHA-HORA:    " + form_data[i][0] + "    resgistrada¡</span>",10);
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
        var Hora = moment($("#txtHora").val()).format("YYYY-MM-DDTHH:mm")
        var listaDataArray = new Array();
        var estado;
        var selectText = $('table#tblDataTableIngreso td').map(function () {
            return $(this).find(':selected').map(function () {
                return $(this).html();
            }).get();
        }).get();

        for (var i = 0; i < selectText.length; i++) {
            var listaData = {};
            estado = "false";
            if (selectText[i] == "Seleccione..") {
                $("#selectLinea_" + jdata[i]).css('border', '2px dashed red');
                //MensajeAdvertencia("<span class='badge badge-danger'>¡Se deben ingresar todos los datos requeridos!</span>");
                //return;
            } else {
                $("#selectLinea_" + jdata[i]).css('border', '');
            }
            if (selectText[i] == "C-CUMPLE") {
                estado = "true";
            }
            if (selectText[i] == "N/A") {
                estado = "null";
            }
            if (ListaDatosDetalle != '') {
                for (var j in ListaDatosDetalle) {
                    if (jdata[i] == ListaDatosDetalle[j].CodigoLinea) {
                        listaData.IdDesinfeccionManosDetalle = ListaDatosDetalle[j].IdDesinfeccionManosDetalle;//Para el ingreso(Guardar) no es necesario que tenga datos.
                    }
                }
            } else {
                listaData.IdDesinfeccionManosDetalle = ListaDatosDetalle.IdDesinfeccionManosDetalle;
            }
            listaData.IdDesinfeccionManos = ListaDatos.IdDesinfeccionManos;
            listaData.Hora = Hora;
            listaData.CodigoLinea = jdata[i];
            listaData.EstadoCumplimiento = estado;
            listaDataArray.push(listaData);
        }
        ListaDatosDetalle = '';
        GuardarDetalle(listaDataArray);
        horaActualizar = '';
    }
}

function quitarEstiloSelect(numSelect) {
    var nombreSelect = "#" + numSelect.id;
    $(nombreSelect).css('border', '');
}

function EliminarConfirmarDetalle(jdata) {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (ListaDatos.EstadoReporte == true) {
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
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
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

function DatePicker() {
    $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
        icons: {
            time: 'far fa-clock',
            date: 'far fa-calendar-alt',
            up: 'fas fa-caret-up',
            down: 'fas fa-caret-down',
            previous: 'fas fa-backward',
            next: 'fas fa-forward',
            today: 'fas fa-calendar-day',
            clear: 'fas fa-trash-alt',
            close: 'fas fa-window-close'
        }
    });
    $('#datetimepicker1').datetimepicker(
        {
            //date: moment().format("DD-MM-YYYY"),
            format: "DD-MM-YYYY",
            //minDate: model.Fecha,
            maxDate: moment(),
            ignoreReadonly: true
        });
}

$("#datetimepicker1").on("change.datetimepicker", ({ date, oldDate }) => {
    CargarCabecera();
})
