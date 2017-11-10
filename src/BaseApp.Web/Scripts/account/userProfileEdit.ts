class userProfileEdit {
    private uiElements = {
        container: null as JQuery
    };
    private readonly changePasswordUrl: string;

    constructor(container: JQuery | string, changePasswordUrl: string) {
        this.uiElements.container = $(container);
        this.changePasswordUrl = changePasswordUrl;

        this.uiElements.container.on("click", ".changePasswordUrl", () => {
            this.changePasswordClick();
        });

        this.uiElements.container.on("click", "input[type='submit']", (e) => {
            e.preventDefault();

            var $form = $('form', this.uiElements.container);

            if (!$form.valid())
                return;

            var params = $form.serialize();
            var postUrl = $form.attr('action');
            $.post(postUrl, params).done((data) => {
                this.uiElements.container.html(data);
                site.validators.refreshValidators(this.uiElements.container);
            });
        });
    }

    private changePasswordClick() {
        const dialogSettings = new appSaveFormDialogOptions();
        dialogSettings.title = "Change Password";
        dialogSettings.saveBtnText = "Change Password";

        site.dialogs.saveFormDialog(this.changePasswordUrl, dialogSettings);
    }
}