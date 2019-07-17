<%@ Page Title="推荐列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListAnnouncement.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.ListAnnouncement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar" style="padding:5px;">
    标题：<input type="text" runat="server" id="txtKeyword" maxlength="50" class="txt" />
    所属类型：<input id="cbtParentType" runat="server" clientidmode="Static" class="easyui-combotree" style="width:200px;" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListAnnouncement.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListAnnouncement.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListAnnouncement.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListAnnouncement.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="内容列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:300">标题</th>
            <th data-options="field:'f2',width:150">所属类型</th>
            <th data-options="field:'f3',width:300">描述说明</th>
            <th data-options="field:'f4',width:100,hidden:true">访问量设置</th>
            <th data-options="field:'f5',width:100">访问量</th>
            <th data-options="field:'f6',width:60">排序</th>
            <th data-options="field:'f7',width:80">是否禁用</th>
            <th data-options="field:'f8',width:120">最后更新时间</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Title")%></td>
                <td><%#Eval("ContentTypeName")%></td>
                <td><%#Eval("Descr").ToString().Length > 20 ? Eval("Descr").ToString().Substring(0, 20) + "......" : Eval("Descr")%></td>
                <td><%#Eval("VirtualViewCount")%></td>
                <td><%#Eval("ViewCount")%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#Eval("IsDisable").ToString() == "True" ? "是":"否"%></td>
                 <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<div id="dlg"></div>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<script type="text/javascript" src="../../Scripts/Admin/AboutSite/ListAnnouncement.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListAnnouncement.GetMyData("myDataForPage");
            sPageIndex = parseInt(myData.PageIndex);
            sPageSize = parseInt(myData.PageSize);
            sTotalRecord = parseInt(myData.TotalRecord);
            sQueryStr = myData.QueryStr.replace(/&amp;/g, '&');

            ListAnnouncement.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
        
    })
</script>

</asp:Content>
