/// <reference path="appAjaxSetup.ts" />
/// <reference path="appUtils.ts" />
/// <reference path="appValidators.ts" />
/// <reference path="dialogs/appDialogs.ts" />
declare var bah: any;
class appSite {

    private readonly appAjaxSetup: appAjaxSetup;
    readonly validators: appValidators;
    readonly dialogs: appDialogs;
    readonly utils: appUtils;

    constructor() {
        _.templateSettings.variable = "model";

        this.appAjaxSetup = new appAjaxSetup();
        this.validators = new appValidators();
        this.dialogs = new appDialogs();
        this.utils = new appUtils();
        //bah.tach();
        $(document).ready(() => {
            site.init();
        });
    }

    init() {
        appShowScriptMessages();
        new appControlAutocomplete().init();
        this.utils.autofocus();
    }
}

var site = new appSite();