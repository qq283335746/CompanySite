
var AddRiskTestQuestion = {
    Init: function () {

    },
    OnCommit: function () {
        var isRight = false;
        var xmlAppend = "";
        var error = "";
        var trArr = $("#dynamicT>tbody>tr");
        trArr.each(function () {
            var currTr = $(this);
            var txtaArr = currTr.find("textarea");
            txtaArr.each(function (i, item) {
                if ($(item).val().indexOf("#") > -1) {
                    error = "检测到输入带有”#“符号，不合法，请去掉”#“符号";
                    return false;
                }
            })
            var qTitle = $.trim(txtaArr.eq(0).val());
            if (qTitle != "") {
                var currSelectOption = $.trim(txtaArr.eq(1).val());
                if (currSelectOption == "") {
                    error = "问题标题不为空字符串时，则问题选项为必填项";
                    return false;
                }
                var optionAppend = "";
                currSelectOption = currSelectOption.replace(/(\s+){2,}/g, "#");
                var selectOptionArr = currSelectOption.split("#");
                var optionIndex = 0;
                for (var i = 0; i < selectOptionArr.length; i++) {
                    if ($.trim(selectOptionArr[i]) != "") {
                        if (optionIndex > 0) optionAppend += "#";
                        optionAppend += selectOptionArr[i];
                        optionIndex++;
                    }
                }
                var answerAppend = $.trim(txtaArr.eq(2).val());
                var layout = currTr.find("[name=ddlLayout]").val();

                xmlAppend += "<Xel>";
                xmlAppend += "<Title><![CDATA[" + qTitle + "]]></Title>";
                xmlAppend += "<Option><![CDATA[" + optionAppend + "]]></Option>";
                xmlAppend += "<Answer><![CDATA[" + answerAppend + "]]></Answer>";
                xmlAppend += "<Layout>" + layout + "</Layout>";
                xmlAppend += "</Xel>";
            }
        })
        if (error != "") {
            $.messager.alert('错误提醒', error, 'error');
            return false;
        }
        if (xmlAppend == "") {
            $.messager.alert('错误提醒', "无任何可提交的数据，请检查", 'error');
            return false;
        }
        xmlAppend = "<Root>" + xmlAppend + "</Root>";
        xmlAppend = xmlAppend.replace(/</g, "&lt;");

        $.ajax({
            url: "/sw/ScriptServices/AdminService.asmx/SaveRiskTestQuestion",
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: '{"xml":"' + xmlAppend + '","Id":"' + $("#hId").val() + '"}',
            beforeSend: function () {
                $.messager.progress({ title: '请稍等', msg: '正在执行...' });
            },
            complete: function () {
                $.messager.progress('close');
            },
            success: function (result) {
                var json = eval("(" + result.d + ")");
                if (!json.success) {
                    $.messager.alert('系统提示', json.message, 'info');
                    return false;
                }
                $("#hId").val(json.data);
                $.messager.show({
                    title: "温馨提示", msg: "操作成功", showType: 'slide',
                    style: {
                        right: '', top: document.body.scrollTop + document.documentElement.scrollTop, bottom: ''
                    }
                });
            }
        });
    },
    AddInput: function () {
        var newRow = $("#dynamicT>tbody>tr:first").clone();
        newRow.find("textarea").text("");
        $("#dynamicT>tbody").append(newRow);
    },
    UpInput: function (t) {
        var $_curr = $(t).parent().parent();
        var $_prev = $_curr.prev();
        $_curr.after($_prev);
    },
    DownInput: function (t) {
        var $_curr = $(t).parent().parent();
        var $_next = $_curr.next();
        $_curr.before($_next);
    },
    DelInput: function (t) {
        var row = $(t).parent().parent();
        if (row.siblings().length == 0) {
            $.messager.alert('错误提醒', '已经是最后一行，无法再移除', 'error');
            return false;
        }
        $(t).parent().parent().remove();
    }
}