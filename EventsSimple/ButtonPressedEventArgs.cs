namespace EventsSimple
{
    public class ButtonPressedEventArgs
    {
        public ButtonPressedEventArgs(char key)
        {
            Key = key;
        }

        public char Key { get; }
    }
}
