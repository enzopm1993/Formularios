$(document).ready(function () {
    ConsultarControl();

});


function ConsultarControl() {
  //  $("#divDetalle").html('');  
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#btnGenerar").prop("hidden", false);
                $("#btnEditar").prop("hidden", true);
              //  $("#divDetalle").html('NO SE HA GENERADO EL CONTROL');  
            } else {
                //$("#txtPcc").prop("disabled", true);
                //$("#txtCodDetectorMetal").prop("disabled", true);
                //$("#chkLomo").prop("disabled", true);
                //$("#chkLata").prop("disabled", true);
                $("#btnEditar").prop("hidden", false);
                $("#btnGenerar").prop("hidden", true);

                $("#txtPcc").val(resultado.Pcc);
                $("#txtIdControl").val(resultado.IdOperatividadMetal);
                $("#chkLomo").val(resultado.Lomos);
                $("#chkLata").val(resultado.Latas);
                $("#txtFerroso").val(resultado.Ferroso);
                $("#txtNoFerroso").val(resultado.NoFerroso);
                $("#txtAceroInoxidable").val(resultado.AceroInoxidable);
                $("#txtCodDetectorMetal").val(resultado.DetectorMetal);
                $("#txtObservacion").val(resultado.Observacion);
              //  console.log(resultado);

            }
               
        },
            error: function (resultado) {
                MensajeError("Error: Comuníquese con sistemas," + resultado, false);
        }
    });
}

function AbrirModal() {
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
        $("#ModalCabecera").modal("show");

    }
}



function Validar() {
    var valida = true;
    if ($("#txtPcc").val() == "") {
        $("#txtPcc").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPcc").css('borderColor', '#ced4da');
    }
   
    if ($("#txtFerroso").val() == "") {
        $("#txtFerroso").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFerroso").css('borderColor', '#ced4da');
    }

    if ($("#txtNoFerroso").val() == "") {
        $("#txtNoFerroso").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNoFerroso").css('borderColor', '#ced4da');
    }

    if ($("#txtAceroInoxidable").val() == "") {
        $("#txtAceroInoxidable").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAceroInoxidable").css('borderColor', '#ced4da');
    }


    if ($("#txtCodDetectorMetal").val() == "") {
        $("#txtCodDetectorMetal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodDetectorMetal").css('borderColor', '#ced4da');
    }
    return valida;
}
function GenerarControl() {
    if (!Validar()) {
        return;
    }
    //alert("ok");
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetal",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            IdOperatividadMetal: $("#txtIdControl").val(),
            Pcc: $("#txtPcc").val(),
            Lomos: $("#chkLomo").val(),
            Latas: $("#chkLata").val(),
            Ferroso: $("#txtFerroso").val(),
            NoFerroso: $("#txtNoFerroso").val(),
            AceroInoxidable: $("#txtAceroInoxidable").val(),
            DetectorMetal: $("#txtCodDetectorMetal").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }else {
                $("#ModalCabecera").modal("hide");
                MensajeCorrecto("Registro Exitoso");
                ConsultarControl();
            }

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas," + resultado, false);
        }
    });
}
