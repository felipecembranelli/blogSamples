using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace JSONSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            this.GetData();
        }

        public async void GetData()
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();

            try
            {
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                Uri resourceUri;
                string jsonText;

                resourceUri = new Uri(" http://fipeapi.appspot.com/api/1/carros/marcas.json");

                Windows.Storage.Streams.IInputStream response = await httpClient.GetInputStreamAsync(resourceUri);

                using (StreamReader reader = new StreamReader(response.AsStreamForRead()))
                {
                    jsonText = await reader.ReadToEndAsync();
                }

                Windows.Data.Json.JsonValue jsonObject = Windows.Data.Json.JsonValue.Parse(jsonText);

                Windows.Data.Json.JsonArray jsonArray = jsonObject.GetArray();
              
                foreach (Windows.Data.Json.JsonValue item in jsonArray)
                {
                    Windows.Data.Json.JsonObject returnedJSON = item.GetObject();

                    output.AppendFormat("Id: {0} \n", returnedJSON["id"].GetNumber().ToString());
                    output.AppendFormat("Marca {0} \n", returnedJSON["fipe_name"].GetString());
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
            this.txtOutput.Text = output.ToString();
        }
    }
}
