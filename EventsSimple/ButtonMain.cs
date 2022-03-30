namespace EventsSimple
{
    public class ButtonMain
    {
        public event EventHandler<ButtonPressedEventArgs>? ButtonPressed;

        public void OnButtonPressed(char key)
        {
            ButtonPressed?.Invoke(this, new ButtonPressedEventArgs(key));
        }
    }
}
