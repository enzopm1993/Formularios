var itemCabecera = [];
var itemDetalle = [];
var itemEditar = [];
var itemImagen = [];
var itemEditarImagen = [];
var siActualizar = false;
var rotation = 0;
var inputMask = JSON.parse(document.getElementById('inputMask').value);
var actulizarFoto = false;
var ordenFabricacion = [];
$(document).ready(function () {
    CargarCabecera();
    $('#selectTurno').select2({
        width: '100%'
    });
    $('#selectTurnoInsertar').select2({
        width: '100%'
    });
    $('#selectIngresarLote').select2({
        width: '100%'
    });
    $('#selectParametros').select2({
        width: '100%'
    });
});

async function ConsultarEstadoRegistro() {
    const data = new FormData();
    data.append('idAnalisis', itemCabecera.IdAnalisis);
    var promesa = fetch("../LaboratorioAnalisisQuimico/ConsultarEstadoReporte", {
        method: 'POST',
        body: data
    });
    return promesa;
}

function CargarCabecera() {
    $('#cargac').show();
    if ($('#txtFechaProduccion').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../LaboratorioAnalisisQuimico/ConsultarCabeceraTurno",
        data: {
            fechaControl: $("#txtFechaProduccion").val(),
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
                $('#txtFecha').prop('disabled', false);
                document.getElementById('txtFecha').value = document.getElementById('txtFechaProduccion').value;
                itemCabecera = [];
                LimpiarModalIngresoCabecera();
                CargarParadas();
            } else {
                itemCabecera = resultado;
                CambiarMensajeEstado(resultado.EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaCabeceraVer").val(moment(resultado.Fecha).format('YYYY-MM-DD'));
                $("#txtFecha").val(moment(resultado.FechaAsignada).format('YYYY-MM-DD'));
                $('#txtFecha').prop('disabled', true);
                $("#txtObservacionVer").val(resultado.ObservacionCtrl);
                CargarDetalle();
                CargarParadas();                
            }
            $('#cargac').hide();
        },
        error: function (result) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

async function GuardarCabecera(siAprobar) {    
    try {
        $('#cargac').show();
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            $('#cargac').hide();
            return;
        } else {
            const data = new FormData();
            data.append('IdAnalisis', itemCabecera.IdAnalisis);
            data.append('Fecha', $("#txtIngresoFecha").val());
            data.append('FechaAsignada', $("#txtFecha").val());
            data.append('Turno', document.getElementById('selectTurnoInsertar').value);   
            data.append('ObservacionCtrl', $("#txtIngresoObservacion").val());
            data.append('siAprobar', siAprobar);   
            
            var promiseCall = fetch('../LaboratorioAnalisisQuimico/GuardarModificarAnalisisQuimico', {
                method: 'post',
                body: data
            });
            var objectPromise = await promiseCall;
            if (!objectPromise.ok) {
                throw 'Error';
            }
            var jsonResult =await objectPromise.json();
            if (jsonResult == "101") {
                window.location.reload();
            }
            if (jsonResult == 0) {
                MensajeCorrecto('Registro guardado correctamente'); 
                $('#txtFecha').prop('disabled', true);
                document.getElementById("txtFechaProduccion").value = moment($("#txtIngresoFecha").val()).format('YYYY-MM-DD');
                $('#selectTurno').val(document.getElementById('selectTurnoInsertar').value).trigger('change');                
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
                document.getElementById("txtFechaProduccion").value = moment($("#txtIngresoFecha").val()).format('YYYY-MM-DD');
                $('#selectTurno').val(document.getElementById('selectTurnoInsertar').value).trigger('change'); 
            } else if (jsonResult == 3) {
                MensajeAdvertencia('Error al ingresar la FECHA  : <span class="badge badge-danger">' + moment($("#txtIngresoFecha").val()).format('DD-MM-YYYY') + '</span>');
                $('#cargac').hide();
                return;
            } else if (jsonResult == 4) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                return;
            } else if (jsonResult == 5) {
                var t = document.getElementById("selectTurnoInsertar");
                var selectedText = t.options[t.selectedIndex].text;
                MensajeAdvertencia('Error, ya existe una FECHA  : <span class="badge badge-danger">' + moment($("#txtIngresoFecha").val()).format('DD-MM-YYYY') + '</span>, TURNO: <span class="badge badge-danger">' + selectedText +'</span>');
                $('#cargac').hide();
                return;
            }else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            $('#ModalIngresoCabecera').modal('hide');
            $('#divBotonesCRUD').prop('hidden', false);
            $('#divMostarTablaDetalle').prop('hidden', false);
            $('#divBotonCrear').prop('hidden', true);
            itemCabecera = [];
            $('#cargac').hide();
            CargarCabecera();
        }
    } catch (e) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

async function EliminarConfirmar() {
    try {
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte == "101") {
            window.location.reload();
        }
        if (estadoReporte.EstadoReporte ==true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    } catch (e) {
        MensajeError(Mensajes.Error,false);
    }
}

async function EliminarCabeceraSi() {
    try {
        $('#cargac').show();
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte == "101") {
            window.location.reload();
        }
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            const data = new FormData();
            data.append('IdAnalisis', itemCabecera.IdAnalisis);
            data.append('Fecha', moment(itemCabecera.Fecha).format('YYYY-MM-DD'));
            var promisess = fetch('../LaboratorioAnalisisQuimico/EliminarAnalisisQuimico', {
                method: 'post',
                body: data
            });
            var objectPromise = await promisess;
            if (!objectPromise.ok) {
                throw 'Error';
            }
            var jsonResult = await objectPromise.json();
            if (jsonResult == "0") {
                MensajeAdvertencia("Falta Parametro IdAnalisis");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (jsonResult == 1) {                           
                MensajeCorrecto("Registro eliminado con Éxito");   
                $('#txtFecha').prop('disabled', false);
            } else if (jsonResult == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            } else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            itemCabecera = [];
            CargarCabecera();
            $('#cargac').hide();
            $("#modalEliminarControl").modal("hide");
        }
    } catch (ex) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error,false);
    }
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

