/**********************************************************************
* Description:	TBD
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	TBD
*
* $Workfile: calendar.js $
* $Revision: 6 $ 
* $Header: /trunk/Arena/Templates/Cccev/liger/js/calendar.js   6   2009-04-06 08:31:19-07:00   JasonO $
* 
* $Log: /trunk/Arena/Templates/Cccev/liger/js/calendar.js $
*  
*  Revision: 6   Date: 2009-04-06 15:31:19Z   User: JasonO 
*  
*  Revision: 5   Date: 2009-04-03 00:24:02Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-04-02 17:46:58Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-04-02 16:50:10Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-03-25 17:57:34Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-03-24 20:13:32Z   User: JasonO 
**********************************************************************/

var calClick;

$(document).ready(function()
{
    initCalendar();
});

function initCalendar()
{
    calClick = false;

    $('div.calendar-overlay').css('opacity', '0.6');
    $('div.events-overlay').css('opacity', '0.6');

    $('div.calendar-holder').click(function()
    {
        calClick = true;
    });
}

function showLoaders()
{
    $('div.calendar-overlay').fadeIn();
    $('div.events-overlay').fadeIn();
    return false;
}

function hideLoaders()
{
    calClick = false;
    $('div.calendar-overlay').fadeOut();
    $('div.events-overlay').fadeOut();
    return false;
}