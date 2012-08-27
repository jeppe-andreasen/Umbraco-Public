<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiListTest.aspx.cs" Inherits="UmbracoPublic.WebSite.test.MultiListTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <iframe id="frame" frameborder="0" height="200" width="800" scrolling="auto" src="/handlers/MultiListEditorHandler.aspx?itemId=1248&provider=UmbracoPublic.Logic.Providers.SubjectProvider,UmbracoPublic.Logic&fieldname=subjects&frame=frame&hiddenId=hiddenId"></iframe>
        <input type="text" id="hiddenId">
    </div>
    </form>
</body>
</html>
