Solution

	Admin
		Content Management System / Administration section of the solution
		As a default it has Account Management and Engine Management consisting of Page and Navigation Management.
		As needed you can add new Controllers and Views to define administration options for the site you’re implementing. 
		The routing for Administration section is based on the default MVC engine; Controller/Action/Id
	API
		Allows for Ajax calls from the public facing site (UI Project) to call server side functionality and receive 
		response as JSON formatted, serialized object.
		Allows for building a publicly open API to enable integration of a third party apps with your app
		As needed you can add new Controllers and methods within to define some server side functionality, open up an API 
		for a third party app to integrate with your application etc.
	Helpers
		Library of classes to help with development and MVC framework
		DB Context definitions, Configuration (Web.config), Enumerators, Form Elements (Dropdown List, Multi-Select List, 
		Action Link), Meta Tags, Security, Error Handling, Engine, API Responses, Digital Files and their types, Action 
		Results, Integration with Google Maps API, Paging for lists, Parser for CSV and dynamic assigning of JS/CSS scripts.
		_Ext directory contains partial classes that should be used to add additional definitions such as database context, 
		configuration, enumerators, form elements, meta-tags and security roles. 
		Besides updating and changing the most common items within the _Ext directory and partial classes contained in that 
		directory, you can add and/or modify existing classes under this project. Modification of the existing/required 
		classes may affect the engine so it is not recommended unless you fully understand how the engine works with all 
		dependencies throughout the solution.
	Library
		Library of static classes to help validate, send requests and read responses, deal with audio or image files, 
		query string, arrays, email etc.
		As needed you can add and/or modify existing classes in order to enhance or extend functionality.
	Models
		Consists of Models (required for the engine to run) covering Administration and Public Facing portion of the website.
		Tab, Section, Link are part of the Navigation / Routing Engine
		Page, Static Page, Custom Page, Accordion, PageComponents and Intro are part of Page and Page Template definitions
		Account is part of the Administration section and defines the basic model of the user with different security roles
		Additional models required by the solution should be added under this project. 
		If there is a need you could modify existing models in order to extend the information architecture.
	Resources
		All of your JavaScript, CSS, xml, uploaded files etc. are loaded within this project.
		Some files are required while others are just additions and libraries to be used.
		As needed you can add new files; CSS, JavaScript etc.
		Reference class methods allow for dynamic creation of the path to the resource files
	UI
		Public facing site with the core engine that reads/displays the pages created through the Administration section.
		Projects and Areas contain other projects within this solution but they CANNOT be “included”, otherwise there will be 
		some conflicts when trying to build-up solution
		UIController, Index page under /Views/UI/ and files under Templates directory are the core of the engine. DO NOT 
		delete these files! If you fully understand the engine you could modify these files to extend or enhance the 
		functionality.
		Other files could be changed as needed.
		Static Pages and an Accordion are created through the database but Custom Pages are created as an actual 
		Controller/View pair within the UI Project as needed and then the references required for the engine to run are 
		added through the database for those.
