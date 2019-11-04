# Features

- **ASP.NET Core 2.2**
  - [Swagger](https://github.com/domaindrivendev/Swashbuckle)
  - [MediatR](https://github.com/jbogard/MediatR)
  - [FeatureFolders](https://github.com/OdeToCode/AddFeatureFolders)
  - [AutoMapper](https://github.com/AutoMapper/AutoMapper)
  - [FluentValidation](https://github.com/JeremySkinner/FluentValidation)
- **MithrilJS**
  - ES6 + JSX
  - [Construct-UI](https://github.com/vrimar/construct-ui)
- **Webpack**
  - [HMR (Hot Module Replacement/Reloading)](https://github.com/aspnet/JavaScriptServices)

# Prerequisites:
 * [.Net Core 2.2](https://www.microsoft.com/net/download/windows)
 * [NodeJS](https://nodejs.org/) >= 10.x
 * [VSCode](https://code.visualstudio.com/) (ideally), or VS2017
 
# Installation:

Make sure to install front-end dependencies 

	cd app && npm install

## Start the application:
You have two choices when it come at how your preffer to run it. You can either use the command line or the build-in run command.

### 1. Using the command line
Run the application using `cd app && npm run dev`. 
 
### 2. Using the built-in run command
Run the application in VSCode/Visual Studio 2017 by hitting `F5`.

## Build minified JS & CSS files:

	cd app && npm run build
  or
  hitting `F5` on VSCode/Visual Studio 2017

Outputs to `wwwroot/`
