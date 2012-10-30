var delim = ",";
var tabSavePath = "/API/Engine/SaveTabOrder/";
var sectionSavePath = "/API/Engine/SaveSectionOrder/";
var linkSavePath = "/API/Engine/SaveLinkOrder/";
var accordionSavePath = "/API/Engine/SaveAccordionItemsOrder/";


var Sort =
{
    Open: function (e, id, title)
    {
        $('#' + id).css('left', $(e).position().left);
        $('#' + id).css('top', $(e).position().top + 20);
        $('#' + id).show();
        $('#' + id + ' div.header').html(title);
    },

    Close: function (id)
    {
        $('#' + id).hide();
    },

    Up: function (id)
    {
        var selectedItem = $('#' + id + ' option:selected');
        var prevItem = selectedItem.prev("option");
        if ($(prevItem).text() != "")
        {
            $(selectedItem).remove();
            $(prevItem).before($(selectedItem));
        }
    },

    Down: function (id)
    {
        var selectedItem = $('#' + id + ' option:selected');
        var nextItem = selectedItem.next('option');
        if ($(nextItem).text() != "")
        {
            $(selectedItem).remove();
            $(nextItem).after($(selectedItem));
        }
    },

    Save: function (id, windowId, option, parentStructureId)
    {
        switch (option)
        {
            case 'tab':
                Sort.SaveTabs(id, windowId);
                break;
            case 'section':
                Sort.SaveSections(parentStructureId, id, windowId);
                break;
            case 'link':
                Sort.SaveLinks(parentStructureId, id, windowId);
                break;
            case 'accordion':
                Sort.SaveAccordions(parentStructureId, id, windowId);
                break; ;
        }
    },

    GetOrder: function (id, delim)
    {
        var order = "";
        var option = $('#' + id + ' option');
        option.each(function (index)
        {
            order += delim + $(this).val();
        });
        order = ((order.length >= 1) ? order.substr(1) : order);

        return order;
    },

    SaveTabs: function (id, windowId)
    {
        var order = Sort.GetOrder(id, delim);
        Ajax.Run(tabSavePath, { order: order, delim: delim }, function (r)
        {
            if (r.Success)
            {
                Sort.Close(windowId);
                Page.Reload('div.actionBox');
            }
            else
            {
                console.log(r.Error);
            }
        });
    },

    SaveSections: function (tabId, id, windowId)
    {
        var order = Sort.GetOrder(id, delim);
        Ajax.Run(sectionSavePath, { tabId: tabId, order: order, delim: delim }, function (r)
        {
            if (r.Success)
            {
                Sort.Close(windowId);
                Page.Reload('div.actionBox');
            }
            else
            {
                console.log(r.Error);
            }
        });
    },

    SaveLinks: function (sectionId, id, windowId)
    {
        var order = Sort.GetOrder(id, delim);

        Ajax.Run(linkSavePath, { sectionId: sectionId, order: order, delim: delim }, function (r)
        {
            if (r.Success)
            {
                Sort.Close(windowId);
                Page.Reload('div.actionBox');
            }
            else
            {
                console.log(r.Error);
            }
        });
    },

    SaveAccordions: function (pageId, id, windowId)
    {
        var order = Sort.GetOrder(id, delim);
        Ajax.Run(accordionSavePath, { pageId: pageId, order: order, delim: delim }, function (r)
        {
            if (r.Success)
            {
                Sort.Close(windowId);
                Page.Reload('div.actionBox');
            }
            else
            {
                console.log(r.Error);
            }
        });
    }
};