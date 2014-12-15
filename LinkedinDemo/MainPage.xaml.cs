using LinkedinDemo.Common;
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
        public static MainPage Current;
        
        private string state;
      

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
            SuspensionManager.RegisterFrame(ScenarioFrame, "scenarioFrame");
            if (ScenarioFrame.Content == null)
            {

                if (state != null)
                {
                    // When the navigation stack isn't restored navigate to the ScenarioList
                    if (!ScenarioFrame.Navigate(typeof(Page1), state))
                    {
                        throw new Exception("Failed to create scenario list");
                    }
                }
                else
                {
                    // When the navigation stack isn't restored navigate to the ScenarioList
                    if (!ScenarioFrame.Navigate(typeof(Page1)))
                    {
                        throw new Exception("Failed to create scenario list");
                    }
                }
            }

             
        }

      

        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            if (StatusBlock != null)
            {
                switch (type)
                {
                    case NotifyType.StatusMessage:
                        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                        break;
                    case NotifyType.ErrorMessage:
                        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                        break;
                }
                StatusBlock.Text = strMessage;

                // Collapse the StatusBlock if it has no text to conserve real estate.
                if (StatusBlock.Text != String.Empty)
                {
                    StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }
    }

    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
