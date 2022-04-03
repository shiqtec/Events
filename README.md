# Events

- [Simple Events](#simple-events)
- [Background Service Events](#background-service-events)
- [MediatR](#mediatR)
- [More on MediatR](#more-on-mediatR)

## Simple Events

This example is to showcase what events actually are.  
  
The `EventsSimple` project uses events, namely events that are invoked from when a button is pressed on the keyboard, and a subscriber then prints out a message on the console containing the key that was pressed.

> In `Program.cs` we subscribe to the *ButtonPressed* event:
>
> ```cs
> buttonMain.ButtonPressed += (sender, eventArgs) =>
> {
>     Console.WriteLine($"{eventArgs.Key.ToString().ToUpper()} was pressed.");
> };
> ```

We call a method, *OnButtonPressed*, in `Program.cs` that then invokes an event *ButtonPressed*:

> ```cs
>public event EventHandler<ButtonPressedEventArgs>? ButtonPressed;
>
> public void OnButtonPressed(char key)
> {
>    ButtonPressed?.Invoke(this, new ButtonPressedEventArgs(key));
> }
> ```

The *ButtonPressed* event is of type *ButtonPressedEventArgs* that stores the value of the key that was pressed:

> ```cs
> public class ButtonPressedEventArgs
> {
>     public ButtonPressedEventArgs(char key)
>     {
>         Key = key;
>     }
>
>     public char Key { get; }
> }
> ```

## Background Service Events

In the `EventsBackgroundService` project we use a background service in a WebAPI to trigger an event for the current time.  

For simplicity, we subscribe to these events in the *TickerService* as well which then prints out the current time in the console.  

We have multiple subscribers:
- OnEveryOneSecond
- OnEveryFiveSecond

### A Problem

If we wanted to print a different GUID every second instead of the time, we would create a new class `TransientService`:

> ```cs
> public class TransientService
> {
>     public Guid guid { get; } = Guid.NewGuid();
> }
> ```

We then can register this as a transient service in `Program`:

> ```cs
> builder.Services.AddTransient<TransientService>();
> ```

Finally, we can inject this new service into the `TickerService` and replace the Console message with the data from the new service:

> ```cs
> public event EventHandler<TickerEventArgs>? Ticked;
> private readonly TransientService _transientService;
> 
> public TickerService(TransientService transientService)
> {
>     _transientService = transientService;
>     Ticked += OnEveryOneSecond;
>     Ticked += OnEveryFiveSecond;
> }
> 
> public void OnEveryOneSecond(object? sender, TickerEventAr>args)
> {
>     Console.WriteLine(_transientService.guid);
> }
> ```

We expect that a new GUID will be generated every time, as it is a transient service which means a new instance should be provided to the service and hence a new GUID.  

This is not what happens:

> ![Screenshot](https://github.com/shiqtec/Events/blob/main/Images/screenshot01.png)

We have a transient service *TransientService* within a singleton *TickerService* which means that the transient service would be resolved every time.  

Even if we change the *TickerService* to a transient service, we would get the same behaviour.

### The Solution

We could pass the *TransientService* all the way down to *TickerEventArgs*, but then we would get dependency nightmare.  

We can use `MediatR` to resolve this and would make things a lot more manageable.

## MediatR

There are two types MediatR messages:
- Request/response messages that is dispatched to a *single handler*.
- Notification messages that is dispatched to *multiple handlers*.

Here we will use the notification-based MediatR.

In our `Program` file we will add `MediatR` as a service:

> ```cs
> builder.Services.AddMediatR(typeof(Program));
> ```

Then, in our `TickerBackgroundService` we will inject `MediatR`, using dependency injection, and publish a `TimedNotification` every second asynchronously. The `TimedNotification` is a model that holds the current time.

> ```cs
> private readonly IMediator _mediator;
> 
> public TickerBackgroundService(IMediator mediator)
> {
>     _mediator = mediator;
> }
> 
> protected override async Task ExecuteAsync(CancellationTok> cancellationToken)
> {
>     while (!cancellationToken.IsCancellationRequested)
>     {
>         var timeNow = TimeOnly.FromDateTime(DateTime.Now);
>         await _mediator.Publish(new TimedNotification(timeNow), cancellationToken);
>         await Task.Delay(1000, cancellationToken);
>     }
> }
> ```

We now add our two handlers, that will print out the current time every second and every five seconds:

> ```cs
> public class EverySecondHandler : > INotificationHandler<TimedNotification>
> {
>     public Task Handle(TimedNotification notification, > CancellationToken cancellationToken)
>     {
>         Console.WriteLine(notification.Time.ToLongTimeString());
>         return Task.CompletedTask;
>     }
> }
> ```

> ```cs
> public class EveryFiveSecondsHandler : INotificationHandler<TimedNotification>
> {
>     public Task Handle(TimedNotification notification, CancellationToken cancellationToken)
>     {
>         if(notification.Time.Second % 5 == 0)
>         {
>             Console.WriteLine(notification.Time.ToLongTimeString());
>         }
>         return Task.CompletedTask;
>     }
> }
> ```

At this point, this behaves the same way as our `EventsBackgroundService` project:

> ![Screenshot](https://github.com/shiqtec/Events/blob/main/Images/screenshot02.png)

This behaves the same way, but we are now easily able to inject our `TransientGUIDService`, which will be instantiated every time i.e. a new GUID will be generated each time, which we were unable to easily do in our `BackgroundService` project: 

In our `EverySecondHandler` we will inject the `TransientGUIDService` and print out the GUID:

> ```cs
> private readonly TransientGUIDService _guidService;
>
> public EverySecondHandler(TransientGUIDService guidService)
> {
>     _guidService = guidService;
> }
> public Task Handle(TimedNotification notification,CancellationToken > cancellationToken)
> {
>     Console.WriteLine(_guidService.guid);
>     return Task.CompletedTask;
> }
> ```

We now get our desired behaviour, where a new GUID is generated every second, with the current time being printed out every five seconds:

> ![Screenshot](https://github.com/shiqtec/Events/blob/main/Images/screenshot03.png)

## More on MediatR

`MediatR` facilitates CQRS and Mediator patterns in .NET.

### Mediator Pattern

The mediator pattern is a behavioural design pattern that helps to reduce chaotic dependencies between objects. The communication between objects only happens between a mediator object and no direct communication between objects are allowed.

This creates a loose coupling between classes and as a result becomes more maintainable.

### CQRS

CQRS stands for Command and Query Responsibility Segregation, which is a pattern that separates read and update operations for a data store.

Implementing CQRS ca help maximise the applications performance, scalability and security and allows systems to better evolve over time.

Some of the advantages of this includes:
- Each can be scaled accordingly.
- Each can have their own security as per the requirements.
- Read operation can have a different architecture, for example to support caching or data transformations.
- Write operations can include data validations, lookups etc. 
- Can be worked on by separate teams.

More information can be found below.

## References

Chapsas, N. [Nick Chapsas]. (2022, March 17). *Are events in C# even relevant anymore?* [Video]. YouTube. https://www.youtube.com/watch?v=NmmpXcMxCjY&ab_channel=NickChapsas

Patel, A. (2021, August 3). *Use MediatR in ASP.NET or ASP.NET Core.* Medium. https://medium.com/dotnet-hub/use-mediatr-in-asp-net-or-asp-net-core-cqrs-and-mediator-in-dotnet-how-to-use-mediatr-cqrs-aspnetcore-5076e2f2880c

Butt, M. (2020, November 16). *Using The CQRS Pattern In C#.* C# Corner. https://www.c-sharpcorner.com/article/using-the-cqrs-pattern-in-c-sharp/