async function ActualizarCabecera() {
    try {
        var objectPromise = await ConsultarEstadoRegistro();
        if (!objectPromise.ok) {
            throw 'Error';
        }
        var jsonResult = objectPromise.json();
        if (jsonResult.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            LimpiarModalIngresoCabecera();
            $("#txtIngresoFecha").val(moment(itemCabecera.Fecha).format("YYYY-MM-DD"));
            $("#txtIngresoObservacion").val(itemCabecera.ObservacionCtrl);
            $('#ModalIngresoCabecera').modal('show');
            $('#selectTurnoInsertar').val(itemCabecera.Turno).trigger('change');
        }
    } catch (e) {
        MensajeError(Mensajes.Error,false);
    }
}

function ModalIngresoCabecera() {
    LimpiarModalIngresoCabecera();
    $('#ModalIngresoCabecera').modal('show');
    $('#selectTurnoInsertar').val(document.getElementById('selectTurno').value).trigger('change');  
    itemCabecera = [];
}

function LimpiarModalIngresoCabecera() {
    $('#txtIngresoFecha').val(moment($('#txtFechaProduccion').val()).format('YYYY-MM-DD'));
    document.getElementById("selectTurnoInsertar").options[0].selected = true;
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
async function CargarParadas() {
    try {
        
            $('#cargac').show();
            let params = {
                fechaProduccion: $('#txtFechaProduccion').val(),
                fechaParadaCocina: document.getElementById('txtFecha').value,
                turno: document.getElementById('selectTurno').value
            }
            let query = Object.keys(params)
                .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
                .join('&');
            let url = '../LaboratorioAnalisisQuimico/ControlAnalisisQuimicoPartial?' + query;
            var promiseCall = fetch(url);
            var objectPromise = await promiseCall;
            if (!objectPromise.ok) {
                throw 'Error';
            }
            var jsonResult = await objectPromise.text();
            if (jsonResult == "101") {
                window.location.reload();
            }
            if (jsonResult == "0") {
                $("#divMostarTablaDetallesVer").html("No existen registros");
                $('#divBotonCrearDetalle').prop('hidden', false);
                $('#selectVerificacion').prop('disabled', false);
            } else {
                $('#divMostarTablaDetallesVer').prop('hidden', false);
                $('#divMostarTablaDetallesVer').html(jsonResult);
                //ConsultarElemento();
            }
            $('#cargac').hide();
        
    } catch (e) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error,false);
    }
}

