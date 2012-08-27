<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateModule.aspx.cs" Inherits="UmbracoPublic.WebSite.test.CreateModule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblName" runat="server" Text="Name" AssociatedControlID="txtName"></asp:Label><br />
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        <hr />
        <asp:Repeater ID="repeater" runat="server">
            <HeaderTemplate>
                <table>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><asp:Literal ID="litName" runat="server" Text='<%# Eval("Key") %>'></asp:Literal></td>
                    <td><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Value") %>'></asp:Literal></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <hr />

        <asp:Label ID="lblParameterName" runat="server" Text="Parameter Name" AssociatedControlID="txtParameterName"></asp:Label>
        <asp:TextBox ID="txtParameterName" runat="server"></asp:TextBox>
        <asp:Label ID="lblParameterType" runat="server" Text="Parameter Type" AssociatedControlID="ddlParameterType"></asp:Label>
        
        <asp:DropDownList ID="ddlParameterType" runat="server"></asp:DropDownList>
        <asp:Button ID="btnAddParameter" runat="server" Text="Add Parameter" OnClick="OnAddParameterClicked" />



        <hr />
        <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="OnCreateClicked" />
    </div>
    </form>
</body>
</html>
