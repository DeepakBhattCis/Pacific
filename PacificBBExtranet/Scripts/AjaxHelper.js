// default ajax settings
$.ajaxSetup({
    global: true,
    cache: false,
    timeout: (1000 * 5000) // 20 second timeout
});

// DEVELOPER NOTE
// we show default loading overlay only on POST for ajax enabled forms
// because /get requests should handle this individually and in the relevant container
/*
$(document).ajaxSend(function (event, request, settings) {
    if (AjaxHelper.IsPost(settings)) {
        $('#ajaxLoadingOverlay').show();
    }
});*/

var _stateIndexer = 0;

$(document).ajaxComplete(function (event, request, settings) {
    /*if (AjaxHelper.IsPost(settings)) {
        $('#ajaxLoadingOverlay').hide();
    }*/

    // To allow users to use the back button w/ ajax requests
    // and stop them from backing out of the application entirely
    
    _stateIndexer++;

    var stateObject = { "state": _stateIndexer },
		title = "AjaxStateManager",
		url = "#/" + _stateIndexer + "/";

    history.pushState(stateObject, title, url);
});

$(document).ajaxError(function (event, jqxhr, settings, exception) {

    // if(exception == 'timeout')
    var message = 'An error occurred (' + exception + ') - Support has been notified';

    if (typeof (jqxhr.responseText) !== 'undefined') {
        message = jqxhr.responseText;
    }

    if (bootbox) {
        bootbox.alert({
            message: message,
            animate: true
        });
    }
    else {
        alert(message);
    }

});

// http://api.jquery.com/category/ajax/global-ajax-event-handlers/
// set global handlers for loading, error, etc.

// should we do it this way?
/*
$.ajaxSetup({
    statusCode: {
        401: function() {
            window.location.href = "path/to/login";
        }
    }
});
*/

var AjaxHelper = {

    AjaxLoadingHtml: function (heightNumeric, loadingElementCss, loadingText) {
        return '<div class="ajax-loading-inner clear-fix ' + loadingElementCss + '"' + (heightNumeric && heightNumeric > 10 ? ' style="height:' + heightNumeric + 'px;"' : '') +
            '><img src="' + GlobalVariables.ImgUrlAjaxLoading() + '" /><div class="ajax-loading-inner-text">' + (loadingText ? loadingText : '') + '</div></div>';
    },

    SetLoadingOnTarget: function (targetSelelector, loadingElementCss, loadingText) {

        var target = $(targetSelelector);
        target.html(AjaxHelper.AjaxLoadingHtml(target.height(), loadingElementCss, loadingText));
    },

    IsPost: function (settings) {
        return settings.type.toUpperCase() === "POST";
    },

    LoadContainer: function (actionUri, targetContainerId, loadingText, loadingElementCss) {

        var selector = (targetContainerId.indexOf('#') > -1 ? targetContainerId : '#' + targetContainerId);

        AjaxHelper.SetLoading(selector, loadingText, loadingElementCss);

        $(selector).load(actionUri);
    },

    SetLoading: function (targetSelelector, loadingText, loadingElementCss) {

        AjaxHelper.SetLoadingOnTarget(targetSelelector, loadingElementCss, loadingText);
        /*
        if (!loadingText || loadingText == '')
            loadingText = "Loading...";

        $(targetSelelector)
            .html('<div class="ajax-loading-inner sm"><img src="' +
                //GlobalVariables.ImgUrlAjaxLoading() +
                GlobalVariables.ImgUrlMiniAjaxLoading() +
                '" /><div class="ajax-loading-inner-text">' + loadingText + '</div></div>');*/
    },

    LoadContainerMini: function (actionUri, targetContainerId) {

        var selector = (targetContainerId.indexOf('#') > -1 ? targetContainerId : '#' + targetContainerId);

        $(selector).html(AjaxHelper.ContentLoadingMini());

        $.get(actionUri, function (data) {
            $(selector).html(data);
        });
    },

    ContentLoadingMini: function () {
        return '<img class="ajax-loading-spinner-mini" src="' + GlobalVariables.ImgUrlMiniAjaxLoading() + '" />';
    }
}