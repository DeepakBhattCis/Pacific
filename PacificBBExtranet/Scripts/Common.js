
/* ================ DOCUMENT.ONREADY SETUP ==============  */

$(function () {
    /* ================ DIALOG-LINK-SETUP ==============  */
    $(document).on('click', 'a[data-dialog-url]', function (e) {

        e.preventDefault();

        DialogHelper.Load($(this));

    });

    $(document).on('click', 'a[data-dialog-select-target]', function (e) {
        e.preventDefault();
        DialogHelper.HandleSelectTarget($(this));
    });



    /* ===================== TOOLTIP ===================== */

    $('[data-toggle="tooltip"]').tooltip({ 'placement': 'bottom' });


    $(document).on('click', 'a[data-action=refresh]', function(evt) {
        evt.preventDefault();
        $('ul li[role=presentation].active > a').click();
    });

});

