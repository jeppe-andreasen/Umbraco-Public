<%@ Page Title="" Language="C#" MasterPageFile="~/handlers/Dialogs/MasterDialog.Master" AutoEventWireup="true" CodeBehind="ShareModuleDialog.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.Dialogs.ShareModuleDialog" ClientIDMode="Predictable" %>
<%@ Register assembly="LinqIt.Components" namespace="LinqIt.Components" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhHead" runat="server">
    <script src="/assets/js/versus-ajax.js" type="text/javascript"></script>
    <script type="text/javascript">
        function createFolder() {
            var name = $('#txtFolder').val();
            var treeview = $('.linqit-treeview', '#treeview-container');
            var provider = treeview.attr("data-provider");
            var referenceId = treeview.attr("data-referenceId");
            var parentId = linqit.treeview.getSelectedValue(treeview);
            var response = createNewFolder(provider, referenceId, parentId, name);
            linqit.treeview.refreshNode(treeview, response.parentId);
            linqit.treeview.selectNode(treeview, response.addedId);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhForm" runat="server">
    <div class="fields">
        <div class="field">
            <asp:Label ID="lblTree" runat="server" Text="Pick folder"></asp:Label>
            <div id="treeview-container" style="border:1px solid #ccc; background:white;padding:5px; height:150px; margin-top:2px; overflow:auto;">
                <cc1:LinqItTreeView ID="treeview" runat="server"></cc1:LinqItTreeView>        
            </div>
        </div>
        <div class="field">
            <asp:Label ID="lblFolder" runat="server" Text="Create New Folder"></asp:Label><br />
            <asp:TextBox ID="txtFolder" runat="server" ClientIDMode="Static" style="float:left; margin-top:1px;width:70%;"></asp:TextBox>
            <asp:Button ID="btnCreateFolder" runat="server" Text="Create" OnClientClick="createFolder(); return false;" UseSubmitBehavior="false" style="float:right; width:24%;" />
        </div>
        <div style="clear:both;">&nbsp;</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhButtons" runat="server">
    
</asp:Content>
