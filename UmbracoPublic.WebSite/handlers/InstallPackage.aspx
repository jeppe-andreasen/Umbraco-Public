<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstallPackage.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.InstallPackage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body 
        {
            font-family:Courier New;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <asp:MultiView ID="multiview" runat="server" ActiveViewIndex="0">
            <asp:View ID="defaultView" runat="server">
                <asp:Label ID="lblPickPackage" runat="server" Text="Choose a deployment package to install" AssociatedControlID="ulPackage"></asp:Label><br>
                <asp:FileUpload ID="ulPackage" runat="server" Width="500" size="80" /><br>
                <asp:Button ID="btnUpload" runat="server" Text="Install" OnClick="OnInstallClicked" />
            </asp:View>
            <asp:View ID="outputView" runat="server">
                <span>Log:</span><br>
                <div style="height:400px; overflow:auto; border:1px solid #ccc">
                    <asp:Literal ID="litOutput" runat="server"></asp:Literal>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
