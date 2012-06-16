using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows;

namespace Shortify.Behaviors
{
    public class FocusBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += (sender, args) => IsFocused = true;
            AssociatedObject.LostFocus += (sender, a) => IsFocused = false;
            AssociatedObject.Loaded += (o, a) => { if (HasInitialFocus || IsFocused) AssociatedObject.Focus(); };

            base.OnAttached();
        }


        public bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        public static readonly DependencyProperty IsFocusedProperty =
          DependencyProperty.Register(
              "IsFocused",
              typeof(bool),
              typeof(FocusBehavior),
              new PropertyMetadata(false, (d, e) =>
              {
                  if ((bool)e.NewValue)
                  {
                      var control = ((FocusBehavior)d).AssociatedObject as TextBox;
                      if (control == null)
                          return;

                      control.Focus();
                      control.SelectAll();
                  }
              }));

        public static readonly DependencyProperty HasInitialFocusProperty =
            DependencyProperty.Register(
            "HasInitialFocus",
            typeof(bool),
            typeof(FocusBehavior),
            new PropertyMetadata(false, null));

        public bool HasInitialFocus
        {
            get { return (bool)GetValue(HasInitialFocusProperty); }
            set { SetValue(HasInitialFocusProperty, value); }
        }
    }
}
