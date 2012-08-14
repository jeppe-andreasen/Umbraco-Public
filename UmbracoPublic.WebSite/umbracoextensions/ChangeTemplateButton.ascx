<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeTemplateButton.ascx.cs" Inherits="UmbracoPublic.WebSite.umbracoextensions.ChangeTemplateButton" %>
<script type="text/javascript">
    function changeDocumentType() {
        try {
            if (UmbClientMgr._mainTree._tree.selected == undefined) {
                alert("Please select a document");
                return false;
            }
            var id = UmbClientMgr._mainTree._tree.selected.attr("id");
            doChangeDocumentType(id);
            top.openContent(id); 
        }
        catch (err) {
            alert(err.message);
        }
        return false;
    }
</script>
<button onclick="changeDocumentType(); return false;" class="topBarButton"><img src="images/aboutNew.png" alt="about" /><span>Change Document Type</span></button>