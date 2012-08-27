<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameTest.aspx.cs" Inherits="UmbracoPublic.WebSite.test.FrameTest" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/assets/js/jquery-v1.7.1.js" type="text/javascript"></script>
    <title></title>
    <style type="text/css">
        html, body 
        {
            width:100%;
            padding:0;
            margin:0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <iframe id="frame" frameborder="1" style="border:1px solid red; width:100%;" src="TableTest.aspx"></iframe>
    </div>
    </form>
</body>
</html>
