/**
 * This set of js functions handles displaying HTML popovers in the DOM
 */

$(document).ready(function()
{
    initPopovers();
});

function initPopovers()
{
    $('.overlayBg').css({
        'opacity': '0.6',
        'height': document.body.clientHeight + 'px'
    });

    $('.central-login a.btn').click(function()
    {
        $('.popup-light-holder').hide();
        $('.overlayBg').fadeOut();
        return false;
    });

    $('.central-login input.btn').click(function()
    {
        //$('.popup-light-holder').hide();
        //$('.overlayBg').fadeOut();
        return true;
    });

    $('a.show-popup').click(function()
    {
        var loginLinkText = $(this).text();

        if (loginLinkText && loginLinkText == "Login")
        {
            $("input[id$=iRedirect]").val( document.location.href );
            $('.overlayBg').fadeIn();
            $('.popup-light-holder').show();
            return false;
        }
        else
        {
            return true;
        }
    }); 

    $('a.email-event').click(function()
    {
        if ($('div.cal-popup-light-holder').length > 0)
        {
            var ids = $(this).attr('id');
            $("input[id$='ihIDs']").val(ids);
            $('.overlayBg').fadeIn();
            $('.cal-popup-light-holder').show();
        }

        return false;
    });

    $('.email-box div input.btn').click(function()
    {
        var regex = new RegExp(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*\.(\w{2}|(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum|tv))$/);

        if ($("input[id$='tbEmailAddress']").val() != '' &&
            regex.test($("input[id$='tbEmailAddress']").val()))
        {
            $('.cal-popup-light-holder').hide();
            $('.overlayBg').fadeOut();
        }

        return true;
    });

    $('.email-box a.btn').click(function()
    {
        $("input[id$='ihIDs']").val('');
        $('.cal-popup-light-holder').hide();
        $('.overlayBg').fadeOut();
        clearEmailPopover();
        return false;
    });
}

function clearEmailPopover()
{
    $("input[id$='tbEmailAddress']").val('');
    $("input[id$='tbTo']").val('');
    $("input[id$='tbFrom']").val('');
    return false;
}