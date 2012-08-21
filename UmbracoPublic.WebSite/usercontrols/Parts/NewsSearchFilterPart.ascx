<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsSearchFilterPart.ascx.cs" Inherits="UmbracoPublic.WebSite.usercontrols.Parts.NewsSearchFilterPart" %>
<div class="well search-filter">
    <h3>Søg i nyheder</h3>
    <fieldset>
        <div class="control-group">
            <asp:Label ID="lblQuery" runat="server" AssociatedControlID="txtQuery" Text="Søg" CssClass="control-label"></asp:Label>
            <div class="controls">
                <asp:TextBox ID="txtQuery" runat="server" CssClass="span2" placeholder="Skriv søgeord"></asp:TextBox>
            </div>
            <asp:PlaceHolder ID="plhCategorizationFilters" runat="server"></asp:PlaceHolder>
            <asp:Label ID="lblFrom" runat="server" AssociatedControlID="txtFrom" Text="Fra dato" CssClass="control-label"></asp:Label>
            <div class="controls">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="span2 datepicker" data-date-format="dd-mm-yyyy"></asp:TextBox>
            </div>
            <asp:Label ID="lblTo" runat="server" AssociatedControlID="txtTo" Text="Til dato" CssClass="control-label"></asp:Label>
            <div class="controls">
                <asp:TextBox ID="txtTo" runat="server" CssClass="span2 datepicker" data-date-format="dd-mm-yyyy"></asp:TextBox>
            </div>
            <p>
                <asp:Button ID="btnSearch" runat="server" Text="Søg" CssClass="btn btn-primary" OnClick="OnSearchClicked" />
            </p>
        </div>
    </fieldset>
</div>