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

We create the database for this sample

![image](https://github.com/luiscoco/GraphQL_dotNet8_with_SQLServer_EntityFramework/assets/32194879/4c0600ef-077d-49eb-a970-c733ac663e24)
