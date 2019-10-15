function CargarEmpleados(formulario) {
    //console.log($('#selectLinea').val());   
    //console.log($('#selectArea').val());   
    //console.log($('#selectCargo').val());  
    if ($('#selectLinea').val() != '') {
        $('#' + formulario).attr("disabled", true);
        $.ajax({
            url: "../SolicitudPermiso/EmpleadoBuscar",
            type: "Get",
            data:
            {
                dsLinea: $('#CodLinea').val(),
                //dsLinea: $('#selectLinea').val(),
                //dsArea: $('#selectArea').val(),
                //dsCargo: $('#selectCargo').val(),
                formulario:"PrestarCuchillo"
            },
            success: function (resultado) {
                $('#ModelCargarEmpleados').html(resultado);
                $("#ModalEmpleado").modal("show");

            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);
                $('#' + formulario).remove("disabled");
            }
        });
    } else {
        MensajeAdvertencia("Seleccione una LINEA", false)
    }

}
//METODOS PARA CUCHILLOS
function check(color) {
    // alert(id);
  
    console.log(color);
    console.log($('#Identificacion').val());
    //7b8a8b
    var estado = "1";
    if ($("#CuchilloBlanco").prop('selectedIndex')==0) {
        //parametros cuchillo victor
        //  alert("seleccione");
        GuardarControlCuchillo($('#Identificacion').val(), color, 1, estado, false);
        //***

        //GuardarControlCuchillo(cedula, color, id, estado, true);
    } else {
        //parametros cuchillo victor
        //     alert("cuchillo seleccionado");
        GuardarControlCuchillo($('#Identificacion').val(), color, $('#CuchilloBlanco').val(), estado, true);
        //**

        //GuardarControlCuchillo(cedula, color, id, estado, false);
    }
}
function GuardarControlCuchillo(cedula, color, numero, estado, check) {
    $.ajax({
        url: "../Asistencia/GuardarControlCuchillo",
        type: "GET",
        data: {
            dsCedula: cedula,
            dsColor: color,
            dsNumero: numero,
            dsEstado: estado,
            dbCheck: check,
            dbTipo: true
        },
        success: function (resultado) {
            MensajeCorrecto(resultado,true)
            //alert(resultado);
            //if (resultado = "No es posible asignar el cuchillo, por que ya ha sido prestado")
            //{
            //    MensajeError(resultado, false);
            //}
        },
        error: function (resultado) {
            //console.log(resultado.responseJSON);
            MensajeError(resultado.responseJSON + "", false);
            //if (color == "B") {
            //    $('#Blanco' + cedula).prop('selectedIndex', 0);
            //}
            //if (color == "R") {
            //    $('#Rojo' + cedula).prop('selectedIndex', 0);
            //}
            //if (color == "N") {
            //    $('#Rojo' + cedula).prop('selectedIndex', 0);
            //}

        }
    });
}
//FIN METODOS PARA CUCHILLOS
function ActivaCombos() {
    alert('activo combo');
    if ($('#Identificacion').val() != "") {
        $('#CuchilloBlanco').prop('disabled', false);
        $('#CuchilloRojo').prop('disabled', false);
        $('#CuchilloNegro').prop('disabled', false);
    } else {
        $('#CuchilloBlanco').prop('disabled', true);
        $('#CuchilloRojo').prop('disabled', true);
        $('#CuchilloNegro').prop('disabled', true);
        }
}

