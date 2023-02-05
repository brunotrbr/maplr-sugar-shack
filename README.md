# maplr-sugar-shack
Maplr Backend Technical Test

[![Contributors][contributors-shield]][contributors-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<p align="center">
  <h3 align="center">Maplr: handle sells and stocks of maple syrup</h3>

  <p align="center">
    Maplr technical test
    <br />
</p>

<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Content</summary>
  <ol>
    <li>
      <a href="#the-problem">The problem</a>
    </li>
    <li>
      <a href="#the-solution">The solution</a>
      <ul>
        <li><a href="#database">Database</a></li>
        <li><a href="#class-modelling">Class modelling</a></li>
        <li><a href="#pre-requisites">Pre-requisites</a></li>
        <li><a href="#preparing-the-environment">Preparing the environment</a></li>
        <li><a href="#running-the-program">Running the program</a></li>
        <li><a href="#tests">Tests</a></li>
      </ul>
    </li>
    <li><a href="#additional-considerations">Additional considerations</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>

&nbsp;

## The problem
Maplr has just opened a brand new sugar shack in the middle of Quebec, you will have to develop the Frontend or the Backend to handle sells and stocks of maple syrup 🍁 In order to help you for this task, an interface contract has been produced.

&nbsp;

## The solution
To solve the problem was created an API to handle sells and stocks.

As demanded the program were created using .NET 6 with Visual Studio IDE. The endpoins could be testd using postman ou swagger.

&nbsp;

### Database

I choose to use Entity Framework In memory as database. To load files at startup application I use the JSON below and was created DataGenerator class.

```json
[
  {
    "id": "f6fb258d-c33f-4de3-9288-253bc86234b0",
    "name": "Amber Maple Syrup",
    "description": "Winding Road 100% Pure Amber Maple Syrup",
    "image": "https://www.windingroadmaple.ca/uploads/userfiles/images/bottles.png",
    "price": 8.00,
    "stock": 10,
    "type": 1
  },
  {
    "id": "880c684d-770b-4274-9891-c00e08d37f2f",
    "name": "Dark Maple Syrup",
    "description": "Winding Road 100% Pure Dark Maple Syrup",
    "image": "https://www.windingroadmaple.ca/uploads/userfiles/images/bottles.png",
    "price": 8.50,
    "stock": 15,
    "type": 2
  },
  {
    "id": "d7e356c8-5aa1-4cc6-b5a8-c76f51d7906e",
    "name": "Clear Maple Syrup",
    "description": "Winding Road 100% Pure Clear Maple Syrup",
    "image": "https://www.windingroadmaple.ca/uploads/userfiles/images/bottles.png",
    "price": 7.50,
    "stock": 18,
    "type": 3
  }
]
```

&nbsp;

### Class modelling 

The class modelling was according the contract available in the test, with one single change.

Products and Cart are using strings as keys (a GUID and productId, respectively) as defined in contract, and I created a sequential ID to orders (the change is to allow multiple orders, simulating an e-commerce with multiple clients), as database models.

There is a problem of use the productId as identifier (best described in **Cart - multiple carts with same productID** at <a href="#additional-considerations">Additional considerations</a> section), but I prefer to not change the contract.

The DTOs were created according the contract.

&nbsp;

### Pre-requisites
    .NET 6.0 e .NET CLI (Dotnet version 6.0.302. Not tested with other versions). URL: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

    Docker (Docker version 20.10.17. Not tested with other versions). URL: https://docs.docker.com/get-docker/
    
    Docker Compose (Docker Compose version v2.6.1. Not tested with other versions). URL: https://docs.docker.com/compose/install/

&nbsp;

### Preparing the environment

*The installation of .NET/.NET CLI, docker and docker-compose are not covered in this readme. Please follow the instructions using the documentation above.*

&nbsp;

Clone the project at repository url https://github.com/brunotrbr/maplr-sugar-shack and go to `maplr-sugar-shack` dir:

```
[Windows]
PS C:\> git clone git@github.com:brunotrbr/maplr-sugar-shack.git
PS C:\> cd maplr-sugar-shack

---------------------------------------------------------------------------

[Linux]
user@machine:~$ git clone git@github.com:brunotrbr/maplr-sugar-shack.git
user@machine:~$ cd maplr-sugar-shack
```

&nbsp;

To prepare the execution environment run the following script at terminal/console of the computer:

```
[Windows]
PS C:\maplr-sugar-shack> .\prepare_env.ps1

---------------------------------------------------------------------------

[Linux]
user@machine:maplr-sugar-shack$ source prepare_env.sh
```

&nbsp;

### Running the program

Use the command `docker-compose up` to build and run the project automatically:

```
[Windows]
PS C:\maplr-sugar-shack> docker-compose up

---------------------------------------------------------------------------

[Linux]
user@machine:maplr-sugar-shack$ docker-compose up
```

&nbsp;

Case the command run successfuly, two containers are started, named `maplr-sugar-shack-maplr-api-1` and `maplr-sugar-shack-maplr-api-tests-1`). At this moment the API is available to access in http://localhost:8080/swagger/index.html

