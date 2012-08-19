<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuildPackage.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.BuildPackage" ClientIDMode="Predictable" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/assets/js/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <script src="/assets/lib/JQueryUI/datetimepicker.js" type="text/javascript"></script>
    <link href="/assets/css/ui-darkness/jquery-ui-1.8.20.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
            .ui-timepicker-div .ui-widget-header { margin-bottom: 8px; }
            .ui-timepicker-div dl { text-align: left; }
            .ui-timepicker-div dl dt { height: 25px; margin-bottom: -25px; }
            .ui-timepicker-div dl dd { margin: 0 10px 10px 65px; }
            .ui-timepicker-div td { font-size: 90%; }
            .ui-tpicker-grid-label { background: none; border: none; margin: 0; padding: 0; }
            
            body { font-family:Tahoma; font-size:11.5px; }
            h1 { font-size:12.5px; }
            .fieldsection { margin-bottom:15px; }
            
            a 
            {
                color:Black;
            }
            a.active 
            {
                color:Black;
            }
            
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("div[id $= TreeView1] input[type=checkbox]").click(function () {
                $(this).closest("table").next("div").find("input[type=checkbox]").attr("checked", this.checked);
            });
            $('div[id $= TreeView1] a[class^="TreeView"]').each(function () {
                $(this).removeAttr("onclick");
                $(this).attr("href", "#");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Generate Deployment Package</h1>
        <div class="fieldsection">
        <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="txtDate" Text="From Date:"></asp:Label><br>
        <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static"></asp:TextBox>
        </div>
        <div class="fieldsection">
            <asp:Label ID="lblExtensions" runat="server" AssociatedControlID="cblExtensions" Text="Accepted Extensions:"></asp:Label><br>
            <div style="width:200px; height:150px; overflow:auto; border:1px solid #ccc;">
                <asp:CheckBoxList ID="cblExtensions" runat="server"></asp:CheckBoxList>                 
            </div>
        </div>
        <div class="fieldsection">
            <asp:Button ID="btnGeneratePreview" runat="server" Text="Get Snapshot" OnClick="OnGetSnapshotClicked" />
        </div>
        <div class="fieldsection">
            <asp:Label ID="lblItems" runat="server" AssociatedControlID="TreeView1" Text="Items">
                <div style="height:250px; overflow:auto; border:1px solid #ccc; padding:10px;">
                    <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="Root">
                    </asp:TreeView>
                </div>
            </asp:Label>
        </div>
        <div class="fieldsection">
            <asp:Button ID="btnGeneratePackage" runat="server" Text="Generate Package" OnClick="OnGeneratePackageClicked" />
        </div>
        <asp:Literal ID="litTEst" runat="server"></asp:Literal>
    </div>
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datetimepicker({
                dateFormat: 'dd.mm.yy',
                timeFormat: 'hh:mm',
                separator: ' '
            });
        });
    </script>
    </form>
</body>
</html>
