class appValidators {
    constructor() {
        const defaultOptions: JQueryValidation.ValidationOptions = {
            ignore: ":hidden:not([data-validatehidden])"
        };
        $.validator.setDefaults(defaultOptions);
    }

    refreshValidators(containerSelector: JQuery | string) {
        var $forms = $(containerSelector);
        if (!$forms.is("form")) {
            $forms = $forms.find("form");
        }
        
        ($forms as any).validateBootstrap(true);
    }
}