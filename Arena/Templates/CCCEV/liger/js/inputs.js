/**********************************************************************
* Description:	Clear field on focus
* Created By:   Nick Airdo @ Central Christian Church of the East Valley
* Date Created:	4/20/2009
*
* $Workfile: inputs.js $
* $Revision: 3 $ 
* $Header: /trunk/Arena/Templates/Cccev/liger/js/inputs.js   3   2009-04-20 16:27:41-07:00   nicka $
**********************************************************************/

$(document).ready(function()
{
    // store the original value in an attribute
    jQuery.each($('.subscribe input:text'), function()
    {       
        $(this).attr("valueHtml", $(this).val());
    });

    $('.subscribe input:text').focus(function()
    {
        $(this).val("");
    });

    $('.subscribe input:text').blur(function()
    {
        if ($(this).val() == "")
        {
            $(this).val($(this).attr("valueHtml"));
        }
    });
});