function appAlertDialog(message: string, onClose: () => void, options: appAlertDialogOptions) {
    var dialog = appDialogShow({
        title: options.title,
        body: message,
        close: onClose,
        dialogClass: options.dialogClass,
        buttons: [
            {
                text: options.okText,
                buttonClass: 'btn-default',
                click() {
                    dialog.close();
                    if (onClose) {
                        onClose();
                    }
                }
            }
        ]
    });
}

class appAlertDialogOptions {
    dialogClass = "";
    title = "";
    okText = "OK";
}