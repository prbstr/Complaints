# Complaints

## Intro
The complaints api is built using .Net Core 3.1, using MVC and an InMemory database for ease of use.

## Testing
A postman collection is included in the repo. 
Import it to test out the endpoints when the project is running.

There are 5 api calls with an example body included

Unauthenticated calls
POST: /api/users/register
POST: /api/users/login

Authenticated calls
GET: /api/users
GET: /api/complaints
POST: /api/complaints/add

## Features
Register new user
Login with user
Get all users
Add complaint
Fetch all complaints

## Out of scope
Attach a user to a complaint
Update or delete users
Update or delete complaints
Roles and claims
