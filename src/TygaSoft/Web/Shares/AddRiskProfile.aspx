<%@ Page Title="风险测试" Language="C#" MasterPageFile="~/Shares/Share.Master" AutoEventWireup="true" CodeBehind="AddRiskProfile.aspx.cs" Inherits="TygaSoft.Web.Shares.AddRiskProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

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
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />

    <script type="text/javascript" src="/sw/Scripts/Shares/Hkbzg.js"></script>
    <script type="text/javascript" src="/sw/Scripts/Shares/AddRiskProfile.js"></script>
    <script type="text/javascript">
        $(function () {
            Hkbzg.Init();
            AddRiskProfile.Init();
        })
    </script>

</asp:Content>
