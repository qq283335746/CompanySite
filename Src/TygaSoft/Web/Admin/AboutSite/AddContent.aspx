<%@ Page Title="新增菜单内容" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddContent.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.AddContent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-layout" data-options="fit:true">
    <div data-options="region:'west',title:'功能菜单',split:true" style="width:500px;">
        <ul id="treeCt" class="easyui-tree" style="padding-left: 5px; margin-top:10px;"></ul>
        <input type="hidden" id="hCurrExpandNode" value="" />
    </div>
    <div data-options="region:'center',title:'内容详情'" style="padding:5px;">
    </div>
</div>

</asp:Content>
