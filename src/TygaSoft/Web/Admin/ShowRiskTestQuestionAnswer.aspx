<%@ Page Title="查看风险测试题答题结果" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ShowRiskTestQuestionAnswer.aspx.cs" Inherits="TygaSoft.Web.Admin.ShowRiskTestQuestionAnswer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<asp:Literal ID="ltrMyData" runat="server"></asp:Literal>

<input type="hidden" id="hQuestionId" runat="server" clientidmode="Static" />
<input type="hidden" id="hAnswerQuestionId" runat="server" clientidmode="Static" />

<script type="text/javascript" src="/sw/Scripts/Admin/ShowRiskTestQuestionAnswer.js"></script>
<script type="text/javascript">
    $(function () {
        ShowRiskTestQuestionAnswer.Init();
    })
</script>

</asp:Content>
