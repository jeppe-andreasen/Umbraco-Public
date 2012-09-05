<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog.aspx.cs" Inherits="UmbracoPublic.WebSite.assets.lib.tiny_mce.plugins.linqitmacro.dialog" ClientIDMode="Predictable" validateRequest="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>{#linqitmacro_dlg.title}</title>
	<script type="text/javascript" src="../../tiny_mce_popup.js"></script>
	<script type="text/javascript" src="js/dialog.js"></script>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/assets/js/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function initializeValues() {
            $('#uiSelection').val($(tinyMCEPopup.editor.selection.getNode()).clone().wrap('<p>').parent().html());
            var theForm = document.forms[0];
            theForm.submit();
        }

        function submitValue() {
            var value = $("#uiValue").val();
            tinyMCEPopup.editor.execCommand('mceInsertContent', false, value);
            tinyMCEPopup.close();
        }
    </script>
    <link href="/assets/css/TinyMCEDialog.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server" action="#">
    <div>
        <div class="macro-editor" style="overflow:auto;">
            <asp:MultiView ID="mvItems" runat="server">
                <asp:View ID="vPicker" runat="server">
                    <asp:DropDownList ID="ddlMacroType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnMacroTypeSelected"></asp:DropDownList>
                </asp:View>
                <asp:View ID="vEditor" runat="server">
                    <asp:PlaceHolder ID="plhEditor" runat="server"></asp:PlaceHolder>
                    <div id="edit_buttons">
                        <p>
                            <asp:Button ID="btnOk" runat="server" Text="Ok" OnClick="OnOkClicked" />
                            <em>or </em>
                            <a onclick="tinyMCEPopup.close();" style="color: blue" href="#" id="cancelbtn">Cancel</a>
                        </p>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
        <asp:HiddenField ID="uiSelection" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="uiInitialized" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="uiValue" runat="server" ClientIDMode="Static" />
    </div>
    <script type="text/javascript">
        $(function () {
            var initialized = $("#uiInitialized").val() == 'true';
            if (!initialized)
                initializeValues();
        });
    </script>
    </form>
</body>
</html>