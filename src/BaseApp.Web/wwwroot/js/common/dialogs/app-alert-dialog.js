function appAlertDialog(message, onClose, options) {
    var mergedOptions = $.extend({
        dialogClass: '',
        title: '',
        okText: 'OK'
    }, options);

    var dialog = appDialogShow({
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
}