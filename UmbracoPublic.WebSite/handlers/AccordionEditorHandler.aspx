<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccordionEditorHandler.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.AccordionEditorHandler" ClientIDMode="Predictable" %>
<%@ Register Assembly="LinqIt.Components" Namespace="LinqIt.Components" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/assets/js/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <script src="/assets/js/jquery.alerts.js" type="text/javascript"></script>
    <script src="/assets/js/versus-ajax.js" type="text/javascript"></script>
    <script src="/assets/js/treeview.js" type="text/javascript"></script>
    <script src="/assets/js/IFramedEditorFunctions.js" type="text/javascript"></script>

    <link href="/assets/css/ui-darkness/jquery-ui-1.8.20.custom.css" rel="stylesheet" type="text/css">
    <link href="/assets/css/jquery.alerts.css" rel="stylesheet" type="text/css">
    <link href="/assets/css/accordioneditor.css" rel="stylesheet" type="text/css">

    <style type="text/css">
        html,body 
        {
            width:100%;
            margin:0;
            padding:0;
            font-family:Tahoma;
            font-size:11.5px;
        }
        fieldset
        {
            border:0;
            padding:0 3px 0 0;
        }
        fieldset .field
        {
            margin-bottom:10px;
        }
        fieldset input[type=text]
        {
            width:100%;
        }
        fieldset textarea 
        {
            width:100%;
        }
        #moduleInfo span, #moduleInfo a 
        {
            margin-right:15px;
        }
        #moduleInfo span
        {
            font-weight:bold;
        }
        .btn 
        {
            margin-top:8px;
            text-decoration:none;
	        color: #ffffff;
	        padding: 2px 6px;
	        background: -moz-linear-gradient(
		        top,
		        #19a3ff 0%,
		        #0044ab);
	        background: -webkit-gradient(
		        linear, left top, left bottom, 
		        from(#19a3ff),
		        to(#0044ab));
	        -moz-border-radius: 3px;
	        -webkit-border-radius: 3px;
	        border-radius: 3px;
	        border: 1px solid #383838;
	        -moz-box-shadow:
		        0px 1px 3px rgba(000,000,000,0.5),
		        inset 0px 0px 2px rgba(255,255,255,0.7);
	        -webkit-box-shadow:
		        0px 1px 3px rgba(000,000,000,0.5),
		        inset 0px 0px 2px rgba(255,255,255,0.7);
	        box-shadow:
		        0px 1px 3px rgba(000,000,000,0.5),
		        inset 0px 0px 2px rgba(255,255,255,0.7);
	        text-shadow:
		        0px -1px 0px rgba(000,000,000,0.4),
		        0px 1px 0px rgba(255,255,255,0.3);
        }
        a.disabled 
        {
            color:Gray;
        }


    </style>
    <script type="text/javascript">
        function addChild() {
            try {
                var treeview = $('.linqit-treeview', '#tree-panel');
                var selectedValue = linqit.treeview.getSelectedValue(treeview);
                var referenceId = treeview.attr('data-referenceId');
                var addedId = addAccordionChild(selectedValue, referenceId);
                linqit.treeview.refreshNode(treeview, selectedValue);
                linqit.treeview.selectNode(treeview, addedId, true);
            }
            catch (exception) {
                alert(exception.Message);
            }
            return false;
        }

        function addSibling(btn) {
            if ($(btn).hasClass('disabled'))
                return false;
            try {
                var treeview = $('.linqit-treeview', '#tree-panel');
                var selectedValue = linqit.treeview.getSelectedValue(treeview);
                var referenceId = treeview.attr('data-referenceId');
                var data = addAccordionSibling(selectedValue, referenceId);
                linqit.treeview.refreshNode(treeview, data.parentId);
                linqit.treeview.selectNode(treeview, data.addedId, true);
            }
            catch (exception) {
                alert(exception.Message);
            }
            return false;
        }


        function sendItemValues() {
            var treeview = $('.linqit-treeview', '#tree-panel');
            var itemId = linqit.treeview.getSelectedValue(treeview);
            var headline = $('#txtHeadline').val();
            var content = $('#txtContent').val();
            var modulePicker = $('.linqit-treeview', '#fields-panel');
            var moduleId = linqit.treeview.getSelectedValue(modulePicker);
            var referenceId = treeview.attr('data-referenceId');
            var updatedValue = updateValues(referenceId, itemId, headline, content, moduleId);
            pushValueToIframe($('#hiddenId').val(), updatedValue);
        }

        function getItemValues() {
            var treeview = $('.linqit-treeview', '#tree-panel');
            var itemId = linqit.treeview.getSelectedValue(treeview);
            var referenceId = treeview.attr('data-referenceId');
            var data = getValues(referenceId, itemId);

            var fieldset = $('fieldset', '#fields-panel');
            if (data.isRoot) {
                fieldset.hide();
            }
            else {
                fieldset.show();

                var txtHeadline = $('#txtHeadline');
                txtHeadline.val(data.headline);
                txtHeadline.focus();
                txtHeadline.select();
                
                $('#txtContent').val(data.content);

                $('#moduleInfo').show();
                $('#moduleTree').hide();

                if (data.moduleId != undefined) {
                    $('#moduleInfo').html('<span class="module-label">' + data.moduleName + '</span><a href="#" onclick="showModuleTree(\'' + data.moduleId + '\'); return false;">Choose</a><a href="#" onclick="clearModule(); return false;">Clear</a>');
                }
                else {
                    $('#moduleInfo').html('<a href="#" onclick="showModuleTree(); return false;">Choose</a>');
                }

                //var modulePicker = $('.linqit-treeview', '#fields-panel');
                //linqit.treeview.selectNode(modulePicker, data.moduleId, true);
            }

            $("#addChild").removeClass('disabled');

            // Update Buttons
            if (data.isRoot) {
                $('#btnRemove').addClass('disabled');
                $('#addSibling').addClass('disabled');
            }
            else {
                $('#btnRemove').removeClass('disabled');
                $('#addSibling').removeClass('disabled');
            }
            if (data.canMoveUp)
                $('#btnMoveUp').removeClass('disabled');
            else
                $('#btnMoveUp').addClass('disabled');
            if (data.canMoveDown)
                $('#btnMoveDown').removeClass('disabled');
            else
                $('#btnMoveDown').addClass('disabled');
        }

        function showModuleTree(selectedValue) {
            var modulePicker = $('.linqit-treeview', '#fields-panel');
            linqit.treeview.selectNode(modulePicker, selectedValue, true);
            $('#moduleInfo').hide();
            $('#moduleTree').show();
        }

        function clearModule() {
            var modulePicker = $('.linqit-treeview', '#fields-panel');
            linqit.treeview.selectNode(modulePicker, undefined, true);
            sendItemValues();
            getItemValues();
        }

        function removeItem(btn) {
            if ($(btn).hasClass('disabled'))
                return false;

            var treeview = $('.linqit-treeview', '#tree-panel');
            var itemId = linqit.treeview.getSelectedValue(treeview);
            var referenceId = treeview.attr('data-referenceId');
            var response = removeAccordionItem(referenceId, itemId);
            pushValueToIframe($('#hiddenId').val(), response.updatedValue);
            linqit.treeview.refreshNode(treeview, response.parentId);
            linqit.treeview.selectNode(treeview, response.parentId, true);
        }

        function moveItem(btn, step) {
            if ($(btn).hasClass('disabled'))
                return false;

            var treeview = $('.linqit-treeview', '#tree-panel');
            var itemId = linqit.treeview.getSelectedValue(treeview);
            var referenceId = treeview.attr('data-referenceId');

            var response = moveAccordionItem(referenceId, itemId, step);
            pushValueToIframe($('#hiddenId').val(), response.updatedValue);

            linqit.treeview.refreshNode(treeview, response.parentId);
            linqit.treeview.selectNode(treeview, itemId, true);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div id="accordion-editor" class="clearfix">
            <div id="tree-panel" style="border:1px solid #ccc; width:250px;height:300px;padding:10px;overflow:auto;float:left;">
                <cc1:LinqItTreeView ID="treeview" runat="server"></cc1:LinqItTreeView>
            </div>
            <div id="fields-panel" style="border:1px solid #ccc; width:500px;height:300px;padding:10px;overflow:auto;float:left;border-left:0;">
                <fieldset style="display:none;">
                    <div class="field">
                        <asp:Label ID="lblHeadline" runat="server" Text="Headline" AssociatedControlID="txtHeadline"></asp:Label><br />
                        <asp:TextBox ID="txtHeadline" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="field">
                        <asp:Label ID="lblContent" runat="server" Text="Content" AssociatedControlID="txtContent"></asp:Label><br />
                        <asp:TextBox ID="txtContent" runat="server" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="field">
                        <asp:Label ID="lblModule" runat="server" Text="Module"></asp:Label><br />
                        <div id="moduleContent">
                            <div id="moduleInfo"></div>
                            <div id="moduleTree" style="padding:5px; overflow:auto; height:100px;border:1px solid #ccc;">
                                <cc1:LinqItTreeView ID="modulePicker" runat="server"></cc1:LinqItTreeView>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
        <div style="clear:both">&nbsp;</div>
        <a id="addChild" class="btn disabled" href="#" onclick="addChild(); return false;">Add Child</a>
        <a id="addSibling" class="btn disabled" href="#" onclick="addSibling(this); return false;">Add Sibling</a>
        <a id="btnRemove" class="btn disabled" href="#" onclick="removeItem(this); return false;">Remove Item</a>
        <a id="btnMoveUp" class="btn disabled" href="#" onclick="moveItem(this,-1); return false;">Move Up</a>
        <a id="btnMoveDown" class="btn disabled" href="#" onclick="moveItem(this,1); return false;">Move Down</a>
        <asp:HiddenField ID="hiddenReference" runat="server" />
        <asp:HiddenField ID="hiddenId" runat="server" ClientIDMode="Static" />
    </div>
    <script type="text/javascript">
        var treeview = $('.linqit-treeview', '#tree-panel');

        treeview.bind('nodeUnselected', function (event, node) {
            /*
            var itemId = $(node).attr('ref');
            var headline = $('#txtHeadline').val();
            var content = $('#txtContent').val();
            var moduleId = '';
            var treeview = $('.linqit-treeview', '#tree-panel');
            var referenceId = treeview.attr('data-referenceId');
            updateValues(referenceId, itemId, headline, content, moduleId);
            */
        });
        treeview.bind('nodeSelected', function (event, node) {
            getItemValues();
        });

        var tvModules = $('.linqit-treeview', '#fields-panel');
        tvModules.bind('nodeSelected', function (event, node) {
            sendItemValues();
            getItemValues();
        });

        $("#txtHeadline").blur(function () {
            var headline = $(this).val();
            $('.linqit-treeview .selected span', '#tree-panel').html(headline);
            sendItemValues();
        });

        $("#txtContent").blur(function () {
            sendItemValues();
        });



        
    </script>
    </form>
</body>
</html>