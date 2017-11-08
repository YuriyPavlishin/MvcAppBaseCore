function appDialogShow(options:any) {
    const modalTemplate = _.template(
        '<div class="modal fade">' +
            '<div class="modal-dialog <%=model.dialogClass%>">'
                + '<div class="modal-content">'

                    + "<% if(model.title) { %>"
                        + '<div class="modal-header">'
                            + '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>'
                            + '<h4 class="modal-title"><%= model.title %></h4>'
                        + '</div>'
                    + "<% } %>"

                    + '<div class="modal-body"><%- model.body %></div>'

                    + '<% if(model.buttons && model.buttons.length > 0) { %>'
                        + '<div class="modal-footer">'
                            + '<% for(var i = 0; i < model.buttons.length; i++) {%>'
                                + '<input type="button" data-sitedialog-button-id="<%= model.buttons[i].buttonId %>" class="btn <%= model.buttons[i].buttonClass %>" value="<%=model.buttons[i].text%>"/>'
                            + '<% } %>'
                        + '</div>'
                    + '<% } %>'

                + '</div>'
            + '</div>'
        + '</div>');

    
    options = $.extend({
            dialogClass: '',
            title: '',
            body: '',
            close: null, //function () { },
            buttons: [] // array of { text: '', buttonClass: '', click: <function> }
        }, options);

    if (options.buttons) {
        for (let i = 0; i < options.buttons.length; i++) {
            options.buttons[i].buttonId = (`button_${Math.random()}`).replace(".", "");
        }
    }

    const htmlText = modalTemplate(options);
    var $dialog = $(htmlText);
    let skipClose = false;

    $dialog.modal('show');

    if (options.buttons) {
        for (let i = 0; i < options.buttons.length; i++) {
            if (options.buttons[i].click) {
                $(`input[data-sitedialog-button-id='${options.buttons[i].buttonId}']`, $dialog).click(options.buttons[i].click);
            }
        }
    }

    $dialog.on('hidden.bs.modal', () => {
        $dialog.remove();
        if (options.close && !skipClose) {
            options.close();
        }
    });

    return {
        container: $dialog,
        close() {
            skipClose = true;
            $dialog.modal("hide");
        }
    };
}