async function CargarDetalle() {
    try {
        $('#cargac').show();
        let params = {
            idAnalisis: itemCabecera.IdAnalisis
        }
        let query = Object.keys(params)
            .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
            .join('&');
        let url = '../LaboratorioAnalisisQuimico/ConsultarDetalle?' + query;
        var promiseCall = fetch(url);
        var objectPromise = await promiseCall;
        if (!objectPromise.ok) {
            throw 'Error';
        }
        var jsonResult = await objectPromise.json();
        if (jsonResult == "101") {
            window.location.reload();
        }
        if (jsonResult == "0") {
            itemDetalle = null;           
        } else {
            itemDetalle = jsonResult;
        }
        $('#cargac').hide();
    } catch (e) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

async function ConsultarElemento() {
    try {
        showTextBox();
        const data = new FormData();
        var idAnalisisDetalle = 0;
        if (itemDetalle != null) {
            if (!siActualizar) {
                itemDetalle.forEach(function (row) {
                    if (row.Cocinador == document.getElementById('lblCocinador').value && row.Parada == document.getElementById('lblParada').value) {
                        idAnalisisDetalle = row.IdAnalisisDetalle;
                    }
                });
            } else {
                idAnalisisDetalle = itemEditar.IdAnalisisDetalle;
                siActualizar = false;
            }
        }
        data.append('idAnalisis', itemCabecera.IdAnalisis);
        data.append('idParametro', document.getElementById('selectParametros').value);
        data.append('idAnalisisDetalle', idAnalisisDetalle);
        var promiseCall = fetch("../LaboratorioAnalisisQuimico/ControlElementoPartial", {
            method: 'POST',
            body: data
        });
        var objectPromise = await promiseCall;
        if (!objectPromise.ok) {
            throw 'Error';
        }
        var jsonResult = await objectPromise.text();
        if (jsonResult == "101") {
            window.location.reload();
        }
        if (jsonResult == "0") {
            $('#divElementos').html('<span class="badge">SIN DATOS</span>');

        } else {
            $('#divElementos').html(jsonResult);
        }
        CargarImagen();
        $('#cargac').hide();
    } catch (e) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }   
}

function mask() {   
    if (inputMask == null) {
        inputMask = 9999, 99;
    } else {       
        inputMask.forEach(function (row) {
            var max = 0;
            var min = 0;
            if (row.Mascara<0) {
                max = row.Mascara * -1;
                min = row.Mascara;
            } else {
                max = row.Mascara;
            }
            $('#txtValor_' + row.IdParametro).inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': +max,'min':min });
            $("#txtValor_" + row.IdParametro).css('border', '');
            $("#txtValor_" + row.IdParametro).css('background-color', 'white');
            document.getElementById('txtValor_' + row.IdParametro).value = '';
        });
    }    
}

function showTextBox() {
    inputMask.forEach(function (row) {
        if (document.getElementById('selectParametros').value == row.IdParametro) {
            $('#txtValor_' + row.IdParametro).prop('hidden', false);
        } else { $('#txtValor_' + row.IdParametro).prop('hidden', true);}
    });
}

async function ConfirmarIngreso() {
    //$('#ModalIngresoSubDetalle').modal('hide');    
    $('#modalConfirmarEdicion').modal('hide')
    $('#ModalIngresoSubDetalle').modal('show');
}