At the end, the summary referring to the tests created to verify that the **order** endpoint and order validations are working correctly will be printed on the console, as shown below::



```
PS C:\maplr-sugar-shack> docker-compose up
[+] Running 3/3
 - Network maplr-sugar-shack_default              Created                                                                                      0.9s
 - Container maplr-sugar-shack-maplr-api-1        Created                                                                                      0.1s
 - Container maplr-sugar-shack-maplr-api-tests-1  Created                                                                                      0.2s
Attaching to maplr-sugar-shack-maplr-api-1, maplr-sugar-shack-maplr-api-tests-1
maplr-sugar-shack-maplr-api-tests-1  | Microsoft (R) Test Execution Command Line Tool Version 17.3.1 (x64)
maplr-sugar-shack-maplr-api-tests-1  | Copyright (c) Microsoft Corporation.  All rights reserved.
maplr-sugar-shack-maplr-api-tests-1  |
maplr-sugar-shack-maplr-api-tests-1  | Starting test execution, please wait...
maplr-sugar-shack-maplr-api-tests-1  | A total of 1 test files matched the specified pattern.
maplr-sugar-shack-maplr-api-1        | info: Microsoft.EntityFrameworkCore.Update[30100]
maplr-sugar-shack-maplr-api-1        |       Saved 3 entities to in-memory store.
[...]
[...]
[...]
maplr-sugar-shack-maplr-api-1        |       Saved 1 entities to in-memory store.
maplr-sugar-shack-maplr-api-1        | info: Microsoft.EntityFrameworkCore.Update[30100]
maplr-sugar-shack-maplr-api-1        |       Saved 1 entities to in-memory store.
maplr-sugar-shack-maplr-api-tests-1  |
maplr-sugar-shack-maplr-api-tests-1  | Passed!  - Failed:     0, Passed:     7, Skipped:     0, Total:     7, Duration: 802 ms - /app/build/maplr-api-tests.dll (net6.0)
maplr-sugar-shack-maplr-api-tests-1 exited with code 0
```

&nbsp;

Alternatively, use `dotnet xxx` commands to run projects independently:

```
[Windows] - Run API
PS C:\maplr-sugar-shack> cd .\maplr-api\
PS C:\maplr-sugar-shack\maplr-api> dotnet restore
PS C:\maplr-sugar-shack\maplr-api> dotnet run

[Windows] - Run tests (the API must be running)
PS C:\maplr-sugar-shack> cd .\maplr-api-tests\
PS C:\maplr-sugar-shack\maplr-api-rests> dotnet restore
PS C:\maplr-sugar-shack\maplr-api-tests> dotnet run

---------------------------------------------------------------------------

[Linux] - Run API
user@machine:maplr-sugar-shack$ cd maplr-api
user@machine:maplr-api$ dotnet restore
user@machine:maplr-api$ dotnet run

[Linux] - Run tests (the API must be running)
user@machine:maplr-sugar-shack$ cd maplr-api-tests
user@machine:maplr-api$ dotnet restore
user@machine:maplr-api$ dotnet test -e "ASPNETCORE_ENVIRONMENT=local"
```

