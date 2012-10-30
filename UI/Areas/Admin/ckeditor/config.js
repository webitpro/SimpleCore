/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/


CKEDITOR.editorConfig = function (config)
{

    //config.htmlEncodeOutput = false;

    //fonts
    config.font_names = 'Century Gothic;';

    //templates
    config.extraPlugins = 'templates';

    //styles Set
    config.stylesSet = [];
    //config.stylesSet = 'CMS:/Content/ckeditor/custom/styles.js';

    //toolbar
    config.toolbarCanCollapse = false;
    config.toolbar = 'CMS';
    config.toolbar_CMS =
    [
        { name: 'document', items: ['Source'] },
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'] },        
        { name: 'styles', items: ['FontSize'] },
        { name: 'colors', items: ['TextColor', 'BGColor'] },
        { name: 'alignment', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'] },
         { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
        { name: 'insert', items: ['Image', 'MediaEmbed', 'Flash', 'Table', 'HorizontalRule', 'SpecialChar'] },
        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'SpellChecker', 'Scayt'] },
        { name: 'editing', items: ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'] },
        { name: 'tools', items: ['Maximize', 'ShowBlocks'] }
    ];


};