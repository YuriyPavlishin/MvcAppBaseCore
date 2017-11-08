function appConfirmDialog(message: string, onClose: (isConfirmed: boolean) => void, options: appConfirmDialogOptions) {
    var onCloseSafe = (isConfirmed: boolean) => {
        if (onClose) {
            onClose(isConfirmed || false);
        }
    };

    var dialog = appDialogShow({
        title: options.title,
        body: message,
        close: onCloseSafe,
        dialogClass: options.dialogClass,
        buttons: [
            {
                text: options.cancelText,
                buttonClass: 'btn-default',
                click: function () {
                    dialog.close();
                    onCloseSafe(false);
                }
            },
            {
                text: options.okText,
                buttonClass: 'btn-primary',
                click: function () {
                    dialog.close();
                    onCloseSafe(true);
                }
            }
        ]
    });
}

class appConfirmDialogOptions {
    dialogClass = "";
    title = "";
    okText = "OK";
    cancelText = "Cancel";
}