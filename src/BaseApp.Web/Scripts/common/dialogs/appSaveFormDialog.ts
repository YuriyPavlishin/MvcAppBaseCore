function appSaveFormDialog(fetchUrl: string, dialogOptions: appSaveFormDialogOptions) {
    const defSaveFormDialog = $.Deferred();

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
                    const $form = $('form', dialog.container);
                    if (!$form.valid())
                        return;

                    const postUrl = $form.attr('action');
                    let request: JQueryXHR;

                    if ($form.attr("enctype") === "multipart/form-data") {
                        const formElem = $form[0] as HTMLFormElement;
                        const formdata = new FormData(formElem);

                        request = $.ajax({
                            type: "POST",
                            url: postUrl,
                            data: formdata,
                            processData: false,
                            contentType: false,
                        });
                    } else {
                        const params = $form.serialize();
                        request = $.post(postUrl, params);
                    }

                    request.done((data, textStatus, xhr) => {
                        if (xhr.getResponseHeader('CloseDialog') === "1") {
                            defSaveFormDialog.resolve(xhr.getResponseHeader('CloseDialogResult'));
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

    return defSaveFormDialog.promise();
}


class appSaveFormDialogOptions {
    fetchParams: any = null;
    title = "Dialog";
    saveBtnText = "Save changes";
    bodyHtml = "<p>Loading...</p>";
    dialogClass = "";//modal-sm, modal-lg
}