# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is **GoHiking** - an ASP.NET MVC web application for organizing and managing hiking activities. The application allows users to browse hiking trails, join hiking groups, and manage trip activities.

## Development Commands

### Build and Run
- **Build the solution**: `dotnet build` or use Visual Studio (`Ctrl+Shift+B`)
- **Run the application**: Use Visual Studio debugger (`F5`) or IIS Express
- **Development server**: Application runs on `http://localhost:55176/` (configured in project settings)

### Package Management
- **Restore NuGet packages**: `dotnet restore` or use Visual Studio Package Manager
- **Update packages**: Use Visual Studio Package Manager Console or NuGet Package Manager UI

## Architecture Overview

### Technology Stack
- **Backend**: ASP.NET MVC 5 (.NET Framework 4.8)
- **Database**: SQL Server with Entity Framework 6 (Database First approach)
- **Frontend**: Razor Views with Bootstrap, jQuery, and custom JavaScript
- **ORM**: Entity Framework 6.5.1 with EDMX model

### Project Structure

#### Controllers
- `HomeController`: Main landing page, activity listings, blog
- `UserController`: User authentication and management
- `ProductsController`: Product/gear management
- `ProfileController`: User profile management
- `TeamController`: Trip group management
- `TripHelperController`: Trip planning utilities (comments, gear, schedule)

#### Data Layer (`data/` folder)
- `HikingModel.edmx`: Entity Framework database model
- `HikingDBEntities1`: Main DbContext class
- **Core Entities**:
  - `User`, `UserDetail`, `UserProfile`: User management
  - `TripActivity`: Hiking trip activities
  - `TripGroup`: Group formations for trips
  - `MountainTrail`: Mountain trail information
  - `GroupImage`: Images for activities/groups
  - `Comment`: User comments and reviews

#### ViewModels
- `HomeViewModel`: Homepage data aggregation
- `ActivityViewModel`: Trip activity display data
- `UserViewModel`, `UserProfileViewModel`: User-related views
- `TripGroupViewModel`: Group management views

#### Frontend Assets
- **Static HTML templates**: `untree.co-sterial/` (template files)
- **CSS**: Bootstrap + custom styles in `css/` folder
- **JavaScript**: jQuery, Bootstrap, AOS, Flatpickr, GLightbox
- **Images**: Stored in `images/` folder

### Database Configuration
- **Connection String**: Uses SQL Server Express (`.\sqlexpress`)
- **Database Name**: `HikingDB`
- **Authentication**: Integrated Security (Windows Authentication)

### Key Features
1. **Activity Management**: Browse, filter, and join hiking activities
2. **User System**: Registration, login, profile management
3. **Trip Planning**: Group formation, equipment planning, scheduling
4. **Content Management**: Activity images, user comments, reviews
5. **Filtering**: By difficulty level, region, duration (walk_days)

## Development Notes

### Entity Framework
- Uses Database First approach with EDMX model
- Auto-generated entity classes (do not modify manually)
- DbContext: `HikingDBEntities1`

### Common Patterns
- Controllers follow standard MVC pattern with dependency injection of DbContext
- ViewModels are used to shape data for views (avoid passing entities directly)
- Image handling through `GroupImage` entity with byte array storage
- Pagination implemented in activity listings

### Frontend Integration
- Bootstrap 5 for responsive design
- Custom CSS files organized in `css/mycss/` folder
- JavaScript libraries: AOS (animations), GLightbox (image galleries), Flatpickr (date picker)

### Testing
No specific test projects identified in the solution. Consider adding unit tests for controllers and data access logic.

### Key Configuration Files
- `Web.config`: Application configuration, connection strings, Entity Framework settings
- `RouteConfig.cs`: MVC routing configuration
- `Global.asax.cs`: Application startup configuration