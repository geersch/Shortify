using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Shortify
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            var hyperlinkButton = (HyperlinkButton)sender;

            var email = new EmailComposeTask
            {
                To = hyperlinkButton.Content.ToString(), 
                Subject = "Shortify"
            };
            email.Show();
        }

        private void Site_Click(object sender, RoutedEventArgs e)
        {
            var hyperlinkButton = (HyperlinkButton)sender;

            var web = new WebBrowserTask()
            {
                Uri = new Uri(hyperlinkButton.Content.ToString())
            };
            web.Show();
        }
    }
}