var Load =
{
    //preload images
    Images: function (arr, basePath)
    {
        /*create an array of image paths
        applying base path to each image name from array*/
        if (basePath)
        {
            for (i = 0; i < arr.length; i++)
            {
                arr[i] = basePath + arr[i];
            }
        }

        //loop through array of full image paths
        for (i = 0; i < arr.length; i++)
        {
            new Image().src = arr[i];
        }
    },
    //load custom scripts
    CSS: function (path, attr, callback)
    {
        if (!this.HaveCSSScript(path))
        {
            var head = document.getElementsByTagName("head")[0]; //head element
            var type = "text/css";
            var tag = null;
            tag = document.createElement("link");
            tag.type = type;
            tag.async = true;
            tag.href = path;
            if (attr)
            {
                for (n = 0; n < attr.length; n++)
                {
                    tag.setAttribute(attr[n].name, attr[n].value);
                }
            }

            if (tag.rel == "")
            {
                tag.rel = "stylesheet";
            }
            if (navigator.appName.indexOf("Microsoft") != -1)
            {
                //IE specific
                tag.onreadystatechange = function ()
                {
                    if (tag.readyState == "loaded" || tag.readyState == "complete")
                    {
                        if (callback)
                        {
                            callback(); //execute callback function
                        }
                        tag.onreadystatechange = null; //reset event handler
                    }
                };
            }
            else
            {
                tag.onload = function ()
                {
                    if (callback)
                    {
                        callback();
                    }
                };
            }
        }
        else
        {
            if (callback)
            {
                callback();
            }
        }

    },
    JS: function (path, async, callback)
    {
        if (!this.HaveJSScript(path))
        {
            var head = document.getElementsByTagName("head")[0]; //head element
            var type = "text/javascript";
            var tag = null;

            tag = document.createElement("script");
            tag.type = type;
            tag.src = path;
            if (async)
            {
                tag.async = true;
            }

            if (navigator.appName.indexOf("Microsoft") != -1)
            {
                //IE specific
                tag.onreadystatechange = function ()
                {
                    if (tag.readyState == "loaded" || tag.readyState == "complete")
                    {
                        if (callback)
                        {
                            callback(); //execute callback function
                        }
                        tag.onreadystatechange = null; //reset event handler
                    }
                };
            }
            else
            {
                tag.onload = function ()
                {
                    if (callback)
                    {
                        callback();
                    }
                };
            }


            /*append dynamically created tag 
            to the head element*/
            head.appendChild(tag);
        }
        else
        {
            if (callback)
            {
                callback();
            }
        }
    },
    HaveJSScript: function (url)
    {
        var loaded = false;
        $('script').each(function ()
        {
            if ($(this).attr('src') == url)
            {
                loaded = true;
            }
        });

        return loaded;

    },
    HaveCSSScript: function (url)
    {
        var loaded = false;
        $('link').each(function ()
        {
            if ($(this).attr('href') == url)
            {
                loaded = true;
            }
        });

        return loaded;
    }

};