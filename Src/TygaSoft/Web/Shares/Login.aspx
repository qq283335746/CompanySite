<%@ Page Title="客服中心/客户登录" Language="C#" MasterPageFile="~/Shares/Share.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TygaSoft.Web.Shares.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

  <script src="/sw/Scripts/JeasyuiExtend.js" type="text/javascript"></script>

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
        <div id="login_hkb">
            <div class="row">
                <span class="rl">&nbsp;</span>
                <div class="fl">
                    <asp:RadioButtonList runat="server" ID="rbtnList" CssClass="rbtnList" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">证件类型：</span>
                <div class="fl">
                    <asp:DropDownList ID="ddlIdType" runat="server" CssClass="txt textbox-invalid select_m"></asp:DropDownList>
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">证件号码：</span>
                <div class="fl">
                    <input id="txtIdNum" class="easyui-textbox txt" data-options="required:true,validType:'idNum'" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">登录密码：</span>
                <div class="fl">
                    <input id="txtPassword" class="easyui-textbox txt" data-options="required:true" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="rl">验 证 码：</span>
                <div class="fl">
                    <input id="txtValidCode" class="easyui-textbox" data-options="required:true,width:158" />
                    <img border="0" id="imgCode" src="/sw/Handlers/ValidateCode.ashx?vcType=rndCode" alt="看不清，单击换一张" onclick="this.src='/sw/Handlers/ValidateCode.ashx?vcType=rndCode&abc='+Math.random()" style="height:20px;" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt30 login_btns">
                <a class="easyui-linkbutton" data-options="width:77">登 录</a>
                <a class="easyui-linkbutton" data-options="width:77">注 册</a>
                <a href="#" style="text-decoration:underline;">忘记密码？</a>
            </div>
            <div style="margin-bottom:50px;">&nbsp;</div>
        </div>

        <asp:Literal ID="ltrMyData" runat="server"></asp:Literal>
    </div>

    <script type="text/javascript" src="/sw/Scripts/Shares/Hkbzg.js"></script>
    <script type="text/javascript" src="/sw/Scripts/Shares/Login.js"></script>
    <script type="text/javascript">
        $(function () {
            Hkbzg.Init();
            Login.Init();
        })
    </script>

</asp:Content>
