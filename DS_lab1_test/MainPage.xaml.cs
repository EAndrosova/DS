using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DS_lab1_test
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private WebClient client;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            client = new WebClient();

            // client id and secret key form SoundCloud
            string ClientId = "ef84c4f73eba7d7c344687eee8eed747";
            string ClientSecret = "97e17d2011a34fa74e399f940ddb8a42";

            // credentials (username & password)
            string username = tbLogin.Text;
            string password = pbPwd.Password;

            // authentication data
            string postData = "client_id=" + ClientId
                + "&client_secret=" + ClientSecret
                + "&grant_type=password&username=" + username
                + "&password=" + password;

            // authentication
            string soundCloudTokenRes = "https://api.soundcloud.com/oauth2/token";
            var scUri = new Uri(soundCloudTokenRes);
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.UploadStringAsync(scUri, postData);

        }

        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error == null)
            {
                lblError.Content = "Succeeded!";
                string tokenInfo = e.Result.ToString();
                MessageBox.Show(tokenInfo);
                // parse access token
                tokenInfo = tokenInfo.Remove(0, tokenInfo.IndexOf("token\":\"") + 8);
                string token = tokenInfo.Remove(tokenInfo.IndexOf("\""));

                // SoundCloud API get request
                string soundCloudMeReq = "https://api.soundcloud.com/me";
                //string soundCloudMeReq = "https://api.soundcloud.com/me/connections.json";
                var scUri = new Uri(soundCloudMeReq + "?oauth_token=" + token);
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(scUri);
            }
            else
            {
                //lblError.Content = e.Error.ToString();
                lblError.Content = "Wrong login or password!";
            }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error == null)
            {
                string meData = e.Result;
                MessageBox.Show(meData);
            }
            else
            {
                //lblError.Content = e.Error.ToString();
                lblError.Content = "Some problem with connecting to server!";
            }
        }
    }
}
