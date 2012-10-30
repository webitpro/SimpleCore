/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/
CKEDITOR.addTemplates('default',
{
    // The name of the subfolder that contains the preview images of the templates.
    imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),

    // Template definitions.
    templates:
		[
            {
                title: 'Sidebar Template',
                image: 'sidebarTemplate.jpg',
                description: 'Body and Sidebar Template',
                html:
                    '<p>Type your paragraph here. Repeat this structure if necessary. Sidebar component is loaded dynamically through the database.</p>'
                    + '<ul>'
                        + '<li>List Item</li>'
                         + '<li>List Item</li>'
                    + '</ul>'

            },
            {
                title: 'FAQ',
                image: 'faqTemplate.jpg',
                description: 'Question and Answer Template',
                html:
                    '<h5>Q:Question?</h5>'
                    + '<div class="a">A:Answer Paragraph.</div>'
                    + '<h5>Q:Question?</h5>'
                    + '<div class="a">A:Answer Paragraph.</div>'
            },
			{
			    title: 'Document Library',
			    image: 'docLibTemplate.jpg',
			    description: 'Library of Documents Template',
			    html:
					'<p>Type your introduction paragraph here. Filter and Document List components are loaded dynamically through the database.</p>'
			},
			{
			    title: 'Employee List',
			    image: 'employeeListTemplate.jpg',
			    description: 'Employee List Template',
			    html:
					'<p>Type your introduction paragraph here. Employee List component is loaded dynamically through the database.</p>'
			}
		]
});