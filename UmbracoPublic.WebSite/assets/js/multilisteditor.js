var multiListEditor = multiListEditor || {
    init: function () {
        $('.multilist-editor').on('click', 'a', function (e) {
            e.preventDefault();
            var anchor = $(this);
            var editor = anchor.closest('.multilist-editor');
            if (editor[0].selectedNode != undefined) {
                $(editor[0].selectedNode).removeClass("selected");
            }
            editor[0].selectedNode = anchor[0];
            anchor.addClass("selected");
        });

        $('.multilist-editor').on('dblclick', 'a', function (e) {
            e.preventDefault();
            $(this).removeClass("selected");
            var li = $(this).closest("li");
            var src = li.closest('.listbox');
            var editor = src.closest('.multilist-editor');
            editor[0].selectedNode = undefined;
            var dst = src.hasClass('srcList') ? editor.find('.dstList') : editor.find('.srcList');

            li.remove();
            li.appendTo(dst);

            multiListEditor.updateValueBox($(this));
        });

        $('.multilist-editor .btnAdd').click(function () {
            multiListEditor.addNode($(this));
        });
        $('.multilist-editor .btnRemove').click(function () {
            multiListEditor.removeNode($(this));
        });
        $('.multilist-editor .btnMoveUp').click(function () {
            multiListEditor.moveNode($(this), -1);
        });
        $('.multilist-editor .btnMoveDown').click(function () {
            multiListEditor.moveNode($(this), 1);
        });
    },
    addNode: function ($button) {
        var editor = $button.closest('.multilist-editor');
        var src = editor.find('.srcList');
        var item = src.find('a.selected');
        if (item.length == 0)
            return;

        item.removeClass('selected');

        var li = item.closest('li');
        li.remove();

        var dst = editor.find('.dstList');
        li.appendTo(dst);
        multiListEditor.updateValueBox($button);
    },
    removeNode: function ($button) {
        var editor = $button.closest('.multilist-editor');
        var src = editor.find('.dstList');
        var item = src.find('a.selected');
        if (item.length == 0)
            return;

        item.removeClass('selected');

        var li = item.closest('li');
        li.remove();

        var dst = editor.find('.srcList');
        li.appendTo(dst);
        multiListEditor.updateValueBox($button);
    },
    moveNode: function ($button, step) {
        var editor = $button.closest('.multilist-editor');
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

        multiListEditor.updateValueBox($button);
    },
    updateValueBox: function ($button) {
        var editor = $button.closest('.multilist-editor');
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