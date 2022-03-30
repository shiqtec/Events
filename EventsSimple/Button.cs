namespace EventsSimple
{
    public class Button
    {
        public event EventHandler ButtonPressed;

        public void OnButtonPressed(char key)
        {
            ButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
