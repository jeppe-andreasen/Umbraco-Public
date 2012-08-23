<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Handler.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.ImageGallery.Handler" %>
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
    <link href="/assets/css/imagegalleryeditor.css" rel="stylesheet" type="text/css">

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
        #imageInfo span, #imageInfo a 
        {
            margin-right:15px;
        }
        #imageInfo span
        {
            font-weight:bold;
        }
        #imageInfo img 
        {
            max-width:200px;
            max-height:100px;
            margin-top:10px;
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
                var result = addImageGalleryChild(selectedValue, referenceId);
                linqit.treeview.refreshNode(treeview, result.rootId);
                linqit.treeview.selectNode(treeview, result.addedId, true);
            }
            catch (exception) {
                alert(exception.Message);
            }
            return false;
        }

        function setValue(key, value) {
            
            var treeview = $('.linqit-treeview', '#tree-panel');
            var referenceId = treeview.attr('data-referenceId');
            var itemId = linqit.treeview.getSelectedValue(treeview);
            var data = {};
            data[key] = value;
            var result = setItemValue(referenceId, itemId, data);
            pushValueToIframe($('#hiddenId').val(), result.value);
            return result;
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

                var txtName = $('#txtName');
                txtName.val(data.name);
                txtName.focus();
                txtName.select();

                $('#txtHeadline').val(data.headline);
                $('#txtContent').val(data.content);

                updateImageView(data);
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

        function showImageTree(selectedValue) {
            var imagePicker = $('.linqit-treeview', '#fields-panel');
            linqit.treeview.selectNode(imagePicker, selectedValue, true);
            $('#imageInfo').hide();
            $('#imageTree').show();
        }

        function clearImage() {
            var result = setValue('imageId', "");
            updateImageView(result);
        }

        function removeItem(btn) {
            if ($(btn).hasClass('disabled'))
                return false;

            var treeview = $('.linqit-treeview', '#tree-panel');
            var itemId = linqit.treeview.getSelectedValue(treeview);
            var referenceId = treeview.attr('data-referenceId');
            var response = removeImageGalleryItem(referenceId, itemId);
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

            var response = moveImageGalleryItem(referenceId, itemId, step);
            pushValueToIframe($('#hiddenId').val(), response.updatedValue);

            linqit.treeview.refreshNode(treeview, response.parentId);
            linqit.treeview.selectNode(treeview, itemId, true);
        }

        function updateImageView(data) {
            $('#imageInfo').show();
            $('#imageTree').hide();
            if (data.imageId != undefined) {
                $('#imageInfo').html('<span class="image-label">' + data.imageName + '</span><a href="#" onclick="showImageTree(\'' + data.imageId + '\'); return false;">Choose</a><a href="#" onclick="clearImage(); return false;">Clear</a><br><img src="' + data.imageUrl + '">');
            }
            else {
                $('#imageInfo').html('<a href="#" onclick="showImageTree(); return false;">Choose</a>');
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div id="accordion-editor" class="clearfix">
            <div id="tree-panel" style="border:1px solid #ccc; width:250px;height:330px;padding:10px;overflow:auto;float:left;">
                <cc1:LinqItTreeView ID="treeview" runat="server"></cc1:LinqItTreeView>
            </div>
            <div id="fields-panel" style="border:1px solid #ccc; width:500px;height:330px;padding:10px;overflow:auto;float:left;border-left:0;">
                <fieldset style="display:none;">
                    <div class="field">
                        <asp:Label ID="lblName" runat="server" Text="Name" AssociatedControlID="txtName"></asp:Label><br />
                        <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="field">
                        <asp:Label ID="lblHeadline" runat="server" Text="Headline" AssociatedControlID="txtHeadline"></asp:Label><br />
                        <asp:TextBox ID="txtHeadline" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="field">
                        <asp:Label ID="lblContent" runat="server" Text="Content" AssociatedControlID="txtContent"></asp:Label><br />
                        <asp:TextBox ID="txtContent" runat="server" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="field">
                        <asp:Label ID="lblImage" runat="server" Text="Image"></asp:Label><br />
                        <div id="imageContent">
                            <div id="imageInfo"></div>
                            <div id="imageTree" style="padding:5px; overflow:auto; height:100px;border:1px solid #ccc;">
                                <cc1:LinqItTreeView ID="imagePicker" runat="server"></cc1:LinqItTreeView>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
        <div style="clear:both">&nbsp;</div>
        <a id="addChild" class="btn disabled" href="#" onclick="addChild(); return false;">Add Image</a>
        <a id="btnRemove" class="btn disabled" href="#" onclick="removeItem(this); return false;">Remove Image</a>
        <a id="btnMoveUp" class="btn disabled" href="#" onclick="moveItem(this,-1); return false;">Move Up</a>
        <a id="btnMoveDown" class="btn disabled" href="#" onclick="moveItem(this,1); return false;">Move Down</a>
        <asp:HiddenField ID="hiddenReference" runat="server" />
        <asp:HiddenField ID="hiddenId" runat="server" ClientIDMode="Static" />
    </div>
    <script type="text/javascript">
        var treeview = $('.linqit-treeview', '#tree-panel');

        treeview.bind('nodeUnselected', function (event, node) {
        });
        treeview.bind('nodeSelected', function (event, node) {
            getItemValues();
        });

        var tvImages = $('.linqit-treeview', '#fields-panel');
        tvImages.bind('nodeSelected', function (event, node) {
            var imageId = $(node).attr("ref");
            var result = setValue('imageId', imageId);
            updateImageView(result);
        });

        $("#txtName").blur(function () {
            var name = $(this).val();
            $('.linqit-treeview .selected span', '#tree-panel').html(name);
            setValue('name', name);
        });

        $("#txtHeadline").blur(function () {
            var headline = $(this).val();
            setValue('headline', headline);
        });

        $("#txtContent").blur(function () {
            var content = $(this).val();
            setValue('content', content);
        });
    </script>
    </form>
</body>
</html>