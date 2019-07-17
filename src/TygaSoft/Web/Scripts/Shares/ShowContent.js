var ShowContent = {
    Init: function () {
        this.InitData();
        this.InitBanner();
    },
    InitEvent: function () {
        
    },
    InitData: function () {
        if ($("#jPager").length > 0) {
            var t = $("#jPager>table");
            $("#jPager>.pagination-info").insertBefore(t);
        }
        if (window.location.href.indexOf("&aId") > -1) $(".editorContent").hide();
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