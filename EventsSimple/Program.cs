using EventsSimple;

var buttonMain = new ButtonMain();

buttonMain.ButtonPressed += (sender, eventArgs) =>
{
    Console.WriteLine($"{eventArgs.Key.ToString().ToUpper()} was pressed.");
};

for (int i = 0; i < 10; i++)
{
    var key = Console.ReadKey(true).KeyChar;
    buttonMain.OnButtonPressed(key);
}