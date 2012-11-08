var Utils =
{
    //SET METHODS
    SetCache: function (min)
    {
        var head = document.getElementsByTagName("head")[0];
        if (min == 0)
        {
            var cacheControl = document.createElement("meta");
            cacheControl.setAttribute("http-equiv", "cache-control");
            cacheControl.setAttribute("content", "no-cache");
            var pragma = document.createElement("meta");
            pragma.setAttribute("http-equiv", "pragma");
            pragma.setAttribute("content", "no-cache");

            head.appendChild(cacheControl);
            head.appendChild(pragma);
        }
        else
        {
            var expires = document.createElement("meta");
            expires.setAttribute("http-equiv", "expires");
            expires.setAttribute("content", Utils.CalcGMTTime(min));
            head.appendChild(expires);
        }
    },
    SetEnterKeyAction: function (arr, actionId)
    {
        for (i = 0; i < arr.length; i++)
        {
            var id = arr[i];
            $('#' + id).bind('keypress', function (e)
            {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13)
                {
                    $('#' + actionId).trigger('click');
                }
            });
        }
    },
    //GET METHODS
    GetCenter: function (id)
    {
        var t = (($(window).height() - $('#' + id).outerHeight()) / 2) + $(window).scrollTop();
        var l = (($(window).width() - $('#' + id).outerWidth()) / 2) + $(window).scrollLeft();
        return { Top: t, Left: l };
    },
    GetDomain: function ()
    {
        var arr = location.href.split('/');
        return arr[0] + '//' + arr[2];
    },
    GetQS: function (name)
    {
        var value = "";
        var qs = "";
        try
        {
            qs = location.href.split('?')[1];
            for (i = 0; i < qs.length; i++)
            {
                if (qs.split('=')[0] == name)
                {
                    value = qs.split('=')[1];
                    break;
                }
            }
        } catch (e) { };

        return value;
    },
    GetArrayDiff: function (a, b)
    {
        var seen = [], diff = [];
        for (var i = 0; i < b.length; i++)
        {
            seen[b[i]] = true;
        }
        for (var i = 0; i < a.length; i++)
        {
            if (!seen[a[i]])
            {
                diff.push(a[i]);
            }
        }

        return diff;
    },
    //HTML METHODS
    HtmlEncode: function (value)
    {
        return $('<div/>').text(txt).html();
    },
    HtmlDecode: function (value)
    {
        return $('<div/>').html(value).text();
    },
    //FORMAT METHODS
    FormatNumber: function (number)
    {
        number += "";
        var x = number.split('.');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1))
        {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    },
    FormatJSONDate: function (s)
    {
        var dateString = s.substr(6);
        dateString = dateString.substr(0, dateString.length - 2);

        var t = parseInt(dateString);


        var d = new Date();
        d.setTime(t);

        return d.toDateString();
    },    
    RemoveSpecialChars: function (safeChars, s)
    {
        var forbid;
        var v = "";
        var chars = ["~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "+", "=", "{", "[", "}", "]", "\\", "|", ";", ":", '"', "'", "<", ">", ".", ",", "?", "/"];
        if (safeChars.length > 0)
        {
            forbid = Utils.GetArrayDiff(chars, safeChars);
        }
        else
        {
            forbid = chars;
        }

        for (i = 0; i < s.length; i++)
        {
            if (forbid.indexOf(s.charAt(i)) != -1)
            {
                //remove character
                v += '';
            }
            else
            {
                //reassign character to new value
                v += s.charAt(i);
            }
        }

        return v;
    },
    ////////////////////////////////////////////////
    // INTERNAL FUNCTIONS NOT TO BE USED DIRECTLY //
    ////////////////////////////////////////////////    
    CalcGMTTime: function (min)
    {
        var currentTime = new Date();
        var exp = new Date(currentTime);
        exp = new Date(exp.setMinutes(currentTime.getMinutes() + min));

        return exp.toUTCString();
    }

};
