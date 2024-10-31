# Store API project
### Project overview
* Project incorporates 3 controllers which are Login, User, Product controllers. User and Product controllers implement required CRUD operaitons;
* There is a possibility to filter products using HTTP query;
* Database is used to store users and their products;
* Systems of authentication and role-based authorization are supported, therefore only authorized users have access to controllers. Moreover, access is restricted based on user's role. Only admin can manage users' accounts, users can modify only those products which they have created;
* There is a Unit testing available for all controllers;
* Followed principles of MVC architectural pattern;

### Used Technologies
* ASP.NET Core;
* Controllers implement RESTful API for communication;
* MS SQL Server is used as Database Management System;
* Entity Framework is used for mapping database records into actual classes in program (code-first approach);
* BCrypt library is used for hashing/securing users' passwords;
* JWT technology is used for implementing authentication and role-based authorization using access tokens;
* Unit testing is presented as a standalone project using XUnit;
