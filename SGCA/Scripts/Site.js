function bloqueiaTela() {

    $.blockUI({
        message: '<h2><img src="../Content/Images/load.gif" /></br>Carregando, por favor aguarde...</h2>',
        css: {
            border: 'none', padding: '15px', backgroundColor: '#000', '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px', opacity: .5, color: '#fff'
        }
    });
}

function desbloqueiaTela() {

    $.unblockUI();

}

function limpaFormulario(formulario) {
    formulario.trigger("reset");
    formulario.each(function () {
        $(this).find(':input')
            .not(':button, :submit, :reset, :hidden, :checkbox, :radio')
            .val(null);
        $(this).find(':checkbox, :radio').prop('checked', false);
    });
}

function ConfirmaResetForm(elemento, formulario, mensagem, lblSim, lblNao) {
    elemento.on('click', function () {

        alertify.confirm()
          .set({
              'labels': {
                  ok: lblSim,
                  cancel: lblNao
              },
              'message': mensagem,
              'onok': function () { limpaFormulario(formulario); }
          }).show();

        return false;
    });
}

function buscaPageLength() {
    
    var pageLength;

    $.ajax({
        type: 'POST',
        url: '/DataTable/BuscaQtdItensPaginas',
        async: false,
        success: function (response) {
            pageLength = response;
        }
    });

    return pageLength;
}

//jQuery.fn.extend({

//    brTelMask: function () {

//        return this.each(function () {

//            var el = $(this);

//            $(el).focus(function () {
//                $(el).mask('(99) 9999-9999?9');
//            });

//            $(el).focusout(function () {
//                var phone, element;
//                element = $(el);
//                element.unmask();
//                phone = element.val().replace(/\D/g, '');
//                if (phone.length > 10) {
//                    element.mask('(99) 99999-999?9');
//                } else {
//                    element.mask('(99) 9999-9999?9');
//                }
//            });
//        });
//    }
//});

jQuery.fn.brTelMask = function () {

    return this.each(function () {
        var el = this;
        $(el).focus(function () {
            $(el).mask('(99) 9999-9999?9');
        });

        $(el).focusout(function () {
            var phone, element;
            element = $(el);
            element.unmask();
            phone = element.val().replace(/\D/g, '');
            if (phone.length > 10) {
                element.mask('(99) 99999-999?9');
            } else {
                element.mask('(99) 9999-9999?9');
            }
        });
    });
}

jQuery.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
