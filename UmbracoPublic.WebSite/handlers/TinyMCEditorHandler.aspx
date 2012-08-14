<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TinyMCEditorHandler.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.TinyMCEditorHandler" %>
<%@ Register assembly="LinqIt.UmbracoCustomFieldTypes" namespace="LinqIt.UmbracoCustomFieldTypes.Components" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body 
        {
            margin:0;
            padding:0;
        }
        .tinymceditor 
        {
            width:200px;
        }
    </style>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function initializeEditorValue(ed, doc, iframeId) {
            var content = window.parent.$("#" + iframeId).val();
            ed.setContent(content);
        }

        function updateEditorValue(ed, doc, iframeId) {
            window.parent.$("#" + iframeId).val(ed.getContent());
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <cc1:TinyMCEditor ID="TinyMCEditor1" runat="server"></cc1:TinyMCEditor>
    </div>
    <script type="text/javascript">
        $(function () {
            window.parent.parent.$(".umbraco").width(755);
            window.parent.parent.$("iframe").width(750);
            //window.parent.$("#Content_iframe").width(720);
        });
    </script>
    </form>
</body>
</html>
