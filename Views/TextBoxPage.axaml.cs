using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using GetStartedApp.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GetStartedApp.Views
{
    public partial class TextBoxPage : UserControl
    {
        private bool _isLoggedIn = false;

        public TextBoxPage()
        {
            InitializeComponent();
          
        }

        private void OnLoginButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
           
        }

        private async void OnEditAvatarButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filters.Add(new FileDialogFilter { Name = "Images", Extensions = { "jpg", "png", "jpeg" } });
            var result = await fileDialog.ShowAsync((Window)this.VisualRoot);

            if (result != null && result.Length > 0)
            {
                var bitmap = new Bitmap(result[0]);
                AvatarImage.Source = bitmap;
            }
        }

        private void OnAvatarImageClick(object sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (_isLoggedIn)
            {
                OnEditAvatarButtonClick(sender, e);
            }
            else
            {
                ShowLoginPrompt();
            }
        }

        private void OnViewAllOrdersButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_isLoggedIn)
            {

            }
            else
            {
                ShowLoginPrompt();
            }
        }

        private void OnViewTicketsButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_isLoggedIn)
            {

            }
            else
            {
                ShowLoginPrompt();
            }
        }

        private void OnViewVisitorsButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_isLoggedIn)
            {
         
            }
            else
            {
                ShowLoginPrompt();
            }
        }

        private void ShowLoginPrompt()
        {
            
        }
    }
}