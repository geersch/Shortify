using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Shortify.ViewModel;

namespace Shortify
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            Url.Focus();
        }

        protected MainViewModel ViewModel
        {
            get
            {
                return LayoutRoot.DataContext as MainViewModel;
            }
        }

        private void Url_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var focusObj = FocusManager.GetFocusedElement();
                if (focusObj != null && focusObj is TextBox)
                {
                    var binding = ((TextBox) focusObj).GetBindingExpression(TextBox.TextProperty);
                    if (binding != null)
                    {
                        binding.UpdateSource();
                    }
                }

                ViewModel.ShortenCommand.Execute(null);
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}