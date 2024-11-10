# Store API project
### Project overview
* Project incorporates 2 microservices, which are managing Users and Products. User and Product controllers implement required CRUD operaitons;
* Users can create personal accounts with ability to recover passwords by following email link.
* There is a possibility to filter products using HTTP query;
* Database is used to store users and their products;
* Systems of authentication and role-based authorization are supported, therefore only authorized users have access to API endpoints. Access is restricted based on user's role. Only admin can manage users' accounts, users can modify only those products which they have created;
* There are integration tests available for all microservices;
* Followed the principles of Clean architecture;
* Swagger UI is used to enhance visual representation;

### Used Technologies
* ASP.NET Core;
* User input validation is implemented using Fluent Validation and MediatR technology;
* Exceptions are processed using ProblemDetails;
* Controllers implement RESTful API for communication;
* MS SQL Server is used as Database Management System;
* Entity Framework is used for mapping database records into actual classes in program (code-first approach);
* BCrypt library is used for hashing/securing users' passwords;
* JWT technology is used for implementing authentication and role-based authorization using access tokens;
* Unit testing is presented as a standalone project using XUnit;
