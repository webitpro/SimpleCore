var Page =
{
    Reload: function (e)
    {
        $(e).slideUp("slow", function () { location.reload() })
    }
};