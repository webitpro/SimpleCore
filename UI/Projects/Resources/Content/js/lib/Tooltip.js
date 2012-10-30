var isAnimated = false;
var duration = 0;
var id = "";
var div = null;

var Tip =
{
    Init: function (divId, animated, animTime)
    {
        div = $('#' + divId);

        div.css("overflow", "hidden");
        div.css("display", "none");
        div.css("position", "absolute");
        div.css("width", "0px");
        div.css("height", "0px");
        div.css("padding", "10px");
        div.css("top", "0");
        div.css("left", "0");

        isAnimated = animated;
        duration = animTime;
        id = divId;
        if (isAnimated)
        {
            div.fadeTo(10, 0);
        }

    },
    Show: function (e, tip)
    {
        div.css('top', e.pageY + 10);
        div.css('left', e.pageX + 20);
        div.css('width', 'auto');
        div.css('height', 'auto');
        div.html(tip);

        if (isAnimated)
        {
            div.fadeIn(duration);
            div.fadeTo(duration / 2, 0.8);
        }
        else
        {
            div.show();
        }
    },
    Hide: function (e)
    {
        div.css('top', 0);
        div.css('left', 0);
        div.css('width', '0px');
        div.css('height', '0px');
        div.html('');

        if (isAnimated)
        {
            div.fadeOut(10);
            div.fadeTo(10, 0);
        }
        else
        {
            div.hide();
        }
    },
    Reposition: function (e)
    {
        div.css('top', e.pageY + 10);
        div.css('left', e.pageX + 20);
    }
};
