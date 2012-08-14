<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsListModuleRendering.ascx.cs" Inherits="UmbracoPublic.WebSite.modules.NewsListModuleRendering, UmbracoPublic.WebSite" %>
<%@ Register src="../usercontrols/Parts/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<asp:Literal ID="litOutput" runat="server"></asp:Literal>
<uc1:Pager ID="pager" runat="server" />

