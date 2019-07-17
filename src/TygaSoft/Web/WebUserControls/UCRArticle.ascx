<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCRArticle.ascx.cs" Inherits="TygaSoft.Web.WebUserControls.UCRArticle" %>

<div class="rowT">
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <div class="row">
                <div class="fl"><a href='/sw/s/t.html?Id=<%=Id %>&aId=<%#Eval("Id") %>'><%#Eval("Title") %></a></div>
                <div class="fr"><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy.MM.dd") %></div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            <div id="jPager" class="easyui-pagination" data-options="total:<%=totalRecords%>,pageSize:10,displayMsg:'{total}条 / <%=totalPages %> 页',layout:['first','prev','links','next','last'],
                onSelectPage: function(pageNumber, pageSize){
                    
            }"></div>
        </FooterTemplate>
    </asp:Repeater>
</div>