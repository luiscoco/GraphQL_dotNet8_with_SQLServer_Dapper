# How to create .NET 8 GraphQL API with SQLServer and Dapper

The source code for this example is available in this github repo: 

https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_Dapper

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

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_Dapper/assets/32194879/bd141059-5e49-438a-9ab4-0c5dccc8c2a0)

## 6. Create the Models

**Author.cs**

```csharp
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLDemo.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)] // Adjust max length as necessary
        public string Name { get; set; }

        // Virtual for enabling lazy loading
        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}
```

**Post.cs**

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLDemo.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)] // Adjust max length as necessary for Title
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        // Virtual for enabling lazy loading
        public virtual Author Author { get; set; }
    }
}
```

## 7. Create the Services

**IAuthorService.cs**

```csharp
using GraphQLDemo.Models;

namespace GraphQLDemo.Services
{
    public interface IAuthorService
    {
        Author GetAuthorById(int id);
        List<Author> GetAllAuthors();
    }
}
```

**IPostService.cs**

```csharp
using GraphQLDemo.Models;

namespace GraphQLDemo.Services
{
    public interface IPostService
    {
        Post GetPostById(int id);
        List<Post> GetAllPosts();
        Post AddPost(Post post);
    }
}
```

**AuthorService.cs**

```csharp
using Dapper;
using GraphQLDemo.Models;
using GraphQLDemo.Utilities;
using System.Data;
using System.Collections.Generic;

namespace GraphQLDemo.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly DapperContext _dapperContext;

        public AuthorService(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Author GetAuthorById(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var authorDictionary = new Dictionary<int, Author>();

            var authors = connection.Query<Author, Post, Author>(
                "SELECT a.*, p.* FROM Authors a LEFT JOIN Posts p ON a.Id = p.AuthorId WHERE a.Id = @Id",
                (author, post) =>
                {
                    if (!authorDictionary.TryGetValue(author.Id, out var authorEntry))
                    {
                        authorEntry = author;
                        authorEntry.Posts = new List<Post>();
                        authorDictionary.Add(authorEntry.Id, authorEntry);
                    }

                    if (post != null)
                    {
                        authorEntry.Posts.Add(post);
                    }

                    return authorEntry;
                },
                param: new { Id = id },
                splitOn: "Id"
            ).FirstOrDefault();

            return authors;
        }

        public List<Author> GetAllAuthors()
        {
            using var connection = _dapperContext.CreateConnection();
            var authorDictionary = new Dictionary<int, Author>();

            var authors = connection.Query<Author, Post, Author>(
                "SELECT a.*, p.* FROM Authors a LEFT JOIN Posts p ON a.Id = p.AuthorId",
                (author, post) =>
                {
                    if (!authorDictionary.TryGetValue(author.Id, out var authorEntry))
                    {
                        authorEntry = author;
                        authorEntry.Posts = new List<Post>();
                        authorDictionary.Add(authorEntry.Id, authorEntry);
                    }

                    if (post != null)
                    {
                        authorEntry.Posts.Add(post);
                    }

                    return authorEntry;
                },
                splitOn: "Id"
            ).Distinct().ToList();

            return authors;
        }
    }
}
```

**PostService.cs**

```csharp
using Dapper;
using GraphQLDemo.Models;
using GraphQLDemo.Utilities;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLDemo.Services
{
    public class PostService : IPostService
    {
        private readonly DapperContext _dapperContext;

        public PostService(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Post GetPostById(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var post = connection.Query<Post, Author, Post>(
                "SELECT p.*, a.* FROM Posts p INNER JOIN Authors a ON p.AuthorId = a.Id WHERE p.Id = @Id",
                (post, author) =>
                {
                    post.Author = author;
                    return post;
                },
                param: new { Id = id },
                splitOn: "Id"
            ).FirstOrDefault();

            return post;
        }

        public List<Post> GetAllPosts()
        {
            using var connection = _dapperContext.CreateConnection();
            var postDictionary = new Dictionary<int, Post>();

            var posts = connection.Query<Post, Author, Post>(
                "SELECT p.*, a.* FROM Posts p INNER JOIN Authors a ON p.AuthorId = a.Id",
                (post, author) =>
                {
                    post.Author = author;
                    return post;
                },
                splitOn: "Id"
            ).ToList();

            return posts;
        }

        public Post AddPost(Post post)
        {
            using var connection = _dapperContext.CreateConnection();

            var authorExists = connection.QuerySingleOrDefault<Author>(
                "SELECT * FROM Authors WHERE Id = @AuthorId",
                new { AuthorId = post.AuthorId });

            if (authorExists == null)
            {
                throw new Exception($"Author with ID {post.AuthorId} not found.");
            }

            var sql = @"
                INSERT INTO Posts (Title, Content, AuthorId) VALUES (@Title, @Content, @AuthorId);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            var postId = connection.QuerySingle<int>(sql, post);
            var newPost = GetPostById(postId); // Re-fetch the post to include the Author

            return newPost;
        }
    }
}
```

## 8. Create the Utilities (DapperContext)


**DapperContext.cs**

```csharp
using System.Data;
using Microsoft.Data.SqlClient;

namespace GraphQLDemo.Utilities
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
```

## 9. Create the GraphQL Types, Query and Mutation

**AuthorType.cs**

```csharp
using GraphQLDemo.Models;
using HotChocolate.Types;

namespace GraphQLDemo.GraphQL
{
    public class AuthorType : ObjectType<Author>
    {
        protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
        {
            descriptor.Field(a => a.Id).Type<NonNullType<IntType>>();
            descriptor.Field(a => a.Name).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.Posts).Type<NonNullType<ListType<NonNullType<PostType>>>>();
        }
    }
}
```

**PostType.cs**

```csharp
using GraphQLDemo.Models;
using HotChocolate.Types;

