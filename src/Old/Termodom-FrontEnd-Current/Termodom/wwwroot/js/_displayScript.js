function OrganizeThumbnails() {
    $(".thumbnail-wrapper img").each(function () {
        var el = $(this);
        if ($(this).outerWidth() > $(this).parent().outerWidth()) {
            var futureWidth = $(this).parent().outerWidth();

            $(this).css("height", "auto");
            $(this).css("width", futureWidth + "px");
            var futureMarginTop = ($(this).parent().outerHeight() - $(this).outerHeight()) / 2;
            $(this).css("margin-top", futureMarginTop + "px");
        }
        else if ($(this).outerWidth() < $(this).parent().outerWidth()) {
            var futureMarginLeft = ($(this).parent().outerWidth() - $(this).outerWidth()) / 2;
            $(this).css("margin-left", futureMarginLeft + "px");
        }

        setTimeout(function () {
            $(el).css("opacity", "1");
        }, 50);
    });
}