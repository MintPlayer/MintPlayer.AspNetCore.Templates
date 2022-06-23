# MintPlayer.AspNetCore.Templates
This repository contains .NET templates with an initial IdentityServer setup.

## Installing the templates

	dotnet new --install MintPlayer.AspNetCore.IdentityServer.Templates

## Creating new projects
Create a new folder and CD inside it

	mkdir OauthProvider && cd OauthProvider

Generate a new project using one of the templates

	dotnet new id-provider
	dotnet new id-application