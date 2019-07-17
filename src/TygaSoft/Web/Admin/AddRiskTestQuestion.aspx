<%@ Page Title="风险测试" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddRiskTestQuestion.aspx.cs" Inherits="TygaSoft.Web.Admin.AddRiskTestQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" data-options="fit:true,border:false">

    <div>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddRiskTestQuestion.AddInput()">添加一行</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="AddRiskTestQuestion.OnCommit()">提 交</a>
    </div>
    <div style="color:#C0C0C0; padding:15px;">使用说明：问题选项区，不能输入符号“#”，且区分多个问题选项请连续敲入至少两个空格</div>

    <table id="dynamicT" class="dynamicT">
        <thead>
            <tr>
                <th style="width:25%; text-align:center;">问题题目</th>
                <th style="width:40%; text-align:center;">问题选项</th>
                <th style="width:18%; text-align:center;">问题答案</th>
                <th style="width:7%; text-align:center;">布局方式</th>
                <th style="width:7%; text-align:center;">操作</th>
            </tr>
        </thead>
        <tbody>
            <asp:Literal ID="ltrTr" runat="server"></asp:Literal>
            
        </tbody>
    </table>

    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
</div>

<script type="text/javascript" src="/sw/Scripts/Admin/AddRiskTestQuestion.js"></script>
<script type="text/javascript">
    $(function () {
        AddRiskTestQuestion.Init();
    })
</script>

</asp:Content>
