<%@ Page Title="" Language="C#" MasterPageFile="~/handlers/Dialogs/MasterDialog.Master" AutoEventWireup="true" CodeBehind="CreateModuleDialog.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.Dialogs.CreateModuleDialog" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhForm" runat="server">
    <div class="fields">
        <div class="field">
            <asp:Label ID="lblName" runat="server" Text="Name" AssociatedControlID="txtName" CssClass="label"></asp:Label>
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        </div>
    </div>
</asp:Content>
