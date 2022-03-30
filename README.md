# Events

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

> We call a method, *OnButtonPressed*, in `Program.cs` that then invokes an event *ButtonPressed*:
> ```cs
>public event EventHandler<ButtonPressedEventArgs>? ButtonPressed;
>
> public void OnButtonPressed(char key)
> {
>    ButtonPressed?.Invoke(this, new ButtonPressedEventArgs(key));
> }
> ```

> The *ButtonPressed* event is of type *ButtonPressedEventArgs* that stores the value of the key that was pressed:
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

## References

Chapsas, N. [Nick Chapsas]. (2022, March 17). *Are events in C# even relevant anymore?* [Video]. YouTube. https://www.youtube.com/watch?v=NmmpXcMxCjY&ab_channel=NickChapsas