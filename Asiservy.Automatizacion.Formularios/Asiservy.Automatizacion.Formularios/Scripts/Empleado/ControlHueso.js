

function CargarControlHueso(id) {

    $.ajax({
        url: "../Empleado/ControlHuesoPartial",
        type: "GET",
        data: {
            id: id         
        },
        success: function (resultado) {
            var bitacora = $('#DivTableControlHueso');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
        }
    });

}

function GenerarControlHueso() {
    var hora = $('#SelectHora').val();
    if (hora == "1") {
        var fecha = new Date();
        hora = fecha.toLocaleTimeString();
        console.log(hora);
    }
    if ($('#SelectLote').val() != 0) {
        $.ajax({
            url: "../Empleado/GenerarControlHueso",
            type: "GET",
            data: {
                dsLinea: $('#txtLinea').val(),
                dsLote: $('#SelectLote').val(),
                Hora: hora
            },
            success: function (resultado) {
                CargarControlHueso(resultado);
                $('#btnGenerar').prop("hidden", true);

            },
            error: function (resultado) {
                MensajeError(resultado.responseJSON, false);
            }
        });
    } else
        MensajeAdvertencia("Seleccione un lote");

}

function ValidaControlHueso(hora) {
    if (hora != 0) {
        if (hora == "1") {
            var fecha = new Date();
            hora = fecha.toLocaleTimeString();
         //   console.log(hora);
        }
        $.ajax({
            url: "../Empleado/ValidaControlHueso",
            type: "GET",
            data: {
                dsLinea: $('#txtLinea').val(),
                Hora: hora
            },
            success: function (resultado) {
                if (resultado == 0) {
                    var bitacora = $('#DivTableControlHueso');
                    bitacora.html('');
                    $('#btnGenerar').prop("hidden", false);
                    $('#SelectLote').prop("disabled", false);

                } else {
                    CargarControlHueso(resultado)
                    $('#SelectLote').prop("selectedIndex", 0);
                    $('#SelectLote').prop("disabled", true);
                    $('#btnGenerar').prop("hidden", true);

                }
            },
            error: function (resultado) {
                MensajeError(resultado.responseJSON, false);
            }
        });

    }
}

function seleccionLote(valor) {
    if (valor != 0) {
        SelectHora = $('#SelectHora').val();
        if (SelectHora == 0) {
            MensajeAdvertencia("Seleccione una Hora");
            $('#SelectLote').prop("selectedIndex", 0);
        } else {
            ValidaControlHueso(SelectHora);
        }

    }

}


function checkControlHueso(id, detalle) {
    id = "#" + id;
    label = "#labelCheck-" + detalle;
    var txtHueso = '#Huesos-' + detalle;
    var huesos = $(txtHueso).val();
    if ($(id).prop('checked')) {
        if (huesos > 0) {
            $(label).css("background", "#28B463");
            $(txtHueso).prop("readonly", true);
            GuardarControlHueso(detalle, huesos);
        } else {
            $(id).prop('checked', false);
            $(label).css("background", "#7b8a8b");
            MensajeAdvertencia("Ingrese una cantidad de huesos");
        }
    } else {
        $(label).css("background", "#7b8a8b");
        $(txtHueso).prop("readonly", false);
        GuardarControlHueso(detalle, 0);

    }
}


function GuardarControlHueso(detalle, hueso) {
        $.ajax({
            url: "../Empleado/GuardarControlHueso",
            type: "GET",
            data: {
                IdControlHuesoDetalle: detalle,
                CantidadHueso: hueso
            },
            success: function (resultado) {                

            },
            error: function (resultado) {
                MensajeError(resultado.responseJSON, false);
            }
        });   

}
