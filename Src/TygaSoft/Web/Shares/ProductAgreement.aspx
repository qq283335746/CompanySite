<%@ Page Title="产品与服务协议" Language="C#" MasterPageFile="~/Shares/Share.Master" AutoEventWireup="true" CodeBehind="ProductAgreement.aspx.cs" Inherits="TygaSoft.Web.Shares.ProductAgreement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div class="lh">
        <div id="agreementContainer" runat="server" clientidmode="Static" class="bg_agreement">
            <div id="agreementBox" class="agreementBox">
                <div class="lh">
                    <asp:Literal runat="server" ID="ltrEditorContent"></asp:Literal>
                </div>
                <div class="btns">
                    <a id="abtnCommit" class="easyui-linkbutton fl" data-options="width:77">接 受</a>
                    <a id="abtnCancel" class="easyui-linkbutton fr" data-options="width:77">拒 绝</a>
                </div>
            </div>
        </div>
    </div>
        
    <script src="/sw/Scripts/Shares/ProductAgreement.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            ProductAgreement.Init();
                
        })
    </script>

</asp:Content>
