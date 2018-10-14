
var Hkbzg = {
    Init: function () {
        var winh = $(window).height();
        var ch = winh - 107 - 389 - 139 - 87;
        if (ch > 0) {
            $(".w_container").css("min-height", "" + ch + "px");
        }
    }
}