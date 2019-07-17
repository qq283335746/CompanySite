var AddOnlineBooking = {
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
    OnSave: function () {
        try {
            var isValid = $('#form1').form('validate');
            if (!isValid) return false;

            var customerName = $("#txtCustomerName").textbox('getValue');
            var clientType = $("[name$=rbtnListCustomerType]").filter(":checked").val();
            if (clientType == undefined) {
                $.messager.alert('错误提醒', "请选择客户类型", 'error');
                return false;
            }
            
            var telPhone = $("#txtTelPhone").textbox('getValue');
            var mobilePhone = $("#txtCustomerName").textbox('getValue');
            var fax = $("#txtFax").textbox('getValue');
            var email = $("#txtEmail").textbox('getValue');
            var address = $("#txtAddress").textbox('getValue');
            var bookProduct = $("#txtProduct").textbox('getValue');
            var price = $("#txtPrice").textbox('getValue');

            $.messager.progress({ title: '请稍等', msg: '正在执行...' });

            var url = "/sw/Services/AjaxService.svc/SaveProductOnlineBook"
            var dataParms = '{ "customerName": "' + customerName + '", "clientType": "' + clientType + '", "telPhone": "' + telPhone + '", "mobilePhone": "' + mobilePhone + '", "fax": "' + fax + '", "email": "' + email + '", "address": "' + address + '", "bookProduct": "' + bookProduct + '", "price": "' + price + '" }';

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
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    OnReset: function () {

    }
}