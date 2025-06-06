SOMAX

Overview

SOMAX is a simple ASP.NET MVC web application designed to demonstrate the Model-View-Controller (MVC) architecture. It is an Asset Management Product.It is an affordable asset monitoring using preventive maintenance.It is an user friendly data representation through easy to understand dashboards and mobile alerts.An important aspect of Somax is
security.Due to security many functionalities are shown or hide with respect to specific user . Notification is also an important aspect in Somax application. Notification features are implemented in Somax with respect to some functionality example Asset add,edit, Workorder Add,edit etc.
SOMAX Application is available to use on any Mobile device that can use a standard browser such as Chrome, Safari, Edge or FireFox.The SOMAX Web Application is Mobile Friendly. There is no need to download anything from any web store. All SOMAX functions are available from any mobile device just by going to the web page and signing in.

Features

• Asset management with  CRUD operations.
• Responsive UI built with Bootstrap.
• Input validation on both client and server sides.
• Database integration using Layers(DataContract,Transactions.Businesss,Stored Procedures)
• Logging and exception handling.

Technologies Used

• Framework: ASP.NET MVC
• Language: C#
• Database: SQL Server
• Frontend: Bootstrap, jQuery
• ADO.NET Framework,LINQ
• Tools: Visual Studio, NuGet,Devexpress,Mobi Scroll(Mobile Friendly)

Prerequisites

• Visual Studio 2022.
•.NET Framework 4.8
• SQL Server 2019.

Installation
• Clone the repository:
• Open the project in Visual Studio.
• Restore NuGet packages
• Update the database connection string in the web.config file
• Build and run the application 
• Close Visual Studio and Install  Devexxpress Components 24.1.5 version.

Project Structure

The application follows the standard ASP.NET MVC folder structure:
• /Controllers: Contains controller classes that handle HTTP requests and business logic.
• /Models: Contains data models representing the application's data structure.
• /BusinessWrapper: Encapsulate the business logic of an application.
• /Views: Contains Razor views for rendering the user interface.
• /Scripts: Client-side scripts such as jQuery and Bootstrap.
• /Content: Stylesheets and other static assets.
• /App_Start: Configuration files for routing, bundling, etc.
• /bin: Compiled binaries and dependencies.
• /obj: Temporary object files.
• /Properties: Project properties and settings.
• /Localization: Resource files for localization.
• /CustomValidation: Create Validation logic for model properties.
• /ActionFilters: Custom action filters.
• /DevExpressReport: Template used for Deveexpress Report.
• /ErrorLog: Log files for debugging and monitoring.

Usage

• Dashboard Page
The landing page provides an overview and links to the Different modules.

• Assets
  In SOMAX, an asset is defined by a piece of equipment in your company. For example, they can be a physical piece of equipment, a location, or a vehicle, and can look like: 
   • conveyor systems
   • assembly lines
   • pumps and valves
   • industrial ovens
   • mixers
   • trucks
   • forklifts

• WorkOrder
  The Work Order Workflow
   Step 1: Submitting Work Requests (Optional)
   Step 2: Approving/Denying Requests and Creating Work Orders
   Step 3: Scheduling Work Orders
   Step 4: Completing Work Orders
   Step 5: Follow Up Work Orders (Optional)

• Preventive Maintenance
     Preventive maintenance or PM as it is commonly called, is a proactive type of maintenance that is designed to prevent equipment failures and enhance its efficiency and lifespan. By using a regular preventive maintenance schedule, you can minimize asset downtime (and decrease work order costs) as you work to prevent and better prepare for unexpected failures.

• Part
  Parts Inventory means repair parts held for resale and used to service farm implements,machinery, attachments, utility and light industrial equipment, and yard and garden quipment.

• Vendors
  A vendor is an entity or company that does business with your organization. For example, a vendor can be any company that:
  • Supplies parts
  • Provides assets 
  • Provides service to your assets

• Purchase Request
  A purchase request is a request made by an employee to the purchasing department for the purchase of goods or services. It is a type of document that is used to initiate the purchasing process. The purchase request is then reviewed by the purchasing department to determine if the purchase is necessary and if it is within the budget.

• Purchase Order
  A purchase order (PO) is a commercial document and first official offer issued by a buyer to a seller indicating types, quantities, and agreed prices for products or services. It is used to control the purchasing of products and services from external suppliers.

• Invoice Matching
  Invoice matching is a process that ensures that the invoice received from a vendor matches the purchase order and the goods or services received. It is an essential part of the accounts payable process and helps to prevent errors and discrepancies in the payment process.

• Sanitation Jobs
   The Sanitation Workflow
   Step 0: Submitting Sanitation Requests (Optional)
   Step 1: Creating Sanitation Jobs
   Step 2: Scheduling Sanitation Jobs
   Step 3: Completing Sanitation Jobs
   Step 4: Verifying Sanitation Jobs

• Fleet
   • Assets--The Fleet – Asset module will allow SOMAX clients to track vehicles in their fleet.
   • Meter History--The Fleet – Meter will allow SOMAX clients to enter and track vehicles odometer and hour meter readings
   • Fuel--The Fleet – Fuel Tracking module will allow SOMAX clients to enter and track vehicles fuel consumption.
   • Scheduled Service--The Fleet – Scheduled Service module will allow SOMAX clients to assign tasks to assets that occur on a set schedule based on a time or meter 
     interval.
   • Issues---The Fleet –Issues module will allow the user to view or add issues that come from vehicle inspections or driver observations. Issues are typically one-time problems
    which represent an unexpected or unplanned service need.Examples include Fluid Leak, Brake Failure or Transmission Noise.
   • Service Orders--The Fleet – Service module will allow SOMAX clients to track the maintenance to their vehicles in their fleet. The module will allow the user to track labor,
    materials, and status of all maintenance service orders. It will also update scheduled maintenance records that are associated with a service order.

• Personnel
  The Personnel module will allow SOMAX clients to track the following:
   • Employee Categorization
   • Employee Events – such as training or certifications
   • Employee Availability
   • Employee Attendance
	
• Projects
   The Projects module will allow SOMAX clients to track major projects that are taking place at their facility. The module will allow the user to track labor, materials,
   and status of all project tasks.	
	
• User Management
The user management page is where you can view and edit all of the users' names, contact information, security profiles, and more. 
In SOMAX there are three types of Users that can Access the SOMAX System:
 • Admin Users, these are users that have been granted Administrative Configuration of the SOMAX System.  These users are counted against your Site's License Count
 • Full Users, these are non admin users who can access SOMAX based on their security profile. These users are counted against your Site's License Count.
 • Request Users, these users are Request only users and do not count against your Site's License Count. They are counted and displayed separately on the Configuration Dashboard  There is an Additional User Type known as a Reference User. This type of User cannot access the SOMAX System but can be Referenced on a Work Order or Sanitation Job.

• User Configuration
   With the help of this module we can change the column, add new column,removed new columns of specific functionalities which are mentioned in UIView.

• Custom Security Profiles
  For Enterprise Customers the option is available to create Custom Security Profiles. This enables you to tailor fit your security profiles and user access for your needs.

• Validation
   The app validates input both on the client (using jQuery) and server sides (using data annotations).
