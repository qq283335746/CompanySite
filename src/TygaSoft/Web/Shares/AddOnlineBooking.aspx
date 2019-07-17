<%@ Page Title="网上预约" Language="C#" MasterPageFile="~/Shares/Share.Master" AutoEventWireup="true" CodeBehind="AddOnlineBooking.aspx.cs" Inherits="TygaSoft.Web.Shares.AddOnlineBooking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

  <script src="../../Scripts/JeasyuiExtend.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div id="banner" class="banner">
        <div class="w">
            <div class="bg_opacity">
                <span id="nav-current" class="nav_curr"></span>
            </div>
                
        </div>
    </div>
    <div class="w">
        <div class="bg_sl"></div>
        <div id="nav-hl" class="nav_hl">
            <span class="parentTitle"></span>
            <span class="split"></span>
            <span class="currTitle"></span>
        </div>
        <div class="bg_sr"></div>
        <span class="clr"></span>
    </div>

<div class="w_container">
    <div class="editorContent">
        <asp:Literal ID="ltrMyData" runat="server"></asp:Literal>
    </div>
    
    <div id="onlineBook">
        <div class="sl">
            <div class="title">
                客户基本信息：
            </div>
            <div class="row mt10">
                <span class="rl">客户名称：</span>
                <div class="fl">
                    <input id="txtCustomerName" class="easyui-textbox stxt" data-options="required:true" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">客户类型：</span>
                <div class="fl">
                    <asp:RadioButtonList runat="server" ID="rbtnListCustomerType" ClientIDMode="Static" CssClass="rbtnList" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">固定电话：</span>
                <div class="fl">
                    <input id="txtTelPhone" class="easyui-textbox stxt" data-options="validType:'telPhone'" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">电子邮件：</span>
                <div class="fl">
                    <input id="txtEmail" class="easyui-textbox stxt" data-options="validType:'email'" />
                </div>
                <span class="clr"></span>
            </div>
        </div>
        <div class="sl">
            <div class="title">
                预约产品信息：
            </div>
            <div class="row mt10">
                <span class="rl">预约产品：</span>
                <div class="fl">
                    <input id="txtProduct" class="easyui-textbox stxt" data-options="required:true" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">预约金额：</span>
                <div class="fl">
                    <input id="txtPrice" class="easyui-textbox stxt" data-options="required:true,validType:'price'" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">手&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;机：</span>
                <div class="fl">
                    <input id="txtPhone" class="easyui-textbox stxt" data-options="required:true,validType:'phone'" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">传&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;真：</span>
                <div class="fl">
                    <input id="txtFax" class="easyui-textbox stxt" />
                </div>
                <span class="clr"></span>
            </div>
        </div>
        <span class="clr"></span>
        <div class="row mt10">
            <span class="rl">通讯地址：</span>
            <div class="fl" style="width:480px;">
                <input id="txtAddress" class="easyui-textbox" style="width:100%;" />
            </div>
        </div>
        <div class="btns">
            <a class="easyui-linkbutton" data-options="width:77" onclick="AddOnlineBooking.OnSave()">提 交</a>
            <a class="easyui-linkbutton" data-options="width:77" onclick="AddOnlineBooking.OnReset()">重 填</a>
        </div>

        <div style="margin-bottom:50px;">&nbsp;</div>
    </div>
</div>

    <script type="text/javascript" src="/sw/Scripts/Shares/Hkbzg.js"></script>
    <script type="text/javascript" src="/sw/Scripts/Shares/AddOnlineBooking.js"></script>
    <script type="text/javascript">
        $(function () {
            Hkbzg.Init();
            AddOnlineBooking.Init();
        })
    </script>

</asp:Content>
