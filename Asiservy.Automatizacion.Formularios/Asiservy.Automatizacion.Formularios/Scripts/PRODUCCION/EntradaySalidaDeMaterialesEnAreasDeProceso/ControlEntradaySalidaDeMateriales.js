var IdCabecera = 0;
var IdDetalle = 0;
var IdSubDetalle = 0;
var Error = 0;
var DatosDetalle;
$(document).ready(function () {
    $('#cmbTurno').prop('selectedIndex', 1);
    $('#txtHora').val(moment().format("YYYY-MM-DDTHH:mm"));
    $('#txtIngreso').inputmask({
        alias: "integer",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
       
        max: 999999,
        rightAlign: false
    });
    $('#txtSalida').inputmask({
        alias: "integer",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
     
        max: 999999,
        rightAlign: false
    });
    ConsultarCabControl();
});

var modal_lv = 0;
$('.modal').on('shown.bs.modal', function (e) {
    $('.modal-backdrop:last').css('zIndex', 1051 + modal_lv);
    $(e.currentTarget).css('zIndex', 1052 + modal_lv);
    modal_lv++
});

$('.modal').on('hidden.bs.modal', function (e) {
    modal_lv--
});

function AbrirModalDetalle() {
    //$('#ModalSubDetalle').modal({ backdrop: 'static', keyboard: false, show: true });
    IdDetalle = 0;
    $('#ModalDetalle').modal({ backdrop: 'static', keyboard: false, show: true });
    LimpiarControlesDetalle();

    $('#btnOrden').prop('disabled', false);
    $('#cmbLote').prop('disabled', false);
}
function AbrirModalSubDetalle() {
    $('#ModalSubDetalle').modal({ backdrop: 'static', keyboard: false, show: true });
    IdSubDetalle = 0;
    LimpiarControlesSubDetalle();
}

