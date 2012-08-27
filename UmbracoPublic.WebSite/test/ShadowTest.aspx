<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShadowTest.aspx.cs" Inherits="UmbracoPublic.WebSite.test.ShadowTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .module
        {
            border:1px solid #b7b7b7;
            border-radius: 4px 4px 4px 4px;
            -moz-box-shadow: 0 0 2px 2px rgba(0,0,0,0.15);
            -webkit-box-shadow: 0 0 2px 2px rgba(0,0,0,0.15);
            box-shadow: 0 0 2px 2px rgba(0,0,0,0.15);
        }
        .module h3
        {
            background-color:Yellow;
            margin:0;
            border-radius: 4px 4px 0px 0px;
        }
        .module p 
        {
            text-align:center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="module">
            <h3>Hello</h3>
            <p>3</p>
        </div>
    </div>
    </form>
</body>
</html>
