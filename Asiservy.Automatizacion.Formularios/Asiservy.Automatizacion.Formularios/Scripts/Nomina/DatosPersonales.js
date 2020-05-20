
$(function () {
    $('#tblDataTable thead tr:eq(0) th:eq(0)').trigger('click');
   
    $(".editarDatos").click(function () {

        $("#txtCodigoData").val('');
        $("#txtCedula").val('');
        $("#txtDireccion").val('');
        $("#txtBarrio").val('');
        $("#txtTelefono").val('');
        $("#txtCelular").val('');
        $("#txtCorreo").val('');
        $("#tituloModal").text('');

        var direccion = $(this).data('direccion');
        var barrio = $(this).data('barrio');
        var telefono = $(this).data('telefono');
        var celular = $(this).data('celular');
        var correo = $(this).data('correo');
        var codigodata = $(this).data('codigodata');
        var cedula = $(this).data('cedula');
        var nombres = $(this).data('nombres');

        $("#txtCodigoData").val(codigodata);
        $("#txtCedula").val(cedula);
        $("#txtDireccion").val(direccion);
        $("#txtBarrio").val(barrio);
        $("#txtTelefono").val(telefono);
        $("#txtCelular").val(celular);
        $("#txtCorreo").val(correo);
        $("#tituloModal").text(nombres);

        $("#DatosEmpleado").modal({ backdrop: 'static', keyboard: false, show: true });

        return false;
    });

    $("#sincronizarDL").click(function () {
        $("#sincronizarDL").prop("disabled", true);
        $("#sincronizarDL").text("Enviando información a DataLife...");
        $.ajax({
            dataType: "json",
            url: "../Nomina/ActualizaDatosEmpleadoDataLife",
            type: "POST",
            data: {
                cedula: $("#txtCedula").val(),
                direccion: $("#txtDireccion").val(),
                barrio: $("#txtBarrio").val(),
                telefono: $("#txtTelefono").val(),
                celular: $("#txtCelular").val(),
                correoPersonal: $("#txtCorreo").val(),
                codigo_data: $("#txtCodigoData").val()
            },
            success: function (resultado) {
                if (resultado.Codigo == "1") {
                    alert("Registro actualizado con éxito");
                    window.location.reload(false);
                } else {
                    $("#sincronizarDL").prop("disabled", false);
                    $("#sincronizarDL").text("Sincronizar con DataLife");
                    alert(resultado.Descripcion);
                }
            },
            error: function (resultado) {
                $("#sincronizarDL").prop("disabled", false);
                $("#sincronizarDL").text("Sincronizar con DataLife");
                MensajeError(resultado.statusText, false);
            }
        });
        return false;
    });
});


