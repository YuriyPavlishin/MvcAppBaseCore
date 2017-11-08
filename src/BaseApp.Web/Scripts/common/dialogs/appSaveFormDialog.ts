function appSaveFormDialog(fetchUrl: string, dialogOptions: appSaveFormDialogOptions, saveSuccessCalback: (result:string) => void = null) {
    var dialog = appDialogShow({
        title: dialogOptions.title,
        dialogClass: dialogOptions.dialogClass,
        body: dialogOptions.bodyHtml,
        buttons: [
            {
                text: "Close",
                buttonClass: 'btn-default',
                click() {
                    dialog.close();
                }
            },
            {
                text: dialogOptions.saveBtnText,
                buttonClass: "btn-primary",
                click() {
                    var $form = $('form', dialog.container);

                    if (!$form.valid())
                        return;

                    const params = $form.serialize();
                    const postUrl = $form.attr('action');
                    $.post(postUrl, params).done((data, textStatus, xhr) => {
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

    $.get(fetchUrl, dialogOptions.fetchParams, data => {
        dialog.container.find(".modal-body").html(data);
        site.validators.refreshValidators(dialog.container);
    });
}


class appSaveFormDialogOptions {
    fetchParams: any = null;
    title = "Dialog";
    saveBtnText = "Save changes";
    bodyHtml = "<p>Loading...</p>";
    dialogClass = "";//modal-sm, modal-lg
}