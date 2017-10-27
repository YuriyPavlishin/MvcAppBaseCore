var site = new appSite();

$(function () {
    site.init();
});

function appSite() {
    var self = this;

    new appAjaxSetup();

    this.validators = new appValidators();
    this.dialogs = new appDialogs();
    this.utils = new appUtils();

    this.init = function () {
        appShowScriptMessages();
        new appControlAutocomplete().init();
        self.utils.autofocus();
    };
};