﻿var itemEditar = [];
var inputSelect = [];
var listaIdIntermedia = [];
$(document).ready(function () {
    CargarCabecera(0);
    document.getElementById("txtFechaCabecera").disabled = true; 
});
function CargarCabecera(op) {
    $('#cargac').show();
    if ($('#txtFecha').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../LavadoCisterna/LavadoCisternaPartial",
        data: {
            fechaDesde:$("#txtFecha").val(),
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
                $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {
    console.log(moment(document.getElementById("txtFechaCabecera").value).format('YYYY-MM-DD'));
    var date = document.getElementById("txtFechaCabecera").value;
    if (date>moment().format('YYYY-MM-DD') || moment(date).format('YYYY')<2015) {
        MensajeAdvertencia('Fecha incorrecta o Mayor a la fecha actual');
        return;
    }
    $('#cargac').show();
    $.ajax({
        url: "../LavadoCisterna/GuardarModificarLavadoCisterna",
        type: "POST",
        data: {
            IdLavadoCisterna: itemEditar[0],
            Fecha: $("#txtFechaCabecera").val(),
            QuimUtilizados: $("#txtQUtilizados").val(),
            Observacion: $("#txtObservacion").val(),
            idMantCisterna: itemEditar[6],
            idIntermedia: listaIdIntermedia,
            siAprobar: 0,
            fechaDesde: $('#txtFecha').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado=='2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            }
            $('#ModalIngresoCabecera').modal('hide');
                LimpiarCabecera();          
                $('#cargac').hide();
                CargarCabecera(0);
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
                    $('#cargac').hide();
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
        //$("#txtFechaCabecera").prop('disabled',true);
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
    var ddl = $('#txtNDescripcion').val();
    //var sel = document.getElementById('txtNDescripcion');
    //var selected = sel.options[sel.selectedIndex].text;
    if (ddl!=null) {
        $('#ddl').text(ddl);
        console.log($('#ddl').val(ddl));
    }
    var con = 0;
    if ($('#txtFechaCabecera').val() == '') {
        $("#txtFechaCabecera").css('border', '1px dashed red');
        con = 1;
    } else $("#txtFechaCabecera").css('border', '');
    if ($('#txtNCisterna').val() == '') {
        $("#txtNCisterna").css('border', '1px dashed red');
        con = 1;
    } else $("#txtNCisterna").css('border', '');
    if ($('#txtQUtilizados').val() == '') {
        $("#txtQUtilizados").css('border', '1px dashed red');
        con = 1;
    } else $("#txtQUtilizados").css('border', '');
    if ($('#txtObservacion').val() == '') {
        $("#txtObservacion").css('border', '1px dashed red');
        con = 1;
    } else $("#txtObservacion").css('border', '');
    
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
                                 <input type="checkbox" id="CheckNCisterna_`+ entry.NDescripcion + `" class="custom-control-input" name="check" value="` + entry.NDescripcion+`" />
                                <label class="custom-control-label" for="CheckNCisterna_`+ entry.NDescripcion + `" id="lblNCisterna">CISTERNA: ` + entry.NDescripcion +`</label>
                            </div>
                        </div>
                        `;        
        document.getElementById('divFila').appendChild(div);
    });
    if ($('#txtNCisterna').val()!='') {
        var splitNDescripcion = $('#txtNCisterna').val().split(';');
        splitNDescripcion.forEach(function (objSplit) {
            if (objSplit!='') {
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