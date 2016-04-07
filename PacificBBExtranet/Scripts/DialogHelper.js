var DialogHelper = {

    GetModal: function (jqSourceDialog) {
        return jqSourceDialog.find('.modal-dialog');
    },

    GetBody: function (jqSourceModal) {
        //<div class='modal-body'><div class='bootbox-body'></div></div>
        var body = jqSourceModal.find('.bootbox-body');
        if (body.length > 0)
            return body;
        else
            return jqSourceModal.find('.modal-body');
    },

    Load: function (qaSourceAnchor) {

        var url = $(qaSourceAnchor).attr('data-dialog-url');
        var title = $(qaSourceAnchor).attr('data-dialog-title');
        var widthPercent = $(qaSourceAnchor).attr('data-dialog-width-percent');

        var data = {
            url:url, title:title , widthPercent:widthPercent
        };

        this.LoadJS(data);
    },

    LoadJS: function (data) {

        var dialog = bootbox.dialog({
            message: (AjaxHelper ? AjaxHelper.AjaxLoadingHtml() : "Loading..."),
            title: data.title,
            buttons: {}
        });

        //dialog.attr('id', 'dialog');

        var modalContainer = DialogHelper.GetModal(dialog);

        if (data.widthPercent != "" && data.widthPercent != "0" && data.widthPercent != 0) {
            modalContainer.css('width', data.widthPercent + '%');
        }

        var modalBody = DialogHelper.GetBody(modalContainer);

        modalBody.load(data.url, DialogHelper.LoadResponseHandler);

        dialog.find('#modalContent').draggable({
            handle: '.modal-header'
        });
    },

    LoadResponseHandler: function (responseText, textStatus, xhr) {

        var modalBody = $(this);

        if (textStatus == "error") {
            modalBody.html('An Error Occurred');
        } else {
            modalBody.find('input:first').focus();
        }
    },

    HandleSelectTarget: function (jqSource) {

        var json = jqSource.data('dialog-select-target');

        var valueControl = $('#' + json.ValueControlID);
        if (valueControl.is('input'))
            valueControl.val(json.Value);
        else
            valueControl.text(json.Value);

        var keyControl = $('#' + json.KeyControlID);
        if (keyControl.is('input'))
            keyControl.val(json.Key);
        else
            keyControl.text(json.Key);
    },

    HookupHiddenCallback: function (childSelector, callback) {

        var dialog = $(childSelector).closest('[role="dialog"]');

        dialog.on('hidden.bs.modal', function () {
            callback();
        });
    },
    
    HideClosestModal: function (id) {
        
        var dialog = $('#' + id).parents("div[role='dialog']");
        if (dialog != null && dialog.length > 0) {
            dialog.first().modal("hide");
        } else {
            bootbox.hideAll();
        }
    }    
}; 