async function ModalIngresoSubDetalle(jdata, cocina, parada, turno) {
    LimpiarDatosImagen();    
    mask();
    showTextBox();
    siActualizar = false;
    LimpiarDetalle();    
    try {
        if (itemCabecera.length != 0) {
            var fProduccion = document.getElementById('txtFechaProduccion').value;
            var fAsignada = document.getElementById('txtFecha').value;
            if (fProduccion != fAsignada) {
                $('#confirmarIngreso').html('Esta a punto de ingresar en un detalle con fecha diferente:</br>Fecha  Asignada:<span class="badge badge-danger">' + moment(fAsignada).format('DD-MM-YYYY') + '</span> </br>Fecha Producción:<span class="badge badge-success"> ' + moment(fProduccion).format('DD-MM-YYYY'));
                $('#modalConfirmarEdicion').modal('show')
                $('#ModalIngresoSubDetalle').modal('hide');
            } else { $('#ModalIngresoSubDetalle').modal('show');}
                if (turno == document.getElementById('selectTurno').value || turno == null) {
                    var estadoReporteAwait = await ConsultarEstadoRegistro();
                    if (!estadoReporteAwait.ok) {
                        throw 'Error';
                    }
                    var estadoReporte = await estadoReporteAwait.json();
                    CambiarMensajeEstado(estadoReporte.EstadoReporte);
                    if (estadoReporte.EstadoReporte == true) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        $('#cargac').hide();
                        return;
                    } else {
                        CargarDetalle();
                        document.getElementById('lblCocinador').innerText = 'COCINA: ' + cocina;
                        document.getElementById('lblParada').innerText = 'PARADA: ' + parada;
                        document.getElementById('lblCocinador').value = cocina;
                        document.getElementById('lblParada').value = parada;
                        //document.getElementById('lblOrdenFabricacion').innerText = 'ORDEN FABRICACIÓN: ' + jdata[0].ORDEN;
                        document.getElementById('lblOrdenFabricacion').value = jdata[0].ORDEN;
                        ordenFabricacion = jdata;
                        $('#tblImagenes').html('');
                        jdata.forEach(function (row) {
                            var x = document.getElementById("selectIngresarLote");
                            var option = document.createElement("option");
                            option.text = row.LOTE;
                            option.value = row.LOTE;
                            x.add(option);
                        });
                        
                        ConsultarElemento();
                        $('#cargac').hide();
                    }
                } else
                    MensajeAdvertencia('El registro fue ingresado en otro TURNO');
            //}
            } else
            MensajeAdvertencia('No existe la cabecera del CONTROL');
    } catch (e) {
        MensajeError(Mensajes.Error,false);
    }   
}

function SelectOrdenFabricacion() {
    ordenFabricacion.forEach(function (row) {
        if (document.getElementById('selectIngresarLote').value == row.LOTE) {
            //document.getElementById('lblOrdenFabricacion').innerText = 'ORDEN FABRICACIÓN: ' + row.ORDEN;
            document.getElementById('lblOrdenFabricacion').value = row.ORDEN;
        }
    }); 
}

async function GuardarDetalle() {
    try {
        LimpiarDetalle();
        if (OnChangeTextBoxDetalle()==1) {
            MensajeAdvertencia('Por favor ingrese todos los datos requeridos');
            return;
        }
        $('#cargac').show();
        const estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            var data = new FormData();
            data.append('IdAnalisisDetalle', itemDetalle.IdAnalisisDetalle);
            data.append('IdAnalisis', itemCabecera.IdAnalisis);
            data.append('Cocinador', document.getElementById('lblCocinador').value);
            data.append('Parada', document.getElementById('lblParada').value);
            data.append('ObservacionDet', document.getElementById('txtObservacionesDet').value);
            var promiseCall = fetch('../LaboratorioAnalisisQuimico/GuardarModificarDetalle', {
                method: 'post',
                body: data
            });
            var objectPromise = await promiseCall;
            if (!objectPromise.ok) {
                throw 'Error';
            }
            var jsonResult = await objectPromise;
            if (jsonResult == "101") {
                window.location.reload();
            }
            if (jsonResult == 0) {
                MensajeCorrecto('Registro guardado correctamente');
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (jsonResult == 2) {
                MensajeAdvertencia('¡Error! No se a guardado  : <span class="badge badge-danger">' + 'SIN DATOS' + '</span>');
                $('#cargac').hide();
                $('#ModalIngresoDetalle').modal('hide');
                return;
            } else if (jsonResult == 3) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                $('#cargac').hide();
                return;
            }
            $('#selectTurnoFiltro').val($('#selectTurno').val());
            $('#selectAreaAuditarFiltro').val($('#selectAreaAuditar').val()).trigger('change');
            $('#cargac').hide();
        }
    } catch (ex) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error,false);
    }
}