async function ConsultarCabControlAjax() {


    const data = new FormData();
    data.append('Fecha', $("#txtFechaProduccion").val());
    data.append('Turno', $("#cmbTurno").val());
    var promesa = fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/ConsultarCabeceraControl", {
        method: 'POST',
        body: data
    })
    return promesa;
}
async function ConsultarCabControl(bandera) {

    try {


        if (!ValidarCabecera()) {
            return;
        }
        //if ($('#cmbTurno').prop('selectedIndex') == 0) {

        //    $('#msjTurno').prop('hidden', false);
        //    return;
        //} else {
        //    $('#msjTurno').prop('hidden', true);
        //}

        //if (bandera != 'of')//bandera para que solo se ejecute si se llama desde onchange de fecha, y no por onchange de orden de fabricacion
        //{
        //    await LLenarComboOrdenes();
        //}
        $('#btnCargando').prop('hidden', false);
        $('#btnConsultar').prop('hidden', true);
        $('#btnLimpiar').prop('hidden', true);
        $('#btnEliminarCabeceraControl').prop('hidden', true);
        $('#btnGuardar').prop('hidden', true);
        var PromesaConsultar = await ConsultarCabControlAjax();
        if (!PromesaConsultar.ok) {
            throw "Error";
        }
        var ResultadoConsulta = await PromesaConsultar.json();
        if (ResultadoConsulta != "0") {

            $('#mensajeRegistros').prop('hidden', true);
    
            IdCabecera = ResultadoConsulta.IdControlEntradaSalidaMateriales;

            if (ResultadoConsulta.EstadoControl == true) {
                $("#estadocontrol").removeClass("badge-danger").addClass("badge-success");
                $('#estadocontrol').text(Mensajes.Aprobado);
            }
            else {
                $("#estadocontrol").removeClass("badge-success").addClass("badge-danger");
                $('#estadocontrol').text(Mensajes.Pendiente);
            }

            $('#Observacion').val(ResultadoConsulta.Observacion);
            $('#CardDetalle').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            $('#brespacio').remove();
            //ValidarParametro();
            //LlenarComboLotes(ResultadoConsulta.OrdenFabricacion);
            ConsultarDetalleControl();


            //SlideCabecera();
        } else {
            $('#brespacio').remove();
            $('#DivCabecera').after('<br id="brespacio">');
            $('#mensajeRegistros').prop('hidden', false);
            $('#mensajeRegistros').text(Mensajes.SinRegistros);

            LimpiarControles();

            $('#CardDetalle').prop('hidden', true);

        }
        //DatosOrdenFabricacion();

    } catch (ex) {
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }
    $('#btnCargando').prop('hidden', true);
    $('#btnConsultar').prop('hidden', false);
    $('#btnLimpiar').prop('hidden', false);
    $('#btnEliminarCabeceraControl').prop('hidden', false);
    $('#btnGuardar').prop('hidden', false);

}
function GuardarCabceraControl() {
    if (!ValidarCabecera()) {
        return;
    } else {
        $('#btnCargando').prop('hidden', false);
        $('#btnConsultar').prop('hidden', true);
        $('#btnLimpiar').prop('hidden', true);
        $('#btnEliminarCabeceraControl').prop('hidden', true);
        $('#btnGuardar').prop('hidden', true);
        const data = new FormData();
        data.append('IdControlEntradaSalidaMateriales', IdCabecera);
        data.append('Fecha', $("#txtFechaProduccion").val());
        data.append('Turno', $("#cmbTurno").val());
        data.append('Observacion', $('#Observacion').val());
        data.append('Linea', $('#Linea').val());
        fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/GuardarCabeceraControl", {
            method: 'POST',
            body: data
        }).then(function (respuesta) {
            if (!respuesta.ok) {
                //MensajeError(respuesta.statusText);
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.json();
        }).then(function (resultado) {
            //console.log(respuesta);
            if (resultado == "101") {
                window.location.reload();
            }
            if (Error == 0) {
                IdCabecera = resultado[2].IdAnalisisQuimicoProductoSe;
                if (resultado[0] == "002" || resultado[0] == "003" || resultado[0] == "444") {
                    MensajeAdvertencia(resultado[1]);

                    $('#Observacion').val(resultado[2].Observacion);

                } else {
                    MensajeCorrecto(resultado[1]);
                    //$('#brespacio').remove();
                    $('#mensajeRegistros').text('');
                    $('#DivDetalle').empty();
                    $('#CardDetalle').prop('hidden', false);
                    $('#brespacio').remove();
                    //SlideCabecera();
                    $('#CardDetalle').prop('hidden', false);
                }
                
            }
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            //LlenarComboLotes($('#cmbOrdeneFabricacion').val());
        })
            .catch(function (resultado) {
                //console.log('error');
                //console.log(resultado);
                MensajeError(resultado.responseText, false);

            })
    }
    
}
function LimpiarControles() {
    //console.log($("#cmbOrdeneFabricacion").val());
    IdCabecera = 0;
    Error = 0;
    IdDetalle = 0;
    $('#estadocontrol').text('');
    $('#Observacion').val('');
    $('#DivDetalles').empty();
    $('#btnEliminarCabeceraControl').prop('disabled', true);
}
function ConfirmarEliminarCab() {
    $('#ModalEliminarControl').modal('show');
}
function EliminarCabecera() {
    $('#btnsicab').prop('disabled', true);
    $('#btnnocab').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdCabecera', IdCabecera);
    data.append('Fechaco', $("#txtFechaProduccion").val());

    fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/EliminarCabeceraControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalEliminarControl').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {

            if (resultado[0] != '002') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);

                LimpiarControles();
                $('#DivDetalle').empty();
                ConsultarCabControl();
            }
            //}

        }
        $('#btnsicab').prop('disabled', false);
        $('#btnnocab').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnsicab').prop('disabled', false);
            $('#btnnocab').prop('disabled', false);
        })
}
function GuardarDetalleControl() {
    Error = 0;
    if (!ValidarDetalle()) {
        return;
    } else {
        $('#cargac').show();
        const data = new FormData();
        data.append('IdDetalleEntradaSalidaMateriales', IdDetalle);
        data.append('Material', $("#cmbMaterial").val());
        data.append('Ingreso', $("#txtIngreso").val());
        data.append('IdCabeceraEntradaSalidaMaterial', IdCabecera);
        data.append('Fechaco', $("#txtFechaProduccion").val());
        fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/GuardarDetalleControl", {
            method: 'POST',
            body: data
        }).then(function (respuesta) {
            if (!respuesta.ok) {
                //MensajeError(respuesta.statusText);
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.json();
        }).then(function (resultado) {
            //console.log(respuesta);
            if (resultado == "101") {
                window.location.reload();
            }
            if (Error == 0) {

                if (resultado[0] == "002" || resultado[0] == "003" || resultado[0] == "444") {
                    MensajeAdvertencia(resultado[1]);
                }
                if (resultado[0] == "000" || resultado[0] == "001") {
                    MensajeCorrecto(resultado[1]);

                    ConsultarDetalleControl();
                }
                $('#ModalDetalle').modal('hide');
                //LimpiarDetalleControles();
            }
            $('#cargac').hide();
        })
            .catch(function (resultado) {
                //console.log('error');
                //console.log(resultado);
                MensajeError(resultado.responseText, false);
                $('#cargac').hide();
            })
    }

}
function ConsultarDetalleControl() {
    $('#cargac').show();
    Error = 0;
    let params = {
        IdCabeceraControl: IdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EntradaySalidaDeMaterialesEnAreasDeProceso/PartialDetalleControl?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#DivDetalle').empty();
                $('#DivDetalle').html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#TableDetalle').DataTable(config.opcionesDT);
                //$('#brespacio').remove();
                //LimpiarDetalleControles();


            } else {
                $('#DivDetalle').empty();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#cargac').hide();
        })

}
function LimpiarControlesDetalle() {
    $('#cmbMaterial').prop('disabled', false);
    $('#cmbMaterial').prop('selectedIndex',0);
    $('#txtIngreso').val('');
    IdDetalle = 0;
    $('#msjerrorIngreso').prop('hidden', true);
    $('#msjerrorMaterial').prop('hidden', true);
    $('#txtIngreso').css('borderColor', '#ced4da');
    $('#cmbMaterial').css('borderColor', '#ced4da');
    
}
function VerSubDetalle(data) {
    //console.log(data);
    $('#CardDetalle').prop('hidden', true);
    $('#CardSubDetalle').prop('hidden', false);
    $('#DivCabecera').prop('hidden', true);
    $('#botonessubdetalle').prop('hidden', false);
    DatosDetalle = data;
    IdDetalle = data.IdDetalleEntradaSalidaMateriales;
    ConsultarSubDetalleControl();
}
function Atras() {
    $('#CardDetalle').prop('hidden', false);
    $('#CardSubDetalle').prop('hidden', true);
    $('#DivCabecera').prop('hidden', false);
    $('#botonessubdetalle').prop('hidden', true);
    IdDetalle = 0;
}
function GuardarSubDetalleControl() {
    Error = 0;
    if (!ValidarSubDetallle()) {
        return;
    } else {
        $('#cargac').show();
        const data = new FormData();
        data.append('IdSubDetalleEntradaSalidaMaterial', IdSubDetalle);
        data.append('Hora', $("#txtHora").val());
        data.append('Salida', $("#txtSalida").val());
        data.append('CabeceraControl', IdCabecera);
        data.append('IdDetalleEntradaSalidaMaterial', IdDetalle);
        data.append('Fechaco', $("#txtFechaProduccion").val());
        fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/GuardarSubDetalleControl", {
            method: 'POST',
            body: data
        }).then(function (respuesta) {
            if (!respuesta.ok) {
                //MensajeError(respuesta.statusText);
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.json();
        }).then(function (resultado) {
            //console.log(respuesta);
            if (resultado == "101") {
                window.location.reload();
            }
            if (Error == 0) {

                if (resultado[0] == "002" || resultado[0] == "003" || resultado[0] == "444") {
                    MensajeAdvertencia(resultado[1]);
                }
                if (resultado[0] == "000" || resultado[0] == "001") {
                    MensajeCorrecto(resultado[1]);

                    ConsultarSubDetalleControl();
                }
                $('#ModalSubDetalle').modal('hide');
                //LimpiarDetalleControles();
            }
            $('#cargac').hide();
        })
            .catch(function (resultado) {
                //console.log('error');
                //console.log(resultado);
                MensajeError(resultado.responseText, false);
                $('#cargac').hide();
            })
    }

}
function ConsultarSubDetalleControl() {
    $('#cargac').show();
    Error = 0;
    let params = {
        IdDetalleControl: IdDetalle
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EntradaySalidaDeMaterialesEnAreasDeProceso/PartialSubDetalleControl?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#DivSubDetalle').empty();
                $('#DivSubDetalle').html(resultado);
                //config.opcionesDT.pageLength = 10;
                //$('#TableSubDetalle').DataTable(config.opcionesDT);

                //$('#brespacio').remove();
                //LimpiarDetalleControles();


            } else {
                $('#DivSubDetalle').empty();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
            $('#cargac').hide();
        })

}
function EditarDetalle() {
    $('#ModalDetalle').modal('show');
    IdDetalle = DatosDetalle.IdDetalleEntradaSalidaMateriales;
    $('#cmbMaterial').val(DatosDetalle.Material);
    $('#txtIngreso').val(DatosDetalle.Ingreso);
    $('#cmbMaterial').prop('disabled', true);
   
}
function EditarSubDetalle(data) {
    $('#ModalSubDetalle').modal('show');
    $('#txtHora').prop('disabled',true);
    $('#txtHora').val(data.Hora);
    $('#txtSalida').val(data.Salida);

    IdSubDetalle = data.IdSubDetalleEntradaSalidaMaterial;
}
function LimpiarControlesSubDetalle() {

    $('#txtHora').prop('disabled', false);
    $('#txtHora').val('');
    $('#txtHora').val(moment().format("YYYY-MM-DDTHH:mm"));
    $('#txtSalida').val('');
    $('#msjerrorHora').prop('hidden', true);
    $('#msjerrorSalida').prop('hidden', true);
    $('#txtHora').css('borderColor', '#ced4da');
    $('#txtSalida').css('borderColor', '#ced4da');
}
function ValidarCabecera() {
    var valida = true;
    if ($('#txtFechaProduccion').val() == '') {
        $('#txtFechaProduccion').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorIngreso').prop('hidden', false);

    } else {
        $('#txtFechaProduccion').css('borderColor', '#ced4da');
        //$('#msjerrorIngreso').prop('hidden', true);
    }
    if ($('#cmbTurno').prop('selectedIndex') == 0) {
        $('#cmbTurno').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorIngreso').prop('hidden', false);

    } else {
        $('#cmbTurno').css('borderColor', '#ced4da');
        //$('#msjerrorIngreso').prop('hidden', true);
    }
    return valida;
}
function ValidarDetalle() {
    var valida = true;
    if ($('#txtIngreso').val() == '') {
        $('#txtIngreso').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorIngreso').prop('hidden', false);

    } else {
        $('#txtIngreso').css('borderColor', '#ced4da');
        //$('#msjerrorIngreso').prop('hidden', true);
    }
    if ($('#cmbMaterial').prop('selectedIndex') == 0) {
        $('#cmbMaterial').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorMaterial').prop('hidden', false);

    } else {
        $('#cmbMaterial').css('borderColor', '#ced4da');
        //$('#msjerrorMaterial').prop('hidden', true);
    }
   
    return valida;

}
function ValidarSubDetallle() {
    var valida = true;
    if ($('#txtHora').val()=='') {
        valida = false;
        $('#txtHora').css('borderColor', '#FA8072');
        //$('#msjerrorHora').prop('hidden', false);

    } else {
        $('#txtHora').css('borderColor', '#ced4da');
        //$('#msjerrorHora').prop('hidden', true);
    }
    if ($('#txtSalida').val() == '') {
        valida = false;
        $('#txtSalida').css('borderColor', '#FA8072');
        //$('#msjerrorSalida').prop('hidden', false);

    } else {
        $('#txtSalida').css('borderColor', '#ced4da');
        //$('#msjerrorSalida').prop('hidden', true);
    }

    return valida;
}
function ConfirmarEliminarSubDetalle(data) {
    $('#ModalEliminarSubDetalle').modal('show');
    IdSubDetalle = data.IdSubDetalleEntradaSalidaMaterial;

}
function EliminarSubDetalle() {
    $('#cargac').show();
    Error = 0;
    const data = new FormData();
    data.append('IdSubdetalle', IdSubDetalle);
    data.append('IdCabecera', IdCabecera);
    data.append('Fechaco', $("#txtFechaProduccion").val());

    fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/EliminarSubDetalleControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalEliminarSubDetalle').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {

            if (resultado[0] != '002') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);

                LimpiarControlesSubDetalle();
                ConsultarSubDetalleControl();
            }
            //}

        }

        $('#cargac').hide();
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#cargac').hide();
        })
}
function ConfirmarEliminarDetalle() {
    //console.log(DatosDetalle);
    $('#ModalEliminarDetalle').modal('show');
    $('#lblMaterial').text(DatosDetalle.MaterialNombre);
    $('#lblIngreso').text(DatosDetalle.Ingreso);
}
function EliminarDetalle() {
    $('#cargac').show();
    Error = 0;
    const data = new FormData();
    data.append('IdDetalle', IdDetalle);
    data.append('IdCabecera', IdCabecera);
    data.append('Fechaco', $("#txtFechaProduccion").val());

    fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/EliminarDetalleControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalEliminarDetalle').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {

            if (resultado[0] != '002') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                $('#CardDetalle').prop('hidden', false);
                $('#CardSubDetalle').prop('hidden', true);
                $('#DivCabecera').prop('hidden', false);
                $('#botonessubdetalle').prop('hidden', true);
                LimpiarControlesDetalle();
                ConsultarDetalleControl();

            }
            //}

        }

        $('#cargac').hide();
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#cargac').hide();
        })
}
function ValidaVacio(input) {
    if (input.value != '') {
        $(input).css('borderColor', '#ced4da');
    }
}