<%@ Page Title="网上预约列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListProductOnlineBook.aspx.cs" Inherits="TygaSoft.Web.Admin.ListProductOnlineBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div id="toolbar" style="padding:5px;">
        <input id="txtKeyword" class="easyui-textbox" data-options="onClickButton:ListProductOnlineBook.Search,buttonText:'查询',buttonIcon:'icon-search',prompt:'关键字'" style="width:250px;" />
        <a class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListProductOnlineBook.Del()">删 除</a>
    </div>

    <table id="dgT" class="easyui-datagrid" title="网上预约列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'f1',width:100">客户名称</th>
                <th data-options="field:'f2',width:80">客户类型</th>
                <th data-options="field:'f3',width:60">固定电话</th>
                <th data-options="field:'f4',width:70">手机</th>
                <th data-options="field:'f5',width:60">传真</th>
                <th data-options="field:'f6',width:100">电子邮件</th>
                <th data-options="field:'f7',width:100">通讯地址</th>
                <th data-options="field:'f8',width:100">预约产品</th>
                <th data-options="field:'f9',width:50">预约金额</th>
                <th data-options="field:'f10',width:100">最后更新时间</th>
           
            </tr>
        </thead>
        <tbody>
        <asp:Repeater ID="rpData" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%#Eval("Id")%></td>
                    <td><%#Eval("CustomerName")%></td>
                    <td><%#Eval("ClientType")%></td>
                    <td><%#Eval("TelPhone")%></td>
                    <td><%#Eval("MobilePhone")%></td>
                    <td><%#Eval("Fax")%></td>
                    <td><%#Eval("Email")%></td>
                    <td><%#Eval("Address")%></td>
                    <td><%#Eval("BookProduct")%></td>
                    <td><%#Eval("Price")%></td>
                    <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </tbody>
    </table>

    <asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

    <script type="text/javascript" src="/sw/Scripts/Admin/ListProductOnlineBook.js"></script>

    <script type="text/javascript">
        var sPageIndex = 0;
        var sPageSize = 0;
        var sTotalRecord = 0;
        var sQueryStr = "";

        $(function () {
            try {
                var myData = ListProductOnlineBook.GetMyData("myDataForPage");
                $.map(myData, function (item) {
                    sPageIndex = parseInt(item.PageIndex);
                    sPageSize = parseInt(item.PageSize);
                    sTotalRecord = parseInt(item.TotalRecord);
                    sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
                })

                ListProductOnlineBook.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })
    </script>

</asp:Content>
