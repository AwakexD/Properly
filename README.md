# Real Estate Property Management System - Properly

## Overview
- __Properly__ is a full-stack ASP.NET application designed for managing properties, rentals, and leases. It enables users to buy, sell, or rent properties, providing seamless interactions between property owners, tenants, and administrators. This application is designed with a scalable architecture and incorporates role-based functionalities for various types of users.

## Content
 - [Overview](#overview)
 - [Architecture](#architecture)
 - [Technologies Used](#technologies-used)
 - [Features](#features)
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

## Setup and Installation

## Feature Enhancements

## Credits
⚠️ __Note__: *This application, Properly, was developed for educational purposes, It is not intended for production use.*

- __ASP.NET Core Template by Nikolay Kostov__: [Template link](https://github.com/NikolayIT/ASP.NET-Core-Template)
- __Theme__: [HomeZen Bootstrap theme](https://themeforest.net/item/homzen-real-estate-html-template/51943020?srsltid=AfmBOoovYaSAAtiojvS2s2Um6uOoYMm9rqpPs1Bxwt9WsO3P2dNoAri2)
- __SendGrid__: For email functionality.
- __Cloudinary__: For image hosting and storage.
  
