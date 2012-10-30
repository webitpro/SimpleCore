var Ajax =
{
    Run: function (url, data, callback)
    {
        $.ajax(
        {
            url: url,
            type: "POST",
            data: data,
            dataType: "json",
            cache: false,
            async: true,
            success: function (r)
            {
                return callback(r);
            }
        });
    },
    Error: function (r)
    {
        //reset errors
        $('.form p.error').html('');
        $('.form span.error').html('');

        //not successful
        for (i = 0; i < r.Errors.length; i++)
        {
            var error = r.Errors[i];

            if (error.Id != "")
            {
                //specific error
                $('#' + error.Id).next('span.error').html(error.Message).fadeTo('slow', 1);

            }
            else
            {
                //global error
                $('.form p.error').append(error.Message + '<br />');
            }
        }

        $('.form p.error').fadeTo('slow', 1);
    },
    Clear: function ()
    {
        $('.form p.error').fadeTo('fast', 0);
        $('.form p.error').html('');
        $('.form span.error').fadeTo('fast', 0);
        $('.form span.error').html('');
    },
    Init: function ()
    {
        Ajax.Clear();

        var position = $('.container h1:first').position();
        var left = position.left - 30;
        var top = position.top + 10;

        $('#ajaxProcessing').css('left', left + 'px').css('top', top + 'px').fadeIn('fast');

    },
    Stop: function ()
    {
        $('#ajaxProcessing').fadeOut('fast');
    },
    SetUploader: function (form, path)
    {
        var base = '/Projects/Resources/Content/js/lib/tools/ajaxupload/';
        var jQueryBlockUI = base + 'jquery.blockUI.min.js';
        var jQueryForm = base + 'jquery.form.min.js';

        Load.JS(jQueryForm, true, function ()
        {
            Load.JS(jQueryBlockUI, true, function ()
            {
                var form = $('#' + form);
                var path = $('#' + path);
                form.ajaxForm(
                {
                    iframe: true,
                    type: 'post',
                    dataType: "json",
                    clearForm: true,
                    complete: function (xhr)
                    {
                        var response = xhr.responseText.replace('<textarea>', '').replace('</textarea>', '');
                        response = $.parseJSON(response);
                        if (response.Success)
                        {
                            $.growlUI(null, 'Success uploading file');
                            path.val(response.Result.Path).show('slow');
                        }
                        else
                        {
                            $.growlUI(null, 'Error uploading file');
                            path.next('span.error').html(response.Errors[0].Message).fadeTo('slow', 1);
                        }
                        form.unblock();
                        form.resetForm();
                    },
                    beforeSubmit: function ()
                    {
                        path.next('span.error').fadeTo('fast', 0).html('');
                        form.block({ message: '<h6><img src="/Projects/Resources/Content/img/ajax-loader.gif" />Uploading file...</h6>' });
                    }
                });
            });
        });
    }
};