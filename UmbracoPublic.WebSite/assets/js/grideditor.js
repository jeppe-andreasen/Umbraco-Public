
var grideditor = {
    init: function () {
    },
    resizing: false,
    resizingQueued: false,
    editModule: function (moduleId) {
        //window.open('/umbraco/editContent.aspx?id=' + moduleId, 'contentEditor', 'width=1000,height=600,location=0,status=0,toolbar=0,menubar=0,directories=0,resizable=1');
        window.open('/handlers/ItemEditor.aspx?id=' + moduleId, 'contentEditor', 'width=1000,height=600,location=0,status=0,toolbar=0,menubar=0,directories=0,resizable=1');

        //var showDialogParameters = {};
        //showDialogParameters.dlg = "CustomContentEditor";
        //showDialogParameters.id = moduleId;
        //this.showDialog("Edit Content", showDialogParameters, undefined, 700, 300);
    },
    dropContainer: function (obj) {
        return obj.closest(".dropcontainer");
    },
    selectedModule: undefined,
    getModuleObj: function (element) {
        var result = {
            "id": element.attr("ref"),
            "colspan": element.attr("colspan"),
            "style": element.attr("style"),
            "idx": parseInt(element.attr("idx"))
        };
        return result;
    },
    repositionAll: function () {
        if (grideditor.resizing == false) {
            grideditor.resizing = true;
            grideditor.resizingQueued = false;
            var dropContainers = $('#grid-editor .dropcontainer');
            var requests = [];
            for (i = 0; i < dropContainers.length; i++) {
                requests.push(grideditor.getModuleRequest($(dropContainers[i]), 'repositionAll'));
            }
            getAllGridEditorItems(grideditor.repositionAllCompleted, requests);
        }
        else {
            grideditor.resizingQueued = true;
        }
    },
    repositionAllCompleted: function (responses) {
        var dropContainers = $('#grid-editor .dropcontainer');
        for (r = 0; r < responses.length; r++) {
            var dropContainer = $(dropContainers[r]);
            var response = responses[r];
            grideditor.iterateModuleResponse(response, dropContainer);
            grideditor.resizeDropContainer(dropContainer, response.rows);
        }
        grideditor.resizing = false;
        if (grideditor.resizingQueued)
            grideditor.repositionAll();
    },
    repositionModules: function (dropContainer, priorityItemId) {
        var request = grideditor.getModuleRequest(dropContainer, 'repositionModule');
        request.priorityItemId = priorityItemId;
        var response = getGridEditorItems(request);
        grideditor.iterateModuleResponse(response, dropContainer);
        grideditor.resizeDropContainer(dropContainer, response.rows);
        grideditor.updateValue();
    },
    getModuleRequest: function (dropContainer, event) {
        var request = {};
        request.width = dropContainer.width();
        request.height = dropContainer.height();
        request.cols = dropContainer.attr("cols");
        request.key = dropContainer.attr("key");
        request.event = event;
        request.provider = $("#grid-editor").attr("data-provider");
        request.existing = [];
        dropContainer.find(".module").each(function () {
            request.existing.push(grideditor.getModuleObj($(this)));
        });
        request.add = "";
        return request;
    },
    iterateModuleResponse: function (response, dropContainer) {
        for (i = 0; i < response.modules.length; i++) {
            var data = response.modules[i];
            var module = dropContainer.find('[ref="' + data.id + '"]');
            module.attr("idx", data.idx);
            grideditor.reposition(module, data);
        }
    },
    reposition: function (module, data) {
        module.css({ left: data.left + 5, top: data.top + 5, width: data.width - 10 });
    },
    resizeDropContainer: function (dropContainer, rows) {
        dropContainer.attr("rows", rows);
        var maxRows = rows;
        var containers = dropContainer.closest(".row").find(".dropcontainer");
        for (i = 0; i < containers.length; i++) {
            var container = $(containers[i]);
            var rowCount = parseInt(container.attr("rows"));
            if (rowCount > maxRows)
                maxRows = rowCount;
        }
        var height = (maxRows + 1) * 60 + 5;

        dropContainer.closest(".row").find(".cell").each(function () {
            $(this).height(height);
        });

        $("#treeview-container").height($(".grid-column .inner-box").outerHeight() - 190);
        $(".module-column .inner-box").height($(".grid-column .inner-box").outerHeight() - 12);
        var frame = $("#grid-editor").attr("frame");
        if (frame != '') {
            adjustFrameHeight();
        }
    },
    initModule: function (module) {
        module.draggable({
            opacity: 0.7,
            stop: function (event, ui) {
                var dropContainer = grideditor.dropContainer(module);
                grideditor.repositionModules(dropContainer, module.attr('ref'));
            }
        });
        module.mousedown(function () {
            grideditor.selectModule($(this));
        });

        module.find("h3 a.share").click(function (e) {
            e.preventDefault();
            var module = $(this).closest(".module");
            grideditor.shareModule(module);
        });
        module.find("h3 a.detach").click(function (e) {
            e.preventDefault();
            var module = $(this).closest(".module");
            grideditor.detachModule(module);
        });
        module.find("h3 a.edit").click(function (e) {
            e.preventDefault();
            var itemId = $(this).closest(".module").attr("ref");
            grideditor.editModule(itemId);
        });
        module.find("h3 a.remove").click(function (e) {
            var dropContainer = grideditor.dropContainer(module);
            $(this).closest(".module").remove();
            grideditor.selectModule(undefined);
            grideditor.repositionModules(dropContainer, '');
            e.preventDefault();
        });

    },
    shareModule: function (module) {
        var gridEditor = $("#grid-editor");
        var parameters = {};
        parameters.dlg = "ShareModule";
        parameters.pid = gridEditor.attr("pageId");
        parameters.mid = module.attr("ref");
        parameters.ph = module.closest(".dropcontainer").attr("key");
        parameters.provider = gridEditor.data("provider");
        this.showDialog('Share Module', parameters, this.itemSharedCallback, 300, 400);
    },
    itemSharedCallback: function (response) {
        var gridEditor = $("#grid-editor");
        var query = '.module[ref="' + response.values.moduleId + '"]';
        gridEditor.find(query, '#grid-editor').each(function () {
            var module = $(this);
            module.removeClass("local");
            module.addClass("global");
        });

        var tv = $("#treeview-container").find('.linqit-treeview');
        window.linqit.treeview.refreshNode(tv, response.values.parentId);
    },
    detachModule: function (module) {
        var provider = $("#grid-editor").attr("data-provider");
        var referenceId = $("#grid-editor").attr("pageId");
        var newId = detach(module.attr("ref"), provider, referenceId);
        module.attr("ref", newId);
        module.removeClass("global");
        module.addClass("local");
        grideditor.updateValue();
    },
    selectModule: function (module) {
        if (grideditor.selectedModule != undefined)
            grideditor.selectedModule.removeClass("selected");
        grideditor.selectedModule = module;
        if (grideditor.selectedModule != undefined) {
            grideditor.selectedModule.addClass("selected");
            var placeholderColumns = parseInt(grideditor.dropContainer(module).attr("cols"));
            var options = grideditor.selectedModule.attr("coloptions").split(",");
            var radioHtml = '';
            var currentCols = grideditor.selectedModule.attr("colspan");
            for (i = 0; i < options.length; i++) {
                radioHtml += grideditor.getRadioBox(options[i], currentCols, placeholderColumns);
            }
            $("#coloptions").html(radioHtml);
            $("#coloptions input").click(function () {
                var value = $(this).attr("value");
                grideditor.selectedModule.attr("colspan", value);
                grideditor.selectedModule.find("p span.cols").html(value);
                var dropContainer = grideditor.dropContainer(module);
                grideditor.repositionModules(dropContainer, module.attr('ref'));
            });
        }
        else {
            $("#coloptions").html('');
        }
    },
    getRadioBox: function (value, selectedValue, placeholderColumns) {
        var result = '<div class="coloption"><input id="coloption' + value + '" type="radio" name="colspan" value="' + value + '"';
        if (value == selectedValue)
            result += ' checked="checked"';
        if (parseInt(value) > placeholderColumns)
            result += ' disabled';
        result += '/><label for="coloption' + value + '">' + value + '</label></div>';
        return result;
    },
    updateValue: function () {
        var editor = $("#grid-editor");
        var hiddenId = editor.attr("hiddenId");
        if (hiddenId == '')
            return;

        var frame = editor.attr("frame");
        var control = undefined;
        if (frame == '')
            control = $("#" + hiddenId);
        else
            control = $("#" + hiddenId, window.parent.document);

        var value = [];
        $("#grid-editor .dropcontainer").each(function () {
            var dropContainer = $(this);
            var ph = {};
            ph.key = dropContainer.attr("key");
            ph.span = parseInt(dropContainer.attr("cols"));
            ph.modules = [];
            var modules = dropContainer.find(".module").each(function () {
                var module = $(this);
                var item = {};
                item.Id = module.attr("ref");
                item.span = parseInt(module.attr("colspan"));
                ph.modules[parseInt(module.attr("idx"))] = item;
            });
            value.push(ph);
        });
        control.val(JSON.stringify(value));
    },
    dropNode: function (event, ui, dropContainer) {
        var droppedId = ui.draggable.attr("ref");
        if ($(this).find('[ref="' + droppedId + '"]').length > 0) {
            jAlert("Item cannot be added twice.");
            return;
        }
        var request = grideditor.getModuleRequest(dropContainer, 'itemDropped');
        request.add = droppedId;
        request.addX = event.pageX - dropContainer.offset().left;
        request.addY = event.pageY - dropContainer.offset().top;
        var mrk = $('<div class="module draggable" style="position:absolute" ref="' + droppedId + '"><h3>' + ui.draggable.html() + '<a class="share" href="#" title="Move to Module Library">&nbsp;</a><a class="detach" href="#" title="Make a local copy">&nbsp;</a><a class="edit" href="#" title="edit">&nbsp;</a><a class="remove" href="#" title="remove">&nbsp;</a></h3><p><span class="cols"></span></p></div>');

        var response = getGridEditorItems(request);

        var placeholderColumns = parseInt(dropContainer.attr("cols"));
        if (response.add.colspan > placeholderColumns) {
            jAlert("This item is too wide to fit in this placeholder.");
            return;
        }
        mrk.attr("coloptions", response.add.coloptions);
        mrk.attr("colspan", response.add.colspan);
        mrk.find("p span.cols").html(response.add.colspan);
        mrk.appendTo(dropContainer);
        grideditor.iterateModuleResponse(response, dropContainer);
        grideditor.resizeDropContainer(dropContainer, response.rows);
        grideditor.initModule(mrk);
        grideditor.selectModule(mrk);
        grideditor.updateValue();
    },
    dropModuleType: function (event, ui, dropContainer) {
        var typename = ui.draggable.find("span").html();
        var gridEditor = $("#grid-editor");
        var parameters = {};
        parameters.dlg = "CreateModule";
        parameters.pid = gridEditor.attr("pageId");
        parameters.tid = ui.draggable.attr("tref");
        parameters.ph = dropContainer.attr("key");
        parameters.provider = gridEditor.data("provider");
        parameters.x = event.pageX - dropContainer.offset().left;
        parameters.y = event.pageY - dropContainer.offset().top;
        this.showDialog('Create ' + typename, parameters, this.itemCreatedCallback, 300, 150);
    },
    dropModule: function (event, ui, dropContainer) {
        var oldContainer = grideditor.dropContainer(ui.draggable);
        if (oldContainer.attr("key") == dropContainer.attr("key"))
            return;

        var droppedId = ui.draggable.attr("ref");
        if ($(this).find('[ref="' + droppedId + '"]').length > 0) {
            jAlert("Item cannot be added twice.");
            return;
        }

        var request = grideditor.getModuleRequest(dropContainer, 'itemDropped');
        request.add = droppedId;
        request.addX = event.pageX - dropContainer.offset().left;
        request.addY = event.pageY - dropContainer.offset().top;

        var mrk = $('<div class="module draggable" style="position:absolute" ref="' + droppedId + '"><h3><img alt=""><span>Yo name</span><a class="share" href="#" title="Move to Module Library">&nbsp;</a><a class="detach" href="#" title="Make a local copy">&nbsp;</a><a class="edit" href="#" title="edit">&nbsp;</a><a class="remove" href="#" title="remove">&nbsp;</a></h3><p><span class="cols"></span></p></div>');
        var srcImg = ui.draggable.find("h3 img");
        var dstImg = mrk.find("h3 img");
        dstImg.attr("src", srcImg.attr("src"));
        dstImg.next().html(srcImg.next().html());


        var response = getGridEditorItems(request);

        var placeholderColumns = parseInt(dropContainer.attr("cols"));
        if (response.add.colspan > placeholderColumns) {
            jAlert("This item is too wide to fit in this placeholder.");
            return;
        }
        mrk.attr("coloptions", response.add.coloptions);
        mrk.attr("colspan", response.add.colspan);
        mrk.find("p span.cols").html(response.add.colspan);
        mrk.appendTo(dropContainer);
        grideditor.iterateModuleResponse(response, dropContainer);
        grideditor.resizeDropContainer(dropContainer, response.rows);
        grideditor.initModule(mrk);
        grideditor.selectModule(mrk);

        ui.draggable.remove();
        grideditor.repositionModules(oldContainer, '');

        grideditor.updateValue();
    },
    itemCreatedCallback: function (response) {
        var dropContainer = $('[key="' + response.values.ph + '"]', "#grid-editor");
        var request = grideditor.getModuleRequest(dropContainer, 'itemDropped');
        request.add = response.values.id;
        request.addX = response.values.x;
        request.addY = response.values.y;
        var mrk = $(response.values.html);
        var response = getGridEditorItems(request);
        var placeholderColumns = parseInt(dropContainer.attr("cols"));
        if (response.add.colspan > placeholderColumns) {
            jAlert("This item is too wide to fit in this placeholder.");
            return;
        }
        mrk.attr("coloptions", response.add.coloptions);
        mrk.attr("colspan", response.add.colspan);
        mrk.find("p span.cols").html(response.add.colspan);
        mrk.appendTo(dropContainer);
        grideditor.iterateModuleResponse(response, dropContainer);
        grideditor.resizeDropContainer(dropContainer, response.rows);
        grideditor.initModule(mrk);
        grideditor.selectModule(mrk);
        grideditor.updateValue();
    },
    handleCommand: function (commandName, commandArgs) {
        switch (commandName) {
            case "showdialog":
                if (commandArgs[0] == "CustomContentEditor")
                    grideditor.editModule(commandArgs[1]);
                else {
                    var showDialogParameters = {};
                    showDialogParameters.dlg = commandArgs[0];
                    showDialogParameters.id = commandArgs[1];
                    this.showDialog("Edit Content", showDialogParameters, undefined, 700, 300);
                }
                break;
        }
    },
    showDialog: function (dialogTitle, parameters, callback, width, height) {

        var dlg = $("#grid-dialog");
        dlg[0].response = {};
        dlg.load('/Handlers/DialogHandler.ashx', parameters);
        dlg.dialog({
            title: dialogTitle,
            close: function (event, ui, obj) {
                var response = dlg[0].response;
                if (callback != undefined && response != undefined) {
                    callback(response);
                }
                if (response != undefined && response.commands != undefined) {
                    for (i = 0; i < response.commands.length; i++) {
                        grideditor.handleCommand(response.commands[i].name, response.commands[i].args.split(','));
                    };
                }
                //window.application.vs.handleResize();
            },
            resizable: false,
            modal: true,
            width: width,
            height: height
        });
        //application.vs.handleResize();
    }
};

$(document).ready(function () {
    grideditor.init();
    $(".linqit-treeview .node").draggable({
        helper: "clone",
        opacity: 0.7
    });
    $(".module-column a").draggable({
        helper: "clone",
        opacity: 0.7
    });
    $('.linqit-treeview').mousedown(function () {
        grideditor.selectModule(null);
    });
    $(".dropcontainer .module").each(function () {
        grideditor.initModule($(this));
    });
    $(window).resize(function () {
        grideditor.repositionAll();
    });
    $(".dropcontainer").droppable({
        accept: ".node,.moduleType,.module",
        activeClass: "droptarget",
        drop: function (event, ui) {
            var dropContainer = $(this);
            if (ui.draggable.hasClass("node"))
                grideditor.dropNode(event, ui, dropContainer);
            else if (ui.draggable.hasClass("moduleType"))
                grideditor.dropModuleType(event, ui, dropContainer);
            else if (ui.draggable.hasClass("module"))
                grideditor.dropModule(event, ui, dropContainer);
        }
    });
});
