<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableTest.aspx.cs" Inherits="UmbracoPublic.WebSite.test.TableTest" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/assets/css/treeview.css" rel="stylesheet" type="text/css">
    <style type="text/css">
        html,body 
        {
            width:100%;
            margin:0;
            padding:0;
        }
        .clearfix:after {
	        content: ".";
	        display: block;
	        clear: both;
	        visibility: hidden;
	        line-height: 0;
	        height: 0;
        }
 
        .clearfix {
	        display: inline-block;
        }
 
        html[xmlns] .clearfix {
	        display: block;
        }
 
        * html .clearfix {
	        height: 1%;
        }
        #grid-editor { width:100%;position:relative; border:1px solid #ccc; }
        #outer-left { float:left; width:20%; }
        #outer-right { float:left; width:80%; }
        #treeview-container { overflow:auto; white-space:nowrap; min-height:200px; padding:5px 5px 85px 5px; border-right:1px solid #ccc; }
        #colspan-container { position:absolute; height:80px; width:20%; left:0;bottom:0; }
        #colspan-outer { padding-right:1px; }
        #colspan-inner { height:80px; border-top:1px solid #ccc; padding:5px; }
        #coloptions .coloption { float:left; margin-right:10px; }
        .pad5 { padding:5px; }
        .span1, .span2, .span3, .span4, .span5, .span6, .span7, .span8, .span9, .span10, .span11, .span12
        {
            float:left;
            padding:0;
        }
        
        .span1 { width:8.33%; }
        .span2 { width:16.66%; }
        .span3 { width:25%; }
        .span4 { width:33.33%; }
        .span5 { width:41.66%; }
        .span6 { width:50%; }
        .span7 { width:58.33%; }
        .span8 { width:66.66% }
        .span9 { width:75%; }
        .span10 { width:83.33%; }
        .span11 { width:91.66%; }
        .span12 { width:100%; }
        
        .dropcontainer 
        {
            height:60px;
            border:1px solid #ccc;
            border-radius:4px;
            margin:0 5px 5px 5px;
        }
        .containerlabel 
        {
            margin:0 5px 3px 5px;
            display:block;
        }
        
        
    </style>
    
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <div id="grid-editor" class="clearfix" >
            <div id="outer-left">
                <div id="treeview-container">
                    <ul class="linqit-treeview" data-item="" data-provider="LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider, LinqIt.UmbracoCustomFieldTypes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" runat="server" id="treeview">
			            <li id="node_1068"><a class="toggler expanded" href="#"><span></span></a><a class="node draggable ui-draggable" href="#" selectable="true" path="" title="Moduler" ref="1068"><img alt="" src="/umbraco/images/umbraco/folder.gif"><span>Moduler</span></a><ul>
	                        <li id="node_1069"><a class="toggler disabled" href="#"><span></span></a><a class="node draggable ui-draggable" href="#" selectable="true" path="/Forside Banner" title="Forside Banner" ref="1069"><img alt="" src="/umbraco/images/umbraco/flexibox.png"><span>Forside Banner</span></a></li>
                        </ul></li>
		            </ul>
                </div>
            </div>
            <div id="outer-right">
                <div class="pad5">
                    <div class="clearfix">
                        <div class="span12">
                            <span class="containerlabel">Top</span>
                            <div class="dropcontainer"></div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <div class="span9">
                            <span class="containerlabel">Content</span>
                            <div class="dropcontainer"></div>
                        </div>
                        <div class="span3">
                            <span class="containerlabel">Right</span>
                            <div class="dropcontainer"></div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <div class="span12">
                            <span class="containerlabel">Bottom</span>
                            <div class="dropcontainer"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="colspan-container">
                <div id="colspan-outer">
                    <div id="colspan-inner">
                        Column Span:<div class="clearfix" id="coloptions"><div class="coloption"><input type="radio" checked="checked" value="3" name="colspan" id="coloption3"><label for="coloption3">3</label></div><div class="coloption"><input type="radio" value="4" name="colspan" id="coloption4"><label for="coloption4">4</label></div><div class="coloption"><input type="radio" value="6" name="colspan" id="coloption6"><label for="coloption6">6</label></div><div class="coloption"><input type="radio" value="9" name="colspan" id="coloption9"><label for="coloption9">9</label></div><div class="coloption"><input type="radio" value="12" name="colspan" id="coloption12"><label for="coloption12">12</label></div></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    //<![CDATA[
        const colspanContainerHeight = 80;
        var gridEditorFrame = $('#frame', window.parent.document);
        function adjustFrameHeight() {
            var innerDoc = (gridEditorFrame.get(0).contentDocument) ? gridEditorFrame.get(0).contentDocument : gridEditorFrame.get(0).contentWindow.document;
            gridEditorFrame.height(innerDoc.body.scrollHeight +35);
        }
        $(function () {
            gridEditorFrame.load(adjustFrameHeight);
            gridEditorFrame.parent().css('float', 'none');
            $("#treeview-container").height($("#outer-right").height() - colspanContainerHeight);
        });
    //]]>
    </script>
    </form>
</body>
</html>
