function pushValueToIframe(hiddenId, value) {
    var control = $("#" + hiddenId, window.parent.document);
    control.val(value);
}