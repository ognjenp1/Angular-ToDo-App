# To-Do List

Is a web application for maintaining to-do lists (like [Google Keep](https://keep.google.com/)). The application consists of 3 segments:
* [API](#the-api)
* [Web Client](#web-client)
* [Security & Deployment](#security-&-deployment)

## The API

An REST API solution which exposes CRUD & search endpoints. Reminder functionality 
that runs as separate service. 

The API supports the following:

* Preview of to-do-list.
* Update of to-do list, title & list items (including list/item reordering).
* Creation of to-do list containing list title & list items.
* Removal of to-do list.
* Searching the to-do lists by title (with partial match & case insensitive, e.g. if search criteria is "Dark", the result should contain item with name "darko").
* Logging functionality.
* Reminder functionality implying email sending for all of to-do-lists which remindMe date has expired.

The API includes the following technologies/service providers:
* .Net Core 2 (Web API)
* Entity Framework Core 2
* [SendGrid](https://sendgrid.com/) email server provider

 
## Web Client

The API is used by the client web application. The client application 
provides user interface which supports the 
following:

* Dashboard page containing the list/grid of all available to-do lists with search input. "Reminded" to-do lists are be on top and with proper indicator.
* Page/popup for creating/editing of to-do list/items. To-do list/item created/update on focus lost event (like in Google Keep).
* Removal of to-do lists/items.
* To-do lists/items position change via drag-and-drop.
* "RemindMe" logic input fields (add/remove and validations).
 
The client includes the following technologies:
* Angular 7


## Security & Deployment

Web client & the API supports authentication and authorization.

* Web client/the api authentication via bearer token provided from Auth0 identity provider.
* Web client/the api authorization.

Security includes the following technologies/service provider
* OAuth 2.0 (via [Auth0](https://auth0.com/) identity provider)
