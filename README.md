# Tree Of A Kind 
Tree Of A Kind is a multi-platform system supporting the creation of a family tree.

###### CQRS, DDD, Clean architecture, EF, Azure, Pub-Sub

## Video presentation
<p align="center">
   <a href="https://youtu.be/yTk-M3A-ttQ"><img src="https://user-images.githubusercontent.com/32818862/107768831-5b75ae80-6d37-11eb-9f87-452d1185ba2e.png"></a>
</p>

## Physical architecture
<p align="center">
   <img src="https://user-images.githubusercontent.com/32818862/107770128-413cd000-6d39-11eb-874d-53b4d3d5e4a0.png">
</p>

## Backend architecture

Modules of application are separated. It allows to maintain low coupling, and as a result, allows easy testing and swapping (eg. swapping database or ORM), because the business layer is fully independent of implementation. 
![image](https://user-images.githubusercontent.com/32818862/115142108-373c9380-a040-11eb-8166-e7b8bc7f4ede.png)

We are using the MediatR library to separate API from the Application layer. 
Requests are segregated into two types: commands and queries, this allows for separation of the read and write model.
Both types are handled by corresponding handlers.

In the application layer pipeline, commands and queries are validated and checked for authorization. Validators and authorizers are injected by DI container which finds them during application startup by assembly scanning.
```csharp
using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddTreeOwner
{
    public class AddTreeOwnerCommandValidator : AbstractValidator<AddTreeOwnerCommand>
    {
        public AddTreeOwnerCommandValidator()
        {
            RuleFor(x => x.AddedPersonMailAddress)
                .EmailAddress()
                .WithMessage("Provided string is not proper e-mail address");
        }
    }
}
```

After business layer operation. Events published by domain objects are aggregated and send by MediatR.
This allows for adding Domain Event Handlers on a basis of PUB-SUB, which further separates business logic from implementation.
We have used this functionality to add sending welcoming messages to newly registered users.



## Logging

Events from the application are logged to DataDog:

![image](https://user-images.githubusercontent.com/32818862/115141971-83d39f00-a03f-11eb-87f5-b223211020d3.png)

Traces of the machines are also aggregated in DataDog service, which provides a clear view on servers response times and allows further analysis:

![image](https://user-images.githubusercontent.com/32818862/115141987-95b54200-a03f-11eb-8daa-7bd1dd8abd5f.png)

## Quote
> My full name is Esteban Julio Ricardo Montoya de la Rosa Ramírez.
>
> ~ Esteban Julio Ricardo Montoya de la Rosa Ramírez, The Suite Life of Zack and Cody
