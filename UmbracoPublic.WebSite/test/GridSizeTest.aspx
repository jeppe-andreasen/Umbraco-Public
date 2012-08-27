<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridSizeTest.aspx.cs" Inherits="UmbracoPublic.WebSite.test.GridSizeTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body
        {
            padding: 0;
            margin: 0;
            width:100%;
        }
    </style>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/assets/css/grideditor.css" rel="stylesheet" type="text/css">
</head>
<body>
    <form id="form1" runat="server">
        <table id="grid-editor" data-provider="LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider, LinqIt.UmbracoCustomFieldTypes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" class="clearfix">
            <tr style="width:100%;">
                <td style="background:cyan;">
                    <div id="treeview-container">
                    <ul id="treeview" data-provider="LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider, LinqIt.UmbracoCustomFieldTypes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" data-item="" class="linqit-treeview">
                        <li id="node_1068"><a href="#" class="toggler"><span></span></a><a ref="1068" title="Moduler" path="" selectable="true" href="#" class="node draggable">
                            <img src="/umbraco/images/umbraco/folder.gif" /><span>Moduler</span></a>
                        </li>
                    </ul>
                    </div>
                    &nbsp;
                </td>
                <td style="background:orange;">
                    <div id="module-container">
                        <div id="dropcontainers">
                            <table id="grid-table" style="width:100%; background:purple;">
                                <tr>
                                    <td colspan="2">
                                        <span class="containerlabel">Top</span><div ref="top" cols="12" class="dropcontainer" style="height: 65px;">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 75%">
                                        <span class="containerlabel">Content</span><div ref="content" cols="9" class="dropcontainer"
                                            style="height: 65px;">
                                        </div>
                                    </td>
                                    <td style="width: 25%">
                                        <span class="containerlabel">Right</span><div ref="right" cols="3" class="dropcontainer" style="height: 65px;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="slider-area">
                            Column Span:<div id="slider-container" class="clearfix">
                         </div>
                        </div>
                    </div>
                    &nbsp;
                </td>
            </tr>
        </table>
                
    </form>
</body>
</html>
