var ListaLotes;
var Error = 0;
var IdCabecera = 0;
var IdDetalle = 0;
function LLenarComboOrdenes(orden) {
    Error = 0;
    //$('#txtProducto').val('');
    //$('#txtTamanoFunda').val('');
    $('#txtCliente').val('');
    //$('#txtPedidoVenta').val('');
    //$('#CodProducto').val('');
    $('#cmbOrdeneFabricacion').empty();
    $('#cmbOrdeneFabricacion').append('<option>Seleccione...</option>');
    let params = {
        Fecha: $("#txtFechaProduccion").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultaSoloOFNivel3?' + query;
    if ($('#txtFechaProduccion').val() == '') {

    } else {
        fetch(url)
            //,body: data
            .then(function (respuesta) {
                if (!respuesta.ok) {
                    //MensajeError(respuesta.statusText);
                    MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                    Error = 1;
                }
                return respuesta.json();
            })
            .then(function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (Error == 0) {
                    //console.log(resultado);
               
                    $.each(resultado, function (key, value) {
                        $('#cmbOrdeneFabricacion').append('<option value=' + value.Orden + '>' + value.Orden + '</option>');
                    });
                    if (orden != null) {
                        $('#cmbOrdeneFabricacion').val(orden);
                    }
                }
            })
            .catch(function (resultado) {
                console.log(resultado);
                MensajeError(resultado.responseText, false);
              
            })
    }

}
function DatosOrdenFabricacion() {
    Error = 0;
    $('#txtCliente').val('');
    //console.log(ListaOrdenesFabricacion);
    //var Datos = Enumerable.From(ListaOrdenesFabricacion)
    //    .Where(function (x) { return x.ORDEN_FABRICACION == $('#cmbOrdeneFabricacion').val() })
    //    .Select(function (x) { return x })
    //    .FirstOrDefault();
    let params = {
        Orden: $("#cmbOrdeneFabricacion").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultarDatosOrdenFabricacion?' + query;
    if ($('#cmbOrdeneFabricacion').prop('selectedIndex') == 0) {

    } else {
        fetch(url)
            //,body: data
            .then(function (respuesta) {
                if (!respuesta.ok) {
                    //MensajeError(respuesta.statusText);
                    MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                    Error = 1;
                }
                return respuesta.json();
            })
            .then(function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (Error == 0) {
                    $('#txtCliente').val(resultado.CLIENTE);
                    //console.log(resultado);
                    //$.each(resultado, function (key, value) {
                    //    $('#cmbLote').append('<option value=' + value.Codigo + '>' + value.Descripcion + '</option>');
                    //});
                    //console.log(resultado);

                }
            })
            .catch(function (resultado) {
                //console.log(resultado);
                MensajeError(resultado.responseText, false);

            })
    }

    //$('#txtCliente').val(Datos.NombreCliente);
    LlenarComboLotes();
}
function ConsultarCabControl() {
    
    if ($('#txtFechaProduccion').val() == '') {
        $('#msjErrorFechaProduccion').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorFechaProduccion').prop('hidden', true);
    }
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    const data = new FormData();
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    fetch("../EvaluacionDeLomoyMigaEnBandeja/ConsultarCabeceraControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
    if (!respuesta.ok) {
        //MensajeError(respuesta.statusText);
        MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
        Error = 1;
    }
    return respuesta.json();
    })
    .then(function (resultado) {
    //console.log(respuesta);
    if (resultado == "101") {
        window.location.reload();
    }
    if (Error == 0) {
        //console.log(resultado);
        $('#txtFechaProduccion').prop('disabled', true);

        LLenarComboOrdenes(resultado.OrdenFabricacion);
        //$("#cmbTurno").val("2");
        if (resultado != "0") {
            $('#mensajeRegistros').prop('hidden', true);
            IdCabecera = resultado.IdEvaluacionDeLomosYMigasEnBandejas;
            $('#cmbOrdeneFabricacion').val(resultado.OrdenFabricacion);
            $('#txtCliente').val(resultado.Cliente);
            if (resultado.Lomo) {
                $('#Lomo').prop('checked', true);
            }
            if (resultado.Miga) {
                $('#Miga').prop('checked', true);
            }
            if (resultado.Empaque) {
                $('#Empaque').prop('checked', true);
            }
            if (resultado.Enlatado) {
                $('#Enlatado').prop('checked', true);
            }
            if (resultado.Pouch) {
                $('#Pouch').prop('checked', true);
            }
            $('#cmbNivelLimpieza').val(resultado.NivelLimpieza);
            $('#Observacion').val(resultado.Observacion);
            $('#CardDetalle').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            LlenarComboLotes(resultado.OrdenFabricacion);
            //ConsultarDetalleControl();
            ConsultarDetalleControl();
        } else {
            $('#mensajeRegistros').prop('hidden', false);
        }
    }
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
        $('#btnLimpiar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('hidden', false);
        $('#btnGuardar').prop('hidden', false);
    })
    .catch(function (resultado) {
        //console.log('error');
        console.log(resultado);
        MensajeError(resultado.responseText, false);

    });
    //LlenarComboLotes();
}
function GuardarCabceraControl() {
    var Lomo = false;
    var Miga = false;
    var Empaque = false;
    var Enlatado = false;
    var Pouch = false;

    if ($('#txtFechaProduccion').val() == '') {
        $('#msjErrorFechaProduccion').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorFechaProduccion').prop('hidden', true);
    }
  
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    const data = new FormData();
    data.append('IdEvaluacionDeLomosYMigasEnBandejas', IdCabecera);
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    data.append('Cliente', $("#txtCliente").val());
    data.append('OrdenFabricacion', $("#cmbOrdeneFabricacion").val());
    data.append('NivelLimpieza', $("#cmbNivelLimpieza").val());
    if ($('#Lomo').is(":checked")) {
        Lomo = true;
    }
    if ($('#Miga').is(":checked")) {
        Miga = true;
    }
    if ($('#Empaque').is(":checked")) {
        Empaque = true;
    }
    if ($('#Enlatado').is(":checked")) {
        Enlatado = true;
    }
    if ($('#Pouch').is(":checked")) {
        Pouch = true;
    }
    data.append('Lomo', Lomo);
    data.append('Miga', Miga);
    data.append('Empaque', Empaque);
    data.append('Enlatado', Enlatado);
    data.append('Pouch', Pouch);
    data.append('Observacion', $('#Observacion').val());
    fetch("../EvaluacionDeLomoyMigaEnBandeja/GuardarCabeceraControl", {
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
            IdCabecera = resultado[2].IdCabProdPouchCuarentena;
            if (resultado[0] == "002") {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                $('#CardDetalle').prop('hidden', false);
            }

        }
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
        $('#btnLimpiar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('hidden', false);
        $('#btnGuardar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function LimpiarControles() {

    IdCabecera = 0;
    Error = 0;
    IdDetalle = 0;
    $('#txtFechaProduccion').val('');
    $('#cmbOrdeneFabricacion').empty();
    $('#cmbOrdeneFabricacion').append('<option>Seleccione...</option>');
    $('#txtCliente').val('');
    $('#txtFechaProduccion').prop('disabled', false);
    $('#mensajeRegistros').prop('hidden', true);
    $('#Lomo').prop('checked', false);
    $('#Miga').prop('checked', false);
    $('#Empaque').prop('checked', false);
    $('#Enlatado').prop('checked', false);
    $('#Pouch').prop('checked', false);
    $('#Observacion').val('');
    $('#CardDetalle').prop('hidden', true);
    LimpiarDetalleControles();
    $('#DivDetalles').empty();
    $('#btnEliminarCabeceraControl').prop('disabled', true);
}
function ConfirmarEliminarCab() {
    $('#ModalEliminarCabecera').modal('show');
}
function EliminarCabecera() {
    $('#btnsicab').prop('disabled', true);
    $('#btnnocab').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdCabecera', IdCabecera);


    fetch("../EvaluacionDeLomoyMigaEnBandeja/EliminarCabeceraControl", {
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
            $('#ModalEliminarCabecera').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {
            MensajeCorrecto(resultado[1]);

            LimpiarControles();
            //}

        }
        $('#btnsicab').prop('disabled', false);
        $('#btnnocab').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function LlenarComboLotes(orden) {
    Error = 0;
    $('#cmbLote').empty();
    $('#cmbLote').append('<option>Seleccione...</option>');

    let params = {
        Orden: orden /*$("#cmbOrdeneFabricacion").val()*/
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultarLotesPorOf?' + query;
    if ($('#cmbOrdeneFabricacion').prop('selectedIndex')== 0) {

    } else {
        fetch(url)
            //,body: data
            .then(function (respuesta) {
                if (!respuesta.ok) {
                    //MensajeError(respuesta.statusText);
                    MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                    Error = 1;
                }
                return respuesta.json();
            })
            .then(function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (Error == 0) {
                    //console.log('combolotes');
                    //console.log(resultado);
                    ListaLotes = resultado;
                    $.each(resultado, function (key, value) {
                        $('#cmbLote').append('<option value=' + value.descripcion + '>' + value.descripcion + '</option>');
                    });
                   
                }
            })
            .catch(function (resultado) {
                //console.log(resultado);
                MensajeError(resultado.responseText, false);

            })
    }
}
function DatosLote(){
     var Datos = Enumerable.From(ListaLotes)
         .Where(function (x) { return x.descripcion == $('#cmbLote').val() })
        .Select(function (x) { return x })
        .FirstOrDefault();
    $('#txtBuque').val(Datos.Barco);
}
function GuardarDetalleControl() {
    Error = 0;
    if ($('#txtHora').val() == '') {
        $('#msjHora').prop('hidden', false);
        ActivaInfo();
        return false;
    } else {
        $('#msjHora').prop('hidden', true);
    }
    if ($('#cmbLinea').prop('selectedIndex') == 0) {
        $('#msjLinea').prop('hidden', false);
        ActivaInfo();
        return false;
    } else {
        $('#msjLinea').prop('hidden', true);
    }
    if ($('#cmbLote').prop('selectedIndex') == 0) {
        $('#msjLote').prop('hidden', false);
        ActivaInfo();
        return false;
    } else {
        $('#msjLote').prop('hidden', true);
    }

    if ($('#cmbProteina').prop('selectedIndex') == 0) {
        $('#msjProteina').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjProteina').prop('hidden', true);
    }

    if ($('#cmbSabor').prop('selectedIndex') == 0) {
        $('#msjSabor').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjSabor').prop('hidden', true);
    }
    if ($('#cmbTextura').prop('selectedIndex') == 0) {
        $('#msjTextura').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjTextura').prop('hidden', true);
    }
    if ($('#cmbColor').prop('selectedIndex') == 0) {
        $('#msjColor').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjColor').prop('hidden', true);
    }
    if ($('#cmbOlor').prop('selectedIndex') == 0) {
        $('#msjOlor').prop('hidden', false);
        ActivaEvaluacion();
        return false;
    } else {
        $('#msjOlor').prop('hidden', true);
    }
    
  

    $('#btnEliminarDetalleControl').prop('hidden', true);
    $('#btnGuardarDetalle').prop('hidden', true);
    $('#btnLimpiarDetalle').prop('hidden', true);
    $('#btnCargandoDetalle').prop('hidden', false)
    const data = new FormData();
    data.append('IdDetalleEvaluacionLomoyMigasEnBandeja', IdDetalle);
    data.append('Hora', $("#txtHora").val());
    data.append('Linea', $("#cmbLinea").val());
    data.append('Lote', $("#cmbLote").val());
    data.append('buque', $("#txtBuque").val());
    data.append('Sabor', $("#cmbSabor").val());
    data.append('Textura', $("#cmbTextura").val());
    data.append('Color', $("#cmbColor").val());
    data.append('Olor', $("#cmbOlor").val());
    data.append('Moretones', $("#txtMoretones").val());
    data.append('HematomasProfundos', $("#txtHematomas").val());
    data.append('Proteina', $("#cmbProteina").val());
    data.append('Trozo', $("#txtTrozos").val());
    data.append('Venas', $("#txtVenas").val());
    data.append('Espinas', $("#txtEspinas").val());
    data.append('Sangre', $("#txtSangre").val());
    data.append('Escamas', $("#txtEscamas").val());
    data.append('Piel', $("#txtPiel").val());
    data.append('IdCabeceraEvaluacionLomosYMigasEnBandeja', IdCabecera);

    fetch("../EvaluacionDeLomoyMigaEnBandeja/GuardarDetalleControl", {
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

            if (resultado[0] == "002") {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                //$('#CardDetalle').prop('hidden', false);
                //ConsultarDetalleControl();
                ConsultarDetalleControl();
            }
            LimpiarDetalleControles();
        }
        $('#btnGuardarDetalle').prop('hidden', false);
        $('#btnLimpiarDetalle').prop('hidden', false);
        $('#btnCargandoDetalle').prop('hidden', true);
        $('#btnEliminarDetalleControl').prop('hidden', false);
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function ConsultarDetalleControl() {
    Error = 0;
    let params = {
        IdCabeceraControl: IdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionDeLomoyMigaEnBandeja/PartialDetalleControl?' + query;
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
                $('#DivDetalles').empty();
                $('#DivDetalles').html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#TableDetalle').DataTable(config.opcionesDT);
                LimpiarDetalleControles();
            } else {
                $('#DivDetalles').empty();
            }
            //console.log(resultado);
        })
        .catch(function (resultado) {
            console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
        })
}
function LimpiarDetalleControles() {
    $('#txtHora').prop('disabled', false);
    $('#btnEliminarDetalleControl').prop('disabled', true);
    $('#txtHora').val('');
    $('#txtBuque').val('');
    $('#txtMoretones').val('');
    $('#txtHematomas').val('');
    $('#txtTrozos').val('');
    $('#txtVenas').val('');
    $('#txtEspinas').val('');
    $('#txtSangre').val('');
    $('#txtEscamas').val('');
    $('#txtPiel').val('')
    $('#cmbProteina').prop('selectedIndex', 0);
    $('#cmbLinea').prop('selectedIndex', 0);
    $('#cmbLote').prop('selectedIndex', 0);
    $('#cmbSabor').prop('selectedIndex', 0);
    $('#cmbTextura').prop('selectedIndex', 0);
    $('#cmbColor').prop('selectedIndex', 0);
    $('#cmbOlor').prop('selectedIndex', 0);
    IdDetalle = 0;
    ActivaInfo();

}
function ModificarDetalle(data) {
    IdDetalle = data.IdDetalle;
    $('#txtHora').val(moment(data.Hora).format('YYYY-MM-DDThh:mm'));
    $('#txtBuque').val(data.Buque);
    $('#txtMoretones').val(data.Moretones);
    $('#txtHematomas').val(data.HematomasProfundos);
    $('#txtTrozos').val(data.Trozos);
    $('#txtVenas').val(data.Venas);
    $('#txtEspinas').val(data.Espinas);
    $('#txtSangre').val(data.Sangre);
    $('#txtEscamas').val(data.Escamas);
    $('#txtPiel').val(data.Piel);
    $('#cmbLinea').val(data.Linea);
    $('#cmbLote').val(data.Lote);
    $('#cmbSabor').val(data.CodSabor);
    $('#cmbTextura').val(data.CodTextura);
    $('#cmbColor').val(data.CodColor);
    $('#cmbOlor').val(data.CodOlor);
    $('#cmbProteina').val(data.CodProteinas);
    $('#txtHora').prop('disabled', true);
    $('#btnEliminarDetalleControl').prop('disabled', false);
}
function ActivaInfo() {
    $('#info').addClass("active");
    $('#evalu').removeClass("active");
    $('#verif').removeClass("active");
    $("#informacion").removeClass("active fade");
    $("#evaluacion").removeClass("active fade");
    $("#verificacion").removeClass("active fade");
    $('#informacion').addClass("active");
}
function ActivaEvaluacion() {
    $('#evalu').addClass("active");
    $('#info').removeClass("active");
    $('#verif').removeClass("active");
    $("#informacion").removeClass("active fade");
    $("#evaluacion").removeClass("active fade");
    $("#verificacion").removeClass("active fade");
    $('#evaluacion').addClass("active");
}
function ActivaVerificacion() {
    $('#verif').addClass("active");
    $('#evalu').removeClass("active");
    $('#info').removeClass("active");
    $("#informacion").removeClass("active fade");
    $("#evaluacion").removeClass("active fade");
    $("#verificacion").removeClass("active fade");
    $('#verificacion').addClass("active");
}
function ConfirmarEliminarDetalle() {
    $('#ModalEliminarDetalle').modal('show');
}
function EliminarDetalle() {
    $('#btnsicdet').prop('disabled', true);
    $('#btnnodet').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdDetalle', IdDetalle);


    fetch("../EvaluacionDeLomoyMigaEnBandeja/EliminarDetalleControl", {
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
            MensajeCorrecto(resultado[1]);

            LimpiarDetalleControles();
            ConsultarDetalleControl();
            //}

        }
        $('#btnsidet').prop('disabled', false);
        $('#btnnodet').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}