<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCRTrend.ascx.cs" Inherits="TygaSoft.Web.WebUserControls.UCRTrend" %>

<div class="rowT">
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <div class="row">
                <div class="fl"><a href='/sw/s/t.html?Id=<%=Id %>&aId=<%#Eval("Id") %>'><%#Eval("Title") %></a></div>
                <div class="fr"><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy.MM.dd") %></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>