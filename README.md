# MathQuest
MathQuest is an educational website designed to help school students enhance their math skills through interactive learning.
[Video Review](https://youtu.be/327z7diD8Uk)

## Table of Contents
- [About](#about)
- [Key Features](#key-features)
- [Technologies Used](#technologies-used)
- [Testing](#testing)
- [How to Run the App](#how-to-run-the-app)
- [Current Status](#current-status)
- [License](#license)
- [Contributing](#contributing)

## About
The platform is built using ASP.NET Core 8 and React.js, following a robust three-layered architecture. It is a comprehensive online platform for school students to learn and practice math. It integrates user authentication, community features, and a points system to motivate learning. Admins have tools to manage users and receive feedback.

## Key Features
- **User Authentication & Authorization**: JWT token-based and role-based authentication ensures secure access. Users can enable two-factor authentication using an external authenticator app.
- **Interactive Learning**: Explore math theories, complete quizzes, and earn points to climb the leaderboard.
- **Community Engagement**: Chat within the community, search for friends, add them, and message directly.
- **Admin Tools**: Admins have the ability to add and remove user roles, view user feedback, and lockout or delete users from the platform.
- **Admin Feedback**: Users can send feedback to admins, specifying desired changes.

## Technologies Used
- **Backend**: ASP.NET Core 8, Entity Framework Core, Identity Framework, Automapper, SignalR
- **Frontend**: React.js, Axios, SignalR
- **Database**: SQL Server
- **Authentication**: JWT Tokens, Two-Factor Authentication (2FA) with external authenticator apps
- **Testing**: NUnit for unit and integration tests
- **API: RESTful API

## Testing
The project includes a dedicated tests project that covers:
- **Unit Tests**: For validating the functionality of the data and business layers.
- **Integration Tests**: To ensure that the Web API integrates seamlessly with other system components.

## How to Run the App

### Prerequisites
Ensure you have the following installed on your local machine:
**1. If Using Visual Studio:**
- Download and install Visual Studio 2022 version 17.8 or later. 
[Visual Studio](https://visualstudio.microsoft.com/downloads/)

**2. If using Visual Studio Code:**
- Download and install Visual Studio Code. 
[Visual Studio](https://visualstudio.microsoft.com/downloads/)

**3. After installing any of those programs:**
- Make sure that .NET SDK 8.0.x (any such version is installed)
- Open a Command Prompt or Terminal and run the following commands to check the installed versions:
```bash dotnet --list-sdks```
- If not installed, download and install the .NET SDK 8.0.x (any such version) from:
[.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

**4. SQL Server 2022 Developer:**
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Good to have: [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

**5. Node Js:**
- Version 20.11.x (any version) or later ones
- [Node.js](https://nodejs.org/en)
- Make sure that it is installed by run the following command in a Command Prompt or Terminal:
```bash node -v```

### Installation
**1. Clone and open the repository using HTTPS or SSH:**
- **Using Https:**
```bash git clone https://github.com/nukri-tskvitaia/MathQuest.git```
```bash cd MathQuest```
- **Or using SSH:**
```bash git clone git@github.com:nukri-tskvitaia/MathQuest.git```
```bash cd MathQuest```

**2. Set up the database:**
- Update the connection string in ***appsettings.Development.json*** file (__Located in MathQuest.Server project__)
- Modify the AttachDbFilename property in your connection string to point to the absolute path of the MathQuest.mdf file.
- Your connection string should look something like this:
```plaintext AttachDbFilename=\\absolute_path_to_your_project\\mathquest-project\\Data\\DB\\MathQuest.mdf```

**3. Google Authentication:**
Currently, Google authentication is not implemented. To enable Google authentication, follow these steps:
You need to configure your Google authentication settings in the `User Secrets` of the `MathQuest.Server` project. Add the following configuration to your `User Secrets`:
```plaintext
   {
     "Google:ClientId": "your client id",
     "Google:ClientSecret": "your client secret"
   }```

The MathQuest.Server project is already set up to use Google authentication. You can verify this in the Startup.cs file. The relevant code snippet is:
```csharp services.AddAuthentication()
        .AddGoogle(opts =>
        {
            opts.ClientId = Configuration["Google:ClientId"];
            opts.ClientSecret = Configuration["Google:ClientSecret"];
        }); ```

Make any additional changes as needed to integrate Google authentication into the application fully.

If you choose not to use Google authentication, you can disable it by commenting out or removing the Google authentication setup code.
In your Startup.cs file, comment out or delete the following lines:
```csharp services.AddAuthentication()
        .AddGoogle(opts =>
        {
            opts.ClientId = Configuration["Google:ClientId"];
            opts.ClientSecret = Configuration["Google:ClientSecret"];
        }); ```

**4. Email Service:**
***to be continued...***
