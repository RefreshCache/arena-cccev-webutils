/**
 * This set of js functions handles scrolling/sliding of various elements in the DOM.
 * It is dependent on the jquery.galleryScroll.1.4.5.js plugin.
**/

$(document).ready(function()
{
    initEvents();
});

function initEvents()
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

    $('#gallery').galleryScroll({
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

    $('a.link-prev').click(function() {
        if (_numEl > 1) {
            _numEl -= 1;
        }
        $('div.counter span.this').text(_numEl);
    });

    $('a.link-next').click(function() {
        if (_numEl < _all) {
            _numEl += 1;
        }
        $('div.counter span.this').text(_numEl);
    });
    
    $('#sidebar .sidebar-area .serving').galleryScroll({
        btPrev: 'a.left',
        btNext: 'a.right',
        holderList: '.cover',
        scrollElParent: 'ul',
        scrollEl: 'li',
        slideNum: false,
        duration: 800,
        step: false,
        circleSlide: true,
        disableClass: 'disable'
    });
    
    $('#content .column .events').galleryScroll({
        btPrev: 'a.left',
        btNext: 'a.right',
        holderList: '.cover',
        scrollElParent: 'ul',
        scrollEl: 'li',
        slideNum: false,
        duration: 800,
        step: false,
        circleSlide: true,
        disableClass: 'disable'
    });

    $('#content .column .calendar').children(':last').children(':last').galleryScroll({
        btPrev: 'a.left',
        btNext: 'a.right',
        holderList: '.cover',
        scrollElParent: 'ul',
        scrollEl: 'li',
        slideNum: false,
        duration: 800,
        step: false,
        circleSlide: true,
        disableClass: 'disable'
    });

    $('#content .holder').galleryScroll({
        btPrev: 'a.left',
        btNext: 'a.right',
        holderList: '.album',
        scrollElParent: 'div.scroller',
        scrollEl: 'div.scroller-item',
        slideNum: false,
        duration: 800,
        step: false,
        circleSlide: true,
        disableClass: 'disable'
    });
    
    $('#content .column .table-holder').galleryScroll({
        btPrev: 'a.left',
        btNext: 'a.right',
        holderList: '.calendar-item',
        scrollElParent: 'ul',
        scrollEl: 'li',
        slideNum: false,
        duration: 800,
        step: 1,
        circleSlide: true,
        disableClass: 'disable'
    });  
}