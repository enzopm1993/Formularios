var itemEditar = [];
var inputSelect = [];
var listaIdIntermedia = [];
$(document).ready(function () {
    CargarCabecera(0);
});
function CargarCabecera(op) {
    $('#cargac').show();
    $.ajax({
        url: "../LimpiezaDesinfeccionPlanta/LavadoCisternaPartial",
        data: {
            fechaDesde: $("#txtFecha").val(),
            fechaHasta: $("#txtFecha").val(),
            idLavadoCisterna: 0,
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
        url: "../LimpiezaDesinfeccionPlanta/GuardarModificarControlLimpieza",
        type: "POST",
        data: {
            IdControlHigiene: itemEditar[0],
            FechaControl: $("#txtIngresoFechaCabecera").val(),
            ObservacionControl: $("#txtObservacion").val()           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                MensajeCorrecto('Registro guardado correctamente');
            } else if (resultado == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            }
            $('#ModalIngresoCabecera').modal('hide');
            $('#divBotonesCRUD').prop('hidden', false);
            $('#divMostrarCabecera').prop('hidden', false);
            $('#divCabecera1').prop('hidden', true);
            setTimeout(function () {
                LimpiarCabecera();
                $('#cargac').hide();
                CargarCabecera(0);
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConfirmar(jdata) {
    if (jdata[5] == 'True') {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        return;
    } else {
        $("#modalEliminarControl").modal("show");
        itemEditar = jdata;
        $("#myModalLabel").text("¿Desea Eliminar el registro?");
    }
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    $.ajax({
        url: "../LavadoCisterna/EliminarLavadoCisterna",
        type: "POST",
        data: {
            IdLavadoCisterna: itemEditar[0],
            fechaDesde: $('#txtFecha').val()
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
                $("#modalEliminarControl").modal("hide");
                CargarCabecera(0);
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

function ActualizarCabecera(jdata, jIdIntermedia) {
    if (jdata[5] == 'True') {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        return;
    } else {
        $("#txtFechaCabecera").prop('disabled', true);
        var date = $("#txtFechaCabecera").val(moment(jdata[1]).format("YYYY-MM-DD"));
        $("#txtFechaCabecera").val(date[0].defaultValue);
        $("#txtNCisterna").val(jdata[2]);
        $("#txtQUtilizados").val(jdata[3]);
        $("#txtObservacion").val(jdata[4]);
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
        listaIdIntermedia = jIdIntermedia;//Lista de IDIntermedia a eliminar
    }
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $("#txtFechaCabecera").prop('disabled', false);
    $('#ModalIngresoCabecera').modal('show');
    $("#txtFechaCabecera").val(moment($("#txtFecha").val()).format("YYYY-MM-DD"));
    itemEditar = [];
    listaIdIntermedia = [];
}

function LimpiarCabecera() {
    $('#txtNCisterna').val('');
    $('#txtQUtilizados').val('');
    $('#txtObservacion').val('');
    $("#txtFechaCabecera").css('border', '');
    $("#txtNCisterna").css('border', '');
    $("#txtQUtilizados").css('border', '');
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
    if ($('#txtIngresoFechaCabecera').val() == '') {
        $("#txtIngresoFechaCabecera").css('border', '1px dashed red');
        con = 1;
    }
    return con;
}

function MultiSelectCisterna(jdata) {
    inputSelect = jdata;
    $('#divFila').empty();
    jdata.forEach(function (entry) {
        $('#ModalIngresoCisterna').modal('show');
        const div = document.createElement('div');
        div.className = 'col-md-12 col-6 col-sm-6';
        div.innerHTML = `
                        <div class="form-group">                            
                            <div class="custom-control custom-checkbox mb-3">                                  
                                 <input type="checkbox" id="CheckNCisterna_`+ entry.NDescripcion + `" class="custom-control-input" name="check" value="` + entry.NDescripcion + `" />
                                <label class="custom-control-label" for="CheckNCisterna_`+ entry.NDescripcion + `" id="lblNCisterna">CISTERNA: ` + entry.NDescripcion + `</label>
                            </div>
                        </div>
                        `;
        document.getElementById('divFila').appendChild(div);
    });
    if ($('#txtNCisterna').val() != '') {
        var splitNDescripcion = $('#txtNCisterna').val().split(';');
        splitNDescripcion.forEach(function (objSplit) {
            if (objSplit != '') {
                jdata.forEach(function (objLista) {
                    if (objSplit == objLista.NDescripcion) {
                        var nomInput = '#CheckNCisterna_' + objSplit;
                        $(nomInput).prop('checked', true);
                    }
                });
            }
        });
    }
}

function AgregarCisternas() {
    var cadena = "";
    var cadenaIdIntermedia = "";
    $('#txtNCisterna').val('');
    inputSelect.forEach(function (entry) {
        var nomInput = '#CheckNCisterna_' + entry.NDescripcion;
        if ($(nomInput).prop('checked') == true) {
            cadena += $(nomInput).val() + ";";
            cadenaIdIntermedia += entry.IdCisterna + ";";//Guardo una cadena de IdMantCistterna para guardar en la tabla intermedia
        }
    });
    itemEditar[6] = cadenaIdIntermedia;
    $('#txtNCisterna').val(cadena);
    $('#ModalIngresoCisterna').modal('hide');
    $("#txtNCisterna").css('border', '');
}