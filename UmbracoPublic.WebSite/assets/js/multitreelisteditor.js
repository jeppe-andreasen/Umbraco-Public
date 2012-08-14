var multiTreeListEditor = multiTreeListEditor || {
    init: function () {

        $('.multitreelist-editor .dstList').on('click', 'a', function (e) {
            e.preventDefault();
            $('.multitreelist-editor .dstList a').removeClass('selected');
            $(this).addClass("selected");
        });

        $('.multitreelist-editor .dstList').on('dblclick', 'a', function (e) {
            e.preventDefault();
            var dst = $(this).closest('.dstList');
            multiTreeListEditor.removeNode(dst);
        });

        $('.multitreelist-editor .srcList').on('dblclick', 'a', function (e) {
            e.preventDefault();
            multiTreeListEditor.addNode($(this));
        });

        $('.multitreelist-editor .btnAdd').click(function () {
            multiTreeListEditor.addNode($(this));
        });
        $('.multitreelist-editor .btnRemove').click(function () {
            multiTreeListEditor.removeNode($(this));
        });
        $('.multitreelist-editor .btnMoveUp').click(function () {
            multiTreeListEditor.moveNode($(this), -1);
        });
        $('.multitreelist-editor .btnMoveDown').click(function () {
            multiTreeListEditor.moveNode($(this), 1);
        });
    },
    addNode: function ($element) {
        var editor = $element.closest('.multitreelist-editor');
        var node = editor.find('.srcList a.selected');
        var li = $('<li><a class="item" href="#" title=""><img alt="" src=""><span></span></a></li>');
        li.find('a').attr("ref", node.attr("ref"));
        li.find('img').attr("src", node.find('img').attr("src"));
        li.find('span').html(node.find('span').html());
        editor.find('.dstList').append(li);
        multiTreeListEditor.updateValueBox($element);
    },
    removeNode: function ($button) {
        var editor = $button.closest('.multitreelist-editor');
        var src = editor.find('.dstList');
        var item = src.find('a.selected');
        if (item.length == 0)
            return;

        var li = item.closest('li');
        li.remove();
        multiTreeListEditor.updateValueBox($button);
    },
    moveNode: function ($button, step) {
        var editor = $button.closest('.multitreelist-editor');
        var dst = editor.find('.dstList');
        var item = dst.find('a.selected');
        if (item.length == 0)
            return;

        var items = dst.find('a');
        var index = items.index(item);
        var newIndex = index + step;
        if (newIndex < 0)
            return;
        else if (newIndex > items.length - 1)
            return;

        var li = item.closest('li');
        li.remove();

        if (newIndex == items.length - 1)
            li.appendTo(dst);
        else
            $(dst.find('li')[newIndex]).before(li);

        multiTreeListEditor.updateValueBox($button);
    },
    updateValueBox: function ($element) {
        var editor = $element.closest('.multitreelist-editor');
        if (editor.attr("hiddenId") != '') {
            var value = '';
            editor.find('.dstList a').each(function () {
                if (value != '')
                    value += ',';
                value += $(this).attr("ref");
            });
            pushValueToIframe(editor.attr("hiddenId"), value);
        }
    }
};