**Obs:**

The application can be manually tested using the endpoints in the contract with postman or acessing swagger after run the application at URL http://localhost/swagger/index.html


&nbsp;

### Tests

I choose to write system tests only to OrderController because it uses others controllers to validate the order, covering more parts of our application.

They were written using NUnit Framework.

To run the tests, you can use the command `docker-compose up` or `dotnet xxx`, as exemplified in <a href="#running-the-program">Running the program</a> section

&nbsp;

## Additional considerations

&nbsp;

#### Authentication & Authorization
To place orders it's obrigatory authenticate at system using username `maplr` and password `maplr`. The other actions (get catalogue, add to cart, change qty, etc) do not require authentication. Users can interact with our store without authentication, but to place orders is necessary.

&nbsp;

#### Interfaces for Controllers 
I could create an interface called `IBaseController` to padronize common methods in all controllers. However, as long as basically there is not common methods (with same signature) at Controllers, it has not been created.

&nbsp;

#### Mappers
The mappers were created because the difference among DTO's and database models. I use them to create/map objects from a type to another type. I.E. `Put` in `CartController` fetch data from `ProductRepository` as `MapleSyrupDto` (in repository is from model `MapleSyrup` to DTO `MapleSyrupDto`) to check if product exists and the amount in stock, then map to `CartLineDto` to send to `CartRepository` and save in Cart Database as `Carts` model.

&nbsp;

#### Cart - multiple carts with same productID
I've noted that method `Put (Add to cart)` in `CartController` send the `productId` as cart identifier. Without changes in contract it's not possible to have more than one cart with the same product (think about postman as customer 1 and swagger as customer 2. They can't have the same product in cart because `productId` is the PK of Carts table).

To handle this situation I could use an unique identifier (GUID, sequential int, etc) in Carts Table. But in contract the methods `Delete (Remove from cart)` and `Patch (change qty)` uses the `ProductId` to remove/update products, and I would had to change the contract to send the ID in route and the `productId` in body.

To not change the contract, I kept `productId` as PK of Carts Table and then is not possible to simulate 2 customers buying at the same time.

&nbsp;

#### Business Layer (BL)
To validate orders business rules (if product is already in cart, if have sufficient amount in stock, etc) I created a file called `OrdersBL`. In case of other validations in other controllers, we could create another files using the same name pattern `<ControllerName>BL`.

&nbsp;

#### Place Order - List of products
The `Post` method on OrderController (`Place Order`) sends a list of `productId's` and ther respective `qty` to place order. It forces me to check if the product is in cart, if the qty in cart is the same as in Place Order and if the stock has the amount ordered during the post operation.

The best thing to avoid these validations at Place Order could be use an unique identifier (GUID, sequential int, etc) to identify Cart's, and the Place Order just send the Cart ID. The products are already validated and we just place the order without any issues.

&nbsp;

#### Exceptions
As you can see in Filters directory, a `CustomExceptionFilter` class has been created to handle unexpected errors in application (unexpected erros, in this context, are any erros not expected in controller's contract or custom validations). If this unexpected erros occurs, API catch them and send back to user an `Internal Server Error` (status code 500) with a friendly message, without show the stack trace. The request method, error message and stack trace are logged in console to further investigations of the cause by developers.

I know it's wrong turn errors 4xx or mask different 5xx into 500, but here is just to show that it's possible catch erros globally, specific for controllers or endpoints.

OBS: To see this working, I create an new endpoint in `ProductsContrller`, `Put`. It should return an generic Exception with message "Just a test", but returns "Ops! Unexpected error. Please try again later." as defined in `CustomExceptionFilter`.

&nbsp;

### Acknowledgements

* [Img Shields](https://shields.io)
* [Table generators for markdown](https://www.tablesgenerator.com/markdown_tables)



[contributors-shield]: https://img.shields.io/github/contributors/brunotrbr/maplr-sugar-shack?style=for-the-badge
[contributors-url]: https://github.com/brunotrbr/maplr-sugar-shack/graphs/contributors
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/brunotrbr/