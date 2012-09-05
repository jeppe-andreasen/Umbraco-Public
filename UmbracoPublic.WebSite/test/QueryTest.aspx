<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryTest.aspx.cs" Inherits="UmbracoPublic.WebSite.test.QueryTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtQuery" runat="server"></asp:TextBox><asp:Button ID="btnQuery" runat="server" OnClick="OnQueryClicked" Text="Try" />
        <p>
            <asp:Literal ID="litOutput" runat="server"></asp:Literal>
        </p>
    </div>
    </form>
</body>
</html>
