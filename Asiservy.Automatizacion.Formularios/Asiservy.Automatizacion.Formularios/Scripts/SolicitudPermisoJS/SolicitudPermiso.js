


function CambioLinea(valor) {
       $("#selectArea").empty();
    $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#selectCargo").empty();
    $("#selectCargo").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoAreas",
        type: "Get",
        data:
        {
            CodLinea: valor
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);           
        }
    });
}


function CambioArea(valor) {
    
    $("#selectCargo").empty();
    $("#selectCargo").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoCargos",
        type: "Get",
        data:
        {
            CodArea: valor
        },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $.each(data, function (create, row) {
                    $("#selectCargo").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}


function CambioHoraFecha() {
    var HoraDesde = document.getElementById("timeHoraSalida");
    var HoraHasta = document.getElementById("timeHoraRegreso");
    var FechaSalidaRegreso = document.getElementById("dateSalidaRegreso");
    var FechaDesde = document.getElementById("dateSalida");
    var FechaHasta = document.getElementById("dateRegreso");
    var check = document.getElementById("switchHoraFecha").checked
    //console.log(check);

    if (check) {
        HoraDesde.removeAttribute("readonly");
        HoraHasta.removeAttribute("readonly");
        FechaSalidaRegreso.removeAttribute("readonly");
        FechaDesde.setAttribute("readonly", true);
        FechaHasta.setAttribute("readonly", true);
        console.log(FechaDesde);
        FechaDesde.value = null;
        FechaHasta.value = null;
    } else {

        HoraDesde.setAttribute("readonly", true);
        HoraHasta.setAttribute("readonly", true);
        FechaSalidaRegreso.setAttribute("readonly", true);
        FechaDesde.removeAttribute("readonly");
        FechaHasta.removeAttribute("readonly");
        FechaSalidaRegreso.value = null;
        HoraDesde.value = null;
        HoraHasta.value = null;
    }
}





