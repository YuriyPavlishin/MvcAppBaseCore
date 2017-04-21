var SiteDialog = new function () {
    var modalTemplate = _.template(
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

    this.show = function(options) {
        options = $.extend({
            dialogClass: '',
            title: '',
            body: '',
            close: null, //function () { },
            buttons: [] // array of { text: '', buttonClass: '', click: <function> }
        }, options);

        if (options.buttons) {
            for (var i = 0; i < options.buttons.length; i++) {
                options.buttons[i].buttonId = ("button_" + Math.random()).replace(".", "");
            }
        }

        var htmlText = modalTemplate(options);
        var $dialog = $(htmlText);

        $dialog.modal('show');

        if (options.buttons) {
            for (var i = 0; i < options.buttons.length; i++) {
                if (options.buttons[i].click) {
                    $("input[data-sitedialog-button-id='" + options.buttons[i].buttonId + "']", $dialog).click(options.buttons[i].click);
                }
            }
        }

        $dialog.on('hidden.bs.modal', function () {
            $dialog.remove();
            if (options.close && !$dialog.skipClose) {
                options.close();
            }
        });

        return {
            container: $dialog,
            close: function () {
                $dialog.skipClose = true;
                $dialog.modal("hide");
            }
        };
    };

    this.saveFormDialog = function (fetchUrl, dialogSettings, saveSuccessCalback) {
        var mergeOptions = $.extend({
            title: "Dialog",
            saveBtnText: "Save changes",
            bodyHtml: "<p>Loading...</p>",
            dialogClass: "" //modal-sm, modal-lg
        }, dialogSettings);

        var dialog = SiteDialog.show({
            title: mergeOptions.title,
            dialogClass: mergeOptions.dialogClass,
            body: mergeOptions.bodyHtml,
            buttons: [
                {
                    text: "Close",
                    buttonClass: 'btn-default',
                    click: function () {
                        dialog.close();
                    }
                },
                {
                    text: mergeOptions.saveBtnText,
                    buttonClass: "btn-primary",
                    click: function() {
                        var $form = $('form', dialog.container);

                        if (!$form.valid())
                            return;

                        var params = $form.serialize();
                        var postUrl = $form.attr('action');
                        $.post(postUrl, params).done(function (data, textStatus, xhr) {
                            if (xhr.getResponseHeader('CloseDialog') === "1") {
                                if (saveSuccessCalback) {
                                    saveSuccessCalback(xhr.getResponseHeader('CloseDialogResult'));
                                }
                                dialog.close();
                                return;
                            }

                            dialog.container.find(".modal-body").html(data);
                            SiteValidators.refreshValidators(dialog.container);
                        });
                    }
                }
            ]
        });

        $.get(fetchUrl, dialogSettings.fetchParams, function(data) {
            dialog.container.find(".modal-body").html(data);
            SiteValidators.refreshValidators(dialog);
        });
    };

    this.alert = function(message, onClose, options) {
        var mergedOptions = $.extend({
            dialogClass: '',
            title: '',
            okText: 'OK',
        }, options);

        var dialog = SiteDialog.show({
            title: mergedOptions.title,
            body: message,
            close: onClose,
            dialogClass: mergedOptions.dialogClass,
            buttons: [
                {
                    text: mergedOptions.okText,
                    buttonClass: 'btn-default',
                    click: function () {
                        dialog.close();
                        if (onClose) {
                            onClose();
                        }
                    }
                }
            ]
        });
    };

    this.confirm = function (message, onClose, options) {
        var mergedOptions = $.extend({
            dialogClass: '',
            title: '',
            okText: 'OK',
            cancelText: 'Cancel'
        }, options);

        var onCloseSafe = function (isConfirmed) {
            if (onClose) {
                onClose(isConfirmed || false);
            }
        };

        var dialog = SiteDialog.show({
            title: mergedOptions.title,
            body: message,
            close: onCloseSafe,
            dialogClass: mergedOptions.dialogClass,
            buttons: [
                {
                    text: mergedOptions.cancelText,
                    buttonClass: 'btn-default',
                    click: function() {
                        dialog.close();
                        onCloseSafe(false);
                    }
                },
                 {
                     text: mergedOptions.okText,
                     buttonClass: 'btn-primary',
                     click: function () {
                         dialog.close();
                         onCloseSafe(true);
                     }
                 }
            ]
        });
    };
};