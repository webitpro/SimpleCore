var NavEngine =
{
    FormatUrl: function (id)
    {
        var s = $('#' + id).val();
        var v = "";

        var arr = s.split(' ');
        var safeChars = [];

        for (var i = 0; i < arr.length; i++)
        {
            v += "-" + Utils.RemoveSpecialChars(safeChars, arr[i].toLowerCase());
        }

        return v.substr(1);
    },
    FormatInput: function (id)
    {
        var chars = ["@", "#", "$", "^", "*", "=", "{", "[", "}", "]", "\\", ";", '"', "<", ">"];
        var s = $('#' + id).val();
        var v = "";
        for (i = 0; i < s.length; i++)
        {
            if (chars.indexOf(s.charAt(i)) != -1)
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
    }
};