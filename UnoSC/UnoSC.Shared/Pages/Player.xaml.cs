using System;
using System.Collections.Generic;
using UnoSC.Shared.Helpers;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UnoSC.Shared.Pages
{
    public sealed partial class Player : Page
    {
        public Player()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;

            if(roamingSettings.Values["userCode"] == null)
            {
                logoutButton.Content = "Login";
            }

            codeTB.Text = "UserCode: " + roamingSettings.Values["userCode"] as string;
            tokenTB.Text = "AccessToken: " + roamingSettings.Values["accessToken"] as string;

            Dictionary<string, object> res = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(WebHelper.MakeRequest("https://api.soundcloud.com/tracks/887334250?client_id=1d57801f1cc71e123c1b9c58aa45c8e7"));
            debugRes.Text = res["title"] as string;

            string author_res = WebHelper.MakeRequest(res["user_uri"] as string + "?client_id=1d57801f1cc71e123c1b9c58aa45c8e7");
            Dictionary<string, object> author = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(author_res);
            debugRes2.Text = author["username"] as string;

            trackPlayer.Source = MediaSource.CreateFromUri(new Uri("https://api.soundcloud.com/tracks/887334250/stream?client_id=1d57801f1cc71e123c1b9c58aa45c8e7"));
            trackPlayer.MediaPlayer.BufferingStarted += (Windows.Media.Playback.MediaPlayer sender, object args) => { trackPlayer.MediaPlayer.Pause(); };
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["userCode"] = null;
            roamingSettings.Values["accessToken"] = null;
            Frame.Navigate(typeof(MainPage), null);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            trackPlayer.MediaPlayer.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            trackPlayer.MediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            trackPlayer.MediaPlayer.Pause();
        }
    }
}
