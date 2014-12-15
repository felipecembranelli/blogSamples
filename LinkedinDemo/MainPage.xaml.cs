using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace LinkedinDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string _consumerKey = "772jmojzy2vnra";
        string _consumerSecretKey = "hQu5DFEqP5JQyPGr";
        string _linkedInRequestTokenUrl = "https://api.linkedin.com/uas/oauth/requestToken";
        string _linkedInAccessTokenUrl = "https://api.linkedin.com/uas/oauth/accessToken";

        string _requestPeopleUrl = "http://api.linkedin.com/v1/people/~";
        string _requestConnectionsUrl = "http://api.linkedin.com/v1/people/~/connections";
        string _requestPositionsUrl = "http://api.linkedin.com/v1/people/~:(positions)";

        string _requestJobsUrl = "http://api.linkedin.com/v1/people/~/suggestions/job-suggestions";
        string _requestJobsByKeyWordsUrl = "https://api.linkedin.com/v1/job-search?keywords=quality";

        string callback = "https://www.linkedin.com/sucess.htm";

        OAuthUtil oAuthUtil = new OAuthUtil();

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            consumerKey.Text = _consumerKey;
            consumerSecretKey.Text = _consumerSecretKey;

            LinkedinAuthentication();

             
        }

        private async  void LinkedinAuthentication()
        {
            // Step 1 : Get request token
            await GetRequestToken();

        
            //
        }

        public string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        private async void getRequestToken_Click_1(object sender, RoutedEventArgs e)
        {
            await GetRequestToken();
        }

        private async System.Threading.Tasks.Task GetRequestToken()
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            string sigBaseStringParams = "oauth_callback=" + this.UrlEncode(callback);
            sigBaseStringParams += "&" + "oauth_consumer_key=" + consumerKey.Text;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;

            sigBaseStringParams += "&" + "oauth_version=1.0";

            string sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(_linkedInRequestTokenUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            string signature = oAuthUtil.GetSignature(sigBaseString, consumerSecretKey.Text);

            var responseText = await oAuthUtil.PostData(_linkedInRequestTokenUrl, sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature));

            if (!string.IsNullOrEmpty(responseText))
            {
                string oauth_token = null;
                string oauth_token_secret = null;
                string oauth_authorize_url = null;
                string[] keyValPairs = responseText.Split('&');

                for (int i = 0; i < keyValPairs.Length; i++)
                {
                    String[] splits = keyValPairs[i].Split('=');
                    switch (splits[0])
                    {
                        case "oauth_token":
                            oauth_token = splits[1];
                            break;
                        case "oauth_token_secret":
                            oauth_token_secret = splits[1];
                            break;
                        case "xoauth_request_auth_url":
                            oauth_authorize_url = splits[1];
                            break;
                    }
                }

                requestToken.Text = oauth_token;
                requestTokenSecretKey.Text = oauth_token_secret;
                oAuthAuthorizeLink.Content = Uri.UnescapeDataString(oauth_authorize_url + "?oauth_token=" + oauth_token);

                //// Step 2 : Call linkedin web page for authentication
                WebViewHost.Navigate(new Uri(oAuthAuthorizeLink.Content.ToString()));

            }
        }

        private async void getAccessToken_Click_1(object sender, RoutedEventArgs e)
        {
            await GetAccessToken();
        }

        private async System.Threading.Tasks.Task GetAccessToken()
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();



            string sigBaseStringParams = "oauth_consumer_key=" + consumerKey.Text;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
            sigBaseStringParams += "&" + "oauth_token=" + requestToken.Text;
            sigBaseStringParams += "&" + "oauth_verifier=" + oAuthVerifier.Text;
            sigBaseStringParams += "&" + "oauth_version=1.0";

            string sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(_linkedInAccessTokenUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            // LinkedIn requires both consumer secret and request token secret
            string signature = oAuthUtil.GetSignature(sigBaseString, consumerSecretKey.Text, requestTokenSecretKey.Text);

            var responseText = await oAuthUtil.PostData(_linkedInAccessTokenUrl, sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature));

            if (!string.IsNullOrEmpty(responseText))
            {
                string oauth_token = null;
                string oauth_token_secret = null;
                string[] keyValPairs = responseText.Split('&');

                for (int i = 0; i < keyValPairs.Length; i++)
                {
                    String[] splits = keyValPairs[i].Split('=');
                    switch (splits[0])
                    {
                        case "oauth_token":
                            oauth_token = splits[1];
                            break;
                        case "oauth_token_secret":
                            oauth_token_secret = splits[1];
                            break;
                    }
                }

                accessToken.Text = oauth_token;
                accessTokenSecretKey.Text = oauth_token_secret;
            }
        }

        private void oAuthAuthorizeLink_Click_1(object sender, RoutedEventArgs e)
        {
            WebViewHost.Visibility = Windows.UI.Xaml.Visibility.Visible;
            WebViewHost.Navigate(new Uri(oAuthAuthorizeLink.Content.ToString()));
        }

        private async void requestUserProfile_Click_1(object sender, RoutedEventArgs e)
        {
            requestLinkedInApi(_requestPeopleUrl);
        }

        private void requestConnections_Click_1(object sender, RoutedEventArgs e)
        {
            requestLinkedInApi(_requestConnectionsUrl);
        }

        private void requestPositions_Click_1(object sender, RoutedEventArgs e)
        {
            requestLinkedInApi(_requestPositionsUrl);
        }

        private async void requestLinkedInApi(string url)
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = int.MaxValue;
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                HttpRequestMessage requestMsg = new HttpRequestMessage();
                requestMsg.Method = new HttpMethod("GET");
                requestMsg.RequestUri = new Uri(url, UriKind.Absolute);

                string sigBaseStringParams = "oauth_consumer_key=" + consumerKey.Text;
                sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
                sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
                sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
                sigBaseStringParams += "&" + "oauth_token=" + accessToken.Text;
                sigBaseStringParams += "&" + "oauth_verifier=" + oAuthVerifier.Text;
                sigBaseStringParams += "&" + "oauth_version=1.0";
                string sigBaseString = "GET&";
                sigBaseString += Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(sigBaseStringParams);

                // LinkedIn requires both consumer secret and request token secret
                string signature = oAuthUtil.GetSignature(sigBaseString, consumerSecretKey.Text, accessTokenSecretKey.Text);

                string data = "realm=\"http://api.linkedin.com/\", oauth_consumer_key=\"" + consumerKey.Text
                              +
                              "\", oauth_token=\"" + accessToken.Text +
                              "\", oauth_verifier=\"" + oAuthVerifier.Text +
                              "\", oauth_nonce=\"" + nonce +
                              "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + timeStamp +
                              "\", oauth_version=\"1.0\", oauth_signature=\"" + Uri.EscapeDataString(signature) + "\"";
                requestMsg.Headers.Authorization = new AuthenticationHeaderValue("OAuth", data);
                var response = await httpClient.SendAsync(requestMsg);
                var text = await response.Content.ReadAsStringAsync();
                WebViewHost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                LinkedInResponse.Visibility = Windows.UI.Xaml.Visibility.Visible;
                LinkedInResponse.Text = text;
            }
            catch (Exception Err)
            {
                throw;
            }
        }

        private void requestJobs_Click_1(object sender, RoutedEventArgs e)
        {
            requestLinkedInApi(_requestJobsUrl);
        }

        private void requestJobsByKeyWords_Click(object sender, RoutedEventArgs e)
        {
            requestLinkedInApi(_requestJobsByKeyWordsUrl);
        }

        private void WebViewHost_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string x = args.Uri.ToString();



        }

        private void WebViewHost_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            string x = args.Uri.ToString();

            if (args.Uri.ToString().Contains("sucess.htm"))
            {
                args.Cancel = true;

                string queryParams = args.Uri.Query;
                if (queryParams.Length > 0)
                {
                    try
                    {
                        //Store the Token and Token Secret
                        QueryString qs = new QueryString(queryParams);

                        //if (qs["oauth_token"] != null)
                        //{
                        //    string _token = qs["oauth_token"];
                        //}
                        if (qs["oauth_verifier"] != null)
                        {
                            this.oAuthVerifier.Text = qs["oauth_verifier"];
                        }

                        GetAccessToken();

                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                }

                
            }
        }

    }
}
