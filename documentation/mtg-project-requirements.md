<div style="display: flex; justify-content: center;">
    <h1 style="text-align: center;">MTG Project Requirements</h1>
</div>

<div style="display: flex; justify-content: center;">
    <div style="flex: 1; text-align: center;">
        <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/7/7d/Microsoft_.NET_logo.svg/1200px-Microsoft_.NET_logo.svg.png" width="200">
    </div>
    <div style="flex: 1; text-align: center;">
        <img src="https://th.bing.com/th/id/R.86e79e09deb9bd1fec8853c01c0b82e9?rik=shU9cZ9ruw6LiQ&riu=http%3a%2f%2fwww.logoeps.com%2fwp-content%2fuploads%2f2013%2f04%2fmagic-the-gathering-vector-logo.png&ehk=%2fl0iXSL6FA2yJjBc%2bH1L0ySw4W14A2e6gSsXDmMYPxs%3d&risl=&pid=ImgRaw&r=0" width="300">
    </div>
</div>

## Teachers
- Matthias Blomme
- Joost Tack

## Goal
This document states the minimum requirements for the MTG Project of the .NET Technology fundamentals module in 23-24.

## Deadline
The deadline for this project is **26th of may 2023 at 23:59**.

## Technical requirements

The following technologies **must** be used in the project.

- GUI
    - Blazor Server
    - Use of components
    - Sessions
- Business Logic
    - Use of LINQ (no plain old sql statements)
    - Use of Services (plain old classes)
- Data Access
    - Use of Entity Framework Core
    - Use of the MTG database
- General
    - Git
    - Github Copilot
    - .NET 8
    - C# 9 or higher

The following technologies are **not** allowed.

- ASP.NET MVC
- .NET < 8
- Client side libraries (e.g. jQuery, Bootstrap, vue, nextjs, react, ...)
- JavaScript (only allowd for special use case in blazor components)
- Any other technology not mentioned in the allowed list.

## Functional requirements

The following requirements **must** be implemented in the project.

- Searching database items with a search bar and filters.
- Creating a personal collection of items (with filters and search).
- A details page for each item
- All forms must have Blazor validation
- Use sessions to store data like the collection, the search query, basket, ...
- At least 3 different pages

## Non-functional requirements

The following requirements **must** be implemented in the project.

- Use of a layered architecture like you learn in the course Object Oriented Programming and Object Oriented Analysis and Secure Design
- Use of clean code and principles like you learn in the course Object Oriented Programming and Object Oriented Analysis and Secure Design
- At least 20 commits per team member
- Database models are not used in the GUI or any other layer above the service layer.
- Authentication and identity (bonus points)
    - Make sure you can register and login
    - Make sure you can only access the application when you are logged in
    - Small admin panel only accessible for admins
    - No help from the teacher
- Record a video in a strict timing format of 5 minutes:
    - One video per team submitted through a Leho assignment
    - Demonstrate from local host
    - Don't show the code or the IDE
    - Timing:
        - 1 minute: Introduction
        - 2 minutes: Demo
        - 1 minute: Enumerate non working features
        - 1 minute: Bonus or extra features



