<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsSearchResultPart.ascx.cs" Inherits="UmbracoPublic.WebSite.usercontrols.Parts.NewsSearchResultPart" %>
<%@ Register src="Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register assembly="UmbracoPublic.Logic" namespace="UmbracoPublic.Logic.Parts.Paging" tagprefix="cc1" %>
<asp:Literal ID="litOutput" runat="server"></asp:Literal>
<cc1:Pager ID="pager" runat="server"></cc1:Pager>



