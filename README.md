# Equipment-Rental-Web-Application
ASP.NET Core 3.0 web application that is used for keeping track of who is assigned what devices, a devices rental histroy, and a form for users to submit to request rental equipment.

### APPLICATIONS NEEDED 
- ASP.NET Core 3.0 hosting environment and SDK
- Visual Studio 2019 Preview
- Microsoft SQL Server 2017
- Notepad++ or use sed type commands

### Install Required Application Packages
Issue the below commands in Visual Studio 2019 Package Manager Console to install required packages.
```powershell
Add-Package BootstrapMVC.Bootstrap4
Add-Package bootstrap.daterangepicker
Add-Package  Microsoft.EntityFrameworkCore
Add-Package Microsoft.EntityFrameworkCore.Tools
Add-Package Microsoft.EntityFrameworkCore.SqlServer
Add-Package Microsoft.EntityFrameworkCore.Relational
Add-Package Microsoft.VisualStudio.Web.CodeGeneration.Design
Add-Package Microsoft.Identity.Client
Add-Package Microsoft.Extensions.Logging.Debug
```
If you want to use the dotnet commands you can cd into the second CheckOutForm directory and issue the below commands.
```powershell
dotnet add package BootstrapMVC.Bootstrap4
dotnet add package bootstrap.daterangepicker
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Relational
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.Identity.Client
dotnet add package Microsoft.Extensions.Logging.Debug
```

### SET UP SQL DATABASE SCHEMA
- To configure the SQL Database schema with the application, issue the below commands in the Package Manager Console in Visual Studio 2019.
```powershell
Add-Migration -Name InitialCreate -Context CheckOutFormContext
Update-Database -Context CheckOutFormContext -Migration InitialCreate
```

If you want to use the dotnet commands you can cd into the second CheckOutForm directory and issue the below commands.
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### CUSTOMIZE FOR YOUR SITUATION
Make these changes in order or unwanted changes will occur.
1. The current email domain for this applicaiton is osbornepro.com. Replace @osbornepro.com in all documents with whatever your email domain is.  
2. This applicaiton uses OsbornePro as the company name. Replace OsbornePro with what your companies name is. 
3. SendEmail.cs will require configuration for your email settings as well as the Index.cshtml file in the Home Folder.
4. The following location is for a company logo that will be sent in emails after users submit a request: C:\Users\Public\source\repos\CheckOutForm\CheckOutForm\wwwroot\images\logobannercolored.png 
5. The Approve link in equipment requests emails will auto generate an email that is sent to help@osbornepro.com. This will need to be set to whatever your ticketing system email is.
6. The following link is to send users to a knowldgebase article that teaches users how to change their default email applicaiton so the approve and deny buttons work correctly: https://helpdesk.osbornepro.com/articles/file-does-not-have-an-application-associated-with-it"">HERE</a> or <a href=""https://helpdesk.osbornepro.com/articles/change-default-applicaiton
7. The following are the current values for SQL names. Replace them with your SQL Server info which is located in RentalsController and appsettings.json: SQLServer=SqlServer Instance=SQLDBInstanceName; Database=SQLDB_Name;
8. The summary method in the DevicesController has [A,D]Devices. These can be customized to discover active and retired tablets, laptops, desktops for whatever your naming conventions are.
9. I use AzureAD for authentication. If you do not want to use Azure for authentication delete the bottom config lines in appsettings.json and the Extensions directory. Also remove the Azure and Authentication packages from CheckOutForm.sln file. 
10. On the devices page I use the color green to show any devices that can be used as loaners. These devices will show up in the dropdown menu when creating a new Rental Database entry. Red is for retired devices. These colors are set in the /Views/Devices/Index.cshtml file.
11. I use colors in the Rentals table as well. Feel free to define these as whatever you want in the Views/Rentals/Index.cshtml file


### Summary
    The purpose of this web application is to allow users to submit an equipment rental request for approval.
    The range of dates will vary based on the renter's selection. A limit of 30 days has been set. This range can be edited via the __/wwwdata/js/script.js__ file.
    __EMAIL:__ If you do not use SMTP2GO you will need to configure the SendEmail function to use authentication based on your SMTP Server. I recommend SMTP2GO as it does not require clear text credentials to be store in a file somewhere.
    __SQL:__ You will need to define a SQL server based on your environment for this applicaiton to use.
    __EXPORT-CSV__ There is a button in the application that exports the SQL databases rental history for the last year. This creates a CSV file based on SQL Servers data and saved it to the source directory on the web server. 
    __NOTE:__ Any attempts to visit the non ssl, http version of the site, should be redirected to the https locationu using the web hosts ruels.

    ##### Authentication
    After a user with permission to access the device database authenticates their identity, they will be taken a new site extension with the ability to search, view, and edit company device data.
    Authentication is handled through Azure AD integration. Members in a group defined by IT have the ability to access the full functionality of the web application after signing in.
    When someone signs in that does not have access to the database they receive an Error Code 500 and they will be denied access.
    Unauthorized users will not be able to access the SQL database or any of it’s features.

    ##### Data Functions
    The columns in the database table can be organized by ascending or descending order. This is done by clicking the blue link column header.
    The database may be searched by assignee name, a device’s asset tag, model name, or device type. Other parameters can be added if ever requested.
    There are presently 10 items displayed per page. This can also be changed if ever requested. The same features are available in the rentals area.
    Any of the entries can be deleted, edited, or shown in a detailed view. New entries can be added through the database using the web application's GUI.
    The status field for the rental database pulls from a list of statuses. More options can be added to this status list if ever needed and unused entries can be removed.

    ##### Logging
    Any and all changes are currently logged through the web platform, the operating system, and the cloud. Custom log messages will be added in a future version to further simplify troubleshooting.
    Error handling has been configured. Please report any errors that do not respond as expected.

    ##### Platform Testing
    Internet Explorer and Edge will not display table outlines. Firefox and Chrome behave as expected.
    Protection against SQL injections and CSRF have been implemented. Penetration testing has been performed to ensure the security of the application. There are no presently discovered vulnerabilities.

If you would like to add or make changes to this applicaiton feel free to send them to me and I will give credit or give you a branch to work off.

CONTACT ME: rosborne@osbornepro.com

