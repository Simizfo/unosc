using System;
using System.Linq;
using UnoSC.Shared.Pages;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UnoSC
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            LoginWebView.NavigationStarting += LoginWebView_NavigationStarting;
            LoginWebView.NavigationCompleted += LoginWebView_NavigationCompleted;
            LoginWebView.NavigationFailed += LoginWebView_NavigationFailed;
        }

        private void LoginWebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            LoginUrl.Text = e.Uri.ToString();
        }

        private void LoginWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            LoginUrl.Text = args.Uri.ToString();
            CheckForCodeAndToken();
        }

        private void LoginWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            LoginUrl.Text = args.Uri.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoginWebView.Navigate(new Uri("https://api.soundcloud.com/connect?client_id=1d57801f1cc71e123c1b9c58aa45c8e7&redirect_uri=&response_type=code_and_token"));
        }

        private void CheckForCodeAndToken()
        {
            if(LoginUrl.Text.Contains("?code") && LoginUrl.Text.Contains("#access_token"))
            {
                string userCode = LoginUrl.Text.Split("?code=")[1].Split("#access_token=")[0];
                string accessToken = LoginUrl.Text.Split("?code=")[1].Split("#access_token=")[1].Split("&scope=")[0];

                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["userCode"] = userCode;
                roamingSettings.Values["accessToken"] = accessToken;

                Frame.Navigate(typeof(Player), null);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Player), null);
        }
    }
}
