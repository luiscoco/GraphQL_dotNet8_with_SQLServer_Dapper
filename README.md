# How to create .NET 8 GraphQL API with SQLServer and Dapper

The source code for this example is available in this github repo: 

https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework

## 1. Pull and run the SQLServer Docker image with Docker Desktop

We run Docker Desktop application

![image](https://github.com/luiscoco/Identity_dotNET8_Authentication/assets/32194879/be7cb784-9f18-4f5f-a557-cfcc47cc3d6f)

We open a command prompt window and run the following command

```
docker run ^
  -e "ACCEPT_EULA=Y" ^
  -e "MSSQL_SA_PASSWORD=Luiscoco123456" ^
  -p 1433:1433 ^
  -d mcr.microsoft.com/mssql/server:2022-latest
```

![image](https://github.com/luiscoco/Identity_dotNET8_Authentication/assets/32194879/5661f705-44eb-4537-855e-ed04279d002e)

We can also see the image in Docker Desktop

![image](https://github.com/luiscoco/Identity_dotNET8_Authentication/assets/32194879/399896b3-0c2b-4ec0-acfc-b83f10691e5d)

And also we can see the running container in Docker Desktop

![image](https://github.com/luiscoco/Identity_dotNET8_Authentication/assets/32194879/9a1b0c37-5bc0-4129-987c-60f77db91448)

We run SSMS SQL Server Management Studio and we connect to the SQL Server running docker container

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/5381d14a-1019-421e-903e-cf37ccb0300d)

## 2. Create the database and tables

We create the database for this sample

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/4c0600ef-077d-49eb-a970-c733ac663e24)

We have to create the Author and Post tables inside the database

For creating the **Authors table** run this sql query

```sql
USE [GraphQLProductDB]
GO

/****** Object:  Table [dbo].[Authors]    Script Date: 27/02/2024 11:59:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```

For creating the **Posts table** run this sql query

```sql
USE [GraphQLProductDB]
GO

/****** Object:  Table [dbo].[Posts]    Script Date: 27/02/2024 12:00:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Posts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Authors_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Authors_AuthorId]
GO
```

We can verify the new tables 

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_Dapper/assets/32194879/7484ad4a-5f81-415f-ba4f-321a5c7726ef)

## 3. Create a new Web API in Visual Studio 2022 Community Edition
  
We run Visual Studio 2022 Community Edition and we create a new project

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/346b3777-7f19-439c-bb06-a24ad1503729)

We select the **api** project template

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/79fa8f9e-b440-4b3f-bd26-78469730622f)

We input the project name and location 

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/95ec4d51-f35e-4e56-9833-d9d042320df5)

We select the project default features

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/8fc0435d-c623-44d9-9173-ec2bd1cdf77b)

After opening the project in Visual Studio we can delete the Controller folder and the Weatherforecast controller and service

## 4. Load the project dependencies

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_Dapper/assets/32194879/b33ff3bb-e6bd-43d4-be0c-e43690e02e4f)

This is the csproj file

```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Dapper" Version="2.1.28" />
    <PackageReference Include="HotChocolate.AspNetCore" Version="13.9.0" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.5" />
  </ItemGroup>

</Project>
```

## 5. Create the project structure


## 6. Create the Models


## 7. Create the Services


## 8. Create the Utilities (DapperContext)

## 9. Create the GraphQL Types, Query and Mutation

## 10. Modify the application middleware (program.cs)

## 11. Modify the appsettings.json


## 12. Run and Test the application 
