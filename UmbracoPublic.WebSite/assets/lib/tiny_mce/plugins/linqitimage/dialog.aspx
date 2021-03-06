﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog.aspx.cs" Inherits="UmbracoPublic.WebSite.assets.lib.tiny_mce.plugins.linqitimage.dialog" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>{#linqitlink_dlg.title}</title>
	<script type="text/javascript" src="../../tiny_mce_popup.js"></script>
	<script type="text/javascript" src="js/dialog.js"></script>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/assets/js/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" action="#">
    <div>
        <p>Insert Link.</p>
        <asp:PlaceHolder ID="plhEditor" runat="server"></asp:PlaceHolder>
    </div>
    <script type="text/javascript">
        $(function () {
            var $e = $('.linqit-imageeditor');

            var value = $(tinyMCEPopup.editor.selection.getNode()).clone().wrap('<p>').parent().html();
            if (!$(value).is('img'))
                value = '<img type="internal">' + tinyMCEPopup.editor.selection.getContent({ format: 'text' }) + "</img>";


            linqit.imageeditor.setValue($e, value);
            linqit.imageeditor.showInput($e);

            $('body').bind('valueChanged', $e, function (event, editor, value) {
                tinyMCEPopup.editor.execCommand('mceInsertContent', false, value);
                tinyMCEPopup.close();
            });
            $('body').bind('updateCancelled', $e, function (event, editor, value) {
                tinyMCEPopup.close();
            });
        });
    </script>
    </form>
</body>
</html>
