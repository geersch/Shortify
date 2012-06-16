using System.Windows.Interactivity;
using System.Windows;

namespace Shortify.Behaviors
{
    public class MessageBoxDisplayBehavior : Behavior<FrameworkElement>
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
          DependencyProperty.Register(
            "Message",
            typeof(string),
            typeof(MessageBoxDisplayBehavior),
            new PropertyMetadata(string.Empty, MessageChanged));

        public static void MessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var msg = e.NewValue as string;
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg);
            }
        }
    }
}
