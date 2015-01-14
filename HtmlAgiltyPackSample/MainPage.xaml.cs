using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Web.Http;
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

namespace HtmlAgiltyPackSample
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

            this.GetPageHtml();
        }



        private async void GetPageHtml()
        {

            Windows.Web.Http.HttpClient httpClient = new HttpClient();
            
            Uri resourceUri;

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();


            resourceUri = new Uri("http://www.uol.com.br");

            Windows.Storage.Streams.IInputStream response = await httpClient.GetInputStreamAsync(resourceUri);

            document.Load(response.AsStreamForRead());

            // Exemplo 1
            this.txtOutput.Text = document.GetElementbyId("ElementoXPTO").InnerText;


            // Exemplo 2
            var ret = (from c in document.GetElementbyId("reports").ChildNodes
                       where c.Name == "table"
                       select c).Single();


            foreach (var item in ret.ChildNodes)
            {
                // faz loop nas linhas da tabela HTML
            }
        }
    }
}
