var ProductAgreement = {
    Init: function () {
        this.InitEvent();
        this.InitData();
    },
    InitEvent: function () {
        $("#abtnCommit").click(function () {
            $.get("/sw/h/tp.html?action=ProductAgreement", {}, function (result) {
                if (!result.success) {
                    $.messager.alert('系统提示', "系统繁忙，请稍后重试", 'info');
                    return false;
                }
                window.location = "/sw/s/a.html";
            }, 'json');
        });
        $("#abtnCancel").click(function () {
            $.messager.alert('系统提示', "请阅读并接受产品与服务的协议", 'error');
        });
    },
    InitData: function () {
        $("#footer").hide();

        var h = $(window).height() - 107;
        $("#agreementContainer").css({ "width": "" + $(window).width() + "", "height": "" + h + "" });
        var vh = ($("#agreementContainer").height() - 653) / 2;
        if (vh < 1) vh = 0;
        $("#agreementBox").css({ "margin-top": "" + vh + "px" });
    }
}