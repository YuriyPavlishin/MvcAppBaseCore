function appAjaxAreaLoading($area, deffered) {
    var fadeDuration = 250;
    var fadeColor = "white";

    if ($area.length <= 0)
        return;

    var tbodyPos = $area.position();
    var divOverlay = $("<div data-loading-overlay=''></div>");

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

    deffered.always(function () {
        divOverlay.fadeOut({
            duration: fadeDuration,
            complete: function () { divOverlay.remove(); }
        });
    });
}