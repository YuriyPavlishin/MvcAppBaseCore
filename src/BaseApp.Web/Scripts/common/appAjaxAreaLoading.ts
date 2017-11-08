function appAjaxAreaLoading($area: JQuery, deffered: JQueryPromise<any>) {
    const fadeDuration = 250;
    const fadeColor = "white";
    if ($area.length <= 0)
        return;

    const tbodyPos = $area.position();
    let divOverlay = $("<div data-loading-overlay=''></div>");

    divOverlay.css({
        position: "absolute",
        "background-color": fadeColor,
        display: "none",
        top: tbodyPos.top,
        left: tbodyPos.left,
        width: $area.width() - 1,
        height: $area.height()
    });

    $area.before(divOverlay);
    divOverlay.fadeIn({ duration: fadeDuration });
    

    deffered.always(() => {
        divOverlay.fadeOut({
            duration: fadeDuration,
            complete: function () { divOverlay.remove(); }
        });
    });
}