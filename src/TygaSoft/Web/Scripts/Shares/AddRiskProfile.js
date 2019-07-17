
var AddRiskProfile = {
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
    },
    OnCommit: function () {
        var rows = $("#questionList .row");
        var xmlAppend = "";
        rows.each(function () {
            var answer = $(this).find(":radio:checked").val();
            if (answer != undefined) {
                xmlAppend += "<Xel><Title>" + $.trim($(this).find(".title").text()) + "</Title><Answer>" + answer + "</Answer></Xel>";
            }
        })
        if (xmlAppend == "") {
            $.messager.alert('错误提醒', "请答题后再提交！", 'error');
            return false;
        }
        xmlAppend = "<Root>"+xmlAppend+"</Root>";
        var url = "/sw/Services/AjaxService.svc/SaveRiskTestQuestionAnswer";
        var dataParms = '{ "username": "","questionId": "' + $("#hId").val() + '", "xmlAnswer": "' + xmlAppend + '" }';

        $.ajax({
            url: url,
            type: "post",
            contentType: "text/json",
            data: dataParms,
            beforeSend: function () {
                $.messager.progress({ title: '请稍等', msg: '正在执行...' });
            },
            complete: function () {
                $.messager.progress('close');
            },
            success: function (result) {
                if (result.ResCode != 1000) {
                    $.messager.alert('提示', result.Msg, 'info');
                    return false;
                }
                $.messager.show({ title: '温馨提示', msg: '恭喜您，操作成功！', showType: 'slide',
                    style: {
                        right: '',
                        top: document.body.scrollTop + document.documentElement.scrollTop,
                        bottom: ''
                    }
                });
            }
        });
    }
}