<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShareTop.ascx.cs" Inherits="TygaSoft.Web.WebUserControls.ShareTop" %>

<div class="w">
    <div class="h_top">
        <div class="fr">
            <a href="#">中文</a>
            <span class="mlr10">|</span>
            <a href="#">English</a>
        </div>
        <span class="clr"></span>
    </div>
    
    <div class="fl logo_hkb">
        <a href="/sw/">
            <img src="/sw/Images/sw-logo.jpg" alt="矽云科技" width="200" height="68" />
        </a>
    </div>
    <div class="fl h_menu">
        <asp:Menu runat="server" ID="shareMenu" Orientation="Horizontal" CssClass="shareMenu" 
            StaticEnableDefaultPopOutImage="False" StaticDisplayLevels="1" PathSeparator="|"
            MaximumDynamicDisplayLevels="2" ClientIDMode="Static" DataSourceID="smdsShare">
        </asp:Menu>
    </div>
    <span class="clr"></span>
</div>


<asp:SiteMapDataSource ID="smdsShare" runat="server" SiteMapProvider="ShareSiteMapProvider" ShowStartingNode="false" />