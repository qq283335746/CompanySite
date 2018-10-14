
var Login = {
    Init: function () {
        this.InitBanner();
    },
    InitBanner: function () {
        var myData = eval("(" + $("#myDataAppend").html() + ")");
        $("#banner").css({ "background-image": "url('" + myData.banner + "')" });

        var navitems = $("#SitePaths>span");
        $("#nav-current").text(navitems.filter(":first").text());
        var lastItem = navitems.filter(":last");
        $("#nav-hl .currTitle").text(lastItem.text()).prev().text("/").prev().text(lastItem.prev().prev().text());
    }
}