namespace GraphQLDemo.GraphQL
{
    public class PostType : ObjectType<Post>
    {
        protected override void Configure(IObjectTypeDescriptor<Post> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<IntType>>();
            descriptor.Field(p => p.Title).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Content).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Author).Type<NonNullType<AuthorType>>();
        }
    }
}
```

**Query.cs**

```csharp
using GraphQLDemo.Models;
using HotChocolate;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL
{
    public class Query
    {
        public Author GetAuthor([Service] IAuthorService authorService, int id) =>
            authorService.GetAuthorById(id);

        public Post GetPost([Service] IPostService postService, int id) =>
            postService.GetPostById(id);
    }
}
```

**Mutation.cs**

```csharp
using GraphQLDemo.Models;
using HotChocolate;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL
{
    public class Mutation
    {
        public Post AddPost([Service] IPostService postService, CreatePostInput input) =>
            postService.AddPost(new Post
            {
                Title = input.Title,
                Content = input.Content,
                AuthorId = input.AuthorId
            });
    }

    public record CreatePostInput(string Title, string Content, int AuthorId);
}
```

## 10. Modify the application middleware (program.cs)

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using GraphQLDemo.GraphQL;
using GraphQLDemo.Services;
using System;
using GraphQLDemo.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DapperContext>();

// Add services to the container.
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPostService, PostService>();

// Add GraphQL services
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<AuthorType>()
    .AddType<PostType>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

// Use GraphQL middleware
app.MapGraphQL();

app.Run();
```

## 11. Modify the appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=GraphQLProductDB;User ID=sa;Password=Luiscoco123456;Encrypt=false;TrustServerCertificate=true;"
  },
  "AllowedHosts": "*"
}
```

## 12. Run and Test the application 

We build and run the application in Visual Studio 

We navigate to the Banana endpoint: https://localhost:7106/graphql/

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/53caeb3b-a90b-4634-9696-2e5ee91f7d69)

We press on the **Create document** button 

We input the samples code, see below

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/adf8f371-15ff-4740-9cd3-6e1dda27aad8)

Query Examples

**Fetch an Author by ID**

This query retrieves an author by their id, including all posts associated with the author

```
query GetAuthor {
  author(id: 1) {
    id
    name
    posts {
      id
      title
      content
    }
  }
}
```

Replace 1 with the actual ID of the author you want to test

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_Dapper/assets/32194879/cfa6337b-f5db-4ca8-a509-bcebb0ea923e)

**Fetch a Post by ID**

This query retrieves a post by its id, including the author details

```
query GetPost {
  post(id: 1) {
    id
    title
    content
    author {
      id
      name
    }
  }
}
```

Replace 1 with the actual ID of the post you want to test

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_Dapper/assets/32194879/40af88ff-14fa-4a43-9a35-2d76105266b8)

**Mutation Example (Add a New Post)**

This mutation adds a new post to the database. You need to provide a title, content, and the author's ID

```
mutation AddNewPost {
  addPost(input: {title: "New GraphQL Post", content: "Exploring GraphQL mutations.", authorId: 1}) {
    id
    title
    content
    author {
      id
      name
    }
  }
}
```

Replace "New GraphQL Post", "Exploring GraphQL mutations.", and 1 with your desired post title, content, and author ID, respectively

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_Dapper/assets/32194879/01cbcc56-dd1b-4236-b1b9-a00202fa1dde)
