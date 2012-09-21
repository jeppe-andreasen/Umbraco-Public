<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Handler.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.MultiTreeList.Handler" %>
<%@ Register src="~/handlers/Controls/MultiListControl.ascx" tagname="MultiListControl" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
            
        body 
        {
            font-family:Tahoma;
            font-size:11.5px;
        }    
            
            
        /* Hides from IE-mac \*/
        * html .clear {
	        height: 1%;
        }
        .clear {
	        display: block;
        }
        /* End hide from IE-mac */

        ul.treeview, ul.listbox
        {
	        list-style:none;
	        padding:0;
	        margin:0;
	        display:block;
	        cursor:default;
        }

        ul.treeview ul 
        {
	        padding-left:20px;
        }

        ul.treeview li, ul.listbox li
        {
	        list-style:none;
	        line-height:20px;
	        padding:0;
	        margin:0;
        }

        ul.treeview li span
        {
	        padding-left:4px;
        }

        ul.treeview a.toggler
        {
	        display:block;
	        float:left;
	        background:url('/sitecore modules/Shell/TreePicker/gfx/expander.png');
	        background-position: left top;
	        width:16px;
	        height:16px;
	        padding:0;
	        margin-top:2px;
	
        }

        ul.treeview a.node, ul.listbox a.item
        {
	        position:relative;
	        text-decoration:none;
	        vertical-align:baseline;
	        padding:2px 5px 3px 16px;
	        margin:0;
        }

        ul.listbox a.item {
	        padding-left:20px;
        }

        ul.treeview a.node img, ul.listbox a.item img
        {
	        position:absolute;
	        padding:0;
	        margin:0;
	        vertical-align:middle;
	        border:0;
	        left:2px;
	        top:2px;
        }

        ul.treeview a.selected, ul.listbox a.selected
        {
	        background:blue;
	        color:White;
	        border-left:1px solid #99DEFD;
	        border-right:1px solid #99DEFD;
        }

        ul.treeview a.toggler.expanded
        {
	        background-position: 15px top;
        }
        ul.treeview a.toggler.disabled
        {
	        background-image:none;
	        background:white;
	        cursor:default;
        }

        .left{
	        float:left;
	        border:1px solid #333;
	        padding:5px;
        }

        .buttons
        {
	        float:left; 
	        margin:5px;
	        padding-top:10px;
        }
        .buttons img
        {
	        margin-bottom:3px;
        }

        div.box
        {
	        float:left;
	        width:40%;
        }
        div.box h3
        {
	        color:Gray;
	        margin:0;
	        font-family:Tahoma;
	        font-size:11.5px;
	        font-weight:normal;
	        padding:0 0 2px 2px;
        }
        div.frame
        {
	        height:160px;
	        border:1px solid #ccc;
	        padding:5px;
	        background:#fff;
	        overflow:auto;
        }

        #readonlyText span
        {
	        padding: 0 0 0 5px;
            vertical-align: top;
        }

        #popup
        {
	        display:none;	
	        float:left;
	        height:150px;
	        overflow:auto;
	        padding:5px 5px 10px 5px;
	        margin-left:5px;
	        background:white; 
	        border:1px solid #ccc;
        }

        legend {
	        display:none;
        }

    </style>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/assets/js/versus-ajax.js" type="text/javascript"></script>
    <script src="/assets/js/IFramedEditorFunctions.js" type="text/javascript"></script>
    <script src="/assets/js/multitreelisteditor.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="editor" class="multitreelist-editor" runat="server">
            <uc1:MultiListControl ID="multiListControl" runat="server" />
        </div>
    </div>
    <script type="text/javascript">
        multiTreeListEditor.init();
    </script>
    </form>
</body>
</html>

