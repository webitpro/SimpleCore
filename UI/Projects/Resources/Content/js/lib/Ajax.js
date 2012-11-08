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
        var global = $('p.error');
        var individual = $('span.error');

        //reset errors
        global.html('');
        individual.html('');

        //not successful
        for (i = 0; i < r.Errors.length; i++)
        {
            var error = r.Errors[i];

            if (error.Id != "")
            {
                //specific error
                $('#' + error.Id).next('span.error').html(error.Text).fadeTo('slow', 1);
            }
            else
            {
                //global error
                global.append(error.Text + '<br />');
            }
        }

        global.fadeTo('slow', 1);
    },
    Clear: function ()
    {
        $('p.error').fadeTo('fast', 0);
        $('p.error').html('');
        $('span.error').fadeTo('fast', 0);
        $('span.error').html('');
    },
    Init: function (id)
    {
        Ajax.Clear();

        var position = $(id).position();
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