async function GuardarElemento(){
    try {
        if (OnChangeTextBoxDetalle() == 1) {
            MensajeAdvertencia('Por favor ingrese todos los datos requeridos');
            return;
        }
        $('#cargac').show();
        const estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            var valor = 0;
            inputMask.forEach(function (row) {
                if (document.getElementById('selectParametros').value == row.IdParametro) {
                    valor=document.getElementById('txtValor_'+row.IdParametro).value;
                }
            });
            var idAnalisisDetalle = 0;
            var idElemento = 0;
            if (itemDetalle != null) {
                if (!siActualizar) {
                    itemDetalle.forEach(function (row) {
                        if (row.Cocinador == document.getElementById('lblCocinador').value && row.Parada == document.getElementById('lblParada').value) {
                            idAnalisisDetalle = row.IdAnalisisDetalle;
                        }
                    });
                } else {
                    idAnalisisDetalle = itemEditar.IdAnalisisDetalle;
                    idElemento = itemEditar.IdElemento;                    
                }
            }
            var data = new FormData();
            data.append('idAnalisis', itemCabecera.IdAnalisis);
            data.append('IdAnalisisDetalle', idAnalisisDetalle);
            data.append('IdElemento', idElemento);
            data.append('IdParametro', document.getElementById('selectParametros').value);
            data.append('Valor', valor);
            data.append('OrdenFabricacion', document.getElementById('lblOrdenFabricacion').value);
            data.append('LoteBarco', document.getElementById('selectIngresarLote').value);
            data.append('Cocinador', document.getElementById('lblCocinador').value);
            data.append('Parada', document.getElementById('lblParada').value);
            var promiseCall = fetch('../LaboratorioAnalisisQuimico/GuardarModificarElemento', {
                method: 'post',
                body: data
            });
            var objectPromise = await promiseCall;
            if (!objectPromise.ok) {
                throw 'Error';
            }
            var jsonResult = await objectPromise.json();
            if (jsonResult == "101") {
                window.location.reload();
            }
            if (jsonResult == 0) {
                MensajeCorrecto('Registro guardado correctamente');
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (jsonResult == 2) {
                MensajeAdvertencia('¡Error! No se a guardado  : <span class="badge badge-danger">' + 'SIN DATOS' + '</span>');
                $('#cargac').hide();
                $('#ModalIngresoSubDetalle').modal('hide');
                return;
            } else if (jsonResult == 3) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                $('#cargac').hide();
                return;
            } else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                $('#ModalIngresoSubDetalle').modal('hide');
                $('#cargac').hide();
                return;
            }            
            mask();
            var callCargarDetalle= await CargarDetalle();
            ConsultarElemento();
            $('#cargac').hide();
        }
    } catch (ex) {
        //console.log(ex);
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

function OnChangeTextBoxDetalle() {
    var con = 0;
    if ($('#txtCocinador').val() == '') {
        $("#txtCocinador").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtCocinador").css('border', ''); }
    if ($('#txtParada').val() == '') {
        $("#txtParada").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtParada").css('border', ''); }
    return con;
}

async function ActualizarDetalle(jdata) { 
    $('div,html').animate({ scrollTop: 0 }, 500);
    inputMask.forEach(function (row) {
        $("#txtValor_"+row.IdParametro).css('border', '1px dashed green');
        $("#txtValor_"+row.IdParametro).css('background-color', 'lightgrey');
        document.getElementById('txtValor_' + row.IdParametro).value = jdata.Valor;
    });
    //document.getElementById('txtValor').value = jdata.Valor; 
    $('#selectIngresarLote').val(jdata.LoteBarco).trigger('change');
    itemEditar = jdata;
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

async function EliminarConfirmarDetalle(jdata) {
    try {
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte == "101") {
            window.location.reload();
        }
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#ModalIngresoSubDetalle").modal("hide");
            $("#modalEliminarControlDetalle").modal("show");
            $("#myModalLabelDetalle").text("¿Desea Eliminar el registro?");
            itemEditar = jdata;
        }
    } catch (e) {
        MensajeError(Mensajes.Error, false);
    }
}

function EliminarDetalleSi() {    
    $.ajax({
        url: "../LaboratorioAnalisisQuimico/EliminarElemento",
        type: "POST",
        data: {
            idAnalisis: itemCabecera.IdAnalisis,
            IdElemento: itemEditar.IdElemento
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdElemento");
                $("#modalEliminarControlDetalle").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControlDetalle").modal("hide");
                $("#ModalIngresoSubDetalle").modal("show");
                ConsultarElemento();
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
                $('#modalEliminarControlDetalle').modal('hide');
                return;
            }
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function LimpiarDetalle() {
    CargarParadas();
    LimpiarDatosImagen();
    ordenFabricacion = [];
    mask();      
    document.getElementById('selectIngresarLote').innerHTML = '';
    $('#divElementos').html('');
}

function NewElement() {
    document.getElementById('txtValor').value = '';
    siActualizar = false;
    CargarDetalle();
    $("#txtValor").css('border', '');
    $("#txtValor").css('background-color', 'white');
}

function EliminarDetalleNo() {    
    $("#modalEliminarControlDetalle").modal("hide");
    $("#ModalIngresoSubDetalle").modal("show");    
}

async function GuardarImagen() {
    try {
        if (OnChangeTextBoxDetalle() == 1) {
            MensajeAdvertencia('Por favor ingrese todos los datos requeridos');
            return;
        }
        $('#cargac').show();
        const estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            if (OnChangeTextBoxAccion() == 1) {
                MensajeAdvertencia('Ingrese todos los datos requeridos');
                $('#cargac').hide();
                return;
            }
            $('#cargac').show();
            var idAnalisisDetalle = 0;
            if (itemDetalle != null) {
                if (!actulizarFoto) {
                    itemDetalle.forEach(function (row) {
                        if (row.Cocinador == document.getElementById('lblCocinador').value && row.Parada == document.getElementById('lblParada').value) {
                            idAnalisisDetalle = row.IdAnalisisDetalle;
                        }
                    });
                } else {
                    idAnalisisDetalle = itemImagen.IdAnalisisDetalle;
                }
            }
            var imagen = $('#file-upload')[0].files[0];
            var data = new FormData();
            data.append("dataImg", imagen);
            data.append('IdAnalisis', itemCabecera.IdAnalisis);
            data.append("IdAnalisisDetalle", idAnalisisDetalle);
            data.append('Cocinador', document.getElementById('lblCocinador').value);
            data.append('Parada', document.getElementById('lblParada').value);
            data.append("IdFoto", itemImagen.IdFoto);
            data.append("ObservacionFoto", document.getElementById('txtObservacionFoto').value);
            data.append("Rotation", rotation);
            var promiseCall = fetch('../LaboratorioAnalisisQuimico/GuardarFoto', {
                method: 'post',
                body: data
            });
            var objectPromise = await promiseCall;
            if (!objectPromise.ok) {
                throw 'Error';
            }
            var jsonResult = await objectPromise.json();
            if (jsonResult == "101") {
                window.location.reload();
            }
            if (jsonResult == 0) {
                MensajeCorrecto('Imagen guardada correctamente');
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (jsonResult == 3) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                $('#cargac').hide();
                return;
            } else if (jsonResult == 4) {
                MensajeAdvertencia('¡Solo se permiten imagenes!', 5);
                $('#cargac').hide();
                return;
            } else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                $('#ModalIngresoSubDetalle').modal('hide');
                $('#cargac').hide();
                return;
            } else {
                var mb = parseFloat(resultado / (1024 * 1024)).toFixed(2);
                MensajeAdvertencia('¡Exedio el limite de capacidad permitido!:  <span class="badge badge-success">5Mb</span>: Su imagen:<span class="badge badge-danger">' + mb + 'Mb</span>');
                $('#cargac').hide();
                return;
            }
            var callCargarDetalle = await CargarDetalle();
            CargarImagen();
            LimpiarDatosImagen();
            $('#cargac').hide();
            itemImagen = [];
        }
    } catch (ex) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

function OnChangeTextBoxAccion() {
    var con = 0;
    if ($('#txtObservacionFoto').val() == '') {
        $("#txtObservacionFoto").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtObservacionFoto").css('border', ''); }
    if (!actulizarFoto) {
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

function LimpiarDatosImagen() {
    $("#txtObservacionFoto").val("");
    $("#file-upload").val('');
    $("#file-preview-zone").html('');
    $('#lblfoto').text('Seleccione archivo');
    $("#lblfoto").css('border', '');
    $("#file-upload").css('border', '');
    $("#txtObservacionFoto").css('border', '');
    rotation = 0;
    actulizarFoto = false;
}

function CargarImagen() {
    var idAnalisisDetalle = 0;
    if (itemDetalle != null) {
        if (!actulizarFoto) {
            itemDetalle.forEach(function (row) {
                if (row.Cocinador == document.getElementById('lblCocinador').value && row.Parada == document.getElementById('lblParada').value) {
                    idAnalisisDetalle = row.IdAnalisisDetalle;
                }
            });
        } else {
            idAnalisisDetalle = itemImagen.IdAnalisisDetalle;
        }
    }
    $.ajax({
        url: "../LaboratorioAnalisisQuimico/VerCrearImagenPartial",
        data: {
            idAnalisisDetalle: idAnalisisDetalle
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == 0) {
                $('#divListarImagen').html('<span class="badge">SIN DATOS</span>');
            } else 
                $('#divListarImagen').html(resultado);},
        error: function (resultado) {
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
                document.getElementById("file-preview").style.height = "250px";
                document.getElementById("file-preview").style.width = "250px";
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

function EditarImagen(jdata) {  
    $('div,html').animate({ scrollTop: 0 }, 500);  
    LimpiarDatosImagen();    
    actulizarFoto = true;
    document.getElementById('txtObservacionFoto').value = jdata.ObservacionFoto;
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
        itemImagen = jdata;
    }
}

function EliminarImagenSi() {
    $.ajax({
        url: "../LaboratorioAnalisisQuimico/EliminarImagen",
        type: "POST",
        data: {
            IdFoto: itemImagen.IdFoto,
            idAnalisis: itemCabecera.IdAnalisis
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdFoto");
                $("#modalEliminarImagen").modal("hide");
                $("#ModalIngresoSubDetalle").modal("show");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                CargarImagen();
                $("#modalEliminarImagen").modal("hide");
                MensajeCorrecto("Registro eliminado con Éxito");
            } else if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            }
            $("#ModalIngresoSubDetalle").modal("show");
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function EliminarImagenNo() {
    $("#modalEliminarImagen").modal("hide");
    $("#ModalIngresoSubDetalle").modal("show");
}

async function EliminarImagenConfirmar(jdata) {
    try {
        if (OnChangeTextBoxDetalle() == 1) {
            MensajeAdvertencia('Por favor ingrese todos los datos requeridos');
            return;
        }
        const estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            $("#ModalIngresoSubDetalle").modal("hide");
            $("#modalEliminarImagen").modal("show");
            $("#accionCorrectiva").text("¿Desea Eliminar la Imagen?");
            itemImagen = jdata;
        }
    } catch (ex) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}