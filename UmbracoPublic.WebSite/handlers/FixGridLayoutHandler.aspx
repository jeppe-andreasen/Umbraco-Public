<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FixGridLayoutHandler.aspx.cs" Inherits="UmbracoPublic.WebSite.handlers.FixGridLayoutHandler" ViewStateMode="Disabled" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body 
        {
            font-family:Tahoma;
            font-size:11.5px;
        }
        h3 
        {
            text-transform:capitalize;
            font-size:14px;
            margin-top:15px;
            margin-bottom:3px;
        }
        h4 
        {
            margin-bottom:3px;
            margin-top:0;
        }
        div.module 
        {
            border:1px solid #ccc;
            margin-bottom:5px;
            padding:5px;
            border-radius: 4px 4px 4px 4px;
        }
        div.local 
        {
            background: #ede5bc;
        }
        div.global 
        {
            background: #cedccd;
        }
        select 
        {
            width:100%;
        }
        em 
        {
            display:block;
            margin-top:4px;
        }
        em span 
        {
            background-repeat:no-repeat;
            padding-left:20px;
        }
        em span.alert 
        {
            background-image:url('/assets/img/grideditor/alert.png');
        }
        em span.info 
        {
            background-image:url('/assets/img/grideditor/info.png');
        }
    </style>
    <script src="/assets/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/assets/js/versus-ajax.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        The grid data is not valid for the current layout. Please reassign the modules to the available placeholders on this layout. 
        <asp:Literal ID="litOutput" runat="server"></asp:Literal>
    </div>
    <script type="text/javascript">
        function updateValues() {
            var module = $(this).closest(".module");
            var request = [];
            $('.cell').each(function () {
                var cell = $(this);
                cell.find('.module').each(function () {
                    var module = $(this);
                    var replacement = {};
                    replacement.id = module.attr("ref");
                    replacement.from = cell.attr("ref");
                    replacement.to = module.find("select").val();
                    request.push(replacement);
                });
            });
            var response = handleChange(request);
            var control = $("#" + response.hiddenId, window.parent.document);
            control.val(response.value);
            $(response.messages).each(function (a, b) {
                var message = this;
                var cell = $('.cell[ref="' + message.ph + '"]');
                var module = cell.find('[ref="' + message.id + '"]');
                var em = module.find("em");
                if (message.type == 'alert')
                    em.html('<span class="alert">' + message.text + '</span>');
                else if (message.type == 'info')
                    em.html('<span class="info">' + message.text + '</span>');
                else
                    em.html('');
            });
        }


        $(function () {
            $('select').change(function (a) {
                updateValues();
            });
            updateValues();
        });
    </script>
    </form>
</body>
</html>
