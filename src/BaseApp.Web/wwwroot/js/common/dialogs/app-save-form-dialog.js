function appSaveFormDialog(fetchUrl, dialogSettings, saveSuccessCalback) {
    var mergeOptions = $.extend({
        title: "Dialog",
        saveBtnText: "Save changes",
        bodyHtml: "<p>Loading...</p>",
        dialogClass: "" //modal-sm, modal-lg
    }, dialogSettings);

    var dialog = appDialogShow({
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
                click: function () {
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
                        site.validators.refreshValidators(dialog.container);
                    });
                }
            }
        ]
    });

    $.get(fetchUrl, dialogSettings.fetchParams, function (data) {
        dialog.container.find(".modal-body").html(data);
        site.validators.refreshValidators(dialog);
    });
}