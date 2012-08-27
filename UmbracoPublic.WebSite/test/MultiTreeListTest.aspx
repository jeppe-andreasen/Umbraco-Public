<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiTreeListTest.aspx.cs" Inherits="UmbracoPublic.WebSite.test.MultiTreeListTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <iframe id="frame" frameborder="0" height="200" width="800" scrolling="auto" src="/handlers/MultiTreeList/Handler.aspx?itemId=1248&provider=LinqIt.UmbracoCustomFieldTypes.ModuleTreeNodeProvider, LinqIt.UmbracoCustomFieldTypes&fieldname=categorizations&frame=frame&hiddenId=hiddenId"></iframe>
        <input type="text" id="hiddenId">
    </div>
    </form>
</body>
</html>