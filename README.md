# BROwser-Backend

The Backend Project of the BROwser Application

[![Build status](https://img.shields.io/github/workflow/status/alexalok/dotAPNS/Test)](https://github.com/BROwser-Project/BROwser-Backend/actions)

### Tech Stack - Backend

- .NET 5
- Mediator Pattern (MediatR)
- FluentValidation
- SignalR
- AutoMapper
- Swagger
- Entity Framework Core
- Identity and Authentication
- Security with JSON Web Token
- Cloudinary
- SendGrid

### Mediator Pattern

Imagine an application in which there are many objects that are communicating with each other. The mediator design pattern is useful when the number of objects grows so large that it becomes difficult to maintain the references to the objects. The mediator is essentially an object that encapsulates how one or more objects interact with each other. The mediator design pattern controls how these objects communicate, and helps to reduce the number of dependencies among them that you have to manage.

In the mediator design pattern, the objects don’t communicate with one another directly but through the mediator. When an object needs to communicate with another object or a set of objects, it transmits the message to the mediator. The mediator then transmits the message to each receiver object in a form that is understandable to it.

By eliminating the direct communication between objects, the mediator design pattern promotes loose coupling. The other benefit of using the mediator design pattern is that it improves code readability and maintainability.

### SignalR

ASP.NET SignalR is a library for ASP.NET developers that makes it incredibly simple to add real-time web functionality to your applications. What is "real-time web" functionality? It's the ability to have your server-side code push content to the connected clients as it happens, in real-time.

### Endpoints of the Backend

| Entity        | Type   | URL                                                  | Description                                                                                       | Success        | Authorize                                       |
| ------------- | ------ | ---------------------------------------------------- | ------------------------------------------------------------------------------------------------- | -------------- | ----------------------------------------------- |
| User          | POST   | /api/account/login                                   | User Login.                                                                                       | 200 OK & Token | No                                              |
|               | POST   | /api/account/register                                | User registration.                                                                                | 200 OK         | No                                              |
|               | POST   | /api/account/refreshToken                            | User token refresh.                                                                               | 200 OK         | Yes                                             |
|               | GET    | /api/account                                         | Get the current logged in user information like token, main image etc...                          | 200 OK         | Yes & Only the logged in user.                  |
| User Photo    | POST   | /api/photos                                          | Upload a photo.                                                                                   | 200 OK         | Yes                                             |
|               | DELETE | /api/photos/{photo id}                               | Delete a photo, the user cannot delete the current main photo.                                    | 200 OK         | Yes                                             |
|               | PUT    | /api/photos/{photo id}/setmain                       | Set the photo to main. (Only one main - profile picture.)                                         | 200 OK         | Yes                                             |
| WorkoutEvents | GET    | /api/workoutevents                                   | Get all workoutevents with pagination.                                                            | 200 OK         | Yes                                             |
|               | GET    | /api/workoutevents?pageSize=2&pageNumber=2           | Get all workoutevents with 2 workoutevents per page and get the second page of the workoutevents. | 200 OK         | Yes                                             |
|               | GET    | /api/workoutevents?isgoing=true                      | Workoutevents of the logged in user where he/she is going.                                        | 200 OK         | Yes                                             |
|               | GET    | /api/workoutevents?ishost=true                       | Workoutevents of the logged in user where he/she is the host of this workoutevents.               | 200 OK         | Yes                                             |
|               | GET    | /api/workoutevents?startDate=2021-06-05              | Get all upcoming workoutevents from this start date.                                              | 200 OK         | Yes                                             |
|               | GET    | /api/workoutevents?startDate=2021-04-14&isGoing=true | Get all upcoming workoutevents from this start date where the logged in user is going.            | 200 OK         | Yes                                             |
|               | GET    | /api/workoutevents/{id}                              | Get the workoutevent by Id.                                                                       | 200 OK         | Yes                                             |
|               | POST   | /api/workoutevents                                   | Create a workoutevent.                                                                            | 200 OK         | Yes                                             |
|               | PUT    | /api/workoutevents/{id}                              | Update / modify a workoutevent.                                                                   | 200 OK         | Yes & Logged in user is the host or Admin role. |
|               | DELETE | /api/workoutevents/{id}                              | Delete a workoutevent.                                                                            | 200 OK         | Yes & Logged in user is the host or Admin role. |
|               | POST   | /api/workoutevents/{id}/attend                       | Subscribe to a workoutevent / Unsubscribe from a workoutevent. (Host cancel / restart it.)        | 200 OK         | Yes                                             |
