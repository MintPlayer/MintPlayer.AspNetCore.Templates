# MintPlayer.AspNetCore.Templates
This repository contains .NET templates with an initial IdentityServer setup.

## Installing the templates

	dotnet new --install MintPlayer.AspNetCore.IdentityServer.Templates

## Updating the installed template packages
You should be able to update the .NET templates using the following command:

    dotnet new --update-apply

## Creating new projects
Create a new folder and CD inside it

	mkdir OauthProvider && cd OauthProvider

Generate a new project using one of the templates

	dotnet new id-provider
	dotnet new id-application
