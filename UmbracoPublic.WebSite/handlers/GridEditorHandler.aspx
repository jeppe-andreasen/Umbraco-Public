<%@ Page Language="C#" AutoEventWireup="true" Inherits="LinqIt.UmbracoCustomFieldTypes.GridEditorHandler, LinqIt.UmbracoCustomFieldTypes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/assets/js/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <script src="/assets/js/jquery.alerts.js" type="text/javascript"></script>
    <script src="/assets/js/versus-ajax.js" type="text/javascript"></script>
    <script src="/assets/js/grideditor.js" type="text/javascript"></script>
    <script src="/assets/js/treeview.js" type="text/javascript"></script>

    <link href="/assets/css/ui-darkness/jquery-ui-1.8.20.custom.css" rel="stylesheet" type="text/css">
    <link href="/assets/css/jquery.alerts.css" rel="stylesheet" type="text/css">
    <link href="/assets/css/grideditor.css" rel="stylesheet" type="text/css">

    <style type="text/css">
        html,body 
        {
            width:100%;
            margin:0;
            padding:0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <asp:PlaceHolder ID="plhContent" runat="server"></asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
