using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SDKTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();

                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);

                Contact contact = await contactPicker.PickContactAsync();

                if (contact != null)
                {
                    // Recupera o nome do contato
                    this.txtName.Text = contact.DisplayName;

                    System.Text.StringBuilder output = new System.Text.StringBuilder();

                    // Recupera os emails
                    foreach (ContactEmail email in contact.Emails as IList<ContactEmail>)
                    {
                        output.AppendFormat("Endereço de Email: {0} ({1})\n", email.Address, email.Kind);
                    }

                    this.txtEmail.Text = output.ToString();
                }

            }
            catch (Exception)
            {
                
                throw;
            }
            
            
        }
    }
}
