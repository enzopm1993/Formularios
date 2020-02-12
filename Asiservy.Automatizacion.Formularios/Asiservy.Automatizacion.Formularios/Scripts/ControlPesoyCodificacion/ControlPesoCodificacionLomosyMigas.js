$(document).ready(function () {
    CargarCabeceraControl();
});
//region Detalle Descripcion de Uso
function LimpiarControlesUsos() {
    $('#cmbUso').prop('selectedIndex',0);
    $('#txtCantidadUso').val('');
    $('#btnEliminarUsos').prop('disabled', true);
    $('#IdDescripcionUso').val('');
    $('#cmbUso').prop('disabled', false);
}
function GuardarUsos() {
    if ($('#txtCantidadUso').val() == '') {
        $('#msjtxtCantidadUso').html('EL campo cantidad es requerido');
        return false;
    } else {
        $('#msjtxtCantidadUso').html('');
    }
    if ($('#cmbUso').prop('selectedIndex') == 0) {
        $('#msjtcmbUso').html('El campo Uso es requerido');
        return false;
    } else {
        $('#msjtcmbUso').html('');
    }
    $('#btnGuardarCargandoUsos').prop('hidden', false);
    $('#btnAgregarDetUsos').prop('hidden', true);
    $('#btnEliminarUsos').prop('hidden', true);
    $('#btnlimpiarUsos').prop('hidden', true);

    $.ajax({
        url: "../ControlPesoyCodificacion/GuardarUso",
        type: "POST",
        data: {
            IdDescripcionUso: $('#IdDescripcionUso').val() != '' ? $('#IdDescripcionUso').val():0 ,
            Codigo: $('#cmbUso').val(),
            Cantidad: $('#txtCantidadUso').val(),
            IdCabeceraControlPesoCodificacion: $('#IdCabControlPeso').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {

                //MensajeCorrecto(resultado[1],false);
                //LimpiarModalMuestras();
                $('#DivDetalleUsos').empty();
                ConsultarUsos();
                $('#btnGuardarCargandoUsos').prop('hidden', true);
                $('#btnAgregarDetUsos').prop('hidden', false);
                $('#btnEliminarUsos').prop('hidden', false);
                $('#btnlimpiarUsos').prop('hidden', false);
                LimpiarControlesUsos();
                MensajeCorrecto(resultado[1]);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnGuardarCargandoUsos').prop('hidden', true);
            $('#btnAgregarDetUsos').prop('hidden', false);
            $('#btnEliminarUsos').prop('hidden', false);
            $('#btnlimpiarUsos').prop('hidden', false);
        }
    });
}
function ConsultarUsos() {
    $.ajax({
        url: "../ControlPesoyCodificacion/PartialConsultarUsos",
        type: "GET",
        data: {
            IdCabeceraControlPesoCodificacion: $('#IdCabControlPeso').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {
                //MensajeCorrecto(resultado[1],false);
                $('#DivDetalleUsos').empty();
                $('#DivDetalleUsos').html(resultado);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function EditarUso(data) {
    $('#IdDescripcionUso').val(data.IdDescripcionUso);
    $('#cmbUso').val(data.Codigo);
    $('#cmbUso').prop('disabled', true);
    $('#txtCantidadUso').val(data.Cantidad);
    $('#btnEliminarUsos').prop('disabled', false);
}
function EliminarUso() {
    $.ajax({
        url: "../ControlPesoyCodificacion/InactivarUso",
        type: "POST",
        data: {
            IdDescripcionUso: $('#IdDescripcionUso').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {
                $('#ModalEliminarUso').modal('hide');
                MensajeCorrecto(resultado[1], false);
                ConsultarUsos();
                LimpiarControlesUsos();
             
            }
        },
        error: function (resultado) {
            $('#ModalEliminarHora').modal('hide');
            MensajeError(resultado.responseText, false);

        }
    });  
}
function ConfirmarEliminarUsos() {
    $('#MensajeEliminarUso').html('¿Esta seguro que desea eliminar el registro de: ' + $('#cmbUso option:selected').html() + '?');
    $('#ModalEliminarUso').modal('show');
}
//endregion
//region Detalle Muestras
function EliminarMuestraHora() {
    $.ajax({
        url: "../ControlPesoyCodificacion/InactivarMuestra",
        type: "POST",
        data: {
            IdDetalleHorasControlPesoCodificacion: $('#IdDetalleHoraModificar').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            
            else {
             
                $('#ModalEliminarMuestraHora').modal('hide');
                MensajeCorrecto(resultado[1], false);
                ConsultarMuestrasPorHora();
                LimpiarControlesMuestraHora();
                //LimpiarControlesHora();
                //$('#btnEliminarHora').prop('disabled', true);
            }
        },
        error: function (resultado) {
            $('#ModalEliminarMuestraHora').modal('hide');
            MensajeError(resultado.responseText, false);

        }
    });  
}
function ConfirmarEliminarMuestraHora() {
    $('#ModalEliminarMuestraHora').modal('show');
    $('#MensajeEliminarMuestraHora').html('¿Esta seguro que desea eliminar la muestra N° ' + $('#NMuestraModal').val()+'?');
}
function LimpiarControlesMuestraHora() {
    $('#NMuestraModal').val('');
    $('#NMuestraModal').prop('disabled', false);
    $('#CantidadModal').val('');
    $('#btnEliminarMuestraHora').prop('disabled', true)
    $('#IdDetalleHoraModificar').val('');
}
function EditarMuestraHora(data) {
    $('#NMuestraModal').prop('disabled', true);
    $('#NMuestraModal').val(data.NumeroMuestra);
    $('#CantidadModal').val(data.Cantidad);
    $('#IdDetalleHoraModificar').val(data.IdDetalleHorasControlPesoCodificacion);
    $('#btnEliminarMuestraHora').prop('disabled', false);
}
function AgregarMuestra(HoraDet) {
    LimpiarControlesMuestraHora();
    $('#ModalMuestrasHoras').modal('show');

    $('#HoraModal').val(HoraDet.Hora);
    $('#IdDetalleModalHora').val(HoraDet.IdDetalleControlPeso);
    ConsultarMuestrasPorHora();

}
function ConsultarMuestrasPorHora() {
    $.ajax({
        url: "../ControlPesoyCodificacion/PartialConsultarMuestrasPorHora",
        type: "GET",
        data: {
            IdDetalleControlPesoCodificacion: $('#IdDetalleModalHora').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {
                //MensajeCorrecto(resultado[1],false);
                $('#DivMuestrasPorHora').empty();
                $('#DivMuestrasPorHora').html(resultado);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function GuardarMuestraHora() {
    $('#btnGuardarMuestraHoraCargando').prop('hidden', false);
    $('#btnGuardarMuestraHora').prop('hidden', true);

    $.ajax({
        url: "../ControlPesoyCodificacion/GuardarMuestrasPorHora",
        type: "POST",
        data: {
            IdDetalleControlPesoCodificacion: $('#IdDetalleModalHora').val(),
            NumeroMuestra: $('#NMuestraModal').val(),
            Cantidad: $('#CantidadModal').val(),
            IdDetalleHorasControlPesoCodificacion: $('#IdDetalleHoraModificar').val() != '' ? $('#IdDetalleHoraModificar').val() : 0

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {

                //MensajeCorrecto(resultado[1],false);
                //LimpiarModalMuestras();
                $('#DivMuestrasPorHora').empty();
                ConsultarMuestrasPorHora();
                $('#btnGuardarMuestraHoraCargando').prop('hidden', true);
                $('#btnGuardarMuestraHora').prop('hidden', false);
                LimpiarControlesMuestraHora();
                MensajeCorrecto(resultado[1]);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnGuardarMuestraHoraCargando').prop('hidden', true);
            $('#btnGuardarMuestraHora').prop('hidden', false);
        }
    });

}
//endregion
//region Hora
function LimpiarControlesHora() {
    $('#txtHora').val('');
    $('#txtTemperaturaAgua').val('');
    $('#IdHora').val('');
    $('#btnEliminarMuestraHora').prop('disabled', true);
    $('#txtHora').prop('disabled', false);
    
}
function ConfirmarEliminarHora() {
    $('#MensajeEliminarHora').html('¿Esta seguro que desea eliminar el registro de: ' + $('#txtHora').val()+'?');
    $('#ModalEliminarHora').modal('show');

}
function EliminarHora() {
    $.ajax({
        url: "../ControlPesoyCodificacion/InactivarHora",
        type: "POST",
        data: {
            IdDetalleControlPeso: $('#IdHora').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {
                $('#ModalEliminarHora').modal('hide');
                MensajeCorrecto(resultado[1],false);
                CargarHorasControl();
                LimpiarControlesHora();
                $('#btnEliminarHora').prop('disabled', true);
            }
        },
        error: function (resultado) {
            $('#ModalEliminarHora').modal('hide');
            MensajeError(resultado.responseText, false);

        }
    });  

}
function EditarHoras(data) {
    //item.Hora, item.IdDetalleControlPeso, item.TemperaturaAguaTermoencogido
    $('#txtHora').val(data.Hora);
    $('#txtTemperaturaAgua').val(data.TemperaturaAguaTermoencogido);
    $('#btnEliminarHora').prop('disabled', false);
    $('#txtHora').prop('disabled', true);
    $('#IdHora').val(data.IdDetalleControlPeso);

}
function GuardarHoras() {
    if ($('#txtHora').val() == '') {
        $('#msjtxthora').html('el campo Hora es requerido');
        return false;
    } else {
        $('#msjtxthora').html('');
    }
    if ($('#txtTemperaturaAgua').val() == '') {
        $('#msjtxtTemperaturaAgua').html('el campo T° Agua de Termoencogido es requerido');
        return false;
    } else {
        $('#msjtxtTemperaturaAgua').html('');
    }
    $('#btnGuardarCargandoHoras').prop('hidden', false);
    $('#btnAgregarDetHoras').prop('hidden', true);
    $('#btnEliminarHora').prop('hidden', true);
    $('#btnlimpiarHora').prop('hidden', true);
    
        $.ajax({
            url: "../ControlPesoyCodificacion/GuardarHorasControl",
            type: "POST",
            data: {
                IdDetalleControlPeso: $('#IdHora').val() != '' ? $('#IdHora').val() : 0,
                IdCabeceraControlPesoCodificacion: $('#IdCabControlPeso').val(),
                Hora: $('#txtHora').val(),
                TemperaturaAguaTermoencogido: $('#txtTemperaturaAgua').val()
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                //MensajeCorrecto(resultado, false);
                else {
                    LimpiarControlesHora();
                    //MensajeCorrecto(resultado[1],false);
                    if (resultado[0] == '666') {
                        MensajeAdvertencia(resultado[1],false);
                    } else {
                        MensajeCorrecto(resultado[1],false);
                    }
                    
                    CargarHorasControl();
                    $('#btnGuardarCargandoHoras').prop('hidden', true);
                    $('#btnAgregarDetHoras').prop('hidden', false);
                    $('#btnEliminarHora').prop('hidden', false);
                    $('#btnlimpiarHora').prop('hidden', false);
                }
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
                $('#btnGuardarCargandoHoras').prop('hidden', true);
                $('#btnAgregarDetHoras').prop('hidden', false);
                $('#btnEliminarHora').prop('hidden', false);
                $('#btnlimpiarHora').prop('hidden', false);
            }
        });  
}
function CargarHorasControl() {
    
    $.ajax({
        url: "../ControlPesoyCodificacion/CargarHorasControl",
        type: "GET",
        data: {
            IdCabeceraCOntrol: $('#IdCabControlPeso').val()
            //,Hora: $('#txtHora').val(),
            //TemperaturaAguaTermoencogido: $('#txtTemperaturaAgua').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
       
            else {
                //$('#DivDetalleMuestrasporHora').empty('');
                $('#DivDetalleMuestrasporHora').html(resultado);

            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
//endregion
//region Detalles
function CargarDetallesControl() {
    $.ajax({
        url: "../ControlPesoyCodificacion/CargarPartialControlDetalles",
        type: "GET",
        data: {
            IdCabeceraCOntrol: $('#IdCabControlPeso').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            else {
                $('#DivCargarDetalles').html(resultado);
                
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
//endregion
//region Cabecera
function CargarCabeceraControl() {
    
    $.ajax({
        url: "../ControlPesoyCodificacion/ConsultarCabeceraControl",
        type: "GET",
        data: {
            FechaProduccion: $('#txtFechaCebeceraControl').val(),
            Turno: $('#selectTurno').val(),
        },
        success: function (resultado) {
 
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            if (resultado[0] == '111') {
                //MensajeCorrecto('Registro ingresado con éxito', false);
                $('#IdCabControlPeso').val(resultado[1]);
                //$('#DivDetallesControl').prop('hidden', false);
                $('#txtObservacion').val(resultado[2].Observacion);
                $('#txtSaldoAnterior').val(resultado[2].SaldoAnterior);
                $('#txtSolicitudProceso').val(resultado[2].SolicitudProceso);
                $('#txtUtilizadas').val(resultado[2].Utilizadas);
                CargarDetallesControl();
                CargarHorasControl();
                ConsultarUsos();
            } else {
                //$('#DivDetallesControl').prop('hidden', true);
                $('#DivDetallesControl').empty();
                $('#txtObservacion').val('');
                $('#txtSaldoAnterior').val('');
                $('#txtSolicitudProceso').val('');
                $('#txtUtilizadas').val('');
            }
            if (resultado[0] == '222') {
                $('#IdCabControlPeso').val('');
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
          
        }
    });
}
function AgregarCabeceraControl() {
    $('#btnGuardarCargando').prop('hidden', false);
    $('#btnAgregarCabeceraControl').prop('hidden', true);
    $.ajax({
        url: "../ControlPesoyCodificacion/GuardarCabeceraControl",
        type: "POST",
        data: {
            IdCabeceraControlPesoYCodificacion: $('#IdCabControlPeso').val() != '' ? $('#IdCabControlPeso').val(): 0,
            FechaProduccion: $('#txtFechaCebeceraControl').val(),
            Turno: $('#selectTurno').val(),
            Observacion: $('#txtObservacion').val(),
            SaldoAnterior: $('#txtSaldoAnterior').val(),
            SolicitudProceso: $('#txtSolicitudProceso').val(),
            Utilizadas: $('#txtUtilizadas').val()
        },
        success: function (resultado) {
            $('#btnGuardarCargando').prop('hidden', true);
            $('#btnAgregarCabeceraControl').prop('hidden', false);
            if (resultado == "101") {
                window.location.reload();
            }
            //MensajeCorrecto(resultado, false);
            if (resultado[0] == '111') {
                MensajeCorrecto(resultado[1], false);
                $('#IdCabControlPeso').val(resultado[2].IdCabeceraControlPesoYCodificacion);
                //$('#DivDetallesControl').prop('hidden', false);
                CargarDetallesControl();
                CargarHorasControl();
            } else {
                MensajeAdvertencia(resultado[1]);
                //$('#DivDetallesControl').prop('hidden', true);
            }
        },
        error: function (resultado) {
            $('#btnGuardarCargando').prop('hidden', true);
            $('#btnAgregarCabeceraControl').prop('hidden', false);
            MensajeError(resultado.responseText, false);
        }
    });
}
//endregion
