# Real Estate Management System - Properly
 ⚠️*This application, Properly, was developed for educational purposes, It is not intended for production use.*
## Overview
- __Properly__ is a full-stack ASP.NET application designed for managing properties, rentals, and leases. It enables users to buy, sell, or rent properties, providing seamless interactions between property owners, tenants, and administrators. This application is designed with a scalable architecture and incorporates role-based functionalities for various types of users.

![homepage](https://github.com/user-attachments/assets/74f59f79-e24f-4423-9ffd-25efc6333d69)

## Content
 - [Overview](#overview)
 - [Architecture](#architecture)
 - [Technologies Used](#technologies-used)
 - [Features](#features)
 - [Photos and Videos](#photos-and-videos)
 - [Setup and Installation](#setup-and-installation)
 - [Feature Enhancements](#feature-enhancements)
 - [Credits](#credits)
 
 
## Architecture
  Properly is structured following a modular architecture to keep concerns separated and improve maintainability. The main components include:

  ### 1. Data Layer
- [Properly.Data](https://github.com/AwakexD/Properly/tree/main/Data/Properly.Data): Contains configurations for all entities, repository implementations, the `DbContext`, and database seeding.
- [Properly.Data.Common](https://github.com/AwakexD/Properly/tree/main/Data/Properly.Data.Common): Provides abstract generic classes and interfaces, such as `IDeletableEntityRepository` and `IRepository`, for data handling.
- [Properly.Data.Models](https://github.com/AwakexD/Properly/tree/main/Data/Properly.Data): Houses all entity classes, representing various database models for properties, users, and listings.
  ### 2. Service Layer
- [Properly.Services.Data](https://github.com/AwakexD/Properly/tree/main/Services/Properly.Services.Data): Implements the core business logic across different services.
- [Properly.Services.Mapping](https://github.com/AwakexD/Properly/tree/main/Services/Properly.Services.Mapping): Contains AutoMapper extension methods and mapping configuration.
- [Properly.Services.Messaging](https://github.com/AwakexD/Properly/tree/main/Services/Properly.Services.Messaging): Integrates with __SendGrid API__ for email communication.
  ### 3. Web Layer
- [Properly.Web](https://github.com/AwakexD/Properly/tree/main/Web/Properly.Web): Contains the main web application, including controllers, views, and SignalR hubs.
- [Properly.Web.Infrastructure](https://github.com/AwakexD/Properly/tree/main/Web/Properly.Web.Infrastructure): Custom model binders and validation attributes.
- [Properly.Web.ViewModels](https://github.com/AwakexD/Properly/tree/main/Web/Properly.Web.ViewModels): Contains view models and form models.
  ### 4. Testing Layer
- [Properly.Services.Data.Tests](https://github.com/AwakexD/Properly/tree/main/Tests/Properly.Services.Data.Tests): Contains unit tests for the service layer.
- [Properly.Web.Tests](https://github.com/AwakexD/Properly/tree/main/Tests/Properly.Web.Tests): Contains tests for the web layer.
  ### 5. Common
- [Properly.Common](https://github.com/AwakexD/Properly/tree/main/Properly.Common): Stores global constants used across the application.

## Technologies Used

### 1. Frontend
  - __HomeZen Bootstrap Theme__: Custom UI theme for a professional look and feel. [Theme link](https://themeforest.net/item/homzen-real-estate-html-template/51943020?srsltid=AfmBOoovYaSAAtiojvS2s2Um6uOoYMm9rqpPs1Bxwt9WsO3P2dNoAri2)
  - __SweetAlert__: To display alerts and messages to users.
  - __jQuery and Vanilla JS__: For dynamic frontend functionality.
  - __DataTables JS__: For interactive data tables in user and admin views.
  - __FontAwesome__: Icon library for UI elements.
### 2. Backend
  - __ASP.NET Core__: Core framework for building a cross-platform web application.
  - __ASP.NET Core Template by Nikolay Kostov__: Initial template structure.
  - __Entity Framework Core__: ORM for database interactions.
  - __Repository Pattern__: Implemented to manage data access through `DbContext`.
  - __SignalR__: Real-time connection for tracking online users, used to display live user activity to the admin.
  - __AutoMapper__: Object mapping between models and view models.
  - __Error and Warning Logging__: Detailed console logging for debugging.
### 3. Utilities
  - __SendGrid__: For sending email notifications to users.
  - __Cloudinary__: For cloud-based image storage.
### 4. Database
  - __SQL Server__: The primary database for storing application data.

## Features
  ### User Features
  - __Property Search__: Users can search properties based on multiple criteria, such as price range, location, property type, and status.
  - __Create and Manage Listings__: Users can create listings (for sale or rent), update their details, and delete them (soft delete). They can mark listings as sold or rented.
  - __Favorites__: Users can save preferred listings to a favorites list for quick access.
  - __Messaging__: Users can send messages to communicate with each other about properties, allowing for questions and appointments for property viewings.
  ### Admin Features
  - __User Management__: Admins can view all registered users, see real-time online status (via __SignalR__), and manage users as needed.
   - __Listing Management__: Admins have full control over all listings on the platform, including create, update, and soft delete options.
  - __Static Data Management__: Admins can manage static data (like `PropertyTypes`, `ListingStatuses`, etc.) to ensure that the application remains flexible for various property types and categories.
  ### Database Features
 - __Static Data Seeding__: During database creation, essential data (`ListingStatuses`, `ListingTypes`, `PropertyTypes`, etc.) is automatically seeded for ease of use and standardization.

## Photos and Videos
 

https://github.com/user-attachments/assets/8cca00a3-4fe1-4fb4-8108-24bdf0bd28cf

![properties-for-rent](https://github.com/user-attachments/assets/ec482159-0fe2-4644-8199-2209a6abe4ed)

https://github.com/user-attachments/assets/69bf8279-fc1d-41b4-ba98-e11f44fe6e77

https://github.com/user-attachments/assets/9abe2401-e13d-4ce3-81a5-cc3c0eb3d214

https://github.com/user-attachments/assets/ac402e51-76f0-470f-9706-b309ec8a9924

![submit-property](https://github.com/user-attachments/assets/2b22f170-b158-4c26-b7bf-2a45d73cf9be)

https://github.com/user-attachments/assets/a00d1670-c324-4b9a-b95e-0c5201be7413

![admin-dashboard](https://github.com/user-attachments/assets/acdce032-aeac-412d-8460-ebab49312adb)

https://github.com/user-attachments/assets/27bc0e1b-8da4-4ef2-9373-d5db2a1937d9

![admin-features](https://github.com/user-attachments/assets/ebc0087e-9943-4da1-8b94-188473937d31)

![admin-listings](https://github.com/user-attachments/assets/a40461ba-9028-4d09-85bf-d1a684648e34)


## Setup and Installation
   __1. Clone the repository__
  ```
   git clone https://github.com/yourusername/Properly.git
   cd Properly
  ```
  __2. Database Setup__
  - Configure your SQL Server connection in `appsettings.json`.
  - Run migrations to set up the database schema.
    
  __3. Configure External Services__
  - Store external service keys in a `.env` file in the root directory. Add the following keys: 
  ```
   CLOUDINARY_URL=<Your Cloudinary API Key>
   SENDGRID_API_KEY=<Your SendGrid API Key>
  ```
  - Ensure that `.env` is properly loaded to use these variables securely.
    
  __4. Build and Run__
  ```
   dotnet build
   dotnet run
  ```
 __5. Admin Credentials__
 - Email : `admin@gmail.com`
 - Password : `Adm1nPassw@rd!`
 ## Feature Enhancements
  - __Review and Rating System for Properties and Agents__
  - __Integrated Calendar for Appointments__
  - __Multi-Language Support__
  - __Advanced Analytics Dashboard__
    
## Credits
⚠️ __Note__: *This application, Properly, was developed for educational purposes, It is not intended for production use.*

- __ASP.NET Core Template by Nikolay Kostov__: [Template link](https://github.com/NikolayIT/ASP.NET-Core-Template)
- __Theme__: [HomeZen Bootstrap theme](https://themeforest.net/item/homzen-real-estate-html-template/51943020?srsltid=AfmBOoovYaSAAtiojvS2s2Um6uOoYMm9rqpPs1Bxwt9WsO3P2dNoAri2)
- __SendGrid__: For email functionality.
- __Cloudinary__: For image hosting and storage.
  
