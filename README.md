# MintPlayer.AspNetCore.Templates
This repository contains .NET templates with an initial IdentityServer setup.

## Version info

| License | Build status | Package | Downloads |
|---------|--------------|---------|-----------|
| [![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://lbesson.mit-license.org/) | [![publish-master](https://github.com/MintPlayer/MintPlayer.AspNetCore.Templates/actions/workflows/publish-master.yml/badge.svg)](https://github.com/MintPlayer/MintPlayer.AspNetCore.Templates/actions/workflows/publish-master.yml) | [![NuGet Version](https://img.shields.io/nuget/v/MintPlayer.AspNetCore.IdentityServer.Templates.svg?style=flat)](https://www.nuget.org/packages/MintPlayer.AspNetCore.IdentityServer.Templates) | [![NuGet](https://img.shields.io/nuget/dt/MintPlayer.AspNetCore.IdentityServer.Templates.svg?style=flat)](https://www.nuget.org/packages/MintPlayer.AspNetCore.IdentityServer.Templates) |

## Installing the templates

	dotnet new --install MintPlayer.AspNetCore.IdentityServer.Templates

## Updating the installed template packages
You should be able to update the .NET templates using the following command:

    dotnet new update

## Creating new projects
Create a new folder and CD inside it

	mkdir OauthProvider && cd OauthProvider

Generate a new project using one of the templates

	dotnet new id-provider
	dotnet new id-application

## About .NET Templating
Some stuff is explained in the [following readme](https://github.com/dotnet/templating/wiki/Conditional-processing-and-comment-syntax#razor-views)
