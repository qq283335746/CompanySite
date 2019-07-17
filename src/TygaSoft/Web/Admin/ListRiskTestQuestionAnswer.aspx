<%@ Page Title="风险测试答题结果列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListRiskTestQuestionAnswer.aspx.cs" Inherits="TygaSoft.Web.Admin.ListRiskTestQuestionAnswer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar" style="padding:5px;">
        起始时间：<input id="txtStartDate" class="easyui-datebox" />
        结束时间：<input id="txtEndDate" class="easyui-datebox" />
        <a class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListRiskTestQuestionAnswer.Search()">查 询</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListRiskTestQuestionAnswer.Del()">删 除</a>
    </div>

    <table id="dgT" class="easyui-datagrid" title="风险测试答题结果列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'f1',width:100">作答结果</th>
                <th data-options="field:'f2',width:80">作答时间</th>
            </tr>
        </thead>
        <tbody>
        <asp:Repeater ID="rpData" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%#Eval("Id") %></td>
                    <td>
                        <a target="_blank" href='/a/tya.html?Id=<%#Eval("Id") %>'>查看</a>
                    </td>
                    <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </tbody>
    </table>

    <asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

    <script type="text/javascript" src="/sw/Scripts/Admin/ListRiskTestQuestionAnswer.js"></script>

    <script type="text/javascript">
        var sPageIndex = 0;
        var sPageSize = 0;
        var sTotalRecord = 0;
        var sQueryStr = "";

        $(function () {
            try {
                var myData = ListRiskTestQuestionAnswer.GetMyData("myDataForPage");
                $.map(myData, function (item) {
                    sPageIndex = parseInt(item.PageIndex);
                    sPageSize = parseInt(item.PageSize);
                    sTotalRecord = parseInt(item.TotalRecord);
                    sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
                })

                ListRiskTestQuestionAnswer.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })
    </script>

</asp:Content>
