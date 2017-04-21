var SiteValidators = new function () {
    this.init = function () {
        var defaultOptions = {
            ignore: ":hidden:not([data-validatehidden])"
        };

        $.validator.setDefaults(defaultOptions);
    };

    this.refreshValidators = function (containerSelector) {
        var $forms = $(containerSelector);
        if (!$forms.is("form")) {
            $forms = $forms.find("form");
        }
        $forms.validateBootstrap(true);
    };
};