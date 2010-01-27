$(document).ready(function()
{
    var _dynamicM = parseInt($('#gallery ul').css('marginLeft'));
    var _liWidth = 0;

    $('#gallery ul li').each(function()
    {
        _liWidth += $(this).outerWidth();
    });

    var _all = Math.ceil(_liWidth / 896);
    $('div.counter span.all').text(_all);
    var _numEl = 1;

    $('#gallery').galleryScroll(
        {
            btPrev: 'a.link-prev',
            btNext: 'a.link-next',
            holderList: 'div.list',
            scrollElParent: 'ul',
            scrollEl: 'li',
            slideNum: false,
            duration: 800,
            step: false,
            circleSlide: false,
            disableClass: 'disable'
        });

    $("a.link-prev").click(function()
    {
        _numEl -= 1;
        if (_numEl < 1)
        {
            _numEl = 1;
        }

        $('div.counter span.this').text(_numEl);
    });

    $("a.link-next").click(function()
    {
        _numEl += 1;
        if (_numEl > _all)
        {
            _numEl = _all;
        }

        $('div.counter span.this').text(_numEl);
    });
});