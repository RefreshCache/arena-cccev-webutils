$(document).ready(function()
{
    initMenu();
});

function initMenu()
{
    var isInNav = false;
    $('div.drop-holder').hide();

    $('a.drop-down').click(function()
    {
        var dropDown = $(this).siblings(':first');

        if ($(dropDown).is(':hidden'))
        {
            $('div.drop-holder').slideUp();

            if ($('a.drop-down').hasClass('active'))
            {
                $('a.drop-down').removeClass('active');
                $(this).addClass('active');
                setTimeout('showMenu()', 500);
            }
            else
            {
                $(this).addClass('active');
                $(dropDown).slideDown();
            }

            return false;
        }
        else
        {
            return hideMenu();
        }
    });

    $('div.link-holder a.close').click(function()
    {
        return hideMenu();
    });

    $('div.drop-holder').mouseenter(function()
    {
        if ($('div.drop-holder').is(':visible'))
        {
            isInNav = true;
        }
    });

    $('div.drop-holder').mouseleave(function()
    {
        if ($('div.drop-holder').is(':visible') && isInNav)
        {
            isInNav = false;
        }
    });

    $(document).click(function()
    {
        if ($('div.drop-holder').is(':visible') && !isInNav)
        {
            return hideMenu();
        }
    });
}

function hideMenu()
{
    $('div.drop-holder').slideUp()
    $('a.drop-down').removeClass('active');
    return false;
}

function showMenu()
{
    $('a.active').siblings(':first').slideDown();
}