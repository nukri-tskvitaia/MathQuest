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
- **User Authentication & Authorization:** JWT token-based and role-based authentication ensures secure access. Users can enable two-factor authentication using an external authenticator app.
- **Interactive Learning:** Explore math theories, complete quizzes, and earn points to climb the leaderboard.
- **Community Engagement:** Chat within the community, search for friends, add them, and message directly.
- **Admin Tools:** Admins have the ability to add and remove user roles, view user feedback, and lockout or delete users from the platform.
- **Admin Feedback:** Users can send feedback to admins, specifying desired changes.

## Technologies Used
- **Backend:** ASP.NET Core 8, Entity Framework Core, Identity Framework, Automapper, SignalR
- **Frontend:** React.js, Axios, SignalR
- **Database:** SQL Server
- **Authentication:** JWT Tokens, Two-Factor Authentication (2FA) with external authenticator apps
- **Testing:** NUnit and Moq for unit and integration tests
- **API:** RESTful API

## Testing
The project includes a dedicated tests project that covers:
- **Unit Tests:** For validating the functionality of the data and business layers.
- **Integration Tests:** To ensure that the Web API integrates seamlessly with other system components.

## How to Run the App

### Prerequisites
Ensure you have the following installed on your local machine:

**1. If Using Visual Studio:**
- Download and install Visual Studio 2022 version 17.8 or later. 
[Visual Studio](https://visualstudio.microsoft.com/downloads/)

**2. If using Visual Studio Code:**
- Download and install Visual Studio Code. 
[Visual Studio Code](https://visualstudio.microsoft.com/downloads/)

**3. After installing any of those programs:**
- Make sure that .NET SDK 8.0.x (any such version is installed)
- Open a Command Prompt or Terminal and run the following commands to check the installed versions:
  
```bash
dotnet --list-sdks
```

- If not installed, download and install the .NET SDK 8.0.x (any such version) from:
[.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

**4. SQL Server 2022 Developer:**
- Download and install SQL Server 2022 Developer:
  [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Good to have:
  [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

**5. Node Js:**
- Download and install Node.js version 20.11.x (any such version) or later one
- [Node.js](https://nodejs.org/en)
- Make sure that it is installed by running the following command in a Command Prompt or Terminal:

```bash
node -v
```

### Installation and Configuration
**1. Clone and open the repository using HTTPS or SSH:**
- **Using Https:**
  
```bash
git clone https://github.com/nukri-tskvitaia/MathQuest.git
```

```bash
cd MathQuest
```

- **Or using SSH:**
  
```bash
git clone git@github.com:nukri-tskvitaia/MathQuest.git
```

```bash
cd MathQuest
```

**2. Set up the database:**
- Ensure that SQL Server is running.
- Update the connection string in ***appsettings.Development.json*** file (Located in __MathQuest.Server project__)
- Modify the __AttachDbFilename__ property in your connection string to point to the absolute path of the __MathQuest.mdf__ file.
- Your connection string should look something like this:

```json
AttachDbFilename=\\absolute_path_to_your_project\\mathquest-project\\Data\\DB\\MathQuest.mdf
```

- Ensure that you have `dotnet-ef` installed by running this command in a Command Prompt or Terminal:

```bash
dotnet ef --version
```

You should see any such version 8.0.x. If not then install it using:

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```
(or any such version 8.0.x For example, --version 8.0.6)
  
- Create a new migration and apply it:
Make sure you are in this directory... ***absolute_path_to_your_folder/Mathquest/Data*** from there you can run the following commands:

  ```bash
  dotnet ef migrations add InitialCreate --output-dir Migrations --startup-project ../MathQuest/MathQuest.Server
  ```

  ```bash
  dotnet ef database update --startup-project ../MathQuest/MathQuest.Server
  ```

**3. Configure JWT Tokens:**
- To set up JWT authentication, simply enter your JWT key in the configuration below, and you’re all set!

```json
  "Jwt": {
    "Key": "enter_your_jwt_key_here",
    "Issuer": "https://localhost:5001",
    "Audience": "https://localhost:5100",
    "AccessTokenExpirationMinutes": 30,
    "RefreshTokenExpirationDays": 7,
    "RequireHttpsMetadata": true,
    "SaveToken": false,
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ValidateIssuerSigningKey": true
  }
```

Replace "enter_your_jwt_key_here" with your actual JWT key.

**4. Google Authentication:**
Currently, Google authentication is not implemented. To enable Google authentication, follow these steps:
You need to configure your Google authentication settings in the `User Secrets` of the `MathQuest.Server` project. Add the following configuration to your `User Secrets`:

```json
   {
     "Google:ClientId": "your client id",
     "Google:ClientSecret": "your client secret"
   }
```

The __MathQuest.Server__ project is already set up to use Google authentication. You can verify this in the Startup.cs file. The relevant code snippet is:
```csharp services.AddAuthentication()
        .AddGoogle(opts =>
        {
            opts.ClientId = Configuration["Google:ClientId"];
            opts.ClientSecret = Configuration["Google:ClientSecret"];
        });```

Make any additional changes as needed to integrate Google authentication into the application fully.

If you choose not to use Google authentication, you can disable it by commenting out or removing the Google authentication setup code.
In your Startup.cs file, comment out or delete the following lines:
```csharp
services.AddAuthentication()
        .AddGoogle(opts =>
        {
            opts.ClientId = Configuration["Google:ClientId"];
            opts.ClientSecret = Configuration["Google:ClientSecret"];
        });
```

**5. Email Service:**
5.1. **Setup SMTP**
If you wish to use the email service rather than the console for sending emails, replace the placeholder values in the configuration below with your actual SMTP server details.

```json
"Mail": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 465,
  "SmtpUsername": "your_smtp_username",
  "SmtpPassword": "your_smtp_password",
  "SenderName": "your_sender_name",
  "SenderEmail": "your_sender_email",
  "UseSsl": true
}
```
5.2. **Modify the Authorization Controller Endpoints:**
5.2.1 **Register Endpoint:**
  If you want to send email confirmation messages, add the following code to the Register endpoint in your AuthorizationController. Uncomment the email sending lines:

```csharp
  // Uncomment this code to send a confirmation email
await _emailSenderService
    .SendEmailAsync(email, "Confirm your email", $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
    .ConfigureAwait(false);
```

5.2.2 ** Remove Console Email Sender (Optional)**
If you prefer not to use the console email sender, comment the following lines from the code above the _emailSenderService:
```csharp
// Comment this code
            await _consoleEmailSenderService.SendEmailAsync(
                model.Email,
                "Confirm your email",
                $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
            .ConfigureAwait(false);
```


5.2.3 **Register/Resend Confirmation Endpoint:**
For resending confirmation emails, add and uncomment the following code in the Register/ResendConfirmation endpoint:
```csharp
// Uncomment this code to resend a confirmation email
await _emailSenderService
    .SendEmailAsync(email, "Confirm your email", $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
    .ConfigureAwait(false);
```

5.2.4 ** Remove Console Email Sender (Optional)**
If you prefer not to use the console email sender, comment the following lines from the code above the _emailSenderService:
```csharp
// Comment this code
await _consoleEmailSenderService.SendEmailAsync(
    email,
    "Confirm your email",
    $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
.ConfigureAwait(false);
```

5.2.5 **Search Email Endpoint:**
For password reset emails, use the following code in the SearchEmail endpoint. Uncomment the relevant lines:

```csharp
// Uncomment this code to send a password reset email
await _emailSenderService
    .SendPasswordResetEmailAsync(email, confirmationLink)
    .ConfigureAwait(false);
```

5.2.6 ** Remove Console Email Sender (Optional)**
If you prefer not to use the console email sender, comment the following lines from the code above the _emailSenderService:
```csharp
// Comment this code
            await _consoleEmailSenderService.SendEmailAsync(
                email,
                "Password Reset",
                $"<p>Please reset your password by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
            .ConfigureAwait(false);
```

### Running the Application

**1. Build the Solution:**
Build the solution using:

```bash
dotnet build
```

During the build process, dotnet restore will automatically run if necessary to restore any missing packages.

**2. Run the Application:**
Make sure you run only `MathQuest.Server` project `MathQuest.Client` will open automatically

```bash
dotnet run
```
**3. Access the App:**
- Your browser should be opened automatically if not then navigate to [Web Api](https://localhost:5001) to view the Web Api.
- The frontend will be running on [Frontend](https://localhost:5100).
- If you face any npm related issues make sure to install `npm`. Open `MathQuest.Client` project directory in Terminal or Command Prompt and run the following command:

  ```bash
  npm install
  ```

## Current Status
The React.js code for the frontend was developed rapidly to meet tight deadlines, meaning there are opportunities for security and performance improvements. While the current implementation is functional, there are areas for optimization and enhancement to deliver a more seamless user experience.

## License
This project is licensed under the MIT License. See the [LICENSE](https://github.com/nukri-tskvitaia/MathQuest/blob/main/LICENSE) file for details.

## Contributing
Contributions are welcome, especially in improving the security and performance of the frontend code. If you have ideas or find issues, feel free to open a pull request.
