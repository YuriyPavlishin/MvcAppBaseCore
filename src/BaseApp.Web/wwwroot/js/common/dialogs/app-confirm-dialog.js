function appConfirmDialog(message, onClose, options) {
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

    var dialog = appDialogShow({
        title: mergedOptions.title,
        body: message,
        close: onCloseSafe,
        dialogClass: mergedOptions.dialogClass,
        buttons: [
            {
                text: mergedOptions.cancelText,
                buttonClass: 'btn-default',
                click: function () {
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
}