﻿
var AddAnnouncement = {
    Init: function () {
        this.CbbContentType("txtParent");
    },
    CbbContentType: function (clientId) {
        var cbt = $("#" + clientId + "");
        var parentId = $.trim(cbt.combotree("getValue"));
        var t = cbt.combotree('tree');
        $.ajax({
            url: "/sw/ScriptServices/AdminService.asmx/GetTreeJsonByParentCode",
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: '{typeCode:"HkbzgRecommend"}',
            dataType: "json",
            success: function (data) {
                var jsonData = (new Function("", "return " + data.d))();
                cbt.combotree('loadData', jsonData);
                if (parentId != "") {
                    var node = t.tree("find", parentId);
                    if (node) {
                        t.tree('select', node.target);
                        cbt.combotree("setValue", node.text);
                    }
                }
                else {
                    var root = t.tree('getRoot');
                    if (root) {
                        t.tree('select', root.target);
                        cbt.combotree("setValue", root.text);
                    }
                }
            }
        });
    },
    OnSave: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#form1').form('submit', {
                url: '/h/taboutsite.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    param.reqName = "SaveAnnouncement";
                    param.txtContent = editor_content.html().replace(/</g, "&lt;");
                    var contentTypeId = "";
                    var t = $("#txtParent").combotree('tree');
                    var node = t.tree("getSelected");
                    if (node) {
                        contentTypeId = node.id;
                    }
                    param.contentTypeId = contentTypeId;
                    if ($("#hId").val() != "") {
                        $("#txtContent").text("");
                    }
                    param.isDisable = $("#rbtnlIsDisable").find("[type=radio]:checked").val();

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    jeasyuiFun.show("温馨提醒", data.message);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
} 