


function CargarEmpleados() {


    $.ajax({
        url: "../Mensaje/EmpleadoBuscar",
        type: "Get",
        success: function (resultado) {
            $('#ModelCargarEmpleados').html(resultado);
            $("#ModalEmpleado").modal("show");

        }
    });

}

function CambioLinea(valor) {
   // console.log(valor);
    $.get("../SolicitudPermiso/ConsultaListadoAreas", { CodLinea: valor }, function (data) {
        if (!$.isEmptyObject(data)) {
            $("#selectArea").empty();
            $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
            $.each(data, function (create, row) {
                $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
            });
        } else {
            $("#selectArea").empty();
            $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
            MensajeCorrecto("La linea seleccionado no tiene areas asignadas", false);
        }
    });
}


function CambioArea(valor) {
    //console.log(valor);
    $.get("../SolicitudPermiso/ConsultaListadoCargos", { CodArea: valor }, function (data) {
        if (!$.isEmptyObject(data)) {
            $("#selectCargo").empty();
            $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
            $.each(data, function (create, row) {
                $("#selectCargo").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
            });
        } else {
            $("#selectCargo").empty();
            $("#selectCargo").append("<option value='' >-- Seleccionar Opción--</option>");
            MensajeCorrecto("La linea seleccionado no tiene cargos asignadas", false);
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
    console.log(check);

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





