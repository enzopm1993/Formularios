$(document).ready(function () {


    $('#txtPlantilla').summernote({
        codeviewFilter: false,
        codeviewIframeFilter: true,
        dialogsInBody: false,
        lang: 'es-ES',
        height: 800,
        toolbar: [

            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']]
        ]
    });

    $(".asignaEtiqueta").click(function () {
        var tag = $(this).data('tag');
        $("#modalTags").modal('hide');
        $('#txtPlantilla').summernote('insertText', tag);

        return false;
    });
});