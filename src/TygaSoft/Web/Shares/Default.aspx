<%@ Page Title="首页" Language="C#" MasterPageFile="~/Shares/Share.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TygaSoft.Web.Shares.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <link href="/sw/Scripts/Plugins/unslider/unslider.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div id="img_split" class="default-slider">
        <ul>
            <li> 
                <a href="javascript:;">
                    <img src="" alt="" />
                </a>
            </li>
        </ul>
    </div>

<div id="editorContent" class="editorContent" style="display:none;">
    <asp:Literal ID="ltrMyData" runat="server"></asp:Literal>
</div>

<script type="text/javascript" src="/sw/Scripts/Shares/Default.js"></script>
<script type="text/javascript" src="/sw/Scripts/Plugins/unslider/unslider-min.js"></script>
<script type="text/javascript">
    $(function () {
        Default.Init();
    })
</script>

</asp:Content>
