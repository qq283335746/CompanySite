
var ShareMaster = {
    Init: function () {
        this.InitTopMenu();
        this.Hover();
    },
    InitTopMenu: function () {
        var aArr = $("#shareMenu>ul>li");
        aArr.each(function () {
            $(this).after("<li class=\"lisp\"><span class=\"split\">|</span></li>");
        })
        aArr.filter(":last").next().remove();
    },
    Hover: function () {
        $("#shareMenu>ul>li").hover(function () {
            if (!$(this).hasClass("lisp")) {
                $(this).addClass("hover").siblings().removeClass("hover");
            }
        }, function () {
            $(this).removeClass("hover");
        })
        $("#shareMenu>ul>li>ul>li").hover(function () {
            $(this).parent().parent().addClass("unhover");
            $(this).addClass("hover");
        }, function () {
            $(this).removeClass("hover");
            $(this).parent().parent().removeClass("unhover");
        })
    }
}