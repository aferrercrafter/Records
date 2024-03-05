# Records API

## User Story
As a user, I want to use the BallastLane API to create, view, update, and delete records for my account so that I can manage my personal information. I expect that:

* Register for an account with a unique username and email address, and a password that meets certain criteria, so that I can access protected features of the application.
* Log in to my account with my registered email address and password, so that I can access protected features of the application
* When I create a new record, I must provide a title and a description. The title must be between 1 and 100 characters long, and the description must be between 1 and 500 characters long.
* When I view a record, I must provide the record ID, and the API should return the record only if it belongs to my account.
When I view all records for my account, the API should return a list of all records that belong to my account.
* When I update a record, I must provide the record ID and the new title and description. The title must be between 1 and 100 characters long, and the description must be between 1 and 500 characters long. The API should update the record only if it belongs to my account.
* When I delete a record, I must provide the record ID, and the API should delete the record only if it belongs to my account.
* Additionally, when I create a new user account, I must provide a unique  and valid email address. If any of these requirements are not met, the API should return a 400 Bad Request status code with a message indicating the error.

## Methodologies Used
* The BallastLane  API was developed using a test-driven development (TDD) approach, which ensured that all code was thoroughly tested before being implemented. 
* The project also applies Clean Architecture by dividing the codebase into layers that are independent of each other. The layers are:

    * Domain: The core business logic of the application, where the entities, and business rules reside.
    * Application: The layer that coordinates the use cases of the system.
    * Infrastructure: The implementation details of the system, such as database.
* And lastly SOLID proinciples where also applied in several ways:
    * Single Responsibility Principle (SRP): Each class has a single responsibility and is only responsible for one thing.
    * Open-Closed Principle (OCP): The code is open for extension but closed for modification. This means that we can add new functionality without changing existing code.
    * Liskov Substitution Principle (LSP): Objects of a superclass should be able to be replaced with objects of a subclass without causing errors in the program.
    * Interface Segregation Principle (ISP): Clients should not be forced to depend on interfaces that they do not use. Interfaces are defined for specific needs.
    * Dependency Inversion Principle (DIP): High-level modules should not depend on low-level modules. Both should depend on abstractions.

## Getting Started
To run the BallastLane  API, follow these steps:

Clone the repository to your local machine.
   
    git clone https://github.com/aferrercrafter/ballastlane.git
   
Navigate to the src folder

    cd src
   
Ensure that you have the latest version of Docker installed and Run the docker compose command up  

    docker-compose up --build -d

Navigate to http://localhost:3000 in your web browser to access a simple react application taht consumes the API.
Or you could enter either
    
    https://localhost:7007/swagger/index.html
    http://localhost:7006/swagger/index.html

For getting access to the swagger definition of the API, and directly consume the endpoints. Take in consideration that Records endpoint need authentication, so first you wil need to consume endpoints:

    POST Users/register

For creating an user. And later for getting a token:

    POST Authentication/login

That the response looks like this (Also visible on the swagger definition):

    {
        "id": 0,
        "email": "string",
        "token": "string"
    }

The token field is the one needed on the Authorize option of swagger, and from there all records endpoints options can be consumed 


### API Endpoints 

The BallastLane  API provides the following endpoints:

    
    GET /records
    Retrieves a list of all records.

    GET /records/{id}
    Retrieves a specific record by ID.

    POST /records
    Creates a new record.

    PUT /records/{id}
    Updates an existing record by ID.

    DELETE /records/{id}
    Deletes an existing record by ID.

