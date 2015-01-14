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

namespace AppPathSample
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

            CheckPaths();
            

        }

        private async void CheckPaths()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Installation folder
            Windows.ApplicationModel.Package package = Windows.ApplicationModel.Package.Current;
            Windows.Storage.StorageFolder installedLocation = package.InstalledLocation;

            sb.Append(String.Format("Installed Location: {0}", installedLocation.Path));
            sb.Append(Environment.NewLine);

            var folders = await installedLocation.GetFoldersAsync(Windows.Storage.Search.CommonFolderQuery.DefaultQuery);

            foreach (var folder in folders)
            {
                sb.Append("Folder = " + folder.Name + Environment.NewLine);

                var files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);

                foreach (var file in files)
                {
                    sb.Append("File = " + file.Name + Environment.NewLine);

                    await file.CopyAsync(localFolder,file.Name,Windows.Storage.NameCollisionOption.ReplaceExisting);
                }
            }

            this.txtInstallationFolderContent.Text = sb.ToString();

        

            //// Local folder
            

            sb.Append(String.Format("Local Folder Location: {0}", localFolder.Path));
            sb.Append(Environment.NewLine);


            var rootFiles = await localFolder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);

                foreach (var file in rootFiles)
                {
                    sb.Append("File = " + file.Name + Environment.NewLine);
                }

                var localFolders = await localFolder.GetFoldersAsync(Windows.Storage.Search.CommonFolderQuery.DefaultQuery);

            foreach (var folder in localFolders)
            {
                sb.Append("Folder = " + folder.Name + Environment.NewLine);

                var files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);

                foreach (var file in files)
                {
                    sb.Append("File = " + file.Name + Environment.NewLine);
                }
            }

            

            this.txtLocalFolderContent.Text = sb.ToString();

        }
    }
}
