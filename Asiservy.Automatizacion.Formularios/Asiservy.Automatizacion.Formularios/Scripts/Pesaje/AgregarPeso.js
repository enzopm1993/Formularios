$(function () {

    var iconLoader = "fa-spinner fa-pulse";
    var iconSend = "fa-iconSend"

    var sPath = window.location.pathname;
    sPath = sPath.substring(1);
    sPath = sPath.split("/");

    var idDocumento = 0;
    if (sPath.length >= 3) {
        if (sPath[2] == "") {
            idDocumento = 0;
        } else {
            idDocumento = parseInt(sPath[2]);
            if (isNaN(idDocumento)) {
                idDocumento = 0;
            }
        }
         
    }

   

    if (idDocumento > 0) {
        $("#nuevoDocumentoPeso").hide();
        $("#formularioPeso").hide();
        $.ajax({
            url: config.baseUrl + "Pesaje/ObtenerDatosCabecera",
            type: "GET",
            data: {
                idCabecera: idDocumento
            },
            success: function (resultado) {
                if (resultado.codigo > 0) {
                    console.log(resultado);
                    if (resultado.Lista.Datos != null) {

                        $("#codCabecera").text(resultado.Lista.Datos.ID);
                        $("#txtLinea").text(resultado.Lista.Datos.LINEA);
                        $("#txtTurno").text(resultado.Lista.Datos.TURNO);
                        $("#txtLote").text(resultado.Lista.Datos.LOTE);
                        $("#txtTipoLimpieza").text(resultado.Lista.Datos.LIMPIEZA);
                        $("#txtOrden").text(resultado.Lista.Datos.ORDEN_FABRICACION);

                        if (resultado.Lista.Totales.length > 0) {
                            $("#txtTotalRegistros").text(resultado.Lista.Totales[0].Registros);
                            $("#txtTotalPeso").text(resultado.Lista.Totales[0].Peso);


                        }
                        
                        $("#ultimosIngresos").empty();

                        if (!$.isEmptyObject(resultado.Lista.Ultimos)) {
                            $.each(resultado.Lista.Ultimos, function (create, newRow) {

                                var bandeja = 'BANDEJA AMARILLA';
                                if (newRow.TIPO_TARA == 'N') {
                                    bandeja = 'BANDEJA NARANJA';
                                }

                                var PN = newRow.PESO_NETO;
                                var PB = newRow.PESO_BRUTO;
                                var ID_DET = newRow.ID;

                                var newRow = '<li class="list-group-item align-items-center py-3 d-flex" >' + bandeja + ' - PN: ' + PN + ' Kg. - PB: ' + PB + ' Kg. <a  data-id="' + ID_DET + '" href="#" class="badge badge-danger ml-auto eliminaPeso">ELIMINAR</a></li>';
                                $("#ultimosIngresos").append(newRow);
                            });
                        }

                        $.ajax({
                            url: config.baseUrl + "Pesaje/ObtieneTaras",
                            type: "GET",
                            data: {
                                idCabecera: idDocumento
                            },
                            success: function (resultado2) {
                                if (resultado2.codigo > 0) {
                                    if (resultado2.codigo > 0) {
                                        if (!$.isEmptyObject(resultado2.Lista)) {
                                            $.each(resultado2.Lista, function (create, row) {
                                                $("#selectTipoTara").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                                            });
                                        }
                                    }
                                }
                                else {
                                    MensajeError(resultado.Mensaje, false);
                                }
                            },
                            error: function (resultado) {
                                MensajeError(resultado.responseText, false);
                            }
                        });


                        $("#formularioPeso").show();
                    } else {
                        alert("El documento no existe");
                       
                    }
                    
                }
                else {
                    MensajeError(resultado.Mensaje,false);
                }
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });

    } else {
        $("#formularioPeso").remove();
    }

    $("#btnNuevo").click(function () {
        var resp = confirm("¿Está seguro de terminar el proceso y crear un documento nuevo?");
        if (resp) {
            window.location.href = config.baseUrl + "Pesaje/AgregarPeso";
        }
    });
    $("#btnOrden").on("click", function () {
        $("#ModalOrdenes").modal('show');
    });
    $("#btnCrearDocumento").click(function () {
        $("#btnCrearDocumento").attr('href', "javascript:void(0)");
        $("#iconSend").removeClass(iconSend);
        $("#iconSend").addClass(iconLoader);
        $("#btnCrearDocumento").addClass("btnWait");


        $.ajax({
            url: config.baseUrl + "Pesaje/CrearDocumentoPesaje",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                'LIMPIEZA': $("#selectTipoLimpieza").val(),
                'LOTE': $("#SelectLote").val(),
                'ORDEN': $("#txtOrdenFabricacion").val(),
                'TURNO': $("#selectTurno").val()
            }),
            success: function (resultado) {
                console.log(resultado);
                if (resultado.codigo > 0) {
                    alert("Documento # " + resultado.codigo+" creado con éxito, a continuación proceder a ingresar los pesos");
                    window.location.href = config.baseUrl + "Pesaje/AgregarPeso/" + resultado.codigo;
                } else {
                    MensajeError(resultado.Mensaje, false);
                    $("#btnCrearDocumento").attr('href', "#");
                    $("#iconSend").removeClass(iconLoader);
                    $("#iconSend").addClass(iconSend);
                    $("#btnCrearDocumento").removeClass("btnWait");
                }
                
            },
            error: function (resultado) {
                MensajeError(resultado, false);
                $("#btnCrearDocumento").attr('href', "#");
                $("#iconSend").removeClass(iconLoader);
                $("#iconSend").addClass(iconSend);
                $("#btnCrearDocumento").removeClass("btnWait");
            }
        });


        return false;
    });
    $("#selectTipoTara").on("change", function () {
        var regTara = $("#selectTipoTara").val();
        regTara = regTara.split("|");
        var pesoTara = parseFloat(regTara[1]);
        var pesoBruto = $('#nqnPesoBruto').val();
        if (pesoBruto == "") {
            pesoBruto = 0;
        }
        var pesoNeto = parseFloat(pesoBruto) - parseFloat(pesoTara);
        pesoNeto = pesoNeto.toFixed(2);
        $("#txtPesoNeto").text(pesoNeto + " Kg.");
        if (pesoNeto > 0) {
            $("#txtPesoNeto").removeClass("badge-danger");
            $("#txtPesoNeto").addClass("badge-success");
        } else {
            $("#txtPesoNeto").removeClass("badge-success");
            $("#txtPesoNeto").addClass("badge-danger");
        }

        $("#nqnPesoTara").val(pesoTara);
        
    });
    $("#btnEnvia").click(function () {
        $("#btnEnvia").attr('href', "javascript:void(0)");
        $("#iconSendPeso").removeClass(iconSend);
        $("#iconSendPeso").addClass(iconLoader);
        $("#btnEnvia").addClass("btnWait");


        $.ajax({
            url: config.baseUrl + "Pesaje/IngresarPeso",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                'ID_CABECERA': $("#codCabecera").text(),
                'PESO_BRUTO': $("#nqnPesoBruto").val(),
                'TIPO_TARA': $("#selectTipoTara").val()
            }),
            success: function (resultado) {
                if (resultado.codigo > 0) {


                    $("#txtTotalRegistros").text(resultado.Lista.Totales[0].Registros);
                    $("#txtTotalPeso").text(resultado.Lista.Totales[0].Peso);

                    $("#ultimosIngresos").empty();
                    
                    if (!$.isEmptyObject(resultado.Lista.Ultimos)) {
                        $.each(resultado.Lista.Ultimos, function (create, newRow) {

                            var bandeja = 'BANDEJA AMARILLA';
                            if (newRow.TIPO_TARA == 'N') {
                                bandeja = 'BANDEJA NARANJA';
                            }

                            var PN = newRow.PESO_NETO;
                            var PB = newRow.PESO_BRUTO;
                            var ID_DET = newRow.ID;

                            var newRow = '<li class="list-group-item align-items-center py-3 d-flex" >' + bandeja + ' - PN: ' + PN + ' Kg. - PB: ' + PB + ' Kg. <a  data-id="' + ID_DET +'" href="#" class="badge badge-danger ml-auto eliminaPeso">ELIMINAR</a></li>';
                            $("#ultimosIngresos").append(newRow);
                        });
                    }

                    MensajeCorrecto("Enviado", false);
                    $("#nqnPesoBruto").val('');
                    $("#txtPesoNeto").text("0 Kg.");
                    $("#txtPesoNeto").removeClass("badge-success");
                    $("#txtPesoNeto").addClass("badge-danger");

                    $("#btnEnvia").attr('href', "#");
                    $("#iconSendPeso").removeClass(iconLoader);
                    $("#iconSendPeso").addClass(iconSend);
                    $("#btnEnvia").removeClass("btnWait");


                } else {
                    MensajeError(resultado.Mensaje, false);
                    $("#btnEnvia").attr('href', "#");
                    $("#iconSendPeso").removeClass(iconLoader);
                    $("#iconSendPeso").addClass(iconSend);
                    $("#btnEnvia").removeClass("btnWait");
                }

            },
            error: function (resultado) {
                MensajeError(resultado, false);
                $("#btnEnvia").attr('href', "#");
                $("#iconSendPeso").removeClass(iconLoader);
                $("#iconSendPeso").addClass(iconSend);
                $("#btnEnvia").removeClass("btnWait");
            }
        });


        return false;
    });

    $("#ultimosIngresos").on("click", "a.eliminaPeso", function () {
        var id = $(this).data('id');
        $.ajax({
            url: config.baseUrl + "Pesaje/Eliminar",
            type: "GET",
            data: {
                idDetalle: id
            },
            success: function (resultado) {
                if (resultado.codigo > 0) {
                    alert("Registro eliminado correctamente");
                    window.location.reload();
                }
                else {
                    MensajeError(resultado.Mensaje, false);
                }
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
        return false;

    });
    //console.log(idDocumento);
    $('#nqnPesoBruto').keyup(function (event) {
        console.log();
        var nqnTara = $("#nqnPesoTara").val();
        var pesoBruto = $('#nqnPesoBruto').val();
        if (pesoBruto == "") {
            pesoBruto = 0;
        }
        var pesoNeto = parseFloat(pesoBruto) - parseFloat(nqnTara);
        pesoNeto = pesoNeto.toFixed(2);
        $("#txtPesoNeto").text(pesoNeto+" Kg.");
        if (pesoNeto > 0) {
            $("#txtPesoNeto").removeClass("badge-danger");
            $("#txtPesoNeto").addClass("badge-success");
        } else {
            $("#txtPesoNeto").removeClass("badge-success");
            $("#txtPesoNeto").addClass("badge-danger");
        }
    });
    

    $("#modal-orden-si").on("click", function () {
        if ($("#SelectOrdenFabricacion").val() == '') {
            $('#validaOrden').prop("hidden", false);
            return;
        }
        $("#txtOrdenFabricacion").val($("#SelectOrdenFabricacion").val());
        CargarLotes($("#SelectOrdenFabricacion").val());
        $("#ModalOrdenes").modal('hide');
        $('#validaOrden').prop("hidden", true);
    });

    $("#modal-orden-no").on("click", function () {
        $("#ModalOrdenes").modal('hide');
    });

    $("#txtFechaProduccion").on("change", function () {
        $("#txtFechaOrden").val($("#txtFechaProduccion").val());
        CargarOrdenFabricacion($('#txtFechaProduccion').val());
    });
  
    CargarOrdenFabricacion($('#txtFechaProduccion').val());
});
function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 2)) {
        return false;
    }
    return true;
}

function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}



function CargarOrdenFabricacion(valor) {
    if (valor == '' || valor == null)
        return;
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: config.baseUrl + "Hueso/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.Orden + "'>" + row.Orden + "</option>")
                });
            }
            //  CargarControlHueso();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}


function CargarLotes(valor) {


    $("#SelectLote").empty();
    $("#SelectLote").append("<option value='0' >-- Seleccionar Opción--</option>");
    if (valor == '') {
        return;
    }
    $.ajax({
        url: config.baseUrl + "Hueso/ConsultarLotesPorLinea",
        type: "GET",
        data: {
            Orden: valor
        },
        success: function (resultado) {
            console.log(valor);
            if (resultado == "101") {
                window.location.reload();
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectLote").append("<option value='" + row.descripcion + "'>" + row.descripcion + "</option>")
                });
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}